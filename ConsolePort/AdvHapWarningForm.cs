using System;
using System.Windows.Forms;

namespace ConsolePort
{
    public partial class AdvHapWarningForm : Form
    {
        public AdvHapWarningForm()
        {
            InitializeComponent();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}