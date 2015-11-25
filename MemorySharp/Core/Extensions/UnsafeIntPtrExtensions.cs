using System;
using System.Runtime.InteropServices;
using System.Text;
using Binarysharp.MemoryManagement.Core.Memory;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     A class containing extension methods releated to <see cref="IntPtr" /> values.
    ///     <remarks>
    ///         This classes Read/Write methods are intended to be used for local reading and writing memory, aka
    ///         "injected" or "internal".
    ///     </remarks>
    /// </summary>
    public static class UnsafeIntPtrExtensions
    {
        /// <summary>
        ///     Adds the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr Add(this IntPtr pointer, int offset) => IntPtr.Add(pointer, offset);

        /// <summary>
        ///     Adds the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr Add(this IntPtr pointer, uint offset) => IntPtr.Add(pointer, (int) offset);

        /// <summary>
        ///     Adds the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="pointer2">The pointer2.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr Add(this IntPtr pointer, IntPtr pointer2) => IntPtr.Add(pointer, pointer2.ToInt32());

        /// <summary>
        ///     Subtracts the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr Subtract(this IntPtr pointer, int offset) => IntPtr.Subtract(pointer, offset);

        /// <summary>
        ///     Subtracts the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="pointer2">The pointer2.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr Subtract(this IntPtr pointer, IntPtr pointer2)
            => IntPtr.Subtract(pointer, pointer2.ToInt32());

        /// <summary>
        ///     Reads the address of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type being read.</typeparam>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>System.IntPtr.</returns>
        public static T Read<T>(this IntPtr address, bool isRelative = false)
        {
            return InternalMemoryCore.Read<T>(address, isRelative);
        }

        /// <summary>
        ///     Writes the specified arrayOfValues at the specfied address.
        /// </summary>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="address">The address where the arrayOfValues is read.</param>
        /// <param name="value">The value to write.</param>
        /// <returns>System.IntPtr.</returns>
        public static void Write<T>(this IntPtr address, T value)
        {
            InternalMemoryCore.Write(address, value);
        }

        /// <summary>
        ///     Reads the specified amount of bytes from the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="count">The count.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array of bytes.</returns>
        public static byte[] ReadBytes(this IntPtr address, int count, bool isRelative = false)
        {
            return InternalMemoryCore.ReadBytes(address, count, isRelative);
        }

        /// <summary>
        ///     Reads an array of a specified type in the  process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public static T[] ReadArray<T>(this IntPtr address, int count, bool isRelative = false)
        {
            return InternalMemoryCore.ReadArray<T>(address, count, isRelative);
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
        public static string ReadString(this IntPtr address, Encoding encoding, int maximumLength = 512,
                                        bool isRelative = false)
        {
            return InternalMemoryCore.ReadString(address, encoding, maximumLength, isRelative);
        }

        /// <summary>
        ///     Writes an array of a specified type in the remote process.
        /// </summary>
        /// <param name="address">The address where the values is written.</param>
        /// <param name="byteArray">The array to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public static void WriteBytes(this IntPtr address, byte[] byteArray, bool isRelative = false)
        {
            InternalMemoryCore.WriteBytes(address, byteArray, isRelative);
        }

        /// <summary>
        ///     Writes the values of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the arrayOfValues.</typeparam>
        /// <param name="address">The address where the arrayOfValues is written.</param>
        /// <param name="arrayOfValues">The array of values to write.</param>
        public static void WriteArray<T>(this IntPtr address, T[] arrayOfValues)
        {
            InternalMemoryCore.WriteArray(address, arrayOfValues);
        }

        /// <summary>
        ///     Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="address">The address where the string is written.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding used.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public static void WriteString(this IntPtr address, string text, Encoding encoding, bool isRelative = false)
        {
            InternalMemoryCore.WriteString(address, text, encoding, isRelative);
        }

        /// <summary>
        ///     Gets the function pointer from a delegate.
        /// </summary>
        /// <param name="delegate">The @delegate.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr GetFunctionPointer(this Delegate @delegate)
        {
            return InternalMemoryCore.GetFunctionPointer(@delegate);
        }

        /// <summary>
        ///     Gets the virtual table pointer.
        /// </summary>
        /// <param name="address">The address the vtable is located in memory.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr GetVTablePointer(this IntPtr address, int index)
        {
            return InternalMemoryCore.GetVTablePointer(address, index);
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
        public static T ToDelegate<T>(this IntPtr address) where T : class
        {
            return InternalMemoryCore.RegisterDelegate<T>(address);
        }
    }
}