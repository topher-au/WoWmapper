using System.Collections.Generic;
using System.Windows.Input;
using WoWmapper.Controllers;

namespace WoWmapper.Controllers
{
    public static class ControllerData
    {
        public static Dictionary<ControllerButton, Key> DefaultBinds { get; } = new Dictionary<ControllerButton, Key>
        {
            {ControllerButton.LStickUp, Key.W},
            {ControllerButton.LStickLeft, Key.A},
            {ControllerButton.LStickDown, Key.S},
            {ControllerButton.LStickRight, Key.D},
            {ControllerButton.ShoulderLeft, Key.LeftShift},
            {ControllerButton.TriggerLeft, Key.LeftCtrl},
            {ControllerButton.TriggerLeft2, Key.Subtract},
            {ControllerButton.ShoulderRight, Key.F7},
            {ControllerButton.TriggerRight, Key.F8},
            {ControllerButton.TriggerRight2, Key.Add},
            {ControllerButton.LFaceUp, Key.F1},
            {ControllerButton.LFaceRight, Key.F2},
            {ControllerButton.LFaceDown, Key.F3},
            {ControllerButton.LFaceLeft, Key.F4},
            {ControllerButton.RFaceUp, Key.F9},
            {ControllerButton.RFaceRight, Key.F10},
            {ControllerButton.RFaceDown, Key.F11},
            {ControllerButton.RFaceLeft, Key.F12},
            {ControllerButton.CenterMiddle, Key.Multiply},
            {ControllerButton.CenterRight, Key.F5},
            {ControllerButton.CenterLeft, Key.F6},
            {ControllerButton.TouchLeft, Key.F13},
            {ControllerButton.TouchRight, Key.F14}
        };

        public static class DS4
        {
            public static readonly List<ControllerButton> ValidButtons = new List<ControllerButton>
            {
                ControllerButton.TriggerLeft,
                ControllerButton.TriggerRight,
                ControllerButton.TriggerLeft2,
                ControllerButton.TriggerRight2,
                ControllerButton.ShoulderLeft,
                ControllerButton.ShoulderRight,
                ControllerButton.LFaceUp,
                ControllerButton.LFaceDown,
                ControllerButton.LFaceRight,
                ControllerButton.LFaceLeft,
                ControllerButton.RFaceUp,
                ControllerButton.RFaceRight,
                ControllerButton.RFaceDown,
                ControllerButton.RFaceLeft,
                ControllerButton.CenterMiddle,
                ControllerButton.CenterRight,
                ControllerButton.CenterLeft,
                ControllerButton.LStickUp,
                ControllerButton.LStickRight,
                ControllerButton.LStickDown,
                ControllerButton.LStickLeft,
                ControllerButton.TouchLeft,
                ControllerButton.TouchRight
            };

            public static readonly Dictionary<ControllerButton, string> ButtonNames = new Dictionary
                <ControllerButton, string>
            {
                {ControllerButton.LStickUp, "Move Forward"},
                {ControllerButton.LStickRight, "Move Right"},
                {ControllerButton.LStickDown, "Move Backward"},
                {ControllerButton.LStickLeft, "Move Left"},
                {ControllerButton.TriggerLeft, "L2"},
                {ControllerButton.TriggerLeft2, "L2 Grip"},
                {ControllerButton.TriggerRight, "R2"},
                {ControllerButton.TriggerRight2, "R2 Grip"},
                {ControllerButton.ShoulderLeft, "L1"},
                {ControllerButton.ShoulderRight, "R1"},
                {ControllerButton.StickLeft, "L3"},
                {ControllerButton.StickRight, "R3"},
                {ControllerButton.LFaceUp, "Dpad Up"},
                {ControllerButton.LFaceRight, "Dpad Right"},
                {ControllerButton.LFaceDown, "Dpad Down"},
                {ControllerButton.LFaceLeft, "Dpad Left"},
                {ControllerButton.RFaceUp, "Triangle"},
                {ControllerButton.RFaceRight, "Circle"},
                {ControllerButton.RFaceDown, "Cross"},
                {ControllerButton.RFaceLeft, "Square"},
                {ControllerButton.CenterMiddle, "PS"},
                {ControllerButton.CenterRight, "Options"},
                {ControllerButton.CenterLeft, "Share"},
                {ControllerButton.TouchLeft, "Touch Left"},
                {ControllerButton.TouchRight, "Touch Right"},
            };

            public static readonly Dictionary<ControllerButton, string> ButtonIcons = new Dictionary
                <ControllerButton, string>
            {
                {ControllerButton.TriggerLeft, "Resources/Buttons/DS4/L2.png"},
                {ControllerButton.TriggerLeft2, "Resources/Buttons/DS4/L2.png"},
                {ControllerButton.TriggerRight, "Resources/Buttons/DS4/R2.png"},
                {ControllerButton.TriggerRight2, "Resources/Buttons/DS4/R2.png"},
                {ControllerButton.ShoulderLeft, "Resources/Buttons/DS4/L1.png"},
                {ControllerButton.ShoulderRight, "Resources/Buttons/DS4/R1.png"},
                {ControllerButton.StickLeft, "Resources/Buttons/DS4/L3.png"},
                {ControllerButton.StickRight, "Resources/Buttons/DS4/R3.png"},
                {ControllerButton.LFaceUp, "Resources/Buttons/DS4/DpadUp.png"},
                {ControllerButton.LFaceRight, "Resources/Buttons/DS4/DpadRight.png"},
                {ControllerButton.LFaceDown, "Resources/Buttons/DS4/DpadDown.png"},
                {ControllerButton.LFaceLeft, "Resources/Buttons/DS4/DpadLeft.png"},
                {ControllerButton.RFaceUp, "Resources/Buttons/DS4/Triangle.png"},
                {ControllerButton.RFaceRight, "Resources/Buttons/DS4/Circle.png"},
                {ControllerButton.RFaceDown, "Resources/Buttons/DS4/Cross.png"},
                {ControllerButton.RFaceLeft, "Resources/Buttons/DS4/Square.png"},
                {ControllerButton.CenterMiddle, "Resources/Buttons/DS4/PS.png"},
                {ControllerButton.CenterRight, "Resources/Buttons/DS4/Options.png"},
                {ControllerButton.CenterLeft, "Resources/Buttons/DS4/Share.png"},
                {ControllerButton.LStickUp, "Resources/Buttons/DS4/DpadUp.png"},
                {ControllerButton.LStickRight, "Resources/Buttons/DS4/DpadRight.png"},
                {ControllerButton.LStickDown, "Resources/Buttons/DS4/DpadDown.png"},
                {ControllerButton.LStickLeft, "Resources/Buttons/DS4/DpadLeft.png"},
                {ControllerButton.TouchLeft, "Resources/Buttons/DS4/DpadLeft.png"},
                {ControllerButton.TouchRight, "Resources/Buttons/DS4/DpadRight.png"}
            };
        }

        public static class Xbox
        {
            public static readonly List<ControllerButton> ValidButtons = new List<ControllerButton>
            {
                ControllerButton.TriggerLeft,
                ControllerButton.TriggerRight,
                ControllerButton.TriggerLeft2,
                ControllerButton.TriggerRight2,
                ControllerButton.ShoulderLeft,
                ControllerButton.ShoulderRight,
                ControllerButton.LFaceUp,
                ControllerButton.LFaceDown,
                ControllerButton.LFaceRight,
                ControllerButton.LFaceLeft,
                ControllerButton.RFaceUp,
                ControllerButton.RFaceRight,
                ControllerButton.RFaceDown,
                ControllerButton.RFaceLeft,
                ControllerButton.CenterMiddle,
                ControllerButton.CenterRight,
                ControllerButton.CenterLeft,
                ControllerButton.LStickUp,
                ControllerButton.LStickDown,
                ControllerButton.LStickLeft,
                ControllerButton.LStickRight
            };

            public static readonly Dictionary<ControllerButton, string> ButtonNames = new Dictionary
                <ControllerButton, string>
            {
                {ControllerButton.LStickUp, "Move Forward"},
                {ControllerButton.LStickRight, "Move Right"},
                {ControllerButton.LStickDown, "Move Backward"},
                {ControllerButton.LStickLeft, "Move Left"},
                {ControllerButton.TriggerLeft, "Left Trigger"},
                {ControllerButton.TriggerLeft2, "Left Trigger Grip"},
                {ControllerButton.TriggerRight, "Right Trigger"},
                {ControllerButton.TriggerRight2, "Right Trigger Grip"},
                {ControllerButton.ShoulderLeft, "Left Bumper"},
                {ControllerButton.ShoulderRight, "Right Bumper"},
                {ControllerButton.StickLeft, "Left Thumb"},
                {ControllerButton.StickRight, "Right Thumb"},
                {ControllerButton.LFaceUp, "Dpad Up"},
                {ControllerButton.LFaceRight, "Dpad Right"},
                {ControllerButton.LFaceDown, "Dpad Down"},
                {ControllerButton.LFaceLeft, "Dpad Left"},
                {ControllerButton.RFaceUp, "Y"},
                {ControllerButton.RFaceRight, "B"},
                {ControllerButton.RFaceDown, "A"},
                {ControllerButton.RFaceLeft, "X"},
                {ControllerButton.CenterMiddle, "Guide"},
                {ControllerButton.CenterRight, "Start"},
                {ControllerButton.CenterLeft, "Back"},
            };

            public static readonly Dictionary<ControllerButton, string> ButtonIcons = new Dictionary
                <ControllerButton, string>
            {
                {ControllerButton.TriggerLeft, "Resources/Buttons/Xbox/TriggerLeft.png"},
                {ControllerButton.TriggerLeft2, "Resources/Buttons/Xbox/TriggerLeft.png"},
                {ControllerButton.TriggerRight, "Resources/Buttons/Xbox/TriggerRight.png"},
                {ControllerButton.TriggerRight2, "Resources/Buttons/Xbox/TriggerRight.png"},
                {ControllerButton.ShoulderLeft, "Resources/Buttons/Xbox/BumperLeft.png"},
                {ControllerButton.ShoulderRight, "Resources/Buttons/Xbox/BumperRight.png"},
                {ControllerButton.StickLeft, "Resources/Buttons/Xbox/LeftStick.png"},
                {ControllerButton.StickRight, "Resources/Buttons/Xbox/RightStick.png"},
                {ControllerButton.LFaceUp, "Resources/Buttons/Xbox/LFaceUp.png"},
                {ControllerButton.LFaceRight, "Resources/Buttons/Xbox/LFaceRight.png"},
                {ControllerButton.LFaceDown, "Resources/Buttons/Xbox/LFaceDown.png"},
                {ControllerButton.LFaceLeft, "Resources/Buttons/Xbox/LFaceLeft.png"},
                {ControllerButton.RFaceUp, "Resources/Buttons/Xbox/RFaceUp.png"},
                {ControllerButton.RFaceRight, "Resources/Buttons/Xbox/RFaceRight.png"},
                {ControllerButton.RFaceDown, "Resources/Buttons/Xbox/RFaceDown.png"},
                {ControllerButton.RFaceLeft, "Resources/Buttons/Xbox/RFaceLeft.png"},
                {ControllerButton.CenterMiddle, "Resources/Buttons/Xbox/CenterMiddle.png"},
                {ControllerButton.CenterRight, "Resources/Buttons/Xbox/CenterRight.png"},
                {ControllerButton.CenterLeft, "Resources/Buttons/Xbox/CenterLeft.png"},
                {ControllerButton.LStickUp, "Resources/Buttons/Xbox/LFaceUp.png"},
                {ControllerButton.LStickRight, "Resources/Buttons/Xbox/LFaceRight.png"},
                {ControllerButton.LStickDown, "Resources/Buttons/Xbox/LFaceDown.png"},
                {ControllerButton.LStickLeft, "Resources/Buttons/Xbox/LFaceLeft.png"}
            };
        }
    }
}