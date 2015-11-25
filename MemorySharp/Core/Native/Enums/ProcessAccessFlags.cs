using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     ProcessUpdateData access rights list.
    /// </summary>
    [Flags]
    public enum ProcessAccessFlags
    {
        /// <summary>
        ///     All possible access rights for a process object.
        /// </summary>
        AllAccess = 0x001F0FFF,

        /// <summary>
        ///     Required to create a process.
        /// </summary>
        CreateProcess = 0x0080,

        /// <summary>
        ///     Required to create a thread.
        /// </summary>
        CreateThread = 0x0002,

        /// <summary>
        ///     Required to duplicate a processHandle using DuplicateHandle.
        /// </summary>
        DupHandle = 0x0040,

        /// <summary>
        ///     Required to retrieve certain information about a process, such as its token, exit code, and priority class (see
        ///     OpenProcessToken).
        /// </summary>
        QueryInformation = 0x0400,

        /// <summary>
        ///     Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob,
        ///     QueryFullProcessImageName).
        ///     A processHandle that has the PROCESS_QUERY_INFORMATION access right is automatically granted
        ///     PROCESS_QUERY_LIMITED_INFORMATION.
        /// </summary>
        QueryLimitedInformation = 0x1000,

        /// <summary>
        ///     Required to set certain information about a process, such as its priority class (see SetPriorityClass).
        /// </summary>
        SetInformation = 0x0200,

        /// <summary>
        ///     Required to set memory limits using SetProcessWorkingSetSize.
        /// </summary>
        SetQuota = 0x0100,

        /// <summary>
        ///     Required to suspend or resume a process.
        /// </summary>
        SuspendResume = 0x0800,

        /// <summary>
        ///     Required to terminate a process using TerminateProcess.
        /// </summary>
        Terminate = 0x0001,

        /// <summary>
        ///     Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
        /// </summary>
        VmOperation = 0x0008,

        /// <summary>
        ///     Required to read memory in a process using <see cref="SafeNativeMethods.ReadProcessMemory" />.
        /// </summary>
        VmRead = 0x0010,

        /// <summary>
        ///     Required to write to memory in a process using WriteProcessMemory.
        /// </summary>
        VmWrite = 0x0020,

        /// <summary>
        ///     Required to wait for the process to terminate using the wait functions.
        /// </summary>
        Synchronize = 0x00100000
    }
}