using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Objects;
using Binarysharp.MemoryManagement.Core.Native.Structs;
using Binarysharp.MemoryManagement.Core.Windows.BaseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Binarysharp.MemoryManagement.Core.Windows.Objects
{
    /// <summary>
    ///     Class repesenting a window in the remote process.
    ///     <remarks>
    ///         Credits for this class goes to: ZenLulz aka Jämes Ménétrey. Feel free to check his products out at
    ///         http://binarysharp.com .
    ///     </remarks>
    /// </summary>
    public class ProxyWindow : IEquatable<ProxyWindow>
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProxyWindow" /> class.
        /// </summary>
        /// <param name="process">the process of the window.</param>
        /// <param name="handle">The processHandle of a window.</param>
        public ProxyWindow(Process process, IntPtr handle = default(IntPtr))
        {
            // Save the parametersrp;
            Handle = handle == (default(IntPtr)) ? process.Handle : handle;
            Handle = Process.Handle;
            Process = process;
            // Create the tools
            Keyboard = new MessageKeyboard(this);
            Mouse = new SendInputMouse(this);
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the process instance.
        /// </summary>
        /// <value>The process instance.</value>
        public Process Process { get; }

        /// <summary>
        ///     Gets all the child windows of this window.
        /// </summary>
        public IEnumerable<ProxyWindow> Children
        {
            get { return ChildrenHandles.Select(handle => new ProxyWindow(Process)); }
        }

        /// <summary>
        ///     Gets all the child window handles of this window.
        /// </summary>
        protected IEnumerable<IntPtr> ChildrenHandles => WindowCore.EnumChildWindows(Handle);

        /// <summary>
        ///     Gets the class name of the window.
        /// </summary>
        public string ClassName => WindowCore.GetClassName(Handle);

        /// <summary>
        ///     The processHandle of the window.
        /// </summary>
        /// <remarks>
        ///     The type here is not <see cref="SafeMemoryHandle" /> because a window cannot be closed by calling
        ///     <see cref="SafeNativeMethods.CloseHandle" />.
        ///     For more information, see:
        ///     http://stackoverflow.com/questions/8507307/why-cant-i-close-the-window-processHandle-in-my-code.
        /// </remarks>
        public IntPtr Handle { get; }

        /// <summary>
        ///     Gets or sets the height of the element.
        /// </summary>
        public int Height
        {
            get { return Placement.NormalPosition.Height; }
            set
            {
                var p = Placement;
                p.NormalPosition.Height = value;
                Placement = p;
            }
        }

        /// <summary>
        ///     Gets if the window is currently activated.
        /// </summary>
        public bool IsActivated => WindowCore.GetForegroundWindow() == Handle;

        /// <summary>
        ///     Tools for managing a virtual keyboard in the window.
        /// </summary>
        public BaseKeyboard Keyboard { get; set; }

        /// <summary>
        ///     Tools for managing a virtual mouse in the window.
        /// </summary>
        public BaseMouse Mouse { get; set; }

        /// <summary>
        ///     Gets or sets the placement of the window.
        /// </summary>
        public WindowPlacement Placement
        {
            get { return WindowCore.GetWindowPlacement(Handle); }
            set { WindowCore.SetWindowPlacement(Handle, value); }
        }

        /// <summary>
        ///     Gets or sets the specified window's show state.
        /// </summary>
        public WindowStates State
        {
            get { return Placement.ShowCmd; }
            set { WindowCore.ShowWindow(Handle, value); }
        }

        /// <summary>
        ///     Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get { return WindowCore.GetWindowText(Handle); }
            set { WindowCore.SetWindowText(Handle, value); }
        }

        /// <summary>
        ///     Gets the native threads from the remote process.
        /// </summary>
        private IEnumerable<ProcessThread> NativeThreads
        {
            get
            {
                Process.Refresh();
                return Process.Threads.Cast<ProcessThread>().ToList();
            }
        }

        /// <summary>
        ///     Gets the thread of the window.
        /// </summary>
        public ProcessThread Thread => GetThreadById(WindowCore.GetWindowThreadId(Handle));

        /// <summary>
        ///     Gets or sets the width of the element.
        /// </summary>
        public int Width
        {
            get { return Placement.NormalPosition.Width; }
            set
            {
                var p = Placement;
                p.NormalPosition.Width = value;
                Placement = p;
            }
        }

        /// <summary>
        ///     Gets or sets the x-coordinate of the window.
        /// </summary>
        public int X
        {
            get { return Placement.NormalPosition.Left; }
            set
            {
                var p = Placement;
                p.NormalPosition.Right = value + p.NormalPosition.Width;
                p.NormalPosition.Left = value;
                Placement = p;
            }
        }

        /// <summary>
        ///     Gets or sets the y-coordinate of the window.
        /// </summary>
        public int Y
        {
            get { return Placement.NormalPosition.Top; }
            set
            {
                var p = Placement;
                p.NormalPosition.Bottom = value + p.NormalPosition.Height;
                p.NormalPosition.Top = value;
                Placement = p;
            }
        }

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(ProxyWindow other)
        {
            return !ReferenceEquals(null, other) && Handle.Equals(other.Handle);
        }

        #endregion Interface Implementations

        private ProcessThread GetThreadById(int id)
        {
            return NativeThreads.First(thread => thread.Id == id);
        }

        /// <summary>
        ///     Activates the window.
        /// </summary>
        public void Activate()
        {
            WindowCore.SetForegroundWindow(Handle);
        }

        /// <summary>
        ///     Closes the window.
        /// </summary>
        public void Close()
        {
            PostMessage(WindowsMessages.Close, UIntPtr.Zero, UIntPtr.Zero);
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ProxyWindow)obj);
        }

        /// <summary>
        ///     Flashes the window one time. It does not change the active state of the window.
        /// </summary>
        public void Flash()
        {
            WindowCore.FlashWindow(Handle);
        }

        /// <summary>
        ///     Flashes the window. It does not change the active state of the window.
        /// </summary>
        /// <param name="count">The number of times to flash the window.</param>
        /// <param name="timeout">The rate at which the window is to be flashed.</param>
        /// <param name="flags">The flash status.</param>
        public void Flash(uint count, TimeSpan timeout, FlashWindowFlags flags = FlashWindowFlags.All)
        {
            WindowCore.FlashWindowEx(Handle, flags, count, timeout);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return 397 ^ Handle.GetHashCode();
            }
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ProxyWindow left, ProxyWindow right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ProxyWindow left, ProxyWindow right)
        {
            return !Equals(left, right);
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

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Title = {Title} ClassName = {ClassName}";
        }
    }
}