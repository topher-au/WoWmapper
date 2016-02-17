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
        public static StreamWriter _outWriter = new StreamWriter("log.txt");

        public static void Write(string text, params object[] parameters)
        {
            Console.WriteLine(text, parameters);
            if(Settings.Default.EnableLogging)
                _outWriter.WriteLine(text, parameters);
        }
    }
}
