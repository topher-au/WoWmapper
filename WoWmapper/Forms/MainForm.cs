using FolderSelect;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WoWmapper.AdvancedHaptics;
using WoWmapper.Input;
using WoWmapper.WoWData;

namespace WoWmapper
{
    public partial class MainForm : Form
    {
        private HapticsModule hapticModule;
        private IInputPlugin inputDevice;
        private byte[] lastTarget = new byte[16];
        private int lastTouchX, lastTouchY;
        private Thread mouseThread, movementThread;
        private bool[] moveKeys = new bool[4];
        private NotifyIcon notifyIcon = new NotifyIcon();
        private int touchState;
        private WoWInteraction wowInteraction;

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

            // Start controller threads
            movementThread = new Thread(MovementThread);
            movementThread.Priority = ThreadPriority.Highest;
            movementThread.Start();

            mouseThread = new Thread(MouseThread);
            mouseThread.Priority = ThreadPriority.Highest;
            mouseThread.Start();
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
                var dsVer = await Github.GetLatestRelease("topher-au", "DS4ConsolePort");

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
                MessageBox.Show(Properties.Resources.STRING_MESSAGE_CLOSE_WOW, "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            fz.ExtractZip(outFile, ".", "WoWmapper_Updater.exe");
            Process updater = new Process();
            updater.StartInfo.FileName = "WoWmapper_Updater.exe";
            updater.StartInfo.Arguments = string.Format("-update \"{0}\"", outFile);
            updater.StartInfo.UseShellExecute = false;
            updater.Start();
            ExitApp();
        }

        //private void buttonLoadPlugin_Click(object sender, EventArgs e)
        //{
        //    var myType = plugins.ElementAt(listInputPlugins.SelectedIndex).GetType();
        //    var assemblyFile = Assembly.GetAssembly(myType).GetName();
        //    var SelectedPluginDll = assemblyFile.Name + ".dll";
        //    foreach (var plugin in plugins)
        //    {
        //        if (Assembly.GetAssembly(plugin.GetType()).GetName().Name != assemblyFile.Name)
        //        {
        //            //plugin.Dispose();
        //        }
        //    }

        //    Properties.Settings.Default.InputPlugin = SelectedPluginDll;
        //    Properties.Settings.Default.Save();
        //    InitializePlugin(SelectedPluginDll);
        //}

        private void buttonKeybinds_Click(object sender, EventArgs e)
        {
            if (inputDevice != null)
            {
                inputDevice.ShowKeybindDialog();
            }
            else
            {
                MessageBox.Show(Properties.Resources.STRING_MESSAGE_NO_PLUGIN_SELECTED, Properties.Resources.STRING_APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSelectPlugin_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon™", "WoWmapper", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
            // TODO: Implement plugin selection (need second plugin first)

            //PluginSelectForm PSF = new PluginSelectForm();
            //PSF.ShowDialog();
        }

        private void DoTouchDown(InputButton Button)
        {
            switch (Button)
            {
                case InputButton.Extra1:
                    switch (touchState)
                    {
                        case 0:
                            wowInteraction.SendMouseDown(MouseButtons.Left);
                            break;

                        case 1:
                            wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("Extra1").Key.Value);
                            break;

                        case 2:
                            wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                            break;
                    }
                    break;

                case InputButton.Extra2:
                    switch (touchState)
                    {
                        case 0:
                            wowInteraction.SendMouseDown(MouseButtons.Right);
                            break;

                        case 1:
                            wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("Extra2").Key.Value);
                            break;

                        case 2:
                            wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                            break;
                    }
                    break;
            }
        }

        private void DoTouchUp(InputButton Button)
        {
            switch (Button)
            {
                case InputButton.Extra1:
                    switch (touchState)
                    {
                        case 0:
                            wowInteraction.SendMouseUp(MouseButtons.Left);
                            break;

                        case 1:
                            wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("Extra1").Key.Value);
                            break;

                        case 2:
                            wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                            break;
                    }
                    break;

                case InputButton.Extra2:
                    switch (touchState)
                    {
                        case 0:
                            wowInteraction.SendMouseUp(MouseButtons.Right);
                            break;

                        case 1:
                            wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("Extra2").Key.Value);
                            break;

                        case 2:
                            wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                            break;
                    }
                    break;
            }
        }

        private void DoTouchUpper()
        {
            if (touchState != 0)
            {
                touchState = 0;
            }
            else
            {
                inputDevice.Settings.Settings.Read("TouchMode", out touchState);
            }
        }

        private void DS4_ButtonDown(InputButton Button)
        {
            if (this != null && (wowInteraction.IsAttached || !Properties.Settings.Default.InactiveDisable))
                switch (Button)
                {
                    case InputButton.RFaceDown:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("RFaceDown").Key.Value);
                        break;

                    case InputButton.RFaceRight:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("RFaceRight").Key.Value);
                        break;

                    case InputButton.RFaceUp:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("RFaceUp").Key.Value);
                        break;

                    case InputButton.RFaceLeft:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("RFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceDown:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("LFaceDown").Key.Value);
                        break;

                    case InputButton.LFaceLeft:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("LFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceRight:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("LFaceRight").Key.Value);
                        break;

                    case InputButton.LFaceUp:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("LFaceUp").Key.Value);
                        break;

                    case InputButton.BumperLeft:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("BumperLeft").Key.Value);
                        break;

                    case InputButton.BumperRight:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("BumperRight").Key.Value);
                        break;

                    case InputButton.TriggerLeft:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("TriggerLeft").Key.Value);
                        break;

                    case InputButton.TriggerRight:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("TriggerRight").Key.Value);
                        break;

                    case InputButton.CenterMiddle:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("CenterMiddle").Key.Value);
                        break;

                    case InputButton.CenterLeft:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                        break;

                    case InputButton.CenterRight:
                        wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                        break;

                    case InputButton.Extra1:
                        DoTouchDown(Button);
                        break;

                    case InputButton.Extra2:
                        DoTouchDown(Button);
                        break;

                    case InputButton.Extra3:
                        DoTouchUpper();
                        break;

                    case InputButton.StickLeft:
                        wowInteraction.SendMouseDown(MouseButtons.Left);
                        break;

                    case InputButton.StickRight:
                        if (wowInteraction.IsKeyDown(Keys.LControlKey))
                        {
                            var wowWidth = wowInteraction.WoWWindow.Right - wowInteraction.WoWWindow.Left;
                            var wowHeight = wowInteraction.WoWWindow.Bottom - wowInteraction.WoWWindow.Top;

                            Cursor.Position = new Point(wowInteraction.WoWWindow.Left + (wowWidth / 2), wowInteraction.WoWWindow.Top + (wowHeight / 2));
                        }
                        else
                        {
                            wowInteraction.SendMouseDown(MouseButtons.Right);
                        }
                        break;
                }
        }

        private void DS4_ButtonUp(InputButton Button)
        {
            if (this != null && (wowInteraction.IsAttached || !Properties.Settings.Default.InactiveDisable))

                switch (Button)
                {
                    case InputButton.RFaceDown:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("RFaceDown").Key.Value);
                        break;

                    case InputButton.RFaceRight:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("RFaceRight").Key.Value);
                        break;

                    case InputButton.RFaceUp:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("RFaceUp").Key.Value);
                        break;

                    case InputButton.RFaceLeft:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("RFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceDown:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("LFaceDown").Key.Value);
                        break;

                    case InputButton.LFaceLeft:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("LFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceRight:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("LFaceRight").Key.Value);
                        break;

                    case InputButton.LFaceUp:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("LFaceUp").Key.Value);
                        break;

                    case InputButton.BumperLeft:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("BumperLeft").Key.Value);
                        break;

                    case InputButton.BumperRight:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("BumperRight").Key.Value);
                        break;

                    case InputButton.TriggerLeft:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("TriggerLeft").Key.Value);
                        break;

                    case InputButton.TriggerRight:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("TriggerRight").Key.Value);
                        break;

                    case InputButton.CenterMiddle:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("CenterMiddle").Key.Value);
                        break;

                    case InputButton.CenterLeft:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                        break;

                    case InputButton.CenterRight:
                        wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                        break;

                    case InputButton.Extra1:
                        DoTouchUp(Button);
                        break;

                    case InputButton.Extra2:
                        DoTouchUp(Button);
                        break;

                    case InputButton.Extra3:
                        DoTouchUpper();
                        break;

                    case InputButton.StickLeft:
                        wowInteraction.SendMouseUp(MouseButtons.Left);
                        break;

                    case InputButton.StickRight:
                        wowInteraction.SendMouseUp(MouseButtons.Right);
                        break;
                }
        }

        private void DS4_OnControllerConnected()
        {
        }

        private void ExitApp()
        {
            Hide();
            notifyIcon.Visible = false;

            movementThread.Abort();
            mouseThread.Abort();

            if (hapticModule != null) hapticModule.Dispose();
            if (inputDevice != null) inputDevice.Dispose();

            wowInteraction.Dispose();

            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private string GetKeyName(int i)
        {
            switch (i)
            {
                case 0:
                    return WoWInteraction.MoveBindName.Forward;

                case 1:
                    return WoWInteraction.MoveBindName.Backward;

                case 2:
                    return WoWInteraction.MoveBindName.Left;

                case 3:
                    return WoWInteraction.MoveBindName.Right;
            }
            return string.Empty;
        }

        private Point GetStickPoint(InputStick Stick)
        {
            // TODO implement getstickpoint
            int x, y;
            switch (Stick)
            {
                case InputStick.Left:
                    x = inputDevice.GetAxis(InputAxis.LeftStickX);
                    y = inputDevice.GetAxis(InputAxis.LeftStickY);
                    break;

                case InputStick.Right:
                    x = inputDevice.GetAxis(InputAxis.RightStickX);
                    y = inputDevice.GetAxis(InputAxis.RightStickY);
                    break;

                default:
                    x = 0;
                    y = 0;
                    break;
            }
            return new Point(x, y);
        }

        private void InitializePlugin(string DllFile)
        {
            if (inputDevice != null)
            {
                // Dispose old plugin
                //InputDevice.Dispose();
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

            if (inputDevice != null)
            {
                inputDevice.OnControllerConnected += DS4_OnControllerConnected;
                inputDevice.OnButtonDown += DS4_ButtonDown;
                inputDevice.OnButtonUp += DS4_ButtonUp;
                inputDevice.OnTouchpadMoved += Touchpad_TouchesMoved;

                int tLeft, tRight;
                inputDevice.Settings.Settings.Read("TriggerLeft", out tLeft);
                inputDevice.Settings.Settings.Read("TriggerRight", out tRight);

                inputDevice.Thresholds = new InputThresholds()
                {
                    TriggerLeft = tLeft,
                    TriggerRight = tRight
                };
                inputDevice.Enabled = true;
            }

            wowInteraction = new WoWInteraction(inputDevice.Keybinds);
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

        private void MouseThread()
        {
            bool movingX = false;
            bool movingY = false;

            bool slowCursor = false;
            float slowMod = 0.5f;

            while (true)
            {
                if (wowInteraction != null)
                    if ((wowInteraction.IsAttached || !Properties.Settings.Default.InactiveDisable)
                        && inputDevice != null)
                    {
                        // Do sticky cursor check
                        if (Properties.Settings.Default.EnableAdvancedHaptics && hapticModule != null)
                        {
                            if (Properties.Settings.Default.EnableStickyCursor)
                            {
                                if (hapticModule.IsWoWAttached && hapticModule.GameState == WoWState.LoggedIn)
                                {
                                    var target = hapticModule.wowReader.MouseGuid;
                                    if (!target.SequenceEqual(new byte[16])) // compare mouseover to blank target
                                    {
                                        slowCursor = true;
                                    }
                                    else
                                    {
                                        slowCursor = false;
                                    }
                                }
                            }
                        }

                        var rightStick = GetStickPoint(InputStick.Right);
                        var curPos = Cursor.Position;

                        int intRightDead, intRightSpeed, intRightCurve;

                        inputDevice.Settings.Settings.Read("RightDead", out intRightDead);
                        inputDevice.Settings.Settings.Read("RightCurve", out intRightCurve);
                        inputDevice.Settings.Settings.Read("RightSpeed", out intRightSpeed);

                        // Process X-axis
                        var rightX = rightStick.X;
                        if (rightX < 0) rightX = -rightX; // flip to positive value

                        if (true)
                        {
                            float rightMax = 127 - intRightDead;
                            float rightVal = rightX;
                            if (rightX > intRightDead)
                                rightVal = rightX - intRightDead;    // decrease by deadzone value

                            float tiltPercent = (rightVal / rightMax);

                            double moveMath = 0;
                            if (slowCursor)
                            {
                                moveMath = Math.Pow(((float)intRightCurve * slowMod) * tiltPercent, 2) + ((intRightSpeed) * tiltPercent) + 1;
                            }
                            else
                            {
                                moveMath = Math.Pow(intRightCurve * tiltPercent, 2) + (intRightSpeed * tiltPercent) + 1;
                            }

                            int moveVal = (int)moveMath; // y = ax^2 + bx where a=

                            if (rightStick.X < -intRightDead || (rightStick.X < -10 && movingY))
                            {
                                curPos.X -= moveVal;
                                movingX = true;
                            }
                            else if (rightStick.X > intRightDead || (rightStick.X > 10 && movingY))
                            {
                                curPos.X += moveVal;
                                movingX = true;
                            }
                            else
                            {
                                movingX = false;
                            }
                        }

                        // Process Y-axis

                        var rightY = rightStick.Y;
                        if (rightY < 0) rightY = -rightY; // flip to positive value

                        if (true)
                        {
                            float rightMax = 127 - intRightDead;
                            float rightVal = rightY;
                            if (rightY > intRightDead)
                                rightVal = rightY - intRightDead;    // decrease by deadzone value

                            float tiltPercent = (rightVal / rightMax);

                            double moveMath = 0;
                            if (slowCursor)
                            {
                                moveMath = Math.Pow(((float)intRightCurve * slowMod) * tiltPercent, 2) + ((intRightSpeed) * tiltPercent) + 1;
                            }
                            else
                            {
                                moveMath = Math.Pow(intRightCurve * tiltPercent, 2) + (intRightSpeed * tiltPercent) + 1;
                            }

                            int moveVal = (int)moveMath; // y = ax^2 + bx where a=

                            if (rightStick.Y < -intRightDead || (rightStick.Y < -10 && movingX))
                            {
                                curPos.Y -= moveVal;
                                movingY = true;
                            }
                            else if (rightStick.Y > intRightDead || (rightStick.Y > 10 && movingX))
                            {
                                curPos.Y += moveVal;
                                movingY = true;
                            }
                            else
                            {
                                movingY = false;
                            }
                        }

                        Cursor.Position = curPos;
                    }

                Thread.Sleep(10);
            }
        }

        private void MovementThread()
        {
            while (true)
            {

                if (wowInteraction != null)
                    if (inputDevice != null && (wowInteraction.IsAttached))
                    {
                        bool[] thisMove = new bool[4];
                        var leftStick = GetStickPoint(InputStick.Left);
                        if (leftStick.Y < -40)
                        {
                            thisMove[(int)WoWInteraction.Direction.Forward] = true;
                        }

                        if (leftStick.Y > 40)
                        {
                            thisMove[(int)WoWInteraction.Direction.Backward] = true;
                        }

                        if (leftStick.X < -40)
                        {
                            thisMove[(int)WoWInteraction.Direction.Left] = true;
                        }

                        if (leftStick.X > 40)
                        {
                            thisMove[(int)WoWInteraction.Direction.Right] = true;
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            if (thisMove[i] && !moveKeys[i])
                            {
                                // send key down
                                wowInteraction.SendKeyDown(inputDevice.Keybinds.FromName(GetKeyName(i)).Key.Value);
                                moveKeys[i] = true;
                            }
                            if (!thisMove[i] && moveKeys[i])
                            {
                                // send key up
                                wowInteraction.SendKeyUp(inputDevice.Keybinds.FromName(GetKeyName(i)).Key.Value);
                                moveKeys[i] = false;
                            }
                        }
                    }

                Thread.Sleep(10);
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
                checkWindowAttached.Checked = wowInteraction.IsAttached;

                UpdateAdvancedHaptics();

                // update battery display
                if (inputDevice.Connected)
                {
                    var battery = inputDevice.Battery;
                    switch (inputDevice.ConnectionType)
                    {
                        case InputConnectionType.Wireless:
                            picConnectionType.Image = Properties.Resources.BT;
                            break;

                        case InputConnectionType.Wired:
                            picConnectionType.Image = Properties.Resources.USB;
                            break;
                    }
                    labelConnectionStatus.Text = string.Format(Properties.Resources.STRING_CONTROLLER_CONNECTED, battery, inputDevice.ControllerName);
                }
                else
                {
                    // No controller connected
                    picConnectionType.Image = null;
                    labelConnectionStatus.Text = Properties.Resources.STRING_CONTROLLER_DISCONNECTED;
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
                            else if (Properties.Settings.Default.ColorLightbar && wowInteraction.IsAttached)
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
                            else if (hapticModule.GameState != WoWState.LoggedIn && Properties.Settings.Default.ColorLightbar && wowInteraction.IsAttached)
                            {
                                // Haptics not logged in, use default color
                                hapticModule.LightbarOverride = false;
                                var lbColor = Properties.Settings.Default.ColorDefault;
                                inputDevice.SetLEDColor(lbColor.R, lbColor.G, lbColor.B);
                            }
                            else if(!wowInteraction.IsAttached)
                            {
                                inputDevice.SetLEDOff();
                            } else
                            {
                                // Disable override, use haptic color
                                hapticModule.LightbarOverride = false;
                            }
                        }
                    }
                }


            }
        }

        private void Touchpad_TouchesMoved(InputTouchpadEventArgs e)
        {
            if (touchState == 0 && (wowInteraction.IsAttached || !Properties.Settings.Default.InactiveDisable)) // Mouse Control
            {
                var x = e.touches[0].DeltaX;
                var y = e.touches[0].DeltaY;
                var curPos = Cursor.Position;
                if (x != lastTouchX)
                {
                    curPos.X += x;
                    lastTouchX = x;
                }
                if (y != lastTouchY)
                {
                    curPos.Y += y;
                    lastTouchY = y;
                }
                Cursor.Position = curPos;
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
                else if (wowInteraction.IsAttached && hapticModule.wowReader.IsAttached && hapticModule.wowReader.OffsetsLoaded)
                {
                    hapticModule.Enabled = true;
                    hapticModule.LedClass = Properties.Settings.Default.EnableLightbarClass;
                    hapticModule.LedHealth = Properties.Settings.Default.EnableLightbarHealth;
                    hapticModule.RumbleTarget = Properties.Settings.Default.EnableRumbleTarget;
                    hapticModule.RumbleDamage = Properties.Settings.Default.EnableRumbleDamage;
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