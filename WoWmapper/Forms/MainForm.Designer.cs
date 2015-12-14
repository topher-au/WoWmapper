namespace WoWmapper
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
            this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonDS4UpdateNow = new System.Windows.Forms.Button();
            this.buttonCPUpdateNow = new System.Windows.Forms.Button();
            this.groupVersion = new System.Windows.Forms.GroupBox();
            this.labelDS4VersionAvailable = new System.Windows.Forms.Label();
            this.labelCPVersionAvailable = new System.Windows.Forms.Label();
            this.labelVersionAvailable = new System.Windows.Forms.Label();
            this.labelVersionInstalled = new System.Windows.Forms.Label();
            this.labelDS4VersionInstalled = new System.Windows.Forms.Label();
            this.labelCPVersionInstalled = new System.Windows.Forms.Label();
            this.labelDS4CP = new System.Windows.Forms.Label();
            this.labelCPAddon = new System.Windows.Forms.Label();
            this.picConnectionType = new System.Windows.Forms.PictureBox();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.checkHapticsAttached = new System.Windows.Forms.CheckBox();
            this.checkHapticsUserLoggedIn = new System.Windows.Forms.CheckBox();
            this.labelPlayerInfo = new System.Windows.Forms.Label();
            this.groupStatus = new System.Windows.Forms.GroupBox();
            this.checkWindowAttached = new System.Windows.Forms.CheckBox();
            this.menuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.WoWmapperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keybindingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupAdvancedHaptics = new System.Windows.Forms.GroupBox();
            this.panelContent = new System.Windows.Forms.Panel();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.buttonKeybinds = new System.Windows.Forms.Button();
            this.buttonSelectPlugin = new System.Windows.Forms.Button();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupVersion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnectionType)).BeginInit();
            this.groupStatus.SuspendLayout();
            this.menuNotify.SuspendLayout();
            this.groupAdvancedHaptics.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerUpdateUI
            // 
            this.timerUpdateUI.Enabled = true;
            this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WoWmapper.Properties.Resources.wowmapper3;
            this.pictureBox1.Location = new System.Drawing.Point(12, -38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(546, 299);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // buttonDS4UpdateNow
            // 
            this.buttonDS4UpdateNow.Location = new System.Drawing.Point(441, 23);
            this.buttonDS4UpdateNow.Name = "buttonDS4UpdateNow";
            this.buttonDS4UpdateNow.Size = new System.Drawing.Size(75, 23);
            this.buttonDS4UpdateNow.TabIndex = 6;
            this.buttonDS4UpdateNow.Text = "Update";
            this.buttonDS4UpdateNow.UseVisualStyleBackColor = true;
            this.buttonDS4UpdateNow.Visible = false;
            this.buttonDS4UpdateNow.Click += new System.EventHandler(this.buttonDS4UpdateNow_Click);
            // 
            // buttonCPUpdateNow
            // 
            this.buttonCPUpdateNow.Location = new System.Drawing.Point(441, 46);
            this.buttonCPUpdateNow.Name = "buttonCPUpdateNow";
            this.buttonCPUpdateNow.Size = new System.Drawing.Size(75, 23);
            this.buttonCPUpdateNow.TabIndex = 5;
            this.buttonCPUpdateNow.Text = "Update";
            this.buttonCPUpdateNow.UseVisualStyleBackColor = true;
            this.buttonCPUpdateNow.Visible = false;
            this.buttonCPUpdateNow.Click += new System.EventHandler(this.buttonCPUpdateNow_Click);
            // 
            // groupVersion
            // 
            this.groupVersion.Controls.Add(this.labelDS4VersionAvailable);
            this.groupVersion.Controls.Add(this.labelCPVersionAvailable);
            this.groupVersion.Controls.Add(this.labelVersionAvailable);
            this.groupVersion.Controls.Add(this.labelVersionInstalled);
            this.groupVersion.Controls.Add(this.labelDS4VersionInstalled);
            this.groupVersion.Controls.Add(this.labelCPVersionInstalled);
            this.groupVersion.Controls.Add(this.labelDS4CP);
            this.groupVersion.Controls.Add(this.labelCPAddon);
            this.groupVersion.Location = new System.Drawing.Point(104, 8);
            this.groupVersion.Name = "groupVersion";
            this.groupVersion.Size = new System.Drawing.Size(331, 68);
            this.groupVersion.TabIndex = 4;
            this.groupVersion.TabStop = false;
            this.groupVersion.Text = "Version";
            // 
            // labelDS4VersionAvailable
            // 
            this.labelDS4VersionAvailable.AutoSize = true;
            this.labelDS4VersionAvailable.Location = new System.Drawing.Point(250, 19);
            this.labelDS4VersionAvailable.Name = "labelDS4VersionAvailable";
            this.labelDS4VersionAvailable.Size = new System.Drawing.Size(16, 15);
            this.labelDS4VersionAvailable.TabIndex = 8;
            this.labelDS4VersionAvailable.Text = "...";
            // 
            // labelCPVersionAvailable
            // 
            this.labelCPVersionAvailable.AutoSize = true;
            this.labelCPVersionAvailable.Location = new System.Drawing.Point(250, 42);
            this.labelCPVersionAvailable.Name = "labelCPVersionAvailable";
            this.labelCPVersionAvailable.Size = new System.Drawing.Size(16, 15);
            this.labelCPVersionAvailable.TabIndex = 7;
            this.labelCPVersionAvailable.Text = "...";
            // 
            // labelVersionAvailable
            // 
            this.labelVersionAvailable.AutoSize = true;
            this.labelVersionAvailable.BackColor = System.Drawing.SystemColors.Control;
            this.labelVersionAvailable.Location = new System.Drawing.Point(250, 0);
            this.labelVersionAvailable.Name = "labelVersionAvailable";
            this.labelVersionAvailable.Size = new System.Drawing.Size(55, 15);
            this.labelVersionAvailable.TabIndex = 6;
            this.labelVersionAvailable.Text = "Available";
            // 
            // labelVersionInstalled
            // 
            this.labelVersionInstalled.AutoSize = true;
            this.labelVersionInstalled.BackColor = System.Drawing.SystemColors.Control;
            this.labelVersionInstalled.Location = new System.Drawing.Point(161, 0);
            this.labelVersionInstalled.Name = "labelVersionInstalled";
            this.labelVersionInstalled.Size = new System.Drawing.Size(51, 15);
            this.labelVersionInstalled.TabIndex = 5;
            this.labelVersionInstalled.Text = "Installed";
            // 
            // labelDS4VersionInstalled
            // 
            this.labelDS4VersionInstalled.AutoSize = true;
            this.labelDS4VersionInstalled.Location = new System.Drawing.Point(161, 19);
            this.labelDS4VersionInstalled.Name = "labelDS4VersionInstalled";
            this.labelDS4VersionInstalled.Size = new System.Drawing.Size(16, 15);
            this.labelDS4VersionInstalled.TabIndex = 3;
            this.labelDS4VersionInstalled.Text = "...";
            // 
            // labelCPVersionInstalled
            // 
            this.labelCPVersionInstalled.AutoSize = true;
            this.labelCPVersionInstalled.Location = new System.Drawing.Point(161, 42);
            this.labelCPVersionInstalled.Name = "labelCPVersionInstalled";
            this.labelCPVersionInstalled.Size = new System.Drawing.Size(16, 15);
            this.labelCPVersionInstalled.TabIndex = 2;
            this.labelCPVersionInstalled.Text = "...";
            // 
            // labelDS4CP
            // 
            this.labelDS4CP.AutoSize = true;
            this.labelDS4CP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDS4CP.Location = new System.Drawing.Point(6, 19);
            this.labelDS4CP.Name = "labelDS4CP";
            this.labelDS4CP.Size = new System.Drawing.Size(81, 15);
            this.labelDS4CP.TabIndex = 1;
            this.labelDS4CP.Text = "WoWmapper";
            this.labelDS4CP.Click += new System.EventHandler(this.labelDS4CP_Click);
            // 
            // labelCPAddon
            // 
            this.labelCPAddon.AutoSize = true;
            this.labelCPAddon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCPAddon.Location = new System.Drawing.Point(6, 42);
            this.labelCPAddon.Name = "labelCPAddon";
            this.labelCPAddon.Size = new System.Drawing.Size(74, 15);
            this.labelCPAddon.TabIndex = 0;
            this.labelCPAddon.Text = "ConsolePort";
            // 
            // picConnectionType
            // 
            this.picConnectionType.Location = new System.Drawing.Point(6, 22);
            this.picConnectionType.Name = "picConnectionType";
            this.picConnectionType.Size = new System.Drawing.Size(39, 20);
            this.picConnectionType.TabIndex = 1;
            this.picConnectionType.TabStop = false;
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new System.Drawing.Point(51, 25);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(79, 15);
            this.labelConnectionStatus.TabIndex = 0;
            this.labelConnectionStatus.Text = "Disconnected";
            this.labelConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkHapticsAttached
            // 
            this.checkHapticsAttached.AutoSize = true;
            this.checkHapticsAttached.Enabled = false;
            this.checkHapticsAttached.Location = new System.Drawing.Point(11, 22);
            this.checkHapticsAttached.Name = "checkHapticsAttached";
            this.checkHapticsAttached.Size = new System.Drawing.Size(113, 19);
            this.checkHapticsAttached.TabIndex = 14;
            this.checkHapticsAttached.Text = "haptics attached";
            this.checkHapticsAttached.UseVisualStyleBackColor = true;
            // 
            // checkHapticsUserLoggedIn
            // 
            this.checkHapticsUserLoggedIn.AutoSize = true;
            this.checkHapticsUserLoggedIn.Enabled = false;
            this.checkHapticsUserLoggedIn.Location = new System.Drawing.Point(11, 41);
            this.checkHapticsUserLoggedIn.Name = "checkHapticsUserLoggedIn";
            this.checkHapticsUserLoggedIn.Size = new System.Drawing.Size(101, 19);
            this.checkHapticsUserLoggedIn.TabIndex = 15;
            this.checkHapticsUserLoggedIn.Text = "user logged in";
            this.checkHapticsUserLoggedIn.UseVisualStyleBackColor = true;
            // 
            // labelPlayerInfo
            // 
            this.labelPlayerInfo.AutoSize = true;
            this.labelPlayerInfo.Location = new System.Drawing.Point(27, 63);
            this.labelPlayerInfo.Name = "labelPlayerInfo";
            this.labelPlayerInfo.Size = new System.Drawing.Size(55, 15);
            this.labelPlayerInfo.TabIndex = 23;
            this.labelPlayerInfo.Text = "user data";
            // 
            // groupStatus
            // 
            this.groupStatus.Controls.Add(this.labelConnectionStatus);
            this.groupStatus.Controls.Add(this.picConnectionType);
            this.groupStatus.Controls.Add(this.checkWindowAttached);
            this.groupStatus.Location = new System.Drawing.Point(6, 84);
            this.groupStatus.Name = "groupStatus";
            this.groupStatus.Size = new System.Drawing.Size(275, 73);
            this.groupStatus.TabIndex = 26;
            this.groupStatus.TabStop = false;
            this.groupStatus.Text = "status";
            // 
            // checkWindowAttached
            // 
            this.checkWindowAttached.AutoSize = true;
            this.checkWindowAttached.Enabled = false;
            this.checkWindowAttached.Location = new System.Drawing.Point(6, 48);
            this.checkWindowAttached.Name = "checkWindowAttached";
            this.checkWindowAttached.Size = new System.Drawing.Size(103, 19);
            this.checkWindowAttached.TabIndex = 13;
            this.checkWindowAttached.Text = "window found";
            this.checkWindowAttached.UseVisualStyleBackColor = true;
            // 
            // menuNotify
            // 
            this.menuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WoWmapperToolStripMenuItem,
            this.toolStripSeparator2,
            this.settingsToolStripMenuItem,
            this.keybindingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.menuNotify.Name = "menuNotify";
            this.menuNotify.Size = new System.Drawing.Size(149, 104);
            // 
            // WoWmapperToolStripMenuItem
            // 
            this.WoWmapperToolStripMenuItem.Name = "WoWmapperToolStripMenuItem";
            this.WoWmapperToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.WoWmapperToolStripMenuItem.Text = "WoWmapper";
            this.WoWmapperToolStripMenuItem.Click += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.settingsToolStripMenuItem.Text = "Configuration";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // keybindingsToolStripMenuItem
            // 
            this.keybindingsToolStripMenuItem.Name = "keybindingsToolStripMenuItem";
            this.keybindingsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.keybindingsToolStripMenuItem.Text = "Keybindings";
            this.keybindingsToolStripMenuItem.Click += new System.EventHandler(this.keybindingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // groupAdvancedHaptics
            // 
            this.groupAdvancedHaptics.Controls.Add(this.checkHapticsAttached);
            this.groupAdvancedHaptics.Controls.Add(this.labelPlayerInfo);
            this.groupAdvancedHaptics.Controls.Add(this.checkHapticsUserLoggedIn);
            this.groupAdvancedHaptics.Location = new System.Drawing.Point(290, 84);
            this.groupAdvancedHaptics.Name = "groupAdvancedHaptics";
            this.groupAdvancedHaptics.Size = new System.Drawing.Size(257, 98);
            this.groupAdvancedHaptics.TabIndex = 27;
            this.groupAdvancedHaptics.TabStop = false;
            this.groupAdvancedHaptics.Text = "advanced haptics";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.SystemColors.Control;
            this.panelContent.Controls.Add(this.groupVersion);
            this.panelContent.Controls.Add(this.groupStatus);
            this.panelContent.Controls.Add(this.groupAdvancedHaptics);
            this.panelContent.Controls.Add(this.buttonDS4UpdateNow);
            this.panelContent.Controls.Add(this.buttonCPUpdateNow);
            this.panelContent.Location = new System.Drawing.Point(12, 244);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(557, 186);
            this.panelContent.TabIndex = 28;
            // 
            // buttonConfig
            // 
            this.buttonConfig.Location = new System.Drawing.Point(321, 436);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(75, 23);
            this.buttonConfig.TabIndex = 29;
            this.buttonConfig.Text = "configure";
            this.buttonConfig.UseVisualStyleBackColor = true;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonKeybinds
            // 
            this.buttonKeybinds.Location = new System.Drawing.Point(403, 436);
            this.buttonKeybinds.Name = "buttonKeybinds";
            this.buttonKeybinds.Size = new System.Drawing.Size(75, 23);
            this.buttonKeybinds.TabIndex = 30;
            this.buttonKeybinds.Text = "keybinds";
            this.buttonKeybinds.UseVisualStyleBackColor = true;
            this.buttonKeybinds.Click += new System.EventHandler(this.buttonKeybinds_Click);
            // 
            // buttonSelectPlugin
            // 
            this.buttonSelectPlugin.Location = new System.Drawing.Point(484, 436);
            this.buttonSelectPlugin.Name = "buttonSelectPlugin";
            this.buttonSelectPlugin.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectPlugin.TabIndex = 31;
            this.buttonSelectPlugin.Text = "plugins";
            this.buttonSelectPlugin.UseVisualStyleBackColor = true;
            this.buttonSelectPlugin.Click += new System.EventHandler(this.buttonSelectPlugin_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(570, 467);
            this.Controls.Add(this.buttonSelectPlugin);
            this.Controls.Add(this.buttonKeybinds);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WoWmapper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupVersion.ResumeLayout(false);
            this.groupVersion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnectionType)).EndInit();
            this.groupStatus.ResumeLayout(false);
            this.groupStatus.PerformLayout();
            this.menuNotify.ResumeLayout(false);
            this.groupAdvancedHaptics.ResumeLayout(false);
            this.groupAdvancedHaptics.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerUpdateUI;
        private System.Windows.Forms.Label labelPlayerInfo;
        private System.Windows.Forms.CheckBox checkHapticsUserLoggedIn;
        private System.Windows.Forms.CheckBox checkWindowAttached;
        private System.Windows.Forms.CheckBox checkHapticsAttached;
        private System.Windows.Forms.ContextMenuStrip menuNotify;
        private System.Windows.Forms.ToolStripMenuItem WoWmapperToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupStatus;
        private System.Windows.Forms.GroupBox groupVersion;
        private System.Windows.Forms.Label labelDS4VersionInstalled;
        private System.Windows.Forms.Label labelCPVersionInstalled;
        private System.Windows.Forms.Label labelDS4CP;
        private System.Windows.Forms.Label labelCPAddon;
        private System.Windows.Forms.PictureBox picConnectionType;
        private System.Windows.Forms.Label labelConnectionStatus;
        private System.Windows.Forms.Label labelDS4VersionAvailable;
        private System.Windows.Forms.Label labelCPVersionAvailable;
        private System.Windows.Forms.Label labelVersionAvailable;
        private System.Windows.Forms.Label labelVersionInstalled;
        private System.Windows.Forms.Button buttonDS4UpdateNow;
        private System.Windows.Forms.Button buttonCPUpdateNow;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem keybindingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupAdvancedHaptics;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Button buttonKeybinds;
        private System.Windows.Forms.Button buttonSelectPlugin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

