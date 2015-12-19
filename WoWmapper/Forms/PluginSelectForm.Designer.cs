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
            this.SuspendLayout();
            // 
            // listInputPlugins
            // 
            this.listInputPlugins.FormattingEnabled = true;
            this.listInputPlugins.ItemHeight = 15;
            this.listInputPlugins.Location = new System.Drawing.Point(12, 27);
            this.listInputPlugins.Name = "listInputPlugins";
            this.listInputPlugins.Size = new System.Drawing.Size(282, 229);
            this.listInputPlugins.TabIndex = 0;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(12, 262);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(282, 31);
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
            // PluginSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 306);
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
    }
}