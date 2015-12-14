using WoWmapper.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WoWmapper
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