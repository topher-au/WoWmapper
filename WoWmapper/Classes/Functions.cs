using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Octokit;

namespace WoWmapper.Classes
{
    public static class Functions
    {
        public static event DownloadFileCompletedEventHandler DownloadFileCompleted;
        public static event DownloadFileProgressChangedEventHandler DownloadFileProgressChanged;

        public delegate void DownloadFileCompletedEventHandler();
        public delegate void DownloadFileProgressChangedEventHandler(DownloadProgressChangedEventArgs Args);

        static List<string> DefaultWoWFolders = new List<string>()
        {
            "C:\\Program Files (x86)\\World of Warcraft",
            "C:\\Program Files\\World of Warcraft",
            "C:\\Games\\World of Warcraft",
            "D:\\Games\\World of Warcraft",
            "C:\\World of Warcraft",
            "D:\\World of Warcraft"
        };

        public static bool ValidateWoWPath(string TestPath = null)
        {
            string SearchPath;
            if (TestPath != null)
            {
                SearchPath = TestPath;
            } else
            {
                SearchPath = Properties.Settings.Default.WoWFolder;
            }
            if (File.Exists(Path.Combine(SearchPath, "WoW.exe"))
            || File.Exists(Path.Combine(SearchPath, "WoW-64.exe")))
            {
                return true;
            }
            return false;
        }

        public static bool FindWoWPath()
        {
            foreach (string Folder in DefaultWoWFolders)
            {
                if (File.Exists(Path.Combine(Folder, "WoW.exe"))
                 || File.Exists(Path.Combine(Folder, "WoW-64.exe")))
                {
                    Properties.Settings.Default.WoWFolder = Folder;
                    Properties.Settings.Default.Save();
                    return true;
                }
            }
            return false;
        }

        public static async Task<Release> GetWoWmapperLatest()
        {
            try { 
                var client = new GitHubClient(new ProductHeaderValue("WoWmapper"));
                var releases = await client.Release.GetAll("topher-au", "WoWmapper");
                return releases[0];
            } catch
            {
                return null;
            }
        }

        public static async Task<Release> GetConsolePortLatest()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("WoWmapper"));
                var releases = await client.Release.GetAll("seblindfors", "ConsolePort");
                return releases[0];
            } catch
            {
                return null;
            }
            
            
        }

        public static async Task<bool> DownloadFile(string URL, string DestFile)
        {
            try
            {
                Uri uri = new Uri(URL);

                var client = new WebClient();
                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                if (File.Exists(DestFile)) File.Delete(DestFile);
                await client.DownloadFileTaskAsync(uri, DestFile);
                return true;
            }
            catch
            {
                
            }
            return false;

        }

        private static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadFileProgressChanged?.Invoke(e);
        }

        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadFileCompleted?.Invoke();
        }
    }
}
