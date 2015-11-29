using Binarysharp.MemoryManagement.Core.Native.Enums;
using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Contains information about a simulated mouse event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        /// <summary>
        ///     The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on
        ///     the value of the <see cref="Flags" /> member.
        ///     Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels
        ///     moved.
        /// </summary>
        public int DeltaX;

        /// <summary>
        ///     The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on
        ///     the value of the <see cref="Flags" /> member.
        ///     Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels
        ///     moved.
        /// </summary>
        public int DeltaY;

        /// <summary>
        ///     An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra
        ///     information.
        /// </summary>
        public IntPtr ExtraInfo;

        /// <summary>
        ///     A set of bit flags that specify various aspects of mouse motion and button clicks.
        ///     The bits in this member can be any reasonable combination of the following values.
        ///     The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.
        ///     For example, if the left mouse button is pressed and held down, <see cref="MouseFlags.LeftDown" /> is set when the
        ///     left
        ///     button is first pressed, but not for subsequent motions. Similarly, <see cref="MouseFlags.LeftUp" /> is set only
        ///     when the button is first released.
        ///     You cannot specify both the <see cref="MouseFlags.Wheel" /> flag and either <see cref="MouseFlags.XDown" /> or
        ///     <see cref="MouseFlags.XUp" /> flags
        ///     simultaneously in the dwFlags parameter, because they both require use of the mouseData field.
        /// </summary>
        public MouseFlags Flags;

        /// <summary>
        ///     If <see cref="Flags" /> contains <see cref="MouseFlags.Wheel" />, then mouseData specifies the amount of wheel
        ///     movement.
        ///     A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that
        ///     the wheel was rotated backward, toward the user.
        ///     One wheel click is defined as WHEEL_DELTA, which is 120.
        ///     Windows Vista: If dwFlags contains <see cref="MouseFlags.HWheel" />, then dwData specifies the amount of wheel
        ///     movement.
        ///     A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was
        ///     rotated to the left.
        ///     One wheel click is defined as WHEEL_DELTA, which is 120.
        ///     If dwFlags does not contain <see cref="MouseFlags.Wheel" />, <see cref="MouseFlags.XDown" />, or
        ///     <see cref="MouseFlags.XUp" />, then mouseData should be zero.
        ///     If dwFlags contains <see cref="MouseFlags.XDown" /> or <see cref="MouseFlags.XUp" />, then mouseData specifies
        ///     which X buttons were pressed or released.
        ///     This value may be any combination of the following flags.
        ///     XBUTTON1 = 0x1
        ///     XBUTTON2 = 0x2
        /// </summary>
        public int MouseData;

        /// <summary>
        ///     The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public int Time;
    }
}