using System;
using System.Windows.Forms;

// ReSharper disable once CheckNamespace

namespace Binarysharp.MemoryManagement.Core.Hooks.Input.Static.Core
{
    /// <summary>
    ///     This class monitors all mouse activities globally (also outside of the application)
    ///     and provides appropriate events.
    /// </summary>
    public static partial class InputHookManager
    {
        #region Public Delegates/Events

        private static event MouseEventHandler SMouseMove;

        /// <summary>
        ///     Occurs when the mouse pointer is moved.
        /// </summary>
        public static event MouseEventHandler MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseMove += value;
            }

            remove
            {
                SMouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event EventHandler<MouseEventExtArgs> SMouseMoveExt;

        /// <summary>
        ///     Occurs when the mouse pointer is moved.
        /// </summary>
        /// <remarks>
        ///     This event provides extended arguments of type <see cref="MouseEventArgs" /> enabling you to
        ///     supress further processing of mouse movement in other applications.
        /// </remarks>
        public static event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseMoveExt += value;
            }

            remove
            {
                SMouseMoveExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseClick;

        /// <summary>
        ///     Occurs when a click was performed by the mouse.
        /// </summary>
        public static event MouseEventHandler MouseClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseClick += value;
            }
            remove
            {
                SMouseClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event EventHandler<MouseEventExtArgs> SMouseClickExt;

        /// <summary>
        ///     Occurs when a click was performed by the mouse.
        /// </summary>
        /// <remarks>
        ///     This event provides extended arguments of type <see cref="MouseEventArgs" /> enabling you to
        ///     supress further processing of mouse click in other applications.
        /// </remarks>
        public static event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseClickExt += value;
            }
            remove
            {
                SMouseClickExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseDown;

        /// <summary>
        ///     Occurs when the mouse a mouse button is pressed.
        /// </summary>
        public static event MouseEventHandler MouseDown
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseDown += value;
            }
            remove
            {
                SMouseDown -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseUp;

        /// <summary>
        ///     Occurs when a mouse button is released.
        /// </summary>
        public static event MouseEventHandler MouseUp
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseUp += value;
            }
            remove
            {
                SMouseUp -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseWheel;

        /// <summary>
        ///     Occurs when the mouse wheel moves.
        /// </summary>
        public static event MouseEventHandler MouseWheel
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseWheel += value;
            }
            remove
            {
                SMouseWheel -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler SMouseDoubleClick;

        //The double click event will not be provided directly from hook.
        //To fire the double click event wee need to monitor mouse up event and when it occures
        //Two times during the time interval which is defined in Windows as a doble click time
        //we fire this event.

        /// <summary>
        ///     Occurs when a double clicked was performed by the mouse.
        /// </summary>
        public static event MouseEventHandler MouseDoubleClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                if (SMouseDoubleClick == null)
                {
                    //We create a timer to monitor interval between two clicks
                    _sDoubleClickTimer = new Timer
                    {
                        //This interval will be set to the value we retrive from windows. This is a windows setting from contro planel.
                        Interval =
                                                 GetDoubleClickTime(),
                        //We do not start timer yet. It will be start when the click occures.
                        Enabled = false
                    };
                    //We define the callback function for the timer
                    _sDoubleClickTimer.Tick += DoubleClickTimeElapsed;
                    //We start to monitor mouse up event.
                    MouseUp += OnMouseUp;
                }
                SMouseDoubleClick += value;
            }
            remove
            {
                if (SMouseDoubleClick != null)
                {
                    SMouseDoubleClick -= value;
                    if (SMouseDoubleClick == null)
                    {
                        //Stop monitoring mouse up
                        MouseUp -= OnMouseUp;
                        //Dispose the timer
                        _sDoubleClickTimer.Tick -= DoubleClickTimeElapsed;
                        _sDoubleClickTimer = null;
                    }
                }
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event KeyPressEventHandler SKeyPress;

        /// <summary>
        ///     Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        ///     Key events occur in the following order:
        ///     <list type="number">
        ///         <item>KeyDown</item>
        ///         <item>KeyPress</item>
        ///         <item>KeyUp</item>
        ///     </list>
        ///     The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown and
        ///     KeyUp events.
        ///     Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes.
        ///     To handle keyboard events only in your application and not enable other applications to receive keyboard events,
        ///     set the KeyPressEventArgs.Handled property in your form's KeyPress event-handling method to <b>true</b>.
        /// </remarks>
        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyPress += value;
            }
            remove
            {
                SKeyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler SKeyUp;

        /// <summary>
        ///     Occurs when a key is released.
        /// </summary>
        public static event KeyEventHandler KeyUp
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyUp += value;
            }
            remove
            {
                SKeyUp -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler SKeyDown;

        /// <summary>
        ///     Occurs when a key is preseed.
        /// </summary>
        public static event KeyEventHandler KeyDown
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyDown += value;
            }
            remove
            {
                SKeyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        #endregion Public Delegates/Events

        #region Fields, Private Properties

        //This field remembers mouse button pressed because in addition to the short interval it must be also the same button.
        private static MouseButtons _sPrevClickedButton;

        //The timer to monitor time interval between two clicks.
        private static Timer _sDoubleClickTimer;

        #endregion Fields, Private Properties

        private static void DoubleClickTimeElapsed(object sender, EventArgs e)
        {
            //Timer is alapsed and no second click ocuured
            _sDoubleClickTimer.Enabled = false;
            _sPrevClickedButton = MouseButtons.None;
        }

        /// <summary>
        ///     This method is designed to monitor mouse clicks in order to fire a double click event if interval between
        ///     clicks was short enaugh.
        /// </summary>
        /// <param name="sender">Is always null</param>
        /// <param name="e">Some information about click heppened.</param>
        private static void OnMouseUp(object sender, MouseEventArgs e)
        {
            //This should not heppen
            if (e.Clicks < 1)
            {
                return;
            }
            //If the secon click heppened on the same button
            if (e.Button.Equals(_sPrevClickedButton))
            {
                //Fire double click
                SMouseDoubleClick?.Invoke(null, e);
                //Stop timer
                _sDoubleClickTimer.Enabled = false;
                _sPrevClickedButton = MouseButtons.None;
            }
            else
            {
                //If it was the firts click start the timer
                _sDoubleClickTimer.Enabled = true;
                _sPrevClickedButton = e.Button;
            }
        }

        //################################################################

        //################################################################
    }
}