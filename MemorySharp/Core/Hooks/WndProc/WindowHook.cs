using Binarysharp.MemoryManagement.Core.Hooks.WndProc.Enums;
using Binarysharp.MemoryManagement.Core.Hooks.WndProc.Interfaces;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Core.Native;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Hooks.WndProc
{
    /// <summary>
    ///     Class containing operations and properties to hook the <code>WndProc</code>
    ///     <remarks>
    ///         All windows messages are sent to the WndProc method after getting filtered through the PreProcessMessage
    ///         method. This means we can hook this method, and intercept a custom windows message and handle it. For more
    ///         information on this method, refer to:
    ///         https://msdn.microsoft.com/en-us/library/system.windows.forms.control.wndproc(v=vs.110).aspx.
    ///     </remarks>
    /// </summary>
    public class WindowHook : INamedElement
    {
        #region Fields, Private Properties

        private int GwlWndproc { get; } = -4;
        private IntPtr WindowHandle { get; }
        private WindowProc OurCallBack { get; set; }
        private IntPtr OurCallBackAddress { get; set; }
        private IntPtr OriginalCallbackAddress { get; set; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowHook" /> class.
        /// </summary>
        /// <param name="windowHandle">A handle to the window and, indirectly, the class to which the window belongs.</param>
        /// <param name="name">The unique name representing this instance.</param>
        /// <param name="engine">The engine to use A default pulse engine is available if desired.</param>
        public WindowHook(IntPtr windowHandle, string name, ref IWindowEngine engine)
        {
            WindowHandle = windowHandle;
            Name = name;
            Engine = engine;
            IsEnabled = false;
        }

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
        ///     collection.
        /// </summary>
        ~WindowHook()
        {
            Dispose();
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the unique name that represents this instance.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets or sets the WM_USER message code for this instance. It can be defined as anything between 0x0400 and 0x7FFF.
        ///     <remarks>The default value is 0x0400.</remarks>
        ///     . This message is intercepted in the call back along with the user message.
        /// </summary>
        /// <value>The custom WM_USER message code for this instance.</value>
        public int WmUser { get; set; } = 0x0400;

        /// <summary>
        ///     Gets or sets the engine reference for this instance.
        /// </summary>
        /// <value>The engine.</value>
        public IWindowEngine Engine { get; set; }

        /// <summary>
        ///     States if the <code>WndProc</code> hook is currently enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the updateris disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        //
        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object. The
        ///     default value is true.
        /// </summary>
        public bool MustBeDisposed { get; set; } = true;

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Enables the <code>WndProc</code> hook.
        /// </summary>
        public void Enable()
        {
            if (IsEnabled)
            {
                Disable();
            }
            // Pins WndProc - will not be garbage collected.
            OurCallBack = WndProc;
            // Store the call back pointer. Storing the result is not needed, however.
            OurCallBackAddress = Marshal.GetFunctionPointerForDelegate(OurCallBack);
            // This helper method will work with x32 or x64.
            OriginalCallbackAddress = SafeNativeMethods.SetWindowLongPtr(WindowHandle, GwlWndproc, OurCallBackAddress);
            // Just to be sure.
            if (OriginalCallbackAddress == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            IsEnabled = true;
        }

        /// <summary>
        ///     Disables the <code>WndProc</code> hook.
        /// </summary>
        public void Disable()
        {
            // We have not successfully enabled the hook yet in this case, so no need to disable.
            if (OurCallBack == null)
            {
                IsEnabled = false;
                return;
            }
            // Sets the call back to the original. This helper method will work with x32 or x64.
            SafeNativeMethods.SetWindowLongPtr(WindowHandle, GwlWndproc, OriginalCallbackAddress);
            OurCallBack = null;
            IsEnabled = false;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Disable();
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Used to send the custom user message to be intercepted in the call back.
        /// </summary>
        /// <param name="message">The message to to send.</param>
        public void SendUserMessage(UserMessage message)
        {
            SafeNativeMethods.SendMessage(WindowHandle, (uint)WmUser, (UIntPtr)message, new IntPtr(0));
        }

        // The custom call back.
        private int WndProc(IntPtr hWnd, int msg, int wParam, int lParam)
        {
            // Intercept the message and handle it.
            if (msg == WmUser && HandleUserMessage((UserMessage)wParam))
            {
                // Already handled, so no need to do anything.
                return 0;
            }
            // Forward the message to the original WndProc function.
            return SafeNativeMethods.CallWindowProc(OriginalCallbackAddress, hWnd, msg, wParam, lParam);
        }

        // This handles the intercepted user messages.
        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
        private bool HandleUserMessage(UserMessage message)
        {
            switch (message)
            {
                case UserMessage.StartUp:
                    Engine.StartUp();
                    return true;

                case UserMessage.ShutDown:
                    Engine.ShutDown();
                    return true;
            }
            return false;
        }

        // Nested delegate for WindowProc.
        private delegate int WindowProc(IntPtr hWnd, int msg, int wParam, int lParam);
    }
}