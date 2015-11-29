using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Structs;
using Binarysharp.MemoryManagement.Core.Windows.BaseClasses;

namespace Binarysharp.MemoryManagement.Core.Windows.Objects
{
    /// <summary>
    ///     Class defining a virtual mouse using the API SendInput.
    ///     <remarks>
    ///         Credits for this class goes to: ZenLulz aka Jämes Ménétrey. Feel free to check his products out at
    ///         http://binarysharp.com .
    ///     </remarks>
    /// </summary>
    public class SendInputMouse : BaseMouse
    {
        #region Fields, Private Properties

        private ProxyWindow ProxyWindow { get; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendInputMouse" /> class.
        /// </summary>
        /// <param name="proxyWindow">The proxy window instance.</param>
        public SendInputMouse(ProxyWindow proxyWindow)
        {
            ProxyWindow = proxyWindow;
        }

        #endregion Constructors, Destructors

        /// <summary>
        ///     Moves the cursor at the specified coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        protected override void MoveToAbsolute(int x, int y)
        {
            var input = CreateInput();
            input.Mouse.DeltaX = CalculateAbsoluteCoordinateX(x);
            input.Mouse.DeltaY = CalculateAbsoluteCoordinateY(y);
            input.Mouse.Flags = MouseFlags.Move | MouseFlags.Absolute;
            input.Mouse.MouseData = 0;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Moves the cursor at the specified coordinate from the position of the window.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public override void MoveTo(int x, int y)
        {
            MoveToAbsolute(ProxyWindow.X + x, ProxyWindow.Y + y);
        }

        /// <summary>
        ///     Presses the left button of the mouse at the current cursor position.
        /// </summary>
        public override void PressLeft()
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.LeftDown;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Presses the middle button of the mouse at the current cursor position.
        /// </summary>
        public override void PressMiddle()
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.MiddleDown;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Presses the right button of the mouse at the current cursor position.
        /// </summary>
        public override void PressRight()
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.RightDown;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Releases the left button of the mouse at the current cursor position.
        /// </summary>
        public override void ReleaseLeft()
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.LeftUp;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Releases the middle button of the mouse at the current cursor position.
        /// </summary>
        public override void ReleaseMiddle()
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.MiddleUp;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Releases the right button of the mouse at the current cursor position.
        /// </summary>
        public override void ReleaseRight()
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.RightUp;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Scrolls horizontally using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="delta">The amount of wheel movement.</param>
        public override void ScrollHorizontally(int delta = 120)
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.HWheel;
            input.Mouse.MouseData = delta;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Scrolls vertically using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="delta">The amount of wheel movement.</param>
        public override void ScrollVertically(int delta = 120)
        {
            var input = CreateInput();
            input.Mouse.Flags = MouseFlags.Wheel;
            input.Mouse.MouseData = delta;
            WindowCore.SendInput(input);
        }

        /// <summary>
        ///     Calculates the x-coordinate with the system metric.
        /// </summary>
        private static int CalculateAbsoluteCoordinateX(int x)
        {
            return (x * 65536) / SafeNativeMethods.GetSystemMetrics(SystemMetrics.CxScreen);
        }

        /// <summary>
        ///     Calculates the y-coordinate with the system metric.
        /// </summary>
        private static int CalculateAbsoluteCoordinateY(int y)
        {
            return (y * 65536) / SafeNativeMethods.GetSystemMetrics(SystemMetrics.CyScreen);
        }

        /// <summary>
        ///     Create an <see cref="Input" /> structure for mouse event.
        /// </summary>
        private static Input CreateInput()
        {
            return new Input(InputTypes.Mouse);
        }
    }
}