using WoWmapper.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;

namespace WoWmapper.Input
{
    [XmlType("Settings")]
    public class SettingsFile
    {
        [XmlAttribute("Version")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("Keybinds")]
        public Keybind Keybinds { get; set; } = new Keybind();

        [XmlElement("Settings")]
        public PluginSettings Settings { get; set; } = new PluginSettings();

        private string fileName { get; set; } = "default.xml";

        public SettingsFile()
        {

        }

        public SettingsFile(string FileName)
        {
            fileName = FileName;
        }



        /// <summary>
        /// Write the plugin settings to file
        /// </summary>
        public void Save()
        {
            using (FileStream f = new FileStream(fileName + ".xml", FileMode.Create))
            {
                XmlSerializer x = new XmlSerializer(typeof(SettingsFile));
                x.Serialize(f, this);
            }
        }

        /// <summary>
        /// Load the plugin settings from file, or defaults if no file exists
        /// </summary>
        public void Load()
        {
            try
            {
                using (FileStream f = new FileStream(fileName + ".xml", FileMode.Open))
                {
                    XmlSerializer x = new XmlSerializer(typeof(SettingsFile));
                    var s = (SettingsFile)x.Deserialize(f);
                    this.Version = s.Version;
                    this.Keybinds = s.Keybinds;
                    this.Settings = s.Settings;
                }
            }
            catch
            {
                this.Version = "0.0.0.0";
                this.Keybinds.Bindings = Defaults.Bindings;
            }
        }
    }

    public class Keybind
    {
        [XmlArray("Bindings")]
        public List<Binding> Bindings = new List<Binding>();

        public Binding FromName(string Name)
        {
            return Bindings.FirstOrDefault(bind => bind.Name == Name);
        }

        public Binding FromButton(InputButton Button)
        {
            return Bindings.FirstOrDefault(bind => bind.Button == Button);
        }

        public Binding FromKey(Keys Key)
        {
            return Bindings.FirstOrDefault(bind => bind.Key == Key);
        }

        public void Add(string Name, Keys? Key, InputButton? Button = null)
        {
            Bindings.Add(new Binding() { Name = Name, Button = Button, Key = Key });
        }

        public void Update(string Name, Keys Key, InputButton Button)
        {
            var bind = Bindings.IndexOf(Bindings.First(b => b.Name == Name));
            var bb = Bindings[bind];
            bb.Button = Button;
            bb.Key = Key;
            Bindings[bind] = bb;
        }

        public void Update(string Name, Keys Key)
        {
            var bind = Bindings.IndexOf(Bindings.First(b => b.Name == Name));
            var bb = Bindings[bind];
            bb.Key = Key;
            Bindings[bind] = bb;
        }

        public void Update(string Name, InputButton Button)
        {
            var bind = Bindings.IndexOf(Bindings.First(b => b.Name == Name));
            var bb = Bindings[bind];
            bb.Button = Button;
            Bindings[bind] = bb;
        }
    }

    public struct Binding
    {
        public Binding(string aName, InputButton? aButton, Keys? aKey)
        {
            Name = aName;
            Button = aButton;
            Key = aKey;
        }

        [XmlAttribute("Name")]
        public string Name;

        [XmlElement("Button")]
        public InputButton? Button;

        [XmlElement("Key")]
        public Keys? Key;
    }

    public class PluginSettings
    {
        [XmlArray("Setting")]
        public List<Setting> Settings { get; set; } = new List<Setting>();

        /// <summary>
        /// Attempt to read a setting from the settings file
        /// </summary>
        /// <typeparam name="T">The return type of the setting</typeparam>
        /// <param name="SettingName">The name of the setting to read</param>
        /// <param name="Value">The value of the Setting object</param>
        /// <returns>True if the lookup was successful, otherwise false</returns>
        public bool Read<T>(string SettingName, out T Value)
        {
            var setting = Settings.FirstOrDefault(set => set.Name == SettingName);
            if(!setting.Equals(default(Setting)))
            {
                Value = (T)setting.Value;
                return true;
            }
            Value = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SettingName"></param>
        /// <param name="Value"></param>
        public bool Write<T>(string SettingName, T Value)
        {
            var setting = Settings.FirstOrDefault(set => set.Name == SettingName);
            var settingIndex = Settings.IndexOf(setting);

            if (settingIndex != -1)
            {
                Settings[settingIndex] = new Setting()
                {
                    Name = SettingName,
                    Value = Value
                };
                
                return true;
            } else
            {
                Settings.Add(new Setting()
                {
                    Name = SettingName,
                    Value = Value
                });
            }

            return false;
        }

    }

    public struct Setting
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement("Value")]
        public object Value { get; set; }
    }

    public static class Defaults
    {
        public static List<Binding> Bindings = new List<Binding>()
        {
            // TODO fix request unknown bindings
            new Binding("LStickUp", null, Keys.W),
            new Binding("LStickDown", null, Keys.S),
            new Binding("LStickLeft", null, Keys.A),
            new Binding("LStickRight",null, Keys.D),
            new Binding("RFaceDown", InputButton.RFaceDown, Keys.F11),
            new Binding("RFaceUp", InputButton.RFaceUp, Keys.F9),
            new Binding("RFaceLeft", InputButton.RFaceLeft, Keys.F12),
            new Binding("RFaceRight", InputButton.RFaceRight, Keys.F10),
            new Binding("LFaceUp", InputButton.LFaceUp, Keys.F1),
            new Binding("LFaceDown", InputButton.LFaceDown, Keys.F3),
            new Binding("LFaceLeft", InputButton.LFaceLeft, Keys.F2),
            new Binding("LFaceRight", InputButton.LFaceRight, Keys.F4),
            new Binding("BumperLeft", InputButton.BumperLeft, Keys.LShiftKey),
            new Binding("BumperRight", InputButton.BumperRight, Keys.F8),
            new Binding("TriggerLeft", InputButton.TriggerLeft, Keys.LControlKey),
            new Binding("TriggerRight", InputButton.TriggerRight, Keys.F7),
            new Binding("CenterLeft", InputButton.CenterLeft, Keys.F6),
            new Binding("CenterRight", InputButton.CenterRight, Keys.F5),
            new Binding("CenterMiddle", InputButton.CenterMiddle, Keys.Multiply),
            new Binding("Extra1", InputButton.Extra1, Keys.Subtract),
            new Binding("Extra2", InputButton.Extra2, Keys.Add)
        };

        public static List<Keys> IllegalKeyCodes = new List<Keys>()
        {
            Keys.Escape,
            Keys.Alt,
            Keys.Shift,
            Keys.ControlKey,
            Keys.ShiftKey,
            Keys.LShiftKey,
            Keys.RShiftKey,
            Keys.Control,
            Keys.LControlKey,
            Keys.RControlKey,
            Keys.LWin, Keys.RWin,
            Keys.Menu
        };
    }
}