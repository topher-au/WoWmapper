using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WoWmapper.WorldOfWarcraft
{
    public class GameInfo
    {
        public enum Classes
        {
            None = 0,
            Warrior = 1,
            Paladin = 2,
            Hunter = 3,
            Rogue = 4,
            Priest = 5,
            DeathKnight = 6,
            Shaman = 7,
            Mage = 8,
            Warlock = 9,
            Druid = 11,
            Monk = 12,
            DemonHunter = 13
        }

        public enum GameState
        {
            LoggedOut = 0x00,
            LoggedIn = 0x01,
            NotRunning = 0xFF
        }

        public static Dictionary<Classes, Color> RaidClassColors = new Dictionary<Classes, Color>()
        {
            { Classes.Warrior, Color.FromArgb(a:0xff, r: 0xc7, g: 0x9c, b: 0x6e) },
            { Classes.Paladin, Color.FromArgb(a:0xff, r: 0xf5, g: 0x8c, b: 0xba) },
            { Classes.Hunter, Color.FromArgb(a:0xff, r: 0xab, g: 0xd4, b: 0x73) },
            { Classes.Rogue, Color.FromArgb(a:0xff, r: 0xff, g: 0xf5, b: 0x69) },
            { Classes.Priest, Color.FromArgb(a:0xff, r: 0xff, g: 0xff, b: 0xff) },
            { Classes.DeathKnight, Color.FromArgb(a:0xff, r: 0xc4, g: 0x1f, b: 0x3b) },
            { Classes.Shaman, Color.FromArgb(a:0xff, r: 0x00, g: 0x70, b: 0xde) },
            { Classes.Mage, Color.FromArgb(a:0xff, r: 0x69, g: 0xcc, b: 0xf0) },
            { Classes.Warlock, Color.FromArgb(a:0xff, r: 0x94, g: 0x82, b: 0xc9) },
            { Classes.Druid, Color.FromArgb(a:0xff, r: 0xff, g: 0x7d, b: 0x0a) },
            { Classes.Monk, Color.FromArgb(a:0xff, r: 0x00, g: 0xff, b: 0x96) },
        };
    }
}
