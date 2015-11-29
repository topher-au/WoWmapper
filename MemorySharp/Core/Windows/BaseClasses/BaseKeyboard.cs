using Binarysharp.MemoryManagement.Core.Native.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Binarysharp.MemoryManagement.Core.Windows.BaseClasses
{
    /// <summary>
    ///     Abstract class defining a virtual keyboard.
    ///     <remarks>
    ///         Credits for this class goes to: ZenLulz aka Jämes Ménétrey. Feel free to check his products out at
    ///         http://binarysharp.com .
    ///     </remarks>
    /// </summary>
    public abstract class BaseKeyboard
    {
        #region Fields, Private Properties

        /// <summary>
        ///     The collection storing the current pressed keys.
        /// </summary>
        protected static readonly List<Tuple<IntPtr, Keys>> PressedKeys = new List<Tuple<IntPtr, Keys>>();

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of a child of the <see cref="BaseKeyboard" /> class.
        /// </summary>
        /// <param name="windowHandle">The processHandle to of the window.</param>
        protected BaseKeyboard(IntPtr windowHandle)
        {
            // Save the parameter
            Handle = windowHandle;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the safe processHandle.
        /// </summary>
        /// <value>The safe processHandle.</value>
        protected IntPtr Handle { get; }

        #endregion Public Properties, Indexers

        /// <summary>
        ///     Presses the specified virtual key to the window.
        /// </summary>
        /// <param name="key">The virtual key to press.</param>
        public abstract void Press(Keys key);

        /// <summary>
        ///     Writes the specified character to the window.
        /// </summary>
        /// <param name="character">The character to write.</param>
        public abstract void Write(char character);

        /// <summary>
        ///     Releases the specified virtual key to the window.
        /// </summary>
        /// <param name="key">The virtual key to release.</param>
        public virtual void Release(Keys key)
        {
            // Create the tuple
            var tuple = Tuple.Create(Handle, key);

            // If the key is pressed with an interval
            if (PressedKeys.Contains(tuple))
                PressedKeys.Remove(tuple);
        }

        /// <summary>
        ///     Presses the specified virtual key to the window at a specified interval.
        /// </summary>
        /// <param name="key">The virtual key to press.</param>
        /// <param name="interval">The interval between the key activations.</param>
        public void Press(Keys key, TimeSpan interval)
        {
            // Create the tuple
            var tuple = Tuple.Create(Handle, key);

            // If the key is already pressed
            if (PressedKeys.Contains(tuple))
                return;

            // Add the key to the collection
            PressedKeys.Add(tuple);
            // Start a new task to press the key at the specified interval
            Task.Run(async () =>
                           {
                               // While the key must be pressed
                               while (PressedKeys.Contains(tuple))
                               {
                                   // Press the key
                                   Press(key);
                                   // Wait the interval
                                   await Task.Delay(interval);
                               }
                           });
        }

        /// <summary>
        ///     Presses and releaes the specified virtual key to the window.
        /// </summary>
        /// <param name="key">The virtual key to press and release.</param>
        public void PressRelease(Keys key)
        {
            Press(key);
            Thread.Sleep(10);
            Release(key);
        }

        /// <summary>
        ///     Writes the text representation of the specified array of objects to the window using the specified format
        ///     information.
        /// </summary>
        /// <param name="text">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(string text, params object[] args)
        {
            foreach (var character in string.Format(text, args))
            {
                Write(character);
            }
        }

        /// <summary>
        ///     Places (posts) a message in the message queue associated with the thread that created the window and returns
        ///     without waiting for the thread to process the message.
        /// </summary>
        /// <param name="message">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        public void PostMessage(WindowsMessages message, UIntPtr wParam, UIntPtr lParam)
        {
            WindowCore.PostMessage(Handle, message, wParam, lParam);
        }

        /// <summary>
        ///     Places (posts) a message in the message queue associated with the thread that created the window and returns
        ///     without waiting for the thread to process the message.
        /// </summary>
        /// <param name="message">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        public void PostMessage(uint message, UIntPtr wParam, UIntPtr lParam)
        {
            WindowCore.PostMessage(Handle, message, wParam, lParam);
        }

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public IntPtr SendMessage(WindowsMessages message, UIntPtr wParam, IntPtr lParam)
        {
            return WindowCore.SendMessage(Handle, message, wParam, lParam);
        }

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public IntPtr SendMessage(uint message, UIntPtr wParam, IntPtr lParam)
        {
            return WindowCore.SendMessage(Handle, message, wParam, lParam);
        }
    }
}