using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The return values for the function <see cref="SafeNativeMethods.WaitForSingleObject" />.
    /// </summary>
    public enum WaitValues : uint
    {
        /// <summary>
        ///     The specified object is a mutex object that was not released by the thread that owned the mutex object before the
        ///     owning thread terminated.
        ///     Ownership of the mutex object is granted to the calling thread and the mutex state is set to nonsignaled.
        ///     If the mutex was protecting persistent state information, you should check it for consistency.
        /// </summary>
        Abandoned = 0x80,

        /// <summary>
        ///     The state of the specified object is signaled. Similar to WAIT_OBJECT_0.
        /// </summary>
        Signaled = 0x0,

        /// <summary>
        ///     The time-out interval elapsed, and the object's state is nonsignaled.
        /// </summary>
        Timeout = 0x102,

        /// <summary>
        ///     The function has failed. To get extended error information, call <see cref="Marshal.GetLastWin32Error" />.
        /// </summary>
        Failed = 0xFFFFFFFF
    }
}