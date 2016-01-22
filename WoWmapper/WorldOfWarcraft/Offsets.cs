using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using FileMode = System.IO.FileMode;

namespace WoWmapper.WorldOfWarcraft
{
    public static class Offsets
    {
        private const long OffsetFileHeader = 0x4F4646534554;
        private const string OffsetFileName = "offsets.dat";
        private const string OffsetFileUri = "http://www.xobanimot.com/wowmapper/offsets.dat";

        public static void WriteDefault()
        {
            var offsetData = new OffsetData();
            using (var outStream = new FileStream(OffsetFileName, FileMode.Create))
            {
                BinaryWriter outWriter = new BinaryWriter(outStream);

                // Write offset header
                outWriter.Write(OffsetFileHeader);

                // Write offset version/game build
                outWriter.Write(offsetData.GameBuild);

                // Write offset count
                outWriter.Write(offsetData.Offsets.Count);

                // Write offsets
                foreach (var offset in offsetData.Offsets)
                {
                    // Write offset type
                    outWriter.Write((byte)offset.Key);

                    // Write offset value
                    outWriter.Write(offset.Value);
                }

                outWriter.Flush();
            }
        }

        public static OffsetData Read()
        {
            if(!File.Exists(OffsetFileName)) Offsets.WriteDefault();
            try
            {
                using (var inStream = new FileStream(OffsetFileName, FileMode.Open))
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

                    return new OffsetData()
                    {
                        GameBuild = gameBuild,
                        Offsets = outDictionary
                    };
                }
            } catch { }
            return null;
        }

        public static OffsetData Read(byte[] data)
        {
            try
            {
                using (MemoryStream inStream = new MemoryStream(data))
                {
                    Dictionary<OffsetType, ulong> outDictionary = new Dictionary<OffsetType, ulong>();
                    BinaryReader inReader = new BinaryReader(inStream);

                    var fileHeader = inReader.ReadUInt64();

                    if (fileHeader != OffsetFileHeader) return null;

                    var gameBuild = inReader.ReadUInt32();

                    var offsetCount = inReader.ReadUInt32();

                    for (int i = 0; i < offsetCount; i++)
                    {
                        var offsetType = (OffsetType) inReader.ReadByte();
                        var offsetValue = inReader.ReadUInt64();
                        outDictionary.Add(offsetType, offsetValue);
                    }

                    return new OffsetData()
                    {
                        GameBuild = gameBuild,
                        Offsets = outDictionary
                    };
                }
            }
            catch
            {
            }
            return null;
        }

        public static bool FetchOnline()
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    var webOffsets = webClient.DownloadData(OffsetFileUri);
                    File.WriteAllBytes("offsets.dat", webOffsets);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    // Base offset data
    public class OffsetData
    {
        public uint GameBuild { get; set; }
            = 20886;

        public Dictionary<OffsetType, ulong> Offsets { get; set; }
            = new Dictionary<OffsetType, ulong>() {
                { OffsetType.GameBuild, 16654516 },
                { OffsetType.GameState, 23730078 },
                { OffsetType.PlayerName, 25092160 },
                { OffsetType.PlayerClass, 25092565 },
                { OffsetType.TargetGuid, 23731720 },
                { OffsetType.MouseGuid, 23731672 },
                { OffsetType.MouseLook, 23441240 },
                { OffsetType.PlayerBase, 23094048},
                { OffsetType.PlayerLevel, 344 },
                { OffsetType.PlayerHealthCurrent, 240 },
                { OffsetType.PlayerHealthMax, 268 }
            };
    }

    public enum OffsetType
    {
        GameBuild,
        GameState,
        PlayerName,
        PlayerClass,
        TargetGuid,
        MouseGuid,
        MouseLook,
        PlayerBase,
        PlayerLevel,
        PlayerHealthCurrent,
        PlayerHealthMax,
    }
}
