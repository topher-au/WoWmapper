namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The type of process information to be retrieved.
    /// </summary>
    public enum ProcessInformationClass
    {
        /// <summary>
        ///     Retrieves a pointer to a PEB structure that can be used to determine whether the specified process is being
        ///     debugged,
        ///     and a unique value used by the system to identify the specified process.
        /// </summary>
        ProcessBasicInformation = 0x0,

        /// <summary>
        ///     Retrieves a DWORD_PTR value that is the port number of the debugger for the process.
        ///     A nonzero value indicates that the process is being run under the control of a ring 3 debugger.
        /// </summary>
        ProcessDebugPort = 0x7,

        /// <summary>
        ///     Determines whether the process is running in the WOW64 environment (WOW64 is the x86 emulator that allows
        ///     Win32-based applications to run on 64-bit Windows).
        /// </summary>
        ProcessWow64Information = 0x1A,

        /// <summary>
        ///     Retrieves a UNICODE_STRING value containing the name of the image file for the process.
        /// </summary>
        ProcessImageFileName = 0x1B
    }
}