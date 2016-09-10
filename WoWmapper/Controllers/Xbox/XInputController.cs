using J2i.Net.XInputWrapper;
using System;
using System.Drawing.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WoWmapper.Controllers.Xbox
{
    internal class XInputController : IController
    {
        private readonly int _deviceIndex;
        private readonly bool[] _buttonStates = new bool[Enum.GetNames(typeof (GamepadButton)).Length];
        private bool _running;

        public GamepadType Type => GamepadType.Xbox;
        public object UnderlyingController => _controller as XboxController;
        public string Name => "XInput " + _deviceIndex;

        public byte BatteryLevel
        {
            get
            {
                if (_controller != null)
                    return (byte)(((double)_controller.BatteryInformationGamepad.BatteryLevel / 3) * 100);
                return 0;
            }
        }

        public event ButtonStateChangedHandler ButtonStateChanged;
        private XboxController _controller;
        private readonly Thread _monitorThread;

        public XInputController(int deviceIndex)
        {
            _deviceIndex = deviceIndex;
            _controller = XboxController.RetrieveController(deviceIndex);
            _running = true;
            _monitorThread = new Thread(MonitorThread);
            _monitorThread.Start();
        }

        private void MonitorThread()
        {
            while (_running)
            {
                Thread.Sleep(1);

                var xbox = UnderlyingController as XboxController;

                if (xbox == null) continue;

                if (xbox.IsYPressed && !_buttonStates[(int) GamepadButton.RFaceUp])
                    SendButtonEvent(GamepadButton.RFaceUp, true);
                if (!(xbox.IsYPressed) && _buttonStates[(int) GamepadButton.RFaceUp])
                    SendButtonEvent(GamepadButton.RFaceUp, false);

                if (xbox.IsAPressed && !_buttonStates[(int) GamepadButton.RFaceDown])
                    SendButtonEvent(GamepadButton.RFaceDown, true);
                if (!xbox.IsAPressed && _buttonStates[(int) GamepadButton.RFaceDown])
                    SendButtonEvent(GamepadButton.RFaceDown, false);

                if (xbox.IsXPressed && !_buttonStates[(int) GamepadButton.RFaceLeft])
                    SendButtonEvent(GamepadButton.RFaceLeft, true);
                if (!xbox.IsXPressed && _buttonStates[(int) GamepadButton.RFaceLeft])
                    SendButtonEvent(GamepadButton.RFaceLeft, false);

                if (xbox.IsBPressed && !_buttonStates[(int) GamepadButton.RFaceRight])
                    SendButtonEvent(GamepadButton.RFaceRight, true);
                if (!xbox.IsBPressed && _buttonStates[(int) GamepadButton.RFaceRight])
                    SendButtonEvent(GamepadButton.RFaceRight, false);

                if (xbox.IsDPadUpPressed && !_buttonStates[(int) GamepadButton.LFaceUp])
                    SendButtonEvent(GamepadButton.LFaceUp, true);
                if (!xbox.IsDPadUpPressed && _buttonStates[(int) GamepadButton.LFaceUp])
                    SendButtonEvent(GamepadButton.LFaceUp, false);

                if (xbox.IsDPadDownPressed && !_buttonStates[(int) GamepadButton.LFaceDown])
                    SendButtonEvent(GamepadButton.LFaceDown, true);
                if (!xbox.IsDPadDownPressed && _buttonStates[(int) GamepadButton.LFaceDown])
                    SendButtonEvent(GamepadButton.LFaceDown, false);

                if (xbox.IsDPadLeftPressed && !_buttonStates[(int) GamepadButton.LFaceLeft])
                    SendButtonEvent(GamepadButton.LFaceLeft, true);
                if (!xbox.IsDPadLeftPressed && _buttonStates[(int) GamepadButton.LFaceLeft])
                    SendButtonEvent(GamepadButton.LFaceLeft, false);

                if (xbox.IsDPadRightPressed && !_buttonStates[(int) GamepadButton.LFaceRight])
                    SendButtonEvent(GamepadButton.LFaceRight, true);
                if (!xbox.IsDPadRightPressed && _buttonStates[(int) GamepadButton.LFaceRight])
                    SendButtonEvent(GamepadButton.LFaceRight, false);

                if (xbox.IsBackPressed && !_buttonStates[(int) GamepadButton.CenterLeft])
                    SendButtonEvent(GamepadButton.CenterLeft, true);
                if (!xbox.IsBackPressed && _buttonStates[(int) GamepadButton.CenterLeft])
                    SendButtonEvent(GamepadButton.CenterLeft, false);

                if (xbox.IsStartPressed && !_buttonStates[(int) GamepadButton.CenterRight])
                    SendButtonEvent(GamepadButton.CenterRight, true);
                if (!xbox.IsStartPressed && _buttonStates[(int) GamepadButton.CenterRight])
                    SendButtonEvent(GamepadButton.CenterRight, false);

                if (xbox.IsGuidePressed && !_buttonStates[(int) GamepadButton.CenterMiddle])
                    SendButtonEvent(GamepadButton.CenterMiddle, true);
                if (!xbox.IsGuidePressed && _buttonStates[(int) GamepadButton.CenterMiddle])
                    SendButtonEvent(GamepadButton.CenterMiddle, false);

                if (xbox.IsLeftShoulderPressed && !_buttonStates[(int) GamepadButton.ShoulderLeft])
                    SendButtonEvent(GamepadButton.ShoulderLeft, true);
                if (!xbox.IsLeftShoulderPressed && _buttonStates[(int) GamepadButton.ShoulderLeft])
                    SendButtonEvent(GamepadButton.ShoulderLeft, false);

                if (xbox.IsRightShoulderPressed && !_buttonStates[(int) GamepadButton.ShoulderRight])
                    SendButtonEvent(GamepadButton.ShoulderRight, true);
                if (!xbox.IsRightShoulderPressed && _buttonStates[(int) GamepadButton.ShoulderRight])
                    SendButtonEvent(GamepadButton.ShoulderRight, false);

                if ((xbox.LeftTrigger > Properties.Settings.Default.TriggerThresholdLeft) &&
                    !_buttonStates[(int) GamepadButton.TriggerLeft])
                    SendButtonEvent(GamepadButton.TriggerLeft, true);
                if (!(xbox.LeftTrigger > Properties.Settings.Default.TriggerThresholdLeft) &&
                    _buttonStates[(int) GamepadButton.TriggerLeft])
                    SendButtonEvent(GamepadButton.TriggerLeft, false);

                if (xbox.RightTrigger > Properties.Settings.Default.TriggerThresholdRight &&
                    !_buttonStates[(int) GamepadButton.TriggerRight])
                    SendButtonEvent(GamepadButton.TriggerRight, true);
                if (!(xbox.RightTrigger > Properties.Settings.Default.TriggerThresholdRight) &&
                    _buttonStates[(int) GamepadButton.TriggerRight])
                    SendButtonEvent(GamepadButton.TriggerRight, false);

                if (xbox.IsLeftStickPressed && !_buttonStates[(int) GamepadButton.LeftStick])
                    SendButtonEvent(GamepadButton.LeftStick, true);
                if (!xbox.IsLeftStickPressed && _buttonStates[(int) GamepadButton.LeftStick])
                    SendButtonEvent(GamepadButton.LeftStick, false);

                if (xbox.IsRightStickPressed && !_buttonStates[(int) GamepadButton.RightStick])
                    SendButtonEvent(GamepadButton.RightStick, true);
                if (!xbox.IsRightStickPressed && _buttonStates[(int) GamepadButton.RightStick])
                    SendButtonEvent(GamepadButton.RightStick, false);
            }
        }

        private void SendButtonEvent(GamepadButton button, bool state)
        {
            _buttonStates[(int) button] = state;
            ButtonStateChanged?.Invoke(button, state);
        }

        public bool GetButtonState(GamepadButton button)
        {
            return _buttonStates[(int) button];
        }

        public int GetAxis(GamepadAxis axis)
        {
            switch (axis)
            {
                case GamepadAxis.StickLeftX:
                    return _controller.LeftThumbStick.X/256;
                case GamepadAxis.StickLeftY:
                    return -_controller.LeftThumbStick.Y/256;
                case GamepadAxis.StickRightX:
                    return _controller.RightThumbStick.X/256;
                case GamepadAxis.StickRightY:
                    return -_controller.RightThumbStick.Y/256;
                case GamepadAxis.TriggerLeft:
                    return _controller.LeftTrigger;
                case GamepadAxis.TriggerRight:
                    return _controller.RightTrigger;
            }
            return 0;
        }

        public void SendRumble(byte left, byte right, int duration)
        {
            _controller.Vibrate((double)left / 255, (double)right/255, TimeSpan.FromMilliseconds(duration));
        }

        public void Stop()
        {
            _running = false;
        }

        public bool IsAlive()
        {
            return _controller != null && _controller.IsConnected;
        }
    }
}