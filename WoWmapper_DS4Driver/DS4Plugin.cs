using DS4Windows;
using DS4Wrapper;
using System;
using System.Reflection;
using WoWmapper.Input;
using WoWmapper_DS4Driver.Forms;

namespace DS4Driver
{
    internal class WoWmapper_DS4Plugin : IInputPlugin
    {
        private DS4 Controller;
        private bool disposedValue = false;
        private SettingsFile settings = new SettingsFile("input_ds4");
        private InputThresholds Threshold;

        public WoWmapper_DS4Plugin()
        {
            InitializeSettings();
        }

        public WoWmapper_DS4Plugin(bool LoadPlugin)
        {
            InitializeSettings();
            if (LoadPlugin)
            {
                Controller = new DS4();
                Controller.ButtonDown += Controller_ButtonDown;
                Controller.ButtonUp += Controller_ButtonUp;
                Controller.TouchpadMoved += Controller_TouchpadMoved;
            }
        }

        public event ButtonDown OnButtonDown;

        public event ButtonUp OnButtonUp;

        public event ControllerConnected OnControllerConnected;

        public event ControllerDisconnected OnControllerDisconnected;

        public event TouchpadMoved OnTouchpadMoved;

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
                if (Connected)
                {
                    var battery = Controller.Battery;
                    if (battery > 100) battery = 100;
                    return battery;
                }
                return 0;
            }
        }

        public bool Connected
        {
            get
            {
                if (!Enabled) return false;

                if (Controller == null) return false;

                return Controller.IsConnected;
            }
        }

        public InputConnectionType ConnectionType
        {
            get
            {
                if (Connected)
                {
                    var DS4Connection = Controller.ConnectionType;
                    switch (DS4Connection)
                    {
                        case DS4Windows.ConnectionType.BT:
                            return InputConnectionType.Bluetooth;

                        case DS4Windows.ConnectionType.USB:
                            return InputConnectionType.Wired;
                    }
                }
                return InputConnectionType.Disconnected;
            }
        }

        public string ControllerName
        {
            get
            {
                return "DualShock 4";
            }
        }

        public bool Enabled { get; set; } = false;

        Keybind IInputPlugin.Keybinds
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
                return "DualShock 4";
            }
        }

        public InputPeripherals Peripherals
        {
            get
            {
                return new InputPeripherals()
                {
                    Rumble = true,
                    Touchpad = true,
                    Gyro = true,
                    LED = true
                };
            }
        }

        public float RightCurve
        {
            get
            {
                float RightCurve;
                settings.Settings.Read<float>("RightCurve", out RightCurve);
                return RightCurve;
            }

            set
            {
                settings.Settings.Write<float>("RightCurve", value);
            }
        }

        public float RightDead
        {
            get
            {
                float RightDead;
                settings.Settings.Read<float>("RightDead", out RightDead);
                return RightDead;
            }

            set
            {
                settings.Settings.Write<float>("RightDead", value);
            }
        }

        public float RightSpeed
        {
            get
            {
                float RightSpeed;
                settings.Settings.Read<float>("RightSpeed", out RightSpeed);
                return RightSpeed;
            }

            set
            {
                settings.Settings.Write<float>("RightSpeed", value);
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
                throw new NotImplementedException();
            }
        }

        public InputThresholds Thresholds
        {
            get
            {
                return Threshold;
            }

            set
            {
                Threshold = value;
            }
        }

        public int touchMode
        {
            get
            {
                int TouchMode;
                settings.Settings.Read("TouchMode", out TouchMode);
                return TouchMode;
            }

            set
            {
                settings.Settings.Write("TouchMode", value);
            }
        }

        public Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        public string Website
        {
            get
            {
                return "";
            }
        }

        public int TouchMode
        {
            get
            {
                int touchMode = 0;
                if( Settings.Settings.Read<int>("TouchMode", out touchMode))
                {
                    return touchMode;
                }
                return 0;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public int GetAxis(InputAxis Axis)
        {
            if (Connected)
                switch (Axis)
                {
                    case InputAxis.LeftStickX:
                        return Controller.GetStickPoint(DS4Stick.Left).X;

                    case InputAxis.LeftStickY:
                        return Controller.GetStickPoint(DS4Stick.Left).Y;

                    case InputAxis.RightStickX:
                        return Controller.GetStickPoint(DS4Stick.Right).X;

                    case InputAxis.RightStickY:
                        return Controller.GetStickPoint(DS4Stick.Right).Y;

                    case InputAxis.LeftTrigger:
                        return (int)(Controller.GetTriggerState(DS4Trigger.L2) * 255);

                    case InputAxis.RightTrigger:
                        return (int)(Controller.GetTriggerState(DS4Trigger.R2) * 255);
                }
            return 0;
        }

        public bool GetButton(InputButton Button)
        {
            if (Controller != null)
            {
                if (Controller.IsConnected)
                {
                    switch (Button)
                    {
                        case InputButton.BumperLeft:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.BumperRight:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.TriggerLeft:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.TriggerRight:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.StickLeft:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.StickRight:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.LFaceUp:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.LFaceLeft:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.LFaceRight:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.LFaceDown:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.RFaceUp:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.RFaceLeft:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.RFaceRight:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.RFaceDown:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.CenterLeft:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.CenterRight:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.CenterMiddle:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.Extra1:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.Extra2:
                            return Controller.GetButtonState(DS4Button.L1);

                        case InputButton.Extra3:
                            return Controller.GetButtonState(DS4Button.L1);
                    }
                }
            }
            return false;
        }

        public InputState GetInputState()
        {
            if (Connected)
            {
                var lStick = Controller.GetStickPoint(DS4Stick.Left);
                var rStick = Controller.GetStickPoint(DS4Stick.Right);
                return new InputState()
                {
                    Buttons = new InputButtonState()
                    {
                        BumperLeft = Controller.GetButtonState(DS4Button.L1),
                        BumperRight = Controller.GetButtonState(DS4Button.R1),
                        TriggerLeft = Controller.GetButtonState(DS4Button.L2),
                        TriggerRight = Controller.GetButtonState(DS4Button.R2),
                        StickLeft = Controller.GetButtonState(DS4Button.L3),
                        StickRight = Controller.GetButtonState(DS4Button.R3),
                        LFaceDown = Controller.GetButtonState(DS4Button.DpadDown),
                        LFaceLeft = Controller.GetButtonState(DS4Button.DpadLeft),
                        LFaceRight = Controller.GetButtonState(DS4Button.DpadRight),
                        LFaceUp = Controller.GetButtonState(DS4Button.DpadUp),
                        RFaceDown = Controller.GetButtonState(DS4Button.Cross),
                        RFaceLeft = Controller.GetButtonState(DS4Button.Square),
                        RFaceRight = Controller.GetButtonState(DS4Button.Circle),
                        RFaceUp = Controller.GetButtonState(DS4Button.Triangle),
                        CenterLeft = Controller.GetButtonState(DS4Button.Share),
                        CenterMiddle = Controller.GetButtonState(DS4Button.PS),
                        CenterRight = Controller.GetButtonState(DS4Button.Options),
                        Extra1 = Controller.GetButtonState(DS4Button.TouchLeft),
                        Extra2 = Controller.GetButtonState(DS4Button.TouchRight),
                        Extra3 = Controller.GetButtonState(DS4Button.TouchUpper)
                    },
                    Axis = new InputAxisState()
                    {
                        LeftStickX = lStick.X,
                        LeftStickY = lStick.Y,
                        RightStickX = rStick.X,
                        RightStickY = rStick.Y
                        // TODO: add triggers
                    }
                };
            }
            return null;
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
            if (Controller != null) Controller.Dispose();
        }

        public void SendRumble(InputRumbleMotor Motor, byte Strength, int Duration)
        {
            if (Connected)
            {
                switch (Motor)
                {
                    case InputRumbleMotor.Left:
                        Controller.Rumble(0, (byte)Strength, Duration);
                        break;

                    case InputRumbleMotor.Right:
                        Controller.Rumble((byte)Strength, 0, Duration);
                        break;

                    case InputRumbleMotor.Both:
                        Controller.Rumble((byte)Strength, (byte)Strength, Duration);
                        break;
                }
            }
        }

        public void SetLEDColor(byte r, byte g, byte b)
        {
            if (Connected)
            {
                var c = new DS4Color(r, g, b);
                Controller.LightBarOn(c);
            }
        }

        public void SetLEDFlash(byte r, byte g, byte b, byte On, byte Off)
        {
            if (Connected)
            {
                Controller.LightBarFlash(new DS4Color(r, g, b), On, Off);
            }
        }

        public void SetLEDOff()
        {
            if (Connected)
            {
                Controller.LightBarOff();
            }
        }

        public void ShowConfigDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowKeybindDialog(bool ShowInTaskbar)
        {
            KeybindForm kbf = new KeybindForm(Controller, settings);
            kbf.ShowInTaskbar = ShowInTaskbar;
            kbf.ShowDialog();
        }

        public void StopRumble()
        {
            if (Connected)
            {
                Controller.Rumble(0, 0, 0);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Controller != null)
                    {
                        Controller.Dispose();
                        Controller = null;
                    }
                }

                disposedValue = true;
            }
        }

        private void Controller_ButtonDown(DS4Button Button)
        {
            switch (Button)
            {
                case DS4Button.Cross:
                    OnButtonDown(InputButton.RFaceDown);
                    break;

                case DS4Button.Circle:
                    OnButtonDown(InputButton.RFaceRight);
                    break;

                case DS4Button.Triangle:
                    OnButtonDown(InputButton.RFaceUp);
                    break;

                case DS4Button.Square:
                    OnButtonDown(InputButton.RFaceLeft);
                    break;

                case DS4Button.L1:
                    OnButtonDown(InputButton.BumperLeft);
                    break;

                case DS4Button.L2:
                    OnButtonDown(InputButton.TriggerLeft);
                    break;

                case DS4Button.L3:
                    OnButtonDown(InputButton.StickLeft);
                    break;

                case DS4Button.R1:
                    OnButtonDown(InputButton.BumperRight);
                    break;

                case DS4Button.R2:
                    OnButtonDown(InputButton.TriggerRight);
                    break;

                case DS4Button.R3:
                    OnButtonDown(InputButton.StickRight);
                    break;

                case DS4Button.DpadUp:
                    OnButtonDown(InputButton.LFaceUp);
                    break;

                case DS4Button.DpadDown:
                    OnButtonDown(InputButton.LFaceDown);
                    break;

                case DS4Button.DpadRight:
                    OnButtonDown(InputButton.LFaceRight);
                    break;

                case DS4Button.DpadLeft:
                    OnButtonDown(InputButton.LFaceLeft);
                    break;

                case DS4Button.Share:
                    OnButtonDown(InputButton.CenterLeft);
                    break;

                case DS4Button.Options:
                    OnButtonDown(InputButton.CenterRight);
                    break;

                case DS4Button.PS:
                    OnButtonDown(InputButton.CenterMiddle);
                    break;

                case DS4Button.TouchLeft:
                    OnButtonDown(InputButton.Extra1);
                    break;

                case DS4Button.TouchRight:
                    OnButtonDown(InputButton.Extra2);
                    break;

                case DS4Button.TouchUpper:
                    OnButtonDown(InputButton.Extra3);
                    break;
            }
        }

        private void Controller_ButtonUp(DS4Button Button)
        {
            switch (Button)
            {
                case DS4Button.Cross:
                    OnButtonUp(InputButton.RFaceDown);
                    break;

                case DS4Button.Circle:
                    OnButtonUp(InputButton.RFaceRight);
                    break;

                case DS4Button.Triangle:
                    OnButtonUp(InputButton.RFaceUp);
                    break;

                case DS4Button.Square:
                    OnButtonUp(InputButton.RFaceLeft);
                    break;

                case DS4Button.L1:
                    OnButtonUp(InputButton.BumperLeft);
                    break;

                case DS4Button.L2:
                    OnButtonUp(InputButton.TriggerLeft);
                    break;

                case DS4Button.L3:
                    OnButtonUp(InputButton.StickLeft);
                    break;

                case DS4Button.R1:
                    OnButtonUp(InputButton.BumperRight);
                    break;

                case DS4Button.R2:
                    OnButtonUp(InputButton.TriggerRight);
                    break;

                case DS4Button.R3:
                    OnButtonUp(InputButton.StickRight);
                    break;

                case DS4Button.DpadUp:
                    OnButtonUp(InputButton.LFaceUp);
                    break;

                case DS4Button.DpadDown:
                    OnButtonUp(InputButton.LFaceDown);
                    break;

                case DS4Button.DpadRight:
                    OnButtonUp(InputButton.LFaceRight);
                    break;

                case DS4Button.DpadLeft:
                    OnButtonUp(InputButton.LFaceLeft);
                    break;

                case DS4Button.Share:
                    OnButtonUp(InputButton.CenterLeft);
                    break;

                case DS4Button.Options:
                    OnButtonUp(InputButton.CenterRight);
                    break;

                case DS4Button.PS:
                    OnButtonUp(InputButton.CenterMiddle);
                    break;

                case DS4Button.TouchLeft:
                    OnButtonUp(InputButton.Extra1);
                    break;

                case DS4Button.TouchRight:
                    OnButtonUp(InputButton.Extra2);
                    break;

                case DS4Button.TouchUpper:
                    OnButtonUp(InputButton.Extra3);
                    break;
            }
        }

        private void Controller_ControllerConnected()
        {
            OnControllerConnected();
        }

        private void Controller_ControllerDisconnected()
        {
            OnControllerDisconnected();
        }

        private void Controller_TouchpadMoved(TouchpadEventArgs e)
        {
            // Convert all touchpad touches and raise event

            InputTouch[] inputTouch = new InputTouch[e.touches.Length];
            for (int i = 0; i < inputTouch.Length; i++)
            {
                inputTouch[i] = new InputTouch()
                {
                    DeltaX = e.touches[i].deltaX,
                    DeltaY = e.touches[i].deltaY,
                    AbsX = e.touches[i].hwX,
                    AbsY = e.touches[i].hwY,
                };
            }

            OnTouchpadMoved(new InputTouchpadEventArgs()
            {
                touches = inputTouch
            });
        }
        // To detect redundant calls
    }
}