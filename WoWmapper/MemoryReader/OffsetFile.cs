using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.Offsets
{
    public class OffsetFile
    {
        private const long OffsetFileHeader = 0x4F4646534554;

        public uint GameBuild { get; set; }
            = 21742;

        public Dictionary<OffsetType, ulong> Offsets { get; set; }
            = new Dictionary<OffsetType, ulong>() {
                { OffsetType.GameBuild, 0x0FE21B4 },
                { OffsetType.GameState, 22125182 },
                { OffsetType.PlayerName, 23162640 },
                { OffsetType.PlayerClass, 23163045 },
                { OffsetType.TargetGuid, 22126824 },
                { OffsetType.MouseGuid, 22126776 },
                { OffsetType.MouseLook, 19605144 },
                { OffsetType.PlayerBase, 21489280 }, 
                { OffsetType.PlayerLevel, 344 },
                { OffsetType.PlayerHealthCurrent, 240 },
                { OffsetType.PlayerHealthMax, 268 }
            };

        public static OffsetFile ReadFromFile(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            try
            {
                using (var inStream = new FileStream(fileName, FileMode.Open))
                {
                    Dictionary<OffsetType, ulong> outDictionary = new Dictionary<OffsetType, ulong>();
                    BinaryReader inReader = new BinaryReader(inStream);

                    var fileHeader = inReader.ReadUInt64();

                    if (fileHeader != OffsetFileHeader) return null;

                    var gameBuild = inReader.ReadUInt32();

                    var offsetCount = inReader.ReadUInt32();

                    for (int i = 0; i < offsetCount; i++)
                    {
                        var offsetType = (OffsetType)inReader.ReadByte();
                        var offsetValue = inReader.ReadUInt64();
                        outDictionary.Add(offsetType, offsetValue);
                    }

                    return new OffsetFile()
                    {
                        GameBuild = gameBuild,
                        Offsets = outDictionary
                    };
                }
            }
            catch { }
            return null;
        }

        public static OffsetFile ReadFromArray(byte[] offsetData)
        {
            try
            {
                using (var inStream = new MemoryStream(offsetData))
                {
                    Dictionary<OffsetType, ulong> outDictionary = new Dictionary<OffsetType, ulong>();
                    BinaryReader inReader = new BinaryReader(inStream);

                    var fileHeader = inReader.ReadUInt64();

                    if (fileHeader != OffsetFileHeader) return null;

                    var gameBuild = inReader.ReadUInt32();

                    var offsetCount = inReader.ReadUInt32();

                    for (int i = 0; i < offsetCount; i++)
                    {
                        var offsetType = (OffsetType)inReader.ReadByte();
                        var offsetValue = inReader.ReadUInt64();
                        outDictionary.Add(offsetType, offsetValue);
                    }

                    return new OffsetFile()
                    {
                        GameBuild = gameBuild,
                        Offsets = outDictionary
                    };
                }
            }
            catch { }
            return null;
        }

        public static void Write()
        {
            
        }
    }
}
