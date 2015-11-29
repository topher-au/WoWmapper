using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Binarysharp.MemoryManagement.Core.Hooks.WndProc.Engines
{
    /// <summary>
    ///     class for running <see cref="Action" /> methods inside the main thread of the process the <see cref="WindowHook" />
    ///     is attached to.
    /// </summary>
    public class WindowActionQueue : IPulsableElement
    {
        #region Fields, Private Properties

        private readonly Queue<Action> _executionQueue;

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowActionQueue"/> class.
        /// </summary>
        public WindowActionQueue()
        {
            _executionQueue = new Queue<Action>();
        }

        #endregion Constructors, Destructors

        #region Interface Implementations

        public void Pulse()
        {
            if (_executionQueue == null)
                return;

            if (_executionQueue.Count == 0)
                return;

            var action = _executionQueue.Dequeue();
            action.Invoke();
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Adds the action to an <see cref="Queue{Action}" /> queue, which is used execute code in the main thread of the
        ///     process the <see cref="WindowHook" /> is attached to.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="postpone">
        ///     <para>
        ///         [Optional] If set to <c>true</c> [postpone the <see cref="Action" /> even if it can be invoked
        ///         right away], else the <see cref="Action" /> will be invoked right away if possible, otherwise it will be added
        ///         to
        ///         the action queue to be invoked later..
        ///     </para>
        /// </param>
        public void QueueAction(Action action, bool postpone = false)
        {
            // If we're already in the main thread we're also in the WndProc hook which means we can run the command without any problems
            // sometimes this is however not desired (maybe this needs to happen the next frame) so we check if we want it postponed.
            if (!postpone && Thread.CurrentThread.ManagedThreadId == Process.GetCurrentProcess().Threads[0].Id)
            {
                action.Invoke();
            }
            else
            {
                _executionQueue.Enqueue(action);
            }
        }
    }
}