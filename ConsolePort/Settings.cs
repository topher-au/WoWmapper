using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS4Wrapper;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

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
            using (FileStream f = new FileStream("test.xml", FileMode.Create))
            {
                XmlSerializer x = new XmlSerializer(typeof(Settings));
                x.Serialize(f, this);
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

        public void Add(string Name, Keys? Key, DS4Button? Button = null)
        {
            Bindings.Add(new Binding() { Name = Name, Button = Button, Key = Key });
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
            new Binding("MoveForward", null, Keys.W),
            new Binding("MoveBackward", null, Keys.S),
            new Binding("MoveLeft", null, Keys.A),
            new Binding("MoveRight",null, Keys.D),
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
        };
    }
}
