using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     Memory-type options list.
    /// </summary>
    [Flags]
    public enum MemoryTypeFlags
    {
        /// <summary>
        ///     This value is not officially present in the Microsoft's enumeration but can occur after testing.
        /// </summary>
        None = 0x0,

        /// <summary>
        ///     Indicates that the memory pages within the region are mapped into the view of an image section.
        /// </summary>
        Image = 0x1000000,

        /// <summary>
        ///     Indicates that the memory pages within the region are mapped into the view of a section.
        /// </summary>
        Mapped = 0x40000,

        /// <summary>
        ///     Indicates that the memory pages within the region are private (that is, not shared by other processes).
        /// </summary>
        Private = 0x20000
    }
}