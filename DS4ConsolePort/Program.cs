using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    internal static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        private static extern int RegisterWindowMessage(string message);

        public static readonly int WM_ACTIVATE_DS4 = RegisterWindowMessage("WM_ACTIVATE_DS4");

        [STAThread]
        private static void Main()
        {
            bool createdNew = false;

            using (Mutex ds4Mutex = new Mutex(true, "{7522ee2c-3977-4ffa-b6aa-4990d4d0bf5f}", out createdNew))
            {
                if (createdNew) // New instance launched
                {
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
                    ds4Mutex.ReleaseMutex();
                }
                else
                {
                    // Existing instance, send message
                    SendMessage(
                        (IntPtr)0xFFFF, // HWND_BROADCAST
                        WM_ACTIVATE_DS4,
                        IntPtr.Zero,
                        IntPtr.Zero);
                }
            }
        }
    }
}