namespace WoWmapper
{
    partial class ConfigForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkCloseToTray = new System.Windows.Forms.CheckBox();
            this.checkMinToTray = new System.Windows.Forms.CheckBox();
            this.checkDisableNoWoW = new System.Windows.Forms.CheckBox();
            this.checkStartMinimized = new System.Windows.Forms.CheckBox();
            this.labelAppPrefs = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelBatteryColor = new System.Windows.Forms.Panel();
            this.buttonEnhancedSettings = new System.Windows.Forms.Button();
            this.panelLEDColor = new System.Windows.Forms.Panel();
            this.checkEnableAdvancedHaptics = new System.Windows.Forms.CheckBox();
            this.checkLightbarBatteryLow = new System.Windows.Forms.CheckBox();
            this.checkLightbarColor = new System.Windows.Forms.CheckBox();
            this.labelAdvancedSettings = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonLocateWoW = new System.Windows.Forms.Button();
            this.textWoWPath = new System.Windows.Forms.TextBox();
            this.labelWoWSettings = new System.Windows.Forms.Label();
            this.buttonDone = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkCloseToTray);
            this.panel1.Controls.Add(this.checkMinToTray);
            this.panel1.Controls.Add(this.checkDisableNoWoW);
            this.panel1.Controls.Add(this.checkStartMinimized);
            this.panel1.Controls.Add(this.labelAppPrefs);
            this.panel1.Location = new System.Drawing.Point(12, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 131);
            this.panel1.TabIndex = 0;
            // 
            // checkCloseToTray
            // 
            this.checkCloseToTray.AutoSize = true;
            this.checkCloseToTray.Location = new System.Drawing.Point(12, 84);
            this.checkCloseToTray.Name = "checkCloseToTray";
            this.checkCloseToTray.Size = new System.Drawing.Size(90, 19);
            this.checkCloseToTray.TabIndex = 3;
            this.checkCloseToTray.Text = "close to tray";
            this.checkCloseToTray.UseVisualStyleBackColor = true;
            this.checkCloseToTray.CheckedChanged += new System.EventHandler(this.checkCloseToTray_CheckedChanged);
            // 
            // checkMinToTray
            // 
            this.checkMinToTray.AutoSize = true;
            this.checkMinToTray.Location = new System.Drawing.Point(12, 59);
            this.checkMinToTray.Name = "checkMinToTray";
            this.checkMinToTray.Size = new System.Drawing.Size(112, 19);
            this.checkMinToTray.TabIndex = 2;
            this.checkMinToTray.Text = "minimize to tray";
            this.checkMinToTray.UseVisualStyleBackColor = true;
            this.checkMinToTray.CheckedChanged += new System.EventHandler(this.checkMinToTray_CheckedChanged);
            // 
            // checkDisableNoWoW
            // 
            this.checkDisableNoWoW.AutoSize = true;
            this.checkDisableNoWoW.Location = new System.Drawing.Point(12, 109);
            this.checkDisableNoWoW.Name = "checkDisableNoWoW";
            this.checkDisableNoWoW.Size = new System.Drawing.Size(189, 19);
            this.checkDisableNoWoW.TabIndex = 4;
            this.checkDisableNoWoW.Text = "disable when wow not running";
            this.checkDisableNoWoW.UseVisualStyleBackColor = true;
            this.checkDisableNoWoW.CheckedChanged += new System.EventHandler(this.checkDisableNoWoW_CheckedChanged);
            // 
            // checkStartMinimized
            // 
            this.checkStartMinimized.AutoSize = true;
            this.checkStartMinimized.Location = new System.Drawing.Point(12, 34);
            this.checkStartMinimized.Name = "checkStartMinimized";
            this.checkStartMinimized.Size = new System.Drawing.Size(108, 19);
            this.checkStartMinimized.TabIndex = 1;
            this.checkStartMinimized.Text = "start minimized";
            this.checkStartMinimized.UseVisualStyleBackColor = true;
            this.checkStartMinimized.CheckedChanged += new System.EventHandler(this.checkStartMinimized_CheckedChanged);
            // 
            // labelAppPrefs
            // 
            this.labelAppPrefs.AutoSize = true;
            this.labelAppPrefs.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAppPrefs.Location = new System.Drawing.Point(-2, 0);
            this.labelAppPrefs.Name = "labelAppPrefs";
            this.labelAppPrefs.Size = new System.Drawing.Size(220, 25);
            this.labelAppPrefs.TabIndex = 0;
            this.labelAppPrefs.Text = "application preferences";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelBatteryColor);
            this.panel2.Controls.Add(this.buttonEnhancedSettings);
            this.panel2.Controls.Add(this.panelLEDColor);
            this.panel2.Controls.Add(this.checkEnableAdvancedHaptics);
            this.panel2.Controls.Add(this.checkLightbarBatteryLow);
            this.panel2.Controls.Add(this.checkLightbarColor);
            this.panel2.Controls.Add(this.labelAdvancedSettings);
            this.panel2.Location = new System.Drawing.Point(12, 236);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 138);
            this.panel2.TabIndex = 1;
            // 
            // panelBatteryColor
            // 
            this.panelBatteryColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBatteryColor.Location = new System.Drawing.Point(189, 59);
            this.panelBatteryColor.Name = "panelBatteryColor";
            this.panelBatteryColor.Size = new System.Drawing.Size(25, 19);
            this.panelBatteryColor.TabIndex = 5;
            this.panelBatteryColor.Click += new System.EventHandler(this.panelBatteryColor_Click);
            // 
            // buttonEnhancedSettings
            // 
            this.buttonEnhancedSettings.Location = new System.Drawing.Point(12, 110);
            this.buttonEnhancedSettings.Name = "buttonEnhancedSettings";
            this.buttonEnhancedSettings.Size = new System.Drawing.Size(202, 23);
            this.buttonEnhancedSettings.TabIndex = 5;
            this.buttonEnhancedSettings.Text = "configure enhanced interaction";
            this.buttonEnhancedSettings.UseVisualStyleBackColor = true;
            this.buttonEnhancedSettings.Click += new System.EventHandler(this.buttonAdvancedHaptics_Click);
            // 
            // panelLEDColor
            // 
            this.panelLEDColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLEDColor.Location = new System.Drawing.Point(189, 34);
            this.panelLEDColor.Name = "panelLEDColor";
            this.panelLEDColor.Size = new System.Drawing.Size(25, 19);
            this.panelLEDColor.TabIndex = 4;
            this.panelLEDColor.Click += new System.EventHandler(this.panelLEDColor_Click);
            // 
            // checkEnableAdvancedHaptics
            // 
            this.checkEnableAdvancedHaptics.AutoSize = true;
            this.checkEnableAdvancedHaptics.Location = new System.Drawing.Point(12, 84);
            this.checkEnableAdvancedHaptics.Name = "checkEnableAdvancedHaptics";
            this.checkEnableAdvancedHaptics.Size = new System.Drawing.Size(176, 19);
            this.checkEnableAdvancedHaptics.TabIndex = 3;
            this.checkEnableAdvancedHaptics.Text = "enable enhanced interaction";
            this.checkEnableAdvancedHaptics.UseVisualStyleBackColor = true;
            this.checkEnableAdvancedHaptics.CheckedChanged += new System.EventHandler(this.checkEnableAdvancedHaptics_CheckedChanged);
            // 
            // checkLightbarBatteryLow
            // 
            this.checkLightbarBatteryLow.AutoSize = true;
            this.checkLightbarBatteryLow.Location = new System.Drawing.Point(12, 59);
            this.checkLightbarBatteryLow.Name = "checkLightbarBatteryLow";
            this.checkLightbarBatteryLow.Size = new System.Drawing.Size(149, 19);
            this.checkLightbarBatteryLow.TabIndex = 2;
            this.checkLightbarBatteryLow.Text = "flash lightbar when low";
            this.checkLightbarBatteryLow.UseVisualStyleBackColor = true;
            this.checkLightbarBatteryLow.CheckedChanged += new System.EventHandler(this.checkLightbarBatteryLow_CheckedChanged);
            // 
            // checkLightbarColor
            // 
            this.checkLightbarColor.AutoSize = true;
            this.checkLightbarColor.Location = new System.Drawing.Point(12, 34);
            this.checkLightbarColor.Name = "checkLightbarColor";
            this.checkLightbarColor.Size = new System.Drawing.Size(97, 19);
            this.checkLightbarColor.TabIndex = 1;
            this.checkLightbarColor.Text = "color lightbar";
            this.checkLightbarColor.UseVisualStyleBackColor = true;
            this.checkLightbarColor.CheckedChanged += new System.EventHandler(this.checkLightbarColor_CheckedChanged);
            // 
            // labelAdvancedSettings
            // 
            this.labelAdvancedSettings.AutoSize = true;
            this.labelAdvancedSettings.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdvancedSettings.Location = new System.Drawing.Point(-2, 0);
            this.labelAdvancedSettings.Name = "labelAdvancedSettings";
            this.labelAdvancedSettings.Size = new System.Drawing.Size(171, 25);
            this.labelAdvancedSettings.TabIndex = 0;
            this.labelAdvancedSettings.Text = "advanced settings";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonLocateWoW);
            this.panel3.Controls.Add(this.textWoWPath);
            this.panel3.Controls.Add(this.labelWoWSettings);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 83);
            this.panel3.TabIndex = 5;
            // 
            // buttonLocateWoW
            // 
            this.buttonLocateWoW.Location = new System.Drawing.Point(121, 57);
            this.buttonLocateWoW.Name = "buttonLocateWoW";
            this.buttonLocateWoW.Size = new System.Drawing.Size(93, 23);
            this.buttonLocateWoW.TabIndex = 8;
            this.buttonLocateWoW.Text = "locate wow";
            this.buttonLocateWoW.UseVisualStyleBackColor = true;
            this.buttonLocateWoW.Click += new System.EventHandler(this.buttonLocateWoW_Click);
            // 
            // textWoWPath
            // 
            this.textWoWPath.Location = new System.Drawing.Point(12, 28);
            this.textWoWPath.Name = "textWoWPath";
            this.textWoWPath.ReadOnly = true;
            this.textWoWPath.Size = new System.Drawing.Size(202, 23);
            this.textWoWPath.TabIndex = 7;
            // 
            // labelWoWSettings
            // 
            this.labelWoWSettings.AutoSize = true;
            this.labelWoWSettings.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWoWSettings.Location = new System.Drawing.Point(-2, 0);
            this.labelWoWSettings.Name = "labelWoWSettings";
            this.labelWoWSettings.Size = new System.Drawing.Size(128, 25);
            this.labelWoWSettings.TabIndex = 6;
            this.labelWoWSettings.Text = "wow settings";
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(165, 379);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(75, 23);
            this.buttonDone.TabIndex = 6;
            this.buttonDone.Text = "done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.buttonDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 413);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WoWmapper Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkCloseToTray;
        private System.Windows.Forms.CheckBox checkMinToTray;
        private System.Windows.Forms.CheckBox checkStartMinimized;
        private System.Windows.Forms.Label labelAppPrefs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelAdvancedSettings;
        private System.Windows.Forms.Button buttonEnhancedSettings;
        private System.Windows.Forms.Panel panelLEDColor;
        private System.Windows.Forms.CheckBox checkEnableAdvancedHaptics;
        private System.Windows.Forms.CheckBox checkLightbarBatteryLow;
        private System.Windows.Forms.CheckBox checkLightbarColor;
        private System.Windows.Forms.Panel panelBatteryColor;
        private System.Windows.Forms.CheckBox checkDisableNoWoW;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonLocateWoW;
        private System.Windows.Forms.TextBox textWoWPath;
        private System.Windows.Forms.Label labelWoWSettings;
        private System.Windows.Forms.Button buttonDone;
    }
}