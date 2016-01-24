using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Octokit;
using WoWmapper.Classes;
using WoWmapper.Controllers;
using WoWmapper.Input;
using WoWmapper.Options;
using WoWmapper.WorldOfWarcraft;
using WoWmapper.Properties;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MenuItem = System.Windows.Controls.MenuItem;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Timer = System.Timers.Timer;

namespace WoWmapper
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static event WoWmapperEvent UpdateContent;

        public static event WoWmapperEvent WindowClosing;

        public static event ShowKeybindHandler ShowKeybindDialogEvent;

        public static event ShowMessageHandler ShowMessage;

        public delegate void ShowMessageHandler(string Title, string Text);
        public delegate void ShowKeybindHandler(ControllerButton Button);
        public delegate void WoWmapperEvent();
        private readonly NotifyIcon _notifyIcon;

        private Release cpRelease;
        private Release wmRelease;

        public static void UpdateChildren()
        {
            UpdateContent?.Invoke();
        }

        private readonly ContextMenu _notifyMenu = new ContextMenu
        {
            Items =
            {
                new MenuItem
                {
                    Header = Properties.Resources.NotifyMenuOpenWowmapper
                },
                new Separator(),
                new MenuItem
                {
                    Header = Properties.Resources.NotifyMenuControllers
                },
                new MenuItem
                {
                    Header = Properties.Resources.NotifyMenuKeybinds
                },
                new Separator(),
                new MenuItem
                {
                    Header = Properties.Resources.NotifyMenuExit
                }
            }
        };

        private readonly OptionsFlyout _optionsFlyout = new OptionsFlyout();
        private readonly Timer _timerUpdateUi = new Timer();
        private ProgressDialogController _bindDialog;

        public MainWindow()
        {
            InitializeComponent();

            Logger.Write("------------------------------------------");
            Logger.Write("WoWmapper {0} initializing...", Assembly.GetExecutingAssembly().GetName().Version);
            Logger.Write("Operating System: Windows {0}, {1}", Environment.OSVersion, Environment.Is64BitOperatingSystem ? "x64" : "x86");

            // Add notification menu items
            ((MenuItem)_notifyMenu.Items[0]).Click += NotifyMenu_Open_WoWmapper;
            ((MenuItem)_notifyMenu.Items[2]).Click += NotifyMenu_Controllers;
            ((MenuItem)_notifyMenu.Items[3]).Click += NotifyMenu_Keybinds;
            ((MenuItem)_notifyMenu.Items[5]).Click += NotifyMenu_Exit;
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Click += NotifyIcon_Click;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.Visible = true;
            _notifyIcon.Icon = Properties.Resources.WoWmapper_Icon;


            // Add custom accents
            ThemeManager.AddAccent("DeathKnight", new Uri("pack://application:,,,/Resources/Accents/DeathKnight.xaml"));
            ThemeManager.AddAccent("Druid", new Uri("pack://application:,,,/Resources/Accents/Druid.xaml"));
            ThemeManager.AddAccent("Hunter", new Uri("pack://application:,,,/Resources/Accents/Hunter.xaml"));
            ThemeManager.AddAccent("Mage", new Uri("pack://application:,,,/Resources/Accents/Mage.xaml"));
            ThemeManager.AddAccent("Monk", new Uri("pack://application:,,,/Resources/Accents/Monk.xaml"));
            ThemeManager.AddAccent("Paladin", new Uri("pack://application:,,,/Resources/Accents/Paladin.xaml"));
            ThemeManager.AddAccent("Priest", new Uri("pack://application:,,,/Resources/Accents/Priest.xaml"));
            ThemeManager.AddAccent("Rogue", new Uri("pack://application:,,,/Resources/Accents/Rogue.xaml"));
            ThemeManager.AddAccent("Shaman", new Uri("pack://application:,,,/Resources/Accents/Shaman.xaml"));
            ThemeManager.AddAccent("Warlock", new Uri("pack://application:,,,/Resources/Accents/Warlock.xaml"));
            ThemeManager.AddAccent("Warrior", new Uri("pack://application:,,,/Resources/Accents/Warrior.xaml"));

            // Load theme and accent
            var appTheme = ThemeManager.AppThemes.FirstOrDefault(theme => theme.Name == Settings.Default.AppTheme);
            var appAccent = ThemeManager.Accents.FirstOrDefault(accent => accent.Name == Settings.Default.AppAccent);

            // Load defaults if invalid
            if (appTheme == null)
            {
                appTheme = ThemeManager.GetAppTheme("BaseLight");
                Settings.Default.AppTheme = "BaseLight";
                Settings.Default.Save();
            }
            if (appAccent == null)
            {
                appAccent = ThemeManager.GetAccent("Blue");
                Settings.Default.AppAccent = "Blue";
                Settings.Default.Save();
            }

            // Set drop shadow
            if (Settings.Default.AppDropShadow)
            {
                BorderThickness = new Thickness(0);
                GlowBrush = Brushes.Black;
            }

            // Apply theme
            ThemeManager.ChangeAppStyle(Application.Current, appAccent, appTheme);

            // Hook events
            AdvancedSettings.ShowFeedbackWarning += AdvancedSettings_ShowFeedbackWarning;
            AdvancedSettings.DoResetAll += AdvancedSettings_DoResetAll;
            ShowMessage += MainWindow_ShowMessage;
            ShowKeybindDialogEvent += OnShowKeybindDialog;

            // Initialize UI timer
            _timerUpdateUi.Elapsed += Timer_UpdateUI;
            _timerUpdateUi.Interval = 100;
            _timerUpdateUi.Start();

            // Initialize controllers
            BindingManager.LoadKeybinds();

            // Initialize process watcher
            ProcessWatcher.Start();
            ControllerManager.Start();
            HapticManager.Start();
            Keymapper.Start();
        }

        private async void OnShowKeybindDialog(ControllerButton Button)
        {
            
            var bindName = ControllerManager.GetActiveController().Type == ControllerType.DualShock4 ? ControllerData.DS4.ButtonNames[Button] : ControllerData.Xbox.ButtonNames[Button];
            _bindDialog = await this.ShowProgressAsync(Properties.Resources.DialogBindKeyTitle, string.Format(Properties.Resources.DialogBindKeyText, bindName), true);

            await Task.Run((Action)
            delegate
            {
                while (!_bindDialog.IsCanceled)
                {
                    Thread.Sleep(1);
                    Key? pressedKey = null;
                    foreach (Key key in Enum.GetValues(typeof(Key)))
                    {
                        bool keyDown = false;
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            try
                            {
                                keyDown = Keyboard.IsKeyDown(key);
                            }
                            catch { }
                        });
                        if (!keyDown) continue;
                        pressedKey = key;
                        break;
                    }

                    var cancel = false;
                    if (!pressedKey.HasValue) continue;
                    switch (pressedKey)
                    {
                        case Key.Escape:
                            cancel = true;
                            break;

                        default:
                            break;
                    }
                    if (cancel) break;
                    BindingManager.SetKeybind(Button, pressedKey.Value);
                    BindingManager.SaveKeybinds();
                    break;
                }
            })
            ;

            await _bindDialog.CloseAsync();
        }

        public static void ShowKeybindDialog(ControllerButton Button)
        {
            ShowKeybindDialogEvent(Button);
        }

        private async void MainWindow_ShowMessage(string Title, string Text)
        {
            var a = await this.ShowMessageAsync(Title, Text);
        }

        public static void ShowMessageBox(string Title, string Text)
        {
            ShowMessage?.Invoke(Title, Text);
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void ShowWindow()
        {
            ShowInTaskbar = true;
            Visibility = Visibility.Visible;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(delegate
                {
                    WindowState = WindowState.Normal;
                    Activate();
                })
                );
        }

        private async Task<bool> DownloadFile(string URL, string fileName)
        {
            var pdController =
                await this.ShowProgressAsync(Properties.Resources.DialogDownloadingTitle, Properties.Resources.DialogDownloadingText, true);
            Functions.DownloadFileProgressChanged +=
                e =>
                {
                    if (e.TotalBytesToReceive > e.BytesReceived)
                    {
                        pdController.SetProgress((double)e.BytesReceived / e.TotalBytesToReceive);
                    }
                    else
                    {
                        pdController.SetIndeterminate();
                    }
                };
            var downloadSuccess = await Functions.DownloadFile(URL, fileName);
            if (pdController.IsOpen)
                await pdController.CloseAsync();
            if (!downloadSuccess)
                ShowMessageBox(Properties.Resources.DialogErrorDownloadFailedTitle,
                    Properties.Resources.DialogErrorDownloadFailedText);
            return downloadSuccess;
        }

        private void NotifyMenu_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NotifyMenu_Keybinds(object sender, RoutedEventArgs e)
        {
            ShowWindow();
            _optionsFlyout.IsOpen = true;
            _optionsFlyout.OpenKeybinds();
        }

        private void NotifyMenu_Controllers(object sender, RoutedEventArgs e)
        {
            ShowWindow();
            _optionsFlyout.IsOpen = true;
            _optionsFlyout.OpenControllers();
        }

        private void NotifyMenu_Open_WoWmapper(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
                _notifyMenu.IsOpen = true;
            }
        }

        private async void AdvancedSettings_DoResetAll(object sender, EventArgs e)
        {
            var result =
                await
                    this.ShowMessageAsync(Properties.Resources.DialogResetAllTitle,
                        Properties.Resources.DialogResetAllText, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                File.Delete("keybinds.dat");
                Settings.Default.Reset();
                Application.Current.Shutdown();
            }
        }

        private void Timer_UpdateUI(object sender, ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(UpdateUI);
            }
            catch
            {
            }
        }

        public static void SetTheme(string Accent, string Theme)
        {
            var curTheme = ThemeManager.DetectAppStyle();
            var accent = curTheme.Item2.Name == Accent ? curTheme.Item2 : ThemeManager.Accents.First(a => a.Name == Accent) ?? ThemeManager.GetAccent(Settings.Default.AppAccent);
            var theme = curTheme.Item1.Name == Theme ? curTheme.Item1 : ThemeManager.AppThemes.First(t => t.Name == Theme) ?? ThemeManager.GetAppTheme(Settings.Default.AppTheme);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme);
        }

        private void UpdateUI()
        {
            Height = _optionsFlyout.IsOpen ? Settings.Default.SettingsHeight : 350;

            // Check game status
            if (ProcessWatcher.GameRunning)
            {
                switch (ProcessWatcher.GameArchitecture)
                {
                    case Enums.GameArchitecture.X86:
                        textWoWStatus.Text = Properties.Resources.MainWindowWowRunning32;
                        break;

                    case Enums.GameArchitecture.X64:
                        textWoWStatus.Text = Properties.Resources.MainWindowWoWRunning64;
                        break;
                }
                if (ProcessWatcher.CanReadMemory)
                {
                    var gameState = ProcessWatcher.MemoryReader.GetGameState();
                    switch (gameState)
                    {
                        case GameInfo.GameState.LoggedIn:
                            var playerInfo = ProcessWatcher.MemoryReader.GetPlayerInfo();
                            if (Settings.Default.AutoClassAccent)
                            {
                                var playerClass = ProcessWatcher.MemoryReader.GetPlayerClass();
                                SetTheme(playerClass.ToString(), Settings.Default.AppTheme);
                            }
                            else
                            {
                                SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                            }
                            textWoWStatus2.Text =
                                $"{ProcessWatcher.MemoryReader.GetPlayerName()} [{playerInfo.Level}]: {playerInfo.CurrentHealth}/{playerInfo.MaxHealth}";
                            break;

                        case GameInfo.GameState.LoggedOut:
                            textWoWStatus2.Text = Properties.Resources.MainWindowWoWLoggedOut;
                            SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                            break;

                        case GameInfo.GameState.NotRunning:
                            textWoWStatus2.Text = Properties.Resources.MainWindowWoWError;
                            SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                            break;
                    }
                }
                else
                {
                    SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                }
            }
            else
            {
                textWoWStatus.Text = Properties.Resources.MainWindowWoWNotRunning;
                SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
            }

            // Check controller status
            var actiCont = ControllerManager.GetActiveController();
            if (actiCont != null &&
                ((actiCont.Type == ControllerType.DualShock4 && !Settings.Default.EnableDS4) ||
                 (actiCont.Type == ControllerType.Xbox && !Settings.Default.EnableXbox)))
            {
                actiCont.Stop();
                actiCont = null;
                UpdateContent?.Invoke();
            }

            if (actiCont != null)
            {
                textControllerStatus.Text = string.Format(Properties.Resources.MainWindowControllerConnected, actiCont.Type);

                switch (actiCont.ConnectionType)
                {
                    case ControllerConnectionType.Bluetooth:
                        imageConnectionType.Source =
                            new BitmapImage(new Uri(@"pack://application:,,,/Resources/ConnectionIcons/Bluetooth.png"));
                        break;

                    case ControllerConnectionType.USB:
                        if (Settings.Default.AppTheme == "BaseDark")
                            imageConnectionType.Source =
                                new BitmapImage(new Uri(@"pack://application:,,,/Resources/ConnectionIcons/UsbLight.png"));
                        else
                            imageConnectionType.Source =
                                new BitmapImage(new Uri(@"pack://application:,,,/Resources/ConnectionIcons/UsbDark.png"));
                        break;

                    case ControllerConnectionType.Wireless:
                        imageConnectionType.Source =
                            new BitmapImage(new Uri(@"pack://application:,,,/Resources/ConnectionIcons/Wireless.png"));
                        break;
                }

                switch (actiCont.BatteryState)
                {
                    case ControllerBatteryState.None:

                        textBatteryStatus.Text = string.Format(Properties.Resources.MainWindowBatteryConnected);
                        break;

                    case ControllerBatteryState.Charging:
                        textBatteryStatus.Text = string.Format(Properties.Resources.MainWindowBatteryCharging,
                            actiCont.Battery);
                        break;

                    case ControllerBatteryState.Discharging:
                        textBatteryStatus.Text = string.Format(Properties.Resources.MainWindowBatteryDischarging,
                            actiCont.Battery);
                        break;

                    case ControllerBatteryState.Full:
                        textBatteryStatus.Text = string.Format(Properties.Resources.MainWindowBatteryConnected);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                textControllerStatus.Text = Properties.Resources.MainWindowControllerNotActive;
                imageConnectionType.Source = null;
                textBatteryStatus.Text = Properties.Resources.MainWindowControllerBatteryNotConnected;
            }
        }

        private async void AdvancedSettings_ShowFeedbackWarning(object sender, EventArgs e)
        {
            var result =
                await
                    this.ShowMessageAsync(Properties.Resources.DialogAdvancedFeaturesWarningTitle,
                        Properties.Resources.DialogAdvancedFeaturesWarningText, MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                Settings.Default.EnableAdvancedFeatures = true;
                Settings.Default.Save();
                ProcessWatcher.Restart();
            }

            UpdateContent?.Invoke();
        }

        private async Task<MessageDialogResult> ShowNoWoWDialog()
        {
            return
                await
                    this.ShowMessageAsync(Properties.Resources.DialogWoWNotFoundTitle,
                        Properties.Resources.DialogWoWNotFoundText, MessageDialogStyle.AffirmativeAndNegative);
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Keymapper.Stop();
            ProcessWatcher.Stop();
            HapticManager.Stop();

            var aC = ControllerManager.GetActiveController();
            aC?.SetLightbar(0,0,0);
            aC?.Stop();
            ControllerManager.Stop();

            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            WindowClosing?.Invoke();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load flyouts
            Flyouts.Items.Add(_optionsFlyout);

            if (Settings.Default.StartMinimized)
            {
                WindowState = WindowState.Minimized;
            }
            else
            {
                Show();
            }

            // Check WoW installation
            if (!Functions.ValidateWoWPath() && WindowState == WindowState.Normal)
            {
                if (!Functions.FindWoWPath())
                {
                    // Prompt user to find path
                    var res = await ShowNoWoWDialog();
                    if (res == MessageDialogResult.Affirmative)
                    {
                        _optionsFlyout.IsOpen = true;
                        _optionsFlyout.OpenWoW();
                    }
                }
            }

            UpdateVersionDisplay();

            UpdateContent();
        }
        
        private async void UpdateVersionDisplay()
        {
            // update current version
            var wmCurrent = Assembly.GetExecutingAssembly().GetName().Version;
            var cpInfo = AddonParser.GetAddonInfo(Settings.Default.WoWFolder, "ConsolePort");
            Version cpCurrent;
            if(cpInfo != null)
            cpCurrent = AddonParser.GetAddonVersion(cpInfo);
            else
            cpCurrent = null;
            textWoWmapperCurrent.Text = string.Format(Properties.Resources.MainWindowVersionWowmapperCurrent, wmCurrent);
            if (cpCurrent == null)
            {
                textConsolePortCurrent.Text = Properties.Resources.MainWindowVersionConsolePortIsNotInstalled;
                buttonUpdateConsolePort.Content = Properties.Resources.MainWindowVersionInstall;
                cpCurrent = new Version(0,0,0);
            } else { textConsolePortCurrent.Text = string.Format(Properties.Resources.MainWindowVersionConsoleportCurrent, cpCurrent); }
            

            // fetch latest

            wmRelease = await Functions.GetWoWmapperLatest();
            cpRelease = await Functions.GetConsolePortLatest();

            if (wmRelease != null && wmCurrent < new Version(wmRelease.TagName))
            {
                // new version available
                textWoWmapperAvailable.Text = string.Format(Properties.Resources.MainWindowVersionWowmapperAvailable, wmRelease.TagName);
                buttonUpdateWoWmapper.Visibility = Visibility.Visible;
            }
            else
            {
                textWoWmapperAvailable.Text = Properties.Resources.MainWindowVersionNoNewVersion;
            }

            if (cpRelease != null && cpCurrent < new Version(cpRelease.TagName))
            {
                // New version available
                textConsolePortAvailable.Text = string.Format(Properties.Resources.MainWindowVersionConsoleportAvailable,
                    cpRelease.TagName);
                buttonUpdateConsolePort.Visibility = Visibility.Visible;
            }
            else
            {
                textConsolePortAvailable.Text = Properties.Resources.MainWindowVersionNoNewVersion;
            }
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            _optionsFlyout.IsOpen = !_optionsFlyout.IsOpen;
        }

        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (!Settings.Default.MinimizeToTray || WindowState != WindowState.Minimized) return;

            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!Settings.Default.CloseToTray || WindowState != WindowState.Normal) return;

            e.Cancel = true;
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
        }

        private async void ButtonUpdateWoWmapper_OnClick(object sender, RoutedEventArgs e)
        {
            var WoWmapperUpdater = "WoWmapper_Updater.exe";
            if (wmRelease == null) return;
            var client = new GitHubClient(new ProductHeaderValue("WoWmapper"));
            var relClient = new ReleasesClient(new ApiConnection(client.Connection));
            var wmAssets = await relClient.GetAllAssets("topher-au", "WoWmapper", wmRelease.Id);
            if (wmAssets == null) return;

            var zipDownloadUrl = wmAssets.FirstOrDefault(file => Path.GetExtension(file.BrowserDownloadUrl)?.ToLower() == ".zip");
            if (zipDownloadUrl == null || zipDownloadUrl.BrowserDownloadUrl == string.Empty) return;

            await DownloadFile(zipDownloadUrl.BrowserDownloadUrl, "update.zip");

            var updateZip = ZipFile.Open("update.zip", ZipArchiveMode.Read);
            var updaterEntry = updateZip.GetEntry("WoWmapper_Updater.exe");
            if(File.Exists(WoWmapperUpdater)) File.Delete(WoWmapperUpdater);
            updaterEntry.ExtractToFile(WoWmapperUpdater);

            Process.Start(WoWmapperUpdater, "update.zip");
            Application.Current.Shutdown();
        }

        private async void ButtonUpdateConsolePort_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Settings.Default.WoWFolder))
            {
                ShowMessage(Properties.Resources.DialogSelectWoWFirstTitle,
                    Properties.Resources.DialogSelectWoWFirstText);
                return;
            }

            if (cpRelease == null) return;

            buttonUpdateConsolePort.Visibility = Visibility.Collapsed;

            var client=new GitHubClient(new ProductHeaderValue("WoWmapper"));
            var relClient = new ReleasesClient(new ApiConnection(client.Connection));
            var cpAssets = await relClient.GetAllAssets("seblindfors", "ConsolePort", cpRelease.Id);
            if (cpAssets == null) return;

            var zipDownloadUrl = cpAssets.FirstOrDefault(file => Path.GetExtension(file.BrowserDownloadUrl)?.ToLower() == ".zip");
            if (zipDownloadUrl == null || zipDownloadUrl.BrowserDownloadUrl == string.Empty) return;

            var dlSuccess = await DownloadFile(zipDownloadUrl.BrowserDownloadUrl, "consoleport_update.zip");
            if (!dlSuccess) return;

            // Install ConsolePort update here
            var updateSuccess = false;
            var exceptionMessage = string.Empty;
            try
            {
                var installPath = Path.Combine(Settings.Default.WoWFolder, "interface/addons");
                var cpDir = Path.Combine(installPath, "ConsolePort");
                var cpkDir = Path.Combine(installPath, "ConsolePortKeyboard");
                if (Directory.Exists(cpDir)) Directory.Delete(cpDir, true);
                if (Directory.Exists(cpkDir)) Directory.Delete(cpkDir, true);
                using (var zipStream = new FileStream("consoleport_update.zip", System.IO.FileMode.Open))
                {
                    ZipArchive zip = new ZipArchive(zipStream);
                    zip.ExtractToDirectory(installPath);
                    updateSuccess = true;
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            if (updateSuccess)
            {
                File.Delete("consoleport_update.zip");
                ShowMessageBox(Properties.Resources.DialogConsoleportUpdateTitle, string.Format(Properties.Resources.DialogConsolePortUpdateSuccessText, cpRelease.TagName));
                buttonUpdateConsolePort.Visibility = Visibility.Collapsed;
                UpdateVersionDisplay();
            }
            else
            {
                ShowMessageBox(Properties.Resources.DialogConsoleportUpdateTitle, string.Format(Properties.Resources.DialogConsolePortUpdateFailedText, exceptionMessage));
                buttonUpdateConsolePort.Visibility = Visibility.Visible;
            }
        }
    }
}