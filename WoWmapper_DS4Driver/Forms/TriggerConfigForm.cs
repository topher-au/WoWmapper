using System;
using System.Windows.Forms;

namespace WoWmapper_DS4Driver.Forms
{
    public partial class TriggerConfigForm : Form
    {
        private DS4Wrapper.DS4 InputDevice;

        public int L2Threshold = 0;
        public int R2Threshold = 0;

        public TriggerConfigForm(DS4Wrapper.DS4 Controller, int L2, int R2)
        {
            InitializeComponent();
            InputDevice = Controller;
            L2Threshold = L2;
            R2Threshold = R2;
            numL2.Value = L2;
            numR2.Value = R2;

            labelTriggerConfig.Text = Properties.Resources.STRING_BIND_SET_TRIGGER;
            buttonSave.Text = Properties.Resources.STRING_BIND_SAVE;
            buttonCancel.Text = Properties.Resources.STRING_BIND_CANCEL;
        }

        private void TriggerConfigForm_Load(object sender, EventArgs e)
        {
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (InputDevice != null)
            {
                var L2state = InputDevice.GetTriggerState(DS4Wrapper.DS4Trigger.L2);
                var R2state = InputDevice.GetTriggerState(DS4Wrapper.DS4Trigger.R2);
                trackL2.Value = (int)(L2state * 100f);
                trackR2.Value = (int)(R2state * 100f);
                checkL2.Checked = (trackL2.Value > numL2.Value);
                checkR2.Checked = (trackR2.Value > numR2.Value);
            }
            else
            {
                trackL2.Value = 0;
                trackR2.Value = 0;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            L2Threshold = (int)numL2.Value;
            R2Threshold = (int)numR2.Value;
            DialogResult = DialogResult.OK;
        }
    }
}