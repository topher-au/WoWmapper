using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WoWmapper.Properties;

namespace WoWmapper
{
    public static class Log
    {
        private static StreamWriter _file = new StreamWriter(new FileStream("log.txt", FileMode.Create)) { AutoFlush=true};

        public static void WriteLine(string text, params string[] args)
        {
            if (Settings.Default.EnableLogging)
            {
                lock(_file)
                    _file.WriteLine($"[{DateTime.Now.ToString("T")}] {text}", args);
            }

            Console.WriteLine($"[{DateTime.Now.ToString("T")}] {text}", args);
        }
    }
}
