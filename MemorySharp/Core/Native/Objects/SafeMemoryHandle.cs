using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Binarysharp.MemoryManagement.Core.Native.Objects
{
    /// <summary>
    ///     Represents a Win32 processHandle safely managed.
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class SafeMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Constructors, Destructors
        /// <summary>
        ///     Parameterless constructor for handles built by the system (like <see cref="SafeNativeMethods.OpenProcess" />).
        /// </summary>
        public SafeMemoryHandle() : base(true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SafeMemoryHandle" /> class, specifying the processHandle to keep in
        ///     safe.
        /// </summary>
        /// <param name="handle">The processHandle to keep in safe.</param>
        public SafeMemoryHandle(IntPtr handle) : base(true)
        {
            SetHandle(handle);
        }
        #endregion

        /// <summary>
        ///     Executes the code required to free the processHandle.
        /// </summary>
        /// <returns>
        ///     True if the processHandle is released successfully; otherwise, in the event of a catastrophic failure, false. In
        ///     this
        ///     case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            // Check whether the processHandle is set AND whether the processHandle has been successfully closed
            return handle != IntPtr.Zero && SafeNativeMethods.CloseHandle(handle);
        }
    }
}