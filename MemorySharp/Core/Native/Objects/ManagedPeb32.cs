using System;
using Binarysharp.MemoryManagement.Core.Memory;
using Binarysharp.MemoryManagement.Core.Memory.Objects;
using Binarysharp.MemoryManagement.Core.Native.Enums;

namespace Binarysharp.MemoryManagement.Core.Native.Objects
{
    /// <summary>
    ///     Class representing the ProcessUpdateData Environment Block of a remote process.
    /// </summary>
    public class ManagedPeb32 : ProxyPointer
    {
        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="ManagedPeb32" /> class.
        /// </summary>
        /// <param name="handle">The handle of the process.</param>
        public ManagedPeb32(IntPtr handle)
            : base(
                handle,
                ExternalMemoryCore.NtQueryInformationProcess(handle).PebBaseAddress)
        {
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     Gets or sets the inherited address space.
        /// </summary>
        /// <value>The inherited address space.</value>
        public byte InheritedAddressSpace
        {
            get { return Read<byte>(PebStructure.InheritedAddressSpace); }
            set { Write(PebStructure.InheritedAddressSpace, value); }
        }

        /// <summary>
        ///     Gets or sets the read image file execute options.
        /// </summary>
        /// <value>The read image file execute options.</value>
        public byte ReadImageFileExecOptions
        {
            get { return Read<byte>(PebStructure.ReadImageFileExecOptions); }
            set { Write(PebStructure.ReadImageFileExecOptions, value); }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [being debugged].
        /// </summary>
        /// <value><c>true</c> if [being debugged]; otherwise, <c>false</c>.</value>
        public bool BeingDebugged
        {
            get { return Read<bool>(PebStructure.BeingDebugged); }
            set { Write(PebStructure.BeingDebugged, value); }
        }

        /// <summary>
        ///     Gets or sets the spare bool.
        /// </summary>
        /// <value>The spare bool.</value>
        public byte SpareBool
        {
            get { return Read<byte>(PebStructure.SpareBool); }
            set { Write(PebStructure.SpareBool, value); }
        }

        /// <summary>
        ///     Gets or sets the mutant.
        /// </summary>
        /// <value>The mutant.</value>
        public IntPtr Mutant
        {
            get { return Read<IntPtr>(PebStructure.Mutant); }
            set { Write(PebStructure.Mutant, value); }
        }

        /// <summary>
        ///     Gets or sets the LDR.
        /// </summary>
        /// <value>The LDR.</value>
        public IntPtr Ldr
        {
            get { return Read<IntPtr>(PebStructure.Ldr); }
            set { Write(PebStructure.Ldr, value); }
        }

        /// <summary>
        ///     Gets or sets the process parameters.
        /// </summary>
        /// <value>The process parameters.</value>
        public IntPtr ProcessParameters
        {
            get { return Read<IntPtr>(PebStructure.ProcessParameters); }
            set { Write(PebStructure.ProcessParameters, value); }
        }

        /// <summary>
        ///     Gets or sets the sub system data.
        /// </summary>
        /// <value>The sub system data.</value>
        public IntPtr SubSystemData
        {
            get { return Read<IntPtr>(PebStructure.SubSystemData); }
            set { Write(PebStructure.SubSystemData, value); }
        }

        /// <summary>
        ///     Gets or sets the process heap.
        /// </summary>
        /// <value>The process heap.</value>
        public IntPtr ProcessHeap
        {
            get { return Read<IntPtr>(PebStructure.ProcessHeap); }
            set { Write(PebStructure.ProcessHeap, value); }
        }

        /// <summary>
        ///     Gets or sets the fast peb lock.
        /// </summary>
        /// <value>The fast peb lock.</value>
        public IntPtr FastPebLock
        {
            get { return Read<IntPtr>(PebStructure.FastPebLock); }
            set { Write(PebStructure.FastPebLock, value); }
        }

        /// <summary>
        ///     Gets or sets the fast peb lock routine.
        /// </summary>
        /// <value>The fast peb lock routine.</value>
        public IntPtr FastPebLockRoutine
        {
            get { return Read<IntPtr>(PebStructure.FastPebLockRoutine); }
            set { Write(PebStructure.FastPebLockRoutine, value); }
        }

        /// <summary>
        ///     Gets or sets the fast peb unlock routine.
        /// </summary>
        /// <value>The fast peb unlock routine.</value>
        public IntPtr FastPebUnlockRoutine
        {
            get { return Read<IntPtr>(PebStructure.FastPebUnlockRoutine); }
            set { Write(PebStructure.FastPebUnlockRoutine, value); }
        }

        /// <summary>
        ///     Gets or sets the environment update count.
        /// </summary>
        /// <value>The environment update count.</value>
        public IntPtr EnvironmentUpdateCount
        {
            get { return Read<IntPtr>(PebStructure.EnvironmentUpdateCount); }
            set { Write(PebStructure.EnvironmentUpdateCount, value); }
        }

        /// <summary>
        ///     Gets or sets the kernel callback table.
        /// </summary>
        /// <value>The kernel callback table.</value>
        public IntPtr KernelCallbackTable
        {
            get { return Read<IntPtr>(PebStructure.KernelCallbackTable); }
            set { Write(PebStructure.KernelCallbackTable, value); }
        }

        /// <summary>
        ///     Gets or sets the system reserved.
        /// </summary>
        /// <value>The system reserved.</value>
        public int SystemReserved
        {
            get { return Read<int>(PebStructure.SystemReserved); }
            set { Write(PebStructure.SystemReserved, value); }
        }

        /// <summary>
        ///     Gets or sets the atl thunk s list PTR32.
        /// </summary>
        /// <value>The atl thunk s list PTR32.</value>
        public int AtlThunkSListPtr32
        {
            get { return Read<int>(PebStructure.AtlThunkSListPtr32); }
            set { Write(PebStructure.AtlThunkSListPtr32, value); }
        }

        /// <summary>
        ///     Gets or sets the free list.
        /// </summary>
        /// <value>The free list.</value>
        public IntPtr FreeList
        {
            get { return Read<IntPtr>(PebStructure.FreeList); }
            set { Write(PebStructure.FreeList, value); }
        }

        /// <summary>
        ///     Gets or sets the TLS expansion counter.
        /// </summary>
        /// <value>The TLS expansion counter.</value>
        public IntPtr TlsExpansionCounter
        {
            get { return Read<IntPtr>(PebStructure.TlsExpansionCounter); }
            set { Write(PebStructure.TlsExpansionCounter, value); }
        }

        /// <summary>
        ///     Gets or sets the TLS bitmap.
        /// </summary>
        /// <value>The TLS bitmap.</value>
        public IntPtr TlsBitmap
        {
            get { return Read<IntPtr>(PebStructure.TlsBitmap); }
            set { Write(PebStructure.TlsBitmap, value); }
        }

        /// <summary>
        ///     Gets or sets the TLS bitmap bits.
        /// </summary>
        /// <value>The TLS bitmap bits.</value>
        public long TlsBitmapBits
        {
            get { return Read<long>(PebStructure.TlsBitmapBits); }
            set { Write(PebStructure.TlsBitmapBits, value); }
        }

        /// <summary>
        ///     Gets or sets the read only shared memory base.
        /// </summary>
        /// <value>The read only shared memory base.</value>
        public IntPtr ReadOnlySharedMemoryBase
        {
            get { return Read<IntPtr>(PebStructure.ReadOnlySharedMemoryBase); }
            set { Write(PebStructure.ReadOnlySharedMemoryBase, value); }
        }

        /// <summary>
        ///     Gets or sets the read only shared memory heap.
        /// </summary>
        /// <value>The read only shared memory heap.</value>
        public IntPtr ReadOnlySharedMemoryHeap
        {
            get { return Read<IntPtr>(PebStructure.ReadOnlySharedMemoryHeap); }
            set { Write(PebStructure.ReadOnlySharedMemoryHeap, value); }
        }

        /// <summary>
        ///     Gets or sets the read only static server data.
        /// </summary>
        /// <value>The read only static server data.</value>
        public IntPtr ReadOnlyStaticServerData
        {
            get { return Read<IntPtr>(PebStructure.ReadOnlyStaticServerData); }
            set { Write(PebStructure.ReadOnlyStaticServerData, value); }
        }

        /// <summary>
        ///     Gets or sets the ANSI code page data.
        /// </summary>
        /// <value>The ANSI code page data.</value>
        public IntPtr AnsiCodePageData
        {
            get { return Read<IntPtr>(PebStructure.AnsiCodePageData); }
            set { Write(PebStructure.AnsiCodePageData, value); }
        }

        /// <summary>
        ///     Gets or sets the oem code page data.
        /// </summary>
        /// <value>The oem code page data.</value>
        public IntPtr OemCodePageData
        {
            get { return Read<IntPtr>(PebStructure.OemCodePageData); }
            set { Write(PebStructure.OemCodePageData, value); }
        }

        /// <summary>
        ///     Gets or sets the unicode case table data.
        /// </summary>
        /// <value>The unicode case table data.</value>
        public IntPtr UnicodeCaseTableData
        {
            get { return Read<IntPtr>(PebStructure.UnicodeCaseTableData); }
            set { Write(PebStructure.UnicodeCaseTableData, value); }
        }

        /// <summary>
        ///     Gets or sets the number of processors.
        /// </summary>
        /// <value>The number of processors.</value>
        public int NumberOfProcessors
        {
            get { return Read<int>(PebStructure.NumberOfProcessors); }
            set { Write(PebStructure.NumberOfProcessors, value); }
        }

        /// <summary>
        ///     Gets or sets the nt global flag.
        /// </summary>
        /// <value>The nt global flag.</value>
        public long NtGlobalFlag
        {
            get { return Read<long>(PebStructure.NtGlobalFlag); }
            set { Write(PebStructure.NtGlobalFlag, value); }
        }

        /// <summary>
        ///     Gets or sets the critical section timeout.
        /// </summary>
        /// <value>The critical section timeout.</value>
        public long CriticalSectionTimeout
        {
            get { return Read<long>(PebStructure.CriticalSectionTimeout); }
            set { Write(PebStructure.CriticalSectionTimeout, value); }
        }

        /// <summary>
        ///     Gets or sets the heap segment reserve.
        /// </summary>
        /// <value>The heap segment reserve.</value>
        public IntPtr HeapSegmentReserve
        {
            get { return Read<IntPtr>(PebStructure.HeapSegmentReserve); }
            set { Write(PebStructure.HeapSegmentReserve, value); }
        }

        /// <summary>
        ///     Gets or sets the heap segment commit.
        /// </summary>
        /// <value>The heap segment commit.</value>
        public IntPtr HeapSegmentCommit
        {
            get { return Read<IntPtr>(PebStructure.HeapSegmentCommit); }
            set { Write(PebStructure.HeapSegmentCommit, value); }
        }

        /// <summary>
        ///     Gets or sets the heap de commit total free threshold.
        /// </summary>
        /// <value>The heap de commit total free threshold.</value>
        public IntPtr HeapDeCommitTotalFreeThreshold
        {
            get { return Read<IntPtr>(PebStructure.HeapDeCommitTotalFreeThreshold); }
            set { Write(PebStructure.HeapDeCommitTotalFreeThreshold, value); }
        }

        /// <summary>
        ///     Gets or sets the heap de commit free block threshold.
        /// </summary>
        /// <value>The heap de commit free block threshold.</value>
        public IntPtr HeapDeCommitFreeBlockThreshold
        {
            get { return Read<IntPtr>(PebStructure.HeapDeCommitFreeBlockThreshold); }
            set { Write(PebStructure.HeapDeCommitFreeBlockThreshold, value); }
        }

        /// <summary>
        ///     Gets or sets the number of heaps.
        /// </summary>
        /// <value>The number of heaps.</value>
        public int NumberOfHeaps
        {
            get { return Read<int>(PebStructure.NumberOfHeaps); }
            set { Write(PebStructure.NumberOfHeaps, value); }
        }

        /// <summary>
        ///     Gets or sets the maximum number of heaps.
        /// </summary>
        /// <value>The maximum number of heaps.</value>
        public int MaximumNumberOfHeaps
        {
            get { return Read<int>(PebStructure.MaximumNumberOfHeaps); }
            set { Write(PebStructure.MaximumNumberOfHeaps, value); }
        }

        /// <summary>
        ///     Gets or sets the process heaps.
        /// </summary>
        /// <value>The process heaps.</value>
        public IntPtr ProcessHeaps
        {
            get { return Read<IntPtr>(PebStructure.ProcessHeaps); }
            set { Write(PebStructure.ProcessHeaps, value); }
        }

        /// <summary>
        ///     Gets or sets the GDI shared handle table.
        /// </summary>
        /// <value>The GDI shared handle table.</value>
        public IntPtr GdiSharedHandleTable
        {
            get { return Read<IntPtr>(PebStructure.GdiSharedHandleTable); }
            set { Write(PebStructure.GdiSharedHandleTable, value); }
        }

        /// <summary>
        ///     Gets or sets the process starter helper.
        /// </summary>
        /// <value>The process starter helper.</value>
        public IntPtr ProcessStarterHelper
        {
            get { return Read<IntPtr>(PebStructure.ProcessStarterHelper); }
            set { Write(PebStructure.ProcessStarterHelper, value); }
        }

        /// <summary>
        ///     Gets or sets the GDI dc attribute list.
        /// </summary>
        /// <value>The GDI dc attribute list.</value>
        public IntPtr GdiDcAttributeList
        {
            get { return Read<IntPtr>(PebStructure.GdiDcAttributeList); }
            set { Write(PebStructure.GdiDcAttributeList, value); }
        }

        /// <summary>
        ///     Gets or sets the loader lock.
        /// </summary>
        /// <value>The loader lock.</value>
        public IntPtr LoaderLock
        {
            get { return Read<IntPtr>(PebStructure.LoaderLock); }
            set { Write(PebStructure.LoaderLock, value); }
        }

        /// <summary>
        ///     Gets or sets the os major version.
        /// </summary>
        /// <value>The os major version.</value>
        public int OsMajorVersion
        {
            get { return Read<int>(PebStructure.OsMajorVersion); }
            set { Write(PebStructure.OsMajorVersion, value); }
        }

        /// <summary>
        ///     Gets or sets the os minor version.
        /// </summary>
        /// <value>The os minor version.</value>
        public int OsMinorVersion
        {
            get { return Read<int>(PebStructure.OsMinorVersion); }
            set { Write(PebStructure.OsMinorVersion, value); }
        }

        /// <summary>
        ///     Gets or sets the os build number.
        /// </summary>
        /// <value>The os build number.</value>
        public ushort OsBuildNumber
        {
            get { return Read<ushort>(PebStructure.OsBuildNumber); }
            set { Write(PebStructure.OsBuildNumber, value); }
        }

        /// <summary>
        ///     Gets or sets the os CSD version.
        /// </summary>
        /// <value>The os CSD version.</value>
        public ushort OsCsdVersion
        {
            get { return Read<ushort>(PebStructure.OsCsdVersion); }
            set { Write(PebStructure.OsCsdVersion, value); }
        }

        /// <summary>
        ///     Gets or sets the os platform identifier.
        /// </summary>
        /// <value>The os platform identifier.</value>
        public int OsPlatformId
        {
            get { return Read<int>(PebStructure.OsPlatformId); }
            set { Write(PebStructure.OsPlatformId, value); }
        }

        /// <summary>
        ///     Gets or sets the image subsystem.
        /// </summary>
        /// <value>The image subsystem.</value>
        public int ImageSubsystem
        {
            get { return Read<int>(PebStructure.ImageSubsystem); }
            set { Write(PebStructure.ImageSubsystem, value); }
        }

        /// <summary>
        ///     Gets or sets the image subsystem major version.
        /// </summary>
        /// <value>The image subsystem major version.</value>
        public int ImageSubsystemMajorVersion
        {
            get { return Read<int>(PebStructure.ImageSubsystemMajorVersion); }
            set { Write(PebStructure.ImageSubsystemMajorVersion, value); }
        }

        /// <summary>
        ///     Gets or sets the image subsystem minor version.
        /// </summary>
        /// <value>The image subsystem minor version.</value>
        public IntPtr ImageSubsystemMinorVersion
        {
            get { return Read<IntPtr>(PebStructure.ImageSubsystemMinorVersion); }
            set { Write(PebStructure.ImageSubsystemMinorVersion, value); }
        }

        /// <summary>
        ///     Gets or sets the image process affinity mask.
        /// </summary>
        /// <value>The image process affinity mask.</value>
        public IntPtr ImageProcessAffinityMask
        {
            get { return Read<IntPtr>(PebStructure.ImageProcessAffinityMask); }
            set { Write(PebStructure.ImageProcessAffinityMask, value); }
        }

        /// <summary>
        ///     Gets or sets the GDI handle buffer.
        /// </summary>
        /// <value>The GDI handle buffer.</value>
        public IntPtr[] GdiHandleBuffer
        {
            get { return ReadArray<IntPtr>(PebStructure.GdiHandleBuffer, 0x22); }
            set { Write(PebStructure.GdiHandleBuffer, value); }
        }

        /// <summary>
        ///     Gets or sets the post process initialize routine.
        /// </summary>
        /// <value>The post process initialize routine.</value>
        public IntPtr PostProcessInitRoutine
        {
            get { return Read<IntPtr>(PebStructure.PostProcessInitRoutine); }
            set { Write(PebStructure.PostProcessInitRoutine, value); }
        }

        /// <summary>
        ///     Gets or sets the TLS expansion bitmap.
        /// </summary>
        /// <value>The TLS expansion bitmap.</value>
        public IntPtr TlsExpansionBitmap
        {
            get { return Read<IntPtr>(PebStructure.TlsExpansionBitmap); }
            set { Write(PebStructure.TlsExpansionBitmap, value); }
        }

        /// <summary>
        ///     Gets or sets the TLS expansion bitmap bits.
        /// </summary>
        /// <value>The TLS expansion bitmap bits.</value>
        public IntPtr[] TlsExpansionBitmapBits
        {
            get { return ReadArray<IntPtr>(PebStructure.TlsExpansionBitmapBits, 0x20); }
            set { Write(PebStructure.TlsExpansionBitmapBits, value); }
        }

        /// <summary>
        ///     Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        public IntPtr SessionId
        {
            get { return Read<IntPtr>(PebStructure.SessionId); }
            set { Write(PebStructure.SessionId, value); }
        }

        /// <summary>
        ///     Gets or sets the application compat flags.
        /// </summary>
        /// <value>The application compat flags.</value>
        public long AppCompatFlags
        {
            get { return Read<long>(PebStructure.AppCompatFlags); }
            set { Write(PebStructure.AppCompatFlags, value); }
        }

        /// <summary>
        ///     Gets or sets the application compat flags user.
        /// </summary>
        /// <value>The application compat flags user.</value>
        public long AppCompatFlagsUser
        {
            get { return Read<long>(PebStructure.AppCompatFlagsUser); }
            set { Write(PebStructure.AppCompatFlagsUser, value); }
        }

        /// <summary>
        ///     Gets or sets the shim data.
        /// </summary>
        /// <value>The shim data.</value>
        public IntPtr ShimData
        {
            get { return Read<IntPtr>(PebStructure.ShimData); }
            set { Write(PebStructure.ShimData, value); }
        }

        /// <summary>
        ///     Gets or sets the application compat information.
        /// </summary>
        /// <value>The application compat information.</value>
        public IntPtr AppCompatInfo
        {
            get { return Read<IntPtr>(PebStructure.AppCompatInfo); }
            set { Write(PebStructure.AppCompatInfo, value); }
        }

        /// <summary>
        ///     Gets or sets the CSD version.
        /// </summary>
        /// <value>The CSD version.</value>
        public long CsdVersion
        {
            get { return Read<long>(PebStructure.CsdVersion); }
            set { Write(PebStructure.CsdVersion, value); }
        }

        /// <summary>
        ///     Gets or sets the activation context data.
        /// </summary>
        /// <value>The activation context data.</value>
        public IntPtr ActivationContextData
        {
            get { return Read<IntPtr>(PebStructure.ActivationContextData); }
            set { Write(PebStructure.ActivationContextData, value); }
        }

        /// <summary>
        ///     Gets or sets the process assembly storage map.
        /// </summary>
        /// <value>The process assembly storage map.</value>
        public IntPtr ProcessAssemblyStorageMap
        {
            get { return Read<IntPtr>(PebStructure.ProcessAssemblyStorageMap); }
            set { Write(PebStructure.ProcessAssemblyStorageMap, value); }
        }

        /// <summary>
        ///     Gets or sets the system default activation context data.
        /// </summary>
        /// <value>The system default activation context data.</value>
        public IntPtr SystemDefaultActivationContextData
        {
            get { return Read<IntPtr>(PebStructure.SystemDefaultActivationContextData); }
            set { Write(PebStructure.SystemDefaultActivationContextData, value); }
        }

        /// <summary>
        ///     Gets or sets the system assembly storage map.
        /// </summary>
        /// <value>The system assembly storage map.</value>
        public IntPtr SystemAssemblyStorageMap
        {
            get { return Read<IntPtr>(PebStructure.SystemAssemblyStorageMap); }
            set { Write(PebStructure.SystemAssemblyStorageMap, value); }
        }

        /// <summary>
        ///     Gets or sets the minimum stack commit.
        /// </summary>
        /// <value>The minimum stack commit.</value>
        public IntPtr MinimumStackCommit
        {
            get { return Read<IntPtr>(PebStructure.MinimumStackCommit); }
            set { Write(PebStructure.MinimumStackCommit, value); }
        }
        #endregion
    }
}