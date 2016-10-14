using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using System.Windows.Media.Animation;
using MahApps.Metro;
using WoWmapper.Controllers;
using WoWmapper.Keybindings;
using WoWmapper.Overlay;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;

namespace WoWmapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static WMOverlay Overlay = new WMOverlay();

        private const string DefaultTheme = "BaseLight";
        private const string DefaultAccent = "Purple";

        private readonly Guid _appGuid = Assembly.GetExecutingAssembly().GetType().GUID;
        private static Mutex _mutex;

        private static readonly NotifyIcon _notifyIcon = new NotifyIcon();
        private static readonly MenuItem _notifyMenuOpen = new MenuItem {Header = "WoWmapper"};
        private static readonly MenuItem _notifyMenuExit = new MenuItem {Header = "Exit"};

        public readonly string[] Args;

        public App()
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            commandLineArgs.ToList().RemoveAt(0);
            Args = commandLineArgs.ToArray();

            Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            Settings.Default.PropertyChanged += (sender, args) =>
            {
                Settings.Default.Save();
            };
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.WriteLine($"Exception occurred: {e.Exception}\n" +
                            "Stack trace:\n" + e.Exception.StackTrace);
            MessageBox.Show($"An exception occurred:\n{e.Exception}\n\nSee the log file for more details.", "Exception occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(64);
        }

        private static readonly ContextMenu _notifyMenu = new ContextMenu()
        {
            Items =
            {
                _notifyMenuOpen,
                new Separator(),
                _notifyMenuExit
            }
        };

        private readonly string[] _deprecatedFiles = new string[]
        {
            "Updater.exe",
            "Xceed.Wpf.Toolkit.dll",
            "offsets.dat",
            "bindings.dat"
        };

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!Args.Contains("--ignore-mutex")) // parameter to ignore existing instances
            {
                // Check if an existing instance of the application is running
                _mutex = new Mutex(false, "Global\\" + _appGuid);
                if (!_mutex.WaitOne(0, false))
                {
                    // Instance already running
                    MessageBox.Show("Another instance of WoWmapper is already open", "Already running", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Environment.Exit(0);
                    return;
                }
            }

            SetTheme();
            
            // Start up threads
            BindManager.LoadBindings();
            ControllerManager.Start();
            ProcessManager.Start();
            InputMapper.Start();

            if(Settings.Default.EnableOverlay) Overlay.Start();

            base.OnStartup(e);

            // Create system tray icon
            _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            _notifyIcon.Text = "WoWmapper";
            _notifyIcon.Visible = true;
            _notifyIcon.Click += (sender, args) =>
            {
                var me = (MouseEventArgs) args;
                if (me.Button == MouseButtons.Right)
                {
                    _notifyMenu.IsOpen = true;
                }
            };
            _notifyIcon.DoubleClick += (sender, args) => { Application.Current.MainWindow.Show(); };
            _notifyMenuOpen.Click += (sender, args) => { Application.Current.MainWindow.Show(); };
            _notifyMenuExit.Click += (sender, args) => { Application.Current.Shutdown(); };
        }

        public static void SetTheme()
        {
            // Apply Metro theme
            AppTheme theme;
            Accent accent;

            try
            {
                // Attempt to read theme settings
                accent = ThemeManager.GetAccent(WoWmapper.Properties.Settings.Default.AppAccent);
                theme = ThemeManager.GetAppTheme(WoWmapper.Properties.Settings.Default.AppTheme);
            }
            catch
            {
                // Failed to read settings, reset to default
                WoWmapper.Properties.Settings.Default.AppTheme = DefaultTheme;
                WoWmapper.Properties.Settings.Default.AppAccent = DefaultAccent;
                WoWmapper.Properties.Settings.Default.Save();

                accent = ThemeManager.GetAccent(DefaultTheme);
                theme = ThemeManager.GetAppTheme(DefaultAccent);
            }

            if (theme == null) theme = ThemeManager.GetAppTheme(DefaultTheme);
            if (accent == null) accent = ThemeManager.GetAccent(DefaultAccent);

            // Apply theme
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current, accent, theme);
            try
            {
                var acb = (SolidColorBrush) accent.Resources["AccentColorBrush"];
                Application.Current.MainWindow.BorderBrush = acb;
            }
            catch
            {
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Shut down threads
            ProcessManager.Stop();
            InputMapper.Stop();
            ControllerManager.Stop();
            Overlay.Stop();


            // Save settings
            WoWmapper.Properties.Settings.Default.Save();

            try
            {
                // Dispose mutex
                _mutex?.ReleaseMutex();
                _mutex?.Dispose();
            } catch { }

            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();

            // Exit application
            base.OnExit(e);
        }
    }
}