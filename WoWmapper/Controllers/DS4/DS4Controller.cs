using DS4Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace WoWmapper.Controllers.DS4
{
    public class DS4Controller : IController
    {
        #region Private Fields



        private bool[] _buttonStates = new bool[Enum.GetNames(typeof (GamepadButton)).Length];

        private Thread _monitorThread;

        #endregion Private Fields

        #region Public Constructors

        public DS4Controller(DS4Device underlyingDevice)
        {
            UnderlyingController = underlyingDevice;
            _running = true;
            _monitorThread = new Thread(MonitorThread);
            _monitorThread.Start();
        }

        #endregion Public Constructors

        #region Public Events

        public event ButtonStateChangedHandler ButtonStateChanged;

        #endregion Public Events

        #region Public Properties

        public byte BatteryLevel { get; internal set; }

        public string Name => (UnderlyingController as DS4Device).MacAddress;

        public GamepadType Type => GamepadType.PlayStation;

        public object UnderlyingController { get; internal set; }

        #endregion Public Properties

        #region Private Properties

        private DS4Device Ds4 => UnderlyingController as DS4Device;
        private bool _running;

        #endregion Private Properties

        #region Public Methods

        public int GetAxis(GamepadAxis axis)
        {
            if (Ds4 == null || !IsAlive()) return 0;

            var cState = Ds4.getCurrentState();

            switch (axis)
            {
                case GamepadAxis.StickLeftX:
                    return cState.LX - 127;
                case GamepadAxis.StickLeftY:
                    return cState.LY - 127;
                case GamepadAxis.StickRightX:
                    return cState.RX - 127;
                case GamepadAxis.StickRightY:
                    return cState.RY - 127;
                case GamepadAxis.TriggerLeft:
                    return cState.L2;
                case GamepadAxis.TriggerRight:
                    return cState.R2;
                default:
                    return 0;
            }
        }

        public bool GetButtonState(GamepadButton button)
        {
            return _buttonStates[(int) button];
        }

        public bool IsAlive()
        {
            return Ds4.IsAlive() && !Ds4.IsDisconnecting && _running;
        }

        public void SendRumble(byte left, byte right, int duration)
        {
            Task.Run(() =>
            {
                Ds4.setRumble(left, right);
                Thread.Sleep(duration);
                Ds4.setRumble(0, 0);
            });
        }

        public void Stop()
        {
            Ds4.LeftHeavySlowRumble = 0;
            Ds4.RightLightFastRumble = 0;
            _running = false;
            
        }

        #endregion Public Methods

        #region Private Methods

        private void MonitorThread()
        {
            while (_running)
            {
                var ds4 = UnderlyingController as DS4Device;
                if (ds4 == null) continue;
                BatteryLevel = (byte) (ds4.Battery > 100 ? 100 : ds4.Battery);

                var cState = ds4.getCurrentState();
                var pState = ds4.getPreviousState();

                if (cState.Triangle & pState.Triangle && !_buttonStates[(int) GamepadButton.RFaceUp])
                    SendButtonEvent(GamepadButton.RFaceUp, true);
                if (!(cState.Triangle & pState.Triangle) && _buttonStates[(int) GamepadButton.RFaceUp])
                    SendButtonEvent(GamepadButton.RFaceUp, false);

                if (cState.Cross & pState.Cross && !_buttonStates[(int) GamepadButton.RFaceDown])
                    SendButtonEvent(GamepadButton.RFaceDown, true);
                if (!(cState.Cross & pState.Cross) && _buttonStates[(int) GamepadButton.RFaceDown])
                    SendButtonEvent(GamepadButton.RFaceDown, false);

                if (cState.Square & pState.Square && !_buttonStates[(int) GamepadButton.RFaceLeft])
                    SendButtonEvent(GamepadButton.RFaceLeft, true);
                if (!(cState.Square & pState.Square) && _buttonStates[(int) GamepadButton.RFaceLeft])
                    SendButtonEvent(GamepadButton.RFaceLeft, false);

                if (cState.Circle & pState.Circle && !_buttonStates[(int) GamepadButton.RFaceRight])
                    SendButtonEvent(GamepadButton.RFaceRight, true);
                if (!(cState.Circle & pState.Circle) && _buttonStates[(int) GamepadButton.RFaceRight])
                    SendButtonEvent(GamepadButton.RFaceRight, false);

                if (cState.DpadUp & pState.DpadUp && !_buttonStates[(int) GamepadButton.LFaceUp])
                    SendButtonEvent(GamepadButton.LFaceUp, true);
                if (!(cState.DpadUp & pState.DpadUp) && _buttonStates[(int) GamepadButton.LFaceUp])
                    SendButtonEvent(GamepadButton.LFaceUp, false);

                if (cState.DpadDown & pState.DpadDown && !_buttonStates[(int) GamepadButton.LFaceDown])
                    SendButtonEvent(GamepadButton.LFaceDown, true);
                if (!(cState.DpadDown & pState.DpadDown) && _buttonStates[(int) GamepadButton.LFaceDown])
                    SendButtonEvent(GamepadButton.LFaceDown, false);

                if (cState.DpadLeft & pState.DpadLeft && !_buttonStates[(int) GamepadButton.LFaceLeft])
                    SendButtonEvent(GamepadButton.LFaceLeft, true);
                if (!(cState.DpadLeft & pState.DpadLeft) && _buttonStates[(int) GamepadButton.LFaceLeft])
                    SendButtonEvent(GamepadButton.LFaceLeft, false);

                if (cState.DpadRight & pState.DpadRight && !_buttonStates[(int) GamepadButton.LFaceRight])
                    SendButtonEvent(GamepadButton.LFaceRight, true);
                if (!(cState.DpadRight & pState.DpadRight) && _buttonStates[(int) GamepadButton.LFaceRight])
                    SendButtonEvent(GamepadButton.LFaceRight, false);

                if (cState.Share & pState.Share && !_buttonStates[(int) GamepadButton.CenterLeft])
                    SendButtonEvent(GamepadButton.CenterLeft, true);
                if (!(cState.Share & pState.Share) && _buttonStates[(int) GamepadButton.CenterLeft])
                    SendButtonEvent(GamepadButton.CenterLeft, false);

                if (cState.Options & pState.Options && !_buttonStates[(int) GamepadButton.CenterRight])
                    SendButtonEvent(GamepadButton.CenterRight, true);
                if (!(cState.Options & pState.Options) && _buttonStates[(int) GamepadButton.CenterRight])
                    SendButtonEvent(GamepadButton.CenterRight, false);

                if (cState.PS & pState.PS && !_buttonStates[(int) GamepadButton.CenterMiddle])
                    SendButtonEvent(GamepadButton.CenterMiddle, true);
                if (!(cState.PS & pState.PS) && _buttonStates[(int) GamepadButton.CenterMiddle])
                    SendButtonEvent(GamepadButton.CenterMiddle, false);

                if (cState.L1 & pState.L1 && !_buttonStates[(int) GamepadButton.ShoulderLeft])
                    SendButtonEvent(GamepadButton.ShoulderLeft, true);
                if (!(cState.L1 & pState.L1) && _buttonStates[(int) GamepadButton.ShoulderLeft])
                    SendButtonEvent(GamepadButton.ShoulderLeft, false);

                if (cState.R1 & pState.R1 && !_buttonStates[(int) GamepadButton.ShoulderRight])
                    SendButtonEvent(GamepadButton.ShoulderRight, true);
                if (!(cState.R1 & pState.R1) && _buttonStates[(int) GamepadButton.ShoulderRight])
                    SendButtonEvent(GamepadButton.ShoulderRight, false);

                if ((cState.L2 > Properties.Settings.Default.TriggerThresholdLeft) &&
                    !_buttonStates[(int) GamepadButton.TriggerLeft])
                    SendButtonEvent(GamepadButton.TriggerLeft, true);
                if (!(cState.L2 > Properties.Settings.Default.TriggerThresholdLeft) &&
                    _buttonStates[(int) GamepadButton.TriggerLeft])
                    SendButtonEvent(GamepadButton.TriggerLeft, false);

                if (cState.R2 > Properties.Settings.Default.TriggerThresholdRight &&
                    !_buttonStates[(int) GamepadButton.TriggerRight])
                    SendButtonEvent(GamepadButton.TriggerRight, true);
                if (!(cState.R2 > Properties.Settings.Default.TriggerThresholdRight) &&
                    _buttonStates[(int) GamepadButton.TriggerRight])
                    SendButtonEvent(GamepadButton.TriggerRight, false);

                if (cState.L3 & pState.L3 && !_buttonStates[(int) GamepadButton.LeftStick])
                    SendButtonEvent(GamepadButton.LeftStick, true);
                if (!(cState.L3 & pState.L3) && _buttonStates[(int) GamepadButton.LeftStick])
                    SendButtonEvent(GamepadButton.LeftStick, false);

                if (cState.R3 & pState.R3 && !_buttonStates[(int) GamepadButton.RightStick])
                    SendButtonEvent(GamepadButton.RightStick, true);
                if (!(cState.R3 & pState.R3) && _buttonStates[(int) GamepadButton.RightStick])
                    SendButtonEvent(GamepadButton.RightStick, false);

                Thread.Sleep(1);
            }
            
        }

        private void SendButtonEvent(GamepadButton button, bool state)
        {
            _buttonStates[(int) button] = state;
            ButtonStateChanged?.Invoke(button, state);
        }

        #endregion Private Methods
    }
}