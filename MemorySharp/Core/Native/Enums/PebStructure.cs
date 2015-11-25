namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The structure of the ProcessUpdateData Environment Block.
    /// </summary>
    /// <remarks>
    ///     Tested on Windows 7 x64, 2013-03-10
    ///     Source: http://blog.rewolf.pl/blog/?p=573#.UTyBo1fJL6p
    /// </remarks>
    public enum PebStructure
    {
        /// <summary>
        ///     The inherited address space
        /// </summary>
        InheritedAddressSpace = 0x0,

        /// <summary>
        ///     The read image file execute options
        /// </summary>
        ReadImageFileExecOptions = 0x1,

        /// <summary>
        ///     Gets if the process is being debugged.
        /// </summary>
        BeingDebugged = 0x2,

        /// <summary>
        ///     The spare bool
        /// </summary>
        SpareBool = 0x3,

        /// <summary>
        ///     The mutant
        /// </summary>
        Mutant = 0x4,

        /// <summary>
        ///     The image base address
        /// </summary>
        ImageBaseAddress = 0x8,

        /// <summary>
        ///     The LDR
        /// </summary>
        Ldr = 0xC,

        /// <summary>
        ///     The process parameters
        /// </summary>
        ProcessParameters = 0x10,

        /// <summary>
        ///     The sub system data
        /// </summary>
        SubSystemData = 0x14,

        /// <summary>
        ///     The process heap
        /// </summary>
        ProcessHeap = 0x18,

        /// <summary>
        ///     The fast peb lock
        /// </summary>
        FastPebLock = 0x1C,

        /// <summary>
        ///     The fast peb lock routine
        /// </summary>
        FastPebLockRoutine = 0x20,

        /// <summary>
        ///     The fast peb unlock routine
        /// </summary>
        FastPebUnlockRoutine = 0x24,

        /// <summary>
        ///     The environment update count
        /// </summary>
        EnvironmentUpdateCount = 0x28,

        /// <summary>
        ///     The kernel callback table
        /// </summary>
        KernelCallbackTable = 0x2C,

        /// <summary>
        ///     The system reserved
        /// </summary>
        SystemReserved = 0x30,

        /// <summary>
        ///     The atl thunk s list PTR32
        /// </summary>
        AtlThunkSListPtr32 = 0x34,

        /// <summary>
        ///     The free list
        /// </summary>
        FreeList = 0x38,

        /// <summary>
        ///     The TLS expansion counter
        /// </summary>
        TlsExpansionCounter = 0x3C,

        /// <summary>
        ///     The TLS bitmap
        /// </summary>
        TlsBitmap = 0x40,

        /// <summary>
        ///     Length: 8 bytes.
        /// </summary>
        TlsBitmapBits = 0x44,

        /// <summary>
        ///     The read only shared memory base
        /// </summary>
        ReadOnlySharedMemoryBase = 0x4C,

        /// <summary>
        ///     The read only shared memory heap
        /// </summary>
        ReadOnlySharedMemoryHeap = 0x50,

        /// <summary>
        ///     The read only static server data
        /// </summary>
        ReadOnlyStaticServerData = 0x54,

        /// <summary>
        ///     The ANSI code page data
        /// </summary>
        AnsiCodePageData = 0x58,

        /// <summary>
        ///     The oem code page data
        /// </summary>
        OemCodePageData = 0x5C,

        /// <summary>
        ///     The unicode case table data
        /// </summary>
        UnicodeCaseTableData = 0x60,

        /// <summary>
        ///     The number of processors
        /// </summary>
        NumberOfProcessors = 0x64,

        /// <summary>
        ///     Length: 8 bytes.
        /// </summary>
        NtGlobalFlag = 0x68,

        /// <summary>
        ///     Length: 8 bytes (LARGE_INTEGER type).
        /// </summary>
        CriticalSectionTimeout = 0x70,

        /// <summary>
        ///     The heap segment reserve
        /// </summary>
        HeapSegmentReserve = 0x78,

        /// <summary>
        ///     The heap segment commit
        /// </summary>
        HeapSegmentCommit = 0x7C,

        /// <summary>
        ///     The heap de commit total free threshold
        /// </summary>
        HeapDeCommitTotalFreeThreshold = 0x80,

        /// <summary>
        ///     The heap de commit free block threshold
        /// </summary>
        HeapDeCommitFreeBlockThreshold = 0x84,

        /// <summary>
        ///     The number of heaps
        /// </summary>
        NumberOfHeaps = 0x88,

        /// <summary>
        ///     The maximum number of heaps
        /// </summary>
        MaximumNumberOfHeaps = 0x8C,

        /// <summary>
        ///     The process heaps
        /// </summary>
        ProcessHeaps = 0x90,

        /// <summary>
        ///     The GDI shared processHandle table
        /// </summary>
        GdiSharedHandleTable = 0x94,

        /// <summary>
        ///     The process starter helper
        /// </summary>
        ProcessStarterHelper = 0x98,

        /// <summary>
        ///     The GDI dc attribute list
        /// </summary>
        GdiDcAttributeList = 0x9C,

        /// <summary>
        ///     The loader lock
        /// </summary>
        LoaderLock = 0xA0,

        /// <summary>
        ///     The os major version
        /// </summary>
        OsMajorVersion = 0xA4,

        /// <summary>
        ///     The os minor version
        /// </summary>
        OsMinorVersion = 0xA8,

        /// <summary>
        ///     Length: 2 bytes.
        /// </summary>
        OsBuildNumber = 0xAC,

        /// <summary>
        ///     Length: 2 bytes.
        /// </summary>
        OsCsdVersion = 0xAE,

        /// <summary>
        ///     The os platform identifier
        /// </summary>
        OsPlatformId = 0xB0,

        /// <summary>
        ///     The image subsystem
        /// </summary>
        ImageSubsystem = 0xB4,

        /// <summary>
        ///     The image subsystem major version
        /// </summary>
        ImageSubsystemMajorVersion = 0xB8,

        /// <summary>
        ///     The image subsystem minor version
        /// </summary>
        ImageSubsystemMinorVersion = 0xBC,

        /// <summary>
        ///     The image process affinity mask
        /// </summary>
        ImageProcessAffinityMask = 0xC0,

        /// <summary>
        ///     Length: 0x88 bytes (0x22 * sizeof(IntPtr)).
        /// </summary>
        GdiHandleBuffer = 0xC4,

        /// <summary>
        ///     The post process initialize routine
        /// </summary>
        PostProcessInitRoutine = 0x14C,

        /// <summary>
        ///     The TLS expansion bitmap
        /// </summary>
        TlsExpansionBitmap = 0x150,

        /// <summary>
        ///     Length: 0x80 bytes (0x20 * sizeof(IntPtr))
        /// </summary>
        TlsExpansionBitmapBits = 0x154,

        /// <summary>
        ///     The session identifier
        /// </summary>
        SessionId = 0x1D4,

        /// <summary>
        ///     Length: 8 bytes (LARGE_INTEGER type).
        /// </summary>
        AppCompatFlags = 0x1D8,

        /// <summary>
        ///     Length: 8 bytes (LARGE_INTEGER type).
        /// </summary>
        AppCompatFlagsUser = 0x1E0,

        /// <summary>
        ///     The shim data
        /// </summary>
        ShimData = 0x1E8,

        /// <summary>
        ///     The application compat information
        /// </summary>
        AppCompatInfo = 0x1EC,

        /// <summary>
        ///     Length: 8 bytes (UNICODE_STRING type).
        /// </summary>
        CsdVersion = 0x1F0,

        /// <summary>
        ///     The activation context data
        /// </summary>
        ActivationContextData = 0x1F8,

        /// <summary>
        ///     The process assembly storage map
        /// </summary>
        ProcessAssemblyStorageMap = 0x1FC,

        /// <summary>
        ///     The system default activation context data
        /// </summary>
        SystemDefaultActivationContextData = 0x200,

        /// <summary>
        ///     The system assembly storage map
        /// </summary>
        SystemAssemblyStorageMap = 0x204,

        /// <summary>
        ///     The minimum stack commit
        /// </summary>
        MinimumStackCommit = 0x208
    }
}