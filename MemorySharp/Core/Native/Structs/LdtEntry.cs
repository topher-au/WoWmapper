using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Describes an entry in the descriptor table. This structure is valid only on x86-based systems.
    /// </summary>
    /// <remarks>This is a simplified version of the original structure.</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct LdtEntry
    {
        /// <summary>
        ///     High bits (24–31) of the base address of the segment.
        /// </summary>
        public byte BaseHi;

        /// <summary>
        ///     The low-order part of the base address of the segment.
        /// </summary>
        public ushort BaseLow;

        /// <summary>
        ///     Middle bits (16–23) of the base address of the segment.
        /// </summary>
        public byte BaseMid;

        /// <summary>
        ///     Values of the Type, Dpl, and Pres members in the Bits structure (not implemented).
        /// </summary>
        public byte Flag1;

        /// <summary>
        ///     Values of the LimitHi, Sys, Reserved_0, Default_Big, and Granularity members in the Bits structure (not
        ///     implemented).
        /// </summary>
        public byte Flag2;

        /// <summary>
        ///     The low-order part of the address of the last byte in the segment.
        /// </summary>
        public ushort LimitLow;
    }
}