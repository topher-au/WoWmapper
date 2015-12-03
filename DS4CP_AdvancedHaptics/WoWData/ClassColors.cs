using DS4ConsolePort.WoWData;
using System.Drawing;

namespace DS4ConsolePort.WoWData
{
    public static class ClassColors
    {
        public static Color Shaman = Color.Blue;
        public static Color Mage = Color.Teal;
        public static Color Hunter = Color.Green;
        public static Color Druid = Color.Orange;
        public static Color DeathKnight = Color.DarkRed;
        public static Color Monk = Color.LimeGreen;
        public static Color Priest = Color.White;
        public static Color Warrior = Color.DarkGoldenrod;
        public static Color Paladin = Color.LightPink;
        public static Color Warlock = Color.Purple;
        public static Color Rogue = Color.Yellow;

        public static Color GetClassColor(WoWClass Class)
        {
            switch ((int)Class)
            {
                case 1: // warrior
                    return Warrior;

                case 2: // paladin
                    return Paladin;

                case 3: // hunter
                    return Hunter;

                case 4: // rogue
                    return Rogue;

                case 5: // priest
                    return Priest;

                case 6: // death knight
                    return DeathKnight;

                case 7: // shaman
                    return Shaman;

                case 8: // mage
                    return Mage;

                case 9: // warlock
                    return Warlock;

                case 11: // druid
                    return Druid;

                default: // other
                    return Color.Green;
            }
        }
    }
}