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
        private const string DefaultTheme = "BaseLight";
        private const string DefaultAccent = "Purple";

        private readonly Guid _appGuid = Assembly.GetExecutingAssembly().GetType().GUID;
        private static Mutex _mutex;

        private static readonly NotifyIcon _notifyIcon = new NotifyIcon();
        private static readonly MenuItem _notifyMenuOpen = new MenuItem {Header = "WoWmapper"};
        private static readonly MenuItem _notifyMenuExit = new MenuItem {Header = "Exit"};
        
        public App()
        {
                
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
        
        protected override async void OnStartup(StartupEventArgs e)
        {
            
            // Check if an existing instance of the application is running
            _mutex = new Mutex(false, "Global\\" + _appGuid);
            if (!_mutex.WaitOne(0, false))
            {
                // Instance already running
                MessageBox.Show("Another instance of WoWmapper is already open", "Already running", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Current.Shutdown();
                return;
            }

            Current.DispatcherUnhandledException += (a, b) =>
            {
                MessageBox.Show("Whoops! Looks like something went wrong, sorry about that!\n\n" +
                                "Here's the error message:\n" +
                                b.Exception.Message + "\n" + 
                                b.Exception.StackTrace,
                                "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            base.OnStartup(e);

            WoWmapper.Properties.Settings.Default.PropertyChanged += (sender, args) =>
            {
                WoWmapper.Properties.Settings.Default.Save();
            };

            // Check application settings and upgrade
            var settingsVersion = new Version(WoWmapper.Properties.Settings.Default.SettingsVersion);
            if (Assembly.GetExecutingAssembly().GetName().Version > settingsVersion)
            {
                WoWmapper.Properties.Settings.Default.Upgrade();
                WoWmapper.Properties.Settings.Default.SettingsVersion =
                    Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

            SetTheme();
            
            // Start up threads
            BindManager.LoadBindings();
            ControllerManager.Start();

            ProcessManager.Start();
            InputMapper.Start();

            // Scan for controllers
            ControllerManager.RefreshControllers();

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