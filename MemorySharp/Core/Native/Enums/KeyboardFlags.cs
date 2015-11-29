using Binarysharp.MemoryManagement.Core.Native.Structs;
using System;

namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The keyboard flags list.
    /// </summary>
    [Flags]
    public enum KeyboardFlags
    {
        /// <summary>
        ///     If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
        /// </summary>
        ExtendedKey = 1,

        /// <summary>
        ///     If specified, the key is being released. If not specified, the key is being pressed.
        /// </summary>
        KeyUp = 2,

        /// <summary>
        ///     If specified, <see cref="KeyboardInput.ScanCode" /> identifies the key and <see cref="KeyboardInput.VirtualKey" />
        ///     is ignored.
        /// </summary>
        ScanCode = 8,

        /// <summary>
        ///     If specified, the system synthesizes a VK_PACKET keystroke. The <see cref="KeyboardInput.VirtualKey" /> parameter
        ///     must be zero.
        ///     This flag can only be combined with the KEYEVENTF_KEYUP flag.
        /// </summary>
        Unicode = 4
    }
}