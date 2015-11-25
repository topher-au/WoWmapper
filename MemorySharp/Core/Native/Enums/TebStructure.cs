namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The structure of the Thread Environment Block.
    /// </summary>
    /// <remarks>Tested on Windows 7 x64, 2013-03-10.</remarks>
    public enum TebStructure
    {
        /// <summary>
        ///     Current Structured Exception Handling (SEH) frame.
        /// </summary>
        CurrentSehFrame = 0x0,

        /// <summary>
        ///     The top of stack.
        /// </summary>
        TopOfStack = 0x4,

        /// <summary>
        ///     The current bottom of stack.
        /// </summary>
        BottomOfStack = 0x8,

        /// <summary>
        ///     The TEB sub system.
        /// </summary>
        SubSystemTeb = 0xC,

        /// <summary>
        ///     The fiber data.
        /// </summary>
        FiberData = 0x10,

        /// <summary>
        ///     The arbitrary data slot.
        /// </summary>
        ArbitraryDataSlot = 0x14,

        /// <summary>
        ///     The linear address of Thread Environment Block (TEB).
        /// </summary>
        Teb = 0x18,

        /// <summary>
        ///     The environment pointer.
        /// </summary>
        EnvironmentPointer = 0x1C,

        /// <summary>
        ///     The process Id.
        /// </summary>
        ProcessId = 0x20,

        /// <summary>
        ///     The current thread Id.
        /// </summary>
        ThreadId = 0x24,

        /// <summary>
        ///     The active RPC processHandle.
        /// </summary>
        RpcHandle = 0x28,

        /// <summary>
        ///     The linear address of the thread-local storage (TLS) array.
        /// </summary>
        Tls = 0x2C,

        /// <summary>
        ///     The linear address of ProcessUpdateData Environment Block (PEB).
        /// </summary>
        Peb = 0x30,

        /// <summary>
        ///     The last error number.
        /// </summary>
        LastErrorNumber = 0x34,

        /// <summary>
        ///     The count of owned critical sections.
        /// </summary>
        CriticalSectionsCount = 0x38,

        /// <summary>
        ///     The address of CSR Client Thread.
        /// </summary>
        CsrClientThread = 0x3C,

        /// <summary>
        ///     Win32 Thread Information.
        /// </summary>
        Win32ThreadInfo = 0x40,

        /// <summary>
        ///     Win32 client information (NT), user32 private data (Wine), 0x60 = LastError (Win95), 0x74 = LastError (WinME).
        ///     (length: 124 bytes)
        /// </summary>
        Win32ClientInfo = 0x44,

        /// <summary>
        ///     Reserved for Wow64. Contains a pointer to FastSysCall in Wow64.
        /// </summary>
        WoW64Reserved = 0xC0,

        /// <summary>
        ///     The current locale.
        /// </summary>
        CurrentLocale = 0xC4,

        /// <summary>
        ///     The FP Software Status Register.
        /// </summary>
        FpSoftwareStatusRegister = 0xC8,

        /// <summary>
        ///     Reserved for OS (NT), kernel32 private data (Wine). (length: 216 bytes)
        ///     herein: FS:[0x124] 4 NT Pointer to KTHREAD (ETHREAD) structure.
        /// </summary>
        SystemReserved1 = 0xCC,

        /// <summary>
        ///     The exception code.
        /// </summary>
        ExceptionCode = 0x1A4,

        /// <summary>
        ///     The activation context stack. (length: 18 bytes)
        /// </summary>
        ActivationContextStack = 0x1A8,

        /// <summary>
        ///     The spare bytes (NT), ntdll private data (Wine). (length: 24 bytes)
        /// </summary>
        SpareBytes = 0x1BC,

        /// <summary>
        ///     Reserved for OS (NT), ntdll private data (Wine). (length: 40 bytes)
        /// </summary>
        SystemReserved2 = 0x1D4,

        /// <summary>
        ///     The GDI TEB Batch (OS), vm86 private data (Wine). (length: 1248 bytes)
        /// </summary>
        GdiTebBatch = 0x1FC,

        /// <summary>
        ///     The GDI Region.
        /// </summary>
        GdiRegion = 0x6DC,

        /// <summary>
        ///     The GDI Pen.
        /// </summary>
        GdiPen = 0x6E0,

        /// <summary>
        ///     The GDI Brush.
        /// </summary>
        GdiBrush = 0x6E4,

        /// <summary>
        ///     The real process Id.
        /// </summary>
        RealProcessId = 0x6E8,

        /// <summary>
        ///     The real thread Id.
        /// </summary>
        RealThreadId = 0x6EC,

        /// <summary>
        ///     The GDI cached process processHandle.
        /// </summary>
        GdiCachedProcessHandle = 0x6F0,

        /// <summary>
        ///     The GDI client process Id (PID).
        /// </summary>
        GdiClientProcessId = 0x6F4,

        /// <summary>
        ///     The GDI client thread Id (TID).
        /// </summary>
        GdiClientThreadId = 0x6F8,

        /// <summary>
        ///     The GDI thread locale information.
        /// </summary>
        GdiThreadLocalInfo = 0x6FC,

        /// <summary>
        ///     Reserved for user application. (length: 20 bytes)
        /// </summary>
        UserReserved1 = 0x700,

        /// <summary>
        ///     Reserved for GL. (length: 1248 bytes)
        /// </summary>
        GlReserved1 = 0x714,

        /// <summary>
        ///     The last value status value.
        /// </summary>
        LastStatusValue = 0xBF4,

        /// <summary>
        ///     The static UNICODE_STRING buffer. (length: 532 bytes)
        /// </summary>
        StaticUnicodeString = 0xBF8,

        /// <summary>
        ///     The pointer to deallocation stack.
        /// </summary>
        DeallocationStack = 0xE0C,

        /// <summary>
        ///     The TLS slots, 4 byte per slot. (length: 256 bytes)
        /// </summary>
        TlsSlots = 0xE10,

        /// <summary>
        ///     The TLS links (LIST_ENTRY structure). (length 8 bytes)
        /// </summary>
        TlsLinks = 0xF10,

        /// <summary>
        ///     Virtual DOS Machine.
        /// </summary>
        Vdm = 0xF18,

        /// <summary>
        ///     Reserved for RPC.
        /// </summary>
        RpcReserved = 0xF1C,

        /// <summary>
        ///     The thread error mode (RtlSetThreadErrorMode).
        /// </summary>
        ThreadErrorMode = 0xF28
    }
}