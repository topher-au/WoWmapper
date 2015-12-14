using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.Input
{
    public interface IInputPlugin : IDisposable
    {
        // Input plugin details
        string Name { get; }
        Version Version { get; }
        string Author { get; }
        string Website { get; }
        string ControllerName { get; }

        // Configuration information
        InputThresholds Thresholds { get; set; }
        Keybind Keybinds { get; set; }
        SettingsFile Settings { get; set; }

        // State information
        bool Enabled { get; set; }
        bool Connected { get; }
        int Battery { get; }
        InputConnectionType ConnectionType { get; }
        InputPeripherals Peripherals { get; }

        int GetAxis(InputAxis Axis);
        bool GetButton(InputButton Button);
        InputState GetInputState();

        // Events
        event ControllerConnected OnControllerConnected;
        event ControllerDisconnected OnControllerDisconnected;
        event ButtonDown OnButtonDown;
        event ButtonUp OnButtonUp;
        event TouchpadMoved OnTouchpadMoved;

        // Rumble
        void SendRumble(InputRumbleMotor Motor, byte Strength, int Duration);
        void StopRumble();

        // LED/Lightbar
        void SetLEDColor(byte r, byte g, byte b);
        void SetLEDFlash(byte r, byte g, byte b, byte On, byte Off);
        void SetLEDOff();

        // Configuration windows
        void ShowKeybindDialog(bool ShowInTaskbar = true);
        void ShowConfigDialog();
    }
}
