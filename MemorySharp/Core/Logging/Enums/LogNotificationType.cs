namespace Binarysharp.MemoryManagement.Core.Logging.Enums
{
    /// <summary>
    ///     Enum LogNotificationType.
    /// </summary>
    public enum LogNotificationType
    {
        /// <summary>Users will not be notified, exceptions will be automatically logged to the registered loggers</summary>
        Silent,

        /// <summary>
        ///     Users will be notified an exception has occurred, exceptions will be automatically logged to the registered
        ///     loggers
        /// </summary>
        Inform,

        /// <summary>Users will be notified an exception has occurred and will be asked if they want the exception logged</summary>
        Ask
    }
}