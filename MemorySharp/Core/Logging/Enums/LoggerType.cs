namespace Binarysharp.MemoryManagement.Core.Logging.Enums
{
    /// <summary>
    ///     Enum that defines the different log instances that can be used.
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        ///     The console logger which writes its logs to the system console.
        /// </summary>
        Console,

        /// <summary>
        ///     The file logger which writes its logs to a text file in the applications folder.
        /// </summary>
        File,

        /// <summary>
        ///     The debug logger which writes its logs to the debug console.
        /// </summary>
        Debug
    }
}