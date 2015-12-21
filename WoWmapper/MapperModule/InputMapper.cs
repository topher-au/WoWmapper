using System;
using System.Windows.Forms;
using WoWmapper.Input;

namespace WoWmapper.MapperModule
{
    internal class InputMapper
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private bool[] keyStates = new bool[Enum.GetNames(typeof(Keys)).Length];
        private bool[] moveKeys;
        private WindowScanner scanner;
        public InputMapper(WindowScanner Scanner)
        {
            scanner = Scanner; moveKeys = new bool[Enum.GetNames(typeof(Direction)).Length];
        }
        public enum Direction
        {
            Forward,
            Backward,
            Left,
            Right,
        }

        public void DoMouseEvent(uint Flags)
        {
            // Send a mouse event with the specified flags to the cursor position
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            WinApi.mouse_event(Flags, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }

        public bool IsButtonDown(InputButton Button)
        {
            return false;
        }

        public bool IsKeyDown(Keys Key)
        {
            return keyStates[(int)Key];
        }

        public void SendKeyDown(Keys Key)
        {
            if (scanner.IsAttached)
            {
                WinApi.PostMessage(scanner.WindowHandle, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
                keyStates[(int)Key] = true;
            }
        }

        public void SendKeyUp(Keys Key)
        {
            if (scanner.IsAttached)
            {
                WinApi.PostMessage(scanner.WindowHandle, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
                keyStates[(int)Key] = false;
            }
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
    }
}