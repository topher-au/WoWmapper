using System;

namespace WoWmapper.Input
{
    public interface IInputPlugin
    {
        event ButtonDown OnButtonDown;

        event ButtonUp OnButtonUp;

        // Events
        event ControllerConnected OnControllerConnected;

        event ControllerDisconnected OnControllerDisconnected;

        event TouchpadMoved OnTouchpadMoved;

        string Author { get; }

        int Battery { get; }

        bool Connected { get; }

        int TouchMode { get; }

        InputConnectionType ConnectionType { get; }

        string ControllerName { get; }

        // State information
        bool Enabled { get; set; }

        Keybind Keybinds { get; set; }

        // Input plugin details
        string Name { get; }

        InputPeripherals Peripherals { get; }
        float RightCurve { get; set; }
        float RightDead { get; set; }
        float RightSpeed { get; set; }
        SettingsFile Settings { get; set; }
        // Configuration information
        InputThresholds Thresholds { get; set; }

        int touchMode { get; set; }
        Version Version { get; }
        string Website { get; }
        int GetAxis(InputAxis Axis);

        bool GetButton(InputButton Button);

        InputState GetInputState();
        void Kill();

        // Rumble
        void SendRumble(InputRumbleMotor Motor, byte Strength, int Duration);

        // LED/Lightbar
        void SetLEDColor(byte r, byte g, byte b);

        void SetLEDFlash(byte r, byte g, byte b, byte On, byte Off);

        void SetLEDOff();

        void ShowConfigDialog();

        // Configuration windows
        void ShowKeybindDialog(bool ShowInTaskbar = true);

        void StopRumble();
    }
}