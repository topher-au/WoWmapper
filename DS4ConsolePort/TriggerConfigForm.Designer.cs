namespace DS4ConsolePort
{
    partial class TriggerConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerConfigForm));
            this.picL2 = new System.Windows.Forms.PictureBox();
            this.picR2 = new System.Windows.Forms.PictureBox();
            this.numL2 = new System.Windows.Forms.NumericUpDown();
            this.numR2 = new System.Windows.Forms.NumericUpDown();
            this.trackL2 = new System.Windows.Forms.TrackBar();
            this.trackR2 = new System.Windows.Forms.TrackBar();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.checkR2 = new System.Windows.Forms.CheckBox();
            this.checkL2 = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelTriggerConfig = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picL2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picR2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numL2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numR2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackL2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackR2)).BeginInit();
            this.SuspendLayout();
            // 
            // picL2
            // 
            this.picL2.Image = global::DS4ConsolePort.Properties.Resources.L2;
            resources.ApplyResources(this.picL2, "picL2");
            this.picL2.Name = "picL2";
            this.picL2.TabStop = false;
            // 
            // picR2
            // 
            this.picR2.Image = global::DS4ConsolePort.Properties.Resources.R2;
            resources.ApplyResources(this.picR2, "picR2");
            this.picR2.Name = "picR2";
            this.picR2.TabStop = false;
            // 
            // numL2
            // 
            resources.ApplyResources(this.numL2, "numL2");
            this.numL2.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numL2.Name = "numL2";
            // 
            // numR2
            // 
            resources.ApplyResources(this.numR2, "numR2");
            this.numR2.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numR2.Name = "numR2";
            // 
            // trackL2
            // 
            resources.ApplyResources(this.trackL2, "trackL2");
            this.trackL2.Maximum = 100;
            this.trackL2.Name = "trackL2";
            this.trackL2.TabStop = false;
            this.trackL2.TickFrequency = 0;
            // 
            // trackR2
            // 
            resources.ApplyResources(this.trackR2, "trackR2");
            this.trackR2.Maximum = 100;
            this.trackR2.Name = "trackR2";
            this.trackR2.TabStop = false;
            this.trackR2.TickFrequency = 0;
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 20;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // checkR2
            // 
            resources.ApplyResources(this.checkR2, "checkR2");
            this.checkR2.Name = "checkR2";
            this.checkR2.TabStop = false;
            this.checkR2.UseVisualStyleBackColor = true;
            // 
            // checkL2
            // 
            resources.ApplyResources(this.checkL2, "checkL2");
            this.checkL2.Name = "checkL2";
            this.checkL2.TabStop = false;
            this.checkL2.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            resources.ApplyResources(this.buttonSave, "buttonSave");
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelTriggerConfig
            // 
            resources.ApplyResources(this.labelTriggerConfig, "labelTriggerConfig");
            this.labelTriggerConfig.Name = "labelTriggerConfig";
            // 
            // TriggerConfigForm
            // 
            this.AcceptButton = this.buttonSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.labelTriggerConfig);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkL2);
            this.Controls.Add(this.checkR2);
            this.Controls.Add(this.numL2);
            this.Controls.Add(this.picL2);
            this.Controls.Add(this.trackR2);
            this.Controls.Add(this.trackL2);
            this.Controls.Add(this.numR2);
            this.Controls.Add(this.picR2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TriggerConfigForm";
            this.Load += new System.EventHandler(this.TriggerConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picL2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picR2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numL2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numR2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackL2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackR2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picL2;
        private System.Windows.Forms.PictureBox picR2;
        private System.Windows.Forms.NumericUpDown numL2;
        private System.Windows.Forms.NumericUpDown numR2;
        private System.Windows.Forms.TrackBar trackL2;
        private System.Windows.Forms.TrackBar trackR2;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.CheckBox checkR2;
        private System.Windows.Forms.CheckBox checkL2;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelTriggerConfig;
    }
}