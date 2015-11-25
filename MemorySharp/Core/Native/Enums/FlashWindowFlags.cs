using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     Flash window flags list.
    /// </summary>
    [Flags]
    public enum FlashWindowFlags
    {
        /// <summary>
        ///     Flash both the window caption and taskbar button. This is equivalent to setting the <see cref="Caption" /> |
        ///     <see cref="Tray" /> flags.
        /// </summary>
        All = 0x3,

        /// <summary>
        ///     Flash the window caption.
        /// </summary>
        Caption = 0x1,

        /// <summary>
        ///     Stop flashing. The system restores the window to its original state.
        /// </summary>
        Stop = 0x0,

        /// <summary>
        ///     Flash continuously, until the <see cref="Stop" /> flag is set.
        /// </summary>
        Timer = 0x4,

        /// <summary>
        ///     Flash continuously until the window comes to the foreground.
        /// </summary>
        TimerNoForeground = 0x0C,

        /// <summary>
        ///     Flash the taskbar button.
        /// </summary>
        Tray = 0x2
    }
}