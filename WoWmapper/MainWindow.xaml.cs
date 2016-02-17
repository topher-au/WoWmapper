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
using Microsoft.Win32;
using Octokit;
using WoWmapper.Classes;
using WoWmapper.Controllers;
using WoWmapper.Input;
using WoWmapper.MemoryReader;
using WoWmapper.Offsets;
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
        public static string AppDataDir;
        public delegate void ShowMessageHandler(string Title, string Text);
        public delegate void ShowKeybindHandler(ControllerButton Button);
        public delegate void WoWmapperEvent();
        private readonly NotifyIcon _notifyIcon;

        public static void UpdateChildren()
        {
            Application.Current.Dispatcher.Invoke(UpdateContent);
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

            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            Logger.Write("------------------------------------------");
            Logger.Write("WoWmapper {0} initializing...",currentVersion);
            Logger.Write("Operating System: Windows {0}, {1}", Environment.OSVersion, Environment.Is64BitOperatingSystem ? "x64" : "x86");
            Logger.Write("Application Path: {0}", System.AppDomain.CurrentDomain.BaseDirectory);

            var dxKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\DirectX");
            var dxVersion = new Version();

            var dxVersionString = (string) dxKey?.GetValue("Version");
            if(dxVersionString != null) dxVersion = new Version(dxVersionString);

            if (dxVersion < new Version("4.09.00.0904"))
            {
                var downloadDXnow = System.Windows.MessageBox.Show(
                    "DirectX 9.0c is required for Xbox controller support. Would you like to download it now?",
                    "DirectX not found", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (downloadDXnow == MessageBoxResult.Yes) Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=8109");
                Settings.Default.EnableXbox = false;
                Settings.Default.Save();
            }


            if (new Version(Settings.Default.LastRunVersion) < currentVersion)
            {
                Logger.Write("Upgrading application settings");
                Settings.Default.Upgrade();
            }

            Settings.Default.LastRunVersion = currentVersion.ToString();
            Settings.Default.Save();

            AppDataDir = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "WoWmapper");
            if (!Directory.Exists(AppDataDir))
            {
                Logger.Write("App data dir not found\nCreating{0}", AppDataDir);
                Directory.CreateDirectory(AppDataDir);
            }

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

            // Load saved theme and accent
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

                if (Settings.Default.EnableAdvancedFeatures)
                {
                    if (MemoryManager.Attached)
                    {
                        var gameState = MemoryManager.GetGameState();

                        switch (gameState)
                        {
                            case GameState.LoggedIn:
                                var playerInfo = new PlayerInfo();

                                var success = MemoryManager.GetPlayerInfo(out playerInfo);

                                if (success)
                                {
                                    // Color accent by class color
                                    if (Settings.Default.AutoClassAccent)
                                    {
                                        var playerClass = MemoryManager.GetPlayerClass();
                                        SetTheme(playerClass.ToString(), Settings.Default.AppTheme);
                                    }
                                    else
                                    {
                                        SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                                    }

                                    textWoWStatus2.Text =
                                        $"{MemoryManager.GetPlayerName()} [{playerInfo.Level}]: {playerInfo.CurrentHealth}/{playerInfo.MaxHealth}";

                                }
                                break;

                            case GameState.LoggedOut:
                                textWoWStatus2.Text = Properties.Resources.MainWindowWoWLoggedOut;
                                SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                                break;

                            case GameState.NotRunning:
                                textWoWStatus2.Text = Properties.Resources.MainWindowWoWError;
                                SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                                break;
                        }
                    }

                    if (!OffsetManager.OffsetsAvailable)
                    {
                        textWoWStatus2.Text = @"Advanced features unavailable";
                    }
                }
                else
                {
                    SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
                    textWoWStatus2.Text = string.Empty;
                }
                
            }
            else
            {
                textWoWStatus.Text = Properties.Resources.MainWindowWoWNotRunning;
                textWoWStatus2.Text = String.Empty;
                SetTheme(Settings.Default.AppAccent, Settings.Default.AppTheme);
            }

            // Check controller status
            var actiCont = ControllerManager.GetActiveController();

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
            // Check current version of WoWmapper and ConsolePort
            var wmCurrent = Assembly.GetExecutingAssembly().GetName().Version;
            var cpInfo = AddonParser.GetAddonInfo(Settings.Default.WoWFolder, "ConsolePort");
            Version cpCurrent;

            cpCurrent = cpInfo != null ? AddonParser.GetAddonVersion(cpInfo) : null;

            // Update current version display
            textWoWmapperCurrent.Text = string.Format(Properties.Resources.MainWindowVersionWowmapperCurrent, wmCurrent);

            if (cpCurrent == null)
            {
                textConsolePortCurrent.Text = Properties.Resources.MainWindowVersionConsolePortIsNotInstalled;
                buttonUpdateConsolePort.Content = Properties.Resources.MainWindowVersionInstall;
                cpCurrent = new Version(0,0,0);
            } else { textConsolePortCurrent.Text = string.Format(Properties.Resources.MainWindowVersionConsoleportCurrent, cpCurrent); }
            

            // Check latest releases

            var wmRelease = await Functions.GetWoWmapperLatest();
            var cpRelease = await Functions.GetConsolePortLatest();

            if (wmRelease != null)// && wmCurrent < new Version(wmRelease.TagName))
            {
                // new version available
                textWoWmapperAvailable.Text = string.Format(Properties.Resources.MainWindowVersionWowmapperAvailable, wmRelease.TagName);
                buttonUpdateWoWmapper.Visibility = Visibility.Visible;
            }
            else
            {
                textWoWmapperAvailable.Text = Properties.Resources.MainWindowVersionNoNewVersion;
            }

            if (cpRelease != null)// && cpCurrent < new Version(cpRelease.TagName))
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

        private async Task<bool> DownloadLatestZip(string author, string repo, string localFile)
        {
            // Show progress dialog
            var progress =
                await
                    this.ShowProgressAsync(Properties.Resources.DialogDownloadingTitle,
                        Properties.Resources.DialogDownloadingText, true);

            // Get list of available releases
            var ghClient = new GitHubClient(new ProductHeaderValue("WoWmapper"));
            var ghReleases = await ghClient.Release.GetAll(author, repo);

            // Fetch latest release and list of assets
            var latestRelease = ghReleases[0];
            var latestAssets = await ghClient.Release.GetAllAssets(author, repo, latestRelease.Id);

            // Find first zip asset
            var zipAsset = latestAssets.FirstOrDefault(asset => Path.GetExtension(asset.BrowserDownloadUrl)?.ToLower() == ".zip");
            if (zipAsset == null) return false;

            // Initialize new WebClient with WoWmapper user agent header
            var webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.UserAgent,
                $"WoWmapper{Assembly.GetExecutingAssembly().GetName().Version}");
            webClient.DownloadFileCompleted += (sender, args) =>
            {
                progress.CloseAsync();
            };
            // Add DownloadProgressChanged event to update progress dialog
            webClient.DownloadProgressChanged += (sender, args) =>
            {
                if(progress.IsCanceled) webClient.CancelAsync();

                if(args.BytesReceived < args.TotalBytesToReceive)
                    progress.SetProgress((double)args.ProgressPercentage/100);
                else
                    progress.SetIndeterminate();
            };

            // Attempt to download asset to local file
            try
            {
                await webClient.DownloadFileTaskAsync(zipAsset.BrowserDownloadUrl, localFile);
            }
            catch (Exception ex)
            {
                await progress.CloseAsync();
                Logger.Write($"Error downloading release: {ex}");
                return false;
            }
            await progress.CloseAsync();
            return true;
        }

        private async void ButtonUpdateWoWmapper_OnClick(object sender, RoutedEventArgs e)
        {
            var updaterExeName = "WoWmapper_Updater.exe";
            var updateFile = Path.Combine(AppDataDir, "update.zip");
            var success = await DownloadLatestZip("topher-au", "WoWmapper", updateFile);

            if (success)
            {
                // Extract updater file from release
                var updateZip = ZipFile.Open(updateFile, ZipArchiveMode.Read);
                var updaterEntry = updateZip.GetEntry("WoWmapper_Updater.exe");
                if (File.Exists(updaterExeName)) File.Delete(updaterExeName);
                updaterEntry.ExtractToFile(updaterExeName);

                // Start updater with update args
                Process.Start(updaterExeName, updateFile);
                Application.Current.Shutdown();
            }
            else
            {
                // Download failed, display error message
                await this.ShowMessageAsync(Properties.Resources.DialogErrorDownloadFailedTitle, Properties.Resources.DialogErrorDownloadFailedText);
            }
            
        }

        private async void ButtonUpdateConsolePort_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if user has specified valid WoW folder before continuing
            if (!Directory.Exists(Settings.Default.WoWFolder))
            {
                ShowMessage(Properties.Resources.DialogSelectWoWFirstTitle,
                    Properties.Resources.DialogSelectWoWFirstText);
                return;
            }

            var updateFile = Path.Combine(AppDataDir, "update.zip");

            var success = await DownloadLatestZip("seblindfors", "ConsolePort", updateFile);

            if (!success)
            {
                // Download failed, display error message
                await this.ShowMessageAsync(Properties.Resources.DialogErrorDownloadFailedTitle, Properties.Resources.DialogErrorDownloadFailedText);
                return;
            }

            // Begin installing ConsolePort update
            var updateSuccess = false;
            var exceptionMessage = string.Empty;
            try
            {
                // Locate path for addons
                var installPath = Path.Combine(Settings.Default.WoWFolder, "interface/addons");
                var cpDir = Path.Combine(installPath, "ConsolePort");
                var cpkDir = Path.Combine(installPath, "ConsolePortKeyboard");

                // Remove existing files
                if (Directory.Exists(cpDir)) Directory.Delete(cpDir, true);
                if (Directory.Exists(cpkDir)) Directory.Delete(cpkDir, true);

                // Extract update
                using (var zipStream = new FileStream(updateFile, System.IO.FileMode.Open))
                {
                    ZipArchive zip = new ZipArchive(zipStream);
                    zip.ExtractToDirectory(installPath);
                    updateSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // Error occurres
                exceptionMessage = ex.Message;
            }

            if (updateSuccess)
            {
                // Success, remove update file
                File.Delete(Path.Combine(AppDataDir, updateFile));
                ShowMessageBox(Properties.Resources.DialogConsoleportUpdateTitle, string.Format(Properties.Resources.DialogConsolePortUpdateSuccessText));
                UpdateVersionDisplay();
            }
            else
            {
                // Show error message
                ShowMessageBox(Properties.Resources.DialogConsoleportUpdateTitle, string.Format(Properties.Resources.DialogConsolePortUpdateFailedText, exceptionMessage));
            }
        }
    }
}