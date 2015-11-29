using Binarysharp.MemoryManagement.Core.Native.Enums;
using System;

namespace Binarysharp.MemoryManagement.Core.Memory.Objects
{
    /// <summary>
    ///     Class providing tools for manipulating memory protection.
    /// </summary>
    public class MemoryProtection : IDisposable
    {
        #region Fields, Private Properties

        /// <summary>
        ///     The opened process processHandle reference.
        /// </summary>
        private IntPtr Handle { get; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryProtection" /> class.
        /// </summary>
        /// <param name="handle">The reference of the opened processHandle of the process to change memory flags in.</param>
        /// <param name="baseAddress">The base address of the memory to change the protection.</param>
        /// <param name="size">The size of the memory to change.</param>
        /// <param name="protection">The new protection to apply.</param>
        /// <param name="mustBeDisposed">The resource will be automatically disposed when the finalizer collects the object.</param>
        public MemoryProtection(IntPtr handle, IntPtr baseAddress, int size,
                                MemoryProtectionFlags protection = MemoryProtectionFlags.ExecuteReadWrite,
                                bool mustBeDisposed = true)
        {
            // Save the parameters
            Handle = handle;
            BaseAddress = baseAddress;
            NewProtection = protection;
            Size = size;
            OldProtection = ExternalMemoryCore.ChangeProtection(Handle, baseAddress, size, protection);
            MustBeDisposed = mustBeDisposed;
        }

        /// <summary>
        ///     Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MemoryProtection()
        {
            if (MustBeDisposed)
                Dispose();
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     The base address of the altered memory.
        /// </summary>
        public IntPtr BaseAddress { get; }

        /// <summary>
        ///     States if the <see cref="MemoryProtection" /> object nust be disposed when it is collected.
        /// </summary>
        public bool MustBeDisposed { get; set; }

        /// <summary>
        ///     Defines the new protection applied to the memory.
        /// </summary>
        public MemoryProtectionFlags NewProtection { get; }

        /// <summary>
        ///     References the inital protection of the memory.
        /// </summary>
        public MemoryProtectionFlags OldProtection { get; }

        /// <summary>
        ///     The size of the altered memory.
        /// </summary>
        public int Size { get; }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Restores the initial protection of the memory.
        /// </summary>
        public virtual void Dispose()
        {
            // Restore the memory protection
            ExternalMemoryCore.ChangeProtection(Handle, BaseAddress, Size,
                OldProtection);
            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return
                $"BaseAddress = 0x{BaseAddress.ToInt64():X} NewProtection = {NewProtection} OldProtection = {OldProtection}";
        }
    }
}