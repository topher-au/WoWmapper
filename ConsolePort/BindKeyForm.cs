using DS4Wrapper;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public partial class BindKeyForm : Form
    {
        public Keys Key;
        private KeyBind bindings;

        public BindKeyForm(Bitmap ButtonImage, DS4Button Button, Keys CurrentKey, KeyBind Bindings)
        {
            InitializeComponent();
            picBindImage.Image = ButtonImage;
            textKeyBind.Text = CurrentKey.ToString();
            bindings = Bindings;
            labelBindName.Text = string.Format(Properties.Resources.STRING_BINDING_SET, Button.ToString());
        }

        private void textKeyBind_KeyDown(object sender, KeyEventArgs e)
        {
            // Illegal key code
            if (Defaults.IllegalKeyCodes.Contains(e.KeyCode))
            {
                labelKeyError.Text = string.Format(Properties.Resources.STRING_BINDING_ILLEGAL, e.KeyCode.ToString());
                return;
            }

            // Existing bind
            var existing = bindings.FromKey(e.KeyCode);
            if (!existing.Equals(default(Binding)))
            {
                labelKeyError.Text = String.Format(Properties.Resources.STRING_BINDING_EXISTS, e.KeyCode.ToString(), existing.Name);
                return;
            }

            // Bind success
            textKeyBind.Text = e.KeyCode.ToString();
            Key = e.KeyCode;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonShift_Click(object sender, EventArgs e)
        {
            // Existing bind
            var existing = bindings.FromKey(Keys.LShiftKey);
            if (!existing.Equals(default(Binding)))
            {
                labelKeyError.Text = String.Format(Properties.Resources.STRING_BINDING_EXISTS, Keys.LShiftKey.ToString(), existing.Name);
                return;
            }

            // Bind success
            textKeyBind.Text = Keys.LShiftKey.ToString();
            Key = Keys.LShiftKey;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonControl_Click(object sender, EventArgs e)
        {
            // Existing bind
            var existing = bindings.FromKey(Keys.LControlKey);
            if (!existing.Equals(default(Binding)))
            {
                labelKeyError.Text = String.Format(Properties.Resources.STRING_BINDING_EXISTS, Keys.LControlKey.ToString(), existing.Name);
                return;
            }

            // Bind success
            textKeyBind.Text = Keys.LControlKey.ToString();
            Key = Keys.LControlKey;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}