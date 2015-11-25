using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Struct for a basic list entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ListEntry
    {
        /// <summary>
        ///     The b link{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr BLink;

        /// <summary>
        ///     The f link{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr FLink;
    }
}