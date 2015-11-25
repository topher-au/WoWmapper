using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Contains information about a x64 bit Peb.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Peb64
    {
        /// <summary>
        ///     The being debugged{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public byte BeingDebugged;

        /// <summary>
        ///     The image base{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr ImageBase;

        /// <summary>
        ///     The inherited address space{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public byte InheritedAddressSpace;

        /// <summary>
        ///     The p LDR{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr pLdr;

        /// <summary>
        ///     The p mutant{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr pMutant;

        /// <summary>
        ///     The read image file execute options{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public byte ReadImageFileExecOptions;

        /// <summary>
        ///     The spare bool{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public byte SpareBool;
    }
}