using System;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement.Core.CallingConvention.Enums;

namespace Binarysharp.MemoryManagement.Models.Calls
{
    /// <summary>
    ///     Class providing tools to execute remote functions.
    /// </summary>
    public class GenericRemoteFunction : IEquatable<GenericRemoteFunction>
    {
        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteFunction" /> class.
        /// </summary>
        /// <param name="memorySharp">The memory sharp.</param>
        /// <param name="address">The address.</param>
        /// <param name="functionName">Name of the function.</param>
        public GenericRemoteFunction(MemorySharp memorySharp, string functionName, IntPtr address)
        {
            Name = functionName;
            MemorySharp = memorySharp;
            BaseAddress = address;
            ;
        }
        #endregion

        #region Public Properties, Indexers
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
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(GenericRemoteFunction other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) ||
                   (BaseAddress.Equals(other.BaseAddress));
        }
        #endregion

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((GenericRemoteFunction) obj);
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>()
        {
            return MemorySharp.Factories.AssemblyFactory.Execute<T>(BaseAddress);
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
            return MemorySharp.Factories.AssemblyFactory.Execute<T>(BaseAddress, parameter);
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
        /// <param name="callingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public T Execute<T>(CallingConventions callingConvention, params dynamic[] parameters)
        {
            return MemorySharp.Factories.AssemblyFactory.Execute<T>(BaseAddress, callingConvention, parameters);
        }

        /// <summary>
        ///     Executes the assembly code in the remote process.
        /// </summary>
        /// <param name="callingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>The return value is the exit code of the thread created to execute the assembly code.</returns>
        public IntPtr Execute(CallingConventions callingConvention, params dynamic[] parameters)
        {
            return Execute<IntPtr>(callingConvention, parameters);
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
            return MemorySharp.Factories.AssemblyFactory.ExecuteAsync<T>(BaseAddress);
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
            return MemorySharp.Factories.AssemblyFactory.ExecuteAsync<T>(BaseAddress, parameter);
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
        /// <param name="callingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<T> ExecuteAsync<T>(CallingConventions callingConvention, params dynamic[] parameters)
        {
            return MemorySharp.Factories.AssemblyFactory.ExecuteAsync<T>(BaseAddress, callingConvention, parameters);
        }

        /// <summary>
        ///     Executes asynchronously the assembly code located in the remote process at the specified address.
        /// </summary>
        /// <param name="callingConvention">The calling convention used to execute the assembly code with the parameters.</param>
        /// <param name="parameters">An array of parameters used to execute the assembly code.</param>
        /// <returns>
        ///     The return value is an asynchronous operation that return the exit code of the thread created to execute the
        ///     assembly code.
        /// </returns>
        public Task<IntPtr> ExecuteAsync(CallingConventions callingConvention, params dynamic[] parameters)
        {
            return ExecuteAsync<IntPtr>(callingConvention, parameters);
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
        public static bool operator ==(GenericRemoteFunction left, GenericRemoteFunction right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(GenericRemoteFunction left, GenericRemoteFunction right)
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