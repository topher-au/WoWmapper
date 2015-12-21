using System;
using System.Drawing;
using System.Windows.Forms;
using WoWmapper.Input;

namespace WoWmapper_DS4Driver.Forms
{
    public partial class BindKeyForm : Form
    {
        public Keys Key;
        public string SwapWith = string.Empty;
        private Keybind bindings;

        public BindKeyForm(Bitmap ButtonImage, string Button, Keys CurrentKey, Keybind Bindings)
        {
            InitializeComponent();
            picBindImage.Image = ButtonImage;
            textKeyBind.Text = CurrentKey.ToString();
            bindings = Bindings;
            labelBindName.Text = string.Format(Properties.Resources.STRING_BIND_SET, Button);
        }

        private void buttonControl_Click(object sender, EventArgs e)
        {
            // Existing bind
            var existing = bindings.FromKey(Keys.LControlKey);
            if (!existing.Equals(default(WoWmapper.Input.Binding)))
            {
                SwapWith = existing.Name;
            }

            // Bind success
            textKeyBind.Text = Keys.LControlKey.ToString();
            Key = Keys.LControlKey;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonShift_Click(object sender, EventArgs e)
        {
            // Existing bind
            var existing = bindings.FromKey(Keys.LShiftKey);
            if (!existing.Equals(default(WoWmapper.Input.Binding)))
            {
                SwapWith = existing.Name;
            }

            // Bind success
            textKeyBind.Text = Keys.LShiftKey.ToString();
            Key = Keys.LShiftKey;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textKeyBind_KeyDown(object sender, KeyEventArgs e)
        {
            // Illegal key code
            if (Defaults.IllegalKeyCodes.Contains(e.KeyCode))
            {
                labelKeyError.Text = string.Format(Properties.Resources.STRING_BIND_ILLEGAL, e.KeyCode.ToString());
                return;
            }

            // Existing bind
            var existing = bindings.FromKey(e.KeyCode);
            if (!existing.Equals(default(WoWmapper.Input.Binding)))
            {
                SwapWith = existing.Name;
            }

            // Bind success
            textKeyBind.Text = e.KeyCode.ToString();
            Key = e.KeyCode;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}