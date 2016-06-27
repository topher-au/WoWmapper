using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WoWmapper.WorldOfWarcraft.AddOns
{
    public class ConsolePortBindWriter
    {
        public static void WriteBindFile(string fileName)
        {
            var templateStream = Application.GetResourceStream(new Uri("pack://application:,,,/WorldOfWarcraft/AddOns/WoWmapper.lua"));
            var textTemplate = new StreamReader(templateStream.Stream);

            while (!textTemplate.EndOfStream)
            {
                var textLine = textTemplate.ReadLine();
            }
        }
    }
}
