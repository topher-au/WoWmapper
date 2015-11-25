using System;
using System.Text;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     Class containing extension methods releated to <see cref="ulong" /> values.
    /// </summary>
    public static class ULongExtensions
    {
        /// <summary>
        ///     Reads the address of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type being read.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>System.IntPtr.</returns>
        public static T Read<T>(this ulong address, bool isRelative) where T : struct
        {
            return Convert.ToInt64(address).Read<T>(isRelative);
        }

        /// <summary>
        ///     Reads an array of a specified type in the  process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public static T[] ReadArray<T>(this ulong address, int count, bool isRelative) where T : struct
        {
            return Convert.ToInt64(address).ReadArray<T>(count, isRelative);
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
        public static string ReadString(this ulong address, Encoding encoding, int maximumLength = 512,
                                        bool isRelative = false)
        {
            return Convert.ToInt64(address).ReadString(encoding, maximumLength, isRelative);
        }

        /// <summary>
        ///     Writes the specified arrayOfValues at the specfied address.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="address">The address where the arrayOfValues is read.</param>
        /// <param name="value">The value to write.</param>
        /// <returns>System.IntPtr.</returns>
        public static void Write<T>(this ulong address, T value) where T : struct
        {
            Convert.ToInt64(address).Write(value);
        }

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the arrayOfValues.</typeparam>
        /// <param name="address">The address where the arrayOfValues is written.</param>
        /// <param name="arrayOfValues">The array of values to write.</param>
        public static void WriteArray<T>(this ulong address, T[] arrayOfValues) where T : struct
        {
            Convert.ToInt64(address).WriteArray(arrayOfValues);
        }

        /// <summary>
        ///     Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="address">The address where the string is written.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding used.</param>
        public static void WriteString(this ulong address, string text, Encoding encoding)
        {
            new IntPtr(Convert.ToInt64(address)).WriteString(text, encoding);
        }

        /// <summary>
        ///     Gets the virtual table pointer.
        /// </summary>
        /// <param name="address">The address the vtable is located in memory.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr GetVTablePointer(this ulong address, int index)
        {
            return new IntPtr(Convert.ToInt64(address)).GetVTablePointer(index);
        }
    }
}