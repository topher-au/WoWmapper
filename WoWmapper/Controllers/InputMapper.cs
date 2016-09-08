using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using DS4Windows;
using WoWmapper.Keybindings;
using WoWmapper.Overlay;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using Cursor = System.Windows.Forms.Cursor;

namespace WoWmapper.Controllers
{
    public static class InputMapper
    {
        private static readonly Thread _inputThread = new Thread(InputWatcherThread);
        private static readonly bool[] _keyStates = new bool[Enum.GetNames(typeof (GamepadButton)).Length];
        private static bool _threadRunning = false;
        private static readonly HapticFeedback HapticFeedback = new HapticFeedback();
        private static DateTime _mouselookStarted;
        private static bool _setMouselook = false;
        private static bool _stopWalk = false;
        private static int _cursorX;
        private static int _cursorY;
        private static bool _crosshairShowing;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        static InputMapper()
        {
            ControllerManager.ControllerButtonStateChanged += ActiveController_ButtonStateChanged;
        }

        private static void InputWatcherThread()
        {
            while (_threadRunning)
            {
                var axisMovement = Properties.Settings.Default.SwapSticks
                    ? ControllerManager.GetRightAxis()
                    : ControllerManager.GetLeftAxis();
                var axisCursor = Properties.Settings.Default.SwapSticks
                    ? ControllerManager.GetLeftAxis()
                    : ControllerManager.GetRightAxis();

                ProcessMovement(axisMovement);

                ProcessCursor(axisCursor);


                if (Settings.Default.EnableMemoryReading && ProcessManager.GameProcess != null && MemoryManager.ReadGameState() == 1)
                {
                    var mouselookState = MemoryManager.ReadMouselook();
                    var foregroundWindow = GetForegroundWindow();
                    if (foregroundWindow == ProcessManager.GameProcess.MainWindowHandle)
                        _setMouselook = false;

                    // Cancel mouselook when alt-tabbed
                    if (Settings.Default.MemoryAutoCancel && !_setMouselook && mouselookState &&
                        foregroundWindow != ProcessManager.GameProcess.MainWindowHandle)
                    {
                        WoWInput.SendMouseClick(MouseButton.Right, true);
                        _setMouselook = true;
                    }

                    // Show/hide the overlay crosshair
                    if (Settings.Default.EnableOverlay && Settings.Default.EnableOverlayCrosshair)
                    {
                        // Show crosshair after mouselooking for 100ms
                        mouselookState = MemoryManager.ReadMouselook();
                        if (mouselookState && DateTime.Now >= _mouselookStarted + TimeSpan.FromMilliseconds(100) &&
                            !App.Overlay.CrosshairVisible && !_crosshairShowing)
                        {
                            App.Overlay.SetCrosshairState(true, _cursorX, _cursorY);
                            _crosshairShowing = true;
                        } // Otherwise hide crosshair
                        else if (!mouselookState && _crosshairShowing)
                        {
                            App.Overlay.SetCrosshairState(false);
                            _crosshairShowing = false;
                        }
                    }

                    // Check if mouselook is inactive
                    if (!MemoryManager.ReadMouselook())
                    {
                        // Update last known cursor position
                        var cursor = Cursor.Position;
                        _cursorX = cursor.X;
                        _cursorY = cursor.Y;

                        // Check if we need to re-center the mouse cursor
                        if (Settings.Default.MemoryAutoCenter &&
                            foregroundWindow == ProcessManager.GameProcess.MainWindowHandle &&
                            _mouselookStarted != DateTime.MinValue &&
                            DateTime.Now >=
                            _mouselookStarted + TimeSpan.FromMilliseconds(Settings.Default.MemoryAutoCenterDelay))
                        {
                            var windowRect = ProcessManager.GetClientRectangle();
                            Cursor.Position = new System.Drawing.Point(
                                windowRect.X + windowRect.Width/2,
                                windowRect.Y + windowRect.Height/2);
                        }

                        // Reset auto-center cooldown timer
                        _mouselookStarted = DateTime.MinValue;
                    }

                    // Check if mouselook is active
                    if (MemoryManager.ReadMouselook())
                    {
                        // If so, start the cooldown timer
                        if (_mouselookStarted == DateTime.MinValue)
                            _mouselookStarted = DateTime.Now;

                        // If the timer has elapsed but mouselook is active, temporarily hide the crosshair
                        else if (Settings.Default.EnableOverlayCrosshair &&
                                 Settings.Default.MemoryAutoCenter &&
                                 DateTime.Now >= _mouselookStarted +
                                 TimeSpan.FromMilliseconds(Settings.Default.MemoryAutoCenterDelay) && _crosshairShowing && App.Overlay.CrosshairVisible)
                        {
                            App.Overlay.SetCrosshairState(false);
                        }
                    }
                }
                
                Thread.Sleep(5);
            }
        }

        private static void ProcessMovement(Point axis)
        {
            var sendLeft = -axis.X > Properties.Settings.Default.MovementThreshold;
            var sendRight = axis.X > Properties.Settings.Default.MovementThreshold;
            var sendUp = -axis.Y > Properties.Settings.Default.MovementThreshold;
            var sendDown = axis.Y > Properties.Settings.Default.MovementThreshold;

            var strength = Math.Sqrt(axis.X*axis.X + axis.Y*axis.Y);
            if (Settings.Default.MemoryAutoWalk && MemoryManager.IsAttached && MemoryManager.ReadGameState() == 1)
            {
                var moveState = MemoryManager.ReadMovementState();
                if (moveState == 0 || moveState == 1)
                {
                    if (strength < Settings.Default.WalkThreshold &&
                        strength >= Settings.Default.MovementThreshold &&
                        moveState == 0) // Activate Walk
                    {
                        WoWInput.SendKeyDown(Key.Divide);
                        WoWInput.SendKeyUp(Key.Divide);
                        _stopWalk = false;
                    }
                    else if (strength >= Settings.Default.WalkThreshold && moveState == 1) // Deactivate walk, start run
                    {
                        WoWInput.SendKeyDown(Key.Divide);
                        WoWInput.SendKeyUp(Key.Divide);
                    }
                    else if (strength < Settings.Default.MovementThreshold && !_stopWalk && moveState == 1)
                        // Deactivate walk, stop moving
                    {
                        WoWInput.SendKeyDown(Key.Divide);
                        WoWInput.SendKeyUp(Key.Divide);
                        _stopWalk = true;
                    }
                }
            }
            if (sendLeft)
            {
                if (!_keyStates[(int) GamepadButton.LeftStickLeft])
                {
                    WorldOfWarcraft.WoWInput.SendKeyDown(BindManager.GetKey(GamepadButton.LeftStickLeft));
                    _keyStates[(int) GamepadButton.LeftStickLeft] = true;
                }
            }
            else
            {
                if (_keyStates[(int) GamepadButton.LeftStickLeft])
                {
                    WorldOfWarcraft.WoWInput.SendKeyUp(BindManager.GetKey(GamepadButton.LeftStickLeft));
                    _keyStates[(int) GamepadButton.LeftStickLeft] = false;
                }
            }

            if (sendRight)
            {
                if (!_keyStates[(int) GamepadButton.LeftStickRight])
                {
                    WorldOfWarcraft.WoWInput.SendKeyDown(BindManager.GetKey(GamepadButton.LeftStickRight));
                    _keyStates[(int) GamepadButton.LeftStickRight] = true;
                }
            }
            else
            {
                if (_keyStates[(int) GamepadButton.LeftStickRight])
                {
                    WorldOfWarcraft.WoWInput.SendKeyUp(BindManager.GetKey(GamepadButton.LeftStickRight));
                    _keyStates[(int) GamepadButton.LeftStickRight] = false;
                }
            }

            if (sendUp)
            {
                if (!_keyStates[(int) GamepadButton.LeftStickUp])
                {
                    WorldOfWarcraft.WoWInput.SendKeyDown(BindManager.GetKey(GamepadButton.LeftStickUp));
                    _keyStates[(int) GamepadButton.LeftStickUp] = true;
                }
            }
            else
            {
                if (_keyStates[(int) GamepadButton.LeftStickUp])
                {
                    WorldOfWarcraft.WoWInput.SendKeyUp(BindManager.GetKey(GamepadButton.LeftStickUp));
                    _keyStates[(int) GamepadButton.LeftStickUp] = false;
                }
            }

            if (sendDown)
            {
                if (!_keyStates[(int) GamepadButton.LeftStickDown])
                {
                    WorldOfWarcraft.WoWInput.SendKeyDown(BindManager.GetKey(GamepadButton.LeftStickDown));
                    _keyStates[(int) GamepadButton.LeftStickDown] = true;
                }
            }
            else
            {
                if (_keyStates[(int) GamepadButton.LeftStickDown])
                {
                    WorldOfWarcraft.WoWInput.SendKeyUp(BindManager.GetKey(GamepadButton.LeftStickDown));
                    _keyStates[(int) GamepadButton.LeftStickDown] = false;
                }
            }
        }

        private static void ProcessCursor(Point axis)
        {
            var axisInput = new Vector2(axis.X, axis.Y);
            if (axisInput.Magnitude > Properties.Settings.Default.CursorDeadzone)
            {
                axisInput = Vector2.Normalize(axisInput)*
                            ((axisInput.Magnitude - Properties.Settings.Default.CursorDeadzone)/
                             (127 - Properties.Settings.Default.CursorDeadzone));

                double curve = (0.05*Properties.Settings.Default.CursorCurve);

                var xSpeed = (axisInput.X < 0 ? -axisInput.X : axisInput.X)*Properties.Settings.Default.CursorSpeed;
                var ySpeed = (axisInput.Y < 0 ? -axisInput.Y : axisInput.Y)*Properties.Settings.Default.CursorSpeed;

                double xMath = Math.Pow(curve*xSpeed, 2) + (curve*xSpeed);
                double yMath = Math.Pow(curve*ySpeed, 2) + (curve*ySpeed);

                var mouseMovement = new Vector2(xMath, yMath);

                if (axis.X < 0) mouseMovement.X = -mouseMovement.X;
                if (axis.Y < 0) mouseMovement.Y = -mouseMovement.Y;

                var modX = 1;
                var modY = 1;

                var m = Cursor.Position;
                m.X += (int) mouseMovement.X*modX;
                m.Y += (int) mouseMovement.Y*modY;
                Cursor.Position = m;
            }
        }

        private static void ProcessCharacterMenu(GamepadButton button, bool state)
        {
            switch (button)
            {
                case GamepadButton.LFaceUp:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Up);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Up);
                    break;
                case GamepadButton.LFaceDown:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Down);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Down);
                    break;
                case GamepadButton.RFaceDown:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Enter);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Enter);
                    break;
                case GamepadButton.CenterMiddle:
                    if (state && !_keyStates[(int) button])
                        WoWInput.SendKeyDown(Key.Escape);
                    if (!state && _keyStates[(int) button])
                        WoWInput.SendKeyUp(Key.Escape);
                    break;
            }

            // Do left/right mouse buttons
            if (button == GamepadButton.LeftStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Left);
                else
                    WoWInput.SendMouseUp(MouseButton.Left);
            }
            else if (button == GamepadButton.RightStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Right);
                else
                    WoWInput.SendMouseUp(MouseButton.Right);
            }

            _keyStates[(int) button] = state;
        }

        private static void ProcessPlayerAoe(GamepadButton button, bool state)
        {
            if(button == Settings.Default.MemoryAoeConfirm)
                WoWInput.SendMouseClick(MouseButton.Left);
            else if (button == Settings.Default.MemoryAoeCancel)
                WoWInput.SendMouseClick(MouseButton.Right);
            else
                ProcessInput(button, state);
        }

        private static void ProcessInput(GamepadButton button, bool state)
        {
            // Do left/right mouse buttons
            if (button == GamepadButton.LeftStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Left);
                else
                    WoWInput.SendMouseUp(MouseButton.Left);
            }
            else if (button == GamepadButton.RightStick)
            {
                if (state)
                    WoWInput.SendMouseDown(MouseButton.Right);
                else
                    WoWInput.SendMouseUp(MouseButton.Right);
            }

            // Do other buttons
            if (_keyStates[(int) button] != state)
            {
                if (state)
                {
                    WoWInput.SendKeyDown(BindManager.GetKey(button));
                }
                else
                {
                    WoWInput.SendKeyUp(BindManager.GetKey(button));
                }
            }

            _keyStates[(int) button] = state;
        }

        private static void ActiveController_ButtonStateChanged(GamepadButton button, bool state)
        {
            if (Properties.Settings.Default.EnableMemoryReading)
            {
                // Process input if player is at character select
                if (Properties.Settings.Default.MemoryOverrideMenu && MemoryManager.ReadGameState() == 0)
                {
                    ProcessCharacterMenu(button, state);
                    return;
                }

                // Process input if player is casting targeted AoE
                if (Properties.Settings.Default.MemoryOverrideAoeCast &&
                    MemoryManager.ReadGameState() == 1 &&
                    MemoryManager.ReadAoeState() == true)
                {
                    ProcessPlayerAoe(button, state);
                    return;
                }
            }

            // Process input
            ProcessInput(button, state);
        }

        public static void Start()
        {
            _threadRunning = true;
            _inputThread.Start();
        }

        public static void Stop()
        {
            _threadRunning = false;
            HapticFeedback.Abort();
        }
    }
}