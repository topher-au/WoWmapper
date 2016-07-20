using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WoWmapper.MemoryReader;

namespace Farplane.Memory
{
    public static class OffsetScanner
    {
        private static SigScan _sigScan = new SigScan();

        //private static Dictionary<GameOffset, int> _offsets = new Dictionary<GameOffset, int>();

        //public static int GetOffset(GameOffset type)
        //{
        //    var foundOffset = _offsets.FirstOrDefault(offset => offset.Key == type).Value;
        //    if (foundOffset == 0)
        //    {
        //        var searchSuccess = FindOffset(type);
        //        if (!searchSuccess)
        //        {
        //            // Offset search failed! Abort! Abort!
        //            MessageBox.Show(
        //                $"A critical error occurred while scanning for offsets:\n\nUnable to locate offset:\n{type}\n\nThe application will now exit.",
        //                "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            Environment.Exit(0);
        //        }
        //        foundOffset = _offsets.FirstOrDefault(offset => offset.Key == type).Value;
        //    }

        //    return foundOffset;
        //}

        public static void Reset()
        {
            //_offsets.Clear();
            _sigScan.ResetRegion();
        }

        //private static bool FindOffset()
        //{
        //    // Build a table of offsets from the current process
        //    _sigScan.Process = MemoryManager.Process;
        //    _sigScan.Address = MemoryManager.Process.MainModule.BaseAddress;
        //    _sigScan.Size = MemoryManager.Process.MainModule.ModuleMemorySize;

        //    //var patternData = _bytePatterns.First(pattern => pattern.Type == offsetType);

        //    var pointer = _sigScan.FindPattern(patternData.Pattern, patternData.ByteOffset);
        //    if (pointer == IntPtr.Zero) return false;

        //    var offset = MemoryManager.Read<IntPtr>(pointer, false);
        //    _offsets.Add(patternData.Type, (int) offset);

        //    return true;
        //}

        public static IntPtr FindClassOffset()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "E8 ????????" + //- call WowT-64.exe+C09800
                    "33 ff" + 
                    "4C 8D 05 ????????" + //- lea r8,[WowT-64.exe+1A315A8] { [00000000] }
                    "48 8D 0D ????????" + //- lea rcx,[WowT-64.exe+172FCF0] { [7FF735EDAB08] }
                    "45 33 C9" + // xor r9d,r9d
                    "0FB6 D0" + // movzx edx,al
                    "89 ?? 24 28" + // mov [rsp+28],ebp
                    "48 89 ?? 24 20" + // mov [rsp+20],rbp
                    "E8 ????????" + // call WowT-64.exe+3715B0
                    "48 8B F8" + // mov rdi,rax
                    "E8 ????????           " + // call WowT-64.exe+C0E3C0
                    "45 33 C0              " + // xor r8d,r8d
                    "0FB6 D0               " + // movzx edx,al
                    "48 8B CF              " + // mov rcx,rdi
                    "E8 ????????           " + // call WowT-64.exe+546A20
                    "E9 ????????           " + // jmp WowT-64.exe+73DDDD
                    "0F10 05 ????????      " + // movups xmm0,[WowT-64.exe+1A501F8] { [00000000] }
                    "48 8D 54 24 ??        " + // lea rdx,[rsp+40]
                    "45 33 C9              " + // xor r9d,r9d
                    "45 33 C0              " + // xor r8d,r8d
                    "48 8B ??              " + // mov rcx,rdi
                    "0F11 44 24 ??         " + // movups [rsp+40],xmm0
                    "E8 ????????           " + // call WowT-64.exe+747460
                    "4C 8D 05 ????????     " + // lea r8,[WowT-64.exe+1160140] { ["ScriptEvents.cpp"] }
                    "48 8D 4C 24 ??        " + // lea rcx,[rsp+40]
                    "41 B9 280E0000        ", // mov r9d,00000E28 { 3624 }


                ByteOffset = 1
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            var classPtr = MemoryManager.Read<int>(patternOffset + 4 + patternPtr + 3, false);
            return patternOffset + 4 + patternPtr + 3 + classPtr + 4;
        }

        public static IntPtr FindGameState()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "40 53" + // push rbx
                    "48 83 EC 50" + // sub rsp,50 { 80 }
                    "0FB6 05 ????????" + // movzx eax,byte ptr [WowT-64.exe+1A36E50] { [00000000] }
                    "0FB6 DA" + // movzx ebx,dl
                    "84 C0" + // test al,al
                    "74 04" + // je WowT-64.exe+C08718
                    "84 D2" + // test dl,dl
                    "74 5D" + // je WowT-64.exe+C08775
                    "80 3D ???????? 00" + // cmp byte ptr [WowT-64.exe+1A36E51],00 { [0] }
                    "74 54" + // je WowT-64.exe+C08775
                    "84 DB" + // test bl,bl
                    "88 0D ????????" + // mov [WowT-64.exe+1A36E52],cl { [0] }
                    "0FB6 C0" + // movzx eax,al
                    "B9 01000000", // mov ecx,00000001 { 1 }

                ByteOffset = 26
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            return patternOffset + patternPtr + 5;
        }

        public static IntPtr FindMouseLook()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "0F44 F8" +
                    "8d 50 0d" +
                    "89 1d ????????" +
                    "89 5c 24 20" + 
                    "89 7c 24 30",
                ByteOffset = 8
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            return patternOffset + patternPtr + 4;
        }

        public static IntPtr FindPlayerName()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "48 8D 58 08           " + // lea rbx,[rax+08]
                    "75 04                 " + // jne WowT-64.exe+C1070F
                    "48 8B 58 08           " + // mov rbx,[rax+08]
                    "E8 ????????           ", // call WowT-64.exe+C0D9C0
                ByteOffset = 11
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            var namePtr = MemoryManager.Read<int>(patternOffset + 4 + patternPtr + 5, false);
            return patternOffset + patternPtr + 4 + namePtr + 9;
        }

        public static IntPtr FindPlayerBase()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "48 83 EC 40           " + // sub rsp,40 { 64 }
                    "48 8B 3D ????????     " + // mov rdi,[WowT-64.exe+18AD618] { [197D75CEC30] }
                    "48 8B E9              " + // mov rbp,rcx
                    "48 85 FF              " + // test rdi,rdi
                    "75 2E                 " + // jne WowT-64.exe+7CE11D
                    "48 8D 4C 24 30        ", // lea rcx,[rsp+30]

                ByteOffset = 7
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            return patternOffset + 4 + patternPtr;
        }

        public static IntPtr FindGameBuild()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "28 62 75 69 6C 64 20 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ??",
                ByteOffset = 7
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            return patternOffset;
        }

        public static IntPtr FindPlayerWalk()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "48 8B 35 ????????     " + // mov rsi,[WowT-64.exe+18C6E18] { [27A1C1574F0] }
                    "8B DA                 " + // mov ebx,edx
                    "48 8B F9              " + // mov rdi,rcx
                    "48 85 F6              " + // test rsi,rsi
                    "75 ??                 " + // jne WowT-64.exe+56CCF5
                    "48 8D 4C 24 40        " + // lea rcx,[rsp+40]
                    "E8 ????????           " + // call WowT-64.exe+4E5810
                    "4C 8D 05 ????????     " + // lea r8,[WowT-64.exe+113F930] 
                    "8D 56 10              " + // lea edx,[rsi+10]
                    "48 8D 4C 24 40        " + // lea rcx,[rsp+40]
                    "41 B9 DB000000        " + // mov r9d,000000DB { 219 }
                    "E8 ????????           " + // call WowT-64.exe+4E6330
                    "48 8B F0              ", // mov rsi,rax
                
                ByteOffset = 3
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            return patternOffset + 4 + patternPtr;
        }

        public static IntPtr FindPlayerAOE()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "74 12" + 
                    "0f10 44 24 30" + 
                    "0f11 43 08" +
                    "48 8b 44 24 40" +
                    "48 89 43 18" +
                    "48 8B 05 ????????" , //- mov rax,[WowT-64.exe+16E4858] { [1E032B4C330] }
                ByteOffset = 23
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            return patternOffset + 4 + patternPtr;
        }

        public static IntPtr FindPlayerTarget()
        {
            var patternData = new OffsetBytePattern()
            {
                Pattern =
                    "48 8B 05 ????????" + //- mov rax,[Wow-64.exe+16162F8] { [00000000] }
                    "48 89 05 ????????" + //- mov [Wow-64.exe+151A0E8],rax { [000101C8] }
                    "48 8B 05 ????????" + //- mov rax,[Wow-64.exe+1616300] { [00000000] }
                    "48 89 05 ????????" + //- mov [Wow-64.exe+151A0F0],rax { [2052D180] }
                    "85 FF", //- test edi,edi

                ByteOffset = 3
            };

            var patternOffset = FindPattern(patternData);

            if (patternOffset == IntPtr.Zero)
                return IntPtr.Zero;

            var patternPtr = MemoryManager.Read<int>(patternOffset, false);
            return patternOffset + 4 + patternPtr;
        }

        private static IntPtr FindPattern(OffsetBytePattern bytePattern)
        {
            _sigScan.Process = MemoryManager.Process;
            _sigScan.Address = MemoryManager.Process.MainModule.BaseAddress;
            _sigScan.Size = MemoryManager.Process.MainModule.ModuleMemorySize;

            var pointer = _sigScan.FindPattern(bytePattern.Pattern, bytePattern.ByteOffset);
            return pointer;
        }
    }

    public class OffsetBytePattern
    {
        public string Pattern { get; set; }
        public int ByteOffset { get; set; }
    }
}