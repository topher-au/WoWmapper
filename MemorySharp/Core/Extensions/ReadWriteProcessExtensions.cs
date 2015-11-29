using Binarysharp.MemoryManagement.Core.Memory;
using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Objects;
using Binarysharp.MemoryManagement.Core.Native.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     Class containing extension methods releated to windows <see cref="Process" />'s.
    ///     <remarks>
    ///         This class is intended to be used for external reading and writing of another processes memory, aka
    ///         "external".
    ///     </remarks>
    /// </summary>
    public static class ReadWriteProcessExtensions
    {
        /// <summary>
        ///     Reads an array of bytes in the memory form the target process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="address">A pointer to the base address in the specified process from which to read.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>The collection of read bytes.</returns>
        public static byte[] ReadProcessMemory(this Process process, IntPtr address, int size)
        {
            return ExternalMemoryCore.ReadProcessMemory(process.Handle, address, size);
        }

        /// <summary>
        ///     Reads the address of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type being read.</typeparam>
        /// <param name="process">The process.</param>
        /// <param name="address">The address where the value is read.</param>
        /// <returns>System.IntPtr.</returns>
        [SuppressMessage("ReSharper", "InvertIf")]
        public static T Read<T>(this Process process, IntPtr address) where T : struct
        {
            return ExternalMemoryCore.Read<T>(process.Handle, address);
        }

        /// <summary>
        ///     Reads an array of a specified type in the  process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="proc">The process.</param>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param>[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public static T[] ReadArray<T>(this Process proc, IntPtr address, int count) where T : struct
        {
            return ExternalMemoryCore.ReadArray<T>(proc.Handle, address, count);
        }

        /// <summary>
        ///     Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="address">The address where the string is read.</param>
        /// <param name="encoding">The encoding used.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="maxLength">
        ///     [Optional] The number of maximum bytes to read. The string is automatically cropped at this end
        ///     ('\0' char).
        /// </param>
        /// <returns>The string.</returns>
        public static string ReadString(this Process process, IntPtr address, Encoding encoding, bool isRelative = true,
                                        int maxLength = 512)
        {
            // Read the string
            var data = encoding.GetString(ExternalMemoryCore.ReadProcessMemory(process.Handle, address, maxLength));
            // Search the end of the string
            var end = data.IndexOf('\0');
            // Crop the string with this end
            return data.Substring(0, end);
        }

        /// <summary>
        ///     Writes data to an area of memory in a specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="address">A pointer to the base address in the specified process to which data is written.</param>
        /// <param name="byteArray">A buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>The number of bytes written.</returns>
        public static int WriteProcessMemory(this Process process, IntPtr address, byte[] byteArray)
        {
            return ExternalMemoryCore.WriteProcessMemory(process.Handle, address, byteArray);
        }

        /// <summary>
        ///     Writes the specified arrayOfValues at the specfied address.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="address">The address where the value is to be written.</param>
        /// <param name="value">The value to write.</param>
        public static void Write<T>(this Process process, IntPtr address, T value) where T : struct
        {
            ExternalMemoryCore.Write(process.Handle, address, value);
        }

        /// <summary>
        ///     Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="process">The process</param>
        /// <param name="address">The address where the values is written.</param>
        /// <param name="dataArray">The array to write.</param>
        /// <param>[Optional] State if the address is relative to the main module.</param>
        public static void WriteArray<T>(this Process process, IntPtr address, T[] dataArray) where T : struct
        {
            ExternalMemoryCore.WriteArray(process.Handle, address, dataArray);
        }

        /// <summary>
        ///     Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="address">The address where the string is written.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding used.</param>
        public static void WriteString(this Process process, IntPtr address, string text, Encoding encoding)
        {
            ExternalMemoryCore.WriteProcessMemory(process.Handle, address, encoding.GetBytes(text + '\0'));
        }

        /// <summary>
        ///     Gets an enumeration of all the processes threads.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>IEnumerable&lt;ProcessThread&gt;.</returns>
        public static IEnumerable<ProcessThread> GetProcessThreads(this Process process)
        {
            // Refresh the process info
            process.Refresh();
            // Enumerates all threads
            return process.Threads.Cast<ProcessThread>();
        }

        /// <summary>
        ///     Gets the native modules that have been loaded in the process.
        /// </summary>
        public static IEnumerable<ProcessModule> GetModules(this Process process)
        {
            return process.Modules.Cast<ProcessModule>();
        }

        /// <summary>
        ///     Gets the main process modules pointer address as an <see cref="IntPtr" /> value.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr ImageBase(this Process process)
        {
            return process.MainModule.BaseAddress;
        }

        /// <summary>
        ///     Gets the main process modules size of the local process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>An integer representing the processes main modules memory size.</returns>
        public static int ImageSize(this Process process)
        {
            return process.MainModule.ModuleMemorySize;
        }

        /// <summary>
        ///     Opens the specified process and returns the processHandle.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="processAccessFlags">The process access flags.</param>
        /// <returns>System.IntPtr.</returns>
        public static IntPtr UnsafeOpenHandle(this Process process, ProcessAccessFlags processAccessFlags)
        {
            return ExternalMemoryCore.OpenProcess(processAccessFlags, process.Id);
        }

        /// <summary>
        ///     Opens the specified process and returns the <see cref="SafeMemoryHandle" />
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="processAccessFlags">The process access flags.</param>
        /// <returns>System.IntPtr.</returns>
        public static SafeMemoryHandle SafeOpenHandle(this Process process, ProcessAccessFlags processAccessFlags)
        {
            return SafeNativeMethods.OpenProcess(processAccessFlags, false, process.Id);
        }

        /// <summary>
        ///     Reserves a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="size">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="protectionFlags">The memory protection for the region of pages to be allocated.</param>
        /// <param name="allocationFlags">The type of memory allocation.</param>
        /// <returns>The base address of the allocated region.</returns>
        public static IntPtr Allocate(this Process process, int size,
                                      MemoryProtectionFlags protectionFlags = MemoryProtectionFlags.ExecuteReadWrite,
                                      MemoryAllocationFlags allocationFlags = MemoryAllocationFlags.Commit)
        {
            return ExternalMemoryCore.Allocate(process.Handle, size, protectionFlags, allocationFlags);
        }

        /// <summary>
        ///     Closes an open object processHandle.
        /// </summary>
        /// <param name="handle">A valid processHandle to an open object.</param>
        public static void CloseHandle(IntPtr handle)
        {
            // Close the processHandle
            if (!UnsafeNativeMethods.CloseHandle(handle))
            {
                throw new Win32Exception($"Couldn't close he handle 0x{handle}.");
            }
        }

        /// <summary>
        ///     Releases a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="address">A pointer to the starting address of the region of memory to be freed.</param>
        public static void Free(this Process process, IntPtr address)
        {
            ExternalMemoryCore.Free(process.Handle, address);
        }

        /// <summary>
        ///     etrieves information about the specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>A <see cref="ProcessBasicInformation" /> structure containg process information.</returns>
        public static ProcessBasicInformation NtQueryInformationProcess(this Process process)
        {
            return ExternalMemoryCore.NtQueryInformationProcess(process.Handle);
        }

        /// <summary>
        ///     Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="address">
        ///     A pointer to the base address of the region of pages whose access protection attributes are to be
        ///     changed.
        /// </param>
        /// <param name="size">The size of the region whose access protection attributes are changed, in bytes.</param>
        /// <param name="protection">The memory protection option.</param>
        /// <returns>The old protection of the region in a <see cref="MemoryBasicInformation" /> structure.</returns>
        public static MemoryProtectionFlags ChangeProtection(this Process process, IntPtr address, int size,
                                                             MemoryProtectionFlags protection)
        {
            return ExternalMemoryCore.ChangeProtection(process.Handle, address, size, protection);
        }

        /// <summary>
        ///     Retrieves information about a range of pages within the virtual address space of a specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="baseAddress">A pointer to the base address of the region of pages to be queried.</param>
        /// <returns>
        ///     A <see cref="MemoryBasicInformation" /> structures in which information about the specified page range
        ///     is returned.
        /// </returns>
        public static MemoryBasicInformation Query(this Process process, IntPtr baseAddress)
        {
            return ExternalMemoryCore.Query(process.Handle, baseAddress);
        }

        /// <summary>
        ///     Retrieves information about a range of pages within the virtual address space of a specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <param name="addressFrom">A pointer to the starting address of the region of pages to be queried.</param>
        /// <param name="addressTo">A pointer to the ending address of the region of pages to be queried.</param>
        /// <returns>A collection of <see cref="MemoryBasicInformation" /> structures.</returns>
        public static IEnumerable<MemoryBasicInformation> Query(this Process process, IntPtr addressFrom,
                                                                IntPtr addressTo)
        {
            return ExternalMemoryCore.Query(process.Handle, addressFrom, addressTo);
        }
    }
}