using System;
using System.Linq;
using Binarysharp.MemoryManagement.Core.Managment.BaseClasses;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Factorys;

namespace Binarysharp.MemoryManagement.Managers
{
    /// <summary>
    ///     Class for managing of <see cref="IFactory" /> instances.
    /// </summary>
    public class InternalFactoryManager : BaseManager<IFactory>, IDisposable
    {
        #region Fields, Private Properties
        /// <summary>
        ///     Gets the memory sharp reference for this instance.
        /// </summary>
        /// <value>The memory sharp reference for this instance.</value>
        private MemoryPlus MemorySharp { get; }
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExternalFactoryManager" /> class.
        /// </summary>
        /// <param name="memorySharp">The memory sharp instance to use for reference in the managers instance.</param>
        public InternalFactoryManager(MemoryPlus memorySharp)
        {
            MemorySharp = memorySharp;
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     A manager for Instances of the <see cref="Factorys.DetourFactory" /> class.
        /// </summary>
        /// <value>The Instance of <see cref="Factorys.DetourFactory" />.</value>
        public DetourFactory DetourFactory { get; private set; }

        /// <summary>
        ///     A manager for hooks that implement the <see cref="INamedElement" /> Interface.
        /// </summary>
        /// <value>The Instance of <see cref="Factorys.HookFactory" />.</value>
        public HookFactory HookFactory { get; private set; }

        /// <summary>
        ///     A factory for patches that implement the <see cref="INamedElement" /> Interface.
        /// </summary>
        /// <value>The Instance of <see cref="Factorys.HookFactory" />.</value>
        public PatchFactory PatchFactory { get; private set; }
        #endregion

        #region Interface Implementations
        public void Dispose()
        {
            RemoveAll();
        }
        #endregion

        /// <summary>
        ///     Disables all items in the manager by removing from the managers items and then disposing them.
        /// </summary>
        public override void DisableAll()
        {
            RemoveAll();
        }

        /// <summary>
        ///     Enables all factories in the manager by creating their object instaces.
        /// </summary>
        public override void EnableAll()
        {
            InternalItems.Add("DetourFactory", DetourFactory = new DetourFactory(MemorySharp));
            InternalItems.Add("HookFactory", HookFactory = new HookFactory(MemorySharp));
            InternalItems.Add("PatchFactory", PatchFactory = new PatchFactory(MemorySharp));
        }

        /// <summary>
        ///     Removes a factory by its name in the manager and then disposes it.
        /// </summary>
        /// <param name="name">The name of the factory key to remove.</param>
        public override void Remove(string name)
        {
            var factory = InternalItems[name];
            InternalItems.Remove(name);
            factory.Dispose();
        }

        /// <summary>
        ///     Removes a factory by its instance reference in the manager.
        /// </summary>
        /// <param name="item">The <see cref="IFactory" /> instance to remove and dispose.</param>
        public override void Remove(IFactory item)
        {
            var keyValuePair = InternalItems.First(factory => factory.Value.Equals(item));
            InternalItems.Remove(keyValuePair.Key);
            keyValuePair.Value.Dispose();
        }

        /// <summary>
        ///     Removes all factories in the manager by removing from the managers items and then disposing them.
        /// </summary>
        public override void RemoveAll()
        {
            foreach (var value in InternalItems)
            {
                Action action = (() => value.Value.Dispose());
                Remove(value.Value);
                action();
            }
        }
    }
}