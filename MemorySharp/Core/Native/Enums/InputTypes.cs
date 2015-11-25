namespace Binarysharp.MemoryManagement.Core.Native.Enums
{
    /// <summary>
    ///     The types used in the function <see cref="SafeNativeMethods.SendInput" /> for input events.
    /// </summary>
    public enum InputTypes
    {
        /// <summary>
        ///     The mouse
        /// </summary>
        Mouse = 0,

        /// <summary>
        ///     The keyboard
        /// </summary>
        Keyboard = 1,

        /// <summary>
        ///     The hardware
        /// </summary>
        Hardware = 2
    }
}