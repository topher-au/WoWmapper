using Binarysharp.MemoryManagement.Core.Hooks.WndProc.Enums;
using Binarysharp.MemoryManagement.Core.Hooks.WndProc.Interfaces;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using System;
using System.Collections.Generic;

namespace Binarysharp.MemoryManagement.Core.Hooks.WndProc.Engines
{
    /// <summary>
    ///     Default Class for adding elements to be executed inside of the thread the window <see cref="WindowHook" /> class is
    ///     attached to.
    /// </summary>
    public class WindowHookPulseEngine : IWindowEngine
    {
        #region Public Delegates/Events

        /// <summary>
        ///     Occurs when the first line of code in the engines <code><see cref="IWindowEngine.StartUp()" /></code> is called.
        /// </summary>
        public event EventHandler<UpdatePulseArgs> StartOfPulse;

        /// <summary>
        ///     Occurs when the last line of code in the engines <code><see cref="IWindowEngine.StartUp()" /></code> is called.
        /// </summary>
        public event EventHandler<UpdatePulseArgs> EndOfPulse;

        #endregion Public Delegates/Events

        #region Fields, Private Properties

        /// <summary>
        ///     Gets the linked list of <code>IPulsableElement</code>'s.
        /// </summary>
        /// <value>
        ///     The linked list of IPulsableElement's.
        /// </value>
        private LinkedList<IPulsableElement> Pulsables { get; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowHookPulseEngine" /> class.
        /// </summary>
        public WindowHookPulseEngine()
        {
            Pulsables = new LinkedList<IPulsableElement>();
        }

        #endregion Constructors, Destructors

        #region Interface Implementations

        /// <summary>
        ///     Shuts the engine down.
        /// </summary>
        void IWindowEngine.ShutDown()
        {
            Pulsables.Clear();
        }

        /// <summary>
        ///     Starts the engine up.
        /// </summary>
        void IWindowEngine.StartUp()
        {
            StartOfPulse?.Invoke(this, new UpdatePulseArgs());
            if (Pulsables == null)
            {
                return;
            }

            if (Pulsables.Count == 0)
            {
                return;
            }

            foreach (var pulsable in Pulsables)
            {
                pulsable.Pulse();
            }
            EndOfPulse?.Invoke(this, new UpdatePulseArgs());
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Adds a <code>IPulseableElement</code> member to the linked list. All elements in the list will have their
        ///     <code>Pulse()</code> method called when the <see cref="UserMessage.StartUp" /> is message is invoked.
        /// </summary>
        /// <param name="windowEngine">The window engine.</param>
        public void RegisterCallback(IPulsableElement windowEngine)
        {
            Pulsables.AddLast(windowEngine);
        }

        /// <summary>
        ///     Adds multiple <code>IPulseableElement</code> member to the linked list. All elements in the list will have their
        ///     <code>Pulse()</code> method called when the <see cref="UserMessage.StartUp" /> is message is invoked.
        /// </summary>
        /// <param name="pulsableElements">The window engine.</param>
        public void RegisterCallbacks(params IPulsableElement[] pulsableElements)
        {
            foreach (var pulsable in pulsableElements)
            {
                RegisterCallback(pulsable);
            }
        }

        /// <summary>
        ///     Removes an element from the <code>IPulseableElement</code> linked list contained in this instance.
        /// </summary>
        /// <param name="windowEngine">The window engine.</param>
        public void RemoveCallback(IPulsableElement windowEngine)
        {
            if (Pulsables.Contains(windowEngine))
            {
                Pulsables.Remove(windowEngine);
            }
        }

        // private static Lazy<Updater> LazyUpdater => new Lazy<Updater>((() => new Updater("Updater", 1000)));
    }
}