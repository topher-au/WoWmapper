namespace WoWmapper
{
    partial class PluginSelectForm
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
            this.listInputPlugins = new System.Windows.Forms.ListBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.labelSelectPlugin = new System.Windows.Forms.Label();
            this.buttonOpenPlugins = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listInputPlugins
            // 
            this.listInputPlugins.FormattingEnabled = true;
            this.listInputPlugins.ItemHeight = 15;
            this.listInputPlugins.Location = new System.Drawing.Point(12, 27);
            this.listInputPlugins.Name = "listInputPlugins";
            this.listInputPlugins.Size = new System.Drawing.Size(191, 64);
            this.listInputPlugins.TabIndex = 0;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(12, 97);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(191, 31);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "load plugin";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // labelSelectPlugin
            // 
            this.labelSelectPlugin.AutoSize = true;
            this.labelSelectPlugin.Location = new System.Drawing.Point(12, 9);
            this.labelSelectPlugin.Name = "labelSelectPlugin";
            this.labelSelectPlugin.Size = new System.Drawing.Size(74, 15);
            this.labelSelectPlugin.TabIndex = 2;
            this.labelSelectPlugin.Text = "select plugin";
            // 
            // buttonOpenPlugins
            // 
            this.buttonOpenPlugins.Location = new System.Drawing.Point(12, 190);
            this.buttonOpenPlugins.Name = "buttonOpenPlugins";
            this.buttonOpenPlugins.Size = new System.Drawing.Size(191, 24);
            this.buttonOpenPlugins.TabIndex = 3;
            this.buttonOpenPlugins.Text = "Open Plugins Folder";
            this.buttonOpenPlugins.UseVisualStyleBackColor = true;
            this.buttonOpenPlugins.Click += new System.EventHandler(this.buttonOpenPlugins_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 45);
            this.label1.TabIndex = 4;
            this.label1.Text = "If no plugins are visible you may\r\nneed to right click the input DLLs\r\nand select" +
    " Properties, then Unblock";
            // 
            // PluginSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 224);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOpenPlugins);
            this.Controls.Add(this.labelSelectPlugin);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.listInputPlugins);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PluginSelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Plugins";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listInputPlugins;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label labelSelectPlugin;
        private System.Windows.Forms.Button buttonOpenPlugins;
        private System.Windows.Forms.Label label1;
    }
}