namespace Updater
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
            this.LabelUpdateStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelUpdateStatus
            // 
            this.LabelUpdateStatus.Location = new System.Drawing.Point(12, 12);
            this.LabelUpdateStatus.Name = "LabelUpdateStatus";
            this.LabelUpdateStatus.Size = new System.Drawing.Size(280, 80);
            this.LabelUpdateStatus.TabIndex = 0;
            this.LabelUpdateStatus.Text = "Update status";
            this.LabelUpdateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 101);
            this.ControlBox = false;
            this.Controls.Add(this.LabelUpdateStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WoWmapper Update";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabelUpdateStatus;
    }
}

