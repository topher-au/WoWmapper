using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     Thread access rights list.
    /// </summary>
    [Flags]
    public enum ThreadAccessFlags
    {
        /// <summary>
        ///     Enables the use of the thread processHandle in any of the wait functions.
        /// </summary>
        Synchronize = 0x00100000,

        /// <summary>
        ///     All possible access rights for a thread object.
        /// </summary>
        AllAccess = 0x001F0FFF,

        /// <summary>
        ///     Required for a server thread that impersonates a client.
        /// </summary>
        DirectImpersonation = 0x0200,

        /// <summary>
        ///     Required to read the context of a thread using <see cref="SafeNativeMethods.GetThreadContext" />.
        /// </summary>
        GetContext = 0x0008,

        /// <summary>
        ///     Required to use a thread's security information directly without calling it by using a communication mechanism that
        ///     provides impersonation services.
        /// </summary>
        Impersonate = 0x0100,

        /// <summary>
        ///     Required to read certain information from the thread object, such as the exit code (see GetExitCodeThread).
        /// </summary>
        QueryInformation = 0x0040,

        /// <summary>
        ///     Required to read certain information from the thread objects (see <see cref="SafeNativeMethods.GetThreadContext" />
        ///     ).
        ///     A processHandle that has the THREAD_QUERY_INFORMATION access right is automatically granted
        ///     THREAD_QUERY_LIMITED_INFORMATION.
        /// </summary>
        QueryLimitedInformation = 0x0800,

        /// <summary>
        ///     Required to write the context of a thread using <see cref="SafeNativeMethods.SetThreadContext" />.
        /// </summary>
        SetContext = 0x0010,

        /// <summary>
        ///     Required to set certain information in the thread object.
        /// </summary>
        SetInformation = 0x0020,

        /// <summary>
        ///     Required to set certain information in the thread object. A processHandle that has the THREAD_SET_INFORMATION
        ///     access right
        ///     is automatically granted THREAD_SET_LIMITED_INFORMATION.
        /// </summary>
        SetLimitedInformation = 0x0400,

        /// <summary>
        ///     Required to set the impersonation token for a thread using SetThreadToken.
        /// </summary>
        SetThreadToken = 0x0080,

        /// <summary>
        ///     Required to suspend or resume a thread (see <see cref="SafeNativeMethods.SuspendThread" /> and
        ///     <see cref="SafeNativeMethods.ResumeThread" />).
        /// </summary>
        SuspendResume = 0x0002,

        /// <summary>
        ///     Required to terminate a thread using <see cref="SafeNativeMethods.TerminateThread" />.
        /// </summary>
        Terminate = 0x0001
    }
}