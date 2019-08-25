using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WoWmapper.Input;
using WoWmapper.Keybindings;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using Cursor = System.Windows.Forms.Cursor;

namespace WoWmapper.Controllers
{
    public static class InputMapper
    {
        private static readonly Thread _inputThread = new Thread(InputWatcherThread);
        private static readonly bool[] _keyStates = new bool[Enum.GetNames(typeof (GamepadButton)).Length];
        private static bool _threadRunning;

        static InputMapper()
        {
            ControllerManager.ControllerButtonStateChanged += ActiveController_ButtonStateChanged;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static void InputWatcherThread()
        {
            while (_threadRunning)
            {
                var axisMovement = Settings.Default.SwapSticks
                    ? ControllerManager.GetRightAxis()
                    : ControllerManager.GetLeftAxis();
                var axisCursor = Settings.Default.SwapSticks
                    ? ControllerManager.GetLeftAxis()
                    : ControllerManager.GetRightAxis();

                ProcessMovement(axisMovement);
                ProcessCursor(axisCursor);

                Thread.Sleep(5);
            }
        }

        private static void ProcessMovement(Point axis)
        {
            //var strength = Math.Sqrt(axis.X*axis.X + axis.Y*axis.Y);
            var threshold = Settings.Default.MovementThreshold;

            var left  = -axis.X > threshold;
            var right =  axis.X > threshold;
            var up    = -axis.Y > threshold;
            var down  =  axis.Y > threshold;

            ProcessButton(GamepadButton.LeftStickLeft, left);
            ProcessButton(GamepadButton.LeftStickRight, right);
            ProcessButton(GamepadButton.LeftStickUp, up);
            ProcessButton(GamepadButton.LeftStickDown, down);

            if (!Settings.Default.SimpleRadial) {
                var absX = Math.Abs(axis.X);
                var absY = Math.Abs(axis.Y);

                var horz = left || right;
                var vert = up   || down;

                var sendHorz = horz && vert && absX > absY * 1.5;
                var sendVert = horz && vert && absY > absX * 1.5;
            
                ProcessButton(GamepadButton.LeftStickHorz, sendHorz);
                ProcessButton(GamepadButton.LeftStickVert, sendVert);
            }
        }

        private static void ProcessCursor(Point axis)
        {
            var axisInput = new Vector2(axis.X, axis.Y);
            if (axisInput.Magnitude > Settings.Default.CursorDeadzone)
            {
                axisInput = Vector2.Normalize(axisInput)*
                            ((axisInput.Magnitude - Settings.Default.CursorDeadzone)/
                             (127 - Settings.Default.CursorDeadzone));

                var curve = 0.05*Settings.Default.CursorCurve;

                var xSpeed = (axisInput.X < 0 ? -axisInput.X : axisInput.X)*Settings.Default.CursorSpeed;
                var ySpeed = (axisInput.Y < 0 ? -axisInput.Y : axisInput.Y)*Settings.Default.CursorSpeed;

                var xMath = Math.Pow(curve*xSpeed, 2) + curve*xSpeed;
                var yMath = Math.Pow(curve*ySpeed, 2) + curve*ySpeed;

                //xMath *= MemoryManager.ReadAoeState() ? 0.5 : 1;
                //yMath *= MemoryManager.ReadAoeState() ? 0.5 : 1;

                var mouseMovement = new Vector2(xMath, yMath);

                if (axis.X < 0) mouseMovement.X = -mouseMovement.X;
                if (axis.Y < 0) mouseMovement.Y = -mouseMovement.Y;
                

                if (Settings.Default.InputHardwareMouse)
                {
                    HardwareInput.MoveMouse((int) mouseMovement.X, (int) mouseMovement.Y);
                }
                else
                {
                    var m = Cursor.Position;
                    m.X += (int) mouseMovement.X;
                    m.Y += (int) mouseMovement.Y;
                    Cursor.Position = m;
                }
            }
        }

        private static void ProcessCharacterMenu(GamepadButton button, bool state)
        {
            switch (button)
            {
                case GamepadButton.LFaceUp:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Up);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Up);
                    break;
                case GamepadButton.LFaceDown:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Down);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Down);
                    break;
                case GamepadButton.RFaceDown:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Enter);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Enter);
                    break;
                case GamepadButton.CenterMiddle:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Escape);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Escape);
                    break;
            }

            // Do left/right mouse buttons
            if (button == GamepadButton.LeftStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Left);
                else
                    WoWInput.SendMouseUp(MouseButton.Left);
            }
            else if (button == GamepadButton.RightStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Right);
                else
                    WoWInput.SendMouseUp(MouseButton.Right);
            }

            _keyStates[(int) button] = state;
        }

        private static void ProcessInput(GamepadButton button, bool state)
        {
            // Do left/right mouse buttons
            if (button == GamepadButton.LeftStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Left);
                else
                    WoWInput.SendMouseUp(MouseButton.Left);
            }
            else if (button == GamepadButton.RightStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Right);
                else
                    WoWInput.SendMouseUp(MouseButton.Right);
            }

            // Do other buttons
            ProcessButton(button, state);
        }

        private static bool ProcessButton(GamepadButton button, bool state)
        {
            if (_keyStates[(int) button] != state)
            {
                if (state)
                    WoWInput.SendKeyDown(BindManager.GetKey(button));
                else
                    WoWInput.SendKeyUp(BindManager.GetKey(button));

                _keyStates[(int) button] = state;
            }
            return state;
        }

        private static void ActiveController_ButtonStateChanged(GamepadButton button, bool state)
        {
            // Process input
            ProcessInput(button, state);
        }

        public static void Start()
        {
            _threadRunning = true;
            _inputThread.Start();
        }

        public static void Stop()
        {
            _threadRunning = false;
        }
    }
}