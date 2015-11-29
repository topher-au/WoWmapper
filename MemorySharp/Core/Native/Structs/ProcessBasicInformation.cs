using Binarysharp.MemoryManagement.Core.Marshaling;
using System;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Structure containing basic information about a process.
    /// </summary>
    public struct ProcessBasicInformation
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
        ///     The exit status.
        /// </summary>
        public IntPtr ExitStatus;

        /// <summary>
        ///     The process id of the parent process.
        /// </summary>
        public int InheritedFromUniqueProcessId;

        /// <summary>
        ///     The base address of ProcessUpdateData Environment Block.
        /// </summary>
        public IntPtr PebBaseAddress;

        /// <summary>
        ///     The process id.
        /// </summary>
        public int ProcessId;

        /// <summary>
        ///     The size of this structure.
        /// </summary>
        public int Size => SafeMarshal<ProcessBasicInformation>.Size;
    }
}