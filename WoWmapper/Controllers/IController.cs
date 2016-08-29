using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WoWmapper.Controllers
{
    public delegate void ButtonStateChangedHandler(GamepadButton button, bool state);
    
    public interface IController
    {
        GamepadType Type { get; }
        byte BatteryLevel { get; }
        string Name { get; }

        event ButtonStateChangedHandler ButtonStateChanged;
        bool GetButtonState(GamepadButton button);
        int GetAxis(GamepadAxis axis);

        object UnderlyingController { get; }

        void SendRumble(byte left, byte right, int duration);

        void Stop();
        bool IsAlive();
    }

    public enum GamepadType
    {
        None,
        PlayStation,
        Xbox
    }

    public enum GamepadButton
    {
        LFaceUp,
        LFaceDown,
        LFaceLeft,
        LFaceRight,
        RFaceUp,
        RFaceDown,
        RFaceLeft,
        RFaceRight,
        CenterLeft,
        CenterRight,
        CenterMiddle,
        LeftStick,
        RightStick,
        ShoulderLeft,
        ShoulderRight,
        TriggerLeft,
        TriggerLeft2,
        TriggerRight,
        TriggerRight2,
        LeftStickUp,
        LeftStickDown,
        LeftStickLeft,
        LeftStickRight,
        TouchpadClick,
    }

    public enum GamepadAxis
    {
        StickLeftX,
        StickLeftY,
        StickRightX,
        StickRightY,
        TriggerLeft,
        TriggerRight,
        TouchpadX,
        TouchpadY,
    }
}

