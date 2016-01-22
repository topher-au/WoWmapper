namespace WoWmapper_Updater
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelInstallText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelInstallText
            // 
            this.labelInstallText.Location = new System.Drawing.Point(12, 9);
            this.labelInstallText.Name = "labelInstallText";
            this.labelInstallText.Size = new System.Drawing.Size(285, 74);
            this.labelInstallText.TabIndex = 1;
            this.labelInstallText.Text = "Installing";
            this.labelInstallText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 92);
            this.ControlBox = false;
            this.Controls.Add(this.labelInstallText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WoWmapper Updater";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelInstallText;
    }
}

