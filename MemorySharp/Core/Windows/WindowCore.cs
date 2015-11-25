using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement.Core.Helpers;
using Binarysharp.MemoryManagement.Core.Native;
using Binarysharp.MemoryManagement.Core.Native.Enums;
using Binarysharp.MemoryManagement.Core.Native.Structs;

namespace Binarysharp.MemoryManagement.Core.Windows
{
    /// <summary>
    ///     Static core class providing tools for managing windows.
    /// </summary>
    public static class WindowCore
    {
        /// <summary>
        ///     Retrieves the name of the class to which the specified window belongs.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window and, indirectly, the class to which the window belongs.</param>
        /// <returns>The return values is the class name string.</returns>
        public static string GetClassName(IntPtr windowHandle)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Get the window class name
            var stringBuilder = new StringBuilder(char.MaxValue);
            if (SafeNativeMethods.GetClassName(windowHandle, stringBuilder, stringBuilder.Capacity) == 0)
                throw new Win32Exception("Couldn't get the class name of the window or the window has no class name.");

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Retrieves a processHandle to the foreground window (the window with which the user is currently working).
        /// </summary>
        /// <returns>
        ///     A processHandle to the foreground window. The foreground window can be <c>IntPtr.Zero</c> in certain circumstances,
        ///     such as when a window is losing activation.
        /// </returns>
        public static IntPtr GetForegroundWindow()
        {
            return SafeNativeMethods.GetForegroundWindow();
        }

        /// <summary>
        ///     Retrieves the specified system metric or system configuration setting.
        /// </summary>
        /// <param name="metric">The system metric or configuration setting to be retrieved.</param>
        /// <returns>The return value is the requested system metric or configuration setting.</returns>
        public static int GetSystemMetrics(SystemMetrics metric)
        {
            var ret = SafeNativeMethods.GetSystemMetrics(metric);

            if (ret != 0)
                return ret;

            throw new Win32Exception(
                "The call of GetSystemMetrics failed. Unfortunately, GetLastError code doesn't provide more information.");
        }

        /// <summary>
        ///     Gets the text of the specified window's title bar.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window containing the text.</param>
        /// <returns>The return value is the window's title bar.</returns>
        public static string GetWindowText(IntPtr windowHandle)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Get the size of the window's title
            var capacity = SafeNativeMethods.GetWindowTextLength(windowHandle);
            // If the window doesn't contain any title
            if (capacity == 0)
                return string.Empty;

            // Get the text of the window's title bar text
            var stringBuilder = new StringBuilder(capacity + 1);
            if (SafeNativeMethods.GetWindowText(windowHandle, stringBuilder, stringBuilder.Capacity) == 0)
                throw new Win32Exception("Couldn't get the text of the window's title bar or the window has no title.");

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window.</param>
        /// <returns>
        ///     The return value is a <see cref="WindowPlacement" /> structure that receives the show state and position
        ///     information.
        /// </returns>
        public static WindowPlacement GetWindowPlacement(IntPtr windowHandle)
        {
            // Check if the handle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Allocate a WindowPlacement structure
            WindowPlacement placement;
            placement.Length = Marshal.SizeOf(typeof (WindowPlacement));

            // Get the window placement
            if (!SafeNativeMethods.GetWindowPlacement(windowHandle, out placement))
                throw new Win32Exception("Couldn't get the window placement.");

            return placement;
        }

        /// <summary>
        ///     Retrieves the identifier of the process that created the window.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window.</param>
        /// <returns>The return value is the identifier of the process that created the window.</returns>
        public static int GetWindowProcessId(IntPtr windowHandle)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Get the process id
            int processId;
            SafeNativeMethods.GetWindowThreadProcessId(windowHandle, out processId);

            return processId;
        }

        /// <summary>
        ///     Retrieves the identifier of the thread that created the specified window.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window.</param>
        /// <returns>The return value is the identifier of the thread that created the window.</returns>
        public static int GetWindowThreadId(IntPtr windowHandle)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Get the thread id
            int trash;
            return SafeNativeMethods.GetWindowThreadProcessId(windowHandle, out trash);
        }

        /// <summary>
        ///     Enumerates all the windows on the screen.
        /// </summary>
        /// <returns>A collection of handles of all the windows.</returns>
        public static IEnumerable<IntPtr> EnumAllWindows()
        {
            // Create the list of windows
            var list = new List<IntPtr>();

            // For each top-level windows
            foreach (var topWindow in EnumTopLevelWindows())
            {
                // Add this window to the list
                list.Add(topWindow);
                // Enumerate and add the children of this window
                list.AddRange(EnumChildWindows(topWindow));
            }

            // Return the list of windows
            return list;
        }

        /// <summary>
        ///     Enumerates recursively all the child windows that belong to the specified parent window.
        /// </summary>
        /// <param name="parentHandle">The parent window processHandle.</param>
        /// <returns>A collection of handles of the child windows.</returns>
        public static IEnumerable<IntPtr> EnumChildWindows(IntPtr parentHandle)
        {
            // Create the list of windows
            var list = new List<IntPtr>();
            // Create the callback
            EnumWindowsProc callback = delegate(IntPtr windowHandle, IntPtr lParam)
                                       {
                                           list.Add(windowHandle);
                                           return true;
                                       };
            // Enumerate all windows
            SafeNativeMethods.EnumChildWindows(parentHandle, callback, IntPtr.Zero);

            // Returns the list of the windows
            return list.ToArray();
        }

        /// <summary>
        ///     Enumerates all top-level windows on the screen. This function does not search child windows.
        /// </summary>
        /// <returns>A collection of handles of top-level windows.</returns>
        public static IEnumerable<IntPtr> EnumTopLevelWindows()
        {
            // When passing a null pointer, this function is equivalent to EnumWindows
            return EnumChildWindows(IntPtr.Zero);
        }

        /// <summary>
        ///     Flashes the specified window one time. It does not change the active state of the window.
        ///     To flash the window a specified number of times, use the
        ///     <see cref="FlashWindowEx(IntPtr, FlashWindowFlags, uint, TimeSpan)" /> function.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window to be flashed. The window can be either open or minimized.</param>
        /// <returns>
        ///     The return value specifies the window's state before the call to the <see cref="FlashWindow" /> function.
        ///     If the window caption was drawn as active before the call, the return value is nonzero. Otherwise, the return value
        ///     is zero.
        /// </returns>
        public static bool FlashWindow(IntPtr windowHandle)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Flash the window
            return SafeNativeMethods.FlashWindow(windowHandle, true);
        }

        /// <summary>
        ///     Flashes the specified window. It does not change the active state of the window.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window to be flashed. The window can be either opened or minimized.</param>
        /// <param name="flags">The flash status.</param>
        /// <param name="count">The number of times to flash the window.</param>
        /// <param name="timeout">The rate at which the window is to be flashed.</param>
        public static void FlashWindowEx(IntPtr windowHandle, FlashWindowFlags flags, uint count, TimeSpan timeout)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Create the data structure
            var flashInfo = new FlashInfo
                            {
                                Size = Marshal.SizeOf(typeof (FlashInfo)),
                                Hwnd = windowHandle,
                                Flags = flags,
                                Count = count,
                                Timeout = Convert.ToInt32(timeout.TotalMilliseconds)
                            };

            // Flash the window
            SafeNativeMethods.FlashWindowEx(ref flashInfo);
        }

        /// <summary>
        ///     Flashes the specified window. It does not change the active state of the window. The function uses the default
        ///     cursor blink rate.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window to be flashed. The window can be either opened or minimized.</param>
        /// <param name="flags">The flash status.</param>
        /// <param name="count">The number of times to flash the window.</param>
        public static void FlashWindowEx(IntPtr windowHandle, FlashWindowFlags flags, uint count)
        {
            FlashWindowEx(windowHandle, flags, count, TimeSpan.FromMilliseconds(0));
        }

        /// <summary>
        ///     Flashes the specified window. It does not change the active state of the window. The function uses the default
        ///     cursor blink rate.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window to be flashed. The window can be either opened or minimized.</param>
        /// <param name="flags">The flash status.</param>
        public static void FlashWindowEx(IntPtr windowHandle, FlashWindowFlags flags)
        {
            FlashWindowEx(windowHandle, flags, 0);
        }

        /// <summary>
        ///     Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a
        ///     virtual-key code.
        ///     To specify a processHandle to the keyboard layout to use for translating the specified code, use the
        ///     MapVirtualKeyEx
        ///     function.
        /// </summary>
        /// <param name="key">
        ///     The virtual key code or scan code for a key. How this value is interpreted depends on the value of the uMapType
        ///     parameter.
        /// </param>
        /// <param name="translation">
        ///     The translation to be performed. The value of this parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>
        ///     The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode
        ///     and uMapType.
        ///     If there is no translation, the return value is zero.
        /// </returns>
        public static uint MapVirtualKey(uint key, TranslationTypes translation)
        {
            return SafeNativeMethods.MapVirtualKey(key, translation);
        }

        /// <summary>
        ///     Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a
        ///     virtual-key code.
        ///     To specify a processHandle to the keyboard layout to use for translating the specified code, use the
        ///     MapVirtualKeyEx
        ///     function.
        /// </summary>
        /// <param name="key">
        ///     The virtual key code for a key. How this value is interpreted depends on the value of the uMapType parameter.
        /// </param>
        /// <param name="translation">
        ///     The translation to be performed. The value of this parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>
        ///     The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode
        ///     and uMapType.
        ///     If there is no translation, the return value is zero.
        /// </returns>
        public static uint MapVirtualKey(Keys key, TranslationTypes translation)
        {
            return MapVirtualKey((uint) key, translation);
        }

        /// <summary>
        ///     Places (posts) a message in the message queue associated with the thread that created the specified window and
        ///     returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="windowHandle">
        ///     A processHandle to the window whose window procedure is to receive the message. The following
        ///     values have special meanings.
        /// </param>
        /// <param name="message">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        public static void PostMessage(IntPtr windowHandle, uint message, UIntPtr wParam, UIntPtr lParam)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Post the message
            if (!SafeNativeMethods.PostMessage(windowHandle, message, wParam, lParam))
                throw new Win32Exception($"Couldn't post the message '{message}'.");
        }

        /// <summary>
        ///     Places (posts) a message in the message queue associated with the thread that created the specified window and
        ///     returns without waiting for the thread to process the message.
        /// </summary>
        /// <param name="windowHandle">
        ///     A processHandle to the window whose window procedure is to receive the message. The following
        ///     values have special meanings.
        /// </param>
        /// <param name="message">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        public static void PostMessage(IntPtr windowHandle, WindowsMessages message, UIntPtr wParam, UIntPtr lParam)
        {
            PostMessage(windowHandle, (uint) message, wParam, lParam);
        }

        /// <summary>
        ///     Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="inputs">
        ///     An array of <see cref="Input" /> structures. Each structure represents an event to be inserted
        ///     into the keyboard or mouse input stream.
        /// </param>
        public static void SendInput(Input[] inputs)
        {
            // Check if the array passed in parameter is not empty
            if (inputs != null && inputs.Length != 0)
            {
                if (SafeNativeMethods.SendInput(inputs.Length, inputs, TypeData<Input>.Size) == 0)
                    throw new Win32Exception("Couldn't send the inputs.");
            }
            else
            {
                throw new ArgumentException("The parameter cannot be null or empty.", nameof(inputs));
            }
        }

        /// <summary>
        ///     Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        /// <param name="input">A structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        public static void SendInput(Input input)
        {
            SendInput(new[] {input});
        }

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window whose window procedure will receive the message.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public static IntPtr SendMessage(IntPtr windowHandle, uint message, UIntPtr wParam, IntPtr lParam)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Send the message
            return SafeNativeMethods.SendMessage(windowHandle, message, wParam, lParam);
        }

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window whose window procedure will receive the message.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public static IntPtr SendMessage(IntPtr windowHandle, WindowsMessages message, UIntPtr wParam, IntPtr lParam)
        {
            return SendMessage(windowHandle, (uint) message, wParam, lParam);
        }

        /// <summary>
        ///     Brings the thread that created the specified window into the foreground and activates the window.
        ///     The window is restored if minimized. Performs no action if the window is already activated.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window that should be activated and brought to the foreground.</param>
        /// <returns>
        ///     If the window was brought to the foreground, the return value is <c>true</c>, otherwise the return value is
        ///     <c>false</c>.
        /// </returns>
        public static void SetForegroundWindow(IntPtr windowHandle)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // If the window is already activated, do nothing
            if (GetForegroundWindow() == windowHandle)
                return;

            // Restore the window if minimized
            ShowWindow(windowHandle, WindowStates.Restore);

            // Activate the window
            if (!SafeNativeMethods.SetForegroundWindow(windowHandle))
                throw new ApplicationException("Couldn't set the window to foreground.");
        }

        /// <summary>
        ///     Sets the current position and size of the specified window.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window.</param>
        /// <param name="left">The x-coordinate of the upper-left corner of the window.</param>
        /// <param name="top">The y-coordinate of the upper-left corner of the window.</param>
        /// <param name="height">The height of the window.</param>
        /// <param name="width">The width of the window.</param>
        public static void SetWindowPlacement(IntPtr windowHandle, int left, int top, int height, int width)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Get a WindowPlacement structure of the current window
            var placement = GetWindowPlacement(windowHandle);

            // Set the values
            placement.NormalPosition.Left = left;
            placement.NormalPosition.Top = top;
            placement.NormalPosition.Height = height;
            placement.NormalPosition.Width = width;

            // Set the window placement
            SetWindowPlacement(windowHandle, placement);
        }

        /// <summary>
        ///     Sets the show state and the restored, minimized, and maximized positions of the specified window.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window.</param>
        /// <param name="placement">
        ///     A pointer to the <see cref="WindowPlacement" /> structure that specifies the new show state and
        ///     window positions.
        /// </param>
        public static void SetWindowPlacement(IntPtr windowHandle, WindowPlacement placement)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // If the debugger is attached and the state of the window is ShowDefault, there's an issue where the window disappears
            if (Debugger.IsAttached && placement.ShowCmd == WindowStates.ShowNormal)
                placement.ShowCmd = WindowStates.Restore;

            // Set the window placement
            if (!SafeNativeMethods.SetWindowPlacement(windowHandle, ref placement))
                throw new Win32Exception("Couldn't set the window placement.");
        }

        /// <summary>
        ///     Sets the text of the specified window's title bar.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window whose text is to be changed.</param>
        /// <param name="title">The new title text.</param>
        public static void SetWindowText(IntPtr windowHandle, string title)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Set the text of the window's title bar
            if (!SafeNativeMethods.SetWindowText(windowHandle, title))
                throw new Win32Exception("Couldn't set the text of the window's title bar.");
        }

        /// <summary>
        ///     Sets the specified window's show state.
        /// </summary>
        /// <param name="windowHandle">A processHandle to the window.</param>
        /// <param name="state">Controls how the window is to be shown.</param>
        /// <returns>
        ///     If the window was previously visible, the return value is <c>true</c>, otherwise the return value is
        ///     <c>false</c>.
        /// </returns>
        public static bool ShowWindow(IntPtr windowHandle, WindowStates state)
        {
            // Check if the processHandle is valid
            HandleManipulator.ValidateAsArgument(windowHandle, "windowHandle");

            // Change the state of the window
            return SafeNativeMethods.ShowWindow(windowHandle, state);
        }

        /// <summary>
        ///     Abstract class defining a virtual keyboard.
        /// </summary>
        public abstract class BaseKeyboard
        {
            #region Fields, Private Properties
            /// <summary>
            ///     The collection storing the current pressed keys.
            /// </summary>
            protected static readonly List<Tuple<IntPtr, Keys>> PressedKeys = new List<Tuple<IntPtr, Keys>>();
            #endregion

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
            #endregion

            #region Public Properties, Indexers
            /// <summary>
            ///     Gets the safe processHandle.
            /// </summary>
            /// <value>The safe processHandle.</value>
            protected IntPtr Handle { get; }
            #endregion

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
        }
    }
}