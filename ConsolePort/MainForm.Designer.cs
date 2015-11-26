namespace ConsolePort
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
            this.components = new System.ComponentModel.Container();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.labelAxisReading = new System.Windows.Forms.Label();
            this.checkWindowAttached = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.labelSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelControllerState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConsolePort = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupAxisReading = new System.Windows.Forms.GroupBox();
            this.labelRightCurveValue = new System.Windows.Forms.Label();
            this.labelRightDeadValue = new System.Windows.Forms.Label();
            this.trackRightCurve = new System.Windows.Forms.TrackBar();
            this.trackRightDead = new System.Windows.Forms.TrackBar();
            this.tabKeybinds = new System.Windows.Forms.TabPage();
            this.statusBar.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabConsolePort.SuspendLayout();
            this.groupAxisReading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackRightCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRightDead)).BeginInit();
            this.SuspendLayout();
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 5;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // labelAxisReading
            // 
            this.labelAxisReading.AutoSize = true;
            this.labelAxisReading.Location = new System.Drawing.Point(6, 19);
            this.labelAxisReading.Name = "labelAxisReading";
            this.labelAxisReading.Size = new System.Drawing.Size(49, 60);
            this.labelAxisReading.TabIndex = 1;
            this.labelAxisReading.Text = "Lx: 127\r\nLy: -127\r\nRx: 127\r\nRy: -127";
            // 
            // checkWindowAttached
            // 
            this.checkWindowAttached.AutoSize = true;
            this.checkWindowAttached.Location = new System.Drawing.Point(6, 6);
            this.checkWindowAttached.Name = "checkWindowAttached";
            this.checkWindowAttached.Size = new System.Drawing.Size(135, 19);
            this.checkWindowAttached.TabIndex = 2;
            this.checkWindowAttached.Text = "WoW window found";
            this.checkWindowAttached.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(6, 31);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(169, 19);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "Advanced haptics attached";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(32, 56);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(130, 19);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.Text = "Character logged in";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelSpacer,
            this.labelControllerState});
            this.statusBar.Location = new System.Drawing.Point(0, 295);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(472, 25);
            this.statusBar.TabIndex = 5;
            this.statusBar.Text = "statusStrip1";
            // 
            // labelSpacer
            // 
            this.labelSpacer.Name = "labelSpacer";
            this.labelSpacer.Size = new System.Drawing.Size(322, 20);
            this.labelSpacer.Spring = true;
            // 
            // labelControllerState
            // 
            this.labelControllerState.Image = global::ConsolePort.Properties.Resources.BT;
            this.labelControllerState.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.labelControllerState.Name = "labelControllerState";
            this.labelControllerState.Size = new System.Drawing.Size(104, 20);
            this.labelControllerState.Text = "Connected";
            this.labelControllerState.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabConsolePort);
            this.tabControl.Controls.Add(this.tabKeybinds);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(455, 269);
            this.tabControl.TabIndex = 6;
            // 
            // tabConsolePort
            // 
            this.tabConsolePort.Controls.Add(this.label2);
            this.tabConsolePort.Controls.Add(this.label1);
            this.tabConsolePort.Controls.Add(this.groupAxisReading);
            this.tabConsolePort.Controls.Add(this.labelRightCurveValue);
            this.tabConsolePort.Controls.Add(this.labelRightDeadValue);
            this.tabConsolePort.Controls.Add(this.trackRightCurve);
            this.tabConsolePort.Controls.Add(this.trackRightDead);
            this.tabConsolePort.Controls.Add(this.checkBox3);
            this.tabConsolePort.Controls.Add(this.checkWindowAttached);
            this.tabConsolePort.Controls.Add(this.checkBox2);
            this.tabConsolePort.Location = new System.Drawing.Point(4, 24);
            this.tabConsolePort.Name = "tabConsolePort";
            this.tabConsolePort.Padding = new System.Windows.Forms.Padding(3);
            this.tabConsolePort.Size = new System.Drawing.Size(447, 241);
            this.tabConsolePort.TabIndex = 0;
            this.tabConsolePort.Text = "ConsolePort";
            this.tabConsolePort.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Right stick curve";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Right stick deadzone";
            // 
            // groupAxisReading
            // 
            this.groupAxisReading.Controls.Add(this.labelAxisReading);
            this.groupAxisReading.Location = new System.Drawing.Point(308, 6);
            this.groupAxisReading.Name = "groupAxisReading";
            this.groupAxisReading.Size = new System.Drawing.Size(103, 87);
            this.groupAxisReading.TabIndex = 9;
            this.groupAxisReading.TabStop = false;
            this.groupAxisReading.Text = "Axis Reading";
            // 
            // labelRightCurveValue
            // 
            this.labelRightCurveValue.AutoSize = true;
            this.labelRightCurveValue.Location = new System.Drawing.Point(320, 188);
            this.labelRightCurveValue.Name = "labelRightCurveValue";
            this.labelRightCurveValue.Size = new System.Drawing.Size(13, 15);
            this.labelRightCurveValue.TabIndex = 8;
            this.labelRightCurveValue.Text = "0";
            // 
            // labelRightDeadValue
            // 
            this.labelRightDeadValue.AutoSize = true;
            this.labelRightDeadValue.Location = new System.Drawing.Point(320, 122);
            this.labelRightDeadValue.Name = "labelRightDeadValue";
            this.labelRightDeadValue.Size = new System.Drawing.Size(13, 15);
            this.labelRightDeadValue.TabIndex = 7;
            this.labelRightDeadValue.Text = "0";
            // 
            // trackRightCurve
            // 
            this.trackRightCurve.Location = new System.Drawing.Point(9, 188);
            this.trackRightCurve.Maximum = 30;
            this.trackRightCurve.Name = "trackRightCurve";
            this.trackRightCurve.Size = new System.Drawing.Size(301, 45);
            this.trackRightCurve.TabIndex = 6;
            this.trackRightCurve.TickFrequency = 3;
            this.trackRightCurve.Scroll += new System.EventHandler(this.trackRightCurve_Scroll);
            // 
            // trackRightDead
            // 
            this.trackRightDead.LargeChange = 10;
            this.trackRightDead.Location = new System.Drawing.Point(9, 122);
            this.trackRightDead.Maximum = 100;
            this.trackRightDead.Name = "trackRightDead";
            this.trackRightDead.Size = new System.Drawing.Size(301, 45);
            this.trackRightDead.TabIndex = 5;
            this.trackRightDead.TickFrequency = 10;
            this.trackRightDead.Scroll += new System.EventHandler(this.trackRightDead_Scroll);
            // 
            // tabKeybinds
            // 
            this.tabKeybinds.Location = new System.Drawing.Point(4, 22);
            this.tabKeybinds.Name = "tabKeybinds";
            this.tabKeybinds.Padding = new System.Windows.Forms.Padding(3);
            this.tabKeybinds.Size = new System.Drawing.Size(447, 243);
            this.tabKeybinds.TabIndex = 1;
            this.tabKeybinds.Text = "Keybinds";
            this.tabKeybinds.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(472, 320);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "WoWConsolePort";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabConsolePort.ResumeLayout(false);
            this.tabConsolePort.PerformLayout();
            this.groupAxisReading.ResumeLayout(false);
            this.groupAxisReading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackRightCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackRightDead)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Label labelAxisReading;
        private System.Windows.Forms.CheckBox checkWindowAttached;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConsolePort;
        private System.Windows.Forms.TabPage tabKeybinds;
        private System.Windows.Forms.TrackBar trackRightDead;
        private System.Windows.Forms.TrackBar trackRightCurve;
        private System.Windows.Forms.Label labelRightDeadValue;
        private System.Windows.Forms.GroupBox groupAxisReading;
        private System.Windows.Forms.Label labelRightCurveValue;
        private System.Windows.Forms.ToolStripStatusLabel labelSpacer;
        private System.Windows.Forms.ToolStripStatusLabel labelControllerState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

