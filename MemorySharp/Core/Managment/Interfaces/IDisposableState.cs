using System;

namespace Binarysharp.MemoryManagement.Core.Managment.Interfaces
{
    /// <summary>
    ///     Defines an IDisposable interface with a known state.
    /// </summary>
    public interface IDisposableState : IDisposable
    {
        #region Public Properties, Indexers

        /// <summary>
        ///     Gets a value indicating whether the element is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        bool MustBeDisposed { get; }

        #endregion Public Properties, Indexers
    }
}