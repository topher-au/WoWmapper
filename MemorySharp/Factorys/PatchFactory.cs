using System;
using Binarysharp.MemoryManagement.Core.Logging.Default;
using Binarysharp.MemoryManagement.Core.Managment;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Models.Memory;
using Binarysharp.MemoryManagement.Models.Patch;

namespace Binarysharp.MemoryManagement.Factorys
{
    /// <summary>
    ///     A manager class to handle memory patches.
    ///     <remarks>All credits to Apoc.</remarks>
    /// </summary>
    public class PatchFactory : SafeManager<Patch>, IFactory
    {
        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="PatchFactory" /> class.
        /// </summary>
        /// <param name="processMemory">The process memory.</param>
        public PatchFactory(ProcessMemory processMemory) : base(new DebugLog())
        {
            ProcessMemory = processMemory;
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     The reference of the <see cref="ProcessMemory" /> object.
        ///     <remarks>This value is invalid if the manager was created for the <see cref="MemorySharp" /> class.</remarks>
        /// </summary>
        protected ProcessMemory ProcessMemory { get; }

        /// <summary>
        ///     Gets a patch instance from the given name from the managers dictonary of current patches.
        /// </summary>
        /// <param name="name">The name of the patch.</param>
        public Models.Patch.Patch this[string name] => InternalItems[name];
        #endregion

        #region Interface Implementations
        public void Dispose()
        {
            foreach (var keyValuePair in InternalItems)
            {
                if (keyValuePair.Value.MustBeDisposed)
                {
                    keyValuePair.Value.Dispose();
                }

                Remove(keyValuePair.Key);
            }
        }
        #endregion

        /// <summary>
        ///     Creates a new <see cref="Patch" /> at the specified address.
        /// </summary>
        /// <param name="address">The address to begin the patch.</param>
        /// <param name="patchWith">The bytes to be written as the patch.</param>
        /// <param name="name">The name of the patch.</param>
        /// <returns>A patch object that exposes the required methods to apply and remove the patch.</returns>
        public Models.Patch.Patch Create(IntPtr address, byte[] patchWith, string name)
        {
            if (InternalItems.ContainsKey(name)) return InternalItems[name];
            InternalItems.Add(name, new Models.Patch.Patch(address, patchWith, name, ProcessMemory));
            return InternalItems[name];
        }

        /// <summary>
        ///     Creates a new <see cref="Patch" /> at the specified address, and applies it.
        /// </summary>
        /// <param name="address">The address to begin the patch.</param>
        /// <param name="patchWith">The bytes to be written as the patch.</param>
        /// <param name="name">The name of the patch.</param>
        /// <returns>A patch object that exposes the required methods to apply and remove the patch.</returns>
        public Models.Patch.Patch CreateAndApply(IntPtr address, byte[] patchWith, string name)
        {
            Create(address, patchWith, name);
            InternalItems[name].Enable();
            return InternalItems[name];
        }
    }
}