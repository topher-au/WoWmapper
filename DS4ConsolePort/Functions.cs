using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace DS4ConsolePort
{
    public static class Functions
    {
        private static List<string> wowDefaultPaths = new List<string>()
        {
            "C:\\Program Files\\World of Warcraft",
            "C:\\Program Files (x86)\\World of Warcraft",
            "C:\\World of Warcraft",
            "D:\\World of Warcraft",
        };

        public static string TryFindWoWPath()
        {
            string wowPath = string.Empty;

            // Check registry
            string wowRegPath = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Blizzard Entertainment\\World of Warcraft", "InstallPath", String.Empty).ToString();

            var testPaths = wowDefaultPaths;
            if (wowRegPath != string.Empty)
                testPaths.Insert(0, wowRegPath);

            // Find install path
            foreach (string Path in testPaths)
            {
                if (CheckForWowExe(Path))
                {
                    wowPath = Path.TrimEnd('\\');
                    break;
                }
            }

            return wowPath;
        }

        public static bool CheckForWowExe(string Folder)
        {
            if (File.Exists(Path.Combine(Folder, "Wow.exe")) || 
                File.Exists(Path.Combine(Folder, "Wow-64.exe"))) return true;

            return false;
        }

        public static bool CheckIsWowRunning()
        {
            var proc32 = Process.GetProcessesByName("Wow");
            var proc64 = Process.GetProcessesByName("Wow-64");
            if (proc64.GetLength(0) > 0 || proc32.GetLength(0) > 0) return true;
            return false;
        }

        public static bool InstallAddOn(string AddonZip)
        {
            if (Directory.Exists(Properties.Settings.Default.WoWInstallPath))
            {
                var addonFolder = Path.Combine(Properties.Settings.Default.WoWInstallPath, "Interface\\AddOns");
                if (!Directory.Exists(addonFolder)) return false;
                using (FileStream zf = new FileStream(AddonZip, FileMode.Open))
                {
                    ZipFile z = new ZipFile(zf);
                    // Find root folders
                    List<string> rootFolders = new List<string>();
                    foreach (ZipEntry f in z)
                    {
                        var root = f.Name.Substring(0, f.Name.IndexOf("/"));
                        if (!rootFolders.Contains(root)) rootFolders.Add(root);
                    }
                    foreach(var folder in rootFolders)
                    {
                        var addonSubFolder = Path.Combine(addonFolder, folder);
                        if (Directory.Exists(addonSubFolder))
                        {
                            Console.WriteLine("Deleting " + addonSubFolder);
                            Directory.Delete(addonSubFolder, true);
                        }
                    }
                }
                Console.WriteLine("Extracting update");
                FastZip unzip = new FastZip();
                unzip.ExtractZip(AddonZip, addonFolder, ".*?");
                return true;
            }
            else return false;
            
                
        }

        public static TocInfo ReadTocInfo(string TocFile)
        {
            if (!File.Exists(TocFile)) return default(TocInfo);
            using (StreamReader s = new StreamReader(TocFile))
            {
                string line = "##";
                TocInfo outinfo = new TocInfo() { SavedVariables = new List<string>(), SavedVariablesPerCharacter = new List<string>() };
                while (line.StartsWith("##"))
                {
                    line = s.ReadLine();
                    var ls = line.Split(':');
                    switch (ls[0])
                    {
                        case "## Interface":
                            outinfo.Interface = line.Substring(line.IndexOf(":") + 1).Trim();
                            break;

                        case "## Title":
                            outinfo.Title = line.Substring(line.IndexOf(":") + 1).Trim();
                            break;

                        case "## Notes":
                            outinfo.Notes = line.Substring(line.IndexOf(":") + 1).Trim();
                            break;

                        case "## Version":
                            outinfo.Version = new Version(line.Substring(line.IndexOf(":") + 1));
                            break;

                        case "## SavedVariables":
                            var ps = line.Substring(line.IndexOf(":") + 1);
                            var svs = ps.Split(',');
                            foreach (var x in svs) outinfo.SavedVariables.Add(x.Trim());
                            break;

                        case "## SavedVariablesPerCharacter":
                            var split = line.Substring(line.IndexOf(":") + 1);
                            var varNames = split.Split(',');
                            foreach (var Var in varNames) outinfo.SavedVariablesPerCharacter.Add(Var.Trim());
                            break;
                    }
                }
                return outinfo;
            }
        }

        public static Version CheckAddonVersion(string WowPath, string AddonName)
        {
            var AddonPath = Path.Combine(WowPath, "Interface\\AddOns", AddonName);
            var AddonToc = Path.Combine(AddonPath, AddonName + ".toc");
            if (File.Exists(AddonToc))
            {
                var AddonInfo = ReadTocInfo(AddonToc);
                return AddonInfo.Version;
            }
            return default(Version);
        }

        public static bool CheckWoWPath()
        {
            var WoWPath = Properties.Settings.Default.WoWInstallPath;
            if (Directory.Exists(WoWPath)) return true;
            WoWPath = Functions.TryFindWoWPath();
            Properties.Settings.Default.WoWInstallPath = WoWPath;
            Properties.Settings.Default.Save();
            if (WoWPath == string.Empty) return false;
            return true;
        }
    }

    public struct TocInfo
    {
        public string Interface;
        public string Title;
        public string Notes;
        public Version Version;
        public List<string> SavedVariables;
        public List<string> SavedVariablesPerCharacter;
    }


}