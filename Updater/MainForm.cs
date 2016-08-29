using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace Updater
{
    public partial class MainForm : Form
    {
        private string _callingProcess;
        private string _updateArchive;

        public MainForm()
        {
            InitializeComponent();
            var args = Environment.GetCommandLineArgs();
            if (args[1] != "update") Close();
            if (!File.Exists(args[3])) Close();
            _callingProcess = args[2];
            _updateArchive = args[3];
            Show();
            
            ProcessUpdate();
        }

        public async void ProcessUpdate()
        {
            LabelUpdateStatus.Text = "Waiting for WoWmapper to exit...";
            await Task.Run(() =>
            {
                while (true)
                {
                    var proc = Process.GetProcessesByName(_callingProcess);
                    if (proc.Length == 0) break;
                }
            });
            LabelUpdateStatus.Text = "Extracting files...";
            await Task.Run(() =>
            {
                var update = ZipFile.OpenRead(_updateArchive);
                foreach (var file in update.Entries)
                {
                    if (file.Name == "Updater.exe" || string.IsNullOrEmpty(Path.GetFileName(file.Name))) continue;
                    var destDir = Path.GetDirectoryName(file.FullName);
                    if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                    try
                    {
                        file.ExtractToFile(file.FullName, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred during extraction. You may need to update manually.\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                update.Dispose();
                File.Delete(_updateArchive);
                Process.Start("WoWmapper.exe");
            });
            Environment.Exit(0);
        }
    }
}
