namespace WoWmapper.Input
{
    public class InputTouch
    {
        public int DeltaX, DeltaY, AbsX, AbsY;
    }

    public class InputThresholds
    {
        public int TriggerLeft, TriggerRight;
    }

    public class InputPeripherals
    {
        public bool Touchpad { get; set; }
        public bool Rumble { get; set; }
        public bool Gyro { get; set; }
        public bool LED { get; set; }
    }

    public class InputState
    {
        public InputButtonState Buttons;
        public InputAxisState Axis;
    }

    public class InputButtonState
    {
        public bool BumperLeft;     // L1
        public bool BumperRight;    // R1
        public bool TriggerLeft;    // L2
        public bool TriggerRight;   // R2
        public bool StickLeft;      // L3
        public bool StickRight;     // R3
        public bool LFaceUp;        // Dpad Up
        public bool LFaceLeft;      // Dpad Left
        public bool LFaceRight;     // Dpad Right
        public bool LFaceDown;      // Dpad Down
        public bool RFaceUp;        // Triangle
        public bool RFaceLeft;      // Square
        public bool RFaceRight;     // Circle
        public bool RFaceDown;      // Cross
        public bool CenterLeft;     // Share
        public bool CenterRight;    // Options
        public bool CenterMiddle;   // PS
        public bool Extra1;         // Touch Left
        public bool Extra2;         // Touch Right
        public bool Extra3;         // Touch Upper
    }

    public class InputAxisState
    {
        public int TriggerLeft;
        public int TriggerRight;
        public int LeftStickX;
        public int LeftStickY;
        public int RightStickX;
        public int RightStickY;
        public int GyroX;
        public int GyroY;
        public int GyroZ;
    }
}