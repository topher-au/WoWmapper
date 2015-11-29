using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Memory.Objects
{
    /// <summary>
    ///     A class for managing process functions.
    /// </summary>
    public class ProcessFunction<T>
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        public ProcessFunction(string name, IntPtr address)
        {
            Name = name;
            Pointer = address;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     The lazy installed fs execute buffer delegate.
        /// </summary>
        private Lazy<T> LazyDelegate => new Lazy<T>((GetDelegate));

        /// <summary>
        ///     The pointer to the function in memory.
        /// </summary>
        public IntPtr Pointer { get; }

        /// <summary>
        ///     Gets the delegate.
        /// </summary>
        /// <value>The delegate.</value>
        public T Delegate => LazyDelegate.Value;

        /// <summary>
        ///     The name representing the function
        /// </summary>
        public string Name { get; }

        #endregion Public Properties, Indexers

        /// <summary>
        ///     Registers a function into a delegate.
        ///     <remarks>The delegate must provide a proper function signature!</remarks>
        /// </summary>
        public T GetDelegate()
        {
            return Marshal.GetDelegateForFunctionPointer<T>(Pointer);
        }
    }
}