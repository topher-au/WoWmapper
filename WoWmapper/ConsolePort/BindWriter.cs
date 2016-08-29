using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.ConsolePort
{
    public static class BindWriter
    {
        public static void WriteBinds()
        {
            if (ProcessManager.GameProcess == null)
                return;

            var file = Path.Combine(Path.GetDirectoryName(ProcessManager.GameProcess.MainModule.FileName),
                "Interface\\AddOns\\ConsolePort\\Controllers\\WoWmapper.lua");

            if (!File.Exists(file)) return;

            if (!Properties.Settings.Default.ExportBindings)
            {
                Log.WriteLine("Clearing ConsolePort bindings at {0}", file);
                File.WriteAllText(file, string.Empty);
                return;
            }

            Log.WriteLine("Writing ConsolePort bind configuration to {0}", file);

            var templateStreamInfo =
                Application.GetResourceStream(new Uri("pack://application:,,,/ConsolePort/WoWmapper.lua"));
            var templateStream = templateStreamInfo.Stream;
            var templateReader = new StreamReader(templateStream);

            var bindDict = new BindDictionary();

            using (var fileStream = new FileStream(file, FileMode.Create))
            using (var fileWriter = new StreamWriter(fileStream))
                while (!templateReader.EndOfStream)
                {
                    var templateLine = templateReader.ReadLine();
                    if (templateLine == null) break;

                    if (!templateLine.Contains('<') && !templateLine.Contains('>'))
                    {
                        fileWriter.WriteLine(templateLine);
                        continue;
                    }

                    var templateKey = templateLine.Substring(templateLine.IndexOf('<') + 1);
                    templateKey = templateKey.Substring(0, templateKey.IndexOf('>'));

                    try
                    {
                        var bindValue = bindDict.GetBindKey(templateKey);
                        fileWriter.WriteLine(templateLine.Replace("<" + templateKey + ">", bindValue));
                    }
                    catch
                    {
                        fileWriter.WriteLine(templateLine);
                    }
                }
        }
    }
}