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
            return true;
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
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("WoWmapper"));
                var releases = await client.Release.GetAll("topher-au", "WoWmapper");
                return releases[0];
            }
            catch (Exception ex)
            {
                Logger.Write($"Unable to retrieve WoWmapper version: {ex.Message}");
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
            } catch (Exception ex)
            {
                Logger.Write($"Unable to retrieve ConsolePort version: {ex.Message}");
                return null;
            }
            
            
        }
    }
}
