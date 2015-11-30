using System;
using System.Reflection;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (!Environment.Is64BitProcess)
            {
                // Incompatibility with x86
                MessageBox.Show("Sorry, this application can only be run in 64-bit mode.", "ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
            if (new Version(Properties.Settings.Default.SettingsVersion) < assemblyVersion)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.SettingsVersion = assemblyVersion.ToString();
                Properties.Settings.Default.Save();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}