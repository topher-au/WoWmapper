using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WoWmapper
{
    internal static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", CharSet = CharSet.Auto)]
        public static extern int SendMessageTimeout(
            IntPtr hwnd,
            uint Msg,
            int wParam,
            int lParam,
            uint fuFlags,
            uint uTimeout,
            out int lpdwResult);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);

        public static readonly uint WM_ACTIVATE_DS4 = RegisterWindowMessage("WM_ACTIVATE_DS4");

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
                    Application.Exit();
                }
                else
                {
                    // Existing instance, send message
                    int res;
                    SendMessageTimeout((IntPtr)0xFFFF, WM_ACTIVATE_DS4, 0, 0, 0, 100, out res);
                    Application.Exit();
                }
            }
        }
    }
}