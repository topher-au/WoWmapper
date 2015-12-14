using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;
using System.Diagnostics;

namespace DS4CP_Updater
{
    class Program
    {
        static List<string> bannedFiles = new List<string>()
        {
            "ds4cp_updater.exe",
            "icsharpcode.sharpziplib.dll"
        };

        static void Main(string[] args)
        {
            if (args.Length < 2) return;
            if (args[0] != "-update") return;
            if (!File.Exists(args[1])) return;

            var ds4c = Process.GetProcessesByName("WoWmapper");
            if (ds4c.Length == 0) ds4c = Process.GetProcessesByName("WoWmapper.vshost");
            if(ds4c.Length > 0)
            if(ds4c[0] != null)
            {
                var ds4p = ds4c[0];
                while(!ds4p.HasExited) { Thread.Sleep(100); }
            }

            using (FileStream f = new FileStream(args[1], FileMode.Open))
            {
                ZipFile zf = new ZipFile(f);
                foreach(ZipEntry ze in zf)
                {
                    if (bannedFiles.Contains(ze.Name.ToLower())) continue;
                    var outDir = Path.GetDirectoryName(ze.Name);
                    if (!Directory.Exists(outDir) && outDir != string.Empty) Directory.CreateDirectory(outDir);
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
