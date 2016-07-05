using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MahApps.Metro.Controls.Dialogs;
using WoWmapper.Classes;
using WoWmapper.Controllers;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft.AddOns;
using MessageBox = System.Windows.MessageBox;

namespace WoWmapper.Input
{
    public static class BindingManager
    {
        public static event BindingsUpdatedHandler BindingsUpdated;

        public delegate void BindingsUpdatedHandler();

        private static KeybindProfile _keybind;

        public static Dictionary<ControllerButton, Key> CurrentBinds
        {
            get
            {
                if (_keybind == null) return ControllerData.DefaultBinds;
                return _keybind.Keybinds;
            }

            set
            {
                _keybind.Keybinds = value;
                BindingsUpdated?.Invoke();
            }
        }

        public static void LoadProfile(KeybindProfile profile)
        {
            _keybind = profile;
            BindingsUpdated?.Invoke();
        }

        public static void SetKeybind(ControllerButton button, Key key)
        {
            foreach (var k in _keybind.Keybinds)
            {
                if (k.Value == key)
                {
                    _keybind.Keybinds[k.Key] = _keybind.Keybinds[button];
                    _keybind.Keybinds[button] = key;
                    BindingsUpdated?.Invoke();
                    return;
                }
            }
            _keybind.Keybinds[button] = key;
            BindingsUpdated?.Invoke();
        }

        public static Dictionary<string, string> GetConsolePortBindDictionary()
        {
            if (_keybind == null) return null;

            var bindDict = new Dictionary<string, string>();
            bindDict.Add("CP_R_UP", GetWoWStringName(CurrentBinds[ControllerButton.RFaceUp]));
            bindDict.Add("CP_R_DOWN", GetWoWStringName(CurrentBinds[ControllerButton.RFaceDown]));
            bindDict.Add("CP_R_LEFT", GetWoWStringName(CurrentBinds[ControllerButton.RFaceLeft]));
            bindDict.Add("CP_R_RIGHT", GetWoWStringName(CurrentBinds[ControllerButton.RFaceRight]));

            bindDict.Add("CP_L_UP", GetWoWStringName(CurrentBinds[ControllerButton.LFaceUp]));
            bindDict.Add("CP_L_DOWN", GetWoWStringName(CurrentBinds[ControllerButton.LFaceDown]));
            bindDict.Add("CP_L_LEFT", GetWoWStringName(CurrentBinds[ControllerButton.LFaceLeft]));
            bindDict.Add("CP_L_RIGHT", GetWoWStringName(CurrentBinds[ControllerButton.LFaceRight]));
            bindDict.Add("CP_X_LEFT", GetWoWStringName(CurrentBinds[ControllerButton.CenterLeft]));
            bindDict.Add("CP_X_CENTER", GetWoWStringName(CurrentBinds[ControllerButton.CenterMiddle]));
            bindDict.Add("CP_X_RIGHT", GetWoWStringName(CurrentBinds[ControllerButton.CenterRight]));
            bindDict.Add("CP_L_PULL", GetWoWStringName(CurrentBinds[ControllerButton.TriggerLeft2]));
            bindDict.Add("CP_R_PULL", GetWoWStringName(CurrentBinds[ControllerButton.TriggerRight2]));
            bindDict.Add("CP_TL1", GetWoWStringName(CurrentBinds[ControllerButton.TriggerLeft]));
            bindDict.Add("CP_TL2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerLeft]));
            bindDict.Add("CP_M1", GetWoWStringName(CurrentBinds[ControllerButton.TriggerRight]));
            bindDict.Add("CP_M2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerRight]));

            var mod1 = CurrentBinds.First(bind => bind.Value == Key.LeftCtrl);
            var mod2 = CurrentBinds.First(bind => bind.Value == Key.LeftShift);

            if ((mod1.Key == ControllerButton.ShoulderLeft && mod2.Key == ControllerButton.TriggerLeft) ||
                (mod1.Key == ControllerButton.TriggerLeft && mod2.Key == ControllerButton.ShoulderLeft))
            {
                // Trigger buttons
                bindDict.Add("CP_T1", GetWoWStringName(CurrentBinds[ControllerButton.ShoulderRight]));
                bindDict.Add("CP_T2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerRight]));

                // Trigger textures
                bindDict.Add("T_T1", GetTextureName(ControllerButton.ShoulderRight));
                bindDict.Add("T_T2", GetTextureName(ControllerButton.TriggerRight));

            } else if (mod1.Key == ControllerButton.ShoulderRight && mod2.Key == ControllerButton.TriggerRight ||
                (mod1.Key == ControllerButton.TriggerRight && mod2.Key == ControllerButton.ShoulderRight))
            {
                // Trigger buttons
                bindDict.Add("CP_T1", GetWoWStringName(CurrentBinds[ControllerButton.ShoulderLeft]));
                bindDict.Add("CP_T2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerLeft]));

                // Trigger textures
                bindDict.Add("T_T1", GetTextureName(ControllerButton.ShoulderLeft));
                bindDict.Add("T_T2", GetTextureName(ControllerButton.TriggerLeft));
            }
            else if (mod1.Key == ControllerButton.ShoulderLeft && mod2.Key == ControllerButton.TriggerRight ||
                (mod1.Key == ControllerButton.TriggerRight && mod2.Key == ControllerButton.ShoulderLeft))
            {
                // Trigger buttons
                bindDict.Add("CP_T1", GetWoWStringName(CurrentBinds[ControllerButton.ShoulderRight]));
                bindDict.Add("CP_T2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerLeft]));

                // Trigger textures
                bindDict.Add("T_T1", GetTextureName(ControllerButton.ShoulderRight));
                bindDict.Add("T_T2", GetTextureName(ControllerButton.TriggerLeft));
            }
            else if (mod1.Key == ControllerButton.ShoulderRight && mod2.Key == ControllerButton.TriggerLeft ||
                (mod1.Key == ControllerButton.TriggerLeft && mod2.Key == ControllerButton.ShoulderLeft))
            {
                // Trigger buttons
                bindDict.Add("CP_T1", GetWoWStringName(CurrentBinds[ControllerButton.ShoulderLeft]));
                bindDict.Add("CP_T2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerRight]));

                // Trigger textures
                bindDict.Add("T_T1", GetTextureName(ControllerButton.ShoulderLeft));
                bindDict.Add("T_T2", GetTextureName(ControllerButton.TriggerRight));
            }
            else if (mod1.Key == ControllerButton.TriggerLeft && mod2.Key == ControllerButton.TriggerRight ||
                (mod1.Key == ControllerButton.TriggerRight && mod2.Key == ControllerButton.TriggerLeft))
            {
                // Trigger buttons
                bindDict.Add("CP_T1", GetWoWStringName(CurrentBinds[ControllerButton.ShoulderLeft]));
                bindDict.Add("CP_T2", GetWoWStringName(CurrentBinds[ControllerButton.ShoulderRight]));

                // Trigger textures
                bindDict.Add("T_T1", GetTextureName(ControllerButton.ShoulderLeft));
                bindDict.Add("T_T2", GetTextureName(ControllerButton.ShoulderRight));
            }
            else if (mod1.Key == ControllerButton.ShoulderLeft && mod2.Key == ControllerButton.ShoulderRight ||
                (mod1.Key == ControllerButton.ShoulderRight && mod2.Key == ControllerButton.ShoulderLeft))
            {
                // Trigger buttons
                bindDict.Add("CP_T1", GetWoWStringName(CurrentBinds[ControllerButton.TriggerLeft]));
                bindDict.Add("CP_T2", GetWoWStringName(CurrentBinds[ControllerButton.TriggerRight]));

                // Trigger textures
                bindDict.Add("T_T1", GetTextureName(ControllerButton.TriggerLeft));
                bindDict.Add("T_T2", GetTextureName(ControllerButton.TriggerRight));
            }

            // Modifier textures
            bindDict.Add("T_M1", GetTextureName(CurrentBinds.First(bind => bind.Value == Key.LeftCtrl).Key));
            bindDict.Add("T_M2", GetTextureName(CurrentBinds.First(bind => bind.Value == Key.LeftShift).Key));


            return bindDict;
        }


        public static string GetTextureName(ControllerButton button)
        {
            switch (button)
            {
                case ControllerButton.ShoulderLeft:
                    return "CP_TL1";
                case ControllerButton.ShoulderRight:
                    return "CP_TR1";
                case ControllerButton.TriggerLeft:
                    return "CP_TL2";
                case ControllerButton.TriggerRight:
                    return "CP_TR2";
            }
            return string.Empty;
        }

        public static Dictionary<Key, string> WowKeysDictionary = new Dictionary<Key, string>()
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
            {Key.Decimal, "NUMPADDECIMAL" },
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
            {Key.OemMinus, "-" },
            {Key.OemPlus, "=" },
            {Key.OemBackslash, "\\"},
            {Key.OemTilde, "`"},
            {Key.OemComma, ","},
            {Key.OemOpenBrackets, "["},
            {Key.OemCloseBrackets, "]"},
            {Key.OemSemicolon, ";"},
            {Key.OemQuestion, "/" },
            {Key.OemQuotes, "\"" },
        };

        public static string GetWoWStringName(Key key)
        {
            try
            {
                return WowKeysDictionary[key];
            }
            catch
            {
                return key.ToString().ToUpper();
            }
        }

        public static List<KeybindListItem> GetKeybindList(ControllerType controllerType)
        {
            if (_keybind == null) return null;

            switch (controllerType)
            {
                case ControllerType.DualShock4:
                    return GetKeybindDS4();
                case ControllerType.Xbox:
                    return GetKeybindXbox();
            }

            return null;
        }

        public static void LoadKeybinds()
        {
            if (File.Exists(Path.Combine(MainWindow.AppDataDir, "bindings.dat")))
            {
                try
                {
                    using (var f = new FileStream(Path.Combine(MainWindow.AppDataDir, "bindings.dat"), FileMode.Open))
                    {
                        Logger.Write("Loading keybind profile");
                        var x = new DataContractSerializer(typeof (KeybindProfile));
                        _keybind = (KeybindProfile) x.ReadObject(f);
                    }
                }
                catch
                {
                    Logger.Write("No profile found. Loading default keybinds.");
                    _keybind = new KeybindProfile {Keybinds = ControllerData.DefaultBinds};
                }
            }
            else
            {
                _keybind = new KeybindProfile {Keybinds = ControllerData.DefaultBinds};
            }
            BindingsUpdated?.Invoke();
        }

        public static void SaveKeybinds()
        {
            using (var outfile = new FileStream(Path.Combine(MainWindow.AppDataDir, "bindings.dat"), FileMode.Create))
            {
                var x = new DataContractSerializer(typeof (KeybindProfile));
                x.WriteObject(outfile, _keybind);
                var consolePortLuaFile = Path.Combine(Settings.Default.WoWFolder, "Interface\\AddOns\\ConsolePort\\Controllers\\WoWmapper.lua");
                if(File.Exists(consolePortLuaFile))
                    ConsolePortBindWriter.WriteBindFile(consolePortLuaFile);
            }

            
        }

        private static List<KeybindListItem> GetKeybindDS4()
        {
            var bindList = new List<KeybindListItem>();
            foreach (var bind in _keybind.Keybinds)
            {
                if (!ControllerData.DS4.ValidButtons.Contains(bind.Key)) continue;

                BitmapImage displayImage =
                    new BitmapImage(new Uri($"pack://application:,,,/{ControllerData.DS4.ButtonIcons[bind.Key]}"));
                string displayKey = bind.Value.ToString();
                string displayButton = bind.Key.ToString();

                try
                {
                    displayButton = ControllerData.DS4.ButtonNames[bind.Key];
                }
                catch
                {
                }

                bindList.Add(new KeybindListItem()
                {
                    DisplayImage = displayImage,
                    DisplayKey = displayKey,
                    DisplayButton = displayButton,
                    BindingKey = bind.Key
                });
            }
            return bindList;
        }


        public static KeybindProfile ReadFile(string Filename)
        {
            if (!File.Exists(Filename)) return null;

            try
            {
                using (FileStream xmlStream = new FileStream(Filename, FileMode.Open))
                {
                    var x = new DataContractSerializer(typeof (KeybindProfile));
                    return (KeybindProfile) x.ReadObject(xmlStream);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"Attempt to import keybinds failed: {ex}");
            }

            return null;
        }

        public static bool WriteFile(KeybindProfile Profile, string Filename)
        {
            try
            {
                using (var outfile = new FileStream(Filename, FileMode.Create))
                {
                    var x = new DataContractSerializer(typeof (KeybindProfile));
                    x.WriteObject(outfile, Profile);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private static List<KeybindListItem> GetKeybindXbox()
        {
            var bindList = new List<KeybindListItem>();
            foreach (var bind in _keybind.Keybinds)
            {
                if (!ControllerData.Xbox.ValidButtons.Contains(bind.Key)) continue;

                BitmapImage displayImage =
                    new BitmapImage(new Uri($"pack://application:,,,/{ControllerData.Xbox.ButtonIcons[bind.Key]}"));
                string displayKey = bind.Value.ToString();
                string displayButton = bind.Key.ToString();

                try
                {
                    displayButton = ControllerData.Xbox.ButtonNames[bind.Key];
                }
                catch
                {
                }

                bindList.Add(new KeybindListItem()
                {
                    DisplayImage = displayImage,
                    DisplayKey = displayKey,
                    DisplayButton = displayButton,
                    BindingKey = bind.Key
                });
            }
            return bindList;
        }

        public static void ResetBinds()
        {
            _keybind = new KeybindProfile();
            _keybind.Keybinds = new Dictionary<ControllerButton, Key>(ControllerData.DefaultBinds);
            SaveKeybinds();
            BindingsUpdated?.Invoke();
        }
    }


    public class KeybindListItem
    {
        public BitmapImage DisplayImage { get; set; }
        public string DisplayButton { get; set; }
        public string DisplayKey { get; set; }
        public ControllerButton BindingKey { get; set; }
    }


    [DataContract]
    public class KeybindProfile
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Dictionary<ControllerButton, Key> Keybinds { get; set; }
    }
}