using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DS4CP_Updater
{
    internal class Program
    {
        private static List<string> bannedFiles = new List<string>()
        {
            "wowmapper_updater.exe",
            "icsharpcode.sharpziplib.dll"
        };

        private static void Main(string[] args)
        {
            if (args.Length < 2) return;
            if (args[0] != "-update") return;
            if (!File.Exists(args[1])) return;

            var ds4c = Process.GetProcessesByName("WoWmapper");
            if (ds4c.Length == 0) ds4c = Process.GetProcessesByName("WoWmapper.vshost");
            if (ds4c.Length > 0)
                if (ds4c[0] != null)
                {
                    var ds4p = ds4c[0];
                    while (!ds4p.HasExited) { Thread.Sleep(100); }
                }

            using (FileStream f = new FileStream(args[1], FileMode.Open))
            {
                ZipFile zf = new ZipFile(f);
                foreach (ZipEntry ze in zf)
                {
                    if (bannedFiles.Contains(ze.Name.ToLower())) continue;
                    var outDir = Path.GetDirectoryName(ze.Name);
                    if (!Directory.Exists(outDir) && outDir != string.Empty)
                        try
                        {
                            Directory.CreateDirectory(outDir);
                        }
                        catch
                        {
                            continue;
                        }
                    if (Path.GetFileName(ze.Name) == string.Empty) continue;
                    using (FileStream of = new FileStream(ze.Name, FileMode.Create))
                    {
                        zf.GetInputStream(ze).CopyTo(of);
                    }
                }
            }
            File.Delete(args[1]);
            Process.Start("WoWmapper.exe", "-updated");
        }
    }
}