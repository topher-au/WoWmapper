using System.Threading;

namespace Binarysharp.MemoryManagement.Core.Windows.BaseClasses
{
    /// <summary>
    ///     Abstract class defining a virtual mouse.
    ///     <remarks>
    ///         Credits for this class goes to: ZenLulz aka Jämes Ménétrey. Feel free to check his products out at
    ///         http://binarysharp.com .
    ///     </remarks>
    /// </summary>
    public abstract class BaseMouse
    {
        /// <summary>
        ///     Moves the cursor at the specified coordinate.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        protected abstract void MoveToAbsolute(int x, int y);

        /// <summary>
        ///     Presses the left button of the mouse at the current cursor position.
        /// </summary>
        public abstract void PressLeft();

        /// <summary>
        ///     Presses the middle button of the mouse at the current cursor position.
        /// </summary>
        public abstract void PressMiddle();

        /// <summary>
        ///     Presses the right button of the mouse at the current cursor position.
        /// </summary>
        public abstract void PressRight();

        /// <summary>
        ///     Releases the left button of the mouse at the current cursor position.
        /// </summary>
        public abstract void ReleaseLeft();

        /// <summary>
        ///     Releases the middle button of the mouse at the current cursor position.
        /// </summary>
        public abstract void ReleaseMiddle();

        /// <summary>
        ///     Releases the right button of the mouse at the current cursor position.
        /// </summary>
        public abstract void ReleaseRight();

        /// <summary>
        ///     Scrolls horizontally using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="delta">The amount of wheel movement.</param>
        public abstract void ScrollHorizontally(int delta = 120);

        /// <summary>
        ///     Scrolls vertically using the wheel of the mouse at the current cursor position.
        /// </summary>
        /// <param name="delta">The amount of wheel movement.</param>
        public abstract void ScrollVertically(int delta = 120);

        /// <summary>
        ///     Clicks the left button of the mouse at the current cursor position.
        /// </summary>
        public void ClickLeft()
        {
            PressLeft();
            ReleaseLeft();
        }

        /// <summary>
        ///     Clicks the middle button of the mouse at the current cursor position.
        /// </summary>
        public void ClickMiddle()
        {
            PressMiddle();
            ReleaseMiddle();
        }

        /// <summary>
        ///     Clicks the right button of the mouse at the current cursor position.
        /// </summary>
        public void ClickRight()
        {
            PressRight();
            ReleaseRight();
        }

        /// <summary>
        ///     Double clicks the left button of the mouse at the current cursor position.
        /// </summary>
        public void DoubleClickLeft()
        {
            ClickLeft();
            Thread.Sleep(10);
            ClickLeft();
        }

        /// <summary>
        ///     Moves the cursor at the specified coordinate from the position.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        public virtual void MoveTo(int x, int y)
        {
            MoveToAbsolute(x, y);
        }
    }
}