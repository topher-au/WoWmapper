using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DS4Windows;
using J2i.Net.XInputWrapper;
using WoWmapper.Controllers.DS4;
using WoWmapper.Controllers.Xbox;
using WoWmapper.Keybindings;
using WoWmapper.Overlay;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using Application = System.Windows.Application;
using Cursor = System.Windows.Forms.Cursor;
using Point = System.Windows.Point;

namespace WoWmapper.Controllers
{
    public static class ControllerManager
    {
        #region Public Fields

        public static IController ActiveController;
        public static readonly List<IController> Controllers = new List<IController>();
        public static DS4Interface DS4;
        public static XInputInterface XInput;
        public static bool IsXInput9 => XboxController.IsXInput9;
        #endregion

        #region Private Fields

        private static Thread _watcherThread = new Thread(ControllerWatcher);
        private static bool _threadRunning;
        private static int _lastBatteryWarn = 100;

        #endregion
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        static IntPtr HWND_MESSAGE = new IntPtr(-3);

        #region Public Delegates

        public delegate void ControllerButtonStateChangedHandler(GamepadButton button, bool state);

        public delegate void ActiveControllerChangedHandler(IController controller);

        public delegate void ControllersChangedHandler();

        #endregion

        #region Public Events

        public static event ControllerButtonStateChangedHandler ControllerButtonStateChanged;
        public static event ControllersChangedHandler ControllersChanged;
        public static event ActiveControllerChangedHandler ActiveControllerChanged;

        #endregion

        #region Public Methods
        public static void Start()
        {
            Log.WriteLine("Controller manager starting up...");
            _threadRunning = true;
            DS4 = new DS4Interface();
            XInput = new XInputInterface();
            _watcherThread.Start();
        }

        private static void UsbNotifierOnDeviceDisconnected()
        {
            Log.WriteLine("USB device disconnected");
            DS4.Scan();
        }

        private static void UsbNotifierOnDeviceConnected()
        {
            Log.WriteLine("USB device connected");
            DS4.Scan();
        }

        public static void Stop()
        {
            Log.WriteLine("Controller manager shutting down...");
            _threadRunning = false;
            DS4.Shutdown();
            XInput.Shutdown();
        }

        public static BitmapImage GetButtonIcon(GamepadButton button)
        {
            switch (Properties.Settings.Default.ButtonStyle)
            {
                case 0:
                    if (ActiveController != null)
                    {
                        switch (ActiveController.Type)
                        {
                            case GamepadType.PlayStation:
                                return DS4Data.GetButtonImage(button);
                            case GamepadType.Xbox:
                                return XboxData.GetButtonImage(button);
                        }
                    }
                    goto default;
                case 1:
                    return DS4Data.GetButtonImage(button);
                case 2:
                    return XboxData.GetButtonImage(button);
                default:
                    return XboxData.GetButtonImage(button);
            }
        }

        public static string GetButtonName(GamepadButton button)
        {
            switch (Properties.Settings.Default.ButtonStyle)
            {
                case 0:
                    if (ActiveController != null)
                    {
                        switch (ActiveController.Type)
                        {
                            case GamepadType.PlayStation:
                                return DS4Data.GetButtonName(button);
                            case GamepadType.Xbox:
                                return XboxData.GetButtonName(button);
                        }
                    }
                    goto default;
                case 1:
                    return DS4Data.GetButtonName(button);
                case 2:
                    return XboxData.GetButtonName(button);
                default:
                    return XboxData.GetButtonName(button);
            }
        }

        public static void RefreshControllers()
        {
            var controllerList = new List<IController>();

            DS4.Scan();

            if (DS4 != null)
                controllerList.AddRange(DS4.Controllers);
            if (XInput != null)
                controllerList.AddRange(XInput.Controllers);

            foreach (var controller in controllerList)
            {
                lock (Controllers)
                {
                    if (controller == null || Controllers.Contains(controller) || !controller.IsAlive()) continue;
                    Controllers.Add(controller);
                    if (ControllersChanged != null)
                        Application.Current.Dispatcher.Invoke(ControllersChanged);
                }
            }
        }

        public static void SetController(IController controller)
        {
            if (ActiveController != null)
                ActiveController.ButtonStateChanged -= ActiveControllerOnButtonStateChanged;
            ActiveController = controller;

            if (controller != null)
            {
                ActiveController.ButtonStateChanged += ActiveControllerOnButtonStateChanged;
                Log.WriteLine("Selected controller: " + controller.Name);
                var device = ActiveController.UnderlyingController as DS4Device;
                if (device != null)
                {
                    var ds4 = device;
                    ds4.Touchpad.TouchesMoved += DS4TouchpadMoved;
                    ds4.Touchpad.TouchButtonDown += DS4TouchpadButtonDown;
                    ds4.Touchpad.TouchButtonUp += DS4TouchpadButtonUp;
                }
                App.Overlay.PopupNotification(new OverlayNotification()
                {
                    Header = "Controller selected",
                    Content = $"Controller {controller.Name} is now the active controller.",
                });
            }

            if (ActiveControllerChanged != null)
                Application.Current.Dispatcher.Invoke(() => { ActiveControllerChanged?.Invoke(controller); });

            ConsolePort.BindWriter.WriteBinds();

            MainWindow.UpdateButtonStyle();
        }

        private static void DS4TouchpadButtonUp(object sender, TouchpadEventArgs args)
        {
            if (!Settings.Default.EnableTouchpad) return;

            switch (Settings.Default.TouchpadMode)
            {
                case 0:
                    WoWInput.SendMouseUp(args.touches.Last().hwX < (1920/2) ? MouseButton.Left : MouseButton.Right);
                    break;
                case 1:
                    WoWInput.SendKeyUp(args.touches.Last().hwX < (1920/2)
                        ? BindManager.GetKey(GamepadButton.CenterLeft)
                        : BindManager.GetKey(GamepadButton.CenterRight));
                    break;
            }
        }

        private static void DS4TouchpadButtonDown(object sender, TouchpadEventArgs args)
        {
            if (!Settings.Default.EnableTouchpad) return;

            switch (Settings.Default.TouchpadMode)
            {
                case 0:
                    WoWInput.SendMouseDown(args.touches.Last().hwX < (1920/2) ? MouseButton.Left : MouseButton.Right);
                    break;
                case 1:
                    WoWInput.SendKeyDown(args.touches.Last().hwX < (1920/2)
                        ? BindManager.GetKey(GamepadButton.CenterLeft)
                        : BindManager.GetKey(GamepadButton.CenterRight));
                    break;
            }
        }

        private static void DS4TouchpadMoved(object sender, TouchpadEventArgs args)
        {
            if (!Settings.Default.EnableTouchpad) return;

            var x = args.touches.Last().deltaX;
            var y = args.touches.Last().deltaY;

            var cur = Cursor.Position;
            cur.X += x;
            cur.Y += y;
            Cursor.Position = cur;
        }

        public static Point GetLeftAxis()
        {
            if (ActiveController == null) return new Point(0, 0);
            var x = ActiveController.GetAxis(GamepadAxis.StickLeftX);
            var y = ActiveController.GetAxis(GamepadAxis.StickLeftY);
            return new Point(x, y);
        }

        public static Point GetRightAxis()
        {
            if (ActiveController == null) return new Point(0, 0);
            var x = ActiveController.GetAxis(GamepadAxis.StickRightX);
            var y = ActiveController.GetAxis(GamepadAxis.StickRightY);
            return new Point(x, y);
        }

        public static void SendRumble(byte left, byte right, int duration)
        {
            ActiveController?.SendRumble(left, right, duration);
        }

        #endregion

        #region Private Methods

        private static void ActiveControllerOnButtonStateChanged(GamepadButton button, bool state)
        {
            ControllerButtonStateChanged?.Invoke(button, state);
        }

        private static bool _showedBatteryLow;
        private static bool _showedBatteryCritical;
        private static void ControllerWatcher()
        {
            while (_threadRunning)
            {
                lock (Controllers)
                {
                    // Check validity of connected controllers
                    var deadControllers = Controllers.Where(controller => !controller.IsAlive()).ToList();

                    if (deadControllers.Contains(ActiveController))
                    {
                        SetController(null);
                        Application.Current.Dispatcher.Invoke(() => { ActiveControllerChanged?.Invoke(null); });
                    }

                    // Remove disconnected devices
                    for (int i = 0; i < deadControllers.Count; i++)
                    {
                        deadControllers[i].Stop();
                        Controllers.Remove(deadControllers[i]);
                        App.Overlay.PopupNotification(new OverlayNotification()
                        {
                            Header = "Controller disconnected",
                            Content = $"Controller {deadControllers[i].Name} was disconnected.",
                        });
                        deadControllers[i] = null;
                    }

                    if (deadControllers.Count > 0)
                    {
                        if (ControllersChanged != null)
                            Application.Current.Dispatcher.Invoke(ControllersChanged);
                    }

                    if (ActiveController == null)
                    {
                        _lastBatteryWarn = 100;
                        RefreshControllers();
                        if (Controllers.Count > 0)
                        {
                            try
                            {
                                var firstActive = Controllers.First(device => device.IsAlive());
                                SetController(firstActive);
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (Settings.Default.EnableOverlay && Settings.Default.EnableOverlayBattery && ActiveController != null)
                    {
                        var battery = ActiveController.BatteryLevel;
                        if (battery > _lastBatteryWarn)
                        {
                            _lastBatteryWarn = 100;
                            if (battery > 35) _showedBatteryCritical = false;
                            if (battery > 45) _showedBatteryLow = false;
                        }
                        else if (battery <= _lastBatteryWarn - 10) // If battery has dropped by 10%
                        {
                            if (battery <= 40 && !_showedBatteryLow)
                            {
                                App.Overlay.PopupNotification(new OverlayNotification()
                                {
                                    Content = $"Your controller battery has reached {battery}%.",
                                    Header = "Battery low",
                                    UniqueID = "LOW_BATTERY",
                                });
                                _showedBatteryLow = true;
                            } else if (battery <= 30 && !_showedBatteryCritical)
                            {
                                App.Overlay.PopupNotification(new OverlayNotification()
                                {
                                    Content =
                                    $"Your controller battery has reached {battery}%. Plug in now to avoid unexpected interruption.",
                                    Header = "Battery critically low",
                                    UniqueID = "LOW_BATTERY",
                                });
                                _showedBatteryCritical = true;
                            }
                            _lastBatteryWarn -= 10;
                        }
                    }
                }

                Thread.Sleep(2000);
            }
        }

        #endregion
    }
}