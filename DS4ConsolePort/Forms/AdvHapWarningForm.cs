using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DS4ConsolePort
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

        private void linkMoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkMoreInfo.Text);
        }
    }
}