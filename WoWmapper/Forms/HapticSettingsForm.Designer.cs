namespace WoWmapper
{
    partial class HapticSettingsForm
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
            this.groupVibration = new System.Windows.Forms.GroupBox();
            this.labelRumbleIntensity = new System.Windows.Forms.Label();
            this.numTargetIntensity = new System.Windows.Forms.NumericUpDown();
            this.numDamageIntensity = new System.Windows.Forms.NumericUpDown();
            this.checkRumbleTarget = new System.Windows.Forms.CheckBox();
            this.checkRumbleDamage = new System.Windows.Forms.CheckBox();
            this.groupLightBar = new System.Windows.Forms.GroupBox();
            this.panelColorHigh = new System.Windows.Forms.Panel();
            this.panelColorMedium = new System.Windows.Forms.Panel();
            this.panelColorLow = new System.Windows.Forms.Panel();
            this.labelHigh = new System.Windows.Forms.Label();
            this.labelMedium = new System.Windows.Forms.Label();
            this.labelLow = new System.Windows.Forms.Label();
            this.labelCritical = new System.Windows.Forms.Label();
            this.checkColorHealth = new System.Windows.Forms.CheckBox();
            this.checkColorClass = new System.Windows.Forms.CheckBox();
            this.panelColorCritical = new System.Windows.Forms.Panel();
            this.groupMiscellaneous = new System.Windows.Forms.GroupBox();
            this.checkStickyCursor = new System.Windows.Forms.CheckBox();
            this.buttonDone = new System.Windows.Forms.Button();
            this.groupVibration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDamageIntensity)).BeginInit();
            this.groupLightBar.SuspendLayout();
            this.groupMiscellaneous.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupVibration
            // 
            this.groupVibration.Controls.Add(this.labelRumbleIntensity);
            this.groupVibration.Controls.Add(this.numTargetIntensity);
            this.groupVibration.Controls.Add(this.numDamageIntensity);
            this.groupVibration.Controls.Add(this.checkRumbleTarget);
            this.groupVibration.Controls.Add(this.checkRumbleDamage);
            this.groupVibration.Location = new System.Drawing.Point(14, 14);
            this.groupVibration.Name = "groupVibration";
            this.groupVibration.Size = new System.Drawing.Size(283, 81);
            this.groupVibration.TabIndex = 0;
            this.groupVibration.TabStop = false;
            this.groupVibration.Text = "vibration settings";
            // 
            // labelRumbleIntensity
            // 
            this.labelRumbleIntensity.AutoSize = true;
            this.labelRumbleIntensity.Location = new System.Drawing.Point(219, 0);
            this.labelRumbleIntensity.Name = "labelRumbleIntensity";
            this.labelRumbleIntensity.Size = new System.Drawing.Size(52, 15);
            this.labelRumbleIntensity.TabIndex = 4;
            this.labelRumbleIntensity.Text = "intensity";
            this.labelRumbleIntensity.Visible = false;
            // 
            // numTargetIntensity
            // 
            this.numTargetIntensity.Location = new System.Drawing.Point(222, 47);
            this.numTargetIntensity.Name = "numTargetIntensity";
            this.numTargetIntensity.Size = new System.Drawing.Size(50, 23);
            this.numTargetIntensity.TabIndex = 3;
            this.numTargetIntensity.Visible = false;
            this.numTargetIntensity.ValueChanged += new System.EventHandler(this.numTargetIntensity_ValueChanged);
            // 
            // numDamageIntensity
            // 
            this.numDamageIntensity.Location = new System.Drawing.Point(222, 21);
            this.numDamageIntensity.Name = "numDamageIntensity";
            this.numDamageIntensity.Size = new System.Drawing.Size(50, 23);
            this.numDamageIntensity.TabIndex = 2;
            this.numDamageIntensity.Visible = false;
            this.numDamageIntensity.ValueChanged += new System.EventHandler(this.numDamageIntensity_ValueChanged);
            // 
            // checkRumbleTarget
            // 
            this.checkRumbleTarget.AutoSize = true;
            this.checkRumbleTarget.Location = new System.Drawing.Point(7, 48);
            this.checkRumbleTarget.Name = "checkRumbleTarget";
            this.checkRumbleTarget.Size = new System.Drawing.Size(183, 19);
            this.checkRumbleTarget.TabIndex = 1;
            this.checkRumbleTarget.Text = "rumble when changing target";
            this.checkRumbleTarget.UseVisualStyleBackColor = true;
            this.checkRumbleTarget.CheckedChanged += new System.EventHandler(this.checkRumbleTarget_CheckedChanged);
            // 
            // checkRumbleDamage
            // 
            this.checkRumbleDamage.AutoSize = true;
            this.checkRumbleDamage.Location = new System.Drawing.Point(7, 22);
            this.checkRumbleDamage.Name = "checkRumbleDamage";
            this.checkRumbleDamage.Size = new System.Drawing.Size(178, 19);
            this.checkRumbleDamage.TabIndex = 0;
            this.checkRumbleDamage.Text = "rumble when taking damage";
            this.checkRumbleDamage.UseVisualStyleBackColor = true;
            this.checkRumbleDamage.CheckedChanged += new System.EventHandler(this.checkRumbleDamage_CheckedChanged);
            // 
            // groupLightBar
            // 
            this.groupLightBar.Controls.Add(this.panelColorHigh);
            this.groupLightBar.Controls.Add(this.panelColorMedium);
            this.groupLightBar.Controls.Add(this.panelColorLow);
            this.groupLightBar.Controls.Add(this.labelHigh);
            this.groupLightBar.Controls.Add(this.labelMedium);
            this.groupLightBar.Controls.Add(this.labelLow);
            this.groupLightBar.Controls.Add(this.labelCritical);
            this.groupLightBar.Controls.Add(this.checkColorHealth);
            this.groupLightBar.Controls.Add(this.checkColorClass);
            this.groupLightBar.Controls.Add(this.panelColorCritical);
            this.groupLightBar.Location = new System.Drawing.Point(14, 102);
            this.groupLightBar.Name = "groupLightBar";
            this.groupLightBar.Size = new System.Drawing.Size(283, 125);
            this.groupLightBar.TabIndex = 1;
            this.groupLightBar.TabStop = false;
            this.groupLightBar.Text = "lightbar settings";
            // 
            // panelColorHigh
            // 
            this.panelColorHigh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorHigh.Location = new System.Drawing.Point(185, 90);
            this.panelColorHigh.Name = "panelColorHigh";
            this.panelColorHigh.Size = new System.Drawing.Size(25, 25);
            this.panelColorHigh.TabIndex = 8;
            this.panelColorHigh.Click += new System.EventHandler(this.panelColorHigh_Click);
            // 
            // panelColorMedium
            // 
            this.panelColorMedium.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorMedium.Location = new System.Drawing.Point(136, 90);
            this.panelColorMedium.Name = "panelColorMedium";
            this.panelColorMedium.Size = new System.Drawing.Size(25, 25);
            this.panelColorMedium.TabIndex = 1;
            this.panelColorMedium.Click += new System.EventHandler(this.panelColorMedium_Click);
            // 
            // panelColorLow
            // 
            this.panelColorLow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorLow.Location = new System.Drawing.Point(94, 90);
            this.panelColorLow.Name = "panelColorLow";
            this.panelColorLow.Size = new System.Drawing.Size(25, 25);
            this.panelColorLow.TabIndex = 7;
            this.panelColorLow.Click += new System.EventHandler(this.panelColorLow_Click);
            // 
            // labelHigh
            // 
            this.labelHigh.AutoSize = true;
            this.labelHigh.Location = new System.Drawing.Point(184, 72);
            this.labelHigh.Name = "labelHigh";
            this.labelHigh.Size = new System.Drawing.Size(31, 15);
            this.labelHigh.TabIndex = 6;
            this.labelHigh.Text = "high";
            // 
            // labelMedium
            // 
            this.labelMedium.AutoSize = true;
            this.labelMedium.Location = new System.Drawing.Point(127, 72);
            this.labelMedium.Name = "labelMedium";
            this.labelMedium.Size = new System.Drawing.Size(52, 15);
            this.labelMedium.TabIndex = 5;
            this.labelMedium.Text = "medium";
            // 
            // labelLow
            // 
            this.labelLow.AutoSize = true;
            this.labelLow.Location = new System.Drawing.Point(93, 72);
            this.labelLow.Name = "labelLow";
            this.labelLow.Size = new System.Drawing.Size(26, 15);
            this.labelLow.TabIndex = 4;
            this.labelLow.Text = "low";
            // 
            // labelCritical
            // 
            this.labelCritical.AutoSize = true;
            this.labelCritical.Location = new System.Drawing.Point(43, 72);
            this.labelCritical.Name = "labelCritical";
            this.labelCritical.Size = new System.Drawing.Size(42, 15);
            this.labelCritical.TabIndex = 3;
            this.labelCritical.Text = "critical";
            // 
            // checkColorHealth
            // 
            this.checkColorHealth.AutoSize = true;
            this.checkColorHealth.Location = new System.Drawing.Point(7, 48);
            this.checkColorHealth.Name = "checkColorHealth";
            this.checkColorHealth.Size = new System.Drawing.Size(180, 19);
            this.checkColorHealth.TabIndex = 2;
            this.checkColorHealth.Text = "color lightbar by health value";
            this.checkColorHealth.UseVisualStyleBackColor = true;
            this.checkColorHealth.CheckedChanged += new System.EventHandler(this.checkColorHealth_CheckedChanged);
            // 
            // checkColorClass
            // 
            this.checkColorClass.AutoSize = true;
            this.checkColorClass.Location = new System.Drawing.Point(7, 22);
            this.checkColorClass.Name = "checkColorClass";
            this.checkColorClass.Size = new System.Drawing.Size(141, 19);
            this.checkColorClass.TabIndex = 1;
            this.checkColorClass.Text = "color lightbar by class";
            this.checkColorClass.UseVisualStyleBackColor = true;
            this.checkColorClass.CheckedChanged += new System.EventHandler(this.checkColorClass_CheckedChanged);
            // 
            // panelColorCritical
            // 
            this.panelColorCritical.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelColorCritical.Location = new System.Drawing.Point(51, 90);
            this.panelColorCritical.Name = "panelColorCritical";
            this.panelColorCritical.Size = new System.Drawing.Size(25, 25);
            this.panelColorCritical.TabIndex = 0;
            this.panelColorCritical.Click += new System.EventHandler(this.panelColorCritical_Click);
            // 
            // groupMiscellaneous
            // 
            this.groupMiscellaneous.Controls.Add(this.checkStickyCursor);
            this.groupMiscellaneous.Location = new System.Drawing.Point(14, 233);
            this.groupMiscellaneous.Name = "groupMiscellaneous";
            this.groupMiscellaneous.Size = new System.Drawing.Size(283, 48);
            this.groupMiscellaneous.TabIndex = 2;
            this.groupMiscellaneous.TabStop = false;
            this.groupMiscellaneous.Text = "miscellaneous";
            // 
            // checkStickyCursor
            // 
            this.checkStickyCursor.AutoSize = true;
            this.checkStickyCursor.Location = new System.Drawing.Point(7, 22);
            this.checkStickyCursor.Name = "checkStickyCursor";
            this.checkStickyCursor.Size = new System.Drawing.Size(130, 19);
            this.checkStickyCursor.TabIndex = 0;
            this.checkStickyCursor.Text = "enable sticky cursor";
            this.checkStickyCursor.UseVisualStyleBackColor = true;
            this.checkStickyCursor.CheckedChanged += new System.EventHandler(this.checkStickyCursor_CheckedChanged);
            // 
            // buttonDone
            // 
            this.buttonDone.Location = new System.Drawing.Point(210, 288);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new System.Drawing.Size(87, 27);
            this.buttonDone.TabIndex = 3;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // HapticSettingsForm
            // 
            this.AcceptButton = this.buttonDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 327);
            this.Controls.Add(this.buttonDone);
            this.Controls.Add(this.groupMiscellaneous);
            this.Controls.Add(this.groupLightBar);
            this.Controls.Add(this.groupVibration);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HapticSettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Haptics";
            this.groupVibration.ResumeLayout(false);
            this.groupVibration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTargetIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDamageIntensity)).EndInit();
            this.groupLightBar.ResumeLayout(false);
            this.groupLightBar.PerformLayout();
            this.groupMiscellaneous.ResumeLayout(false);
            this.groupMiscellaneous.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupVibration;
        private System.Windows.Forms.Label labelRumbleIntensity;
        private System.Windows.Forms.NumericUpDown numTargetIntensity;
        private System.Windows.Forms.NumericUpDown numDamageIntensity;
        private System.Windows.Forms.CheckBox checkRumbleTarget;
        private System.Windows.Forms.CheckBox checkRumbleDamage;
        private System.Windows.Forms.GroupBox groupLightBar;
        private System.Windows.Forms.Panel panelColorHigh;
        private System.Windows.Forms.Panel panelColorMedium;
        private System.Windows.Forms.Panel panelColorLow;
        private System.Windows.Forms.Label labelHigh;
        private System.Windows.Forms.Label labelMedium;
        private System.Windows.Forms.Label labelLow;
        private System.Windows.Forms.Label labelCritical;
        private System.Windows.Forms.CheckBox checkColorHealth;
        private System.Windows.Forms.CheckBox checkColorClass;
        private System.Windows.Forms.Panel panelColorCritical;
        private System.Windows.Forms.GroupBox groupMiscellaneous;
        private System.Windows.Forms.CheckBox checkStickyCursor;
        private System.Windows.Forms.Button buttonDone;
    }
}