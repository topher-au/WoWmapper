using Binarysharp.MemoryManagement.Core.CallingConvention.Enums;
using System;
using System.Threading.Tasks;

namespace Binarysharp.MemoryManagement.Models.Calls
{
    /// <summary>
    ///     Class representing a function in the remote process.
    /// </summary>
    public class RemoteFunction : IEquatable<RemoteFunction>
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteFunction" /> class.
        /// </summary>
        /// <param name="memorySharp">The memory sharp.</param>
        /// <param name="address">The address.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="callingConventions">The calling conventions the function uses.</param>
        public RemoteFunction(MemorySharp memorySharp, string functionName, IntPtr address,
                              CallingConventions callingConventions)
        {
            Name = functionName;
            MemorySharp = memorySharp;
            BaseAddress = address;
            CallingConventions = callingConventions;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the calling convention used for this remote function instance.
        /// </summary>
        /// <value>The calling convention.</value>
        public CallingConventions CallingConventions { get; }

        /// <summary>
        ///     Gets the memory sharp reference for this instance.
        /// </summary>
        /// <value>The memory sharp instance.</value>
        protected MemorySharp MemorySharp { get; }

        /// <summary>
        ///     Gets the base address of the remote function in memory.
        /// </summary>
        /// <value>The base address.</value>
        public IntPtr BaseAddress { get; }

        /// <summary>
        ///     The name of the function.
        /// </summary>
        public string Name { get; }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteFunction other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) ||
                   (BaseAddress.Equals(other.BaseAddress));
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>()
        {
            return MemorySharp.Factories.AssemblyFactory.Execute<T>(BaseAddress, CallingConventions);
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute()
        {
            return Execute<IntPtr>();
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(dynamic parameter)
        {
            return MemorySharp.Factories.AssemblyFactory.Execute<T>(BaseAddress, CallingConventions, parameter);
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="parameter">The parameter used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(dynamic parameter)
        {
            return Execute<IntPtr>(parameter);
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(params dynamic[] parameters)
        {
            return MemorySharp.Factories.AssemblyFactory.Execute<T>(BaseAddress, CallingConventions, parameters);
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(params dynamic[] parameters)
        {
            return Execute<IntPtr>(CallingConventions, parameters);
        }

        /// <summary>
        ///     Executes asynchronously the assembly code in the remote process.
        /// </summary>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<T> ExecuteAsync<T>()
        {
            return MemorySharp.Factories.AssemblyFactory.ExecuteAsync<T>(BaseAddress, CallingConventions);
        }

        /// <summary>
        ///     Executes asynchronously the assembly code in the remote process.
        /// </summary>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<IntPtr> ExecuteAsync()
        {
            return ExecuteAsync<IntPtr>();
        }

        /// <summary>
        ///     Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="parameter">The parameter used to execute the assembly code.</param>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<T> ExecuteAsync<T>(dynamic parameter)
        {
            return MemorySharp.Factories.AssemblyFactory.ExecuteAsync<T>(BaseAddress, CallingConventions, parameter);
        }

        /// <summary>
        ///     Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="parameter">The parameter used to execute the assembly code.</param>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<IntPtr> ExecuteAsync(dynamic parameter)
        {
            return ExecuteAsync<IntPtr>(parameter);
        }

        /// <summary>
        ///     Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<IntPtr> ExecuteAsync(params dynamic[] parameters)
        {
            return ExecuteAsync<IntPtr>(parameters);
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RemoteFunction)obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return BaseAddress.GetHashCode();
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RemoteFunction left, RemoteFunction right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RemoteFunction left, RemoteFunction right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"BaseAddress = 0x{BaseAddress.ToInt64():X} Name = {Name}";
        }
    }
}