namespace ConsolePort
{
    partial class BindKeyForm
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
            this.labelBindName = new System.Windows.Forms.Label();
            this.picBindImage = new System.Windows.Forms.PictureBox();
            this.textKeyBind = new System.Windows.Forms.TextBox();
            this.labelKeyError = new System.Windows.Forms.Label();
            this.buttonControl = new System.Windows.Forms.Button();
            this.buttonShift = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBindImage)).BeginInit();
            this.SuspendLayout();
            // 
            // labelBindName
            // 
            this.labelBindName.Location = new System.Drawing.Point(14, 50);
            this.labelBindName.Name = "labelBindName";
            this.labelBindName.Size = new System.Drawing.Size(229, 22);
            this.labelBindName.TabIndex = 0;
            this.labelBindName.Text = "Press the key you wish to bind";
            this.labelBindName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picBindImage
            // 
            this.picBindImage.Location = new System.Drawing.Point(106, 4);
            this.picBindImage.Name = "picBindImage";
            this.picBindImage.Size = new System.Drawing.Size(48, 48);
            this.picBindImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBindImage.TabIndex = 1;
            this.picBindImage.TabStop = false;
            // 
            // textKeyBind
            // 
            this.textKeyBind.Location = new System.Drawing.Point(93, 111);
            this.textKeyBind.Name = "textKeyBind";
            this.textKeyBind.ReadOnly = true;
            this.textKeyBind.Size = new System.Drawing.Size(71, 23);
            this.textKeyBind.TabIndex = 2;
            this.textKeyBind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textKeyBind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textKeyBind_KeyDown);
            // 
            // labelKeyError
            // 
            this.labelKeyError.ForeColor = System.Drawing.Color.Red;
            this.labelKeyError.Location = new System.Drawing.Point(14, 72);
            this.labelKeyError.Name = "labelKeyError";
            this.labelKeyError.Size = new System.Drawing.Size(229, 36);
            this.labelKeyError.TabIndex = 3;
            this.labelKeyError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonControl
            // 
            this.buttonControl.Location = new System.Drawing.Point(170, 111);
            this.buttonControl.Name = "buttonControl";
            this.buttonControl.Size = new System.Drawing.Size(75, 23);
            this.buttonControl.TabIndex = 4;
            this.buttonControl.Text = "Control";
            this.buttonControl.UseVisualStyleBackColor = true;
            this.buttonControl.Click += new System.EventHandler(this.buttonControl_Click);
            // 
            // buttonShift
            // 
            this.buttonShift.Location = new System.Drawing.Point(12, 111);
            this.buttonShift.Name = "buttonShift";
            this.buttonShift.Size = new System.Drawing.Size(75, 23);
            this.buttonShift.TabIndex = 5;
            this.buttonShift.Text = "Shift";
            this.buttonShift.UseVisualStyleBackColor = true;
            this.buttonShift.Click += new System.EventHandler(this.buttonShift_Click);
            // 
            // BindKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 143);
            this.Controls.Add(this.buttonShift);
            this.Controls.Add(this.buttonControl);
            this.Controls.Add(this.labelKeyError);
            this.Controls.Add(this.textKeyBind);
            this.Controls.Add(this.picBindImage);
            this.Controls.Add(this.labelBindName);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BindKeyForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Key Binding";
            ((System.ComponentModel.ISupportInitialize)(this.picBindImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBindName;
        private System.Windows.Forms.PictureBox picBindImage;
        private System.Windows.Forms.TextBox textKeyBind;
        private System.Windows.Forms.Label labelKeyError;
        private System.Windows.Forms.Button buttonControl;
        private System.Windows.Forms.Button buttonShift;
    }
}