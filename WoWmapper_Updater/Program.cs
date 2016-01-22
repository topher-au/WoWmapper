using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWmapper_Updater
{
    public static class Program
    {
        public static string UpdateFile = string.Empty;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0) return;

            UpdateFile = args.FirstOrDefault(arg => File.Exists(arg.Trim('\"')))?.Trim('\"');

            if (UpdateFile == string.Empty) return;
            //MessageBox.Show(UpdateFile);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
