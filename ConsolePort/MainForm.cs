using ConsolePort.WoWData;
using ConsolePort.AdvancedHaptics;
using DS4Wrapper;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace ConsolePort
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private int intRightDead = 15;
        private int intRightCurve = 5;
        private int intRightSpeed = 2;
        Point ptRightStick;
        Point ptLeftStick;
        public void MouseLeftClick()
        {
            wowInteraction.SendClick(WoWInteraction.MouseButton.Left);
        }

        public void MouseRightClick()
        {
            wowInteraction.SendClick(WoWInteraction.MouseButton.Right);
        }

        private DS4 DS4;
        private Haptics advHaptics;
        private Settings settings = new Settings();
        private WoWInteraction wowInteraction;
        private Thread movementThread;
        private Thread mouseThread;

        public MainForm()
        {
            InitializeComponent();

            // Set right stick panel to double buffered
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelRStickAxis, new object[] { true });

            // Enable advanced haptics
            if (Environment.GetCommandLineArgs().Length > 1)
                advHaptics = new Haptics(DS4);

            // Load default keybinds
            //settings.KeyBinds.Bindings = Defaults.Bindings;
            //settings.Save();
            settings.Load();

            wowInteraction = new WoWInteraction(settings.KeyBinds);

            movementThread = new Thread(MovementWatcher);
            movementThread.Start();

            mouseThread = new Thread(MouseWatcher);
            mouseThread.Start();

            labelRightCurveValue.Text = intRightCurve.ToString();
            labelRightDeadValue.Text = intRightDead.ToString();
            labelRightSpeedValue.Text = intRightSpeed.ToString();
            trackRightCurve.Value = intRightCurve;
            trackRightDead.Value = intRightDead;
            trackRightSpeed.Value = intRightSpeed;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DS4 = new DS4();
            
            DS4.ControllerConnected += () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    switch(DS4.Controller.ConnectionType)
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
                
            };
            DS4.ControllerDisconnected += () =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    labelControllerState.Image = null;
                    labelControllerState.Text = "Disconnected";
                });
            };
            DS4.ButtonDown += (DS4Button Button) =>
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

                        case DS4Button.L3:
                            MouseLeftClick();
                            break;

                        case DS4Button.R3:
                            MouseRightClick();
                            break;
                    }
            };

            DS4.ButtonUp += (DS4Button Button) =>
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
                    }
            };

            RefreshKeyBindings();
        }

        private void MovementWatcher()
        {
            while (true)
            {
                if(wowInteraction.IsAttached && DS4 != null)
                {
                    var leftStick = DS4.GetStickPoint(DS4Stick.Left);
                    if (leftStick.Y < -40)
                    {
                        wowInteraction.Move(WoWInteraction.Direction.Forward);
                    }

                    if (leftStick.Y > 40)
                    {
                        wowInteraction.Move(WoWInteraction.Direction.Backward);
                    }

                    if (leftStick.X < -40)
                    {
                        wowInteraction.Move(WoWInteraction.Direction.Left);
                    }

                    if (leftStick.X > 40)
                    {
                        wowInteraction.Move(WoWInteraction.Direction.Right);
                    }

                    if (leftStick.Y > -40)
                        if (leftStick.Y < 40)
                            wowInteraction.Move(WoWInteraction.Direction.StopY);

                    if (leftStick.X > -40)
                        if (leftStick.X < 40)
                            wowInteraction.Move(WoWInteraction.Direction.StopX);
                }
                
                Thread.Sleep(10);
            }
        }

        private void MouseWatcher()
        {
            bool movingX = false;
            bool movingY = false;
            while (true)
            {
                if(wowInteraction.IsAttached && DS4 != null)
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (advHaptics != null) advHaptics.Dispose();
            DS4.Dispose();
            wowInteraction.Dispose();
            movementThread.Abort();
            mouseThread.Abort();
        }

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            // Display axis readings
            ptLeftStick = DS4.GetStickPoint(DS4Stick.Left);
            ptRightStick = DS4.GetStickPoint(DS4Stick.Right);

            labelAxisReading.Text = string.Format("Lx: {0}\nLy: {1}\nRx: {2}\nRy: {3}", ptLeftStick.X, ptLeftStick.Y, ptRightStick.X, ptRightStick.Y);
            checkWindowAttached.Checked = wowInteraction.IsAttached;

            panelRStickAxis.Refresh();

            if (advHaptics != null)
            {
                checkHapticsAttached.Checked = advHaptics.IsWoWAttached;
                if (advHaptics.IsWoWAttached)
                {
                    if(advHaptics.GameState == WoWState.LoggedIn)
                    {
                        var pi = advHaptics.wowDataReader.GetPlayerInfo();
                        labelPlayerInfo.Text = String.Format("{0} - {1} ({2})\nLevel {3} {4}\n{5}/{6}", pi.Name, pi.RealmName, pi.AccountName, pi.Level, pi.Class, pi.CurrentHP, pi.MaxHP);
                    }
                    checkHapticsUserLoggedIn.Checked = advHaptics.GameState == WoWState.LoggedIn ? true : false;
                }
                
            }
            else
            {
                checkHapticsAttached.Checked = false;
                checkHapticsUserLoggedIn.Checked = false;
            }
            
        }

        private void trackRightCurve_Scroll(object sender, EventArgs e)
        {
            intRightCurve = trackRightCurve.Value;
            labelRightCurveValue.Text = intRightCurve.ToString();
        }

        private void trackRightDead_Scroll(object sender, EventArgs e)
        {
            intRightDead = trackRightDead.Value;
            labelRightDeadValue.Text = intRightDead.ToString();
        }

        private void tabKeybinds_Click(object sender, EventArgs e)
        {

        }

        private void trackRightSpeed_Scroll(object sender, EventArgs e)
        {
            intRightSpeed = trackRightSpeed.Value;
            labelRightSpeedValue.Text = intRightSpeed.ToString();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            var deadDisplay = intRightDead - 4;
            Rectangle rectRightBounds = panelRStickAxis.DisplayRectangle;
            Rectangle rectStickOutline = new Rectangle(
                rectRightBounds.X,
                rectRightBounds.Y,
                rectRightBounds.Width - 1,
                rectRightBounds.Height - 1);
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

            e.Graphics.DrawEllipse(Pens.Black, rectStickOutline);
            e.Graphics.DrawEllipse(Pens.Red, rectStickDeadzone);
            e.Graphics.DrawEllipse(Pens.Blue, rectStickIndicator);
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

        private void DoKeyBind(TextBox BindBox, Bitmap Image, DS4Button Button, Keys Key)
        {
            var BindForm = new BindKeyForm(Image, Button, Key, settings.KeyBinds);
            BindForm.ShowDialog();
            if (BindForm.DialogResult == DialogResult.OK)
            {
                var bindKey = BindForm.Key;
                BindBox.Text = bindKey.ToString();
                settings.KeyBinds.Update(BindBox.Tag.ToString(), bindKey);
                settings.Save();
            }
        }
    }
}