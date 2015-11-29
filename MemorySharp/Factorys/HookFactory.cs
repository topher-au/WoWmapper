using Binarysharp.MemoryManagement.Core.Hooks.Input;
using Binarysharp.MemoryManagement.Core.Hooks.WndProc;
using Binarysharp.MemoryManagement.Core.Hooks.WndProc.Enums;
using Binarysharp.MemoryManagement.Core.Hooks.WndProc.Interfaces;
using Binarysharp.MemoryManagement.Core.Logging.Default;
using Binarysharp.MemoryManagement.Core.Managment;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using System;
using System.Linq;

namespace Binarysharp.MemoryManagement.Factorys
{
    /// <summary>
    ///     Class to manage hooks that implement the <see cref="INamedElement" /> Interface.
    /// </summary>
    public class HookFactory : SafeManager<INamedElement>, IFactory
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HookFactory" /> class.
        /// </summary>
        /// <param name="processMemory">The process memory.</param>
        public HookFactory(MemoryPlus processMemory) : base(new DebugLog())
        {
            ProcessMemory = processMemory;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     The reference of the <see cref="ProcessMemory" /> object.
        /// </summary>
        protected MemoryPlus ProcessMemory { get; }

        /// <summary>
        ///     Gets a hook instance from the given name from the managers dictonary of current hooks.
        /// </summary>
        /// <param name="name">The name of the hook.</param>
        public INamedElement this[string name] => InternalItems[name];

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var hookValue in InternalItems.Values.Where(hook => hook.IsEnabled))
            {
                hookValue.Disable();
            }
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Creates the WND proc hook.
        /// </summary>
        /// <param name="name">The name representingthe hook Instance.</param>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="engine">
        ///     ///     The instance of the <see cref="IWindowEngine" /> to use. The <code>StartUp()</code> and
        ///     <code>ShutDown</code> methods inside the defined interface given will be called when the corresponding
        ///     <see cref="UserMessage" /> is invoked and sent.
        /// </param>
        /// <returns>WindowHook.</returns>
        public WindowHook CreateWndProcHook(string name, IntPtr windowHandle, ref IWindowEngine engine)
        {
            InternalItems[name] = new WindowHook(windowHandle, name, ref engine);
            return (WindowHook)InternalItems[name];
        }

        /// <summary>
        ///     Creates an instance of a low level mouse hook. This instance is also added to the manager and returned as the
        ///     result.
        /// </summary>
        /// <param name="name">The name of the low level mouse hook.</param>
        /// <param name="mustBeDisposed">
        ///     if set to <c>true</c> the hook must be disposed], else it will not be disposed on garbage
        ///     collection.
        /// </param>
        /// <returns>MouseHook.</returns>
        public MouseHook CreateKMouseHook(string name, bool mustBeDisposed = true)
        {
            InternalItems[name] = new MouseHook(name, mustBeDisposed);
            return (MouseHook)InternalItems[name];
        }

        /// <summary>
        ///     Creates an instance of a low level keyboard hook. This instance is also added to the manager and returned as the
        ///     result.
        /// </summary>
        /// <param name="name">The name of the low level keyboard hook.</param>
        /// <param name="mustBeDisposed">
        ///     if set to <c>true</c> the hook must be disposed], else it will not be disposed on garbage
        ///     collection.
        /// </param>
        /// <returns>MouseHook.</returns>
        public KeyboardHook CreateKeyboardHook(string name, bool mustBeDisposed = true)
        {
            InternalItems[name] = new KeyboardHook(name, mustBeDisposed);
            return (KeyboardHook)InternalItems[name];
        }
    }
}