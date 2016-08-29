using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using WoWmapper.Controllers;

namespace WoWmapper.Keybindings
{
    [XmlType("Keybind")]
    public class Keybind
    {
        [XmlAttribute("BindType")]
        public GamepadButton BindType { get; set; }

        [XmlAttribute("Key")]
        public Key Key { get; set; }
    }
}