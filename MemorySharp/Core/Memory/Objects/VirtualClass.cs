using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Memory.Objects
{
    /// <summary>
    ///     A virtual class object with various methods/values related to virtual classes.
    /// </summary>
    public class VirtualClass
    {
        #region Constructors, Destructors
        //private IMemory mem;
        /// <summary>
        ///     Initializes a new instance of the <see cref="VirtualClass" /> class.
        /// </summary>
        /// <param name="address">The address the class is located at in memory.</param>
        public VirtualClass(IntPtr address)
        {
            Address = address;
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     The address the class is located at in memory.
        /// </summary>
        public IntPtr Address { get; }
        #endregion

        /// <summary>
        ///     Gets a virtual function delegate at the given index.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="functionIndex">The index the function is located at in memory from the base address.</param>
        /// <returns>A delegate.</returns>
        public T GetVirtualFunction<T>(int functionIndex) where T : class
        {
            return Marshal.GetDelegateForFunctionPointer<T>(GetObjectVtableFunctionPointer(Address, functionIndex));
        }

        /// <summary>
        ///     Gets a virtual function pointer.
        /// </summary>
        /// <param name="objectBase">The base address.</param>
        /// <param name="functionIndex">The index the function is located at in memory from the base address.</param>
        /// <returns>A <code>IntPtr</code>.</returns>
        public IntPtr GetObjectVtableFunctionPointer(IntPtr objectBase, int functionIndex)
        {
            return GetVTableEntry(objectBase, functionIndex);
        }

        /// <summary>
        ///     Gets a virtual function pointer.
        /// </summary>
        /// <param name="addr">The base address.</param>
        /// <param name="index">The index the function is located at in memory from the base address.</param>
        /// <returns>A <code>IntPtr</code>.</returns>
        private static unsafe IntPtr GetVTableEntry(IntPtr addr, int index)
        {
            var pAddr = (void***) addr.ToPointer();
            return new IntPtr((*pAddr)[index]);
        }
    }
}