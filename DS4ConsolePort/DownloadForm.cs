using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace DS4ConsolePort
{
    public partial class DownloadForm : Form
    {
        private Thread updateThread;
        private WebClient wClient = new WebClient();
        public string OutputFile = string.Empty;

        private void StartDownload(string DownloadUrl)
        {
            updateThread = new Thread(() =>
            {
                OutputFile = DownloadUrl.Substring(DownloadUrl.LastIndexOf("/") + 1);
                wClient.DownloadFileAsync(new Uri(DownloadUrl), OutputFile);
            });
            updateThread.Start();
            ShowDialog();
        }

        public void SetProgressValue(int Value)
        {
            if(progressBar1.InvokeRequired)
            {
                try
                {
                    Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Value = Value;
                        Console.WriteLine(String.Format("Downloading... {0}%", Value));
                    });
                } catch
                {
                    // window was probably closed
                }
                
            }
        }

        public DownloadForm(string DownloadUrl)
        {
            InitializeComponent();
            wClient.Headers.Add(HttpRequestHeader.UserAgent, "DS4ConsolePort");
            wClient.DownloadProgressChanged += (obj, args) =>
            {
                if(!Disposing)
                    SetProgressValue(args.ProgressPercentage);
            };
            wClient.DownloadFileCompleted += (obj, args) =>
            {
                wClient.Dispose();
                DialogResult = DialogResult.OK;
            };
            StartDownload(DownloadUrl);
        }

        private void DownloadForm_Load(object sender, EventArgs e)
        {
        }

        private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            wClient.CancelAsync();
            wClient.Dispose();
            updateThread.Abort();
            try { File.Delete(OutputFile); } catch { }
            DialogResult = DialogResult.Cancel;
        }
    }
}