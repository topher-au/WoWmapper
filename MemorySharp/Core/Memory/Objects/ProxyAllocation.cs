using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using System;

namespace Binarysharp.MemoryManagement.Core.Memory.Objects
{
    /// <summary>
    ///     Class representing an allocated memory in a remote process.
    /// </summary>
    public class ProxyAllocation : ProxyRegion, IDisposableState
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProxyAllocation" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        /// <param name="size">The size of the allocated memory.</param>
        /// <param name="protection">The protection of the allocated memory.</param>
        /// <param name="mustBeDisposed">The allocated memory will be released when the finalizer collects the object.</param>
        public ProxyAllocation(IntPtr processHandle, int size,
                               MemoryProtectionFlags protection = MemoryProtectionFlags.ExecuteReadWrite,
                               bool mustBeDisposed = true)
            : base(
                processHandle,
                ExternalMemoryCore.Allocate(processHandle, size, protection)
                )
        {
            // Set local vars
            MustBeDisposed = mustBeDisposed;
            IsDisposed = false;
        }

        /// <summary>
        ///     Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~ProxyAllocation()
        {
            if (MustBeDisposed)
                Dispose();
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets a value indicating whether the element is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        public bool MustBeDisposed { get; set; }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Releases all resources used by the <see cref="ProxyAllocation" /> object.
        /// </summary>
        /// <remarks>Don't use the IDisposable pattern because the class is sealed.</remarks>
        public virtual void Dispose()
        {
            if (!IsDisposed)
            {
                // Set the flag to true
                IsDisposed = true;
                // Release the allocated memory
                Release();
                // Remove this object from the collection of allocated memory
                ExternalMemoryCore.Free(ProcessHandle, BaseAddress);
                // Remove the pointer
                BaseAddress = IntPtr.Zero;
                // Avoid the finalizer
                GC.SuppressFinalize(this);
            }
        }

        #endregion Interface Implementations
    }
}