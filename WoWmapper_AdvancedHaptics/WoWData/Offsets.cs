using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WoWmapper.WoWData
{
    public class OffsetReader
    {
        public bool Loaded { get; private set; } = false;
        private OffsetFile offsetFileData;

        public OffsetReader()
        {
            Load();
        }

        public void Load()
        {
            var exeLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var exePath = Path.GetDirectoryName(exeLoc);
            var offsetFile = Path.Combine(exePath, "offsets.xml");
            if (File.Exists(offsetFile))
            {
                // Load the offset data from file
                try
                {
                    using (FileStream inFile = new FileStream(offsetFile, FileMode.Open))
                    {
                        XmlSerializer x = new XmlSerializer(typeof(OffsetFile));
                        offsetFileData = (OffsetFile)x.Deserialize(inFile);
                    }
                    Loaded = true;
                }
                catch
                {
                    Loaded = false;
                }
            }
            else
            {
                Loaded = false;
            }
        }

        public bool BuildExists(int build, int arch)
        {
            if (Loaded && offsetFileData != null)
            {
                var BuildOffsets = offsetFileData.GameOffsetList.Find(offset => offset.GameBuild == build && offset.GameArchitecture == arch);
                if (BuildOffsets != null) return true;
            }
            return false;
        }

        public OffsetFile.GameOffset LoadOffsets(int build, int arch)
        {
            if (Loaded && offsetFileData != null)
            {
                var BuildOffsets = offsetFileData.GameOffsetList.Find(offset => offset.GameBuild == build && offset.GameArchitecture == arch);
                if (BuildOffsets != null) return BuildOffsets;
            }
            return default(OffsetFile.GameOffset);
        }
    }

    [XmlType("OffsetFile")]
    public class OffsetFile
    {
        [XmlAttribute("DS4CPVersion")]
        public string Version { get; set; }

        [XmlArray("GameOffsets")]
        public List<GameOffset> GameOffsetList { get; set; }

        [XmlType("GameOffset")]
        public class GameOffset
        {
            [XmlAttribute("GameBuild")]
            public int GameBuild { get; set; }

            [XmlAttribute("GameArchitecture")]
            public int GameArchitecture {get;set;}

            [XmlArray("Offsets")]
            public List<OffsetData> OffsetData { get; set; }

            public int ReadOffset(string Name)
            {
                return (int)OffsetData.Find(offset => offset.Name == Name).Address;
            }
        }

        public GameOffset GetBuildOffsets(int Build, int Architecture)
        {
            if(GameOffsetList != null)
            {
                return GameOffsetList.Find(offset => offset.GameBuild == Build && offset.GameArchitecture == Architecture);
            }
            return default(GameOffset);
        }

        [XmlType("Offset")]
        public class OffsetData
        {
            [XmlAttribute("Name")]
            public string Name { get;set; }

            [XmlAttribute("Address")]
            public uint Address { get; set; }

            public OffsetData()
            {
                Name = string.Empty;
                Address = 0;
            }

            public OffsetData(string n, uint o)
            {
                Name = n.ToString(); Address = o;
            }
        }
    }
}