using System.Drawing;
using System.Windows.Forms;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     A class containing extension methods for windows forms rich text box's.
    /// </summary>
    public static class RichTextBoxExtensions
    {
        /// <summary>
        ///     Appends the text.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendText(this RichTextBox box, string text, Color color, params object[] args)
        {
            text = string.Format(text, args);
            if (color == Color.Empty)
            {
                box.AppendText(text);
                return;
            }

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;

            box.SelectionStart = box.TextLength;
            box.ScrollToCaret();
        }

        /// <summary>
        ///     Appends the line.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendLine(this RichTextBox box, string text, Color color, params object[] args)
        {
            if (!box.InvokeRequired)
            {
                box.AppendText("\n" + text, color == Color.Empty ? box.ForeColor : color, args);
            }
            else
            {
                box.InvokeAppendText("\n" + text, color == Color.Empty ? box.ForeColor : color, args);
            }
        }

        /// <summary>
        ///     Invokes the append text.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="args">The arguments.</param>
        private static void InvokeAppendText(this RichTextBox box, string text, Color color, params object[] args)
        {
            box.Invoke((MethodInvoker) delegate { box.AppendText(text, color, args); });
        }
    }
}