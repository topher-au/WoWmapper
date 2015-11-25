using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Structs;

namespace Binarysharp.MemoryManagement.Core.Hooks.Input
{
    /// <summary>
    ///     Class for intercepting low level Windows mouse hooks.
    /// </summary>
    public class MouseHook : INamedElement
    {
        #region Public Delegates/Events
        /// <summary>
        ///     Function to be called when defined even occurs
        /// </summary>
        /// <param name="mouseStruct">MSLLHOOKSTRUCT mouse structure</param>
        public delegate void MouseHookCallback(MouseInput mouseStruct);

        /// <summary>
        ///     Occurs when [left button down].
        /// </summary>
        public event MouseHookCallback LeftButtonDown;

        /// <summary>
        ///     Occurs when [left button up].
        /// </summary>
        public event MouseHookCallback LeftButtonUp;

        /// <summary>
        ///     Occurs when [right button down].
        /// </summary>
        public event MouseHookCallback RightButtonDown;

        /// <summary>
        ///     Occurs when [right button up].
        /// </summary>
        public event MouseHookCallback RightButtonUp;

        /// <summary>
        ///     Occurs when [mouse move].
        /// </summary>
        public event MouseHookCallback MouseMove;

        /// <summary>
        ///     Occurs when [mouse wheel].
        /// </summary>
        public event MouseHookCallback MouseWheel;

        /// <summary>
        ///     Occurs when [double click].
        /// </summary>
        public event MouseHookCallback DoubleClick;

        /// <summary>
        ///     Occurs when [middle button down].
        /// </summary>
        public event MouseHookCallback MiddleButtonDown;

        /// <summary>
        ///     Occurs when [middle button up].
        /// </summary>
        public event MouseHookCallback MiddleButtonUp;
        #endregion

        #region Fields, Private Properties
        private const int WhMouseLl = 14;
        private MouseHookHandler HookHandler { get; set; }
        private IntPtr HookId { get; set; } = IntPtr.Zero;
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MouseHook" /> class.
        /// </summary>
        /// <param name="name">The name that represents this instance instance.</param>
        /// <param name="mustBeDisposed">
        ///     Whether the low level mouse hook must be disposed when the Garbage Collector collects the
        ///     object.
        /// </param>
        public MouseHook(string name, bool mustBeDisposed = true)
        {
            Name = name;
            MustBeDisposed = mustBeDisposed;
        }

        /// <summary>
        ///     Destructor. Unhook current hook
        /// </summary>
        ~MouseHook()
        {
            Dispose();
        }
        #endregion

        #region Public Properties, Indexers
        /// <summary>
        ///     Gets a value indicating whether the low level mouse hook is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the low level mouse hook must be disposed when the Garbage Collector collects the
        ///     object.
        /// </summary>
        public bool MustBeDisposed { get; }

        /// <summary>
        ///     States if the low level mouse hook is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        ///     The name of the low level mouse hook.
        /// </summary>
        public string Name { get; }
        #endregion

        #region Interface Implementations
        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (!MustBeDisposed) return;
            Disable();
            IsDisposed = true;
        }

        /// <summary>
        ///     Disables the low level mouse hook.
        /// </summary>
        public void Disable()
        {
            if (HookId == IntPtr.Zero)
                return;

            SafeNativeMethods.UnhookWindowsHookEx(HookId);
            HookId = IntPtr.Zero;
            IsEnabled = false;
        }

        /// <summary>
        ///     Enables the low level mouse hook.
        /// </summary>
        public void Enable()
        {
            HookHandler = HookFunc;
            HookId = SetHook(HookHandler);
            IsEnabled = true;
        }
        #endregion

        /// <summary>
        ///     Sets hook and assigns its ID for tracking
        /// </summary>
        /// <param name="proc">Internal callback function</param>
        /// <returns>Hook ID</returns>
        private static IntPtr SetHook(MouseHookHandler proc)
        {
            using (var module = Process.GetCurrentProcess().MainModule)
                return InternalSetWindowsHookEx(WhMouseLl, proc, SafeNativeMethods.GetModuleHandle(module.ModuleName), 0);
        }

        /// <summary>
        ///     Callback function
        /// </summary>
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // parse system messages
            if (nCode < 0) return SafeNativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch ((MouseFlags) wParam)
            {
                case MouseFlags.LeftDown:
                    LeftButtonDown?.Invoke((MouseInput) Marshal.PtrToStructure(lParam, typeof (MouseInput)));
                    break;
                case MouseFlags.LeftUp:
                    LeftButtonUp?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.RightDown:
                    RightButtonDown?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.RightUp:
                    RightButtonUp?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.Move:
                    MouseMove?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.Wheel:
                    MouseWheel?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.DoubleLeftClick:
                    DoubleClick?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.MiddleDown:
                    MiddleButtonDown?.Invoke(MouseInputToStructure(lParam));
                    break;
                case MouseFlags.MiddleUp:
                    MiddleButtonUp?.Invoke((MouseInputToStructure(lParam)));
                    break;
            }

            return SafeNativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
        }

        private static MouseInput MouseInputToStructure(IntPtr lParam)
        {
            return (MouseInput) Marshal.PtrToStructure(lParam, typeof (MouseInput));
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr InternalSetWindowsHookEx(int idHook,
                                                              MouseHookHandler lpfn, IntPtr hMod, uint dwThreadId);


        /// <summary>
        ///     Internal callback processing function.
        /// </summary>
        private delegate IntPtr MouseHookHandler(int nCode, IntPtr wParam, IntPtr lParam);
    }
}