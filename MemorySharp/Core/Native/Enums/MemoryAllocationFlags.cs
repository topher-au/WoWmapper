using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     Memory-allocation options list.
    /// </summary>
    [Flags]
    public enum MemoryAllocationFlags
    {
        /// <summary>
        ///     Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved
        ///     memory pages.
        ///     The function also guarantees that when the caller later initially accesses the memory, the contents will be zero.
        ///     Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.
        ///     To reserve and commit pages in one step, call <see cref="SafeNativeMethods.VirtualAllocEx" /> with MEM_COMMIT |
        ///     MEM_RESERVE.
        ///     The function fails if you attempt to commit a page that has not been reserved. The resulting error code is
        ///     ERROR_INVALID_ADDRESS.
        ///     An attempt to commit a page that is already committed does not cause the function to fail.
        ///     This means that you can commit pages without first determining the current commitment state of each page.
        /// </summary>
        Commit = 0x00001000,

        /// <summary>
        ///     Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or
        ///     in the paging file on disk.
        ///     You commit reserved pages by calling <see cref="SafeNativeMethods.VirtualAllocEx" /> again with MEM_COMMIT.
        ///     To reserve and commit pages in one step, call VirtualAllocEx with MEM_COMMIT | MEM_RESERVE.
        ///     Other memory allocation functions, such as malloc and LocalAlloc, cannot use reserved memory until it has been
        ///     released.
        /// </summary>
        Reserve = 0x00002000,

        /// <summary>
        ///     Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.
        ///     The pages should not be read from or written to the paging file.
        ///     However, the memory block will be used again later, so it should not be decommitted. This value cannot be used with
        ///     any other value.
        ///     Using this value does not guarantee that the range operated on with MEM_RESET will contain zeros. If you want the
        ///     range to contain zeros, decommit the memory and then recommit it.
        ///     When you use MEM_RESET, the VirtualAllocEx function ignores the value of fProtect. However, you must still set
        ///     fProtect to a valid protection value, such as PAGE_NOACCESS.
        ///     <see cref="SafeNativeMethods.VirtualAllocEx" /> returns an error if you use MEM_RESET and the range of memory is
        ///     mapped to a file.
        ///     A shared view is only acceptable if it is mapped to a paging file.
        /// </summary>
        Reset = 0x00080000,

        /// <summary>
        ///     MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier.
        ///     It indicates that the data in the specified memory range specified by lpAddress and dwSize is of interest to the
        ///     caller and attempts to reverse the effects of MEM_RESET.
        ///     If the function succeeds, that means all data in the specified address range is intact.
        ///     If the function fails, at least some of the data in the address range has been replaced with zeroes.
        ///     This value cannot be used with any other value.
        ///     If MEM_RESET_UNDO is called on an address range which was not MEM_RESET earlier, the behavior is undefined.
        ///     When you specify MEM_RESET, the <see cref="SafeNativeMethods.VirtualAllocEx" /> function ignores the value of
        ///     flProtect.
        ///     However, you must still set flProtect to a valid protection value, such as PAGE_NOACCESS.
        /// </summary>
        ResetUndo = 0x1000000,

        /// <summary>
        ///     Allocates memory using large page support.
        ///     The size and alignment must be a multiple of the large-page minimum. To obtain this value, use the
        ///     GetLargePageMinimum function.
        /// </summary>
        LargePages = 0x20000000,

        /// <summary>
        ///     Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.
        ///     This value must be used with MEM_RESERVE and no other values.
        /// </summary>
        Physical = 0x00400000,

        /// <summary>
        ///     Allocates memory at the highest possible address. This can be slower than regular allocations, especially when
        ///     there are many allocations.
        /// </summary>
        TopDown = 0x00100000
    }
}