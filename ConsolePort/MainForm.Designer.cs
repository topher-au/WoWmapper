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
            this.timerUpdateUI = new System.Windows.Forms.Timer(this.components);
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.labelSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelControllerState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConsolePort = new System.Windows.Forms.TabPage();
            this.labelWoWAddon = new System.Windows.Forms.Label();
            this.labelDS4ConsolePort = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabKeybinds = new System.Windows.Forms.TabPage();
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
            this.checkWindowAttached = new System.Windows.Forms.CheckBox();
            this.menuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dS4ConsolePortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabConsolePort.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabKeybinds.SuspendLayout();
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
            this.panelAdvancedHaptics.SuspendLayout();
            this.groupHapticStatus.SuspendLayout();
            this.groupHapticSettings.SuspendLayout();
            this.menuNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerUpdateUI
            // 
            this.timerUpdateUI.Enabled = true;
            this.timerUpdateUI.Interval = 5;
            this.timerUpdateUI.Tick += new System.EventHandler(this.timerUpdateUI_Tick);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelSpacer,
            this.labelControllerState});
            this.statusBar.Location = new System.Drawing.Point(0, 544);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(856, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 5;
            this.statusBar.Text = "statusStrip1";
            // 
            // labelSpacer
            // 
            this.labelSpacer.Name = "labelSpacer";
            this.labelSpacer.Size = new System.Drawing.Size(776, 17);
            this.labelSpacer.Spring = true;
            // 
            // labelControllerState
            // 
            this.labelControllerState.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.labelControllerState.Name = "labelControllerState";
            this.labelControllerState.Size = new System.Drawing.Size(65, 17);
            this.labelControllerState.Text = "Connected";
            this.labelControllerState.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabConsolePort);
            this.tabControl.Controls.Add(this.tabKeybinds);
            this.tabControl.Controls.Add(this.tabAdvanced);
            this.tabControl.Location = new System.Drawing.Point(4, 4);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(850, 534);
            this.tabControl.TabIndex = 6;
            // 
            // tabConsolePort
            // 
            this.tabConsolePort.Controls.Add(this.labelWoWAddon);
            this.tabConsolePort.Controls.Add(this.labelDS4ConsolePort);
            this.tabConsolePort.Controls.Add(this.pictureBox1);
            this.tabConsolePort.Location = new System.Drawing.Point(4, 24);
            this.tabConsolePort.Name = "tabConsolePort";
            this.tabConsolePort.Padding = new System.Windows.Forms.Padding(3);
            this.tabConsolePort.Size = new System.Drawing.Size(842, 506);
            this.tabConsolePort.TabIndex = 0;
            this.tabConsolePort.Text = "consoleport";
            this.tabConsolePort.UseVisualStyleBackColor = true;
            // 
            // labelWoWAddon
            // 
            this.labelWoWAddon.AutoSize = true;
            this.labelWoWAddon.Location = new System.Drawing.Point(304, 330);
            this.labelWoWAddon.Name = "labelWoWAddon";
            this.labelWoWAddon.Size = new System.Drawing.Size(250, 15);
            this.labelWoWAddon.TabIndex = 2;
            this.labelWoWAddon.Text = "Unofficial World of Warcraft Controller Addon";
            // 
            // labelDS4ConsolePort
            // 
            this.labelDS4ConsolePort.AutoSize = true;
            this.labelDS4ConsolePort.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDS4ConsolePort.Location = new System.Drawing.Point(340, 300);
            this.labelDS4ConsolePort.Name = "labelDS4ConsolePort";
            this.labelDS4ConsolePort.Size = new System.Drawing.Size(172, 30);
            this.labelDS4ConsolePort.TabIndex = 1;
            this.labelDS4ConsolePort.Text = "DS4ConsolePort";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ConsolePort.Properties.Resources.CONSOLEPORTLOGO;
            this.pictureBox1.Location = new System.Drawing.Point(173, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 291);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabKeybinds
            // 
            this.tabKeybinds.BackColor = System.Drawing.Color.DimGray;
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
            this.tabKeybinds.Size = new System.Drawing.Size(842, 506);
            this.tabKeybinds.TabIndex = 1;
            this.tabKeybinds.Text = "keybinds";
            // 
            // labelTouchUpper
            // 
            this.labelTouchUpper.ForeColor = System.Drawing.Color.Yellow;
            this.labelTouchUpper.Location = new System.Drawing.Point(370, 35);
            this.labelTouchUpper.Name = "labelTouchUpper";
            this.labelTouchUpper.Size = new System.Drawing.Size(100, 70);
            this.labelTouchUpper.TabIndex = 42;
            this.labelTouchUpper.Text = "Press the upper touchpad to toggle mouse control";
            this.labelTouchUpper.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelTouchMode
            // 
            this.labelTouchMode.ForeColor = System.Drawing.Color.White;
            this.labelTouchMode.Location = new System.Drawing.Point(357, 109);
            this.labelTouchMode.Name = "labelTouchMode";
            this.labelTouchMode.Size = new System.Drawing.Size(124, 15);
            this.labelTouchMode.TabIndex = 41;
            this.labelTouchMode.Text = "Touchpad Mode";
            this.labelTouchMode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBindTouchRight
            // 
            this.textBindTouchRight.Location = new System.Drawing.Point(423, 160);
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
            this.textBindTouchLeft.Location = new System.Drawing.Point(340, 160);
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
            this.comboTouchMode.Location = new System.Drawing.Point(360, 127);
            this.comboTouchMode.Name = "comboTouchMode";
            this.comboTouchMode.Size = new System.Drawing.Size(121, 23);
            this.comboTouchMode.TabIndex = 38;
            this.comboTouchMode.SelectedIndexChanged += new System.EventHandler(this.comboTouchMode_SelectedIndexChanged);
            // 
            // textPS
            // 
            this.textPS.Location = new System.Drawing.Point(370, 397);
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
            this.labelChangeBindings.Location = new System.Drawing.Point(336, 14);
            this.labelChangeBindings.Name = "labelChangeBindings";
            this.labelChangeBindings.Size = new System.Drawing.Size(172, 15);
            this.labelChangeBindings.TabIndex = 37;
            this.labelChangeBindings.Text = "Double click binding to change";
            // 
            // textOptions
            // 
            this.textOptions.Location = new System.Drawing.Point(475, 44);
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
            this.textShare.Location = new System.Drawing.Point(267, 44);
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
            this.panelCamera.Location = new System.Drawing.Point(538, 397);
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
            this.labelCamera.Text = "label2";
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
            this.labelRightCurve.Location = new System.Drawing.Point(74, 35);
            this.labelRightCurve.Name = "labelRightCurve";
            this.labelRightCurve.Size = new System.Drawing.Size(38, 15);
            this.labelRightCurve.TabIndex = 22;
            this.labelRightCurve.Text = "Curve";
            // 
            // labelRightDead
            // 
            this.labelRightDead.AutoSize = true;
            this.labelRightDead.Location = new System.Drawing.Point(74, 6);
            this.labelRightDead.Name = "labelRightDead";
            this.labelRightDead.Size = new System.Drawing.Size(59, 15);
            this.labelRightDead.TabIndex = 21;
            this.labelRightDead.Text = "Deadzone";
            // 
            // textSquare
            // 
            this.textSquare.Location = new System.Drawing.Point(727, 301);
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
            this.textCross.Location = new System.Drawing.Point(727, 265);
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
            this.textCircle.Location = new System.Drawing.Point(727, 213);
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
            this.textTriangle.Location = new System.Drawing.Point(728, 165);
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
            this.textDpadLeft.Location = new System.Drawing.Point(11, 301);
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
            this.textDpadDown.Location = new System.Drawing.Point(12, 258);
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
            this.textDpadRight.Location = new System.Drawing.Point(12, 208);
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
            this.panelMovement.Location = new System.Drawing.Point(12, 397);
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
            this.labelMovement.Text = "label3";
            this.labelMovement.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // picLStickLeft
            // 
            this.picLStickLeft.Image = global::ConsolePort.Properties.Resources.LStickLeft;
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
            this.picLStickDown.Image = global::ConsolePort.Properties.Resources.LStickDown;
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
            this.picLStickRight.Image = global::ConsolePort.Properties.Resources.LStickRight;
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
            this.picLStickUp.Image = global::ConsolePort.Properties.Resources.LStickUp;
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
            this.textR2.Location = new System.Drawing.Point(581, 58);
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
            this.textR1.Location = new System.Drawing.Point(727, 107);
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
            this.textL2.Location = new System.Drawing.Point(161, 58);
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
            this.picDS4.Image = global::ConsolePort.Properties.Resources.DS4_Config;
            this.picDS4.Location = new System.Drawing.Point(93, 53);
            this.picDS4.Name = "picDS4";
            this.picDS4.Size = new System.Drawing.Size(657, 408);
            this.picDS4.TabIndex = 0;
            this.picDS4.TabStop = false;
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.checkEnableAdvancedHaptics);
            this.tabAdvanced.Controls.Add(this.panelAdvancedHaptics);
            this.tabAdvanced.Controls.Add(this.checkWindowAttached);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 24);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(842, 506);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "advanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // checkEnableAdvancedHaptics
            // 
            this.checkEnableAdvancedHaptics.AutoSize = true;
            this.checkEnableAdvancedHaptics.Location = new System.Drawing.Point(14, 343);
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
            this.panelAdvancedHaptics.Location = new System.Drawing.Point(3, 365);
            this.panelAdvancedHaptics.Name = "panelAdvancedHaptics";
            this.panelAdvancedHaptics.Size = new System.Drawing.Size(424, 138);
            this.panelAdvancedHaptics.TabIndex = 24;
            // 
            // groupHapticStatus
            // 
            this.groupHapticStatus.Controls.Add(this.checkHapticsAttached);
            this.groupHapticStatus.Controls.Add(this.checkHapticsUserLoggedIn);
            this.groupHapticStatus.Controls.Add(this.labelPlayerInfo);
            this.groupHapticStatus.Location = new System.Drawing.Point(5, 3);
            this.groupHapticStatus.Name = "groupHapticStatus";
            this.groupHapticStatus.Size = new System.Drawing.Size(200, 122);
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
            this.labelPlayerInfo.Size = new System.Drawing.Size(89, 15);
            this.labelPlayerInfo.TabIndex = 23;
            this.labelPlayerInfo.Text = "Player info here";
            // 
            // groupHapticSettings
            // 
            this.groupHapticSettings.Controls.Add(this.checkRumbleDamage);
            this.groupHapticSettings.Controls.Add(this.checkRumbleTarget);
            this.groupHapticSettings.Controls.Add(this.checkLightbarHealth);
            this.groupHapticSettings.Controls.Add(this.checkLightbarClass);
            this.groupHapticSettings.Location = new System.Drawing.Point(211, 3);
            this.groupHapticSettings.Name = "groupHapticSettings";
            this.groupHapticSettings.Size = new System.Drawing.Size(200, 122);
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
            // checkWindowAttached
            // 
            this.checkWindowAttached.AutoSize = true;
            this.checkWindowAttached.Enabled = false;
            this.checkWindowAttached.Location = new System.Drawing.Point(14, 13);
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
            this.ClientSize = new System.Drawing.Size(856, 566);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusBar);
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
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabConsolePort.ResumeLayout(false);
            this.tabConsolePort.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabKeybinds.ResumeLayout(false);
            this.tabKeybinds.PerformLayout();
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
            this.tabAdvanced.PerformLayout();
            this.panelAdvancedHaptics.ResumeLayout(false);
            this.groupHapticStatus.ResumeLayout(false);
            this.groupHapticStatus.PerformLayout();
            this.groupHapticSettings.ResumeLayout(false);
            this.groupHapticSettings.PerformLayout();
            this.menuNotify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerUpdateUI;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConsolePort;
        private System.Windows.Forms.TabPage tabKeybinds;
        private System.Windows.Forms.ToolStripStatusLabel labelSpacer;
        private System.Windows.Forms.ToolStripStatusLabel labelControllerState;
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
        private System.Windows.Forms.PictureBox pictureBox1;
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
        private System.Windows.Forms.Label labelWoWAddon;
        private System.Windows.Forms.Label labelDS4ConsolePort;
        private System.Windows.Forms.Label labelTouchUpper;
        private System.Windows.Forms.ContextMenuStrip menuNotify;
        private System.Windows.Forms.ToolStripMenuItem dS4ConsolePortToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

