using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WoWmapper.WorldOfWarcraft
{
    public static class AddonParser
    {
        private const string InterfaceAddons = "Interface\\AddOns";
        public static List<AddonInfo> ListAllAddons(string wowPath)
        {
            var addonsPath = Path.Combine(wowPath, InterfaceAddons);
            if (!Directory.Exists(addonsPath)) return null;

            var addonList = new List<AddonInfo>();
            foreach (var folder in Directory.GetDirectories(addonsPath))
            {
                var addonName = folder.Substring(folder.LastIndexOf('\\')+1);
                var addonToc = Path.Combine(addonsPath, addonName, addonName + ".toc");
                if (!File.Exists(addonToc)) continue;

                var addonInfo = parseTocFile(addonToc);

                if (addonInfo == null) continue;

                addonList.Add(addonInfo);
            }

            return addonList;
        }

        private static AddonInfo parseTocFile(string tocFile)
        {
            using (var tocStream = new FileStream(tocFile, FileMode.Open))
            using (var tocReader = new StreamReader(tocStream))
            {
                var tocLine = tocReader.ReadLine();

                var addonInfo = new AddonInfo();
                
                while (tocLine != null && tocLine.StartsWith("##"))
                {
                    var infoType = tocLine.Substring(tocLine.IndexOf(' ') + 1, tocLine.IndexOf(':') - 3);
                    var infoValue = tocLine.Substring(tocLine.IndexOf(':') + 2);
                    switch (infoType.ToLower())
                    {
                        case "title":
                            addonInfo.Title = infoValue;
                            break;
                        case "interface":
                            addonInfo.Interface = infoValue;
                            break;
                        case "version":
                            addonInfo.Version = infoValue;
                            break;
                        case "author":
                            addonInfo.Author = infoValue;
                            break;
                        case "x-website":
                            addonInfo.Website = infoValue;
                            break;
                        case "x-localizations":
                            addonInfo.Localizations = infoValue;
                            break;
                        case "notes":
                            addonInfo.Notes = infoValue;
                            break;
                        case "savedvariablespercharacter":
                            addonInfo.SavedVariablesPerCharacter = infoValue;
                            break;
                        case "savedvariables":
                            addonInfo.SavedVariables = infoValue;
                            break;
                    }
                    tocLine = tocReader.ReadLine();
                }

                return addonInfo;
            }
        }

        public static AddonInfo GetAddonInfo(string wowPath, string addonFolder)
        {
            var addonPath = Path.Combine(wowPath, InterfaceAddons, addonFolder);
            if (!Directory.Exists(addonPath)) return null;

            var addonToc = Path.Combine(addonPath, addonFolder + ".toc");
            if(!File.Exists(addonToc)) return null;

            return parseTocFile(addonToc);
        }

        public static Version GetAddonVersion(AddonInfo addonInfo)
        {
            var addonVersion = addonInfo.Version;
            var addonSplit = addonVersion.Split('.');
            return new Version(int.Parse(addonSplit[0]), int.Parse(addonSplit[1]), int.Parse(addonSplit[2]));
        }
    }

    public class AddonInfo
    {
        public string Interface { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public string Localizations { get; set; }
        public string Notes { get; set; }
        public string SavedVariables { get; set; }
        public string SavedVariablesPerCharacter { get; set; }

        public override string ToString()
        {
            if (this.Title != string.Empty) return this.Title;
            return base.ToString();
        }
    }
}
