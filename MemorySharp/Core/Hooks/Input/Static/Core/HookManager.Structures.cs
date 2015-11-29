using Binarysharp.MemoryManagement.Core.Native.Structs;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Hooks.Input.Static.Core
{
    public static partial class InputHookManager
    {
        /// <summary>
        ///     The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MouseLlHookStruct
        {
            /// <summary>
            ///     Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public Point Point;

            /// <summary>
            ///     If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta.
            ///     The low-order word is reserved. A positive value indicates that the wheel was rotated forward,
            ///     away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
            ///     One wheel click is defined as WHEEL_DELTA, which is 120.
            ///     If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            ///     or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
            ///     and the low-order word is reserved. This value can be one or more of the following values. Otherwise, MouseData is
            ///     not used.
            ///     XBUTTON1
            ///     The first X button was pressed or released.
            ///     XBUTTON2
            ///     The second X button was pressed or released.
            /// </summary>
            public readonly int MouseData;

            /// <summary>
            ///     Specifies the event-injected flag. An application can use the following value to test the mouse Flags. Value
            ///     Purpose
            ///     LLMHF_INJECTED Test the event-injected flag.
            ///     0
            ///     Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///     1-15
            ///     Reserved.
            /// </summary>
            private readonly int Flags;

            /// <summary>
            ///     Specifies the Time stamp for this message.
            /// </summary>
            private readonly int Time;

            /// <summary>
            ///     Specifies extra information associated with the message.
            /// </summary>
            private readonly int ExtraInfo;
        }

        /// <summary>
        ///     The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        /// <remarks>
        ///     http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            /// <summary>
            ///     Specifies a virtual-key code. The code must be a value in the range 1 to 254.
            /// </summary>
            public readonly int VirtualKeyCode;

            /// <summary>
            ///     Specifies a hardware scan code for the key.
            /// </summary>
            public readonly int ScanCode;

            /// <summary>
            ///     Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public readonly int Flags;

            /// <summary>
            ///     Specifies the Time stamp for this message.
            /// </summary>
            private readonly int Time;

            /// <summary>
            ///     Specifies extra information associated with the message.
            /// </summary>
            private readonly int ExtraInfo;
        }
    }
}