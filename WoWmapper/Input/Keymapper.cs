using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WoWmapper.Classes;
using WoWmapper.Controllers;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.Input
{
    public static class Keymapper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private static Thread _inputThread;
        private static bool[] keyStates = new bool[Enum.GetNames(typeof(ControllerButton)).Length];

        private static int _lastTouchX;
        private static int _lastTouchY;

        public static void Start()
        {
            if (_inputThread != null) return;
            Logger.Write("Starting controller state thread...");
            _inputThread = new Thread(ControllerThread) { IsBackground = true };
            _inputThread.Start();
        }

        public static void Stop()
        {
            Logger.Write("Stopping controller state thread...");
            _inputThread?.Abort();
            _inputThread = null;
        }

        private static void ControllerThread()
        {
            while (true)
            {
                var controller = ControllerManager.GetActiveController();

                if (controller != null)
                    lock (controller)
                    {
                        var state = controller.GetControllerState();
                        var cTouch = controller.GetTouchpadState();

                        // Process button inputs
                        if (state.ShoulderLeft) ProcessKeyDown(ControllerButton.ShoulderLeft);
                        if (!state.ShoulderLeft) ProcessKeyUp(ControllerButton.ShoulderLeft);

                        if (state.ShoulderRight) ProcessKeyDown(ControllerButton.ShoulderRight);
                        if (!state.ShoulderRight) ProcessKeyUp(ControllerButton.ShoulderRight);

                        if (state.LFaceUp) ProcessKeyDown(ControllerButton.LFaceUp);
                        if (!state.LFaceUp) ProcessKeyUp(ControllerButton.LFaceUp);

                        if (state.LFaceRight) ProcessKeyDown(ControllerButton.LFaceRight);
                        if (!state.LFaceRight) ProcessKeyUp(ControllerButton.LFaceRight);

                        if (state.LFaceDown) ProcessKeyDown(ControllerButton.LFaceDown);
                        if (!state.LFaceDown) ProcessKeyUp(ControllerButton.LFaceDown);

                        if (state.LFaceLeft) ProcessKeyDown(ControllerButton.LFaceLeft);
                        if (!state.LFaceLeft) ProcessKeyUp(ControllerButton.LFaceLeft);

                        if (state.RFaceUp) ProcessKeyDown(ControllerButton.RFaceUp);
                        if (!state.RFaceUp) ProcessKeyUp(ControllerButton.RFaceUp);

                        if (state.RFaceRight) ProcessKeyDown(ControllerButton.RFaceRight);
                        if (!state.RFaceRight) ProcessKeyUp(ControllerButton.RFaceRight);

                        if (state.RFaceDown) ProcessKeyDown(ControllerButton.RFaceDown);
                        if (!state.RFaceDown) ProcessKeyUp(ControllerButton.RFaceDown);

                        if (state.RFaceLeft) ProcessKeyDown(ControllerButton.RFaceLeft);
                        if (!state.RFaceLeft) ProcessKeyUp(ControllerButton.RFaceLeft);

                        if (state.CenterLeft) ProcessKeyDown(ControllerButton.CenterLeft);
                        if (!state.CenterLeft && !state.TouchButton) ProcessKeyUp(ControllerButton.CenterLeft);

                        if (state.CenterRight) ProcessKeyDown(ControllerButton.CenterRight);
                        if (!state.CenterRight && !state.TouchButton) ProcessKeyUp(ControllerButton.CenterRight);

                        if (state.CenterMiddle) ProcessKeyDown(ControllerButton.CenterMiddle);
                        if (!state.CenterMiddle) ProcessKeyUp(ControllerButton.CenterMiddle);

                        if (state.StickLeft) ProcessKeyDown(ControllerButton.StickLeft);
                        if (!state.StickLeft) ProcessKeyUp(ControllerButton.StickLeft);

                        if (state.StickRight) ProcessKeyDown(ControllerButton.StickRight);
                        if (!state.StickRight) ProcessKeyUp(ControllerButton.StickRight);

                        switch (Settings.Default.TouchpadMode)
                        {
                            case 0: // Mouse control
                                if (state.TouchLeft && state.TouchButton) ProcessKeyDown(ControllerButton.TouchLeft);
                                if (!state.TouchButton) ProcessKeyUp(ControllerButton.TouchLeft);
                                if (state.TouchRight && state.TouchButton) ProcessKeyDown(ControllerButton.TouchRight);
                                if (!state.TouchButton) ProcessKeyUp(ControllerButton.TouchRight);
                                break;
                            case 1: // Share/options
                                if (state.TouchLeft && state.TouchButton) ProcessKeyDown(ControllerButton.CenterLeft);
                                if (!state.TouchButton && !state.CenterLeft) ProcessKeyUp(ControllerButton.CenterLeft);
                                if (state.TouchRight && state.TouchButton) ProcessKeyDown(ControllerButton.CenterRight);
                                if (!state.TouchButton && !state.CenterRight) ProcessKeyUp(ControllerButton.CenterRight);
                                break;
                            case 2: // TouchLeft/TouchRight
                                if (state.TouchLeft && state.TouchButton) ProcessKeyDown(ControllerButton.TouchLeft);
                                if (!state.TouchButton) ProcessKeyUp(ControllerButton.TouchLeft);
                                if (state.TouchRight && state.TouchButton) ProcessKeyDown(ControllerButton.TouchRight);
                                if (!state.TouchButton) ProcessKeyUp(ControllerButton.TouchRight);
                                break;
                        }

                        if (Properties.Settings.Default.EnableTriggerGrip)
                        {
                            if ((state.TriggerLeft >= Properties.Settings.Default.ThresholdLeft) &&
                                (state.TriggerLeft < Properties.Settings.Default.ThresholdLeftClick)) ProcessKeyDown(ControllerButton.TriggerLeft);
                            if (!(state.TriggerLeft >= Properties.Settings.Default.ThresholdLeft) ||
                                !(state.TriggerLeft < Properties.Settings.Default.ThresholdLeftClick)) ProcessKeyUp(ControllerButton.TriggerLeft);

                            if ((state.TriggerRight >= Properties.Settings.Default.ThresholdRight) &&
                                (state.TriggerRight < Properties.Settings.Default.ThresholdRightClick)) ProcessKeyDown(ControllerButton.TriggerRight);
                            if (!(state.TriggerRight >= Properties.Settings.Default.ThresholdRight) ||
                                !(state.TriggerRight < Properties.Settings.Default.ThresholdRightClick)) ProcessKeyUp(ControllerButton.TriggerRight);

                            if (state.TriggerLeft >= Properties.Settings.Default.ThresholdLeftClick) ProcessKeyDown(ControllerButton.TriggerLeft2);
                            if (state.TriggerRight >= Properties.Settings.Default.ThresholdRightClick) ProcessKeyDown(ControllerButton.TriggerRight2);

                            if (state.TriggerLeft < Properties.Settings.Default.ThresholdLeftClick) ProcessKeyUp(ControllerButton.TriggerLeft2);
                            if (state.TriggerRight < Properties.Settings.Default.ThresholdRightClick) ProcessKeyUp(ControllerButton.TriggerRight2);
                        }
                        else
                        {
                            if (state.TriggerLeft >= Properties.Settings.Default.ThresholdLeft) ProcessKeyDown(ControllerButton.TriggerLeft);
                            if (!(state.TriggerLeft >= Properties.Settings.Default.ThresholdLeft)) ProcessKeyUp(ControllerButton.TriggerLeft);

                            if (state.TriggerRight >= Properties.Settings.Default.ThresholdLeft) ProcessKeyDown(ControllerButton.TriggerRight);
                            if (!(state.TriggerRight >= Properties.Settings.Default.ThresholdLeft)) ProcessKeyUp(ControllerButton.TriggerRight);
                        }

                        // Process analog sticks

                        // Left stick/Movement
                        if (state.LeftY < -40) ProcessKeyDown(ControllerButton.LStickUp);
                        if (!(state.LeftY < -40)) ProcessKeyUp(ControllerButton.LStickUp);
                        if (state.LeftY > 40) ProcessKeyDown(ControllerButton.LStickDown);
                        if (!(state.LeftY > 40)) ProcessKeyUp(ControllerButton.LStickDown);
                        if (state.LeftX < -40) ProcessKeyDown(ControllerButton.LStickLeft);
                        if (!(state.LeftX < -40)) ProcessKeyUp(ControllerButton.LStickLeft);
                        if (state.LeftX > 40) ProcessKeyDown(ControllerButton.LStickRight);
                        if (!(state.LeftX > 40)) ProcessKeyUp(ControllerButton.LStickRight);

                        // Right stick/Mouse
                        Vector2 stickInput = new Vector2(state.RightX, state.RightY);
                        
                        if (stickInput.Magnitude > Settings.Default.RightDeadzone)
                        {
                            stickInput = Vector2.Normalize(stickInput) * ((stickInput.Magnitude - Settings.Default.RightDeadzone) / (127 - Settings.Default.RightDeadzone));
                            Vector2 mouseMovement = ApplyMouseMath((float)stickInput.X, (float)stickInput.Y);

                            if (state.RightX < 0) mouseMovement.X = -mouseMovement.X;
                            if (state.RightY < 0) mouseMovement.Y = -mouseMovement.Y;

                            var m = Cursor.Position;
                            m.X += (int)mouseMovement.X;
                            m.Y += (int)mouseMovement.Y;
                            Cursor.Position = m;
                        }

                        // Touchpad
                        if (Settings.Default.TouchpadMode == 0)
                        {
                            if (cTouch != null && (cTouch.TouchX != _lastTouchX || cTouch.TouchY != _lastTouchY))
                            {
                                var m = Cursor.Position;
                                m.X += cTouch.DeltaX;
                                m.Y += cTouch.DeltaY;
                                Cursor.Position = m;
                                _lastTouchX = cTouch.TouchX;
                                _lastTouchY = cTouch.TouchY;
                            }
                        }
                    }

                Thread.Sleep(5);
            }
        }

        private static Vector2 ApplyMouseMath(float x, float y)
        {
            double curve = (0.05 * Settings.Default.RightCurve);

            var xSpeed = (x < 0 ? -x : x) * Settings.Default.RightSpeed;
            var ySpeed = (y < 0 ? -y : y) * Settings.Default.RightSpeed;

            double xMath = Math.Pow(curve * xSpeed, 2) + (curve * xSpeed);
            double yMath = Math.Pow(curve * ySpeed, 2) + (curve * ySpeed);

            var vMath = new Vector2(xMath, yMath);
            return vMath;
        }

        private static void ProcessKeyDown(ControllerButton Button)
        {
            // Don't process if key is already held
            if (keyStates[(int)Button] || BindingManager.CurrentBinds == null) return;

            // TODO send key logic
            switch (Button)
            {
                case ControllerButton.StickLeft:
                case ControllerButton.TouchLeft:
                    DoMouseDown(MouseButtons.Left);
                    break;

                case ControllerButton.StickRight:
                case ControllerButton.TouchRight:
                    DoMouseDown(MouseButtons.Right);
                    break;

                default:
                    ProcessWatcher.Interaction.SendKeyDown(BindingManager.CurrentBinds[Button]);
                    break;
            }

            Console.WriteLine("KeyDown " + Button);

            keyStates[(int)Button] = true;
        }

        private static void ProcessKeyUp(ControllerButton Button)
        {
            // Don't process if key isn't held
            if (!keyStates[(int)Button] || BindingManager.CurrentBinds == null) return;

            // TODO send key logic
            switch (Button)
            {
                case ControllerButton.StickLeft:
                    DoMouseUp(MouseButtons.Left);
                    break;

                case ControllerButton.StickRight:
                    DoMouseUp(MouseButtons.Right);
                    break;

                default:
                    ProcessWatcher.Interaction.SendKeyUp(BindingManager.CurrentBinds[Button]);
                    break;
            }
            Console.WriteLine("KeyUp " + Button);

            keyStates[(int)Button] = false;
        }

        private static void DoMouseDown(MouseButtons buttons)
        {
            if (buttons.HasFlag(MouseButtons.Left))
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, (uint) Cursor.Position.X, (uint) Cursor.Position.Y, 0, 0);
                return;
            }


            if (buttons.HasFlag(MouseButtons.Right))
            { 
                mouse_event(MOUSEEVENTF_RIGHTDOWN, (uint) Cursor.Position.X, (uint) Cursor.Position.Y, 0, 0);
                return;
            }
        }

        private static void DoMouseUp(MouseButtons buttons)
        {
            if (buttons.HasFlag(MouseButtons.Left))
            {
                mouse_event(MOUSEEVENTF_LEFTUP, (uint) Cursor.Position.X, (uint) Cursor.Position.Y, 0, 0);
                return;
            }

            if (buttons.HasFlag(MouseButtons.Right))
            {
                mouse_event(MOUSEEVENTF_RIGHTUP, (uint) Cursor.Position.X, (uint) Cursor.Position.Y, 0, 0);
                return;
            }
        }
    }
}