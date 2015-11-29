using System;

namespace Binarysharp.MemoryManagement.Models.Threads
{
    /// <summary>
    ///     Class containing a frozen thread. If an instance of this class is disposed, its associated thread is resumed.
    /// </summary>
    public class FrozenThread : IDisposable
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FrozenThread" /> class.
        /// </summary>
        /// <param name="thread">The frozen thread.</param>
        internal FrozenThread(RemoteThread thread)
        {
            // Save the parameter
            Thread = thread;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     The frozen thread.
        /// </summary>
        public RemoteThread Thread { get; }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Releases all resources used by the <see cref="RemoteThread" /> object.
        /// </summary>
        public virtual void Dispose()
        {
            // Unfreeze the thread
            Thread.Resume();
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Id = {Thread.Id}";
        }
    }
}