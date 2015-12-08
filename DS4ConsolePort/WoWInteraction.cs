using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public class WoWInteraction : IDisposable
    {
        #region Windows API declarations

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        internal static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_RBUTTONUP = 0x0205;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        #endregion Windows API declarations

        public bool IsAttached { get; private set; }
        public bool AdvancedHapticsEnabled { get; private set; }
        public bool AdvancedHapticsAttached { get; private set; }
        public bool PostMessageKeys { get; set; } = true;

        private Process wowProcess;
        private Thread scannerThread;
        private KeyBind bindings;
        private bool[] moveKeys;
        private bool[] keyStates = new bool[Enum.GetNames(typeof(Keys)).Length];

        public WoWInteraction(KeyBind Bindings)
        {
            scannerThread = new Thread(WindowScanner);
            scannerThread.Start();

            moveKeys = new bool[Enum.GetNames(typeof(Direction)).Length];
            bindings = Bindings;
        }

        public void Dispose()
        {
            if (scannerThread != null)
                scannerThread.Abort();
        }

        public bool IsKeyDown(Keys Key)
        {
            return keyStates[(int)Key];
        }

        public RECT WoWWindow
        {
            get
            {
                RECT wowRect = new RECT();
                var b = GetWindowRect(new HandleRef(this, wowProcess.MainWindowHandle), out wowRect);
                return wowRect;
            }
        }

        private void WindowScanner()
        {
            while (scannerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                // Scan for WoW process and get window handle
                var wowProcesses = Process.GetProcessesByName("WoW-64");

                if (wowProcesses.Length > 0)
                {
                    var wowWindow = wowProcesses[0].MainWindowHandle;
                    if (wowWindow != IntPtr.Zero)
                    {
                        // WoW window found
                        wowProcess = wowProcesses[0];
                        IsAttached = true;
                    }
                }
                else
                {
                    // WoW window not found
                    wowProcess = null;
                    IsAttached = false;
                }

                Thread.Sleep(1000);
            }
        }

        public void SendKeyDown(Keys Key)
        {
            if (IsAttached)
            {
                PostMessage(wowProcess.MainWindowHandle, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
                keyStates[(int)Key] = true;
            }
        }

        public void SendKeyPress(Keys Key)
        {
            if (IsAttached)
            {
                PostMessage(wowProcess.MainWindowHandle, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
                PostMessage(wowProcess.MainWindowHandle, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
            }
        }

        public void SendKeyUp(Keys Key)
        {
            if (IsAttached)
            {
                PostMessage(wowProcess.MainWindowHandle, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
                keyStates[(int)Key] = false;
            }
        }

        private IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        #region Mouse Functions

        private void DoMouseEvent(uint Flags)
        {
            // Send a mouse event with the specified flags to the cursor position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(Flags, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public void SendMouseDown(MouseButtons Button)
        {
            switch (Button)
            {
                case MouseButtons.Left:
                    DoMouseEvent(MOUSEEVENTF_LEFTDOWN);
                    break;

                case MouseButtons.Right:
                    DoMouseEvent(MOUSEEVENTF_RIGHTDOWN);
                    break;
            }
        }

        public void SendMouseUp(MouseButtons Button)
        {
            switch (Button)
            {
                case MouseButtons.Left:
                    DoMouseEvent(MOUSEEVENTF_LEFTUP);
                    break;

                case MouseButtons.Right:
                    DoMouseEvent(MOUSEEVENTF_RIGHTUP);
                    break;
            }
        }

        public void SendLeftClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public void SendRightClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        #endregion Mouse Functions

        public enum Direction
        {
            Forward,
            Backward,
            Left,
            Right,
            StopX,
            StopY
        }

        public static class MoveBindName
        {
            public static string Forward = "LStickUp";
            public static string Backward = "LStickDown";
            public static string Left = "LStickLeft";
            public static string Right = "LStickRight";
        }
    }
}