using ConsolePort.AdvancedHaptics;
using ConsolePort.WoWData;
using DS4Wrapper;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public partial class MainForm : Form
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;

        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;

        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private Haptics advHaptics;
        bool[] moveKeys = new bool[4];

        private DS4 DS4;

        private int intRightCurve, intRightDead, intRightSpeed;

        private Thread mouseThread, movementThread;
        private Point ptLeftStick, ptRightStick;

        private Settings settings = new Settings();

        private WoWInteraction wowInteraction;

        private int touchState;

        private NotifyIcon notifyIcon = new NotifyIcon();

        public void DoVersionCheck()
        {
            // check addon version
            var wowPath = Functions.TryFindWoWPath();
            var addonVersion = Functions.CheckAddonVersion(wowPath, "ConsolePort");

            // check website version
        }

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
            checkSendMouseDirect.Text = Properties.Resources.STRING_SETTING_MOUSE_DIRECT;
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

            new ToolTip().SetToolTip(picResetBinds, Properties.Resources.STRING_TOOLTIP_RESET_BINDS);

            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            notifyIcon.ContextMenuStrip = menuNotify;

            // Set right stick panel to double buffered
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelRStickAxis, new object[] { true });

            typeof(TabPage).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, tabKeybinds, new object[] { true });

            typeof(TabPage).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, tabAdvanced, new object[] { true });

            typeof(TabPage).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, tabConsolePort, new object[] { true });

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
            checkSendMouseDirect.Checked = Properties.Settings.Default.SendMouseDirect;
            checkStartMinimized.Checked = Properties.Settings.Default.StartMinimized;

            wowInteraction = new WoWInteraction(settings.KeyBinds);

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

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        public void MouseLeftClick()
        {
            wowInteraction.SendClick(MouseButtons.Left);
        }

        public void MouseRightClick()
        {
            wowInteraction.SendClick(MouseButtons.Right);
        }

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

        private void MouseThread()
        {
            bool movingX = false;
            bool movingY = false;
            while (mouseThread.ThreadState == System.Threading.ThreadState.Running)
            {
                if ((wowInteraction.IsAttached || !Properties.Settings.Default.InactiveDisable) && DS4 != null)
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

        private void DoKeyBind(TextBox BindBox, Bitmap Image, DS4Button Button, Keys Key)
        {
            var BindForm = new BindKeyForm(Image, Button, Key, settings.KeyBinds);
            BindForm.ShowDialog();
            if (BindForm.DialogResult == DialogResult.OK)
            {
                var bindKey = BindForm.Key;
                var swapWith = BindForm.SwapWith;
                if(swapWith != string.Empty)
                {
                    var swapBind = settings.KeyBinds.FromName(swapWith);
                    var oldBind = settings.KeyBinds.FromName(BindBox.Tag.ToString());
                    if(!swapBind.Equals(default(Binding)))
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

        private void DS4_ButtonDown(DS4Button Button)
        {
            if (this != null)

                switch (Button)
                {
                    case DS4Button.Cross:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("Cross").Key.Value);
                        break;

                    case DS4Button.Circle:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("Circle").Key.Value);
                        break;

                    case DS4Button.Triangle:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("Triangle").Key.Value);
                        break;

                    case DS4Button.Square:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("Square").Key.Value);
                        break;

                    case DS4Button.DpadDown:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("DpadDown").Key.Value);
                        break;

                    case DS4Button.DpadLeft:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("DpadLeft").Key.Value);
                        break;

                    case DS4Button.DpadRight:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("DpadRight").Key.Value);
                        break;

                    case DS4Button.DpadUp:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("DpadUp").Key.Value);
                        break;

                    case DS4Button.L1:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("L1").Key.Value);
                        break;

                    case DS4Button.R1:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("R1").Key.Value);
                        break;

                    case DS4Button.L2:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("L2").Key.Value);
                        break;

                    case DS4Button.R2:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("R2").Key.Value);
                        break;

                    case DS4Button.PS:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("PS").Key.Value);
                        break;

                    case DS4Button.Share:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("Share").Key.Value);
                        break;

                    case DS4Button.Options:
                        wowInteraction.SendKeyDown(settings.KeyBinds.FromName("Options").Key.Value);
                        break;

                    case DS4Button.TouchLeft:
                        DoTouchLeft();
                        break;

                    case DS4Button.TouchRight:
                        DoTouchRight();
                        break;

                    case DS4Button.TouchUpper:
                        DoTouchUpper();
                        break;

                    case DS4Button.L3:
                        wowInteraction.DoMouseDown(MouseButtons.Left);
                        break;

                    case DS4Button.R3:
                        wowInteraction.DoMouseDown(MouseButtons.Right);
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

        private void DoTouchLeft()
        {
            switch (touchState)
            {
                case 0: // Mouse control
                    wowInteraction.SendClick(MouseButtons.Left);
                    break;

                case 1: // Touch left
                    wowInteraction.SendKeyDown(settings.KeyBinds.FromName("TouchLeft").Key.Value);
                    break;

                case 2: // Emulate share
                    wowInteraction.SendKeyPress(settings.KeyBinds.FromName("Share").Key.Value);
                    break;
            }
        }

        private void DoTouchRight()
        {
            switch (touchState)
            {
                case 0: // Mouse control
                    wowInteraction.SendClick(MouseButtons.Right);
                    break;

                case 1: // Touch right
                    wowInteraction.SendKeyDown(settings.KeyBinds.FromName("TouchRight").Key.Value);
                    break;

                case 2: // Emulate options
                    wowInteraction.SendKeyPress(settings.KeyBinds.FromName("Options").Key.Value);
                    break;
            }
        }

        private void DS4_ButtonUp(DS4Button Button)
        {
            if (this != null)

                switch (Button)
                {
                    case DS4Button.Cross:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("Cross").Key.Value);
                        break;

                    case DS4Button.Circle:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("Circle").Key.Value);
                        break;

                    case DS4Button.Triangle:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("Triangle").Key.Value);
                        break;

                    case DS4Button.Square:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("Square").Key.Value);
                        break;

                    case DS4Button.DpadDown:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("DpadDown").Key.Value);
                        break;

                    case DS4Button.DpadLeft:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("DpadLeft").Key.Value);
                        break;

                    case DS4Button.DpadRight:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("DpadRight").Key.Value);
                        break;

                    case DS4Button.DpadUp:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("DpadUp").Key.Value);
                        break;

                    case DS4Button.L1:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("L1").Key.Value);
                        break;

                    case DS4Button.R1:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("R1").Key.Value);
                        break;

                    case DS4Button.L2:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("L2").Key.Value);
                        break;

                    case DS4Button.R2:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("R2").Key.Value);
                        break;

                    case DS4Button.PS:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("PS").Key.Value);
                        break;

                    case DS4Button.Share:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("Share").Key.Value);
                        break;

                    case DS4Button.Options:
                        wowInteraction.SendKeyUp(settings.KeyBinds.FromName("Options").Key.Value);
                        break;

                    case DS4Button.L3:
                        wowInteraction.DoMouseUp(MouseButtons.Left);
                        break;

                    case DS4Button.R3:
                        wowInteraction.DoMouseUp(MouseButtons.Right);
                        break;

                    case DS4Button.TouchLeft:
                        if (touchState == 1)
                        {
                            wowInteraction.SendKeyUp(settings.KeyBinds.FromName("TouchLeft").Key.Value);
                        }
                        break;

                    case DS4Button.TouchRight:
                        if (touchState == 1)
                        {
                            wowInteraction.SendKeyUp(settings.KeyBinds.FromName("TouchRight").Key.Value);
                        }
                        break;
                }
        }

        private void DS4_OnControllerConnected()
        {
            this.Invoke((MethodInvoker)delegate
            {
                switch (DS4.Controller.ConnectionType)
                {
                    case DS4Windows.ConnectionType.BT:
                        labelControllerState.Image = Properties.Resources.BT;
                        break;

                    case DS4Windows.ConnectionType.USB:
                        labelControllerState.Image = Properties.Resources.USB;
                        break;
                }
                labelControllerState.Text = "Connected";
            });
            DS4.Controller.Touchpad.TouchesMoved += Touchpad_TouchesMoved;
        }

        private void DS4_OnControllerDisconnected()
        {
            this.Invoke((MethodInvoker)delegate
            {
                labelControllerState.Image = null;
                labelControllerState.Text = "Disconnected";
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // close to tray
            if(e.CloseReason == CloseReason.UserClosing && Properties.Settings.Default.CloseToTray)
            {
                WindowState = FormWindowState.Minimized;
                e.Cancel = true;
                return;
            }
            ExitApp();
        }

        private void ExitApp()
        {
            Hide();
            notifyIcon.Visible = false;
            movementThread.Abort();
            mouseThread.Abort();

            if (advHaptics != null) advHaptics.Dispose();

            DS4.Dispose();
            wowInteraction.Dispose();
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DS4 = new DS4();
            DS4.ControllerConnected += DS4_OnControllerConnected;
            DS4.ControllerDisconnected += DS4_OnControllerDisconnected;
            DS4.ButtonDown += DS4_ButtonDown;
            DS4.ButtonUp += DS4_ButtonUp;

            RefreshKeyBindings();
            UpdateHapticTab();

            if(Properties.Settings.Default.StartMinimized) WindowState = FormWindowState.Minimized;
            if(Properties.Settings.Default.MinToTray) ShowInTaskbar = false;
        }

        private int lastTouchX, lastTouchY;

        private void Touchpad_TouchesMoved(object sender, DS4Windows.TouchpadEventArgs e)
        {
            if (touchState == 0) // Mouse Control
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

        private void MovementThread()
        {
            
            while (movementThread.ThreadState == ThreadState.Running)
            {
                if (wowInteraction.IsAttached && DS4 != null)
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

                    for (int i = 0; i < 4; i++){
                        if(thisMove[i] && !moveKeys[i])
                        {
                            // send key down
                            wowInteraction.SendKeyDown(settings.KeyBinds.FromName(GetKeyName(i)).Key.Value);
                            moveKeys[i] = true;
                        }
                        if(!thisMove[i] && moveKeys[i])
                        {
                            // send key up
                            wowInteraction.SendKeyUp(settings.KeyBinds.FromName(GetKeyName(i)).Key.Value);
                            moveKeys[i] = false;
                        }
                    }
                }

                Thread.Sleep(10);
            }
        }

        private string GetKeyName(int i)
        {
            switch(i)
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

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            if (DS4 != null)
            {
                // Display axis readings
                ptLeftStick = DS4.GetStickPoint(DS4Stick.Left);
                ptRightStick = DS4.GetStickPoint(DS4Stick.Right);

                checkWindowAttached.Checked = wowInteraction.IsAttached;

                panelRStickAxis.Refresh();

                if (checkEnableAdvancedHaptics.Checked == true && advHaptics == null)
                {
                    advHaptics = new Haptics(DS4);
                    advHaptics.Enabled = true;
                }

                if (Properties.Settings.Default.EnableAdvancedHaptics)
                {
                    if (advHaptics != null && wowInteraction.IsAttached)
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
                                var pi = advHaptics.wowDataReader.GetPlayerInfo();
                                labelPlayerInfo.Text = String.Format("{0} / {3} {4}\n{5}/{6}", pi.Name, pi.RealmName, pi.AccountName, pi.Level, pi.Class, pi.CurrentHP, pi.MaxHP);
                                checkHapticsUserLoggedIn.Checked = true;
                            }
                            else
                            {
                                labelPlayerInfo.Text = string.Empty;
                                checkHapticsUserLoggedIn.Checked = false;
                            }
                            checkHapticsUserLoggedIn.Checked = advHaptics.GameState == WoWState.LoggedIn ? true : false;
                        }
                    }
                    else
                    {
                        checkHapticsAttached.Checked = false;
                        checkHapticsUserLoggedIn.Checked = false;
                        labelPlayerInfo.Text = "";
                    }
                }
                else
                {
                    if (advHaptics != null)
                    {
                        advHaptics.Enabled = false;
                        advHaptics.Dispose();
                        advHaptics = null;
                    }

                    checkHapticsAttached.Checked = false;
                    checkHapticsUserLoggedIn.Checked = false;
                    labelPlayerInfo.Text = "";
                }
            }
        }

        private void numRCurve_ValueChanged(object sender, EventArgs e)
        {
            intRightCurve = (int)numRCurve.Value;
            Properties.Settings.Default.RStickCurve = intRightCurve;
            Properties.Settings.Default.Save();
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

        private void checkRumbleTarget_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableRumbleTarget = checkRumbleTarget.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkRumbleDamage_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableRumbleDamage = checkRumbleDamage.Checked;
            Properties.Settings.Default.Save();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && Properties.Settings.Default.MinToTray)
            {
                // Minimize to tray
                ShowInTaskbar = false;
            }
            else
            {
                // Show from tray
                ShowInTaskbar = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void picResetBinds_Click(object sender, EventArgs e)
        {
            var wMB = MessageBox.Show(Properties.Resources.STRING_WARN_RESET_BINDS, "DS4ConsolePort", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (wMB == DialogResult.No) return;
            settings.KeyBinds.Bindings = Defaults.Bindings;
            settings.Save();
            RefreshKeyBindings();
        }

        private void checkMinTray_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MinToTray = checkMinTray.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkCloseTray_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CloseToTray = checkCloseTray.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkSendKeysDirect_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SendKeysDirect = checkSendKeysDirect.Checked;
            Properties.Settings.Default.Save();
            if(wowInteraction != null)
                wowInteraction.PostMessageKeys = checkSendKeysDirect.Checked;
        }

        private void checkSendMouseDirect_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SendMouseDirect = checkSendMouseDirect.Checked;
            Properties.Settings.Default.Save();
            if (wowInteraction != null)
                wowInteraction.PostMessageMouse = checkSendMouseDirect.Checked;
        }

        private void checkDisableBG_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.InactiveDisable = checkDisableBG.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = checkStartMinimized.Checked;
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

        private void UpdateHapticTab()
        {
            panelAdvancedHaptics.Enabled = Properties.Settings.Default.EnableAdvancedHaptics;
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
    }
}