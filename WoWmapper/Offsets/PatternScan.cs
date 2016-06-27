using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoWmapper.MemoryReader;
using WoWmapper.Offsets;

namespace PatternScan
{
    public static class OffsetScanner
    {
        public static string GetPlayerName(Process process)
        {
            var ss = new SigScan(process, process.MainModule.BaseAddress, process.MainModule.ModuleMemorySize);
            var location = ss.FindPattern("48 8D 05 ?? ?? ?? ?? 41 b8 03 00 00 00 0f 1f 00", 3);
            var offset = MemoryManager.Read<int>(location, false);
            var pointer = location + 4 + offset + 0x10;
            
            var bName = new byte[12];
            var bytesRead = 0;

            MemoryManager.ReadProcessMemory(MemoryManager.MemoryHandle,
                pointer, bName,
                bName.Length, ref bytesRead);

            var nameLength = Array.FindIndex(bName, item => item == 0x00);

            if (nameLength > 0)
                return Encoding.Default.GetString(bName, 0, nameLength);
            else
                return Encoding.Default.GetString(bName, 0, bytesRead);
        }
    }

    public class OffsetBytePattern
    {
        public GameOffset Type { get; set; }
        public string Pattern { get; set; }
        public int PatternOffset { get; set; }
    }

    public enum GameOffset
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
        PlayerHealthMax
    }
}           