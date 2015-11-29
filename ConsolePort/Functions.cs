using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsolePort
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
                if (CheckWoWFolder(Path))
                {
                    wowPath = Path.TrimEnd('\\');
                    break;
                }
            }

            return wowPath;
        }

        private static bool CheckWoWFolder(string Folder)
        {
            if (File.Exists(Path.Combine(Folder, "WoW-64.exe"))) return true;
            return false;
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