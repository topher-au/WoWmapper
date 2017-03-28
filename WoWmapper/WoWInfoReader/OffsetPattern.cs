using System.Collections.Generic;

namespace WoWmapper.WoWInfoReader
{
    public class OffsetPattern
    {
        public static OffsetPattern GameState = new OffsetPattern
        {
            Pattern =
                    "80 3D ???????? 00" + // cmp byte ptr [WowT-64.exe+1A36E51],00 { [0] }
                    "74 ??" + // je WowT-64.exe+C08775
                    "84 DB" + // test bl,bl
                    "88 0D ????????" , // mov [WowT-64.exe+1A36E52],cl { [0] }
            Offset = 2
        };

        public static OffsetPattern PlayerBase = new OffsetPattern
        {
            Pattern =
                "48 83 EC ??           " + // sub rsp,40 { 64 }
                "48 8B 3D ????????     " + // mov rdi,[WowT-64.exe+18AD618] { [197D75CEC30] }
                "48 ????              " + // mov rbp,rcx
                "48 ????              " + // test rdi,rdi
                "75 ??                 " + // jne WowT-64.exe+7CE11D
                "48 8D 4C 24 ??        ", // lea rcx,[rsp+30]
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
                "0F10 04 C8"+//            - movups xmm0,[rax+rcx*8]
                "48 8B 0D F0B8ED00"+//     - mov rcx,[Wow-64.exe+14B0010] { [253FA5EE050] }
                "49 6B C5 34"+//           - imul rax,r13,34
                "0F11 45 48",//            - movups [rbp+48],xmm0
            Offset = 7
        };

        public string Pattern { get; set; }
        public int Offset { get; set; }
    }
}