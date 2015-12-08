using DS4ConsolePort.AdvancedHaptics;
using DS4ConsolePort.WoWData;
using DS4Windows;
using DS4Wrapper;
using FolderSelect;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public partial class MainForm : Form
    {
        #region Private Fields

        private const string DS4CP_USER = "topher-au";
        private const string DS4CP_REPO = "DS4ConsolePort";
        private Haptics advHaptics;
        private DS4 DS4;
        private WoWInteraction interaction;
        private int intRightCurve, intRightDead, intRightSpeed;
        private int lastTouchX, lastTouchY;
        private Thread mouseThread, movementThread;
        private bool[] moveKeys = new bool[4];
        private NotifyIcon notifyIcon = new NotifyIcon();
        private Point ptLeftStick, ptRightStick;
        private Settings settings = new Settings();
        private int touchState;

        #endregion Private Fields

        #region Public Constructors

        public MainForm()
        {
            InitializeComponent();

            // Load resources
            Icon = Properties.Resources.WoWConsolePort;
            notifyIcon.Icon = Icon;
            notifyIcon.Text = Properties.Resources.STRING_NOTIFY_TOOLTIP;
            tabConsolePort.Text = Properties.Resources.STRING_TAB_CONSOLEPORT;
            tabKeybinds.Text = Properties.Resources.STRING_TAB_KEYBINDS;
            tabAdvanced.Text = Properties.Resources.STRING_TAB_ADVANCED;
            labelCamera.Text = Properties.Resources.STRING_BIND_CAMERA;
            labelMovement.Text = Properties.Resources.STRING_BIND_MOVEMENT;
            groupHapticStatus.Text = Properties.Resources.STRING_HAPTIC_STATUS;
            checkHapticsAttached.Text = Properties.Resources.STRING_HAPTIC_ATTACHED;
            checkHapticsUserLoggedIn.Text = Properties.Resources.STRING_HAPTIC_CHARLOGGEDIN;
            checkLightbarClass.Text = Properties.Resources.STRING_HAPTIC_LBCLASS;
            checkLightbarHealth.Text = Properties.Resources.STRING_HAPTIC_LBHEALTH;
            checkRumbleDamage.Text = Properties.Resources.STRING_HAPTIC_RUMBLEDAMAGE;
            checkRumbleTarget.Text = Properties.Resources.STRING_HAPTIC_RUMBLETARGET;
            groupHapticSettings.Text = Properties.Resources.STRING_HAPTIC_SETTINGS;
            checkWindowAttached.Text = Properties.Resources.STRING_WOW_WINDOW_FOUND;
            labelChangeBindings.Text = Properties.Resources.STRING_BINDING_CHANGE;
            checkEnableAdvancedHaptics.Text = Properties.Resources.STRING_HAPTIC_ENABLE;
            labelTouchMode.Text = Properties.Resources.STRING_TOUCHPAD_MODE;
            comboTouchMode.Items[0] = Properties.Resources.STRING_TOUCHPAD_MOUSE;
            comboTouchMode.Items[1] = Properties.Resources.STRING_TOUCHPAD_BUTTONS;
            comboTouchMode.Items[2] = Properties.Resources.STRING_TOUCHPAD_SHARE_OPTIONS;
            checkMinTray.Text = Properties.Resources.STRING_SETTING_MIN_TRAY;
            checkCloseTray.Text = Properties.Resources.STRING_SETTING_CLOSE_TRAY;
            checkSendKeysDirect.Text = Properties.Resources.STRING_SETTING_KEY_DIRECT;
            checkDisableBG.Text = Properties.Resources.STRING_SETTING_INPUT_BGDISABLE;
            groupInteraction.Text = Properties.Resources.STRING_INTERACTION;
            groupInteractionSettings.Text = Properties.Resources.STRING_INTERACTION_SETTINGS;
            groupDS4CSettings.Text = Properties.Resources.STRING_DS4CSETTINGS;
            checkStartMinimized.Text = Properties.Resources.STRING_SETTING_START_MINIMIZED;
            picLStickUp.Image = Properties.Resources.LStickUp;
            picLStickDown.Image = Properties.Resources.LStickDown;
            picLStickLeft.Image = Properties.Resources.LStickLeft;
            picLStickRight.Image = Properties.Resources.LStickRight;
            picDS4.Image = Properties.Resources.DS4_Config;
            groupHaptics.Text = Properties.Resources.STRING_SETTING_HAPTICS;
            checkLightbarBattery.Text = Properties.Resources.STRING_LB_BATTERY;
            groupDS4Status.Text = Properties.Resources.STRING_GROUP_DS4_STATUS;
            groupWoWFolder.Text = Properties.Resources.STRING_GROUP_WOW_PATH;
            buttonLocateWoW.Text = Properties.Resources.STRING_FIND_WOW;

            new ToolTip().SetToolTip(picResetBinds, Properties.Resources.STRING_TOOLTIP_RESET_BINDS);

            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.ContextMenuStrip = menuNotify;

            // Set right stick panel to double buffered
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelRStickAxis, new object[] { true });

            if (!Functions.CheckWoWPath()) // check for valid install path
            {
                var findWoWNow = MessageBox.Show(Properties.Resources.STRING_NO_WOW_PATH, "DS4ConsolePort", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

            // Load settings
            settings.Load();
            checkEnableAdvancedHaptics.Checked = Properties.Settings.Default.EnableAdvancedHaptics;
            comboTouchMode.SelectedIndex = Properties.Settings.Default.TouchMode;
            touchState = Properties.Settings.Default.TouchMode;
            checkRumbleDamage.Checked = Properties.Settings.Default.EnableRumbleDamage;
            checkRumbleTarget.Checked = Properties.Settings.Default.EnableRumbleTarget;
            checkLightbarClass.Checked = Properties.Settings.Default.EnableLightbarClass;
            checkLightbarHealth.Checked = Properties.Settings.Default.EnableLightbarHealth;
            numRCurve.Value = Properties.Settings.Default.RStickCurve;
            numRDeadzone.Value = Properties.Settings.Default.RStickDeadzone;
            numRSpeed.Value = Properties.Settings.Default.RStickSpeed;
            intRightCurve = Properties.Settings.Default.RStickCurve;
            intRightDead = Properties.Settings.Default.RStickDeadzone;
            intRightSpeed = Properties.Settings.Default.RStickSpeed;
            checkCloseTray.Checked = Properties.Settings.Default.CloseToTray;
            checkMinTray.Checked = Properties.Settings.Default.MinToTray;
            checkDisableBG.Checked = Properties.Settings.Default.InactiveDisable;
            checkSendKeysDirect.Checked = Properties.Settings.Default.SendKeysDirect;
            checkStartMinimized.Checked = Properties.Settings.Default.StartMinimized;
            checkLightbarBattery.Checked = Properties.Settings.Default.LightbarBattery;
            textWoWFolder.Text = Properties.Settings.Default.WoWInstallPath;

            interaction = new WoWInteraction(settings.KeyBinds);

            movementThread = new Thread(MovementThread);
            movementThread.Priority = ThreadPriority.Highest;
            movementThread.Start();

            mouseThread = new Thread(MouseThread);
            mouseThread.Priority = ThreadPriority.Highest;
            mouseThread.Start();

            numRCurve.Value = intRightCurve;
            numRDeadzone.Value = intRightDead;
            numRSpeed.Value = intRightSpeed;

            // DoVersionCheck();
        }

        #endregion Public Constructors

        #region Public Methods

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

            if (true) // Todo: Checkforupdates
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
                        Console.WriteLine("Error parsing DS4ConsolePort version number");
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
                        } else if(addonVersion < v) // addon out of date
                        {
                            buttonCPUpdateNow.Visible = true;
                        }
                        
                    }
                }
                else
                {
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void BindBox_DoubleClick(object sender, EventArgs e)
        {
            var bindBox = sender as TextBox;

            // Attempt to find button name from tag
            DS4Button button;
            Enum.TryParse<DS4Button>(bindBox.Tag.ToString(), out button);

            DoKeyBind(bindBox,
                Properties.Resources.ResourceManager.GetObject(bindBox.Tag.ToString()) as Bitmap,
                button,
                settings.KeyBinds.FromName(bindBox.Tag.ToString()).Key.Value);
        }

        private void checkCloseTray_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CloseToTray = checkCloseTray.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkDisableBG_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.InactiveDisable = checkDisableBG.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkEnableAdvancedHaptics_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEnableAdvancedHaptics.Checked && !Properties.Settings.Default.EnableAdvancedHaptics)
            {
                checkEnableAdvancedHaptics.Checked = false;
                var hapwarn = new AdvHapWarningForm();
                var agreed = hapwarn.ShowDialog();
                if (agreed == DialogResult.OK) // agree
                {
                    Properties.Settings.Default.EnableAdvancedHaptics = true;
                    Properties.Settings.Default.Save();
                    checkEnableAdvancedHaptics.Checked = true;
                }
                else // no agree
                {
                    Properties.Settings.Default.EnableAdvancedHaptics = false;
                    Properties.Settings.Default.Save();
                    checkEnableAdvancedHaptics.Checked = false;
                }
            }
            else
            {
                Properties.Settings.Default.EnableAdvancedHaptics = checkEnableAdvancedHaptics.Checked;
                Properties.Settings.Default.Save();
            }
            UpdateHapticTab();
        }

        private void checkLightbarClass_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableLightbarClass = checkLightbarClass.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkLightbarHealth_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableLightbarHealth = checkLightbarHealth.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkMinTray_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MinToTray = checkMinTray.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkRumbleDamage_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableRumbleDamage = checkRumbleDamage.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkRumbleTarget_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableRumbleTarget = checkRumbleTarget.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkSendKeysDirect_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SendKeysDirect = checkSendKeysDirect.Checked;
            Properties.Settings.Default.Save();
            if (interaction != null)
                interaction.PostMessageKeys = checkSendKeysDirect.Checked;
        }

        private void checkSendMouseDirect_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = checkStartMinimized.Checked;
            Properties.Settings.Default.Save();
        }

        private void comboTouchMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTouchMode.SelectedIndex == 1)
            {
                textBindTouchLeft.Visible = true;
                textBindTouchRight.Visible = true;
            }
            else
            {
                textBindTouchLeft.Visible = false;
                textBindTouchRight.Visible = false;
            }
            labelTouchUpper.Visible = (comboTouchMode.SelectedIndex == 0) ? false : true;

            touchState = comboTouchMode.SelectedIndex;
            Properties.Settings.Default.TouchMode = comboTouchMode.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void DoKeyBind(TextBox BindBox, Bitmap Image, DS4Button Button, Keys Key)
        {
            var BindForm = new BindKeyForm(Image, Button, Key, settings.KeyBinds);
            BindForm.ShowDialog();
            if (BindForm.DialogResult == DialogResult.OK)
            {
                var bindKey = BindForm.Key;
                var swapWith = BindForm.SwapWith;
                if (swapWith != string.Empty)
                {
                    var swapBind = settings.KeyBinds.FromName(swapWith);
                    var oldBind = settings.KeyBinds.FromName(BindBox.Tag.ToString());
                    if (!swapBind.Equals(default(Binding)))
                    {
                        settings.KeyBinds.Update(swapBind.Name, oldBind.Key.Value);
                    }
                }
                BindBox.Text = bindKey.ToString();
                settings.KeyBinds.Update(BindBox.Tag.ToString(), bindKey);
                settings.Save();
                RefreshKeyBindings();
            }
        }

        private void DoTouchDown(DS4Button Button)
        {
            switch (Button)
            {
                case DS4Button.TouchLeft:
                    switch (touchState)
                    {
                        case 0:
                            interaction.SendMouseDown(MouseButtons.Left);
                            break;

                        case 1:
                            interaction.SendKeyDown(settings.KeyBinds.FromName("TouchLeft").Key.Value);
                            break;

                        case 2:
                            interaction.SendKeyDown(settings.KeyBinds.FromName("Share").Key.Value);
                            break;
                    }
                    break;

                case DS4Button.TouchRight:
                    switch (touchState)
                    {
                        case 0:
                            interaction.SendMouseDown(MouseButtons.Right);
                            break;

                        case 1:
                            interaction.SendKeyDown(settings.KeyBinds.FromName("TouchRight").Key.Value);
                            break;

                        case 2:
                            interaction.SendKeyDown(settings.KeyBinds.FromName("Options").Key.Value);
                            break;
                    }
                    break;
            }
        }

        private void DoTouchUp(DS4Button Button)
        {
            switch (Button)
            {
                case DS4Button.TouchLeft:
                    switch (touchState)
                    {
                        case 0:
                            interaction.SendMouseUp(MouseButtons.Left);
                            break;

                        case 1:
                            interaction.SendKeyUp(settings.KeyBinds.FromName("TouchLeft").Key.Value);
                            break;

                        case 2:
                            interaction.SendKeyUp(settings.KeyBinds.FromName("Share").Key.Value);
                            break;
                    }
                    break;

                case DS4Button.TouchRight:
                    switch (touchState)
                    {
                        case 0:
                            interaction.SendMouseUp(MouseButtons.Right);
                            break;

                        case 1:
                            interaction.SendKeyUp(settings.KeyBinds.FromName("TouchRight").Key.Value);
                            break;

                        case 2:
                            interaction.SendKeyUp(settings.KeyBinds.FromName("Options").Key.Value);
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
                touchState = Properties.Settings.Default.TouchMode;
            }
        }

        private void DS4_ButtonDown(DS4Button Button)
        {
            if (this != null && (interaction.IsAttached || !Properties.Settings.Default.InactiveDisable))
                switch (Button)
                {
                    case DS4Button.Cross:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("Cross").Key.Value);
                        break;

                    case DS4Button.Circle:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("Circle").Key.Value);
                        break;

                    case DS4Button.Triangle:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("Triangle").Key.Value);
                        break;

                    case DS4Button.Square:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("Square").Key.Value);
                        break;

                    case DS4Button.DpadDown:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("DpadDown").Key.Value);
                        break;

                    case DS4Button.DpadLeft:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("DpadLeft").Key.Value);
                        break;

                    case DS4Button.DpadRight:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("DpadRight").Key.Value);
                        break;

                    case DS4Button.DpadUp:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("DpadUp").Key.Value);
                        break;

                    case DS4Button.L1:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("L1").Key.Value);
                        break;

                    case DS4Button.R1:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("R1").Key.Value);
                        break;

                    case DS4Button.L2:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("L2").Key.Value);
                        break;

                    case DS4Button.R2:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("R2").Key.Value);
                        break;

                    case DS4Button.PS:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("PS").Key.Value);
                        break;

                    case DS4Button.Share:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("Share").Key.Value);
                        break;

                    case DS4Button.Options:
                        interaction.SendKeyDown(settings.KeyBinds.FromName("Options").Key.Value);
                        break;

                    case DS4Button.TouchLeft:
                        DoTouchDown(Button);
                        break;

                    case DS4Button.TouchRight:
                        DoTouchDown(Button);
                        break;

                    case DS4Button.TouchUpper:
                        DoTouchUpper();
                        break;

                    case DS4Button.L3:
                        interaction.SendMouseDown(MouseButtons.Left);
                        break;

                    case DS4Button.R3:
                        if (interaction.IsKeyDown(Keys.LControlKey))
                        {
                            var wowWidth = interaction.WoWWindow.Right - interaction.WoWWindow.Left;
                            var wowHeight = interaction.WoWWindow.Bottom - interaction.WoWWindow.Top;

                            Cursor.Position = new Point(interaction.WoWWindow.Left + (wowWidth / 2), interaction.WoWWindow.Top + (wowHeight / 2));
                        }
                        else
                        {
                            interaction.SendMouseDown(MouseButtons.Right);
                        }
                        break;
                }
        }

        private void DS4_ButtonUp(DS4Button Button)
        {
            if (this != null && (interaction.IsAttached || !Properties.Settings.Default.InactiveDisable))

                switch (Button)
                {
                    case DS4Button.Cross:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("Cross").Key.Value);
                        break;

                    case DS4Button.Circle:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("Circle").Key.Value);
                        break;

                    case DS4Button.Triangle:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("Triangle").Key.Value);
                        break;

                    case DS4Button.Square:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("Square").Key.Value);
                        break;

                    case DS4Button.DpadDown:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("DpadDown").Key.Value);
                        break;

                    case DS4Button.DpadLeft:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("DpadLeft").Key.Value);
                        break;

                    case DS4Button.DpadRight:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("DpadRight").Key.Value);
                        break;

                    case DS4Button.DpadUp:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("DpadUp").Key.Value);
                        break;

                    case DS4Button.L1:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("L1").Key.Value);
                        break;

                    case DS4Button.R1:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("R1").Key.Value);
                        break;

                    case DS4Button.L2:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("L2").Key.Value);
                        break;

                    case DS4Button.R2:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("R2").Key.Value);
                        break;

                    case DS4Button.PS:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("PS").Key.Value);
                        break;

                    case DS4Button.Share:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("Share").Key.Value);
                        break;

                    case DS4Button.Options:
                        interaction.SendKeyUp(settings.KeyBinds.FromName("Options").Key.Value);
                        break;

                    case DS4Button.L3:
                        interaction.SendMouseUp(MouseButtons.Left);
                        break;

                    case DS4Button.R3:
                        interaction.SendMouseUp(MouseButtons.Right);
                        break;

                    case DS4Button.TouchLeft:
                        DoTouchUp(Button);
                        break;

                    case DS4Button.TouchRight:
                        DoTouchUp(Button);
                        break;
                }
        }

        private void DS4_OnControllerConnected()
        {
            DS4.Controller.Touchpad.TouchesMoved += Touchpad_TouchesMoved; // add event for touchpad mouse
        }

        private void ExitApp()
        {
            Hide();
            notifyIcon.Visible = false;
            movementThread.Abort();
            mouseThread.Abort();

            if (advHaptics != null) advHaptics.Dispose();

            DS4.Dispose();
            interaction.Dispose();
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
            DS4 = new DS4();
            DS4.ControllerConnected += DS4_OnControllerConnected;
            DS4.ButtonDown += DS4_ButtonDown;
            DS4.ButtonUp += DS4_ButtonUp;
            DS4.TriggerSensitivity.L2 = Properties.Settings.Default.L2Sensitivity;
            DS4.TriggerSensitivity.R2 = Properties.Settings.Default.R2Sensitivity;

            RefreshKeyBindings();
            UpdateHapticTab();

            if (Properties.Settings.Default.StartMinimized) { Minimize(); }

            var vCheck = DoVersionCheck();

            Show();
            
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
                MessageBox.Show(String.Format(Properties.Resources.STRING_DS4_UPDATE_SUCCESS, assemblyVersion.ToString()), "DS4ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                Visible = false;
            }
        }

        private void MouseThread()
        {
            bool movingX = false;
            bool movingY = false;
            while (mouseThread.ThreadState == System.Threading.ThreadState.Running)
            {
                if ((interaction.IsAttached || !Properties.Settings.Default.InactiveDisable) && DS4 != null)
                {
                    var rightStick = DS4.GetStickPoint(DS4Stick.Right);
                    var curPos = Cursor.Position;

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

                        double moveMath = Math.Pow(intRightCurve * tiltPercent, 2) + (intRightSpeed * tiltPercent) + 1;

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

                        double moveMath = Math.Pow(intRightCurve * tiltPercent, 2) + (intRightSpeed * tiltPercent) + 1;

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
            while (movementThread.ThreadState == System.Threading.ThreadState.Running)
            {
                if (DS4 != null && (interaction.IsAttached))
                {
                    bool[] thisMove = new bool[4];
                    var leftStick = DS4.GetStickPoint(DS4Stick.Left);
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
                            interaction.SendKeyDown(settings.KeyBinds.FromName(GetKeyName(i)).Key.Value);
                            moveKeys[i] = true;
                        }
                        if (!thisMove[i] && moveKeys[i])
                        {
                            // send key up
                            interaction.SendKeyUp(settings.KeyBinds.FromName(GetKeyName(i)).Key.Value);
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

        private void numRCurve_ValueChanged(object sender, EventArgs e)
        {
            intRightCurve = (int)numRCurve.Value;
            Properties.Settings.Default.RStickCurve = intRightCurve;
            Properties.Settings.Default.Save();
        }

        private void numRDeadzone_ValueChanged(object sender, EventArgs e)
        {
            intRightDead = (int)numRDeadzone.Value;
            Properties.Settings.Default.RStickDeadzone = intRightDead;
            Properties.Settings.Default.Save();
        }

        private void numRSpeed_ValueChanged(object sender, EventArgs e)
        {
            intRightSpeed = (int)numRSpeed.Value;
            Properties.Settings.Default.RStickSpeed = intRightSpeed;
            Properties.Settings.Default.Save();
        }

        private void panelRStick_Paint(object sender, PaintEventArgs e)
        {
            var deadDisplay = intRightDead - 4;
            Rectangle rectRightBounds = panelRStickAxis.DisplayRectangle;
            Rectangle rectStickOutline = new Rectangle(
                rectRightBounds.X + 3,
                rectRightBounds.Y + 3,
                rectRightBounds.Width - 7,
                rectRightBounds.Height - 7);
            Rectangle rectStickDeadzone = new Rectangle(
                rectRightBounds.X + rectRightBounds.Width / 2 - (deadDisplay / 2),
                rectRightBounds.Y + rectRightBounds.Width / 2 - (deadDisplay / 2),
                deadDisplay,
                deadDisplay);
            Rectangle rectStickIndicator = new Rectangle(
                (rectRightBounds.Width / 2) + (ptRightStick.X / 4) - 2,
                (rectRightBounds.Height / 2) + (ptRightStick.Y / 4) - 2,
                4,
                4);

            e.Graphics.DrawEllipse(new Pen(Color.Black, 4f), rectStickOutline);
            e.Graphics.FillEllipse(Brushes.White, rectStickOutline);

            e.Graphics.DrawEllipse(Pens.Blue, rectStickIndicator);
            e.Graphics.FillEllipse(Brushes.Blue, rectStickIndicator);

            e.Graphics.DrawEllipse(Pens.Red, rectStickDeadzone);
        }

        private void picResetBinds_Click(object sender, EventArgs e)
        {
            var wMB = MessageBox.Show(Properties.Resources.STRING_WARN_RESET_BINDS, "DS4ConsolePort", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (wMB == DialogResult.No) return;
            settings.KeyBinds.Bindings = Defaults.Bindings;
            settings.Save();
            RefreshKeyBindings();
        }

        private void checkLightbarBattery_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LightbarBattery = checkLightbarBattery.Checked;
            Properties.Settings.Default.Save();
        }

        private void RefreshKeyBindings()
        {
            textL1.Text = settings.KeyBinds.FromName("L1").Key.Value.ToString();
            textL2.Text = settings.KeyBinds.FromName("L2").Key.Value.ToString();
            textR1.Text = settings.KeyBinds.FromName("R1").Key.Value.ToString();
            textR2.Text = settings.KeyBinds.FromName("R2").Key.Value.ToString();
            textDpadUp.Text = settings.KeyBinds.FromName("DpadUp").Key.Value.ToString();
            textDpadRight.Text = settings.KeyBinds.FromName("DpadRight").Key.Value.ToString();
            textDpadDown.Text = settings.KeyBinds.FromName("DpadDown").Key.Value.ToString();
            textDpadLeft.Text = settings.KeyBinds.FromName("DpadLeft").Key.Value.ToString();
            textTriangle.Text = settings.KeyBinds.FromName("Triangle").Key.Value.ToString();
            textCircle.Text = settings.KeyBinds.FromName("Circle").Key.Value.ToString();
            textCross.Text = settings.KeyBinds.FromName("Cross").Key.Value.ToString();
            textSquare.Text = settings.KeyBinds.FromName("Square").Key.Value.ToString();
            textPS.Text = settings.KeyBinds.FromName("PS").Key.Value.ToString();
            textShare.Text = settings.KeyBinds.FromName("Share").Key.Value.ToString();
            textOptions.Text = settings.KeyBinds.FromName("Options").Key.Value.ToString();
            textMoveForward.Text = settings.KeyBinds.FromName("LStickUp").Key.Value.ToString();
            textMoveRight.Text = settings.KeyBinds.FromName("LStickRight").Key.Value.ToString();
            textMoveBackward.Text = settings.KeyBinds.FromName("LStickDown").Key.Value.ToString();
            textMoveLeft.Text = settings.KeyBinds.FromName("LStickLeft").Key.Value.ToString();
            textBindTouchLeft.Text = settings.KeyBinds.FromName("TouchLeft").Key.Value.ToString();
            textBindTouchRight.Text = settings.KeyBinds.FromName("TouchRight").Key.Value.ToString();
        }

        private void buttonLocateWoW_Click(object sender, EventArgs e)
        {
            FolderSelectDialog fsd = new FolderSelectDialog();
            var res = fsd.ShowDialog();
            if (!res) return; // no folder selected
            textWoWFolder.Text = fsd.FileName;
            Properties.Settings.Default.WoWInstallPath = fsd.FileName;
            Properties.Settings.Default.Save();
        }

        private void ShowForm()
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void buttonCPUpdateNow_Click(object sender, EventArgs e)
        {
            if (Functions.CheckIsWowRunning())
            {
                MessageBox.Show(Properties.Resources.STRING_WOW_RUNNING_CLOSE, "DS4ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var cpVer = Github.GetLatestRelease2("seblindfors", "ConsolePort");
            DownloadForm dlf = new DownloadForm(cpVer.assets[0].browser_download_url);
            if (dlf.DialogResult == DialogResult.Cancel) return;

            var outFile = dlf.OutputFile;
            Functions.InstallAddOn(outFile);
            MessageBox.Show(string.Format(Properties.Resources.STRING_CP_UPDATE_SUCCESS, cpVer.tag_name), "DS4ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Information);
            File.Delete(outFile);
            var vCheck = DoVersionCheck();
        }

        private void labelDS4CP_Click(object sender, EventArgs e)
        {

        }

        private void ShowTriggerConfig()
        {
            TriggerConfigForm triggerConfig = new TriggerConfigForm(DS4.TriggerSensitivity.L2, DS4.TriggerSensitivity.R2);
            triggerConfig.ShowDialog();
            if (triggerConfig.DialogResult == DialogResult.Cancel) return;
            Properties.Settings.Default.L2Sensitivity = triggerConfig.L2Threshold;
            Properties.Settings.Default.R2Sensitivity = triggerConfig.R2Threshold;
            DS4.TriggerSensitivity.L2 = triggerConfig.L2Threshold;
            DS4.TriggerSensitivity.R2 = triggerConfig.R2Threshold;
            Properties.Settings.Default.Save();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowTriggerConfig();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowTriggerConfig();
        }

        private void buttonDS4UpdateNow_Click(object sender, EventArgs e)
        {
            if (Functions.CheckIsWowRunning())
            {
                MessageBox.Show(Properties.Resources.STRING_WOW_RUNNING_CLOSE, "DS4ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var dsVer = Github.GetLatestRelease2("topher-au", "DS4ConsolePort");
            DownloadForm dlf;
            try
            {
                dlf = new DownloadForm(dsVer.assets[0].browser_download_url);
            } catch
            {
                MessageBox.Show("Error","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (dlf.DialogResult == DialogResult.Cancel) return;

            var outFile = dlf.OutputFile;
            FastZip fz = new FastZip();
            fz.ExtractZip(outFile, ".", "DS4CP_Updater.exe");
            Process updater = new Process();
            updater.StartInfo.FileName = "DS4CP_Updater.exe";
            updater.StartInfo.Arguments = string.Format("-update \"{0}\"", outFile);
            updater.StartInfo.UseShellExecute = false;
            updater.Start();
            ExitApp();
        }

        private void UpdateAdvancedHaptics()
        {
            // Initialize haptics if not already
            if (Properties.Settings.Default.EnableAdvancedHaptics)
                try
                {
                    if (advHaptics == null)
                        advHaptics = new Haptics(DS4);
                }
                catch
                {
                    // Error loading advanced haptics, disable
                    advHaptics = null;
                    Properties.Settings.Default.EnableAdvancedHaptics = false;
                    Properties.Settings.Default.Save();
                    checkEnableAdvancedHaptics.Checked = false;
                    UpdateHapticTab();
                    MessageBox.Show(Properties.Resources.STRING_HAPTICS_DISABLED_ERROR, "DS4ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            // Update advanced haptic display
            if (Properties.Settings.Default.EnableAdvancedHaptics && advHaptics != null)
            {
                if (advHaptics.wowReader.IsAttached && !advHaptics.wowReader.OffsetsLoaded)
                {
                    // No suitable offsets found, disable haptics
                    Properties.Settings.Default.EnableAdvancedHaptics = false;
                    Properties.Settings.Default.Save();
                    checkEnableAdvancedHaptics.Checked = false;
                    UpdateHapticTab();
                    advHaptics.Dispose();
                    advHaptics = null;
                    MessageBox.Show(Properties.Resources.STRING_HAPTICS_DISABLED_NO_OFFSETS, "DS4ConsolePort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (interaction.IsAttached && advHaptics.wowReader.IsAttached && advHaptics.wowReader.OffsetsLoaded)
                {
                    advHaptics.Enabled = true;
                    advHaptics.LightbarClass = checkLightbarClass.Checked;
                    advHaptics.LightbarHealth = checkLightbarHealth.Checked;
                    advHaptics.RumbleOnTarget = checkRumbleTarget.Checked;
                    advHaptics.RumbleOnDamage = checkRumbleDamage.Checked;

                    checkHapticsAttached.Checked = advHaptics.IsWoWAttached;
                    if (advHaptics.IsWoWAttached)
                    {
                        if (advHaptics.GameState == WoWState.LoggedIn)
                        {
                            var pi = advHaptics.wowReader.ReadPlayerInfo();
                            labelPlayerInfo.Text = String.Format("{0} / {3} {4}\n{5}/{6}", pi.Name, pi.RealmName, pi.AccountName, pi.Level, pi.Class, pi.CurrentHP, pi.MaxHP);
                            checkHapticsUserLoggedIn.Checked = true;
                        }
                        else
                        {
                            checkHapticsUserLoggedIn.Checked = false;
                        }
                        checkHapticsUserLoggedIn.Checked = advHaptics.GameState == WoWState.LoggedIn ? true : false;
                    }
                }
                else
                {
                    checkHapticsAttached.Checked = false;
                    checkHapticsUserLoggedIn.Checked = false;
                    advHaptics.Enabled = false;
                }
            }
            else
            {
                if (advHaptics != null)
                {
                    advHaptics.Dispose();
                    advHaptics.Enabled = false;
                    advHaptics = null;
                }

                checkHapticsAttached.Checked = false;
                checkHapticsUserLoggedIn.Checked = false;
                labelPlayerInfo.Text = "";
            }
        }

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            if (DS4 != null)
            {
                // Display axis readings
                ptLeftStick = DS4.GetStickPoint(DS4Stick.Left);
                ptRightStick = DS4.GetStickPoint(DS4Stick.Right);

                checkWindowAttached.Checked = interaction.IsAttached;

                panelRStickAxis.Refresh();

                UpdateAdvancedHaptics();

                // update battery display
                if (DS4.Controller != null)
                {
                    var battery = DS4.Battery;
                    switch (DS4.Controller.ConnectionType)
                    {
                        case DS4Windows.ConnectionType.BT:
                            picConnectionType.Image = Properties.Resources.BT;
                            break;

                        case DS4Windows.ConnectionType.USB:
                            picConnectionType.Image = Properties.Resources.USB;
                            break;
                    }
                    labelConnectionStatus.Text = string.Format(Properties.Resources.STRING_CONTROLLER_CONNECTED, battery);

                    if (battery > 0 && battery < Properties.Settings.Default.LightbarBatteryThreshold && !DS4.Controller.LightBarColor.Equals(new DS4Color(Properties.Settings.Default.LightbarBatteryColor)))
                    {
                        if (advHaptics != null)
                        {
                            advHaptics.LightbarClass = false;
                            advHaptics.LightbarHealth = false;
                        }
                        DS4.Controller.LightBarColor = new DS4Color(Properties.Settings.Default.LightbarBatteryColor);
                        DS4.Controller.LightBarOffDuration = 100;
                        DS4.Controller.LightBarOnDuration = 100;
                    }
                    else
                    {
                        if (advHaptics != null)
                        {
                            advHaptics.LightbarClass = Properties.Settings.Default.EnableLightbarClass;
                            advHaptics.LightbarHealth = Properties.Settings.Default.EnableLightbarHealth;
                        }
                    }
                }
                else
                {
                    picConnectionType.Image = null;
                    labelConnectionStatus.Text = Properties.Resources.STRING_CONTROLLER_DISCONNECTED;
                }
            }
        }

        private void Touchpad_TouchesMoved(object sender, DS4Windows.TouchpadEventArgs e)
        {
            if (touchState == 0 && (interaction.IsAttached || !Properties.Settings.Default.InactiveDisable)) // Mouse Control
            {
                var x = e.touches[0].deltaX;
                var y = e.touches[0].deltaY;
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

        private void UpdateHapticTab()
        {
            if (!File.Exists("CPAdvancedHaptics.dll") || (!Environment.Is64BitProcess))
            {
                Properties.Settings.Default.EnableAdvancedHaptics = false;
                Properties.Settings.Default.Save();
                checkEnableAdvancedHaptics.Enabled = false;
            }

            checkEnableAdvancedHaptics.Checked = Properties.Settings.Default.EnableAdvancedHaptics;
            panelAdvancedHaptics.Enabled = Properties.Settings.Default.EnableAdvancedHaptics;
        }

        #endregion Private Methods
    }
}