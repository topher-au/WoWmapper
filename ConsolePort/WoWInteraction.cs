using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsolePort;
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

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;

        public bool  IsAttached { get; private set; }

        IntPtr hWnd;
        Thread scannerThread;
        KeyBind bindings;
        bool[] moveKeys;

        public WoWInteraction(KeyBind Bindings)
        {
            scannerThread = new Thread(WoWScanner);
            scannerThread.Start();

            moveKeys = new bool[Enum.GetNames(typeof(Direction)).Length];
            bindings = Bindings;
        }

        public void Dispose()
        {
            if(scannerThread != null)
                scannerThread.Abort();
        }

        private void WoWScanner()
        {
            while(true)
            {
                var wowWindow = FindWindow(IntPtr.Zero, "World of Warcraft");
                if (wowWindow != IntPtr.Zero)
                {
                    hWnd = wowWindow;
                    IsAttached = true;
                } else
                {
                    hWnd = IntPtr.Zero;
                    IsAttached = false;
                }
                Thread.Sleep(100);
            }
        }

        public void SendKeyDown(Keys Key)
        {
            PostMessage(hWnd, WM_KEYDOWN, (IntPtr)Key, IntPtr.Zero);
        }

        public void SendKeyUp(Keys Key)
        {
            PostMessage(hWnd, WM_KEYUP, (IntPtr)Key, IntPtr.Zero);
        }

        public void Move(Direction Dir)
        {
            switch(Dir)
            {
                case Direction.Forward:
                    SendKeyDown(bindings.FromName("MoveForward").Key.Value);
                    SendKeyUp(bindings.FromName("MoveBackward").Key.Value);
                    moveKeys[(int)Direction.Forward] = true;
                    moveKeys[(int)Direction.Backward] = false;
                    break;
                case Direction.Backward:
                    SendKeyDown(bindings.FromName("MoveBackward").Key.Value);
                    SendKeyUp(bindings.FromName("MoveForward").Key.Value);
                    moveKeys[(int)Direction.Backward] = true;
                    moveKeys[(int)Direction.Forward] = false;
                    break;
                case Direction.Left:
                    SendKeyDown(bindings.FromName("MoveLeft").Key.Value);
                    SendKeyUp(bindings.FromName("MoveRight").Key.Value);
                    moveKeys[(int)Direction.Left] = true;
                    moveKeys[(int)Direction.Right] = false;
                    break;
                case Direction.Right:
                    SendKeyDown(bindings.FromName("MoveRight").Key.Value);
                    SendKeyUp(bindings.FromName("MoveLeft").Key.Value);
                    moveKeys[(int)Direction.Right] = true;
                    moveKeys[(int)Direction.Left] = false;
                    break;
                case Direction.StopX:
                    if (moveKeys[(int)Direction.Left])
                    {
                        SendKeyUp(bindings.FromName("MoveLeft").Key.Value);
                        moveKeys[(int)Direction.Left] = false;
                    }
                    if (moveKeys[(int)Direction.Right])
                    {
                        SendKeyUp(bindings.FromName("MoveRight").Key.Value);
                        moveKeys[(int)Direction.Right] = false;
                    }
                    break;

                case Direction.StopY:
                    if (moveKeys[(int)Direction.Forward])
                    {
                        SendKeyUp(bindings.FromName("MoveForward").Key.Value);
                        moveKeys[(int)Direction.Forward] = false;
                    }
                    if (moveKeys[(int)Direction.Backward])
                    {
                        SendKeyUp(bindings.FromName("MoveBackward").Key.Value);
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
