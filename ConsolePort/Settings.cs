using DS4Wrapper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ConsolePort
{
    [XmlType("Settings")]
    public class Settings
    {
        [XmlAttribute("Version")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("KeyBinds")]
        public KeyBind KeyBinds { get; set; } = new KeyBind();

        public void Save()
        {
            using (FileStream f = new FileStream("settings.xml", FileMode.Create))
            {
                XmlSerializer x = new XmlSerializer(typeof(Settings));
                x.Serialize(f, this);
            }
        }

        public void Load()
        {
            try
            {
                using (FileStream f = new FileStream("settings.xml", FileMode.Open))
                {
                    XmlSerializer x = new XmlSerializer(typeof(Settings));
                    var s = (Settings)x.Deserialize(f);
                    this.Version = s.Version;
                    this.KeyBinds = s.KeyBinds;
                }
            }
            catch
            {
                this.Version = "0.0.0.0";
                this.KeyBinds.Bindings = Defaults.Bindings;
            }
        }
    }

    public class KeyBind
    {
        [XmlArray("Bindings")]
        public List<Binding> Bindings = new List<Binding>();

        public Binding FromName(string Name)
        {
            return Bindings.FirstOrDefault(bind => bind.Name == Name);
        }

        public Binding FromButton(DS4Button Button)
        {
            return Bindings.FirstOrDefault(bind => bind.Button == Button);
        }

        public Binding FromKey(Keys Key)
        {
            return Bindings.FirstOrDefault(bind => bind.Key == Key);
        }

        public void Add(string Name, Keys? Key, DS4Button? Button = null)
        {
            Bindings.Add(new Binding() { Name = Name, Button = Button, Key = Key });
        }

        public void Update(string Name, Keys Key, DS4Button Button)
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

        public void Update(string Name, DS4Button Button)
        {
            var bind = Bindings.IndexOf(Bindings.First(b => b.Name == Name));
            var bb = Bindings[bind];
            bb.Button = Button;
            Bindings[bind] = bb;
        }
    }

    public struct Binding
    {
        public Binding(string aName, DS4Button? aButton, Keys? aKey)
        {
            Name = aName;
            Button = aButton;
            Key = aKey;
        }

        [XmlAttribute("Name")]
        public string Name;

        [XmlElement("Button")]
        public DS4Button? Button;

        [XmlElement("Key")]
        public Keys? Key;
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
            new Binding("Cross", DS4Button.Cross, Keys.F11),
            new Binding("Triangle", DS4Button.Triangle, Keys.F9),
            new Binding("Square", DS4Button.Square, Keys.F12),
            new Binding("Circle", DS4Button.Circle, Keys.F10),
            new Binding("DpadUp", DS4Button.DpadUp, Keys.F1),
            new Binding("DpadDown", DS4Button.DpadDown, Keys.F3),
            new Binding("DpadLeft", DS4Button.DpadLeft, Keys.F2),
            new Binding("DpadRight", DS4Button.DpadRight, Keys.F4),
            new Binding("L1", DS4Button.L1, Keys.F7),
            new Binding("R1", DS4Button.R1, Keys.F8),
            new Binding("L2", DS4Button.L2, Keys.LShiftKey),
            new Binding("R2", DS4Button.R2, Keys.LControlKey),
            new Binding("Share", DS4Button.Share, Keys.F6),
            new Binding("Options", DS4Button.Options, Keys.F5),
            new Binding("PS", DS4Button.PS, Keys.Multiply),
            new Binding("TouchLeft", DS4Button.TouchLeft, Keys.Subtract),
            new Binding("TouchRight", DS4Button.TouchRight, Keys.Add)
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