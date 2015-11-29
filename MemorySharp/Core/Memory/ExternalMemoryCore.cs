using Binarysharp.MemoryManagement.Core.Marshaling;
using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#pragma warning disable 1584,1711,1572,1581,1580

namespace Binarysharp.MemoryManagement.Core.Memory
{
    /// <summary>
    ///     Static core class providing tools for memory editing a remote process, aka 'external'.
    ///     <remarks>
    ///         Some of these methods and operations have valid uses besides external memory operations. However, most of
    ///         this class is intended to be used while out of process.
    ///     </remarks>
    /// </summary>
    public static class ExternalMemoryCore
    {
        /// <summary>
        ///     Reads an array of bytes in the memory form the target process.
        /// </summary>
        /// <param name="processHandle">A processHandle to the process with memory that is being read.</param>
        /// <param name="address">A pointer to the base address in the specified process from which to read.</param>
        /// <param name="size">The number of bytes to be read from the specified process.</param>
        /// <returns>The collection of read bytes.</returns>
        public static byte[] ReadProcessMemory(IntPtr processHandle, IntPtr address, int size)
        {
            // Check if the handles are valid
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to the ReadProcessMemory method was invalid.");

            // Allocate the buffer
            var buffer = new byte[size];
            int nbBytesRead;

            // Read the data from the target process
            if (UnsafeNativeMethods.ReadProcessMemory(processHandle, address, buffer, size, out nbBytesRead) &&
                size == nbBytesRead)
                return buffer;

            // Else the data couldn't be read, throws an exception
            throw new Win32Exception($"Couldn't read {size} byte(s) from 0x{address.ToString("X")}.");
        }

        /// <summary>
        ///     Reads the address of a specified type in the process.
        /// </summary>
        /// <typeparam name="T">The type being read.</typeparam>
        /// <param name="processHandle">The processHandle to the process.</param>
        /// <param name="address">The address where the value is read.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>System.IntPtr.</returns>
        [SuppressMessage("ReSharper", "InvertIf")]
        public static unsafe T Read<T>(IntPtr processHandle, IntPtr address)
        {
            var bytes = new byte[SafeMarshal<T>.Size];
            bytes = ReadProcessMemory(processHandle, address, bytes.Length);
            if (SafeMarshal<T>.IsIntPtr)
            {
                object ret;
                fixed (byte* pByte = bytes)
                    ret = new IntPtr(*(void**)pByte);
                return (T)ret;
            }
            return SafeMarshal<T>.ByteArrayToObject(bytes);
        }

        /// <summary>
        ///     Reads an array of a specified type in the  process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="processHandle">The process processHandle.</param>
        /// <param name="address">The address where the values is read.</param>
        /// <param name="count">The number of cells in the array.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <returns>An array.</returns>
        public static T[] ReadArray<T>(IntPtr processHandle, IntPtr address, int count)
        {
            // Allocate an array to store the results
            var array = new T[count];
            // Read the array in the remote process
            for (var i = 0; i < count; i++)
            {
                array[i] = Read<T>(processHandle, address + SafeMarshal<T>.Size * i);
            }
            return array;
        }

        /// <summary>
        ///     Reads a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="processHandle">The processHandle to the process.</param>
        /// <param name="address">The address where the string is read.</param>
        /// <param name="encoding">The encoding used.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        /// <param name="maxLength">
        ///     [Optional] The number of maximum bytes to read. The string is automatically cropped at this end
        ///     ('\0' char).
        /// </param>
        /// <returns>The string.</returns>
        public static string ReadString(IntPtr processHandle, IntPtr address, Encoding encoding, bool isRelative = true,
                                        int maxLength = 512)
        {
            // Read the string
            var data = encoding.GetString(ReadProcessMemory(processHandle, address, maxLength));
            // Search the end of the string
            var end = data.IndexOf('\0');
            // Crop the string with this end
            return data.Substring(0, end);
        }

        /// <summary>
        ///     Writes data to an area of memory in a specified process.
        /// </summary>
        /// <param name="processHandle">A processHandle to the process memory to be modified.</param>
        /// <param name="address">A pointer to the base address in the specified process to which data is written.</param>
        /// <param name="byteArray">A buffer that contains data to be written in the address space of the specified process.</param>
        /// <returns>The number of bytes written.</returns>
        public static int WriteProcessMemory(IntPtr processHandle, IntPtr address, byte[] byteArray)
        {
            // Check if the handles are valid
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to the WriteProcessMemory method was invalid.");

            // Create the variable storing the number of bytes written
            int nbBytesWritten;

            // Write the data to the target process
            if (UnsafeNativeMethods.WriteProcessMemory(processHandle, address, byteArray, byteArray.Length,
                out nbBytesWritten))
            {
                // Check whether the length of the data written is equal to the inital array
                if (nbBytesWritten == byteArray.Length)
                    return nbBytesWritten;
            }

            // Else the data couldn't be written, throws an exception
            throw new Win32Exception($"Couldn't write {byteArray.Length} bytes to 0x{address.ToString("X")}");
        }

        /// <summary>
        ///     Writes the specified arrayOfValues at the specfied address.
        /// </summary>
        /// <param name="processHandle">The process processHandle.</param>
        /// <typeparam name="T">The type of the value to write.</typeparam>
        /// <param name="address">The address where the value is to be written.</param>
        /// <param name="value">The value to write.</param>
        public static void Write<T>(IntPtr processHandle, IntPtr address, T value)
        {
            var bytes = SafeMarshal<T>.ObjectToByteArray(value);
            WriteProcessMemory(processHandle, address, bytes);
        }

        /// <summary>
        ///     Writes an array of a specified type in the remote process.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="processHandle">The process processHandle.</param>
        /// <param name="address">The address where the values is written.</param>
        /// <param name="dataArray">The array to write.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public static void WriteArray<T>(IntPtr processHandle, IntPtr address, T[] dataArray)
        {
            // Write the array in the remote process
            for (var i = 0; i < dataArray.Length; i++)
            {
                Write(processHandle, address + SafeMarshal<T>.Size * i, dataArray[i]);
            }
        }

        /// <summary>
        ///     Writes a string with a specified encoding in the remote process.
        /// </summary>
        /// <param name="handle">The processHandle to the process.</param>
        /// <param name="address">The address where the string is written.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding used.</param>
        /// <param name="isRelative">[Optional] State if the address is relative to the main module.</param>
        public static void WriteString(IntPtr handle, IntPtr address, string text, Encoding encoding)
        {
            WriteProcessMemory(handle, address, encoding.GetBytes(text + '\0'));
        }

        /// <summary>
        ///     Reserves a region of memory within the virtual address space of a specified process.
        /// </summary>
        /// <param name="processHandle">The processHandle to a process.</param>
        /// <param name="size">The size of the region of memory to allocate, in bytes.</param>
        /// <param name="protectionFlags">The memory protection for the region of pages to be allocated.</param>
        /// <param name="allocationFlags">The type of memory allocation.</param>
        /// <returns>The base address of the allocated region.</returns>
        public static IntPtr Allocate(IntPtr processHandle, int size,
                                      MemoryProtectionFlags protectionFlags = MemoryProtectionFlags.ExecuteReadWrite,
                                      MemoryAllocationFlags allocationFlags = MemoryAllocationFlags.Commit)
        {
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to the RemoteMemory.Allocate method was invalid.");
            // Allocate a memory page
            var ret = UnsafeNativeMethods.VirtualAllocEx(processHandle, IntPtr.Zero, size, allocationFlags,
                protectionFlags);

            // Check whether the memory page is valid
            if (ret != IntPtr.Zero)
                return ret;

            // If the pointer isn't valid, throws an exception
            throw new Win32Exception($"Couldn't allocate memory of {size} byte(s).");
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
        /// <param name="processHandle">A processHandle to a process.</param>
        /// <param name="address">A pointer to the starting address of the region of memory to be freed.</param>
        public static void Free(IntPtr processHandle, IntPtr address)
        {
            // Free the memory
            if (!UnsafeNativeMethods.VirtualFreeEx(processHandle, address, 0, MemoryReleaseFlags.Release))
            {
                // If the memory wasn't correctly freed, throws an exception
                throw new Win32Exception($"The memory page 0x{address.ToString("X")} cannot be freed.");
            }
        }

        /// <summary>
        ///     etrieves information about the specified process.
        /// </summary>
        /// <param name="processHandle">A processHandle to the process to query.</param>
        /// <returns>A <see cref="ProcessBasicInformation" /> structure containg process information.</returns>
        public static ProcessBasicInformation NtQueryInformationProcess(IntPtr processHandle)
        {
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to the NtQueryInformationProcess method was invalid.");

            // Create a structure to store process info
            var info = new ProcessBasicInformation();

            // Get the process info
            var ret = UnsafeNativeMethods.NtQueryInformationProcess(processHandle,
                ProcessInformationClass.ProcessBasicInformation, ref info, info.Size, IntPtr.Zero);

            // If the function succeeded
            if (ret == 0)
                return info;

            // Else, couldn't get the process info, throws an exception
            throw new ApplicationException(
                $"Couldn't get the information from the process, error code '{ret}'.");
        }

        /// <summary>
        ///     Opens an existing local process object.
        /// </summary>
        /// <param name="accessFlags">The access level to the process object.</param>
        /// <param name="processId">The identifier of the local process to be opened.</param>
        /// <returns>An open processHandle to the specified process.</returns>
        public static IntPtr OpenProcess(ProcessAccessFlags accessFlags, int processId)
        {
            // Get an processHandle from the remote process
            var handle = UnsafeNativeMethods.OpenProcess(accessFlags, false, processId);

            // Check whether the processHandle is valid
            if (handle != IntPtr.Zero)
                return handle;

            // Else the processHandle isn't valid, throws an exception
            throw new Win32Exception($"Couldn't open the process {processId}.");
        }

        /// <summary>
        ///     Changes the protection on a region of committed pages in the virtual address space of a specified process.
        /// </summary>
        /// <param name="processHandle">A processHandle to the process whose memory protection is to be changed.</param>
        /// <param name="address">
        ///     A pointer to the base address of the region of pages whose access protection attributes are to be
        ///     changed.
        /// </param>
        /// <param name="size">The size of the region whose access protection attributes are changed, in bytes.</param>
        /// <param name="protection">The memory protection option.</param>
        /// <returns>The old protection of the region in a <see cref="MemoryBasicInformation" /> structure.</returns>
        public static MemoryProtectionFlags ChangeProtection(IntPtr processHandle, IntPtr address, int size,
                                                             MemoryProtectionFlags protection)
        {
            // Check if the handles are valid
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to the ReadProcessMemory method was invalid.");

            // Create the variable storing the old protection of the memory page
            MemoryProtectionFlags oldProtection;

            // Change the protection in the target process
            if (UnsafeNativeMethods.VirtualProtectEx(processHandle, address, size, protection, out oldProtection))
            {
                // Return the old protection
                return oldProtection;
            }

            // Else the protection couldn't be changed, throws an exception
            throw new Win32Exception(
                $"Couldn't change the protection of the memory at 0x{address.ToString("X")} of {size} byte(s) to {protection}.");
        }

        /// <summary>
        ///     Retrieves information about a range of pages within the virtual address space of a specified process.
        /// </summary>
        /// <param name="processHandle">A processHandle to the process whose memory information is queried.</param>
        /// <param name="baseAddress">A pointer to the base address of the region of pages to be queried.</param>
        /// <returns>
        ///     A <see cref="MemoryBasicInformation" /> structures in which information about the specified page range
        ///     is returned.
        /// </returns>
        public static MemoryBasicInformation Query(IntPtr processHandle, IntPtr baseAddress)
        {
            // Allocate the structure to store information of memory
            MemoryBasicInformation memoryInfo;

            // Check if the handles are valid
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to RemoteMemory.Query method was invalid.");

            // Query the memory region
            if (
                UnsafeNativeMethods.VirtualQueryEx(processHandle, baseAddress, out memoryInfo,
                    SafeMarshal<MemoryBasicInformation>.Size) != 0)
                return memoryInfo;

            // Else the information couldn't be got
            throw new Win32Exception($"Couldn't query information about the memory region 0x{baseAddress.ToString("X")}");
        }

        /// <summary>
        ///     Retrieves information about a range of pages within the virtual address space of a specified process.
        /// </summary>
        /// <param name="processHandle">A processHandle to the process whose memory information is queried.</param>
        /// <param name="addressFrom">A pointer to the starting address of the region of pages to be queried.</param>
        /// <param name="addressTo">A pointer to the ending address of the region of pages to be queried.</param>
        /// <returns>A collection of <see cref="MemoryBasicInformation" /> structures.</returns>
        public static IEnumerable<MemoryBasicInformation> Query(IntPtr processHandle, IntPtr addressFrom,
                                                                IntPtr addressTo)
        {
            // Check if the handles are valid
            if (processHandle == IntPtr.Zero)
                throw new Exception("The processHandle passed to RemoteMemory.Query method was invalid.");

            // Convert the addresses to Int64
            var numberFrom = addressFrom.ToInt64();
            var numberTo = addressTo.ToInt64();

            // The first address must be lower than the second
            if (numberFrom >= numberTo)
                throw new ArgumentException("The starting address must be lower than the ending address.",
                    nameof(addressFrom));

            // Create the variable storing the result of the call of VirtualQueryEx
            int ret;

            // Enumerate the memory pages
            do
            {
                // Allocate the structure to store information of memory
                MemoryBasicInformation memoryInfo;

                // Get the next memory page
                ret = UnsafeNativeMethods.VirtualQueryEx(processHandle, new IntPtr(numberFrom), out memoryInfo,
                    SafeMarshal<MemoryBasicInformation>.Size);

                // Increment the starting address with the size of the page
                numberFrom += memoryInfo.RegionSize;

                // Return the memory page
                if (memoryInfo.State != MemoryStateFlags.Free)
                    yield return memoryInfo;
            } while (numberFrom < numberTo && ret != 0);
        }
    }
}