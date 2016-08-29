using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;

namespace WoWmapper.Keybindings
{
    public static class Defaults
    {
        public static class KeybindStyles
        {
            public static List<Keybind> GetDefault(int style)
            {
                var binds = new List<Keybind>();

                switch (style)
                {
                    case 0:
                        binds.AddRange(new List<Keybind>
                        {
                            new Keybind {BindType = GamepadButton.ShoulderLeft, Key = Key.LeftShift},
                            new Keybind {BindType = GamepadButton.ShoulderRight, Key = Key.F7},
                            new Keybind {BindType = GamepadButton.TriggerLeft, Key = Key.LeftCtrl},
                            new Keybind {BindType = GamepadButton.TriggerRight, Key = Key.F8},
                        });
                        break;
                    case 1:
                        binds.AddRange(new List<Keybind>
                        {
                            new Keybind {BindType = GamepadButton.ShoulderLeft, Key = Key.F7},
                            new Keybind {BindType = GamepadButton.ShoulderRight, Key = Key.F8},
                            new Keybind {BindType = GamepadButton.TriggerLeft, Key = Key.LeftShift},
                            new Keybind {BindType = GamepadButton.TriggerRight, Key = Key.LeftCtrl},
                        });
                        break;
                    case 2:
                        binds.AddRange(new List<Keybind>
                        {
                            new Keybind {BindType = GamepadButton.ShoulderLeft, Key = Key.F7},
                            new Keybind {BindType = GamepadButton.TriggerLeft, Key = Key.F8},
                            new Keybind {BindType = GamepadButton.ShoulderRight, Key = Key.LeftShift},
                            new Keybind {BindType = GamepadButton.TriggerRight, Key = Key.LeftCtrl},
                        });
                        break;
                    case 3:
                        binds.AddRange(new List<Keybind>
                        {
                            new Keybind {BindType = GamepadButton.TriggerLeft, Key = Key.F7},
                            new Keybind {BindType = GamepadButton.TriggerRight, Key = Key.F8},
                            new Keybind {BindType = GamepadButton.ShoulderLeft, Key = Key.LeftShift},
                            new Keybind {BindType = GamepadButton.ShoulderRight, Key = Key.LeftCtrl},
                        });
                        break;
                }
                binds.AddRange(DefaultBase);
                return binds;
            }

            public static List<Keybind> DefaultBase = new List<Keybind>
            {
                new Keybind {BindType = GamepadButton.RFaceUp, Key = Key.F9},
                new Keybind {BindType = GamepadButton.RFaceRight, Key = Key.F10},
                new Keybind {BindType = GamepadButton.RFaceDown, Key = Key.F11},
                new Keybind {BindType = GamepadButton.RFaceLeft, Key = Key.F12},

                new Keybind {BindType = GamepadButton.LFaceUp, Key = Key.F1},
                new Keybind {BindType = GamepadButton.LFaceRight, Key = Key.F2},
                new Keybind {BindType = GamepadButton.LFaceDown, Key = Key.F3},
                new Keybind {BindType = GamepadButton.LFaceLeft, Key = Key.F4},

                new Keybind {BindType = GamepadButton.CenterLeft, Key = Key.F5},
                new Keybind {BindType = GamepadButton.CenterRight, Key = Key.F6},
                new Keybind {BindType = GamepadButton.CenterMiddle, Key = Key.Multiply},

                new Keybind {BindType = GamepadButton.LeftStickUp, Key = Key.W},
                new Keybind {BindType = GamepadButton.LeftStickLeft, Key = Key.A},
                new Keybind {BindType = GamepadButton.LeftStickDown, Key = Key.S},
                new Keybind {BindType = GamepadButton.LeftStickRight, Key = Key.D},
            };
        }
    }
}