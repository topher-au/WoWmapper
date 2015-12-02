using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public class WoWInteraction : IDisposable
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        internal static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, Rectangle rect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public void DoMouseDown(MouseButtons Button)
        {
            switch(Button)
            {
                case MouseButtons.Left:
                    MouseEvent(MOUSEEVENTF_LEFTDOWN);
                    break;
                case MouseButtons.Right:
                    MouseEvent(MOUSEEVENTF_RIGHTDOWN);
                    break;
            }
        }

        public void DoMouseUp(MouseButtons Button)
        {
            switch (Button)
            {
                case MouseButtons.Left:
                    MouseEvent(MOUSEEVENTF_LEFTUP);
                    break;
                case MouseButtons.Right:
                    MouseEvent(MOUSEEVENTF_RIGHTUP);
                    break;
            }
        }

        private void MouseEvent(uint Flags)
        {
            // Send a mouse event with the specified flags to the cursor position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(Flags, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public void DoLeftClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public void DoRightClick()
        {
            //Call the imported function with the cursor's current position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_RBUTTONUP = 0x0205;

        public bool IsAttached { get; private set; }
        public bool AdvancedHapticsEnabled { get; private set; }
        public bool AdvancedHapticsAttached { get; private set; }
        public bool PostMessageMouse { get; set; } = false;
        public bool PostMessageKeys { get; set; } = true;

        private IntPtr wowHandle;
        private Thread scannerThread;
        private KeyBind bindings;
        private bool[] moveKeys;

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
                        wowHandle = wowWindow;
                        IsAttached = true;
                    }
                }
                else
                {
                    // WoW window not found
                    wowHandle = IntPtr.Zero;
                    IsAttached = false;
                }

                Thread.Sleep(1000);
            }
        }

        public void SendKeyDown(Keys Key)
        {
            PostMessage(wowHandle, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
        }

        public void SendKeyPress(Keys Key)
        {
            PostMessage(wowHandle, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
            PostMessage(wowHandle, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
        }

        public void SendKeyUp(Keys Key)
        {
            PostMessage(wowHandle, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
        }

        private IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xFFFF));
        }

        public void SendClick(MouseButtons Button)
        {
            Rectangle wowRect = new Rectangle();
            GetWindowRect(wowHandle, wowRect);

            var relX = Cursor.Position.X - wowRect.Left;
            var relY = Cursor.Position.Y - wowRect.Top;

            switch (Button)
            {
                case MouseButtons.Left:
                    if (PostMessageMouse)
                    {
                        PostMessage(wowHandle, WM_LBUTTONDOWN, (IntPtr)1, MakeLParam(relX, relY));
                        PostMessage(wowHandle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                    }
                    else
                    {
                        DoLeftClick();
                    }
                    break;

                case MouseButtons.Right:
                    if (PostMessageMouse)
                    {
                        //PostMessage(wowHandle, WM_RBUTTONDOWN, (IntPtr)1, MakeLParam(relX, relY));
                        //PostMessage(wowHandle, WM_RBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                    }
                    else
                    {
                        DoRightClick();
                    }
                    break;
            }
        }

        public enum Direction
        {
            Forward,
            Backward,
            Left,
            Right,
            StopX,
            StopY
        }

        public static class MoveBindName {
            public static string Forward = "LStickUp";
            public static string Backward = "LStickDown";
            public static string Left = "LStickLeft";
            public static string Right = "LStickRight";
        }
    }
}