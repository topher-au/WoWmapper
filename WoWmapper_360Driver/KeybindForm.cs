using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WoWmapper.Input;
using SlimDX.XInput;

namespace WoWmapper_360Driver.Forms
{
    public partial class KeybindForm : Form
    {
        SettingsFile kb;
        Controller device;

        private void ShowTriggerConfig()
        {
            TriggerConfigForm triggerConfig = new TriggerConfigForm(device, 40,40);
            triggerConfig.ShowDialog();
            if (triggerConfig.DialogResult == DialogResult.Cancel) return;
            kb.Settings.Write("TriggerLeft", triggerConfig.L2Threshold);
            kb.Settings.Write("TriggerRight", triggerConfig.R2Threshold);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowTriggerConfig();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowTriggerConfig();
        }

        public Point GetStickPoint(InputStick Stick)
        {
            if (device != null)
            {
                var state = device.GetState();
                switch (Stick)
                {
                    case InputStick.Left:
                        return new Point()
                        {
                            X = state.Gamepad.LeftThumbX - 128,
                            Y = state.Gamepad.LeftThumbY - 128
                        };

                    case InputStick.Right:
                        return new Point()
                        {
                            X = (state.Gamepad.RightThumbX / 256),
                            Y = (-state.Gamepad.RightThumbY / 256)
                        };
                }
            }

            return new Point(0, 0);
        }

        public KeybindForm(Controller InputDevice, SettingsFile BindFile)
        {
            InitializeComponent();
            kb = BindFile;
            device = InputDevice;

            int rSpeed, rCurve, rDead, touchMode;
            kb.Settings.Read("RightDead", out rDead);
            kb.Settings.Read("RightCurve", out rCurve);
            kb.Settings.Read("RightSpeed", out rSpeed);
            kb.Settings.Read("TouchMode", out touchMode);

            numRCurve.Value = rCurve;
            numRSpeed.Value = rSpeed;
            numRDeadzone.Value = rDead;

            // Set right stick panel to double buffered
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelRStickAxis, new object[] { true });

            kb.Load();
            RefreshKeyBindings();
        }

        private void RefreshKeyBindings()
        {
            textL1.Text = kb.Keybinds.FromName("BumperLeft").Key.Value.ToString();
            textL2.Text = kb.Keybinds.FromName("TriggerLeft").Key.Value.ToString();
            textR1.Text = kb.Keybinds.FromName("BumperRight").Key.Value.ToString();
            textR2.Text = kb.Keybinds.FromName("TriggerRight").Key.Value.ToString();
            textDpadUp.Text = kb.Keybinds.FromName("LFaceUp").Key.Value.ToString();
            textDpadRight.Text = kb.Keybinds.FromName("LFaceRight").Key.Value.ToString();
            textDpadDown.Text = kb.Keybinds.FromName("LFaceDown").Key.Value.ToString();
            textDpadLeft.Text = kb.Keybinds.FromName("LFaceLeft").Key.Value.ToString();
            textTriangle.Text = kb.Keybinds.FromName("RFaceUp").Key.Value.ToString();
            textCircle.Text = kb.Keybinds.FromName("RFaceRight").Key.Value.ToString();
            textCross.Text = kb.Keybinds.FromName("RFaceDown").Key.Value.ToString();
            textSquare.Text = kb.Keybinds.FromName("RFaceLeft").Key.Value.ToString();
            textPS.Text = kb.Keybinds.FromName("CenterMiddle").Key.Value.ToString();
            textShare.Text = kb.Keybinds.FromName("CenterLeft").Key.Value.ToString();
            textOptions.Text = kb.Keybinds.FromName("CenterRight").Key.Value.ToString();
            textMoveForward.Text = kb.Keybinds.FromName("LStickUp").Key.Value.ToString();
            textMoveRight.Text = kb.Keybinds.FromName("LStickRight").Key.Value.ToString();
            textMoveBackward.Text = kb.Keybinds.FromName("LStickDown").Key.Value.ToString();
            textMoveLeft.Text = kb.Keybinds.FromName("LStickLeft").Key.Value.ToString();
        }

        private void numRCurve_ValueChanged(object sender, EventArgs e)
        {
            kb.Settings.Write<int>("RightCurve", (int)numRCurve.Value);
            kb.Save();
        }

        private void numRDeadzone_ValueChanged(object sender, EventArgs e)
        {
            kb.Settings.Write<int>("RightDead", (int)numRDeadzone.Value);
            kb.Save();
        }

        private void numRSpeed_ValueChanged(object sender, EventArgs e)
        {
            kb.Settings.Write<int>("RightSpeed", (int)numRSpeed.Value);
            kb.Save();
        }

        private void panelRStick_Paint(object sender, PaintEventArgs e)
        {
            int rightDead = 15;
            kb.Settings.Read<int>("RightDead", out rightDead);
            var deadDisplay = rightDead /2;
            var ptRightStick = GetStickPoint(InputStick.Right);
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
            var wMB = MessageBox.Show("reset all", "WoWmapper", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (wMB == DialogResult.No) return;
            kb.Keybinds.Bindings = Defaults.Bindings;
            RefreshKeyBindings();
        }


        private void DoKeyBind(TextBox BindBox, Bitmap Image, InputButton Button, Keys Key)
        {
            var BindForm = new BindKeyForm(Image, Button, Key, kb.Keybinds);
            BindForm.ShowDialog();
            if (BindForm.DialogResult == DialogResult.OK)
            {
                var bindKey = BindForm.Key;
                var swapWith = BindForm.SwapWith;
                if (swapWith != string.Empty)
                {
                    var swapBind = kb.Keybinds.FromName(swapWith);
                    var oldBind = kb.Keybinds.FromName(BindBox.Tag.ToString());
                    if (!swapBind.Equals(default(WoWmapper.Input.Binding)))
                    {
                        kb.Keybinds.Update(swapBind.Name, oldBind.Key.Value);
                    }
                }
                BindBox.Text = bindKey.ToString();
                kb.Keybinds.Update(BindBox.Tag.ToString(), bindKey);
                kb.Save();
                RefreshKeyBindings();
            }
        }

        private void BindBox_DoubleClick(object sender, EventArgs e)
        {
            var bindBox = sender as TextBox;

            // Attempt to find button name from tag
            InputButton button;
            var success = Enum.TryParse<InputButton>(bindBox.Tag.ToString(), out button);
            if (!success) return;

            DoKeyBind(bindBox,
                new Bitmap(32,32), //Properties.Resources.ResourceManager.GetObject(bindBox.Tag.ToString()) as Bitmap,
                button,
                kb.Keybinds.FromName(bindBox.Tag.ToString()).Key.Value);
        }

        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            panelRStickAxis.Refresh();
        }
    }
}
