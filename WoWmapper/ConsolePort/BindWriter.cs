using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WoWmapper.Overlay;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.ConsolePort
{
    public static class BindWriter
    {
        public static void WriteBinds()
        {
            if (ProcessManager.GameProcess == null)
                return;

            // Attempt to locate WoWmapper.lua
            var addonPath = Path.GetDirectoryName(ProcessManager.GameProcess.MainModule.FileName);
            if (addonPath == null) return;

            var luaFile = Path.Combine(addonPath, "Interface\\AddOns\\ConsolePort\\Controllers\\WoWmapper.lua");

            if (!File.Exists(luaFile)) return;

            // If export is disabled, write blank file
            if (!Settings.Default.ExportBindings)
            {
                Log.WriteLine("Clearing ConsolePort bindings at {0}", luaFile);
                File.WriteAllText(luaFile, "return");
                return;
            }

            // Get last modify time and see if it needs updating
            var fileInfo = new FileInfo(luaFile);
            if (fileInfo.LastWriteTime >= Settings.Default.BindingsModified) return;

            Log.WriteLine("Writing ConsolePort bind configuration to {0}", luaFile);

            var templateStreamInfo =
                Application.GetResourceStream(new Uri("pack://application:,,,/ConsolePort/WoWmapper.lua"));
            var templateStream = templateStreamInfo.Stream;
            var templateReader = new StreamReader(templateStream);

            var bindDict = new BindDictionary();

            try
            {
                using (var fileStream = new FileStream(luaFile, FileMode.Create))
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
                App.Overlay.PopupNotification(new OverlayNotification()
                {
                    Content =
                        $"Your ConsolePort settings have been updated. Reload your user interface to activate your changes.",
                    Header = "Settings updated",
                    Duration = 10000,
                    UniqueID = "CP_UPDATED"
                });
                new Task(() =>
                {
                    WoWInput.SendKeyDown(Key.LeftAlt, true);
                    WoWInput.SendKeyDown(Key.LeftCtrl, true);
                    WoWInput.SendKeyDown(Key.LeftShift, true);
                    Thread.Sleep(50);
                    WoWInput.SendKeyDown(Key.F12, true);
                    Thread.Sleep(50);
                    WoWInput.SendKeyUp(Key.F12, true);
                    WoWInput.SendKeyUp(Key.LeftShift, true);
                    WoWInput.SendKeyUp(Key.LeftCtrl, true);
                    WoWInput.SendKeyUp(Key.LeftAlt, true);
                }).Start();
                
            }
            catch
            {
                
            }
            
        }
    }
}