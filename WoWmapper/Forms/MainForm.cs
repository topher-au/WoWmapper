using FolderSelect;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using WoWmapper.EnhancedInteraction;
using WoWmapper.Input;
using WoWmapper.MapperModule;
using WoWmapper.WoWData;
using WoWmapper.Properties;
namespace WoWmapper
{
    public partial class MainForm : Form
    {
        private HapticsModule hapticModule;
        private IInputPlugin inputDevice;

        //private byte[] lastTarget = new byte[16];
        private NotifyIcon notifyIcon = new NotifyIcon();

        private int touchState;
        private WoWMapper wowMapper;
        string displayName = "Controller";

        public MainForm()
        {
            InitializeComponent();

            DoVersionCheck();

            // Load resources
            Icon = Properties.Resources.WoWConsolePort;

            groupStatus.Text = Properties.Resources.STRING_STATUS;
            labelConnectionStatus.Text = Properties.Resources.STRING_CONTROLLER_DISCONNECTED;
            checkWindowAttached.Text = Properties.Resources.STRING_WOW_WINDOW_FOUND;

            groupAdvancedHaptics.Text = Properties.Resources.STRING_HAPTIC_STATUS;
            checkHapticsAttached.Text = Properties.Resources.STRING_HAPTIC_ATTACHED;
            checkHapticsUserLoggedIn.Text = Properties.Resources.STRING_HAPTIC_CHARLOGGEDIN;
            labelPlayerInfo.Text = string.Empty;

            buttonConfig.Text = Properties.Resources.STRING_SHOW_CONFIG;
            buttonKeybinds.Text = Properties.Resources.STRING_SHOW_KEYBINDS;
            buttonSelectPlugin.Text = Properties.Resources.STRING_SHOW_PLUGINS;

            // Set up tray icon
            notifyIcon.Icon = Icon;
            notifyIcon.Text = Properties.Resources.STRING_NOTIFY_TOOLTIP;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.ContextMenuStrip = menuNotify;

            // Check for a valid wow directory
            if (!Functions.CheckWoWPath()) // check for valid install path
            {
                var findWoWNow = MessageBox.Show(Properties.Resources.STRING_MESSAGE_NO_WOW_PATH, "WoWmapper", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (findWoWNow == DialogResult.Yes)
                {
                    FolderSelectDialog fbd = new FolderSelectDialog();
                    var res = fbd.ShowDialog();
                    if (res && Directory.Exists(fbd.FileName))
                    {
                        Properties.Settings.Default.WoWInstallPath = fbd.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        public async Task DoVersionCheck()
        {
            // Update app version display
            var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
            Version addonVersion = null;

            if (assemblyVersion.Revision == 0)
            {
                assemblyVersion = new Version(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build);
            }

            labelDS4VersionInstalled.Text = assemblyVersion.ToString();

            if (Functions.CheckWoWPath()) // If we have valid wow path
            {
                // Attempt to check addon version
                try
                {
                    addonVersion = Functions.CheckAddonVersion(Properties.Settings.Default.WoWInstallPath, "ConsolePort");
                }
                catch { }

                if (addonVersion == null)
                {
                    labelCPVersionInstalled.Text = "N/A";
                }
                else
                if (addonVersion.Revision == 0)
                {
                    addonVersion = new Version(addonVersion.Major, addonVersion.Minor, addonVersion.Build);
                    labelCPVersionInstalled.Text = addonVersion.ToString();
                }
                else
                {
                    labelCPVersionInstalled.Text = addonVersion.ToString();
                }
            }

            if (true)
            {
                // check website version
                var dsVer = await Github.GetLatestRelease("topher-au", "WoWmapper");

                if (dsVer != null)
                {
                    Version v = new Version("0.0.0");
                    try
                    {
                        v = new Version(dsVer.tag_name);
                    }
                    catch
                    {
                        Console.WriteLine("Error parsing WoWmapper version number");
                        return;
                    }
                    labelDS4VersionAvailable.Text = v.ToString();

                    if (assemblyVersion < v)
                    {
                        buttonDS4UpdateNow.Visible = true;
                    }
                }
                else
                {
                }

                // Check github for a new release
                var cpVer = await Github.GetLatestRelease("seblindfors", "ConsolePort");

                if (cpVer != null)
                {
                    Version v = new Version();
                    try
                    {
                        v = new Version(cpVer.tag_name);
                    }
                    catch
                    {
                        Console.WriteLine("Error parsing ConsolePort version number");
                        return;
                    }
                    labelCPVersionAvailable.Text = v.ToString();

                    if (Functions.CheckWoWPath()) // valid install dir
                    {
                        if (addonVersion == null) // addon not installed
                        {
                            buttonCPUpdateNow.Text = Properties.Resources.STRING_INSTALL_BUTTON;
                            buttonCPUpdateNow.Visible = true;
                        }
                        else if (addonVersion < v) // addon out of date
                        {
                            buttonCPUpdateNow.Visible = true;
                        }
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Program.WM_ACTIVATE_DS4)
            { // Received call from another instance
                ShowForm(); // Bring current instance to focus
                var onTop = TopMost;
                TopMost = true;
                TopMost = onTop;
                Activate();
            }
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            ShowConfig();
        }

        private void buttonCPUpdateNow_Click(object sender, EventArgs e)
        {
            if (Functions.CheckIsWowRunning())
            {
                MessageBox.Show(Resources.STRING_MESSAGE_CLOSE_WOW, "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var cpVer = Github.GetLatestRelease2("seblindfors", "ConsolePort");
            DownloadForm dlf = new DownloadForm(cpVer.assets[0].browser_download_url);
            if (dlf.DialogResult == DialogResult.Cancel) return;

            var outFile = dlf.OutputFile;
            Functions.InstallAddOn(outFile);
            MessageBox.Show(string.Format(Properties.Resources.STRING_MESSAGE_CP_UPDATE_SUCCESS, cpVer.tag_name), "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
            File.Delete(outFile);
            var vCheck = DoVersionCheck();
        }

        private void buttonDS4UpdateNow_Click(object sender, EventArgs e)
        {
            if (Functions.CheckIsWowRunning())
            {
                MessageBox.Show(Properties.Resources.STRING_MESSAGE_CLOSE_WOW, "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var dsVer = Github.GetLatestRelease2("topher-au", "WoWmapper");
            DownloadForm dlf;
            try
            {
                dlf = new DownloadForm(dsVer.assets[0].browser_download_url);
            }
            catch
            {
                MessageBox.Show("Error", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dlf.DialogResult == DialogResult.Cancel) return;

            var outFile = dlf.OutputFile;
            FastZip fz = new FastZip();
            try
            {
                fz.ExtractZip(outFile, ".", "WoWmapper_Updater.exe");
            } catch
            {
                MessageBox.Show("Error extracting WoWmapper updater");
            }
            
            Process updater = new Process();
            updater.StartInfo.FileName = "WoWmapper_Updater.exe";
            updater.StartInfo.Arguments = string.Format("-update \"{0}\"", outFile);
            updater.StartInfo.UseShellExecute = false;
            updater.Start();
            ExitApp();
        }

        private void buttonKeybinds_Click(object sender, EventArgs e)
        {
            if (inputDevice != null)
            {
                inputDevice.ShowKeybindDialog();
            }
            else
            {
                MessageBox.Show(Resources.STRING_MESSAGE_NO_PLUGIN_SELECTED, Properties.Resources.STRING_APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSelectPlugin_Click(object sender, EventArgs e)
        {
            PluginSelectForm pluginForm = new PluginSelectForm();
            pluginForm.ShowDialog();
            if (pluginForm.DialogResult != DialogResult.OK) return;
            InitializePlugin(Properties.Settings.Default.InputPlugin);
        }

        private void ExitApp()
        {
            Hide();
            notifyIcon.Visible = false;
            if (wowMapper != null) wowMapper.Dispose();

            inputDevice.Kill();
            if (hapticModule != null) hapticModule.Dispose();

            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void InitializePlugin(string DllFile)
        {
            if (inputDevice != null)
            {
                // Dispose old plugin
                inputDevice.Kill();
            }

            Type pluginType = typeof(IInputPlugin);
            string pluginPath = Path.Combine("plugins", DllFile);
            var pluginName = AssemblyName.GetAssemblyName(pluginPath);
            Assembly pluginAssembly;
            pluginAssembly = Assembly.Load(pluginName);

            Type[] types = pluginAssembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.GetInterface(pluginType.FullName) != null)
                {
                    inputDevice = (IInputPlugin)Activator.CreateInstance(type, new object[] { true });
                    break;
                }
            }

            if (wowMapper != null)
            {
                wowMapper.Dispose();
            }
            wowMapper = new WoWMapper(inputDevice);
            inputDevice.Settings.Settings.Read("TouchMode", out touchState);
        }

        private void keybindingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inputDevice != null)
            {
                inputDevice.ShowKeybindDialog();
                inputDevice.Settings.Settings.Read("TouchMode", out touchState);
            }
        }

        private void labelDS4CP_Click(object sender, EventArgs e)
        {
            inputDevice.ShowKeybindDialog();
            inputDevice.Settings.Settings.Read("TouchMode", out touchState);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // close to tray
            if (e.CloseReason == CloseReason.UserClosing && Properties.Settings.Default.CloseToTray)
            {
                Minimize();
                e.Cancel = true;
                return;
            }
            ExitApp();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.StartMinimized) { Minimize(); } else Show();

            // Load last input plugin
            var InputPlugin = Properties.Settings.Default.InputPlugin;

            if (File.Exists(Path.Combine("plugins", InputPlugin)))
            {
                InitializePlugin(InputPlugin);
            }
            else
            {
                InitializePlugin("input_ds4.dll");
            }

            // App was loaded from updater
            if (Environment.GetCommandLineArgs().Length > 1)
                if (Environment.GetCommandLineArgs()[1] == "-updated")
                {
                    var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
                    var addonVersion = new Version();
                    if (assemblyVersion.Revision == 0)
                    {
                        assemblyVersion = new Version(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build);
                    }
                    MessageBox.Show(String.Format(Properties.Resources.STRING_MESSAGE_DS4_UPDATE_SUCCESS, assemblyVersion.ToString()), "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Minimize();
            }
            else
            {
                ShowForm();
            }
        }

        private void Minimize()
        {
            WindowState = FormWindowState.Minimized;
            if (Properties.Settings.Default.MinToTray)
            {
                ShowInTaskbar = false;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowConfig();
        }

        private void ShowConfig()
        {
            ConfigForm cf = new ConfigForm();

            if (this.WindowState == FormWindowState.Minimized)
                cf.ShowInTaskbar = true;

            cf.ShowDialog();
        }

        private void ShowForm()
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            if (inputDevice != null)
            {
                checkWindowAttached.Checked = wowMapper.IsAttached;

                UpdateAdvancedHaptics();

                // update battery display
                if (inputDevice.Connected)
                {
                    var battery = inputDevice.Battery;
                    switch (inputDevice.ConnectionType)
                    {
                        case InputConnectionType.Wireless:
                            picConnectionType.Image = Resources.Wireless;
                            break;

                        case InputConnectionType.Bluetooth:
                            picConnectionType.Image = Resources.Bluetooth;
                            break;

                        case InputConnectionType.Wired:
                            picConnectionType.Image = Resources.USB;
                            break;
                    }
                    labelConnectionStatus.Text = string.Format(Resources.STRING_CONTROLLER_CONNECTED, battery);
                    if(displayName != inputDevice.ControllerName)
                    {
                        groupStatus.Text = string.Format(Resources.STRING_GROUP_STATUS, inputDevice.ControllerName);
                        displayName = inputDevice.ControllerName;
                    }
                }
                else
                {
                    // No controller connected
                    picConnectionType.Image = null;
                    labelConnectionStatus.Text = Properties.Resources.STRING_CONTROLLER_DISCONNECTED;
                    if(displayName != Resources.STRING_CONTROLLER_DEFAULT)
                    {
                        groupStatus.Text = string.Format(Resources.STRING_GROUP_STATUS, Resources.STRING_CONTROLLER_DEFAULT);
                        displayName = Resources.STRING_CONTROLLER_DEFAULT;
                    }
                    
                }

                // Check Led color
                if (inputDevice != null)
                {
                    if (inputDevice.Peripherals.LED)
                    {
                        if (hapticModule == null)
                        {
                            // No advanced haptics module
                            if (Properties.Settings.Default.LightbarBattery && inputDevice.Battery < Properties.Settings.Default.LightbarBatteryThreshold && inputDevice.Battery > 0)
                            {
                                // Battery low
                                var lbColor = Properties.Settings.Default.LightbarBatteryColor;
                                inputDevice.SetLEDFlash(lbColor.R, lbColor.G, lbColor.B, 100, 100);
                            }
                            else if (Properties.Settings.Default.ColorLightbar && wowMapper.IsAttached)
                            {
                                // Default color
                                var lbColor = Properties.Settings.Default.ColorDefault;
                                inputDevice.SetLEDColor(lbColor.R, lbColor.G, lbColor.B);
                            }
                            else
                            {
                                inputDevice.SetLEDOff();
                            }
                        }
                        else
                        {
                            // Advanced haptics module enabled
                            if (Properties.Settings.Default.LightbarBattery && inputDevice.Battery < Properties.Settings.Default.LightbarBatteryThreshold)
                            {
                                // Battery low, override advanced haptics
                                hapticModule.LightbarOverride = true;
                                var lbColor = Properties.Settings.Default.LightbarBatteryColor;
                                inputDevice.SetLEDFlash(lbColor.R, lbColor.G, lbColor.B, 100, 100);
                            }
                            else if (hapticModule.GameState != WoWState.LoggedIn && Properties.Settings.Default.ColorLightbar && wowMapper.IsAttached)
                            {
                                // Haptics not logged in, use default color
                                hapticModule.LightbarOverride = false;
                                var lbColor = Properties.Settings.Default.ColorDefault;
                                inputDevice.SetLEDColor(lbColor.R, lbColor.G, lbColor.B);
                            }
                            else if (!wowMapper.IsAttached)
                            {
                                inputDevice.SetLEDOff();
                            }
                            else
                            {
                                // Disable override, use haptic color
                                hapticModule.LightbarOverride = false;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateAdvancedHaptics()
        {
            // Initialize haptics if not already
            if (Properties.Settings.Default.EnableAdvancedHaptics)
                try
                {
                    if (hapticModule == null)
                        hapticModule = new HapticsModule(inputDevice);
                }
                catch
                {
                    // Error loading advanced haptics, disable
                    hapticModule = null;
                    Properties.Settings.Default.EnableAdvancedHaptics = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show(Properties.Resources.STRING_MESSAGE_HAPTICS_DISABLED_ERROR, "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            // Update advanced haptic display
            if (Properties.Settings.Default.EnableAdvancedHaptics && hapticModule != null)
            {
                if (hapticModule.wowReader.IsAttached && !hapticModule.wowReader.OffsetsLoaded)
                {
                    // No suitable offsets found, disable haptics
                    Properties.Settings.Default.EnableAdvancedHaptics = false;
                    Properties.Settings.Default.Save();
                    hapticModule.Dispose();
                    hapticModule = null;
                    MessageBox.Show(Properties.Resources.STRING_MESSAGE_HAPTICS_DISABLED_NO_OFFSETS, "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (wowMapper.IsAttached && hapticModule.wowReader.IsAttached && hapticModule.wowReader.OffsetsLoaded)
                {
                    hapticModule.Enabled = true;
                    hapticModule.LedClass = Properties.Settings.Default.EnableLightbarClass;
                    hapticModule.LedHealth = Properties.Settings.Default.EnableLightbarHealth;
                    hapticModule.RumbleTarget = Properties.Settings.Default.EnableRumbleTarget;
                    hapticModule.RumbleDamage = Properties.Settings.Default.EnableRumbleDamage;
                    hapticModule.AutoCenter = Properties.Settings.Default.AutoCenter;

                    hapticModule.HealthColors = new HapticsModule.Colors()
                    {
                        Critical = Properties.Settings.Default.ColorCritical,
                        Low = Properties.Settings.Default.ColorLow,
                        Medium = Properties.Settings.Default.ColorMedium,
                        High = Properties.Settings.Default.ColorHigh
                    };

                    checkHapticsAttached.Checked = hapticModule.IsWoWAttached;

                    if (hapticModule.IsWoWAttached)
                    {
                        if (hapticModule.GameState == WoWState.LoggedIn)
                        {
                            var pi = hapticModule.wowReader.ReadPlayerInfo();
                            labelPlayerInfo.Text = String.Format("{0} / {3} {4}\n{5}/{6}", pi.Name, pi.RealmName, pi.AccountName, pi.Level, pi.Class, pi.CurrentHP, pi.MaxHP);
                            checkHapticsUserLoggedIn.Checked = true;
                        }
                        else
                        {
                            checkHapticsUserLoggedIn.Checked = false;
                            labelPlayerInfo.Text = string.Empty;
                        }

                        checkHapticsUserLoggedIn.Checked = hapticModule.GameState == WoWState.LoggedIn ? true : false;
                    }
                }
                else
                {
                    checkHapticsAttached.Checked = false;
                    checkHapticsUserLoggedIn.Checked = false;
                    hapticModule.Enabled = false;
                    labelPlayerInfo.Text = string.Empty;
                }
            }
            else
            {
                if (hapticModule != null)
                {
                    hapticModule.Dispose();
                    hapticModule.Enabled = false;
                    hapticModule = null;
                }

                checkHapticsAttached.Checked = false;
                checkHapticsUserLoggedIn.Checked = false;
                labelPlayerInfo.Text = "";
            }
        }
    }
}