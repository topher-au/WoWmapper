
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsolePort;
using ConsolePort.AdvancedHaptics;
using System.Windows.Forms;

namespace ConsolePort
{
    public class WoWInteraction : IDisposable
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        internal static extern IntPtr FindWindow(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, Rectangle rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint WM_LBUTTONDOWN = 0x0201;
        const uint WM_LBUTTONUP = 0x0202;
        const uint WM_RBUTTONDOWN = 0x0204;
        const uint WM_RBUTTONUP = 0x0205;

        public bool IsAttached { get; private set; }
        public bool AdvancedHapticsEnabled { get; private set; }
        public bool AdvancedHapticsAttached { get; private set; }

        IntPtr wowHandle;
        Thread scannerThread;
        KeyBind bindings;
        bool[] moveKeys;

        public WoWInteraction(KeyBind Bindings)
        {
            scannerThread = new Thread(WindowScanner);
            scannerThread.Start();

            moveKeys = new bool[Enum.GetNames(typeof(Direction)).Length];
            bindings = Bindings;
        }

        public void Dispose()
        {
            if(scannerThread != null)
                scannerThread.Abort();
        }

        private void WindowScanner()
        {
            while(true)
            {
                var wowWindow = FindWindow(IntPtr.Zero, "World of Warcraft");
                if (wowWindow != IntPtr.Zero)
                {
                    // WoW window found
                    wowHandle = wowWindow;
                    IsAttached = true;
                } else
                {
                    // WoW window not found
                    wowHandle = IntPtr.Zero;
                    IsAttached = false;
                }
                Thread.Sleep(5000);
            }
        }

        public void SendKeyDown(Keys Key)
        {
            PostMessage(wowHandle, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
        }

        public void SendKeyUp(Keys Key)
        {
            PostMessage(wowHandle, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
        }

        private IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xFFFF));
        }


        public void SendClick(MouseButton Button)
        {
            Rectangle wowRect = new Rectangle();
            GetWindowRect(wowHandle, wowRect);

            var relX = Cursor.Position.X - wowRect.Left;
            var relY = Cursor.Position.Y - wowRect.Top;

            switch(Button)
            {
                case MouseButton.Left:
                    PostMessage(wowHandle, WM_LBUTTONDOWN, (IntPtr)1, MakeLParam(relX, relY));
                    PostMessage(wowHandle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                    break;
                case MouseButton.Right:
                    PostMessage(wowHandle, WM_RBUTTONDOWN, (IntPtr)1, MakeLParam(relX, relY));
                    PostMessage(wowHandle, WM_RBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                    break;
            }
        }

        public enum MouseButton
        {
            Left,
            Right
        }

        public void Move(Direction Dir)
        {
            switch(Dir)
            {
                case Direction.Forward:
                    SendKeyDown(bindings.FromName("LStickUp").Key.Value);
                    SendKeyUp(bindings.FromName("LStickDown").Key.Value);
                    moveKeys[(int)Direction.Forward] = true;
                    moveKeys[(int)Direction.Backward] = false;
                    break;
                case Direction.Backward:
                    SendKeyDown(bindings.FromName("LStickDown").Key.Value);
                    SendKeyUp(bindings.FromName("LStickUp").Key.Value);
                    moveKeys[(int)Direction.Backward] = true;
                    moveKeys[(int)Direction.Forward] = false;
                    break;
                case Direction.Left:
                    SendKeyDown(bindings.FromName("LStickLeft").Key.Value);
                    SendKeyUp(bindings.FromName("LStickRight").Key.Value);
                    moveKeys[(int)Direction.Left] = true;
                    moveKeys[(int)Direction.Right] = false;
                    break;
                case Direction.Right:
                    SendKeyDown(bindings.FromName("LStickRight").Key.Value);
                    SendKeyUp(bindings.FromName("LStickLeft").Key.Value);
                    moveKeys[(int)Direction.Right] = true;
                    moveKeys[(int)Direction.Left] = false;
                    break;
                case Direction.StopX:
                    if (moveKeys[(int)Direction.Left])
                    {
                        SendKeyUp(bindings.FromName("LStickLeft").Key.Value);
                        moveKeys[(int)Direction.Left] = false;
                    }
                    if (moveKeys[(int)Direction.Right])
                    {
                        SendKeyUp(bindings.FromName("LStickRight").Key.Value);
                        moveKeys[(int)Direction.Right] = false;
                    }
                    break;

                case Direction.StopY:
                    if (moveKeys[(int)Direction.Forward])
                    {
                        SendKeyUp(bindings.FromName("LStickUp").Key.Value);
                        moveKeys[(int)Direction.Forward] = false;
                    }
                    if (moveKeys[(int)Direction.Backward])
                    {
                        SendKeyUp(bindings.FromName("LStickDown").Key.Value);
                        moveKeys[(int)Direction.Backward] = false;
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
    }
}
