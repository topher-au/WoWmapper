using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWmapper.Properties;

namespace WoWmapper.Classes
{
    public static class Logger
    {
        public static void Write(string text, params object[] parameters)
        {
            if(Settings.Default.EnableLogging)
                File.AppendAllLines("log.txt", new[] { string.Format(text, parameters) });
        }
    }
}
