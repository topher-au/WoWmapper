using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WoWmapper.Input;

namespace WoWmapper.WorldOfWarcraft.AddOns
{
    public class ConsolePortBindWriter
    {
        public static void WriteBindFile(string fileName)
        {
            var cpBinds = BindingManager.GetConsolePortBindDictionary();
            var templateStream = Application.GetResourceStream(new Uri("pack://application:,,,/WorldOfWarcraft/AddOns/WoWmapper.lua"));
            var textTemplate = new StreamReader(templateStream.Stream);

            using (var outStream = new FileStream(fileName, FileMode.Create))
            using (var outWriter = new StreamWriter(outStream))

                while (!textTemplate.EndOfStream)
                {
                    // Parse lines from template
                    var textLine = textTemplate.ReadLine();

                    var templateStart = textLine.IndexOf('{') + 1;
                    if (templateStart != -1)
                    {
                        // Read template tag
                        var templateLength = textLine.Substring(templateStart).IndexOf('}');

                        if (templateLength == -1)
                        {
                            outWriter.WriteLine(textLine);
                            continue;
                        }
                        var templateTag = textLine.Substring(templateStart, templateLength);

                        try
                        {
                            var templateKey = cpBinds[templateTag];
                            outWriter.WriteLine(textLine.Replace($"{{{templateTag}}}", templateKey));
                        }
                        catch
                        {
                            outWriter.WriteLine(textLine);
                        }
                    }
                    else
                    {
                        outWriter.WriteLine(textLine);
                    }
                }
        }
    }
}
