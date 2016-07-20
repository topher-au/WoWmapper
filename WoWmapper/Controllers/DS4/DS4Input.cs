using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WoWmapper.Controllers;
using DS4Windows;

namespace WoWmapper.Controllers.DS4
{
    class DS4Input : IController
    {
        private DS4Device _controller;
        private int _touchX;
        private int _touchY;
        private bool wasTouching1;

        public DS4Input(string MAC)
        {
            var control = DS4Devices.getDS4Controller(MAC);
            if (control == null) return;
            _controller = control;
        }

        public int Battery
        {
            get
            {
                if (_controller == null) return 0;
                return _controller?.Battery > 100 ? 100 : _controller.Battery;
            }
        }

        public ControllerBatteryState BatteryState
        {
            get
            {
                if (_controller != null)
                {
                    return _controller.Charging ? ControllerBatteryState.Charging : ControllerBatteryState.Discharging;
                }
                return ControllerBatteryState.None;
            }
        }

        public ControllerConnectionType ConnectionType
        {
            get
            {
                if(_controller != null)
                {
                    return _controller.ConnectionType == DS4Windows.ConnectionType.BT ? ControllerConnectionType.Bluetooth : ControllerConnectionType.USB;
                }
                return ControllerConnectionType.None;
            }
        }

        public object UnderlyingDevice => _controller?.HidDevice;
        public string ControllerID => _controller?.MacAddress;

        public ControllerPeripherals Peripherals => new ControllerPeripherals()
        {
            Chatpad = false,
            Gyro = true,
            Touchpads = 1,
            Lightbar = true,
            Vibration = true
        };

        public ControllerType Type => ControllerType.DualShock4;

        public ControllerState GetControllerState()
        {
            if (_controller == null)
                return new ControllerState();

            DS4State cState, pState;

            try
            {
                cState = _controller.getCurrentState();
                pState = _controller.getPreviousState();
            }
            catch
            {
                return new ControllerState();
            }

            var outState = new ControllerState();

            if (cState.L1 && pState.L1) outState.ShoulderLeft = true;
            if (cState.R1 && pState.R1) outState.ShoulderRight = true;
            if (cState.L3 && pState.L3) outState.StickLeft = true;
            if (cState.R3 && pState.R3) outState.StickRight = true;
            if (cState.DpadUp && pState.DpadUp) outState.LFaceUp = true;
            if (cState.DpadRight && pState.DpadRight) outState.LFaceRight = true;
            if (cState.DpadDown && pState.DpadDown) outState.LFaceDown = true;
            if (cState.DpadLeft && pState.DpadLeft) outState.LFaceLeft = true;
            if (cState.Triangle && pState.Triangle) outState.RFaceUp = true;
            if (cState.Circle && pState.Circle) outState.RFaceRight = true;
            if (cState.Cross && pState.Cross) outState.RFaceDown = true;
            if (cState.Square && pState.Square) outState.RFaceLeft = true;
            if (cState.Share && pState.Share) outState.CenterLeft = true;
            if (cState.Options && pState.Options) outState.CenterRight = true;
            if (cState.PS && pState.PS) outState.CenterMiddle = true;
            if (cState.TouchLeft && pState.TouchLeft) outState.TouchLeft = true;
            if (cState.TouchRight && pState.TouchRight) outState.TouchRight = true;
            if (cState.TouchButton) outState.TouchButton = true;
            // TODO: TouchUpper

            outState.TriggerLeft = cState.L2;
            outState.TriggerRight = cState.R2;

            outState.LeftX = cState.LX-127;
            outState.LeftY = cState.LY-127;
            outState.RightX = cState.RX-127;
            outState.RightY = cState.RY-127;

            return outState;
        }

        public ControllerTouchpadState GetTouchpadState()
        {
            if(_controller == null) return null;

            var ds4Touch = _controller.Touchpad;
            var tX = ds4Touch.lastTouchPadX1;
            var tY = ds4Touch.lastTouchPadY1;

            if (ds4Touch.lastIsActive1 && !wasTouching1)
            {
                _touchX = tX;
                _touchY = tY;
            }

            var outState = new ControllerTouchpadState
            {
                TouchX = tX,
                TouchY = tY,
                DeltaX = tX - _touchX,
                DeltaY = tY - _touchY,
                TouchDown = ds4Touch.lastTouchPadIsDown
            };

            _touchX = tX;
            _touchY = tY;

            wasTouching1 = ds4Touch.lastIsActive1;

            return outState;
        }

        public bool IsAlive()
        {
            if (_controller == null) return false;
            try
            {
                return _controller.HidDevice.IsConnected && _controller.HidDevice.IsOpen;
            }
            catch
            {
                return false;
            }
            
        }

        public void SendRumble(byte Left, byte Right, int Duration)
        {
            Task.Run(new Action(() =>
            {
                _controller?.setRumble(Right, Left);
                Thread.Sleep(Duration);
                _controller?.setRumble(0, 0);
            }));
            
        }

        public void SetLightbar(byte r, byte g, byte b)
        {
            if (_controller == null) return;

            if (!_controller.LightBarColor.Equals(new DS4Color(r, g, b)))
            {
                lock (_controller)
                {
                    try
                    {
                        _controller.LightBarOffDuration = 0;
                        _controller.LightBarOnDuration = 255;
                        Thread.Sleep(5);
                        _controller.LightBarColor = new DS4Color(r, g, b);
                    } catch { }
                    
                }
            }
        }

        public void SetLightbar(byte r, byte g, byte b, int flashOn, int flashOff)
        {
            if (_controller == null ||
                _controller.LightBarColor.Equals(new DS4Color(r, g, b))) return;
            
            var on = (byte)(flashOn / 10);
            var off = (byte)(flashOff / 10);

            lock (_controller)
            {
                _controller.LightBarOffDuration = off;
                _controller.LightBarOnDuration = on;
                Thread.Sleep(5);
                _controller.LightBarColor = new DS4Color(r, g, b);
            }
        }

        public void Stop()
        {
            Console.WriteLine("DS4 disconnecting");
            if (_controller == null) return;
            _controller.LightBarColor=new DS4Color(0,0,0);
            _controller.LightBarOnDuration = 0;
            _controller.LightBarOffDuration = 0;
            _controller.FlushHID();
            if (_controller.ConnectionType == DS4Windows.ConnectionType.BT)
                _controller.DisconnectBT();
            _controller = null;
        }
    }
}
