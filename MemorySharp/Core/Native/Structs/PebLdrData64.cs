using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Struct containing data about Ldr peb data for 64 bit.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PebLdrData64
    {
        /// <summary>
        ///     The entry in progress{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr EntryInProgress;

        /// <summary>
        ///     The in initialization order module list{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ListEntry InInitializationOrderModuleList;

        /// <summary>
        ///     The initialized{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public byte Initialized;

        /// <summary>
        ///     The in load order module list{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ListEntry InLoadOrderModuleList;

        /// <summary>
        ///     The in memory order module list{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ListEntry InMemoryOrderModuleList;

        /// <summary>
        ///     The length{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public int Length;

        /// <summary>
        ///     The ss processHandle{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr SsHandle;
    }
}