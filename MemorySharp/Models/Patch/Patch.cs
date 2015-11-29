using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Models.Memory;
using System;
using System.Linq;

namespace Binarysharp.MemoryManagement.Models.Patch
{
    /// <summary>
    ///     Class Patch.
    /// </summary>
    public class Patch : INamedElement
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Patch" /> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="patchWith">The patch with.</param>
        /// <param name="name">The name.</param>
        /// <param name="memory">The memory.</param>
        public Patch(IntPtr address, byte[] patchWith, string name, ProcessMemory memory)
        {
            Name = name;
            ProcessMemory = memory;
            Address = address;
            PatchBytes = patchWith;
            OriginalBytes = ProcessMemory.ReadBytes(address, patchWith.Length);
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets a value indicating whether this memory patch is applied.
        /// </summary>
        /// <value><c>true</c> if this memory patch is applied; otherwise, <c>false</c>.</value>
        public bool IsApplied { get; internal set; }

        /// <summary>
        ///     The reference of the <see cref="ProcessMemory" /> object.
        /// </summary>
        protected ProcessMemory ProcessMemory { get; }

        /// <summary>
        ///     Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public IntPtr Address { get; }

        /// <summary>
        ///     Gets the original bytes.
        /// </summary>
        /// <value>The original bytes.</value>
        public byte[] OriginalBytes { get; }

        /// <summary>
        ///     Gets the patch bytes.
        /// </summary>
        /// <value>The patch bytes.</value>
        public byte[] PatchBytes { get; }

        /// <summary>
        ///     States if the Patch is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled => ProcessMemory.ReadBytes(Address, PatchBytes.Length).SequenceEqual(PatchBytes);

        /// <summary>
        ///     The name of the memory patch.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets a value indicating whether the element is disposed.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        public bool MustBeDisposed { get; set; } = true;

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Disables the memory patch.
        /// </summary>
        public void Disable()
        {
            try
            {
                ProcessMemory.WriteBytes(Address, OriginalBytes);
                IsApplied = false;
            }
            catch
            {
                IsApplied = false;
                // Ignored.
            }
        }

        /// <summary>
        ///     Enables the memory patch.
        /// </summary>
        public void Enable()
        {
            try
            {
                ProcessMemory.WriteBytes(Address, PatchBytes);
                IsApplied = true;
            }
            catch
            {
                IsApplied = false;
                // Ignored
            }
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (IsApplied && MustBeDisposed)
            {
                Disable();
            }
        }

        #endregion Interface Implementations
    }
}