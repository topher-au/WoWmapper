using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Binarysharp.MemoryManagement.Core.Hooks.Input.Static.Core
{
    public static partial class InputHookManager
    {
        #region Fields, Private Properties

        /// <summary>
        ///     This field is not objectively needed but we need to keep a reference on a delegate which will be
        ///     passed to unmanaged code. To avoid GC to clean it up.
        ///     When passing delegates to unmanaged code, they must be kept alive by the managed application
        ///     until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc _sMouseDelegate;

        /// <summary>
        ///     Stores the handle to the mouse hook procedure.
        /// </summary>
        private static int _sMouseHookHandle;

        private static int _mOldX;
        private static int _mOldY;

        /// <summary>
        ///     This field is not objectively needed but we need to keep a reference on a delegate which will be
        ///     passed to unmanaged code. To avoid GC to clean it up.
        ///     When passing delegates to unmanaged code, they must be kept alive by the managed application
        ///     until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc _sKeyboardDelegate;

        /// <summary>
        ///     Stores the handle to the Keyboard hook procedure.
        /// </summary>
        private static int _sKeyboardHookHandle;

        #endregion Fields, Private Properties

        /// <summary>
        ///     A callback function which will be called every Time a mouse activity detected.
        /// </summary>
        /// <param name="nCode">
        ///     [in] Specifies whether the hook procedure must process the message.
        ///     If nCode is HC_ACTION, the hook procedure must process the message.
        ///     If nCode is less than zero, the hook procedure must pass the message to the
        ///     CallNextHookEx function without further processing and must return the
        ///     value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        ///     [in] Specifies whether the message was sent by the current thread.
        ///     If the message was sent by the current thread, it is nonzero; otherwise, it is zero.
        /// </param>
        /// <param name="lParam">
        ///     [in] Pointer to a CWPSTRUCT structure that contains details about the message.
        /// </param>
        /// <returns>
        ///     If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ///     If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx
        ///     and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC
        ///     hooks will not receive hook notifications and may behave incorrectly as a result. If the hook
        ///     procedure does not call CallNextHookEx, the return value should be zero.
        /// </returns>
        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_sMouseHookHandle, nCode, wParam, lParam);
            }
            //Marshall the data from callback.
            var mouseHookStruct = (MouseLlHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLlHookStruct));

            //detect button clicked
            var button = MouseButtons.None;
            short mouseDelta = 0;
            var clickCount = 0;
            var mouseDown = false;
            var mouseUp = false;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (wParam)
            {
                case WmLbuttondown:
                    mouseDown = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;

                case WmLbuttonup:
                    mouseUp = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;

                case WmLbuttondblclk:
                    button = MouseButtons.Left;
                    clickCount = 2;
                    break;

                case WmRbuttondown:
                    mouseDown = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;

                case WmRbuttonup:
                    mouseUp = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;

                case WmRbuttondblclk:
                    button = MouseButtons.Right;
                    clickCount = 2;
                    break;

                case WmMousewheel:
                    //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta.
                    //One wheel click is defined as WHEEL_DELTA, which is 120.
                    //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                    mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);

                    //TODO: X BUTTONS (I havent them so was unable to test)
                    //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
                    //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
                    //and the low-order word is reserved. This value can be one or more of the following values.
                    //Otherwise, MouseData is not used.
                    break;
            }

            //generate event
            var e = new MouseEventExtArgs(
                button,
                clickCount,
                mouseHookStruct.Point.X,
                mouseHookStruct.Point.Y,
                mouseDelta);

            //Mouse up
            if (SMouseUp != null && mouseUp)
            {
                SMouseUp.Invoke(null, e);
            }

            //Mouse down
            if (SMouseDown != null && mouseDown)
            {
                SMouseDown.Invoke(null, e);
            }

            //If someone listens to click and a click is heppened
            if (SMouseClick != null && clickCount > 0)
            {
                SMouseClick.Invoke(null, e);
            }

            //If someone listens to click and a click is heppened
            if (SMouseClickExt != null && clickCount > 0)
            {
                SMouseClickExt.Invoke(null, e);
            }

            //If someone listens to double click and a click is heppened
            if (SMouseDoubleClick != null &&
                clickCount == 2)
            {
                SMouseDoubleClick.Invoke(null, e);
            }

            //Wheel was moved
            if (SMouseWheel != null && mouseDelta != 0)
            {
                SMouseWheel.Invoke(null, e);
            }

            //If someone listens to move and there was a change in coordinates raise move event
            if ((SMouseMove != null ||
                 SMouseMoveExt != null) &&
                (_mOldX != mouseHookStruct.Point.X || _mOldY != mouseHookStruct.Point.Y))
            {
                _mOldX = mouseHookStruct.Point.X;
                _mOldY = mouseHookStruct.Point.Y;
                SMouseMove?.Invoke(null, e);

                SMouseMoveExt?.Invoke(null, e);
            }

            if (e.Handled)
            {
                return -1;
            }

            //call next hook
            return CallNextHookEx(_sMouseHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            // install Mouse hook only if it is not installed and must be installed
            if (_sMouseHookHandle != 0) return;
            //See comment of this field. To avoid GC to clean it up.
            _sMouseDelegate = MouseHookProc;
            //install hook
            _sMouseHookHandle = SetWindowsHookEx(
                WhMouseLl,
                _sMouseDelegate,
                Marshal.GetHINSTANCE(
                    Assembly.GetExecutingAssembly().GetModules()[0]),
                0);
            //If SetWindowsHookEx fails.
            if (_sMouseHookHandle != 0) return;
            //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set.
            var errorCode = Marshal.GetLastWin32Error();
            //do cleanup
            //Initializes and throws a new instance of the Win32Exception class with the specified error.
            throw new Win32Exception(errorCode);
        }

        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            MulticastDelegate[] array;
            GetMultiCastDelegates(out array);
            //if no subsribers are registered unsubsribe from hook.
            if (array.All(a => a == null))
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        private static void GetMultiCastDelegates(out MulticastDelegate[] array)
        {
            array = new MulticastDelegate[]
                    {
                        SMouseClick,
                        SMouseDown,
                        SMouseMove,
                        SMouseUp,
                        SMouseClickExt,
                        SMouseMoveExt,
                        SMouseWheel
                    };
        }

        private static void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (_sMouseHookHandle == 0) return;
            //uninstall hook
            var result = UnhookWindowsHookEx(_sMouseHookHandle);
            //reset invalid handle
            _sMouseHookHandle = 0;
            //Free up for GC
            _sMouseDelegate = null;
            //if failed and exception must be thrown
            if (result != 0) return;
            //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set.
            var errorCode = Marshal.GetLastWin32Error();
            //Initializes and throws a new instance of the Win32Exception class with the specified error.
            throw new Win32Exception(errorCode);
        }

        /// <summary>
        ///     A callback function which will be called every Time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        ///     [in] Specifies whether the hook procedure must process the message.
        ///     If nCode is HC_ACTION, the hook procedure must process the message.
        ///     If nCode is less than zero, the hook procedure must pass the message to the
        ///     CallNextHookEx function without further processing and must return the
        ///     value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        ///     [in] Specifies whether the message was sent by the current thread.
        ///     If the message was sent by the current thread, it is nonzero; otherwise, it is zero.
        /// </param>
        /// <param name="lParam">
        ///     [in] Pointer to a CWPSTRUCT structure that contains details about the message.
        /// </param>
        /// <returns>
        ///     If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ///     If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx
        ///     and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC
        ///     hooks will not receive hook notifications and may behave incorrectly as a result. If the hook
        ///     procedure does not call CallNextHookEx, the return value should be zero.
        /// </returns>
        private static int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            //indicates if any of underlaing events set e.Handled flag
            var handled = false;

            if (nCode >= 0)
            {
                //read structure KeyboardHookStruct at lParam
                var myKeyboardHookStruct =
                    (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //raise KeyDown
                if (SKeyDown != null &&
                    (wParam == WmKeydown || wParam == WmSyskeydown))
                {
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;
                    var e = new KeyEventArgs(keyData);
                    SKeyDown.Invoke(null, e);
                    handled = e.Handled;
                }

                // raise KeyPress
                if (SKeyPress != null &&
                    wParam == WmKeydown)
                {
                    var isDownShift = ((GetKeyState(VkShift) & 0x80) == 0x80 ? true : false);
                    var isDownCapslock = (GetKeyState(VkCapital) != 0 ? true : false);

                    var keyState = new byte[256];
                    GetKeyboardState(keyState);
                    var inBuffer = new byte[2];
                    if (ToAscii(myKeyboardHookStruct.VirtualKeyCode,
                        myKeyboardHookStruct.ScanCode,
                        keyState,
                        inBuffer,
                        myKeyboardHookStruct.Flags) == 1)
                    {
                        var key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && char.IsLetter(key)) key = char.ToUpper(key);
                        var e = new KeyPressEventArgs(key);
                        SKeyPress.Invoke(null, e);
                        handled = handled || e.Handled;
                    }
                }

                // raise KeyUp
                if (SKeyUp != null &&
                    (wParam == WmKeyup || wParam == WmSyskeyup))
                {
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;
                    var e = new KeyEventArgs(keyData);
                    SKeyUp.Invoke(null, e);
                    handled = handled || e.Handled;
                }
            }

            //if event handled in application do not handoff to other listeners
            if (handled)
                return -1;

            //forward to other application
            return CallNextHookEx(_sKeyboardHookHandle, nCode, wParam, lParam);
        }

        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (_sKeyboardHookHandle != 0) return;
            //See comment of this field. To avoid GC to clean it up.
            _sKeyboardDelegate = KeyboardHookProc;
            //install hook
            _sKeyboardHookHandle = SetWindowsHookEx(
                WhKeyboardLl,
                _sKeyboardDelegate,
                Marshal.GetHINSTANCE(
                    Assembly.GetExecutingAssembly().GetModules()[0]),
                0);
            //If SetWindowsHookEx fails.
            if (_sKeyboardHookHandle != 0) return;
            //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set.
            var errorCode = Marshal.GetLastWin32Error();
            //do cleanup

            //Initializes and throws a new instance of the Win32Exception class with the specified error.
            throw new Win32Exception(errorCode);
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            //if no subsribers are registered unsubsribe from hook
            if (SKeyDown == null &&
                SKeyUp == null &&
                SKeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle == 0) return;
            //uninstall hook
            var result = UnhookWindowsHookEx(_sKeyboardHookHandle);
            //reset invalid handle
            _sKeyboardHookHandle = 0;
            //Free up for GC
            _sKeyboardDelegate = null;
            //if failed and exception must be thrown
            if (result != 0) return;
            //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set.
            var errorCode = Marshal.GetLastWin32Error();
            //Initializes and throws a new instance of the Win32Exception class with the specified error.
            throw new Win32Exception(errorCode);
        }

        /// <summary>
        ///     The CallWndProc hook procedure is an application-defined or library-defined callback
        ///     function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer
        ///     to this callback function. CallWndProc is a placeholder for the application-defined
        ///     or library-defined function name.
        /// </summary>
        /// <param name="nCode">
        ///     [in] Specifies whether the hook procedure must process the message.
        ///     If nCode is HC_ACTION, the hook procedure must process the message.
        ///     If nCode is less than zero, the hook procedure must pass the message to the
        ///     CallNextHookEx function without further processing and must return the
        ///     value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        ///     [in] Specifies whether the message was sent by the current thread.
        ///     If the message was sent by the current thread, it is nonzero; otherwise, it is zero.
        /// </param>
        /// <param name="lParam">
        ///     [in] Pointer to a CWPSTRUCT structure that contains details about the message.
        /// </param>
        /// <returns>
        ///     If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        ///     If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx
        ///     and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC
        ///     hooks will not receive hook notifications and may behave incorrectly as a result. If the hook
        ///     procedure does not call CallNextHookEx, the return value should be zero.
        /// </returns>
        /// <remarks>
        ///     http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/callwndproc.asp
        /// </remarks>
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        //##############################################################################

        //##############################################################################
    }
}