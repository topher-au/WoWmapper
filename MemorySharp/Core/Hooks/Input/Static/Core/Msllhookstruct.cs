using System;
using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement.Core.Native.Structs;

namespace Binarysharp.MemoryManagement.Core.Hooks.Input.Static.Core
{
    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Msllhookstruct
    {
        /// <summary>
        ///     The point representing the x-y cordinates of the mouse.
        /// </summary>
        public Point Point;

        /// <summary>
        ///     The mouse data <code>int</code> code.
        /// </summary>
        public int MouseData;

        // be careful, this must be ints, not uints (was wrong before I changed it...). regards, cmew.


        /// <summary>
        ///     The flags.
        /// </summary>
        public int Flags;

        /// <summary>
        ///     The time.
        /// </summary>
        public int Time;

        /// <summary>
        ///     The extra information.
        /// </summary>
        public UIntPtr ExtraInfo;
    }
}