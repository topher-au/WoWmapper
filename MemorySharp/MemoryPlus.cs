using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement.Core.Memory;
using Binarysharp.MemoryManagement.Core.Memory.Objects;
using Binarysharp.MemoryManagement.Managers;
using Binarysharp.MemoryManagement.Models.Memory;
using Binarysharp.MemoryManagement.Models.Modules;

namespace Binarysharp.MemoryManagement
{
    /// <summary>
    ///     A class providing tools to manage a local processes memory.
    /// </summary>
    public sealed class MemoryPlus : ProcessMemory, IEquatable<MemoryPlus>
    {
        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessMemory" /> class.
        /// </summary>
        /// <param name="process">The process.</param>
        public MemoryPlus(Process process) : base(process)
        {
            Factories = new InternalFactoryManager(this);
            Factories.EnableAll();
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     Gets the factory manager instance.
        /// </summary>
        /// <value>The instance for managing factories.</value>
        public InternalFactoryManager Factories { get; }


        /// <summary>
        ///     Gets or sets the <see cref="PatternScanner" /> with the specified module name.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>ProcessModulePatternScanner.</returns>
        public PatternScanner this[string moduleName] => CreatePatternScanner(GetModule(moduleName).ModuleName);

        /// <summary>
        ///     Gets the <see cref="ProxyPointer" /> with the specified address.
        /// </summary>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>Binarysharp.MemoryManagement.LocalProcess.Objects.ProxyPointer.</returns>
        public ProxyPointer this[IntPtr address, bool isRelative = false] => CreateProxyPointer(address, isRelative);
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(MemoryPlus other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Handle.Equals(other.Handle);
        }
        #endregion

        /// <summary>
        ///     Reads the specified amount of bytes from the specified address.
        /// </summary>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="count">The count.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns>An array of bytes.</returns>
        public override byte[] ReadBytes(IntPtr address, int count, bool isRelative = false)
        {
            return InternalMemoryCore.ReadBytes(address, count, isRelative);
        }

        /// <summary>
        ///     Reads the value of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A value.</returns>
        public override T Read<T>(IntPtr address, bool isRelative = false)
        {
            return InternalMemoryCore.Read<T>(address, isRelative);
        }

        /// <summary>
        ///     Reads an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public override T[] ReadArray<T>(IntPtr address, int count, bool isRelative = false)
        {
            return InternalMemoryCore.ReadArray<T>(address, count, isRelative);
        }

        /// <summary>
        ///     Writes the specified bytes at the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="byteArray">The bytes.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        public override void WriteBytes(IntPtr address, byte[] byteArray, bool isRelative = false)
        {
            InternalMemoryCore.WriteBytes(address, byteArray, isRelative);
        }

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is written.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public override void Write<T>(IntPtr address, T value, bool isRelative = false)
        {
            InternalMemoryCore.Write(address, value);
        }

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is written.</param>
        /// <param name="arrayOfValues">The array of values to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public override void WriteArray<T>(IntPtr address, T[] arrayOfValues, bool isRelative = false)
        {
            InternalMemoryCore.WriteArray(address, arrayOfValues);
        }

        /// <summary>
        ///     Gets the funtion pointer from a delegate.
        /// </summary>
        /// <param name="delegate">The delegate to extract the pointer from</param>
        /// <returns></returns>
        /// <remarks>Created 2012-01-16 20:40 by Nesox.</remarks>
        public IntPtr GetDelegatePointer(Delegate @delegate)
        {
            return InternalMemoryCore.GetFunctionPointer(@delegate);
        }

        /// <summary>
        ///     Gets the VF table entry.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <remarks>Created 2012-01-16 20:40 by Nesox.</remarks>
        public IntPtr GetVTablePointer(IntPtr address, int index)
        {
            return InternalMemoryCore.GetVTablePointer(address, index);
        }

        /// <summary>
        ///     Creates the proxy memory object.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="isRelative"></param>
        /// <returns>ProxyPointer.</returns>
        public ProxyPointer CreateProxyPointer(IntPtr address, bool isRelative = false)
        {
            if (isRelative)
            {
                address = ToAbsolute(address);
            }
            return new ProxyPointer(Handle, address);
        }

        /// <summary>
        ///     Gets a new Instance of the <see cref="ProcessFunction{T}" /> class.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="name">The name that represents the function.</param>
        /// <param name="address">The address where the function is located in memory.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns>A new Instance of <see cref="ProcessFunction{T}" />.</returns>
        public ProcessFunction<T> CreateProcessFunction<T>(IntPtr address, string name = "Default",
                                                           bool isRelative = false)
        {
            if (name == "Default")
            {
                name = typeof (T).Name;
            }
            return new ProcessFunction<T>(name, address);
        }

        /// <summary>
        ///     Gets a new Instance of the <see cref="VirtualClass" /> class.
        /// </summary>
        /// <param name="address">The address where the virtual class is located in memory.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns>A new Instance of <see cref="VirtualClass" />.</returns>
        public VirtualClass CreateProxyVirtualClass(IntPtr address, bool isRelative = false)
        {
            return new VirtualClass(address);
        }

        /// <summary>
        ///     Creates a function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address">The address.</param>
        /// <param name="isRelative">if set to <c>true</c> [address is relative].</param>
        /// <returns></returns>
        /// <remarks>Created 2012-01-16 20:40 by Nesox.</remarks>
        public T RegisterDelegate<T>(IntPtr address, bool isRelative = false) where T : class
        {
            return Marshal.GetDelegateForFunctionPointer(isRelative ? ToAbsolute(address) : address, typeof (T)) as T;
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MemoryPlus) obj);
        }

        /// <summary>
        ///     Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MemoryPlus left, MemoryPlus right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MemoryPlus left, MemoryPlus right)
        {
            return !Equals(left, right);
        }
    }
}