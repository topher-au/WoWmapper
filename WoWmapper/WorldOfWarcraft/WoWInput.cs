using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WoWmapper.Input;
using Cursor = System.Windows.Forms.Cursor;

namespace WoWmapper.WorldOfWarcraft
{
    public static class WoWInput
    {
        #region Native Methods and Structures

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;

        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        #endregion

        private static Process _process => ProcessManager.GameProcess;

        public static void SendKeyDown(Key key, bool forceDirect = false)
        {
            if (_process == null) return;

            if (Properties.Settings.Default.InputDirectKeyboard)
            {
                // Send direct key messages to WoW window
                PostMessage(new HandleRef(null, _process.MainWindowHandle), WM_KEYDOWN,
                    (IntPtr) KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
            }
            else
            {
                // Simulate hardware inputs
                KeyInput.SendKey(key, true);
            }
        }

        public static void SendKeyUp(Key key, bool forceDirect = false)
        {
            if (_process == null) return;

            if (Properties.Settings.Default.InputDirectKeyboard)
            {
                // Send direct key messages to WoW window
                PostMessage(new HandleRef(null, _process.MainWindowHandle), WM_KEYUP,
                    (IntPtr) KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
            }
            else
            {
                // Simulate hardware inputs
                KeyInput.SendKey(key, false);
            }
        }

        public static void SendMouseClick(MouseButton button, bool forceDirect = false)
        {
            SendMouseDown(button);
            SendMouseUp(button);
        }

        public static void SendMouseDown(MouseButton button, bool forceDirect = false)
        {
            if (_process == null) return;
            
            if (Properties.Settings.Default.InputDirectMouse)
            {
                // Send direct mouse click messages to WoW window
                uint mouseMessage = 0;

                switch (button)
                {
                    case MouseButton.Left:
                        mouseMessage = WM_LBUTTONDOWN;
                        break;
                    case MouseButton.Right:
                        mouseMessage = WM_RBUTTONDOWN;
                        break;
                }

                var lparamCoords = Cursor.Position.X |
                                   (Cursor.Position.Y << 16);

                PostMessage(new HandleRef(null, _process.MainWindowHandle), mouseMessage,
                    (IntPtr)lparamCoords, IntPtr.Zero);
            }
            else
            {
                // Simulate hardware inputs
                KeyInput.SendMouse(button, true);
            }
        }

        public static void SendMouseUp(MouseButton button, bool forceDirect = false)
        {
            if (_process == null) return;
            
            if (Properties.Settings.Default.InputDirectMouse)
            {
                // Send direct mouse click messages to WoW window
                uint mouseMessage = 0;

                switch (button)
                {
                    case MouseButton.Left:
                        mouseMessage = WM_LBUTTONUP;
                        break;
                    case MouseButton.Right:
                        mouseMessage = WM_RBUTTONUP;
                        break;
                }

                var lparamCoords = Cursor.Position.X |
                                   (Cursor.Position.Y << 16);

                PostMessage(new HandleRef(null, _process.MainWindowHandle), mouseMessage,
                    (IntPtr)lparamCoords, IntPtr.Zero);
            }
            else
            {
                // Simulate hardware inputs
                KeyInput.SendMouse(button, false);
            }
        }
    }
}