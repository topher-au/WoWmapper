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
using WoWmapper_Updater.Properties;

namespace WoWmapper_Updater
{
    public partial class MainForm : Form
    {
        private const string _update = "_update";
        public MainForm()
        {
            InitializeComponent();
            var FileName = Environment.GetCommandLineArgs()[1];

            Text = Resources.WindowTitle;
            this.labelInstallText.Text = Resources.UpdateWaiting;

            Show();
            while(Process.GetProcesses().Count(process => process.ProcessName.ToLower() == "wowmapper") > 0)
                Application.DoEvents();


            this.labelInstallText.Text = Resources.UpdateInstalling;

            ExtractUpdate(FileName);

            if (File.Exists("Wowmapper.exe")) Process.Start("WoWmapper.exe", "-updated");

            Close();
        }

        public void ExtractUpdate(string zipFile)
        {
            if (!File.Exists(zipFile)) return;

            using (var zipStream = new FileStream(zipFile, FileMode.Open))
            {
                var zipArchive = new ZipArchive(zipStream);
                if(Directory.Exists(_update)) Directory.Delete(_update, true);

                zipArchive.ExtractToDirectory(_update);

                foreach (var file in Directory.GetFiles(_update, "*.*", SearchOption.AllDirectories))
                {
                    if (Path.GetFileName(file) == "WoWmapper_Updater.exe") continue;

                    // Move all update files
                    var destFile = file.Substring(_update.Length + 1);
                    var destFolder = destFile.Substring(0, destFile.Length - Path.GetFileName(destFile).Length).TrimEnd('\\');

                    if (!Directory.Exists(destFolder) && destFolder != string.Empty) Directory.CreateDirectory(destFolder);
                    Console.WriteLine($"Copying {file} to {destFile}");
                    File.Copy(file, destFile, true);
                }

                Directory.Delete(_update,true);
            }
        }
    }
}
