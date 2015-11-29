using Binarysharp.MemoryManagement.Core.Native.Enums;
using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Contains the flash status for a window and the number of times the system should flash the window.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FlashInfo
    {
        /// <summary>
        ///     The count
        /// </summary>
        public uint Count;

        /// <summary>
        ///     The flags
        /// </summary>
        public FlashWindowFlags Flags;

        /// <summary>
        ///     The HWND
        /// </summary>
        public IntPtr Hwnd;

        /// <summary>
        ///     The size
        /// </summary>
        public int Size;

        /// <summary>
        ///     The timeout
        /// </summary>
        public int Timeout;
    }
}