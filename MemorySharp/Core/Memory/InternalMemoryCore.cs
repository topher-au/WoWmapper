using Binarysharp.MemoryManagement.Core.Extensions;
using Binarysharp.MemoryManagement.Core.Marshaling;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Binarysharp.MemoryManagement.Core.Memory
{
    /// <summary>
    ///     Static core class providing tools for memory editing a local process, aka 'injected'.
    /// </summary>
    public static class InternalMemoryCore
    {
        #region Fields, Private Properties

        private static readonly IntPtr ImageBase = Process.GetCurrentProcess().MainModule.BaseAddress;

        #endregion Fields, Private Properties

        private static IntPtr Rebase(this IntPtr address) => ImageBase.Add(address);

        /// <summary>
        ///     Reads the value of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A value.</returns>
        public static T Read<T>(IntPtr address, bool isRelative = false)
        {
            if (isRelative)
            {
                address = Rebase(address);
            }
            return UnsafeMarshal<T>.PointerToStructure(address);
        }

        /// <summary>
        ///     Writes the specified value at the specfied address.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="value"></param>
        /// <returns>A value.</returns>
        public static void Write<T>(IntPtr address, T value)
        {
            UnsafeMarshal<T>.StructureToPointer(address, value);
        }

        /// <summary>
        ///     Reads the specified amount of bytes from the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="count">The count.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array of bytes.</returns>
        public static unsafe byte[] ReadBytes(IntPtr address, int count, bool isRelative = false)
        {
            if (isRelative)
            {
                address = Rebase(address);
            }
            var buffer = new byte[count];
            var ptr = (byte*)address;

            for (var i = 0; i < count; i++)
            {
                buffer[i] = ptr[i];
            }

            return buffer;
        }

        /// <summary>
        ///     Reads an array of a specified type in the  process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public static T[] ReadArray<T>(IntPtr address, int count, bool isRelative = false)
        {
            if (isRelative)
            {
                address = Rebase(address);
            }
            var size = UnsafeMarshal<T>.Size;
            var ret = new T[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = Read<T>(address + (i * size));
            }
            return ret;
        }

        /// <summary>
        ///     Reads a string with the specified encoding at the specified address.
        /// </summary>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="maximumLength">The maximum length of bytes the string can contian.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>A string</returns>
        /// <exception cref="ArgumentNullException">Encoding may not be null.</exception>
        /// <exception cref="ArgumentException">Address may not be IntPtr.Zero.</exception>
        /// <exception cref="DecoderFallbackException">
        ///     A fallback occurred (see Character Encoding in the .NET Framework for
        ///     complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to
        ///     <see cref="T:System.Text.DecoderExceptionFallback" />.
        /// </exception>
        public static string ReadString(IntPtr address, Encoding encoding, int maximumLength = 512,
                                        bool isRelative = false)
        {
            if (isRelative)
            {
                address = Rebase(address);
            }
            var buffer = ReadBytes(address, maximumLength);
            var ret = encoding.GetString(buffer);
            if (ret.IndexOf('\0') != -1)
            {
                ret = ret.Remove(ret.IndexOf('\0'));
            }
            return ret;
        }

        /// <summary>
        ///     Writes an array of a specified type in the remote process.
        /// </summary>
        /// <param name="address">The address where the values is written.</param>
        /// <param name="byteArray">The array to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public static unsafe void WriteBytes(IntPtr address, byte[] byteArray, bool isRelative = false)
        {
            if (isRelative)
            {
                address = Rebase(address);
            }
            var ptr = (byte*)address;
            for (var i = 0; i < byteArray.Length; i++)
            {
                ptr[i] = byteArray[i];
            }
        }

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="address">The address where the value is written.</param>
        /// <param name="value">The array of values to write.</param>
        /// s
        public static void WriteArray<T>(IntPtr address, T[] value)
        {
            var size = UnsafeMarshal<T>.Size;
            for (var i = 0; i < value.Length; i++)
            {
                var val = value[i];
                Write(address + (i * size), val);
            }
        }

        /// <summary>
        ///     Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="address">The address where the string is written.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding used.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public static void WriteString(IntPtr address, string text, Encoding encoding, bool isRelative = false)
        {
            if (text[text.Length - 1] != '\0')
            {
                text += '\0';
            }

            WriteBytes(address, encoding.GetBytes(text));
        }

        /// <summary>
        ///     Gets the function pointer from a delegate.
        /// </summary>
        /// <param name="delegate">The @delegate.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr GetFunctionPointer(Delegate @delegate)
        {
            return Marshal.GetFunctionPointerForDelegate(@delegate);
        }

        /// <summary>
        ///     Gets the virtual table pointer.
        /// </summary>
        /// <param name="address">The address the vtable is located in memory.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr GetVTablePointer(IntPtr address, int index)
        {
            return Read<IntPtr>(Read<IntPtr>(address) + (index * 4));
        }

        /// <summary>
        ///     Convert a function pointer a managed delegate.
        /// </summary>
        /// <typeparam name="T">
        ///     A delegate type to convert to. Must be adorned with
        ///     <see cref="UnmanagedFunctionPointerAttribute" />
        /// </typeparam>
        /// <param name="address">The function pointer</param>
        /// <returns>(T)</returns>
        public static T RegisterDelegate<T>(IntPtr address) where T : class
        {
            return Marshal.GetDelegateForFunctionPointer(address, typeof(T)) as T;
        }
    }
}