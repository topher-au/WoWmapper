using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;

namespace WoWmapper.ConsolePort
{
    public class BindDictionary
    {
        private readonly List<Keybind> _bindings = Properties.Settings.Default.CustomBindings
            ? BindManager.CurrentKeybinds
            : Defaults.KeybindStyles.GetDefault(Properties.Settings.Default.ModifierStyle);

        private Dictionary<string, string> _dict = new Dictionary<string, string>();

        public BindDictionary()
        {
            _dict.Add("CP_R_UP", GetWoWStringName(GamepadButton.RFaceUp));
            _dict.Add("CP_R_DOWN", GetWoWStringName(GamepadButton.RFaceDown));
            _dict.Add("CP_R_LEFT", GetWoWStringName(GamepadButton.RFaceLeft));
            _dict.Add("CP_R_RIGHT", GetWoWStringName(GamepadButton.RFaceRight));
            _dict.Add("CP_L_UP", GetWoWStringName(GamepadButton.LFaceUp));
            _dict.Add("CP_L_DOWN", GetWoWStringName(GamepadButton.LFaceDown));
            _dict.Add("CP_L_LEFT", GetWoWStringName(GamepadButton.LFaceLeft));
            _dict.Add("CP_L_RIGHT", GetWoWStringName(GamepadButton.LFaceRight));
            _dict.Add("CP_X_LEFT", GetWoWStringName(GamepadButton.CenterLeft));
            _dict.Add("CP_X_CENTER", GetWoWStringName(GamepadButton.CenterMiddle));
            _dict.Add("CP_X_RIGHT", GetWoWStringName(GamepadButton.CenterRight));
            _dict.Add("CP_L_PULL", GetWoWStringName(GamepadButton.TriggerLeft2));
            _dict.Add("CP_R_PULL", GetWoWStringName(GamepadButton.TriggerRight2));
            _dict.Add("CP_TL1", GetWoWStringName(GamepadButton.TriggerLeft));
            _dict.Add("CP_TL2", GetWoWStringName(GamepadButton.TriggerLeft));
            _dict.Add("CP_M1", GetWoWStringName(GamepadButton.TriggerRight));
            _dict.Add("CP_M2", GetWoWStringName(GamepadButton.TriggerRight));

            var triggerBinds = new List<GamepadButton>
            {
                GamepadButton.ShoulderLeft,
                GamepadButton.ShoulderRight,
                GamepadButton.TriggerLeft,
                GamepadButton.TriggerRight
            };

            var mod1 = _bindings.First(bind => bind.Key == Key.LeftCtrl);
            var mod2 = _bindings.First(bind => bind.Key == Key.LeftShift);

            triggerBinds.Remove(mod1.BindType);
            triggerBinds.Remove(mod2.BindType);

            _dict.Add("CP_T1", GetWoWStringName(triggerBinds[0]));
            _dict.Add("CP_T2", GetWoWStringName(triggerBinds[1]));
            _dict.Add("T_T1", GetTextureName(triggerBinds[0]));
            _dict.Add("T_T2", GetTextureName(triggerBinds[1]));
            _dict.Add("T_M1", GetTextureName(_bindings.First(bind => bind.Key == Key.LeftShift).BindType));
            _dict.Add("T_M2", GetTextureName(_bindings.First(bind => bind.Key == Key.LeftCtrl).BindType));

            if (Properties.Settings.Default.ButtonStyle == 0)
            {
                if (ControllerManager.ActiveController != null)
                    _dict.Add("TYPE", ControllerManager.ActiveController.Type == GamepadType.Xbox ? "XBOX" : "PS4");
                else
                    _dict.Add("TYPE", "XBOX");
            }
            else if (Properties.Settings.Default.ButtonStyle == 1)
            {
                _dict.Add("TYPE", "PS4");
            }
            else if (Properties.Settings.Default.ButtonStyle == 2)
            {
                _dict.Add("TYPE", "XBOX");
            }
            
            _dict.Add("TOGGLERUN", "NUMPADDIVIDE");
            _dict.Add("ID", Properties.Settings.Default.BindingsModified.ToFileTimeUtc().ToString());
        }

        public string GetBindKey(string cpBindName)
        {
            return _dict[cpBindName];
        }

        private string GetTextureName(GamepadButton button)
        {
            switch (button)
            {
                case GamepadButton.ShoulderLeft:
                    return "CP_TL1";
                case GamepadButton.ShoulderRight:
                    return "CP_TR1";
                case GamepadButton.TriggerLeft:
                    return "CP_TL2";
                case GamepadButton.TriggerRight:
                    return "CP_TR2";
            }
            return string.Empty;
        }

        private static Dictionary<Key, string> WowKeysDictionary = new Dictionary<Key, string>()
        {
            {Key.LeftShift, "LSHIFT"},
            {Key.RightShift, "RSHIFT"},
            {Key.LeftCtrl, "LCTRL"},
            {Key.RightCtrl, "RCTRL"},
            {Key.LeftAlt, "LALT"},
            {Key.RightAlt, "RALT"},
            {Key.Add, "NUMPADPLUS"},
            {Key.Subtract, "NUMPADMINUS"},
            {Key.Multiply, "NUMPADMULTIPLY"},
            {Key.Divide, "NUMPADDIVIDE"},
            {Key.Decimal, "NUMPADDECIMAL"},
            {Key.D0, "0"},
            {Key.D1, "1"},
            {Key.D2, "2"},
            {Key.D3, "3"},
            {Key.D4, "4"},
            {Key.D5, "5"},
            {Key.D6, "6"},
            {Key.D7, "7"},
            {Key.D8, "8"},
            {Key.D9, "9"},
            {Key.OemMinus, "-"},
            {Key.OemPlus, "="},
            {Key.OemBackslash, "\\"},
            {Key.OemTilde, "`"},
            {Key.OemComma, ","},
            {Key.OemOpenBrackets, "["},
            {Key.OemCloseBrackets, "]"},
            {Key.OemSemicolon, ";"},
            {Key.OemQuestion, "/"},
            {Key.OemQuotes, "\""},
        };

        private string GetWoWStringName(GamepadButton button)
        {
            var bindKey = _bindings.FirstOrDefault(bind => bind.BindType == button)?.Key;
            var bindVal = WowKeysDictionary.FirstOrDefault(bind => bind.Key == bindKey);
            if (bindVal.Key == Key.None)
            {
                return bindKey.ToString().ToUpper();
            }
            else
            {
                return WowKeysDictionary[bindVal.Key];
            }
        }
    }
}