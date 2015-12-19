using WoWmapper.Input;
using System;
using System.Windows.Forms;
using SlimDX.XInput;

namespace WoWmapper_360Driver.Forms
{
    public partial class TriggerConfigForm : Form
    {
        private Controller InputDevice;

        public int L2Threshold = 0;
        public int R2Threshold = 0;

        public TriggerConfigForm(Controller Controller, int L2, int R2)
        {
            InitializeComponent();
            InputDevice = Controller;
            L2Threshold = L2;
            R2Threshold = R2;
            numL2.Value = L2;
            numR2.Value = R2;
        }

        private void TriggerConfigForm_Load(object sender, EventArgs e)
        {
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (InputDevice != null)
            {
                var state = InputDevice.GetState();
                var L2state = state.Gamepad.LeftTrigger;
                var R2state = state.Gamepad.RightTrigger;
                trackL2.Value = (int)(((float)L2state / 255) *100);
                trackR2.Value = (int)(((float)R2state / 255)*100);
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