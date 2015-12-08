using DS4Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public partial class TriggerConfigForm : Form
    {
        private DS4 Controller = new DS4();

        public int L2Threshold = 0;
        public int R2Threshold = 0;

        public TriggerConfigForm(int L2, int R2)
        {
            InitializeComponent();

            L2Threshold = L2;
            R2Threshold = R2;
            numL2.Value = L2;
            numR2.Value = R2;

            labelTriggerConfig.Text = Properties.Resources.STRING_TRIGGERS_ADJUST;
            buttonSave.Text = Properties.Resources.STRING_SAVE;
            buttonCancel.Text = Properties.Resources.STRING_CANCEL;
        }

        private void TriggerConfigForm_Load(object sender, EventArgs e)
        {

        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if(Controller != null)
            {
                var L2state = Controller.GetTriggerState(DS4Trigger.L2);
                var R2state = Controller.GetTriggerState(DS4Trigger.R2);
                trackL2.Value = (int)(L2state * 100f);
                trackR2.Value = (int)(R2state * 100f);
                checkL2.Checked = (trackL2.Value > numL2.Value);
                checkR2.Checked = (trackR2.Value > numR2.Value);
                
            } else
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
