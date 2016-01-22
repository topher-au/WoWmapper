using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.Controllers
{
    public interface IController
    {
        object UnderlyingDevice { get; }
        string ControllerID { get; }
        int Battery { get; }

        ControllerType Type { get; }
        ControllerPeripherals Peripherals { get; }
        ControllerBatteryState BatteryState { get; }
        ControllerConnectionType ConnectionType { get; }

        ControllerState GetControllerState();
        ControllerTouchpadState GetTouchpadState();
        void SendRumble(byte Left, byte Right, int Duration);
        void SetLightbar(byte r, byte g, byte b);
        void SetLightbar(byte r, byte g, byte b, int flashOn, int flashOff);
        void Stop();
        bool IsAlive();
    }

    public class ControllerPeripherals
    {
        public int Touchpads { get; set; }
        public bool Lightbar { get; set; }
        public bool Chatpad { get; set; }
        public bool Vibration { get; set; }
        public bool Gyro { get; set; }
    }

    public class ControllerState
    {
        public bool ShoulderLeft { get; internal set; }
        public bool ShoulderRight { get; internal set; }

        public int TriggerLeft { get; internal set; }
        public int TriggerRight { get; internal set; }

        public bool StickLeft { get; set; }
        public bool StickRight { get; set; }

        public bool CenterLeft { get; internal set; }
        public bool CenterRight { get; internal set; }
        public bool CenterMiddle { get; internal set; }

        public bool LFaceUp { get; internal set; }
        public bool LFaceRight { get; internal set; }
        public bool LFaceDown { get; internal set; }
        public bool LFaceLeft { get; internal set; }

        public bool RFaceUp { get; internal set; }
        public bool RFaceRight { get; internal set; }
        public bool RFaceDown { get; internal set; }
        public bool RFaceLeft { get; internal set; }

        public bool TouchLeft { get; internal set; }
        public bool TouchRight { get; internal set; }
        public bool TouchButton { get; internal set; }
        
        public int GyroX { get; internal set; }
        public int GyroY { get; internal set; }
        public int GyroZ { get; internal set; }

        public int LeftX { get; internal set; }
        public int LeftY { get; internal set; }
        public int RightX { get; internal set; }
        public int RightY { get; internal set; }

        public ControllerTouchpadState[] Touchpads { get; internal set; }
    }

    public class ControllerTouchpadState
    {
        public int TouchX { get; internal set; }
        public int TouchY { get; internal set; }
        public int DeltaX { get; internal set; }
        public int DeltaY { get; internal set; }
        public bool TouchDown { get; internal set; }
    }

    public class ControllerHapticState
    {
        public int LeftRumble { get; set; }
        public int RightRumble { get; set; }
        public int LightbarR { get; set; }
        public int LightbarG { get; set; }
        public int LightbarB { get; set; }
}
    public enum ControllerBatteryState
    {
        None,
        Charging,
        Discharging,
        Full
    }

    public enum ControllerType
    {
        None,
        DualShock4,
        Xbox
    }

    public enum ControllerConnectionType
    {
        None,
        USB,
        Wireless,
        Bluetooth
    }

    public enum ControllerAxis
    {
        LStickX,
        LStickY,
        RStickX,
        RStickY,
        GyroX,
        GyroY,
        GyroZ
    }

    public enum ControllerButton
    {
        TriggerLeft,
        TriggerLeft2,
        TriggerRight,
        TriggerRight2,
        ShoulderLeft,
        ShoulderRight,
        StickLeft,
        StickRight,
        LFaceUp,
        LFaceRight,
        LFaceDown,
        LFaceLeft,
        RFaceUp,
        RFaceRight,
        RFaceDown,
        RFaceLeft,
        CenterLeft,
        CenterRight,
        CenterMiddle,
        TouchLeft,
        TouchRight,
        TouchButton,
        LStickUp,
        LStickRight,
        LStickDown,
        LStickLeft
    }
}
