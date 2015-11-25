using System;
using Binarysharp.MemoryManagement.Core.Logging.Default;
using Binarysharp.MemoryManagement.Core.Logging.Enums;
using Binarysharp.MemoryManagement.Core.Logging.Interfaces;
using Binarysharp.MemoryManagement.Core.Managment;

namespace Binarysharp.MemoryManagement.Core.Logging
{
    /// <summary>
    ///     Class providing tools to manage a set of <see cref="IManagedLog" /> members. This class cannot be inherited.
    /// </summary>
    public class LogManager : SafeManager<IManagedLog>
    {
        #region Constructors, Destructors
        /// <summary>
        ///     Prevents a default instance of the <see cref="LogManager" /> class from being created.
        /// </summary>
        public LogManager() : base(new DebugLog())
        {
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     Gets the <see cref="IManagedLog" /> with the specified updater name.
        /// </summary>
        /// <param name="updaterName">Name of the ManagedLog .</param>
        /// <returns>A ManagedLog.</returns>
        public IManagedLog this[string updaterName] => InternalItems[updaterName];
        #endregion

        /// <summary>
        ///     Logs the text to all logs in the manager as a 'info' log type.
        /// </summary>
        /// <param name="text">The text to be logged.</param>
        public void LogInfo(string text)
        {
            foreach (var logReader in InternalItems.Values)
                logReader.LogInfo(text);
        }

        /// <summary>
        ///     Logs the text to all logs in the manager as a 'warning' log type.
        /// </summary>
        /// <param name="text">The text to be logged.</param>
        public void LogWarning(string text)
        {
            foreach (var logReader in InternalItems.Values)
                logReader.LogWarning(text);
        }

        /// <summary>
        ///     Logs the text to all logs in the manager as a 'error' log type.
        /// </summary>
        /// <param name="text">The text to be logged.</param>
        public void LogError(string text)
        {
            foreach (var logReader in InternalItems.Values)
                logReader.LogError(text);
        }

        /// <summary>
        ///     Logs the text to all logs in the manager as a 'normal' log type.
        /// </summary>
        /// <param name="text">The text to be logged.</param>
        public void LogNormal(string text)
        {
            foreach (var logReader in InternalItems.Values)
                logReader.LogNormal(text);
        }

        /// <summary>
        ///     Gets the logType instance of a given type and adds it to the log manager.
        /// </summary>
        /// <param name="logType">The type of log to get.</param>
        /// <param name="name">The name.</param>
        /// <param name="enableRightAway">If the log should be enabled right away automatically.</param>
        /// <returns>ILog.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void AddLogger(LoggerType logType, string name, bool enableRightAway)
        {
            switch (logType)
            {
                case LoggerType.Console:
                    InternalItems[name] = new ConsoleLog();
                    break;
                case LoggerType.Debug:
                    InternalItems[name] = new DebugLog();
                    break;
                case LoggerType.File:
                    InternalItems[name] = FileLog.Create(name, name, "Logs", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (enableRightAway)
            {
                InternalItems[name].Enable();
            }
        }

        /// <summary>
        ///     Logs the specified message.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        /// <param name="type">T[Optinal] the type of message being sent. Default is normal.</param>
        public void LogMessage(string message, MessageType type = MessageType.Normal)
        {
            switch (type)
            {
                case MessageType.Normal:
                    LogNormal(message);
                    break;
                case MessageType.Info:
                    LogNormal(message);
                    break;
                case MessageType.Error:
                    LogError(message);
                    break;
                case MessageType.Warn:
                    LogWarning(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}