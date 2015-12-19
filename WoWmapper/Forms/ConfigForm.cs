using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FolderSelect;
using WoWmapper.Input;

namespace WoWmapper
{
    public partial class ConfigForm : Form
    {
        IInputPlugin inputDevice;
        public ConfigForm(IInputPlugin InputDevice)
        {
            InitializeForm();

            inputDevice = InputDevice;
            checkLightbarColor.Enabled = inputDevice.Peripherals.LED;
            checkLightbarBatteryLow.Enabled = inputDevice.Peripherals.LED;
        }

        public ConfigForm()
        {
            InitializeForm();
        }

        public void InitializeForm()
        {
            InitializeComponent();

            // Load string resources
            labelWoWSettings.Text = Properties.Resources.STRING_SETTING_WOW;
            buttonLocateWoW.Text = Properties.Resources.STRING_SETTING_LOCATE_WOW;

            labelAppPrefs.Text = Properties.Resources.STRING_SETTING_APP_PREFS;
            checkStartMinimized.Text = Properties.Resources.STRING_SETTING_START_MINIMIZED;
            checkMinToTray.Text = Properties.Resources.STRING_SETTING_MIN_TRAY;
            checkCloseToTray.Text = Properties.Resources.STRING_SETTING_CLOSE_TRAY;
            checkDisableNoWoW.Text = Properties.Resources.STRING_SETTING_DISABLE_WOW_NOT_RUNNING;

            labelHapticSettings.Text = Properties.Resources.STRING_SETTING_HAPTIC;
            checkLightbarColor.Text = Properties.Resources.STRING_HAPTIC_LED;
            checkLightbarBatteryLow.Text = Properties.Resources.STRING_HAPTIC_LED_BATTERY;
            checkEnableAdvancedHaptics.Text = Properties.Resources.STRING_SETTING_ENABLE_ADVANCED_HAPTICS;
            buttonAdvancedHaptics.Text = Properties.Resources.STRING_SETTING_CONFIG_ADVANCED_HAPTICS;

            buttonDone.Text = Properties.Resources.STRING_SETTING_DONE;

            // Load user settings
            textWoWPath.Text = Properties.Settings.Default.WoWInstallPath;

            checkStartMinimized.Checked = Properties.Settings.Default.StartMinimized;
            checkMinToTray.Checked = Properties.Settings.Default.MinToTray;
            checkCloseToTray.Checked = Properties.Settings.Default.CloseToTray;
            checkDisableNoWoW.Checked = Properties.Settings.Default.InactiveDisable;

            checkLightbarColor.Checked = Properties.Settings.Default.ColorLightbar;
            panelLEDColor.BackColor = Properties.Settings.Default.ColorDefault;

            checkLightbarBatteryLow.Checked = Properties.Settings.Default.LightbarBattery;
            panelBatteryColor.BackColor = Properties.Settings.Default.LightbarBatteryColor;

            checkEnableAdvancedHaptics.Checked = Properties.Settings.Default.EnableAdvancedHaptics;
            buttonAdvancedHaptics.Enabled = Properties.Settings.Default.EnableAdvancedHaptics;
        }

        private void buttonAdvancedHaptics_Click(object sender, EventArgs e)
        {
            HapticSettingsForm hsf = new HapticSettingsForm();
            hsf.ShowDialog();
        }

        private void panelLEDColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog()
            {
                Color = Properties.Settings.Default.ColorDefault,
                FullOpen = true
            };
            var cdr = cd.ShowDialog();
            if (cdr != DialogResult.OK) return;
            Properties.Settings.Default.ColorDefault = cd.Color;
            Properties.Settings.Default.Save();
            panelLEDColor.BackColor = cd.Color;
        }

        private void checkLightbarColor_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ColorLightbar = checkLightbarColor.Checked;
        }

        private void checkLightbarBatteryLow_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LightbarBattery = checkLightbarBatteryLow.Checked;
        }

        private void checkEnableAdvancedHaptics_CheckedChanged(object sender, EventArgs e)
        {


            // User must agree to terms
            if(!Properties.Settings.Default.EnableAdvancedHaptics && checkEnableAdvancedHaptics.Checked == true)
            {
                AdvHapWarningForm hapWarn = new AdvHapWarningForm();
                var warnres = hapWarn.ShowDialog();
                if (warnres != DialogResult.OK)
                {
                    checkEnableAdvancedHaptics.Checked = false;
                    return;
                }
            }

            // Enable advanced haptics
            Properties.Settings.Default.EnableAdvancedHaptics = checkEnableAdvancedHaptics.Checked;
            buttonAdvancedHaptics.Enabled = Properties.Settings.Default.EnableAdvancedHaptics;
        }

        private void checkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = checkStartMinimized.Checked;
        }

        private void checkMinToTray_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MinToTray = checkMinToTray.Checked;
        }

        private void checkCloseToTray_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CloseToTray = checkCloseToTray.Checked;
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void panelBatteryColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog()
            {
                Color = Properties.Settings.Default.LightbarBatteryColor,
                FullOpen = true
            };
            var cdr = cd.ShowDialog();
            if (cdr != DialogResult.OK) return;
            Properties.Settings.Default.LightbarBatteryColor = cd.Color;
            Properties.Settings.Default.Save();
            panelBatteryColor.BackColor = cd.Color;
        }

        private void checkDisableNoWoW_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.InactiveDisable = checkDisableNoWoW.Checked;
        }

        private void buttonLocateWoW_Click(object sender, EventArgs e)
        {
            FolderSelectDialog fbd = new FolderSelectDialog();
            var res = fbd.ShowDialog();
            if (res != true) return;
            Properties.Settings.Default.WoWInstallPath = fbd.FileName;
            Properties.Settings.Default.Save();
            textWoWPath.Text = fbd.FileName;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
