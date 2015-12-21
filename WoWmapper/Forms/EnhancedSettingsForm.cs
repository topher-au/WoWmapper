using System;
using System.Windows.Forms;

namespace WoWmapper
{
    public partial class EnhancedSettingsForm : Form
    {
        public EnhancedSettingsForm()
        {
            InitializeComponent();

            // Initialize resources
            groupLightBar.Text = Properties.Resources.STRING_HAPTIC_LED;
            groupVibration.Text = Properties.Resources.STRING_HAPTIC_RUMBLE;
            groupMiscellaneous.Text = Properties.Resources.STRING_MISC;

            checkColorClass.Text = Properties.Resources.STRING_HAPTIC_LED_CLASS;
            checkColorHealth.Text = Properties.Resources.STRING_HAPTIC_LED_HEALTH;
            checkRumbleDamage.Text = Properties.Resources.STRING_HAPTIC_RUMBLE_DAMAGE;
            checkRumbleTarget.Text = Properties.Resources.STRING_HAPTIC_RUMBLE_TARGET;
            checkAutoCenter.Text = Properties.Resources.STRING_HAPTIC_AUTO_CENTER;

            labelCritical.Text = Properties.Resources.STRING_COLOR_CRIT;
            labelLow.Text = Properties.Resources.STRING_COLOR_LOW;
            labelMedium.Text = Properties.Resources.STRING_COLOR_MED;
            labelHigh.Text = Properties.Resources.STRING_COLOR_HIGH;

            // Initialize settings
            checkColorClass.Checked = Properties.Settings.Default.EnableLightbarClass;
            checkColorHealth.Checked = Properties.Settings.Default.EnableLightbarHealth;
            checkRumbleDamage.Checked = Properties.Settings.Default.EnableRumbleDamage;
            checkRumbleTarget.Checked = Properties.Settings.Default.EnableRumbleTarget;
            checkAutoCenter.Checked = Properties.Settings.Default.AutoCenter;

            panelColorCritical.BackColor = Properties.Settings.Default.ColorCritical;
            panelColorLow.BackColor = Properties.Settings.Default.ColorLow;
            panelColorMedium.BackColor = Properties.Settings.Default.ColorMedium;
            panelColorHigh.BackColor = Properties.Settings.Default.ColorHigh;
        }

        private void panelColorCritical_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog()
            {
                Color = Properties.Settings.Default.ColorCritical,
                FullOpen = true
            };
            var cdr = cd.ShowDialog();
            if (cdr != DialogResult.OK) return;
            Properties.Settings.Default.ColorCritical = cd.Color;
            Properties.Settings.Default.Save();
            panelColorCritical.BackColor = cd.Color;
        }

        private void panelColorLow_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog()
            {
                Color = Properties.Settings.Default.ColorLow,
                FullOpen = true
            };
            var cdr = cd.ShowDialog();
            if (cdr != DialogResult.OK) return;
            Properties.Settings.Default.ColorLow = cd.Color;
            Properties.Settings.Default.Save();
            panelColorLow.BackColor = cd.Color;
        }

        private void panelColorMedium_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog()
            {
                Color = Properties.Settings.Default.ColorMedium,
                FullOpen = true
            };
            var cdr = cd.ShowDialog();
            if (cdr != DialogResult.OK) return;
            Properties.Settings.Default.ColorMedium = cd.Color;
            Properties.Settings.Default.Save();
            panelColorMedium.BackColor = cd.Color;
        }

        private void panelColorHigh_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog()
            {
                Color = Properties.Settings.Default.ColorHigh,
                FullOpen = true
            };
            var cdr = cd.ShowDialog();
            if (cdr != DialogResult.OK) return;
            Properties.Settings.Default.ColorHigh = cd.Color;
            Properties.Settings.Default.Save();
            panelColorHigh.BackColor = cd.Color;
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

        private void checkColorClass_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableLightbarClass = checkColorClass.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkColorHealth_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableLightbarHealth = checkColorHealth.Checked;
            Properties.Settings.Default.Save();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkAutoCenter_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoCenter = checkAutoCenter.Checked;
            Properties.Settings.Default.Save();
        }
    }
}