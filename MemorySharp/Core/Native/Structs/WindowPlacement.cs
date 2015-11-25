using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement.Core.Native.Enums;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Contains information about the placement of a window on the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        /// <summary>
        ///     Specifies flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public int Flags;

        /// <summary>
        ///     The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set
        ///     this member to sizeof(WINDOWPLACEMENT).
        /// </summary>
        public int Length;

        /// <summary>
        ///     The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public Point MaxPosition;

        /// <summary>
        ///     The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public Point MinPosition;

        /// <summary>
        ///     The window's coordinates when the window is in the restored position.
        /// </summary>
        public Rectangle NormalPosition;

        /// <summary>
        ///     The current show state of the window.
        /// </summary>
        public WindowStates ShowCmd;
    }
}