using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Native;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Timer = System.Timers.Timer;

namespace WoWmapper.Overlay
{
    /// <summary>
    /// Interaction logic for OverlayWindow.xaml
    /// </summary>
    public partial class OverlayWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }

            public int X
            {
                get { return Left; }
                set
                {
                    Right -= (Left - value);
                    Left = value;
                }
            }

            public int Y
            {
                get { return Top; }
                set
                {
                    Bottom -= (Top - value);
                    Top = value;
                }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT) obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle) obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle) this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture,
                    "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }

        private Thread _controller;

        public OverlayWindow()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            _controller = new Thread(ControllerThread);
            _controller.Start();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowExTransparent(hwnd);
        }

        public void CloseOverlay()
        {
            _controller.Abort();
            Close();
        }

        public void SetCrosshairState(bool visible, int x = 0, int y = 0)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    var rect = ProcessManager.GetClientRectangle();
                    Canvas.SetLeft(ImageCrosshair, x - ImageCrosshair.Width/2 - rect.Left);
                    Canvas.SetTop(ImageCrosshair, y - ImageCrosshair.Height/2 - rect.Top);
                    ImageCrosshair.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
                });
            }
            catch
            {
            }
        }

        public void PopupNotification(OverlayNotification notification)
        {
            if (Settings.Default.EnableOverlay)
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        if (notification.UniqueID != null &&
                            StackNotifications.GetChildObjects()
                                .Any(
                                    toast => (toast as OverlayToast)?.BaseNotification.UniqueID == notification.UniqueID))
                            return;
                        var popupNotification = new OverlayToast(notification);
                        popupNotification.NotificationCompleted += (sender, args) =>
                        {
                            StackNotifications.Children.Remove(popupNotification);
                            popupNotification = null;
                        };
                        StackNotifications.Children.Add(popupNotification);
                    });
                }
                catch (ThreadAbortException)
                {
                    
                }
                catch (Exception ex)
                {
                    Log.WriteLine($"Exception occured during popup creation: {ex.Message}");
                }
        }

        private Rectangle _lastRect;

        private void ControllerThread()
        {
            while (_controller.ThreadState == ThreadState.Running)
                try
                {
                    // If the game has focus, attempt to position overlay
                    if (ProcessManager.GameProcess != null && ProcessManager.GameProcess.MainWindowHandle != IntPtr.Zero)
                    {
                        var hWndFg = GetForegroundWindow();
                        var rect = ProcessManager.GetClientRectangle();
                        var topMost = Dispatcher.Invoke(() => Topmost);

                        // If game window has moved or resized
                        if (rect != _lastRect)
                        {
                            // Update overlay position
                            Dispatcher.Invoke(() =>
                            {
                                Left = rect.Left;
                                Top = rect.Top;
                                Width = rect.Width;
                                Height = rect.Height;
                            });
                            _lastRect = rect;
                        }

                        // If game is foreground window and overlay is hidden, show it
                        if (hWndFg == ProcessManager.GameProcess.MainWindowHandle && !topMost)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Topmost = true;
                                Visibility = Visibility.Visible;
                            });
                        }
                        // Otherwise hide the overlay
                        else if (hWndFg != ProcessManager.GameProcess.MainWindowHandle && topMost)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Topmost = false;
                                Visibility = Visibility.Hidden;
                            });
                        }
                    }
                    else
                    {
                        // Game not running or not focused, disable overlay
                        Dispatcher.Invoke(() =>
                        {
                            Topmost = false;
                            Visibility = Visibility.Hidden;
                        });
                    }
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception ex)
                {
                    Log.WriteLine($"Exception occured during overlay update: {ex.Message}");
                }
                finally
                {
                    Thread.Sleep(100);
                }
        }
    }
}