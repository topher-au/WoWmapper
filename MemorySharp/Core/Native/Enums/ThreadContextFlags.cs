using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     Determines which registers are returned or set when using <see cref="SafeNativeMethods.GetThreadContext" /> or
    ///     <see cref="SafeNativeMethods.SetThreadContext" />.
    /// </summary>
    [Flags]
    public enum ThreadContextFlags
    {
        /// <summary>
        ///     The Intel 80386 microprocessor, also known as the i386.
        /// </summary>
        Intel386 = 0x10000,

        /// <summary>
        ///     The Intel 80486 microprocessor, also known as the i486.
        /// </summary>
        Intel486 = 0x10000,

        /// <summary>
        ///     SS:SP, CS:IP, FLAGS, BP
        /// </summary>
        Control = Intel386 | 0x01,

        /// <summary>
        ///     AX, BX, CX, DX, SI, DI
        /// </summary>
        Integer = Intel386 | 0x02,

        /// <summary>
        ///     DS, ES, FS, GS
        /// </summary>
        Segments = Intel386 | 0x04,

        /// <summary>
        ///     387 state
        /// </summary>
        FloatingPoint = Intel386 | 0x08,

        /// <summary>
        ///     DB 0-3,6,7
        /// </summary>
        DebugRegisters = Intel386 | 0x10,

        /// <summary>
        ///     CPU specific extensions
        /// </summary>
        ExtendedRegisters = Intel386 | 0x20,

        /// <summary>
        ///     All flags excepted FloatingPoint, DebugRegisters and ExtendedRegisters.
        /// </summary>
        Full = Control | Integer | Segments,

        /// <summary>
        ///     All flags.
        /// </summary>
        All = Control | Integer | Segments | FloatingPoint | DebugRegisters | ExtendedRegisters
    }
}