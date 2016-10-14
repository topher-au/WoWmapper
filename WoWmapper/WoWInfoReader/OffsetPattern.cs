using System.Collections.Generic;

namespace WoWmapper.WoWInfoReader
{
    public class OffsetPattern
    {
        public static OffsetPattern GameState = new OffsetPattern
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
            Offset = 26
        };

        public static OffsetPattern PlayerBase = new OffsetPattern
        {
            Pattern =
                "48 83 EC 40           " + // sub rsp,40 { 64 }
                "48 8B 3D ????????     " + // mov rdi,[WowT-64.exe+18AD618] { [197D75CEC30] }
                "48 8B E9              " + // mov rbp,rcx
                "48 85 FF              " + // test rdi,rdi
                "75 2E                 " + // jne WowT-64.exe+7CE11D
                "48 8D 4C 24 30        ", // lea rcx,[rsp+30]
            Offset = 7
        };

        public static OffsetPattern MouselookState = new OffsetPattern
        {
            Pattern =
                    "83 F8 01" + //              - cmp eax,01 { 1 }
                    "75 0F" + //                 - jne Wow-64.exe+1FCF62
                    "C7 05 ???????? 00000000" + // - mov [Wow-64.exe+14CF440],00000000 { [00000001] }
                    "E8 ????????" + //           - call Wow-64.exe+1FDCB0
                    "E8 ????????" + //           - call Wow-64.exe+1FA8A0
                    "89 05 ????????" + //        - mov [Wow-64.exe+14CF434],eax { [00DB4288] }
                    "48 83 C4 ??", //           - add rsp,28 { 40 }
            Offset = 7
        };

        public static OffsetPattern AoeState = new OffsetPattern
        {
            Pattern =
                "48 8B 05 ????????" + //     - mov rax,[WowB-64.exe+16F0D78] { [1E49AE0B100] }
                "80 B8 ??0C0000 00", //     - cmp byte ptr [rax+00000C60],00 { 0 }
            Offset = 3
        };

        public static OffsetPattern WalkState = new OffsetPattern
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
            Offset = 3
        };

        public string Pattern { get; set; }
        public int Offset { get; set; }
    }
}