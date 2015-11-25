using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Structure containing basic information about a thread.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadBasicInformation
    {
        /// <summary>
        ///     The affinity mask.
        /// </summary>
        public uint AffinityMask;

        /// <summary>
        ///     The base priority.
        /// </summary>
        public uint BasePriority;

        /// <summary>
        ///     the exit status.
        /// </summary>
        public uint ExitStatus;

        /// <summary>
        ///     The priority.
        /// </summary>
        public uint Priority;

        /// <summary>
        ///     The process id which owns the thread.
        /// </summary>
        public int ProcessId;

        /// <summary>
        ///     The base address of Thread Environment Block.
        /// </summary>
        public IntPtr TebBaseAdress;

        /// <summary>
        ///     The thread id.
        /// </summary>
        public int ThreadId;
    }
}