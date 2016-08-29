using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using WoWmapper.ConsolePort;
using WoWmapper.Controllers;
using WoWmapper.SettingsPanels;

namespace WoWmapper.Keybindings
{
    public static class BindManager
    {
        public static event EventHandler BindingsChanged;
        public static readonly string KeybindFile = "keybinds.xml";

        public static List<Keybind> CurrentKeybinds = new List<Keybind>();

        public static Key GetKey(GamepadButton button)
        {
            try
            {
                if (Properties.Settings.Default.CustomBindings)
                {
                    return CurrentKeybinds.First(
                        bind => bind.BindType == button).Key;
                }
                else
                {
                    return
                        Defaults.KeybindStyles.GetDefault(Properties.Settings.Default.ModifierStyle).First(
                            bind => bind.BindType == button).Key;
                }
            }
            catch
            {
                return Key.None;
            }
        }

        public static void SetKey(GamepadButton button, Key key)
        {
            var binding = CurrentKeybinds.First(bind => bind.BindType == button);
            binding.Key = key;

            SaveBindings();
            BindingsChanged?.Invoke(null, EventArgs.Empty);

            Properties.Settings.Default.BindingsModified = DateTime.Now;
            BindWriter.WriteBinds();
        }

        public static void LoadBindings()
        {
            if (!File.Exists(KeybindFile)) ResetDefaults(0);

            // Load keybinds from file
            var xml = new XmlSerializer(typeof (List<Keybind>));

            using (var fs = new FileStream(KeybindFile, FileMode.Open))
                CurrentKeybinds = (List<Keybind>) xml.Deserialize(fs);
        }

        public static void SaveBindings()
        {
            // Save keybinds to file
            var xml = new XmlSerializer(typeof (List<Keybind>));
            using (var fs = new FileStream(KeybindFile, FileMode.Create))
                xml.Serialize(fs, CurrentKeybinds);
        }

        public static void ResetDefaults(int profile)
        {
            CurrentKeybinds.Clear();
            CurrentKeybinds.AddRange(Defaults.KeybindStyles.GetDefault(Properties.Settings.Default.ModifierStyle));
            
            Properties.Settings.Default.ModifierStyle = profile;
            SaveBindings();
            BindingsChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}