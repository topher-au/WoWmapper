using Binarysharp.MemoryManagement.Core.Extensions;
using Binarysharp.MemoryManagement.Core.Helpers;
using Binarysharp.MemoryManagement.Models.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Binarysharp.MemoryManagement.Models.Memory
{
    /// <summary>
    ///     Defines a set of memory reading and writing operations.
    /// </summary>
    public abstract class ProcessMemory : IDisposable
    {
        #region Fields, Private Properties

        private Lazy<PatternScanner> LazyMainModulePatterns { get; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessMemory" /> class.
        /// </summary>
        /// <param name="process">The process.</param>
        protected ProcessMemory(Process process)
        {
            Process = process;
            LazyMainModulePatterns = new Lazy<PatternScanner>((() => new PatternScanner(this, Process.MainModule)));
            Handle = process.Handle;
            ImageBase = process.MainModule.BaseAddress;
        }

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
        ///     collection.
        /// </summary>
        ~ProcessMemory()
        {
            Dispose();
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     The native process.
        /// </summary>
        public Process Process { get; }

        /// <summary>
        ///     The address of the local processes main module.
        /// </summary>
        public IntPtr ImageBase { get; }

        /// <summary>
        ///     The handle to the process.
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        ///     Gets the native modules that have been loaded in the remote process.
        /// </summary>
        public IEnumerable<ProcessModule> NativeModules => Process.Modules.Cast<ProcessModule>();

        /// <summary>
        ///     Gets the a pattern scanning instance for the local processes main mdoule using a
        ///     <see cref="Lazy{PatternScanner}" /> value.
        /// </summary>
        public PatternScanner MainModulePatterns => LazyMainModulePatterns.Value;

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public virtual void Dispose()
        {
            // Patches.DisableAll();
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Reads the specified amount of bytes from the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="count">The count.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns>An array of bytes.</returns>
        public abstract byte[] ReadBytes(IntPtr address, int count, bool isRelative = false);

        /// <summary>
        ///     Reads the value of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A value.</returns>
        public abstract T Read<T>(IntPtr address, bool isRelative = false);

        /// <summary>
        ///     Reads the value of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="offsets">The offsets to apply in order to the given address.</param>
        /// <returns>A value.</returns>
        public T ReadMultilevelPointer<T>(bool isRelative, IntPtr address, params int[] offsets)
        {
            if (isRelative)
            {
                address = ToAbsolute(address);
            }
            if (offsets.Length == 0)
            {
                throw new InvalidOperationException("Cannot read a value from unspecified addresses.");
            }

            var temp = Read<IntPtr>(address);

            for (var i = 0; i < offsets.Length - 1; i++)
            {
                temp = Read<IntPtr>(temp + offsets[i]);
            }
            return Read<T>(temp + offsets[offsets.Length - 1]);
        }

        /// <summary>
        ///     Reads an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public abstract T[] ReadArray<T>(IntPtr address, int count, bool isRelative = false);

        /// <summary>
        ///     Reads a string with the specified encoding at the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="maximumLength">The maximum length.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="ArgumentNullException">Encoding may not be null.</exception>
        /// <exception cref="ArgumentException">Address may not be IntPtr.Zero.</exception>
        /// <exception cref="DecoderFallbackException">
        ///     A fallback occurred (see Character Encoding in the .NET Framework for
        ///     complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to
        ///     <see cref="T:System.Text.DecoderExceptionFallback" />.
        /// </exception>
        public string ReadString(IntPtr address, Encoding encoding, int maximumLength = 512,
                                 bool isRelative = false)
        {
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));
            Requires.NotNull(encoding, nameof(encoding));

            var buffer = ReadBytes(address, maximumLength, isRelative);
            var ret = encoding.GetString(buffer);
            if (ret.IndexOf('\0') != -1)
            {
                ret = ret.Remove(ret.IndexOf('\0'));
            }
            return ret;
        }

        /// <summary>
        ///     Writes the specified string at the specified address using the specified encoding.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Address may not be IntPtr.Zero.</exception>
        /// <exception cref="ArgumentNullException">Encoding may not be null.</exception>
        /// <exception cref="EncoderFallbackException">
        ///     A fallback occurred (see Character Encoding in the .NET Framework for
        ///     complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to
        ///     <see cref="T:System.Text.EncoderExceptionFallback" />.
        /// </exception>
        public void WriteString(IntPtr address, string value, Encoding encoding, bool isRelative = false)
        {
            Requires.NotEqual(address, IntPtr.Zero, nameof(address));
            Requires.NotNull(encoding, nameof(encoding));

            if (value[value.Length - 1] != '\0')
            {
                value += '\0';
            }

            WriteBytes(address, encoding.GetBytes(value), isRelative);
        }

        /// <summary>
        ///     Writes the specified bytes at the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="byteArray">The bytes.</param>
        /// <param name="isRelative">if set to <c>true</c> [is relative].</param>
        public abstract void WriteBytes(IntPtr address, byte[] byteArray, bool isRelative = false);

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is written.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public abstract void Write<T>(IntPtr address, T value, bool isRelative = false);

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is written.</param>
        /// <param name="arrayOfValues">The array of values to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public abstract void WriteArray<T>(IntPtr address, T[] arrayOfValues, bool isRelative = false);

        /// <summary>
        ///     Creates a <see cref="PatternScanner" /> Instance for the given process module name.
        /// </summary>
        /// <param name="moduleName">The name of the process module that contains the data to match patterns with.</param>
        /// <returns>A new <see cref="PatternScanner" /> Instance.</returns>
        public PatternScanner CreatePatternScanner(string moduleName)
        {
            return new PatternScanner(this, GetModule(moduleName));
        }

        /// <summary>
        ///     Gets the module loaded by the opened process that matches the given string.
        /// </summary>
        /// <param name="sModuleName">String specifying which module to return.</param>
        /// <returns>Returns the module loaded by the opened process that matches the given string.</returns>
        public ProcessModule GetModule(string sModuleName)
        {
            return
                NativeModules.FirstOrDefault(
                    pMod => pMod.ModuleName.ToLower().Equals(sModuleName.ToLower()));
        }

        /// <summary>
        ///     Converts the specified absolute address to a relative address.
        /// </summary>
        /// <param name="absoluteAddress">The absolute address.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">absoluteAddress may not be IntPtr.Zero.</exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr ToRelative(IntPtr absoluteAddress)
        {
            Requires.NotEqual(absoluteAddress, IntPtr.Zero, nameof(absoluteAddress));
            return ImageBase.Subtract(absoluteAddress);
        }

        /// <summary>
        ///     Converts the specified relative address to an absolute address.
        /// </summary>
        /// <param name="relativeAddress">The relative address.</param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IntPtr ToAbsolute(IntPtr relativeAddress)
        {
            // In this case, we allow IntPtr zero - relative + base = base, so no harm done.
            return ImageBase.Add(relativeAddress);
        }
    }
}