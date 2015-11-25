using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The mouse flags list.
    /// </summary>
    [Flags]
    public enum MouseFlags
    {
        /// <summary>
        ///     The DeltaX and DeltaY members contain normalized absolute coordinates. If the flag is not set, DeltaX and DeltaY
        ///     contain relative data
        ///     (the change in position since the last reported position). This flag can be set, or not set, regardless of what
        ///     kind of mouse or other
        ///     pointing device, if any, is connected to the system.
        /// </summary>
        Absolute = 0x8000,

        /// <summary>
        ///     The wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in MouseData.
        ///     Windows XP/2000:  This value is not supported.
        /// </summary>
        HWheel = 0x1000,

        /// <summary>
        ///     Movement occurred.
        /// </summary>
        Move = 1,

        /// <summary>
        ///     The WM_MOUSEMOVE messages will not be coalesced. The default behavior is to coalesce WM_MOUSEMOVE messages.
        ///     Windows XP/2000:  This value is not supported.
        /// </summary>
        MoveNoCoalesce = 0x2000,

        /// <summary>
        ///     The left button was pressed.
        /// </summary>
        LeftDown = 2,

        /// <summary>
        ///     The left button was released.
        /// </summary>
        LeftUp = 4,

        /// <summary>
        ///     The right button was pressed.
        /// </summary>
        RightDown = 8,

        /// <summary>
        ///     The right button was released.
        /// </summary>
        RightUp = 0x10,

        /// <summary>
        ///     The middle button was pressed.
        /// </summary>
        MiddleDown = 0x20,

        /// <summary>
        ///     The middle button was released.
        /// </summary>
        MiddleUp = 0x40,

        /// <summary>
        ///     The left button was clicked twice rapidly.
        /// </summary>
        DoubleLeftClick = 0x0203,

        /// <summary>
        ///     Maps coordinates to the entire desktop. Must be used with <see cref="Absolute" />.
        /// </summary>
        VirtualDesk = 0x4000,

        /// <summary>
        ///     The wheel was moved, if the mouse has a wheel. The amount of movement is specified in MouseData.
        /// </summary>
        Wheel = 0x800,

        /// <summary>
        ///     An X button was pressed.
        /// </summary>
        XDown = 0x80,

        /// <summary>
        ///     An X button was released.
        /// </summary>
        XUp = 0x100
    }
}