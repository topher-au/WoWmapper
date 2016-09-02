using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Octokit;

namespace WoWmapper
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow
    {
        private Release _release;
        public UpdateWindow(Release release)
        {
            InitializeComponent();
            _release = release;
            TextReleaseTitle.Text = _release.Name;
            TextReleaseNotes.Text = _release.Body;
        }

        private void ButtonDownload_OnClick(object sender, RoutedEventArgs e)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(250));
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250));

            fadeOut.Completed += (o, args) =>
            {
                PanelButton.Visibility = Visibility.Hidden;
                GridProgress.BeginAnimation(OpacityProperty, fadeIn);
                GridProgress.Visibility = Visibility.Visible;
            };
            PanelButton.BeginAnimation(OpacityProperty, fadeOut);
            DownloadUpdate();
        }

        private void DownloadUpdate()
        {
            try
            {
                var fileUrl = _release.Assets.First(asset => asset.BrowserDownloadUrl.ToLower().EndsWith(".zip"));
                if(File.Exists("_update.zip")) File.Delete("_update.zip");

                var updateClient = new WebClient();
                updateClient.DownloadProgressChanged += (sender, args) =>
                {
                    ProgressDownload.Value = args.ProgressPercentage;
                };
                updateClient.DownloadFileCompleted += (sender, args) =>
                {
                    if (args.Cancelled) return;

                    // Extract updater from downloaded archive
                    var updateZip = ZipFile.OpenRead("_update.zip");
                    var updater = updateZip.Entries.First(entry => entry.Name == "WoWmapper_Updater.exe");
                    updater.ExtractToFile("WoWmapper_Updater.exe", true);

                    // Start downloaded updater
                    Process.Start("WoWmapper_Updater.exe", "_update.zip");
                    App.Current.Shutdown();
                };
                Closing += (sender, args) =>
                {
                    updateClient.CancelAsync();
                };
                updateClient.DownloadFileAsync(new Uri(fileUrl.BrowserDownloadUrl), "_update.zip");
            }
            catch
            {
                
            }
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }
}