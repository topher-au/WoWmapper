using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WoWmapper;
using WoWmapper.Controllers.Xbox;

namespace J2i.Net.XInputWrapper
{
    public class XboxController
    {
        int _playerIndex;
        static bool keepRunning;
        static int updateFrequency;
        static int waitTime;
        static bool isRunning;
        static object SyncLock;
        static Thread pollingThread;
        private static IXInput _xInput;
        private static bool _isXinput9;
        public static bool IsXInput9 => _isXinput9;
        bool _stopMotorTimerActive;
        DateTime _stopMotorTime;
        XInputBatteryInformation _batteryInformationGamepad;
        XInputBatteryInformation _batterInformationHeadset;
        XInputCapabilities _capabilities;

        XInputState gamepadStatePrev = new XInputState();
        XInputState gamepadStateCurrent = new XInputState();

        public static int UpdateFrequency
        {
            get { return updateFrequency; }
            set
            {
                updateFrequency = value;
                waitTime = 1000/updateFrequency;
            }
        }

        public XInputBatteryInformation BatteryInformationGamepad
        {
            get { return _batteryInformationGamepad; }
            internal set { _batteryInformationGamepad = value; }
        }

        public XInputBatteryInformation BatteryInformationHeadset
        {
            get { return _batterInformationHeadset; }
            internal set { _batterInformationHeadset = value; }
        }

        public const int MAX_CONTROLLER_COUNT = 4;
        public const int FIRST_CONTROLLER_INDEX = 0;
        public const int LAST_CONTROLLER_INDEX = MAX_CONTROLLER_COUNT - 1;

        static XboxController[] Controllers;



        static XboxController()
        {
            _xInput=new XInputUniversal();
            Controllers = new XboxController[MAX_CONTROLLER_COUNT];
            SyncLock = new object();
            for (int i = FIRST_CONTROLLER_INDEX; i <= LAST_CONTROLLER_INDEX; ++i)
            {
                Controllers[i] = new XboxController(i);
            }
            UpdateFrequency = 25;
        }

        public event EventHandler<XboxControllerStateChangedEventArgs> StateChanged = null;

        public static XboxController RetrieveController(int index)
        {
            return Controllers[index];
        }

        private XboxController(int playerIndex)
        {
            _playerIndex = playerIndex;
            gamepadStatePrev.Copy(gamepadStateCurrent);
        }

        public void UpdateBatteryState()
        {
            var gamepad = new XInputBatteryInformation();

            _xInput.GetBatteryInformation(_playerIndex, (byte) BatteryDeviceType.BATTERY_DEVTYPE_GAMEPAD, ref gamepad);

            BatteryInformationGamepad = gamepad;
        }

        protected void OnStateChanged()
        {
            if (StateChanged != null)
                StateChanged(this,
                    new XboxControllerStateChangedEventArgs()
                    {
                        CurrentInputState = gamepadStateCurrent,
                        PreviousInputState = gamepadStatePrev
                    });
        }

        public XInputCapabilities GetCapabilities()
        {
            XInputCapabilities capabilities = new XInputCapabilities();
            _xInput.GetCapabilities(_playerIndex, XInputConstants.XINPUT_FLAG_GAMEPAD, ref capabilities);
            return capabilities;
        }

        #region Digital Button States

        public bool IsDPadUpPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_DPAD_UP); }
        }

        public bool IsDPadDownPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_DPAD_DOWN); }
        }

        public bool IsDPadLeftPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_DPAD_LEFT); }
        }

        public bool IsDPadRightPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_DPAD_RIGHT); }
        }

        public bool IsAPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_A); }
        }

        public bool IsBPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_B); }
        }

        public bool IsXPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_X); }
        }

        public bool IsYPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_Y); }
        }


        public bool IsBackPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_BACK); }
        }


        public bool IsStartPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_START); }
        }


        public bool IsLeftShoulderPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_LEFT_SHOULDER); }
        }


        public bool IsRightShoulderPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_RIGHT_SHOULDER); }
        }

        public bool IsLeftStickPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_LEFT_THUMB); }
        }

        public bool IsRightStickPressed
        {
            get { return gamepadStateCurrent.Gamepad.IsButtonPressed((int) ButtonFlags.XINPUT_GAMEPAD_RIGHT_THUMB); }
        }

        public bool IsGuidePressed

        {
            get
            {
                var stateSecret = new XInputStateSecret();
                _xInput.GetStateSecret(_playerIndex, out stateSecret);
                return (stateSecret.wButtons & 0x0400) == 0x0400;
            }
        }

        #endregion

        #region Analogue Input States

        public int LeftTrigger
        {
            get { return (int) gamepadStateCurrent.Gamepad.bLeftTrigger; }
        }

        public int RightTrigger
        {
            get { return (int) gamepadStateCurrent.Gamepad.bRightTrigger; }
        }

        public Point LeftThumbStick
        {
            get
            {
                Point p = new Point()
                {
                    X = gamepadStateCurrent.Gamepad.sThumbLX,
                    Y = gamepadStateCurrent.Gamepad.sThumbLY
                };
                return p;
            }
        }

        public Point RightThumbStick
        {
            get
            {
                Point p = new Point()
                {
                    X = gamepadStateCurrent.Gamepad.sThumbRX,
                    Y = gamepadStateCurrent.Gamepad.sThumbRY
                };
                return p;
            }
        }

        #endregion

        bool _isConnected;

        public bool IsConnected
        {
            get { return _isConnected; }
            internal set { _isConnected = value; }
        }

        #region Polling

        public static void StartPolling()
        {
            if (!isRunning)
            {
                lock (SyncLock)
                {
                    if (!isRunning)
                    {
                        pollingThread = new Thread(PollerLoop);
                        pollingThread.Start();
                    }
                }
            }
        }

        public static void StopPolling()
        {
            if (isRunning)
                keepRunning = false;
        }

        static void PollerLoop()
        {
            lock (SyncLock)
            {
                if (isRunning == true)
                    return;
                isRunning = true;
            }
            keepRunning = true;
            while (keepRunning)
            {
                for (int i = FIRST_CONTROLLER_INDEX; i <= LAST_CONTROLLER_INDEX; ++i)
                {
                    Controllers[i].UpdateState();
                }
                Thread.Sleep(updateFrequency);
            }
            lock (SyncLock)
            {
                isRunning = false;
            }
        }

        public void UpdateState()
        {
            int result = _xInput.GetState(_playerIndex, ref gamepadStateCurrent);
            IsConnected = (result == 0);

            UpdateBatteryState();
            if (gamepadStateCurrent.PacketNumber != gamepadStatePrev.PacketNumber)
            {
                OnStateChanged();
            }
            gamepadStatePrev.Copy(gamepadStateCurrent);

            if (_stopMotorTimerActive && (DateTime.Now >= _stopMotorTime))
            {
                XInputVibration stopStrength = new XInputVibration() {LeftMotorSpeed = 0, RightMotorSpeed = 0};
                _xInput.SetState(_playerIndex, ref stopStrength);
            }
        }

        #endregion

        #region Motor Functions

        public void Vibrate(double leftMotor, double rightMotor)
        {
            Vibrate(leftMotor, rightMotor, TimeSpan.MinValue);
        }

        public void Vibrate(double leftMotor, double rightMotor, TimeSpan length)
        {
            leftMotor = Math.Max(0d, Math.Min(1d, leftMotor));
            rightMotor = Math.Max(0d, Math.Min(1d, rightMotor));

            XInputVibration vibration = new XInputVibration()
            {
                LeftMotorSpeed = (ushort) (65535d*leftMotor),
                RightMotorSpeed = (ushort) (65535d*rightMotor)
            };
            Vibrate(vibration, length);
        }


        public void Vibrate(XInputVibration strength)
        {
            _stopMotorTimerActive = false;
            _xInput.SetState(_playerIndex, ref strength);
        }

        public void Vibrate(XInputVibration strength, TimeSpan length)
        {
            _xInput.SetState(_playerIndex, ref strength);
            if (length != TimeSpan.MinValue)
            {
                _stopMotorTime = DateTime.Now.Add(length);
                _stopMotorTimerActive = true;
            }
        }

        #endregion

        public override string ToString()
        {
            return _playerIndex.ToString();
        }
    }
}