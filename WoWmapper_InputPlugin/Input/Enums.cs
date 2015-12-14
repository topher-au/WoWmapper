using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.Input
{
    public enum InputStick
    {
        Left,
        Right
    }

    public enum InputButton
    {
        // DS4 Button Name
        // ---------------
        BumperLeft,     // L1
        BumperRight,    // R1
        TriggerLeft,    // L2
        TriggerRight,   // R2
        StickLeft,      // L3
        StickRight,     // R3
        LFaceUp,        // Dpad Up
        LFaceLeft,      // Dpad Left
        LFaceRight,     // Dpad Right
        LFaceDown,      // Dpad Down
        RFaceUp,        // Triangle
        RFaceLeft,      // Square
        RFaceRight,     // Circle
        RFaceDown,      // Cross
        CenterLeft,     // Share
        CenterRight,    // Options
        CenterMiddle,   // PS
        Extra1,         // Touch Left
        Extra2,         // Touch Right
        Extra3,         // Touch Upper
    }

    public enum InputAxis
    {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        LeftTrigger,
        RightTrigger,
        GyroX,
        GyroY,
        GyroZ
    }

    public enum InputRumbleMotor
    {
        Left,
        Right,
        Both
    }

    public enum InputConnectionType
    {
        Disconnected,
        Wired,
        Wireless
    }

}
