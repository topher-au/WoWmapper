using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     The <see cref="Rectangle" /> structure defines the coordinates of the upper-left and lower-right corners of a
    ///     rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        /// <summary>
        ///     The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom;

        /// <summary>
        ///     The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left;

        /// <summary>
        ///     The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right;

        /// <summary>
        ///     The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        ///     Gets or sets the height of the element.
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = Top + value; }
        }

        /// <summary>
        ///     Gets or sets the width of the element.
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
            set { Right = Left + value; }
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Left = {Left} Top = {Top} Height = {Height} Width = {Width}";
        }
    }
}