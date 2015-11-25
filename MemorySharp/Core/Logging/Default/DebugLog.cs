using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Binarysharp.MemoryManagement.Core.Logging.Interfaces;

#pragma warning disable 1591

namespace Binarysharp.MemoryManagement.Core.Logging.Default
{
    [SuppressMessage("ReSharper", "InvocationIsSkipped")]
    public class DebugLog : IManagedLog
    {
        #region Public Properties, Indexers
        /// <summary>
        ///     Gets a value indicating whether the element is disposed.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        public bool MustBeDisposed { get; set; }

        /// <summary>
        ///     States if the element is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     The name of the element.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarning(string message)
        {
            if (!IsEnabled)
                return;
            Debug.WriteLine($"{"[LogWarning]["}{DateTime.Now}{"]: "}{message}");
        }


        /// <summary>
        ///     Logs the normal.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogNormal(string message)
        {
            if (!IsEnabled)
                return;
            Debug.WriteLine($"{"[LogNormal]["}{DateTime.Now}{"]: "}{message}");
        }


        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogError(string message)
        {
            if (!IsEnabled)
                return;
            Debug.WriteLine($"{"[LogError]["}{DateTime.Now}{"]: "}{message}");
        }

        /// <summary>
        ///     Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInfo(string message)
        {
            if (!IsEnabled)
                return;
            Debug.WriteLine($"{"[LogInfo]["}{DateTime.Now}{"]: "}{message}");
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (!MustBeDisposed) return;
            if (MustBeDisposed)
            {
                IsEnabled = false;
                IsDisposed = true;
            }
        }

        /// <summary>
        ///     Disables the element.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
        }

        /// <summary>
        ///     Enables the element.
        /// </summary>
        public void Enable()
        {
            IsDisposed = false;
            MustBeDisposed = true;
            IsEnabled = true;
        }
        #endregion
    }
}