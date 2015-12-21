using SlimDX.XInput;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using WoWmapper.Input;
using WoWmapper_360Driver.Forms;

namespace input_360
{
    public class Xbox360Driver : IInputPlugin
    {
        public XINPUT_GAMEPAD_SECRET controllerState;
        private int activeControllerNumber;
        private bool[] buttonStates;
        private bool connected = false;
        private Controller controller;
        private SettingsFile settings = new SettingsFile("input_360");
        private Thread stateThread, buttonThread;
        private InputThresholds thresholds = new InputThresholds() { TriggerLeft = 40, TriggerRight = 40 };
        public Xbox360Driver()
        {
        }

        public Xbox360Driver(bool Initialize)
        {
            if (Initialize)
            {
                InitializeSettings();
                buttonStates = new bool[Enum.GetNames(typeof(XboxButtons)).Length];
                OnButtonDown += Xbox360Driver_OnButtonDown;
                OnButtonUp += Xbox360Driver_OnButtonUp;
                OnControllerConnected += Xbox360Driver_OnControllerConnected;
                OnControllerDisconnected += Xbox360Driver_OnControllerDisconnected;
                OnTouchpadMoved += Xbox360Driver_OnTouchpadMoved;
                stateThread = new Thread(ControllerWatcherThread);
                stateThread.Start();
                buttonThread = new Thread(ButtonWatcherThread);
                buttonThread.Start();
            }
        }

        ~Xbox360Driver()
        {
            if (buttonThread != null) buttonThread.Abort();
            if (stateThread != null) stateThread.Abort();
        }

        public event ButtonDown OnButtonDown;

        public event ButtonUp OnButtonUp;

        public event ControllerConnected OnControllerConnected;

        public event ControllerDisconnected OnControllerDisconnected;

        public event TouchpadMoved OnTouchpadMoved;

        private enum XboxButtons
        {
            A,
            B,
            X,
            Y,
            DPadUp,
            DPadDown,
            DPadLeft,
            DPadRight,
            LeftShoulder,
            RightShoulder,
            TriggerLeft,
            TriggerRight,
            LeftThumb,
            RightThumb,
            Back,
            Start,
            Guide
        }

        public string Author
        {
            get
            {
                return "Topher Sheridan";
            }
        }

        public int Battery
        {
            get
            {
                if (controller != null && controller.IsConnected)
                {
                    var batteryInfo = controller.GetBatteryInformation(BatteryDeviceType.Gamepad);
                    switch (batteryInfo.Level)
                    {
                        case BatteryLevel.Full:
                            return 100;

                        case BatteryLevel.Medium:
                            return 50;

                        case BatteryLevel.Low:
                            return 25;

                        case BatteryLevel.Empty:
                            return 0;
                    }
                }
                return 0;
            }
        }

        public bool Connected
        {
            get
            {
                return connected;
            }
        }

        public InputConnectionType ConnectionType
        {
            get
            {
                return InputConnectionType.Wireless;
            }
        }

        public string ControllerName
        {
            get
            {
                return "Xbox Controller";
            }
        }

        public bool Enabled { get; set; } = false;

        public Keybind Keybinds
        {
            get
            {
                return settings.Keybinds;
            }

            set
            {
                settings.Keybinds = value;
                settings.Save();
            }
        }

        public string Name
        {
            get
            {
                return "Xbox Controller";
            }
        }

        public InputPeripherals Peripherals
        {
            get
            {
                return new InputPeripherals()
                {
                    LED = false,
                    Gyro = false,
                    Rumble = true,
                    Touchpad = false
                };
            }
        }

        public float RightCurve
        {
            get
            {
                float RightCurve;
                settings.Settings.Read("RightCurve", out RightCurve);
                return RightCurve;
            }

            set
            {
                settings.Settings.Write("RightCurve", value);
            }
        }

        public float RightDead
        {
            get
            {
                float RightDead;
                settings.Settings.Read("RightDead", out RightDead);
                return RightDead;
            }

            set
            {
                settings.Settings.Write("RightDead", value);
            }
        }

        public float RightSpeed
        {
            get
            {
                float RightSpeed;
                settings.Settings.Read("RightSpeed", out RightSpeed);
                return RightSpeed;
            }

            set
            {
                settings.Settings.Write("RightSpeed", value);
            }
        }

        public SettingsFile Settings
        {
            get
            {
                return settings;
            }

            set
            {
                settings = value;
                settings.Save();
            }
        }

        public InputThresholds Thresholds
        {
            get
            {
                return thresholds;
            }

            set
            {
                thresholds = value;
            }
        }

        public int touchMode
        {
            // Not supported
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Version Version
        {
            get
            {
                return new Version("0.1.0");
            }
        }

        public string Website
        {
            get
            {
                return "https://www.github.com/topher-au/WoWmapper";
            }
        }

        public int GetAxis(InputAxis Axis)
        {
            if (controller != null && controller.IsConnected)
            {
                var state = controller.GetState();
                switch (Axis)
                {
                    case InputAxis.LeftStickX:
                        return state.Gamepad.LeftThumbX / 256;

                    case InputAxis.LeftStickY:
                        return -state.Gamepad.LeftThumbY / 256;

                    case InputAxis.RightStickX:
                        return state.Gamepad.RightThumbX / 256;

                    case InputAxis.RightStickY:
                        return -state.Gamepad.RightThumbY / 256;

                    case InputAxis.LeftTrigger:
                        return state.Gamepad.LeftTrigger;

                    case InputAxis.RightTrigger:
                        return state.Gamepad.RightTrigger;
                }
            }
            return 0;
        }

        public bool GetButton(InputButton Button)
        {
            throw new NotImplementedException();
        }

        public InputState GetInputState()
        {
            throw new NotImplementedException();
        }

        public void InitializeSettings()
        {
            settings.Load();
            float rDead, rCurve, rSpeed;
            int tLeft, tRight, touchMode;

            // setup defaults
            if (!settings.Settings.Read("RightDead", out rDead))
            {
                settings.Settings.Write("RightDead", 15f);
            }
            if (!settings.Settings.Read("TriggerLeft", out tLeft))
            {
                settings.Settings.Write("TriggerLeft", 40);
            }
            if (!settings.Settings.Read("TriggerRight", out tRight))
            {
                settings.Settings.Write("TriggerRight", 40);
            }
            if (!settings.Settings.Read("RightCurve", out rCurve))
            {
                settings.Settings.Write("RightCurve", 5f);
            }
            if (!settings.Settings.Read("RightSpeed", out rSpeed))
            {
                settings.Settings.Write("RightSpeed", 5f);
            }
            if (!settings.Settings.Read("TouchMode", out touchMode))
            {
                settings.Settings.Write("TouchMode", 0);
            }

            settings.Version = "0.1.0";
            settings.Save();
        }

        public void Kill()
        {
            if (buttonThread != null) buttonThread.Abort();
            if (stateThread != null) stateThread.Abort();
        }

        public void SendRumble(InputRumbleMotor Motor, byte Strength, int Duration)
        {
            if (controller != null && controller.IsConnected)
            {
                var strengthVal = (ushort)(((float)Strength / 255f) * ushort.MaxValue);
                switch (Motor)
                {
                    case InputRumbleMotor.Left:
                        controller.SetVibration(new Vibration()
                        {
                            LeftMotorSpeed = strengthVal
                        });
                        break;

                    case InputRumbleMotor.Right:
                        controller.SetVibration(new Vibration()
                        {
                            RightMotorSpeed = strengthVal
                        });
                        break;

                    case InputRumbleMotor.Both:
                        controller.SetVibration(new Vibration()
                        {
                            LeftMotorSpeed = strengthVal,
                            RightMotorSpeed = strengthVal
                        });
                        break;
                }
                Thread.Sleep(Duration);
                controller.SetVibration(new Vibration() { RightMotorSpeed = 0, LeftMotorSpeed = 0 });
            }
        }

        public void ShowConfigDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowKeybindDialog(bool ShowInTaskbar = true)
        {
            KeybindForm kbf = new KeybindForm(controller, settings);

            kbf.ShowDialog();
        }

        public void StopRumble()
        {
            throw new NotImplementedException();
        }

        [DllImport("xinput1_3.dll", EntryPoint = "#100")]
        private static extern int secret_get_gamepad(int playerIndex, out XINPUT_GAMEPAD_SECRET struc);

        private void ButtonWatcherThread()
        {
            while (true)
            {
                if (controller != null && controller.IsConnected)
                {
                    // pull controller state using secret_get_gamepad -- needed for Guide button
                    var stat = secret_get_gamepad(activeControllerNumber, out controllerState);
                    var buttons = controllerState.wButtons;
                    if (GetFlag(buttons, (short)GamepadButtonFlags.A))

                        // check for and raise button press events
                        if (GetFlag(buttons, (short)GamepadButtonFlags.A) && !buttonStates[(int)XboxButtons.A])
                            DoButtonDown(InputButton.RFaceDown, XboxButtons.A);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.A) && buttonStates[(int)XboxButtons.A])
                        DoButtonUp(InputButton.RFaceDown, XboxButtons.A);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.B) && !buttonStates[(int)XboxButtons.B])
                        DoButtonDown(InputButton.RFaceRight, XboxButtons.B);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.B) && buttonStates[(int)XboxButtons.B])
                        DoButtonUp(InputButton.RFaceRight, XboxButtons.B);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.X) && !buttonStates[(int)XboxButtons.X])
                        DoButtonDown(InputButton.RFaceLeft, XboxButtons.X);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.X) && buttonStates[(int)XboxButtons.X])
                        DoButtonUp(InputButton.RFaceLeft, XboxButtons.X);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.Y) && !buttonStates[(int)XboxButtons.Y])
                        DoButtonDown(InputButton.RFaceUp, XboxButtons.Y);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.Y) && buttonStates[(int)XboxButtons.Y])
                        DoButtonUp(InputButton.RFaceUp, XboxButtons.Y);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.DPadDown) && !buttonStates[(int)XboxButtons.DPadDown])
                        DoButtonDown(InputButton.LFaceDown, XboxButtons.DPadDown);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.DPadDown) && buttonStates[(int)XboxButtons.DPadDown])
                        DoButtonUp(InputButton.LFaceDown, XboxButtons.DPadDown);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.DPadRight) && !buttonStates[(int)XboxButtons.DPadRight])
                        DoButtonDown(InputButton.LFaceRight, XboxButtons.DPadRight);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.DPadRight) && buttonStates[(int)XboxButtons.DPadRight])
                        DoButtonUp(InputButton.LFaceRight, XboxButtons.DPadRight);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.DPadLeft) && !buttonStates[(int)XboxButtons.DPadLeft])
                        DoButtonDown(InputButton.LFaceLeft, XboxButtons.DPadLeft);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.DPadLeft) && buttonStates[(int)XboxButtons.DPadLeft])
                        DoButtonUp(InputButton.LFaceLeft, XboxButtons.DPadLeft);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.DPadUp) && !buttonStates[(int)XboxButtons.DPadUp])
                        DoButtonDown(InputButton.LFaceUp, XboxButtons.DPadUp);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.DPadUp) && buttonStates[(int)XboxButtons.DPadUp])
                        DoButtonUp(InputButton.LFaceUp, XboxButtons.DPadUp);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.LeftShoulder) && !buttonStates[(int)XboxButtons.LeftShoulder])
                        DoButtonDown(InputButton.BumperLeft, XboxButtons.LeftShoulder);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.LeftShoulder) && buttonStates[(int)XboxButtons.LeftShoulder])
                        DoButtonUp(InputButton.BumperLeft, XboxButtons.LeftShoulder);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.RightShoulder) && !buttonStates[(int)XboxButtons.RightShoulder])
                        DoButtonDown(InputButton.BumperRight, XboxButtons.RightShoulder);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.RightShoulder) && buttonStates[(int)XboxButtons.RightShoulder])
                        DoButtonUp(InputButton.BumperRight, XboxButtons.RightShoulder);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.LeftThumb) && !buttonStates[(int)XboxButtons.LeftThumb])
                        DoButtonDown(InputButton.StickLeft, XboxButtons.LeftThumb);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.LeftThumb) && buttonStates[(int)XboxButtons.LeftThumb])
                        DoButtonUp(InputButton.StickLeft, XboxButtons.LeftThumb);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.RightThumb) && !buttonStates[(int)XboxButtons.RightThumb])
                        DoButtonDown(InputButton.StickRight, XboxButtons.RightThumb);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.RightThumb) && buttonStates[(int)XboxButtons.RightThumb])
                        DoButtonUp(InputButton.StickRight, XboxButtons.RightThumb);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.Back) && !buttonStates[(int)XboxButtons.Back])
                        DoButtonDown(InputButton.CenterLeft, XboxButtons.Back);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.Back) && buttonStates[(int)XboxButtons.Back])
                        DoButtonUp(InputButton.CenterLeft, XboxButtons.Back);

                    if (GetFlag(buttons, (short)GamepadButtonFlags.Start) && !buttonStates[(int)XboxButtons.Start])
                        DoButtonDown(InputButton.CenterRight, XboxButtons.Start);
                    if (!GetFlag(buttons, (short)GamepadButtonFlags.Start) && buttonStates[(int)XboxButtons.Start])
                        DoButtonUp(InputButton.CenterRight, XboxButtons.Start);

                    if (GetFlag(buttons, 0x0400) && !buttonStates[(int)XboxButtons.Guide])
                        DoButtonDown(InputButton.CenterMiddle, XboxButtons.Guide);
                    if (!GetFlag(buttons, 0x0400) && buttonStates[(int)XboxButtons.Guide])
                        DoButtonUp(InputButton.CenterMiddle, XboxButtons.Guide);

                    if ((controllerState.bLeftTrigger > thresholds.TriggerLeft) && !buttonStates[(int)XboxButtons.TriggerLeft])
                        DoButtonDown(InputButton.TriggerLeft, XboxButtons.TriggerLeft);
                    if (!(controllerState.bLeftTrigger > thresholds.TriggerLeft) && buttonStates[(int)XboxButtons.TriggerLeft])
                        DoButtonUp(InputButton.TriggerLeft, XboxButtons.TriggerLeft);

                    if ((controllerState.bRightTrigger > thresholds.TriggerRight) && !buttonStates[(int)XboxButtons.TriggerRight])
                        DoButtonDown(InputButton.TriggerRight, XboxButtons.TriggerRight);
                    if (!(controllerState.bRightTrigger > thresholds.TriggerRight) && buttonStates[(int)XboxButtons.TriggerRight])
                        DoButtonUp(InputButton.TriggerRight, XboxButtons.TriggerRight);
                    Thread.Sleep(5);
                    continue;
                }
                Thread.Sleep(500);
            }
        }

        private void ControllerWatcherThread()
        {
            while (true)
            {
                if (controller != null)
                    if (!controller.IsConnected)
                    {
                        // controller has disconnected
                        connected = false;
                        OnControllerDisconnected();
                    }

                if (!connected)
                {
                    UserIndex userIndex;
                    bool foundDevice = FindActiveDevice(out userIndex);
                    if (foundDevice)
                    {
                        activeControllerNumber = (int)userIndex;
                        controller = new Controller(userIndex);
                        connected = true;
                        OnControllerConnected();
                    }
                }
                Thread.Sleep(500);
            }
        }

        private void DoButtonDown(InputButton Button, XboxButtons XboxButton)
        {
            buttonStates[(int)XboxButton] = true;
            OnButtonDown(Button);
        }

        private void DoButtonUp(InputButton Button, XboxButtons XboxButton)
        {
            buttonStates[(int)XboxButton] = false;
            OnButtonUp(Button);
        }

        private bool FindActiveDevice(out UserIndex FirstIndex)
        {
            for (int i = 0; i < 4; i++)
            {
                UserIndex userIndex = (UserIndex)i;
                Controller checkController;
                try
                {
                    checkController = new Controller(userIndex);
                    if (checkController.IsConnected)
                    {
                        FirstIndex = userIndex;
                        return true;
                    }
                }
                catch
                {
                }
            }
            FirstIndex = UserIndex.Any;
            return false;
        }

        private bool GetFlag(ushort Buttons, short Flag)
        {
            bool value = (Buttons & Flag) != 0;
            return value;
        }

        private void Xbox360Driver_OnButtonDown(InputButton Button)
        {
            Console.WriteLine("ButtonDown" + Button);
        }

        private void Xbox360Driver_OnButtonUp(InputButton Button)
        {
            Console.WriteLine("ButtonUp " + Button);
        }

        private void Xbox360Driver_OnControllerConnected()
        {
        }

        private void Xbox360Driver_OnControllerDisconnected()
        {
        }

        private void Xbox360Driver_OnTouchpadMoved(InputTouchpadEventArgs e)
        {
            throw new NotImplementedException();
        }

        public struct XINPUT_GAMEPAD_SECRET
        {
            public Byte bLeftTrigger;
            public Byte bRightTrigger;
            public UInt32 eventCount;
            public short sThumbLX;
            public short sThumbLY;
            public short sThumbRX;
            public short sThumbRY;
            public ushort wButtons;
        }
        #region Unsupported Functions

        public void SetLEDColor(byte r, byte g, byte b)
        {
            // Not supported
            return;
        }

        public void SetLEDFlash(byte r, byte g, byte b, byte On, byte Off)
        {
            // Not supported
            return;
        }

        public void SetLEDOff()
        {
            // Not supported
            return;
        }

        #endregion Unsupported Functions
    }
}