namespace Binarysharp.MemoryManagement.Core.Hooks.WndProc.Enums
{
    /// <summary>
    ///     Usermessage used for the <see cref="WindowHook" /> class.
    /// </summary>
    public enum UserMessage
    {
        /// <summary>
        ///     Used as the message to invoke <code>Engine.StartUp()</code> method.
        /// </summary>
        StartUp,

        /// <summary>
        ///     Used as the message to invoke <code>Engine.ShutDown()</code> method.
        /// </summary>
        ShutDown
    }
}