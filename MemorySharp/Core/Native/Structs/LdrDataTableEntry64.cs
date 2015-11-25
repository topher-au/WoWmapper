using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Struct LdrDataTableEntry64 containins data about an entry from the Ldr structure table.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LdrDataTableEntry64
    {
        /// <summary>
        ///     The base DLL name{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public UnicodeString BaseDllName;

        /// <summary>
        ///     The check sum{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint CheckSum;

        /// <summary>
        ///     The DLL base{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr DllBase;

        /// <summary>
        ///     The entry point{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr EntryPoint;

        /// <summary>
        ///     The entry point activation context{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr EntryPointActivationContext;

        /// <summary>
        ///     The flags{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint Flags;

        /// <summary>
        ///     The full DLL name{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public UnicodeString FullDllName;

        /// <summary>
        ///     The in initialization order module list{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ListEntry InInitializationOrderModuleList;

        /// <summary>
        ///     The in load order module list{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ListEntry InLoadOrderModuleList;

        /// <summary>
        ///     The in memory order module list{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ListEntry InMemoryOrderModuleList;

        /// <summary>
        ///     The load count{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public short LoadCount;

        /// <summary>
        ///     The loaded imports{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr LoadedImports;

        /// <summary>
        ///     The patch information{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr PatchInformation;

        /// <summary>
        ///     The section pointer{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr SectionPointer;

        /// <summary>
        ///     The size of image{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public int SizeOfImage;

        /// <summary>
        ///     The TLS index{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public short TlsIndex;
    }
}