using Binarysharp.MemoryManagement.Core.Native.Enums;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Returned if <see cref="ThreadContextFlags.FloatingPoint" /> flag is set.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FloatingSaveArea
    {
        /// <summary>
        ///     The control word{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint ControlWord;

        /// <summary>
        ///     The CR0 NPX state{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint Cr0NpxState;

        /// <summary>
        ///     The data offset{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint DataOffset;

        /// <summary>
        ///     The data selector{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint DataSelector;

        /// <summary>
        ///     The error offset{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint ErrorOffset;

        /// <summary>
        ///     The error selector{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint ErrorSelector;

        /// <summary>
        ///     The register area{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] RegisterArea;

        /// <summary>
        ///     The status word{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint StatusWord;

        /// <summary>
        ///     The tag word{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public uint TagWord;
    }
}