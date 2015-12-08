namespace DS4ConsolePort
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConsolePort = new System.Windows.Forms.TabPage();
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
            this.groupDS4Status = new System.Windows.Forms.GroupBox();
            this.picConnectionType = new System.Windows.Forms.PictureBox();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.picConsolePort = new System.Windows.Forms.PictureBox();
            this.tabKeybinds = new System.Windows.Forms.TabPage();
            this.picConfigR2 = new System.Windows.Forms.PictureBox();
            this.picConfigL2 = new System.Windows.Forms.PictureBox();
            this.labelCenterCursor = new System.Windows.Forms.Label();
            this.picResetBinds = new System.Windows.Forms.PictureBox();
            this.labelTouchUpper = new System.Windows.Forms.Label();
            this.labelTouchMode = new System.Windows.Forms.Label();
            this.textBindTouchRight = new System.Windows.Forms.TextBox();
            this.textBindTouchLeft = new System.Windows.Forms.TextBox();
            this.comboTouchMode = new System.Windows.Forms.ComboBox();
            this.textPS = new System.Windows.Forms.TextBox();
            this.labelChangeBindings = new System.Windows.Forms.Label();
            this.textOptions = new System.Windows.Forms.TextBox();
            this.textShare = new System.Windows.Forms.TextBox();
            this.panelCamera = new System.Windows.Forms.Panel();
            this.labelCamera = new System.Windows.Forms.Label();
            this.numRSpeed = new System.Windows.Forms.NumericUpDown();
            this.numRCurve = new System.Windows.Forms.NumericUpDown();
            this.numRDeadzone = new System.Windows.Forms.NumericUpDown();
            this.labelRightSpeed = new System.Windows.Forms.Label();
            this.panelRStickAxis = new System.Windows.Forms.Panel();
            this.labelRightCurve = new System.Windows.Forms.Label();
            this.labelRightDead = new System.Windows.Forms.Label();
            this.textSquare = new System.Windows.Forms.TextBox();
            this.textCross = new System.Windows.Forms.TextBox();
            this.textCircle = new System.Windows.Forms.TextBox();
            this.textTriangle = new System.Windows.Forms.TextBox();
            this.textDpadLeft = new System.Windows.Forms.TextBox();
            this.textDpadDown = new System.Windows.Forms.TextBox();
            this.textDpadRight = new System.Windows.Forms.TextBox();
            this.textDpadUp = new System.Windows.Forms.TextBox();
            this.panelMovement = new System.Windows.Forms.Panel();
            this.labelMovement = new System.Windows.Forms.Label();
            this.picLStickLeft = new System.Windows.Forms.PictureBox();
            this.textMoveLeft = new System.Windows.Forms.TextBox();
            this.picLStickDown = new System.Windows.Forms.PictureBox();
            this.textMoveBackward = new System.Windows.Forms.TextBox();
            this.picLStickRight = new System.Windows.Forms.PictureBox();
            this.textMoveRight = new System.Windows.Forms.TextBox();
            this.picLStickUp = new System.Windows.Forms.PictureBox();
            this.textMoveForward = new System.Windows.Forms.TextBox();
            this.textR2 = new System.Windows.Forms.TextBox();
            this.textR1 = new System.Windows.Forms.TextBox();
            this.textL2 = new System.Windows.Forms.TextBox();
            this.textL1 = new System.Windows.Forms.TextBox();
            this.picDS4 = new System.Windows.Forms.PictureBox();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.groupWoWFolder = new System.Windows.Forms.GroupBox();
            this.buttonLocateWoW = new System.Windows.Forms.Button();
            this.textWoWFolder = new System.Windows.Forms.TextBox();
            this.groupHaptics = new System.Windows.Forms.GroupBox();
            this.checkLightbarBattery = new System.Windows.Forms.CheckBox();
            this.checkEnableAdvancedHaptics = new System.Windows.Forms.CheckBox();
            this.panelAdvancedHaptics = new System.Windows.Forms.Panel();
            this.groupHapticStatus = new System.Windows.Forms.GroupBox();
            this.checkHapticsAttached = new System.Windows.Forms.CheckBox();
            this.checkHapticsUserLoggedIn = new System.Windows.Forms.CheckBox();
            this.labelPlayerInfo = new System.Windows.Forms.Label();
            this.groupHapticSettings = new System.Windows.Forms.GroupBox();
            this.checkRumbleDamage = new System.Windows.Forms.CheckBox();
            this.checkRumbleTarget = new System.Windows.Forms.CheckBox();
            this.checkLightbarHealth = new System.Windows.Forms.CheckBox();
            this.checkLightbarClass = new System.Windows.Forms.CheckBox();
            this.groupDS4CSettings = new System.Windows.Forms.GroupBox();
            this.checkStartMinimized = new System.Windows.Forms.CheckBox();
            this.checkCloseTray = new System.Windows.Forms.CheckBox();
            this.checkMinTray = new System.Windows.Forms.CheckBox();
            this.groupInteraction = new System.Windows.Forms.GroupBox();
            this.groupInteractionSettings = new System.Windows.Forms.GroupBox();
            this.checkDisableBG = new System.Windows.Forms.CheckBox();
            this.checkSendKeysDirect = new System.Windows.Forms.CheckBox();
            this.checkWindowAttached = new System.Windows.Forms.CheckBox();
            this.menuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dS4ConsolePortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.tabConsolePort.SuspendLayout();
            this.groupVersion.SuspendLayout();
            this.groupDS4Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnectionType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConsolePort)).BeginInit();
            this.tabKeybinds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigR2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigL2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picResetBinds)).BeginInit();
            this.panelCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRDeadzone)).BeginInit();
            this.panelMovement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDS4)).BeginInit();
            this.tabAdvanced.SuspendLayout();
            this.groupWoWFolder.SuspendLayout();
            this.groupHaptics.SuspendLayout();
            this.panelAdvancedHaptics.SuspendLayout();
            this.groupHapticStatus.SuspendLayout();
            this.groupHapticSettings.SuspendLayout();
            this.groupDS4CSettings.SuspendLayout();
            this.groupInteraction.SuspendLayout();
            this.groupInteractionSettings.SuspendLayout();
            this.menuNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerUpdateUI
            // 
            this.timerUpdateUI.Enabled = true;
            this.timerUpdateUI.Interval = 5;
            this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabConsolePort);
            this.tabControl.Controls.Add(this.tabKeybinds);
            this.tabControl.Controls.Add(this.tabAdvanced);
            this.tabControl.Location = new System.Drawing.Point(4, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(772, 524);
            this.tabControl.TabIndex = 6;
            // 
            // tabConsolePort
            // 
            this.tabConsolePort.Controls.Add(this.buttonDS4UpdateNow);
            this.tabConsolePort.Controls.Add(this.buttonCPUpdateNow);
            this.tabConsolePort.Controls.Add(this.groupVersion);
            this.tabConsolePort.Controls.Add(this.groupDS4Status);
            this.tabConsolePort.Controls.Add(this.picConsolePort);
            this.tabConsolePort.Location = new System.Drawing.Point(4, 24);
            this.tabConsolePort.Name = "tabConsolePort";
            this.tabConsolePort.Padding = new System.Windows.Forms.Padding(3);
            this.tabConsolePort.Size = new System.Drawing.Size(764, 496);
            this.tabConsolePort.TabIndex = 0;
            this.tabConsolePort.Text = "consoleport";
            this.tabConsolePort.UseVisualStyleBackColor = true;
            // 
            // buttonDS4UpdateNow
            // 
            this.buttonDS4UpdateNow.Location = new System.Drawing.Point(551, 356);
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
            this.buttonCPUpdateNow.Location = new System.Drawing.Point(551, 333);
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
            this.groupVersion.Location = new System.Drawing.Point(214, 318);
            this.groupVersion.Name = "groupVersion";
            this.groupVersion.Size = new System.Drawing.Size(331, 68);
            this.groupVersion.TabIndex = 4;
            this.groupVersion.TabStop = false;
            this.groupVersion.Text = "Version";
            // 
            // labelDS4VersionAvailable
            // 
            this.labelDS4VersionAvailable.AutoSize = true;
            this.labelDS4VersionAvailable.Location = new System.Drawing.Point(250, 42);
            this.labelDS4VersionAvailable.Name = "labelDS4VersionAvailable";
            this.labelDS4VersionAvailable.Size = new System.Drawing.Size(16, 15);
            this.labelDS4VersionAvailable.TabIndex = 8;
            this.labelDS4VersionAvailable.Text = "...";
            // 
            // labelCPVersionAvailable
            // 
            this.labelCPVersionAvailable.AutoSize = true;
            this.labelCPVersionAvailable.Location = new System.Drawing.Point(250, 19);
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
            this.labelDS4VersionInstalled.Location = new System.Drawing.Point(161, 42);
            this.labelDS4VersionInstalled.Name = "labelDS4VersionInstalled";
            this.labelDS4VersionInstalled.Size = new System.Drawing.Size(16, 15);
            this.labelDS4VersionInstalled.TabIndex = 3;
            this.labelDS4VersionInstalled.Text = "...";
            // 
            // labelCPVersionInstalled
            // 
            this.labelCPVersionInstalled.AutoSize = true;
            this.labelCPVersionInstalled.Location = new System.Drawing.Point(161, 19);
            this.labelCPVersionInstalled.Name = "labelCPVersionInstalled";
            this.labelCPVersionInstalled.Size = new System.Drawing.Size(16, 15);
            this.labelCPVersionInstalled.TabIndex = 2;
            this.labelCPVersionInstalled.Text = "...";
            // 
            // labelDS4CP
            // 
            this.labelDS4CP.AutoSize = true;
            this.labelDS4CP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDS4CP.Location = new System.Drawing.Point(6, 42);
            this.labelDS4CP.Name = "labelDS4CP";
            this.labelDS4CP.Size = new System.Drawing.Size(97, 15);
            this.labelDS4CP.TabIndex = 1;
            this.labelDS4CP.Text = "DS4ConsolePort";
            this.labelDS4CP.Click += new System.EventHandler(this.labelDS4CP_Click);
            // 
            // labelCPAddon
            // 
            this.labelCPAddon.AutoSize = true;
            this.labelCPAddon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCPAddon.Location = new System.Drawing.Point(7, 19);
            this.labelCPAddon.Name = "labelCPAddon";
            this.labelCPAddon.Size = new System.Drawing.Size(113, 15);
            this.labelCPAddon.TabIndex = 0;
            this.labelCPAddon.Text = "ConsolePort Addon";
            // 
            // groupDS4Status
            // 
            this.groupDS4Status.Controls.Add(this.picConnectionType);
            this.groupDS4Status.Controls.Add(this.labelConnectionStatus);
            this.groupDS4Status.Location = new System.Drawing.Point(269, 396);
            this.groupDS4Status.Name = "groupDS4Status";
            this.groupDS4Status.Size = new System.Drawing.Size(229, 57);
            this.groupDS4Status.TabIndex = 3;
            this.groupDS4Status.TabStop = false;
            this.groupDS4Status.Text = "ds4 status";
            // 
            // picConnectionType
            // 
            this.picConnectionType.Location = new System.Drawing.Point(182, 23);
            this.picConnectionType.Name = "picConnectionType";
            this.picConnectionType.Size = new System.Drawing.Size(39, 20);
            this.picConnectionType.TabIndex = 1;
            this.picConnectionType.TabStop = false;
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnectionStatus.Location = new System.Drawing.Point(6, 19);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(132, 25);
            this.labelConnectionStatus.TabIndex = 0;
            this.labelConnectionStatus.Text = "Disconnected";
            this.labelConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picConsolePort
            // 
            this.picConsolePort.Image = global::DS4ConsolePort.Properties.Resources.CONSOLEPORTLOGO;
            this.picConsolePort.Location = new System.Drawing.Point(128, 6);
            this.picConsolePort.Name = "picConsolePort";
            this.picConsolePort.Size = new System.Drawing.Size(512, 291);
            this.picConsolePort.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picConsolePort.TabIndex = 0;
            this.picConsolePort.TabStop = false;
            // 
            // tabKeybinds
            // 
            this.tabKeybinds.BackColor = System.Drawing.Color.DimGray;
            this.tabKeybinds.Controls.Add(this.picConfigR2);
            this.tabKeybinds.Controls.Add(this.picConfigL2);
            this.tabKeybinds.Controls.Add(this.labelCenterCursor);
            this.tabKeybinds.Controls.Add(this.picResetBinds);
            this.tabKeybinds.Controls.Add(this.labelTouchUpper);
            this.tabKeybinds.Controls.Add(this.labelTouchMode);
            this.tabKeybinds.Controls.Add(this.textBindTouchRight);
            this.tabKeybinds.Controls.Add(this.textBindTouchLeft);
            this.tabKeybinds.Controls.Add(this.comboTouchMode);
            this.tabKeybinds.Controls.Add(this.textPS);
            this.tabKeybinds.Controls.Add(this.labelChangeBindings);
            this.tabKeybinds.Controls.Add(this.textOptions);
            this.tabKeybinds.Controls.Add(this.textShare);
            this.tabKeybinds.Controls.Add(this.panelCamera);
            this.tabKeybinds.Controls.Add(this.textSquare);
            this.tabKeybinds.Controls.Add(this.textCross);
            this.tabKeybinds.Controls.Add(this.textCircle);
            this.tabKeybinds.Controls.Add(this.textTriangle);
            this.tabKeybinds.Controls.Add(this.textDpadLeft);
            this.tabKeybinds.Controls.Add(this.textDpadDown);
            this.tabKeybinds.Controls.Add(this.textDpadRight);
            this.tabKeybinds.Controls.Add(this.textDpadUp);
            this.tabKeybinds.Controls.Add(this.panelMovement);
            this.tabKeybinds.Controls.Add(this.textR2);
            this.tabKeybinds.Controls.Add(this.textR1);
            this.tabKeybinds.Controls.Add(this.textL2);
            this.tabKeybinds.Controls.Add(this.textL1);
            this.tabKeybinds.Controls.Add(this.picDS4);
            this.tabKeybinds.Location = new System.Drawing.Point(4, 24);
            this.tabKeybinds.Name = "tabKeybinds";
            this.tabKeybinds.Padding = new System.Windows.Forms.Padding(3);
            this.tabKeybinds.Size = new System.Drawing.Size(764, 496);
            this.tabKeybinds.TabIndex = 1;
            this.tabKeybinds.Text = "keybinds";
            // 
            // picConfigR2
            // 
            this.picConfigR2.Image = global::DS4ConsolePort.Properties.Resources.cogs;
            this.picConfigR2.Location = new System.Drawing.Point(645, 58);
            this.picConfigR2.Name = "picConfigR2";
            this.picConfigR2.Size = new System.Drawing.Size(24, 24);
            this.picConfigR2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picConfigR2.TabIndex = 46;
            this.picConfigR2.TabStop = false;
            this.picConfigR2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // picConfigL2
            // 
            this.picConfigL2.Image = global::DS4ConsolePort.Properties.Resources.cogs;
            this.picConfigL2.Location = new System.Drawing.Point(98, 58);
            this.picConfigL2.Name = "picConfigL2";
            this.picConfigL2.Size = new System.Drawing.Size(24, 24);
            this.picConfigL2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picConfigL2.TabIndex = 45;
            this.picConfigL2.TabStop = false;
            this.picConfigL2.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelCenterCursor
            // 
            this.labelCenterCursor.AutoSize = true;
            this.labelCenterCursor.ForeColor = System.Drawing.Color.White;
            this.labelCenterCursor.Location = new System.Drawing.Point(675, 393);
            this.labelCenterCursor.Name = "labelCenterCursor";
            this.labelCenterCursor.Size = new System.Drawing.Size(80, 30);
            this.labelCenterCursor.TabIndex = 44;
            this.labelCenterCursor.Text = "Control+R3:\r\nCenter Cursor";
            // 
            // picResetBinds
            // 
            this.picResetBinds.Image = global::DS4ConsolePort.Properties.Resources.Reset_Binds;
            this.picResetBinds.Location = new System.Drawing.Point(734, 6);
            this.picResetBinds.Name = "picResetBinds";
            this.picResetBinds.Size = new System.Drawing.Size(24, 24);
            this.picResetBinds.TabIndex = 43;
            this.picResetBinds.TabStop = false;
            this.picResetBinds.Click += new System.EventHandler(this.picResetBinds_Click);
            // 
            // labelTouchUpper
            // 
            this.labelTouchUpper.ForeColor = System.Drawing.Color.Yellow;
            this.labelTouchUpper.Location = new System.Drawing.Point(333, 35);
            this.labelTouchUpper.Name = "labelTouchUpper";
            this.labelTouchUpper.Size = new System.Drawing.Size(100, 70);
            this.labelTouchUpper.TabIndex = 42;
            this.labelTouchUpper.Text = "Press the upper touchpad to toggle mouse control";
            this.labelTouchUpper.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelTouchMode
            // 
            this.labelTouchMode.ForeColor = System.Drawing.Color.White;
            this.labelTouchMode.Location = new System.Drawing.Point(320, 109);
            this.labelTouchMode.Name = "labelTouchMode";
            this.labelTouchMode.Size = new System.Drawing.Size(124, 15);
            this.labelTouchMode.TabIndex = 41;
            this.labelTouchMode.Text = "Touchpad Mode";
            this.labelTouchMode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBindTouchRight
            // 
            this.textBindTouchRight.Location = new System.Drawing.Point(386, 160);
            this.textBindTouchRight.Name = "textBindTouchRight";
            this.textBindTouchRight.ReadOnly = true;
            this.textBindTouchRight.Size = new System.Drawing.Size(75, 23);
            this.textBindTouchRight.TabIndex = 40;
            this.textBindTouchRight.Tag = "TouchRight";
            this.textBindTouchRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBindTouchRight.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textBindTouchLeft
            // 
            this.textBindTouchLeft.Location = new System.Drawing.Point(302, 160);
            this.textBindTouchLeft.Name = "textBindTouchLeft";
            this.textBindTouchLeft.ReadOnly = true;
            this.textBindTouchLeft.Size = new System.Drawing.Size(75, 23);
            this.textBindTouchLeft.TabIndex = 39;
            this.textBindTouchLeft.Tag = "TouchLeft";
            this.textBindTouchLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBindTouchLeft.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // comboTouchMode
            // 
            this.comboTouchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTouchMode.FormattingEnabled = true;
            this.comboTouchMode.Items.AddRange(new object[] {
            "Mouse Control",
            "TouchLeft/TouchRight",
            "Share/Options"});
            this.comboTouchMode.Location = new System.Drawing.Point(321, 127);
            this.comboTouchMode.Name = "comboTouchMode";
            this.comboTouchMode.Size = new System.Drawing.Size(121, 23);
            this.comboTouchMode.TabIndex = 38;
            this.comboTouchMode.SelectedIndexChanged += new System.EventHandler(this.comboTouchMode_SelectedIndexChanged);
            // 
            // textPS
            // 
            this.textPS.Location = new System.Drawing.Point(330, 394);
            this.textPS.Name = "textPS";
            this.textPS.ReadOnly = true;
            this.textPS.Size = new System.Drawing.Size(100, 23);
            this.textPS.TabIndex = 27;
            this.textPS.Tag = "PS";
            this.textPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textPS.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // labelChangeBindings
            // 
            this.labelChangeBindings.AutoSize = true;
            this.labelChangeBindings.ForeColor = System.Drawing.Color.White;
            this.labelChangeBindings.Location = new System.Drawing.Point(301, 13);
            this.labelChangeBindings.Name = "labelChangeBindings";
            this.labelChangeBindings.Size = new System.Drawing.Size(172, 15);
            this.labelChangeBindings.TabIndex = 37;
            this.labelChangeBindings.Text = "Double click binding to change";
            // 
            // textOptions
            // 
            this.textOptions.Location = new System.Drawing.Point(438, 44);
            this.textOptions.Name = "textOptions";
            this.textOptions.ReadOnly = true;
            this.textOptions.Size = new System.Drawing.Size(100, 23);
            this.textOptions.TabIndex = 22;
            this.textOptions.Tag = "Options";
            this.textOptions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textOptions.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textShare
            // 
            this.textShare.Location = new System.Drawing.Point(230, 44);
            this.textShare.Name = "textShare";
            this.textShare.ReadOnly = true;
            this.textShare.Size = new System.Drawing.Size(100, 23);
            this.textShare.TabIndex = 21;
            this.textShare.Tag = "Share";
            this.textShare.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textShare.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // panelCamera
            // 
            this.panelCamera.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCamera.Controls.Add(this.labelCamera);
            this.panelCamera.Controls.Add(this.numRSpeed);
            this.panelCamera.Controls.Add(this.numRCurve);
            this.panelCamera.Controls.Add(this.numRDeadzone);
            this.panelCamera.Controls.Add(this.labelRightSpeed);
            this.panelCamera.Controls.Add(this.panelRStickAxis);
            this.panelCamera.Controls.Add(this.labelRightCurve);
            this.panelCamera.Controls.Add(this.labelRightDead);
            this.panelCamera.Location = new System.Drawing.Point(457, 389);
            this.panelCamera.Name = "panelCamera";
            this.panelCamera.Size = new System.Drawing.Size(212, 93);
            this.panelCamera.TabIndex = 36;
            // 
            // labelCamera
            // 
            this.labelCamera.ForeColor = System.Drawing.Color.White;
            this.labelCamera.Location = new System.Drawing.Point(3, 2);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(65, 19);
            this.labelCamera.TabIndex = 30;
            this.labelCamera.Text = "camera";
            // 
            // numRSpeed
            // 
            this.numRSpeed.Location = new System.Drawing.Point(150, 62);
            this.numRSpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRSpeed.Name = "numRSpeed";
            this.numRSpeed.Size = new System.Drawing.Size(50, 23);
            this.numRSpeed.TabIndex = 29;
            this.numRSpeed.ValueChanged += new System.EventHandler(this.numRSpeed_ValueChanged);
            // 
            // numRCurve
            // 
            this.numRCurve.Location = new System.Drawing.Point(150, 33);
            this.numRCurve.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numRCurve.Name = "numRCurve";
            this.numRCurve.Size = new System.Drawing.Size(50, 23);
            this.numRCurve.TabIndex = 28;
            this.numRCurve.ValueChanged += new System.EventHandler(this.numRCurve_ValueChanged);
            // 
            // numRDeadzone
            // 
            this.numRDeadzone.Location = new System.Drawing.Point(150, 4);
            this.numRDeadzone.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numRDeadzone.Name = "numRDeadzone";
            this.numRDeadzone.Size = new System.Drawing.Size(50, 23);
            this.numRDeadzone.TabIndex = 27;
            this.numRDeadzone.ValueChanged += new System.EventHandler(this.numRDeadzone_ValueChanged);
            // 
            // labelRightSpeed
            // 
            this.labelRightSpeed.AutoSize = true;
            this.labelRightSpeed.ForeColor = System.Drawing.Color.White;
            this.labelRightSpeed.Location = new System.Drawing.Point(74, 64);
            this.labelRightSpeed.Name = "labelRightSpeed";
            this.labelRightSpeed.Size = new System.Drawing.Size(39, 15);
            this.labelRightSpeed.TabIndex = 24;
            this.labelRightSpeed.Text = "Speed";
            // 
            // panelRStickAxis
            // 
            this.panelRStickAxis.Location = new System.Drawing.Point(3, 20);
            this.panelRStickAxis.Name = "panelRStickAxis";
            this.panelRStickAxis.Size = new System.Drawing.Size(65, 65);
            this.panelRStickAxis.TabIndex = 26;
            this.panelRStickAxis.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRStick_Paint);
            // 
            // labelRightCurve
            // 
            this.labelRightCurve.AutoSize = true;
            this.labelRightCurve.ForeColor = System.Drawing.Color.White;
            this.labelRightCurve.Location = new System.Drawing.Point(74, 35);
            this.labelRightCurve.Name = "labelRightCurve";
            this.labelRightCurve.Size = new System.Drawing.Size(38, 15);
            this.labelRightCurve.TabIndex = 22;
            this.labelRightCurve.Text = "Curve";
            // 
            // labelRightDead
            // 
            this.labelRightDead.AutoSize = true;
            this.labelRightDead.ForeColor = System.Drawing.Color.White;
            this.labelRightDead.Location = new System.Drawing.Point(74, 6);
            this.labelRightDead.Name = "labelRightDead";
            this.labelRightDead.Size = new System.Drawing.Size(59, 15);
            this.labelRightDead.TabIndex = 21;
            this.labelRightDead.Text = "Deadzone";
            // 
            // textSquare
            // 
            this.textSquare.Location = new System.Drawing.Point(650, 297);
            this.textSquare.Name = "textSquare";
            this.textSquare.ReadOnly = true;
            this.textSquare.Size = new System.Drawing.Size(100, 23);
            this.textSquare.TabIndex = 34;
            this.textSquare.Tag = "Square";
            this.textSquare.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textSquare.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textCross
            // 
            this.textCross.Location = new System.Drawing.Point(650, 265);
            this.textCross.Name = "textCross";
            this.textCross.ReadOnly = true;
            this.textCross.Size = new System.Drawing.Size(100, 23);
            this.textCross.TabIndex = 33;
            this.textCross.Tag = "Cross";
            this.textCross.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textCross.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textCircle
            // 
            this.textCircle.Location = new System.Drawing.Point(648, 213);
            this.textCircle.Name = "textCircle";
            this.textCircle.ReadOnly = true;
            this.textCircle.Size = new System.Drawing.Size(100, 23);
            this.textCircle.TabIndex = 32;
            this.textCircle.Tag = "Circle";
            this.textCircle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textCircle.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textTriangle
            // 
            this.textTriangle.Location = new System.Drawing.Point(648, 165);
            this.textTriangle.Name = "textTriangle";
            this.textTriangle.ReadOnly = true;
            this.textTriangle.Size = new System.Drawing.Size(100, 23);
            this.textTriangle.TabIndex = 31;
            this.textTriangle.Tag = "Triangle";
            this.textTriangle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textTriangle.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textDpadLeft
            // 
            this.textDpadLeft.Location = new System.Drawing.Point(11, 300);
            this.textDpadLeft.Name = "textDpadLeft";
            this.textDpadLeft.ReadOnly = true;
            this.textDpadLeft.Size = new System.Drawing.Size(100, 23);
            this.textDpadLeft.TabIndex = 30;
            this.textDpadLeft.Tag = "DpadLeft";
            this.textDpadLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textDpadLeft.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textDpadDown
            // 
            this.textDpadDown.Location = new System.Drawing.Point(12, 257);
            this.textDpadDown.Name = "textDpadDown";
            this.textDpadDown.ReadOnly = true;
            this.textDpadDown.Size = new System.Drawing.Size(100, 23);
            this.textDpadDown.TabIndex = 29;
            this.textDpadDown.Tag = "DpadDown";
            this.textDpadDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textDpadDown.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textDpadRight
            // 
            this.textDpadRight.Location = new System.Drawing.Point(12, 206);
            this.textDpadRight.Name = "textDpadRight";
            this.textDpadRight.ReadOnly = true;
            this.textDpadRight.Size = new System.Drawing.Size(100, 23);
            this.textDpadRight.TabIndex = 28;
            this.textDpadRight.Tag = "DpadRight";
            this.textDpadRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textDpadRight.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textDpadUp
            // 
            this.textDpadUp.Location = new System.Drawing.Point(12, 165);
            this.textDpadUp.Name = "textDpadUp";
            this.textDpadUp.ReadOnly = true;
            this.textDpadUp.Size = new System.Drawing.Size(100, 23);
            this.textDpadUp.TabIndex = 27;
            this.textDpadUp.Tag = "DpadUp";
            this.textDpadUp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textDpadUp.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // panelMovement
            // 
            this.panelMovement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelMovement.Controls.Add(this.labelMovement);
            this.panelMovement.Controls.Add(this.picLStickLeft);
            this.panelMovement.Controls.Add(this.textMoveLeft);
            this.panelMovement.Controls.Add(this.picLStickDown);
            this.panelMovement.Controls.Add(this.textMoveBackward);
            this.panelMovement.Controls.Add(this.picLStickRight);
            this.panelMovement.Controls.Add(this.textMoveRight);
            this.panelMovement.Controls.Add(this.picLStickUp);
            this.panelMovement.Controls.Add(this.textMoveForward);
            this.panelMovement.Location = new System.Drawing.Point(12, 389);
            this.panelMovement.Name = "panelMovement";
            this.panelMovement.Size = new System.Drawing.Size(292, 93);
            this.panelMovement.TabIndex = 25;
            // 
            // labelMovement
            // 
            this.labelMovement.ForeColor = System.Drawing.Color.White;
            this.labelMovement.Location = new System.Drawing.Point(171, 2);
            this.labelMovement.Name = "labelMovement";
            this.labelMovement.Size = new System.Drawing.Size(114, 19);
            this.labelMovement.TabIndex = 31;
            this.labelMovement.Text = "movement";
            this.labelMovement.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // picLStickLeft
            // 
            this.picLStickLeft.Image = global::DS4ConsolePort.Properties.Resources.LStickLeft;
            this.picLStickLeft.Location = new System.Drawing.Point(108, 31);
            this.picLStickLeft.Name = "picLStickLeft";
            this.picLStickLeft.Size = new System.Drawing.Size(24, 24);
            this.picLStickLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLStickLeft.TabIndex = 34;
            this.picLStickLeft.TabStop = false;
            // 
            // textMoveLeft
            // 
            this.textMoveLeft.Location = new System.Drawing.Point(2, 31);
            this.textMoveLeft.Name = "textMoveLeft";
            this.textMoveLeft.ReadOnly = true;
            this.textMoveLeft.Size = new System.Drawing.Size(100, 23);
            this.textMoveLeft.TabIndex = 33;
            this.textMoveLeft.Tag = "LStickLeft";
            this.textMoveLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textMoveLeft.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // picLStickDown
            // 
            this.picLStickDown.Image = global::DS4ConsolePort.Properties.Resources.LStickDown;
            this.picLStickDown.Location = new System.Drawing.Point(132, 59);
            this.picLStickDown.Name = "picLStickDown";
            this.picLStickDown.Size = new System.Drawing.Size(24, 24);
            this.picLStickDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLStickDown.TabIndex = 32;
            this.picLStickDown.TabStop = false;
            // 
            // textMoveBackward
            // 
            this.textMoveBackward.Location = new System.Drawing.Point(25, 60);
            this.textMoveBackward.Name = "textMoveBackward";
            this.textMoveBackward.ReadOnly = true;
            this.textMoveBackward.Size = new System.Drawing.Size(100, 23);
            this.textMoveBackward.TabIndex = 31;
            this.textMoveBackward.Tag = "LStickDown";
            this.textMoveBackward.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textMoveBackward.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // picLStickRight
            // 
            this.picLStickRight.Image = global::DS4ConsolePort.Properties.Resources.LStickRight;
            this.picLStickRight.Location = new System.Drawing.Point(156, 31);
            this.picLStickRight.Name = "picLStickRight";
            this.picLStickRight.Size = new System.Drawing.Size(24, 24);
            this.picLStickRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLStickRight.TabIndex = 30;
            this.picLStickRight.TabStop = false;
            // 
            // textMoveRight
            // 
            this.textMoveRight.Location = new System.Drawing.Point(186, 32);
            this.textMoveRight.Name = "textMoveRight";
            this.textMoveRight.ReadOnly = true;
            this.textMoveRight.Size = new System.Drawing.Size(100, 23);
            this.textMoveRight.TabIndex = 29;
            this.textMoveRight.Tag = "LStickRight";
            this.textMoveRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textMoveRight.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // picLStickUp
            // 
            this.picLStickUp.Image = global::DS4ConsolePort.Properties.Resources.LStickUp;
            this.picLStickUp.Location = new System.Drawing.Point(132, 3);
            this.picLStickUp.Name = "picLStickUp";
            this.picLStickUp.Size = new System.Drawing.Size(24, 24);
            this.picLStickUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLStickUp.TabIndex = 28;
            this.picLStickUp.TabStop = false;
            // 
            // textMoveForward
            // 
            this.textMoveForward.Location = new System.Drawing.Point(25, 3);
            this.textMoveForward.Name = "textMoveForward";
            this.textMoveForward.ReadOnly = true;
            this.textMoveForward.Size = new System.Drawing.Size(100, 23);
            this.textMoveForward.TabIndex = 27;
            this.textMoveForward.Tag = "LStickUp";
            this.textMoveForward.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textMoveForward.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textR2
            // 
            this.textR2.Location = new System.Drawing.Point(540, 58);
            this.textR2.Name = "textR2";
            this.textR2.ReadOnly = true;
            this.textR2.Size = new System.Drawing.Size(100, 23);
            this.textR2.TabIndex = 10;
            this.textR2.Tag = "R2";
            this.textR2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textR2.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textR1
            // 
            this.textR1.Location = new System.Drawing.Point(648, 107);
            this.textR1.Name = "textR1";
            this.textR1.ReadOnly = true;
            this.textR1.Size = new System.Drawing.Size(100, 23);
            this.textR1.TabIndex = 9;
            this.textR1.Tag = "R1";
            this.textR1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textR1.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textL2
            // 
            this.textL2.Location = new System.Drawing.Point(128, 58);
            this.textL2.Name = "textL2";
            this.textL2.ReadOnly = true;
            this.textL2.Size = new System.Drawing.Size(100, 23);
            this.textL2.TabIndex = 2;
            this.textL2.Tag = "L2";
            this.textL2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textL2.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // textL1
            // 
            this.textL1.Location = new System.Drawing.Point(12, 107);
            this.textL1.Name = "textL1";
            this.textL1.ReadOnly = true;
            this.textL1.Size = new System.Drawing.Size(100, 23);
            this.textL1.TabIndex = 1;
            this.textL1.Tag = "L1";
            this.textL1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textL1.DoubleClick += new System.EventHandler(this.BindBox_DoubleClick);
            // 
            // picDS4
            // 
            this.picDS4.Image = global::DS4ConsolePort.Properties.Resources.DS4_Config;
            this.picDS4.Location = new System.Drawing.Point(55, 53);
            this.picDS4.Name = "picDS4";
            this.picDS4.Size = new System.Drawing.Size(657, 408);
            this.picDS4.TabIndex = 0;
            this.picDS4.TabStop = false;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.groupWoWFolder);
            this.tabAdvanced.Controls.Add(this.groupHaptics);
            this.tabAdvanced.Controls.Add(this.groupDS4CSettings);
            this.tabAdvanced.Controls.Add(this.groupInteraction);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(764, 498);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // groupWoWFolder
            // 
            this.groupWoWFolder.Controls.Add(this.buttonLocateWoW);
            this.groupWoWFolder.Controls.Add(this.textWoWFolder);
            this.groupWoWFolder.Location = new System.Drawing.Point(4, 4);
            this.groupWoWFolder.Name = "groupWoWFolder";
            this.groupWoWFolder.Size = new System.Drawing.Size(501, 52);
            this.groupWoWFolder.TabIndex = 31;
            this.groupWoWFolder.TabStop = false;
            this.groupWoWFolder.Text = "wow folder";
            // 
            // buttonLocateWoW
            // 
            this.buttonLocateWoW.Location = new System.Drawing.Point(402, 21);
            this.buttonLocateWoW.Name = "buttonLocateWoW";
            this.buttonLocateWoW.Size = new System.Drawing.Size(91, 24);
            this.buttonLocateWoW.TabIndex = 32;
            this.buttonLocateWoW.Text = "find wow";
            this.buttonLocateWoW.UseVisualStyleBackColor = true;
            this.buttonLocateWoW.Click += new System.EventHandler(this.buttonLocateWoW_Click);
            // 
            // textWoWFolder
            // 
            this.textWoWFolder.Location = new System.Drawing.Point(6, 22);
            this.textWoWFolder.Name = "textWoWFolder";
            this.textWoWFolder.ReadOnly = true;
            this.textWoWFolder.Size = new System.Drawing.Size(390, 23);
            this.textWoWFolder.TabIndex = 31;
            // 
            // groupHaptics
            // 
            this.groupHaptics.Controls.Add(this.checkLightbarBattery);
            this.groupHaptics.Controls.Add(this.checkEnableAdvancedHaptics);
            this.groupHaptics.Controls.Add(this.panelAdvancedHaptics);
            this.groupHaptics.Location = new System.Drawing.Point(10, 189);
            this.groupHaptics.Name = "groupHaptics";
            this.groupHaptics.Size = new System.Drawing.Size(440, 207);
            this.groupHaptics.TabIndex = 28;
            this.groupHaptics.TabStop = false;
            this.groupHaptics.Text = "Haptics";
            // 
            // checkLightbarBattery
            // 
            this.checkLightbarBattery.AutoSize = true;
            this.checkLightbarBattery.Location = new System.Drawing.Point(6, 22);
            this.checkLightbarBattery.Name = "checkLightbarBattery";
            this.checkLightbarBattery.Size = new System.Drawing.Size(189, 19);
            this.checkLightbarBattery.TabIndex = 0;
            this.checkLightbarBattery.Text = "flash lightbar when battery low";
            this.checkLightbarBattery.UseVisualStyleBackColor = true;
            this.checkLightbarBattery.CheckedChanged += new System.EventHandler(this.checkLightbarBattery_CheckedChanged);
            // 
            // checkEnableAdvancedHaptics
            // 
            this.checkEnableAdvancedHaptics.AutoSize = true;
            this.checkEnableAdvancedHaptics.Location = new System.Drawing.Point(6, 47);
            this.checkEnableAdvancedHaptics.Name = "checkEnableAdvancedHaptics";
            this.checkEnableAdvancedHaptics.Size = new System.Drawing.Size(156, 19);
            this.checkEnableAdvancedHaptics.TabIndex = 25;
            this.checkEnableAdvancedHaptics.Text = "enable advanced haptics";
            this.checkEnableAdvancedHaptics.UseVisualStyleBackColor = true;
            this.checkEnableAdvancedHaptics.CheckedChanged += new System.EventHandler(this.checkEnableAdvancedHaptics_CheckedChanged);
            // 
            // panelAdvancedHaptics
            // 
            this.panelAdvancedHaptics.Controls.Add(this.groupHapticStatus);
            this.panelAdvancedHaptics.Controls.Add(this.groupHapticSettings);
            this.panelAdvancedHaptics.Location = new System.Drawing.Point(6, 72);
            this.panelAdvancedHaptics.Name = "panelAdvancedHaptics";
            this.panelAdvancedHaptics.Size = new System.Drawing.Size(434, 128);
            this.panelAdvancedHaptics.TabIndex = 24;
            // 
            // groupHapticStatus
            // 
            this.groupHapticStatus.Controls.Add(this.checkHapticsAttached);
            this.groupHapticStatus.Controls.Add(this.checkHapticsUserLoggedIn);
            this.groupHapticStatus.Controls.Add(this.labelPlayerInfo);
            this.groupHapticStatus.Location = new System.Drawing.Point(3, 3);
            this.groupHapticStatus.Name = "groupHapticStatus";
            this.groupHapticStatus.Size = new System.Drawing.Size(210, 122);
            this.groupHapticStatus.TabIndex = 25;
            this.groupHapticStatus.TabStop = false;
            this.groupHapticStatus.Text = "HapticStatus";
            // 
            // checkHapticsAttached
            // 
            this.checkHapticsAttached.AutoSize = true;
            this.checkHapticsAttached.Enabled = false;
            this.checkHapticsAttached.Location = new System.Drawing.Point(9, 22);
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
            this.checkHapticsUserLoggedIn.Location = new System.Drawing.Point(21, 47);
            this.checkHapticsUserLoggedIn.Name = "checkHapticsUserLoggedIn";
            this.checkHapticsUserLoggedIn.Size = new System.Drawing.Size(101, 19);
            this.checkHapticsUserLoggedIn.TabIndex = 15;
            this.checkHapticsUserLoggedIn.Text = "user logged in";
            this.checkHapticsUserLoggedIn.UseVisualStyleBackColor = true;
            // 
            // labelPlayerInfo
            // 
            this.labelPlayerInfo.AutoSize = true;
            this.labelPlayerInfo.Location = new System.Drawing.Point(6, 73);
            this.labelPlayerInfo.Name = "labelPlayerInfo";
            this.labelPlayerInfo.Size = new System.Drawing.Size(0, 15);
            this.labelPlayerInfo.TabIndex = 23;
            // 
            // groupHapticSettings
            // 
            this.groupHapticSettings.Controls.Add(this.checkRumbleDamage);
            this.groupHapticSettings.Controls.Add(this.checkRumbleTarget);
            this.groupHapticSettings.Controls.Add(this.checkLightbarHealth);
            this.groupHapticSettings.Controls.Add(this.checkLightbarClass);
            this.groupHapticSettings.Location = new System.Drawing.Point(219, 3);
            this.groupHapticSettings.Name = "groupHapticSettings";
            this.groupHapticSettings.Size = new System.Drawing.Size(210, 122);
            this.groupHapticSettings.TabIndex = 24;
            this.groupHapticSettings.TabStop = false;
            this.groupHapticSettings.Text = "HapticSettings";
            // 
            // checkRumbleDamage
            // 
            this.checkRumbleDamage.AutoSize = true;
            this.checkRumbleDamage.Location = new System.Drawing.Point(6, 97);
            this.checkRumbleDamage.Name = "checkRumbleDamage";
            this.checkRumbleDamage.Size = new System.Drawing.Size(110, 19);
            this.checkRumbleDamage.TabIndex = 3;
            this.checkRumbleDamage.Text = "rumble damage";
            this.checkRumbleDamage.UseVisualStyleBackColor = true;
            this.checkRumbleDamage.CheckedChanged += new System.EventHandler(this.checkRumbleDamage_CheckedChanged);
            // 
            // checkRumbleTarget
            // 
            this.checkRumbleTarget.AutoSize = true;
            this.checkRumbleTarget.Location = new System.Drawing.Point(6, 72);
            this.checkRumbleTarget.Name = "checkRumbleTarget";
            this.checkRumbleTarget.Size = new System.Drawing.Size(95, 19);
            this.checkRumbleTarget.TabIndex = 2;
            this.checkRumbleTarget.Text = "rumbletarget";
            this.checkRumbleTarget.UseVisualStyleBackColor = true;
            this.checkRumbleTarget.CheckedChanged += new System.EventHandler(this.checkRumbleTarget_CheckedChanged);
            // 
            // checkLightbarHealth
            // 
            this.checkLightbarHealth.AutoSize = true;
            this.checkLightbarHealth.Location = new System.Drawing.Point(6, 47);
            this.checkLightbarHealth.Name = "checkLightbarHealth";
            this.checkLightbarHealth.Size = new System.Drawing.Size(103, 19);
            this.checkLightbarHealth.TabIndex = 1;
            this.checkLightbarHealth.Text = "lightbar health";
            this.checkLightbarHealth.UseVisualStyleBackColor = true;
            this.checkLightbarHealth.CheckedChanged += new System.EventHandler(this.checkLightbarHealth_CheckedChanged);
            // 
            // checkLightbarClass
            // 
            this.checkLightbarClass.AutoSize = true;
            this.checkLightbarClass.Location = new System.Drawing.Point(6, 22);
            this.checkLightbarClass.Name = "checkLightbarClass";
            this.checkLightbarClass.Size = new System.Drawing.Size(95, 19);
            this.checkLightbarClass.TabIndex = 0;
            this.checkLightbarClass.Text = "lightbar class";
            this.checkLightbarClass.UseVisualStyleBackColor = true;
            this.checkLightbarClass.CheckedChanged += new System.EventHandler(this.checkLightbarClass_CheckedChanged);
            // 
            // groupDS4CSettings
            // 
            this.groupDS4CSettings.Controls.Add(this.checkStartMinimized);
            this.groupDS4CSettings.Controls.Add(this.checkCloseTray);
            this.groupDS4CSettings.Controls.Add(this.checkMinTray);
            this.groupDS4CSettings.Location = new System.Drawing.Point(4, 62);
            this.groupDS4CSettings.Name = "groupDS4CSettings";
            this.groupDS4CSettings.Size = new System.Drawing.Size(166, 99);
            this.groupDS4CSettings.TabIndex = 27;
            this.groupDS4CSettings.TabStop = false;
            this.groupDS4CSettings.Text = "ds4consoleport settings";
            // 
            // checkStartMinimized
            // 
            this.checkStartMinimized.AutoSize = true;
            this.checkStartMinimized.Location = new System.Drawing.Point(6, 24);
            this.checkStartMinimized.Name = "checkStartMinimized";
            this.checkStartMinimized.Size = new System.Drawing.Size(108, 19);
            this.checkStartMinimized.TabIndex = 2;
            this.checkStartMinimized.Text = "start minimized";
            this.checkStartMinimized.UseVisualStyleBackColor = true;
            this.checkStartMinimized.CheckedChanged += new System.EventHandler(this.checkStartMinimized_CheckedChanged);
            // 
            // checkCloseTray
            // 
            this.checkCloseTray.AutoSize = true;
            this.checkCloseTray.Location = new System.Drawing.Point(6, 74);
            this.checkCloseTray.Name = "checkCloseTray";
            this.checkCloseTray.Size = new System.Drawing.Size(90, 19);
            this.checkCloseTray.TabIndex = 1;
            this.checkCloseTray.Text = "close to tray";
            this.checkCloseTray.UseVisualStyleBackColor = true;
            this.checkCloseTray.CheckedChanged += new System.EventHandler(this.checkCloseTray_CheckedChanged);
            // 
            // checkMinTray
            // 
            this.checkMinTray.AutoSize = true;
            this.checkMinTray.Location = new System.Drawing.Point(6, 49);
            this.checkMinTray.Name = "checkMinTray";
            this.checkMinTray.Size = new System.Drawing.Size(112, 19);
            this.checkMinTray.TabIndex = 0;
            this.checkMinTray.Text = "minimize to tray";
            this.checkMinTray.UseVisualStyleBackColor = true;
            this.checkMinTray.CheckedChanged += new System.EventHandler(this.checkMinTray_CheckedChanged);
            // 
            // groupInteraction
            // 
            this.groupInteraction.Controls.Add(this.groupInteractionSettings);
            this.groupInteraction.Controls.Add(this.checkWindowAttached);
            this.groupInteraction.Location = new System.Drawing.Point(177, 62);
            this.groupInteraction.Name = "groupInteraction";
            this.groupInteraction.Size = new System.Drawing.Size(241, 121);
            this.groupInteraction.TabIndex = 26;
            this.groupInteraction.TabStop = false;
            this.groupInteraction.Text = "interaction";
            // 
            // groupInteractionSettings
            // 
            this.groupInteractionSettings.Controls.Add(this.checkDisableBG);
            this.groupInteractionSettings.Controls.Add(this.checkSendKeysDirect);
            this.groupInteractionSettings.Location = new System.Drawing.Point(6, 47);
            this.groupInteractionSettings.Name = "groupInteractionSettings";
            this.groupInteractionSettings.Size = new System.Drawing.Size(229, 69);
            this.groupInteractionSettings.TabIndex = 27;
            this.groupInteractionSettings.TabStop = false;
            this.groupInteractionSettings.Text = "settings";
            // 
            // checkDisableBG
            // 
            this.checkDisableBG.AutoSize = true;
            this.checkDisableBG.Location = new System.Drawing.Point(6, 47);
            this.checkDisableBG.Name = "checkDisableBG";
            this.checkDisableBG.Size = new System.Drawing.Size(179, 19);
            this.checkDisableBG.TabIndex = 16;
            this.checkDisableBG.Text = "disable when wow not found";
            this.checkDisableBG.UseVisualStyleBackColor = true;
            this.checkDisableBG.CheckedChanged += new System.EventHandler(this.checkDisableBG_CheckedChanged);
            // 
            // checkSendKeysDirect
            // 
            this.checkSendKeysDirect.AutoSize = true;
            this.checkSendKeysDirect.Enabled = false;
            this.checkSendKeysDirect.Location = new System.Drawing.Point(7, 22);
            this.checkSendKeysDirect.Name = "checkSendKeysDirect";
            this.checkSendKeysDirect.Size = new System.Drawing.Size(193, 19);
            this.checkSendKeysDirect.TabIndex = 14;
            this.checkSendKeysDirect.Text = "send keystrokes directly to wow";
            this.checkSendKeysDirect.UseVisualStyleBackColor = true;
            this.checkSendKeysDirect.CheckedChanged += new System.EventHandler(this.checkSendKeysDirect_CheckedChanged);
            // 
            // checkWindowAttached
            // 
            this.checkWindowAttached.AutoSize = true;
            this.checkWindowAttached.Enabled = false;
            this.checkWindowAttached.Location = new System.Drawing.Point(6, 22);
            this.checkWindowAttached.Name = "checkWindowAttached";
            this.checkWindowAttached.Size = new System.Drawing.Size(103, 19);
            this.checkWindowAttached.TabIndex = 13;
            this.checkWindowAttached.Text = "window found";
            this.checkWindowAttached.UseVisualStyleBackColor = true;
            // 
            // menuNotify
            // 
            this.menuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dS4ConsolePortToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.menuNotify.Name = "menuNotify";
            this.menuNotify.Size = new System.Drawing.Size(160, 54);
            // 
            // dS4ConsolePortToolStripMenuItem
            // 
            this.dS4ConsolePortToolStripMenuItem.Name = "dS4ConsolePortToolStripMenuItem";
            this.dS4ConsolePortToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.dS4ConsolePortToolStripMenuItem.Text = "DS4ConsolePort";
            this.dS4ConsolePortToolStripMenuItem.Click += new System.EventHandler(this.NotifyIcon_DoubleClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(778, 531);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DS4ConsolePort";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl.ResumeLayout(false);
            this.tabConsolePort.ResumeLayout(false);
            this.groupVersion.ResumeLayout(false);
            this.groupVersion.PerformLayout();
            this.groupDS4Status.ResumeLayout(false);
            this.groupDS4Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnectionType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConsolePort)).EndInit();
            this.tabKeybinds.ResumeLayout(false);
            this.tabKeybinds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigR2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConfigL2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picResetBinds)).EndInit();
            this.panelCamera.ResumeLayout(false);
            this.panelCamera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRDeadzone)).EndInit();
            this.panelMovement.ResumeLayout(false);
            this.panelMovement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLStickUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDS4)).EndInit();
            this.tabAdvanced.ResumeLayout(false);
            this.groupWoWFolder.ResumeLayout(false);
            this.groupWoWFolder.PerformLayout();
            this.groupHaptics.ResumeLayout(false);
            this.groupHaptics.PerformLayout();
            this.panelAdvancedHaptics.ResumeLayout(false);
            this.groupHapticStatus.ResumeLayout(false);
            this.groupHapticStatus.PerformLayout();
            this.groupHapticSettings.ResumeLayout(false);
            this.groupHapticSettings.PerformLayout();
            this.groupDS4CSettings.ResumeLayout(false);
            this.groupDS4CSettings.PerformLayout();
            this.groupInteraction.ResumeLayout(false);
            this.groupInteraction.PerformLayout();
            this.groupInteractionSettings.ResumeLayout(false);
            this.groupInteractionSettings.PerformLayout();
            this.menuNotify.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerUpdateUI;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConsolePort;
        private System.Windows.Forms.TabPage tabKeybinds;
        private System.Windows.Forms.PictureBox picDS4;
        private System.Windows.Forms.TextBox textL2;
        private System.Windows.Forms.TextBox textL1;
        private System.Windows.Forms.TextBox textR2;
        private System.Windows.Forms.TextBox textR1;
        private System.Windows.Forms.TextBox textOptions;
        private System.Windows.Forms.TextBox textShare;
        private System.Windows.Forms.Panel panelMovement;
        private System.Windows.Forms.PictureBox picLStickLeft;
        private System.Windows.Forms.TextBox textMoveLeft;
        private System.Windows.Forms.PictureBox picLStickDown;
        private System.Windows.Forms.TextBox textMoveBackward;
        private System.Windows.Forms.PictureBox picLStickRight;
        private System.Windows.Forms.TextBox textMoveRight;
        private System.Windows.Forms.PictureBox picLStickUp;
        private System.Windows.Forms.TextBox textMoveForward;
        private System.Windows.Forms.TextBox textPS;
        private System.Windows.Forms.TextBox textSquare;
        private System.Windows.Forms.TextBox textCross;
        private System.Windows.Forms.TextBox textCircle;
        private System.Windows.Forms.TextBox textTriangle;
        private System.Windows.Forms.TextBox textDpadLeft;
        private System.Windows.Forms.TextBox textDpadDown;
        private System.Windows.Forms.TextBox textDpadRight;
        private System.Windows.Forms.TextBox textDpadUp;
        private System.Windows.Forms.TabPage tabAdvanced;
        private System.Windows.Forms.Label labelRightSpeed;
        private System.Windows.Forms.Label labelRightDead;
        private System.Windows.Forms.Label labelRightCurve;
        private System.Windows.Forms.Label labelPlayerInfo;
        private System.Windows.Forms.CheckBox checkHapticsUserLoggedIn;
        private System.Windows.Forms.CheckBox checkWindowAttached;
        private System.Windows.Forms.CheckBox checkHapticsAttached;
        private System.Windows.Forms.Panel panelRStickAxis;
        private System.Windows.Forms.Panel panelCamera;
        private System.Windows.Forms.Label labelChangeBindings;
        private System.Windows.Forms.Panel panelAdvancedHaptics;
        private System.Windows.Forms.GroupBox groupHapticSettings;
        private System.Windows.Forms.CheckBox checkRumbleDamage;
        private System.Windows.Forms.CheckBox checkRumbleTarget;
        private System.Windows.Forms.CheckBox checkLightbarHealth;
        private System.Windows.Forms.CheckBox checkLightbarClass;
        private System.Windows.Forms.PictureBox picConsolePort;
        private System.Windows.Forms.CheckBox checkEnableAdvancedHaptics;
        private System.Windows.Forms.NumericUpDown numRSpeed;
        private System.Windows.Forms.NumericUpDown numRCurve;
        private System.Windows.Forms.NumericUpDown numRDeadzone;
        private System.Windows.Forms.ComboBox comboTouchMode;
        private System.Windows.Forms.TextBox textBindTouchRight;
        private System.Windows.Forms.TextBox textBindTouchLeft;
        private System.Windows.Forms.Label labelTouchMode;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.Label labelMovement;
        private System.Windows.Forms.GroupBox groupHapticStatus;
        private System.Windows.Forms.Label labelTouchUpper;
        private System.Windows.Forms.ContextMenuStrip menuNotify;
        private System.Windows.Forms.ToolStripMenuItem dS4ConsolePortToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PictureBox picResetBinds;
        private System.Windows.Forms.GroupBox groupInteraction;
        private System.Windows.Forms.GroupBox groupInteractionSettings;
        private System.Windows.Forms.CheckBox checkDisableBG;
        private System.Windows.Forms.CheckBox checkSendKeysDirect;
        private System.Windows.Forms.GroupBox groupDS4CSettings;
        private System.Windows.Forms.CheckBox checkCloseTray;
        private System.Windows.Forms.CheckBox checkMinTray;
        private System.Windows.Forms.CheckBox checkStartMinimized;
        private System.Windows.Forms.Label labelCenterCursor;
        private System.Windows.Forms.GroupBox groupHaptics;
        private System.Windows.Forms.CheckBox checkLightbarBattery;
        private System.Windows.Forms.GroupBox groupVersion;
        private System.Windows.Forms.Label labelDS4VersionInstalled;
        private System.Windows.Forms.Label labelCPVersionInstalled;
        private System.Windows.Forms.Label labelDS4CP;
        private System.Windows.Forms.Label labelCPAddon;
        private System.Windows.Forms.GroupBox groupDS4Status;
        private System.Windows.Forms.PictureBox picConnectionType;
        private System.Windows.Forms.Label labelConnectionStatus;
        private System.Windows.Forms.Label labelDS4VersionAvailable;
        private System.Windows.Forms.Label labelCPVersionAvailable;
        private System.Windows.Forms.Label labelVersionAvailable;
        private System.Windows.Forms.Label labelVersionInstalled;
        private System.Windows.Forms.GroupBox groupWoWFolder;
        private System.Windows.Forms.Button buttonLocateWoW;
        private System.Windows.Forms.TextBox textWoWFolder;
        private System.Windows.Forms.Button buttonDS4UpdateNow;
        private System.Windows.Forms.Button buttonCPUpdateNow;
        private System.Windows.Forms.PictureBox picConfigR2;
        private System.Windows.Forms.PictureBox picConfigL2;
    }
}

