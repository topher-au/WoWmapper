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
using System.Windows;
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
                    _keybind = new KeybindProfile { Keybinds = ControllerData.DefaultBinds };
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
            using (var outfile = new FileStream(Path.Combine(MainWindow.AppDataDir,"bindings.dat"), FileMode.Create))
            {
                var x = new DataContractSerializer(typeof(KeybindProfile));
                x.WriteObject(outfile, _keybind);
            }
        }

        private static List<KeybindListItem> GetKeybindDS4()
        {
            var bindList = new List<KeybindListItem>();
            foreach (var bind in _keybind.Keybinds)
            {
                if (!ControllerData.DS4.ValidButtons.Contains(bind.Key)) continue;

                BitmapImage displayImage = new BitmapImage(new Uri($"pack://application:,,,/{ControllerData.DS4.ButtonIcons[bind.Key]}"));
                string displayKey = bind.Value.ToString();
                string displayButton = bind.Key.ToString();

                try
                {
                    displayButton = ControllerData.DS4.ButtonNames[bind.Key];
                }
                catch { }

                bindList.Add(new KeybindListItem()
                {
                    DisplayImage = displayImage, DisplayKey = displayKey, DisplayButton = displayButton, BindingKey = bind.Key
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
                    var x = new DataContractSerializer(typeof(KeybindProfile));
                    return (KeybindProfile)x.ReadObject(xmlStream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            return null;
        }

        public static bool WriteFile(KeybindProfile Profile, string Filename)
        {
            try
            {
                using (var outfile = new FileStream(Filename, FileMode.Create))
                {
                    var x = new DataContractSerializer(typeof(KeybindProfile));
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

                BitmapImage displayImage = new BitmapImage(new Uri($"pack://application:,,,/{ControllerData.Xbox.ButtonIcons[bind.Key]}"));
                string displayKey = bind.Value.ToString();
                string displayButton = bind.Key.ToString();

                try
                {
                    displayButton = ControllerData.Xbox.ButtonNames[bind.Key];
                }
                catch { }

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
