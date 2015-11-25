using System;

namespace Binarysharp.MemoryManagement.Core.Patterns.Objects
{
    /// <summary>
    ///     Contains data regarding a pattern scan result.
    /// </summary>
    public class ScanResult
    {
        #region Public Properties, Indexers
        /// <summary>
        ///     The address found.
        /// </summary>
        public IntPtr Address { get; set; }

        /// <summary>
        ///     The offset found.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     The original address found.
        /// </summary>
        public IntPtr OriginalAddress { get; set; }
        #endregion
    }
}