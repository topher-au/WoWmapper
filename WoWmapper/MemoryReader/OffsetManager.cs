using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WoWmapper.Classes;

namespace WoWmapper.Offsets
{
    public class OffsetManager
    {
        private const string RemoteOffsetUrl = "http://wowmapper.consoleport.net/offsets";
        private const string OffsetDatFile = "offsets.dat";
        private static string _localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");
        private static OffsetFile _loadedOffsets = null;

        public static bool OffsetsAvailable => _loadedOffsets != null;

        public static bool InitializeOffsets(int gameBuild)
        {
            var localOffsetFile = Path.Combine(_localAppData, "WoWmapper", OffsetDatFile);
            OffsetFile localOffsets = null;

            // Check if local offset file exists
            if (File.Exists(localOffsetFile))
            {
                // If so, load it, check version against process build
                localOffsets = OffsetFile.ReadFromFile(localOffsetFile);
                if (localOffsets.GameBuild == gameBuild)
                {
                    _loadedOffsets = localOffsets;
                    return true;
                }
            }

            // If file does not exist, attempt download from remote server
            var offsetClient = new WebClient();
            offsetClient.Headers.Add(HttpRequestHeader.UserAgent, $"WoWmapper/{Assembly.GetExecutingAssembly().GetName().Version}");
            try
            {
                var remoteOffsetData = offsetClient.DownloadData(new Uri(RemoteOffsetUrl));
                var remoteOffsets = OffsetFile.ReadFromArray(remoteOffsetData);

                if (remoteOffsets.GameBuild == gameBuild)
                {
                    var filePath = Path.GetDirectoryName(localOffsetFile);
                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
                    File.WriteAllBytes(localOffsetFile, remoteOffsetData);
                    _loadedOffsets = remoteOffsets;
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Server returned an error
                Logger.Write("Error downloading offsets: {0}", ex.Message);
            }
            
            

            // No valid offsets were located
            _loadedOffsets = null;
            return false;
        }

        public static int GetOffset(OffsetType offset)
        {
            if (!OffsetsAvailable) return 0;

            var iOffset = _loadedOffsets.Offsets[offset];
            return (int)iOffset;
        }

    }
}
