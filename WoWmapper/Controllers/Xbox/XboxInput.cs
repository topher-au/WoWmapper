using J2i.Net.XInputWrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WoWmapper.Controllers.Xbox
{
    internal class XboxInput : IController
    {
        private int _user;
        private XboxController _controller;

        public XboxInput(int UserIndex)
        {
            _user = UserIndex;
            _controller = XboxController.RetrieveController(_user);
        }

        public int Battery => _controller == null ? 0 : _controller.BatteryInformationGamepad.BatteryLevel;

        public string ControllerID => string.Format($"XInput:{_user + 1}");

        public ControllerPeripherals Peripherals => new ControllerPeripherals()
        {
            Touchpads = 0,
            Gyro = false,
            Lightbar = false,
            Vibration = true,
            Chatpad = false
        };

        public ControllerType Type => ControllerType.Xbox;

        public ControllerBatteryState BatteryState => ControllerBatteryState.Charging;

        public ControllerConnectionType ConnectionType => ControllerConnectionType.Wireless;

        public object UnderlyingDevice
        {
            get { return _controller; }
        }

        public ControllerState GetControllerState()
        {
            if(_controller != null)
                lock (_controller)
                {
                    try
                    {
                        var state = new ControllerState()
                        {
                            LFaceUp = _controller.IsDPadUpPressed,
                            LFaceRight = _controller.IsDPadRightPressed,
                            LFaceDown = _controller.IsDPadDownPressed,
                            LFaceLeft = _controller.IsDPadLeftPressed,
                            RFaceUp = _controller.IsYPressed,
                            RFaceRight = _controller.IsBPressed,
                            RFaceDown = _controller.IsAPressed,
                            RFaceLeft = _controller.IsXPressed,
                            CenterLeft = _controller.IsBackPressed,
                            CenterRight = _controller.IsStartPressed,
                            ShoulderLeft = _controller.IsLeftShoulderPressed,
                            ShoulderRight = _controller.IsRightShoulderPressed,
                            TriggerLeft = _controller.LeftTrigger,
                            TriggerRight = _controller.RightTrigger,
                            CenterMiddle = _controller.IsGuidePressed,
                            LeftX = _controller.LeftThumbStick.X/256,
                            LeftY = -_controller.LeftThumbStick.Y/256,
                            RightX = _controller.RightThumbStick.X/256,
                            RightY = -_controller.RightThumbStick.Y/256,
                            StickLeft = _controller.IsLeftStickPressed,
                            StickRight=_controller.IsRightStickPressed
                        };
                        return state;
                    }
                    catch
                    {                 }
                    
                }
            
            return new ControllerState();
        }

        public ControllerTouchpadState GetTouchpadState()
        {
            return null;
        }

        public void SendRumble(byte Left, byte Right, int Duration)
        {
            
            _controller?.Vibrate(new XInputVibration() { LeftMotorSpeed=Left, RightMotorSpeed=Right}, TimeSpan.FromMilliseconds(Duration));
        }

        public void SetLightbar(byte r, byte g, byte b)
        {
            return;
        }

        public void SetLightbar(byte r, byte g, byte b, int flashOn, int flashOff)
        {
            return;
        }

        public void Stop()
        {
            _controller = null;
            _user = 0;
        }

        public bool IsAlive()
        {
            return _controller != null && _controller.IsConnected;
        }
    }
}