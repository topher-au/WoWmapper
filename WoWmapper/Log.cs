using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper
{
    public static class Log
    {
        private static StreamWriter _file = new StreamWriter(new FileStream("log.txt", FileMode.Create)) { AutoFlush=true};

        public static async void WriteLine(string text, params string[] args)
        {
            _file.WriteLine($"[{DateTime.Now.ToString("T")}] {text}", args);

            Console.WriteLine($"[{DateTime.Now.ToString("T")}] {text}", args);
        }
    }
}
