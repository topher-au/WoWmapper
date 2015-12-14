using DS4Windows;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace DS4Wrapper
{
    public enum DS4Axis : int
    {
        L2,
        R2,
        Lx,
        Ly,
        Rx,
        Ry,
        Gx,
        Gy,
        Gz
    }

    public enum DS4Button : int
    {
        Cross,
        Circle,
        Triangle,
        Square,
        L1,
        L2,
        L3,
        R1,
        R2,
        R3,
        DpadUp,
        DpadDown,
        DpadRight,
        DpadLeft,
        Share,
        Options,
        PS,
        TouchLeft,
        TouchRight,
        TouchUpper,
        LStickUp,
        LStickDown,
        LStickRight,
        LStickLeft
    }

    public enum DS4Stick
    {
        Left,
        Right
    }

    public enum DS4Trigger
    {
        L2,
        R2
    }

    public enum TriggerMode
    {
        Analog,
        Buttons
    }

    public class DS4 : IDisposable
    {
        #region Public Fields

        private DS4Device Controller;

        public bool IsConnected;

        #endregion Public Fields

        #region Private Fields
        private float[] axisThresholds;

        private bool[] buttonStates, axisStates;

        private Thread controllerThread, stateThread;

        private bool Suspended = false;

        public ConnectionType ConnectionType { get
            {
                if(Controller != null)
                    return Controller.ConnectionType;
                return ConnectionType.USB;
            } }

        #endregion Private Fields

        #region Public Constructors

        public DS4()
        {
            // Start a thread to monitor the DS4 connection state
            controllerThread = new Thread(ControllerThread);
            controllerThread.Priority = ThreadPriority.Highest;
            controllerThread.IsBackground = true;
            controllerThread.Start();

            // Monitor button presses
            stateThread = new Thread(StateMonitor);
            stateThread.Priority = ThreadPriority.Highest;
            stateThread.IsBackground = true;
            stateThread.Start();

            // Hook events
            ControllerConnected += OnControllerConnected;
            ControllerDisconnected += OnControllerDisconnected;
            ButtonDown += OnButtonDown;
            ButtonUp += OnButtonUp;
            AxisThresholdReached += OnAxisOver;
            AxisThresholdDropped += OnAxisUnder;

            // Create an array of digital button states
            var buttonNames = Enum.GetNames(typeof(DS4Button));
            buttonStates = new bool[buttonNames.Length];

            // Array of axis thresholds and states
            var axisNames = Enum.GetNames(typeof(DS4Axis));
            axisThresholds = new float[axisNames.Length];
            axisStates = new bool[axisNames.Length];

            for (int i = 0; i < axisThresholds.Length; i++)
            {
                axisThresholds[i] = 0.8f; // default threshold
            }

            TriggerMode = TriggerMode.Buttons;
            TriggerSensitivity.L2 = 40;
            TriggerSensitivity.R2 = 40;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void AxisThresholdDroppedHandler(DS4Axis Axis);

        public delegate void AxisThresholdHandler(DS4Axis Axis);

        public delegate void ButtonDownHandler(DS4Button Button);

        public delegate void ButtonUpHandler(DS4Button Button);

        public delegate void ControllerConnectedHandler();

        public delegate void ControllerDisconnectedHandler();

        public delegate void TouchpadMovedHandler(TouchpadEventArgs e);

        #endregion Public Delegates

        #region Public Events

        public event AxisThresholdDroppedHandler AxisThresholdDropped;

        public event AxisThresholdHandler AxisThresholdReached;

        public event ButtonDownHandler ButtonDown;

        public event ButtonDownHandler ButtonUp;

        public event ControllerConnectedHandler ControllerConnected;

        public event ControllerDisconnectedHandler ControllerDisconnected;

        public event TouchpadMovedHandler TouchpadMoved;

        #endregion Public Events

        #region Public Properties

        public int Battery
        {
            get
            {
                if (Controller != null)
                {
                    var cBat = Controller.Battery;
                    if (cBat > 100) cBat = 100;
                    return cBat;
                }
                else return 0;
            }
        }

        // Settings
        public TriggerMode TriggerMode { get; set; } // Triggers analog/digital

        public DS4Sensitivity TriggerSensitivity { get; set; } = new DS4Sensitivity();

        #endregion Public Properties

        public class DS4Sensitivity
        {
            public int L2 { get; set; }
            public int R2 { get; set; }
        }

        #region Public Methods

        // Trigger sensitivity
        public void ControllerThread()
        {
            while (true)
            {
                if (Controller != null)
                {
                    if (Controller.IsDisconnecting)
                    {
                        Controller = null;
                        IsConnected = false;
                        ControllerDisconnected();
                        continue;
                    }
                    Thread.Sleep(5);
                    continue;
                }
                DS4Devices.findControllers();
                var Controllers = DS4Devices.getDS4Controllers();
                if (Controllers.Count() > 0)
                {
                    Controller = Controllers.First();
                    IsConnected = true;
                    if (Controller != null)
                    {
                        try
                        {
                            ControllerConnected();
                            
                        } catch { }
                        Controller.Touchpad.TouchesMoved += Touchpad_TouchesMoved;

                    }
                }
                Thread.Sleep(5);
            }
        }

        private void Touchpad_TouchesMoved(object sender, TouchpadEventArgs e)
        {
            TouchpadMoved(e);
        }

        public bool GetButtonState(DS4Button Button)
        {
            return buttonStates[(int)Button];
        }

        public float GetStickAxis(byte Axis)
        {
            float StickAxis = Axis - 127;
            StickAxis *= 2;
            StickAxis /= 255;
            return StickAxis;
        }

        public Point GetStickPoint(DS4Stick Stick)
        {
            if (Controller != null)
            {
                var state = Controller.getCurrentState();
                switch (Stick)
                {
                    case DS4Stick.Left:
                        return new Point()
                        {
                            X = state.LX - 128,
                            Y = state.LY - 128
                        };

                    case DS4Stick.Right:
                        return new Point()
                        {
                            X = state.RX - 128,
                            Y = state.RY - 128
                        };
                }
            }

            return new Point(0, 0);
        }

        public float GetThreshold(DS4Axis Axis)
        {
            return axisThresholds[(int)Axis];
        }

        public float AxisToFloat(byte Axis)
        {
            float TriggerAxis = Axis / 255f;
            return TriggerAxis;
        }

        public float GetTriggerState(DS4Trigger Trigger)
        {
            if (Controller != null)
            {
                var state = Controller.getCurrentState();
                switch (Trigger)
                {
                    case DS4Trigger.L2:
                        return AxisToFloat(state.L2);
                    case DS4Trigger.R2:
                        return AxisToFloat(state.R2);
                }
            }
            return 0f;
        }

        public bool IsButtonPressed(DS4Button Button)
        {
            return buttonStates[(int)Button];
        }

        public void LightBarFlash(DS4Color c, byte On, byte Off)
        {
            if (Controller != null)
            {
                Controller.LightBarColor = c;
                Controller.LightBarOffDuration = Off;
                Controller.LightBarOnDuration = On;
            }
        }

        public void LightBarOff()
        {
            if (Controller != null)
            {
                Controller.LightBarColor = new DS4Color(Color.Black);
                Controller.LightBarOnDuration = 0x00;
                Controller.LightBarOffDuration = 0x00;
            }
        }

        public void LightBarOn(DS4Color c)
        {
            if (Controller != null)
            {
                var curCol = Controller.LightBarColor;
                if(!c.Equals(curCol))
                {
                    Controller.LightBarColor = c;
                    Controller.LightBarOnDuration = 0xFF;
                    //Controller.LightBarOffDuration = 0x00;
                }
                
            }
        }

        /// <summary>
        /// Raised whenever a controller is connected.
        /// </summary>
        public void OnControllerConnected()
        {
        }

        public void Resume()
        {
            Suspended = false;
        }

        public void Rumble(byte strengthL, byte strengthR, int duration)
        {
            if (Controller != null)
            {
                Controller.setRumble(strengthL, strengthR);
                Thread.Sleep(duration);
                Controller.setRumble(0, 0);
            }
        }

        public void RumbleBig(byte strength, int duration)
        {
            if (Controller != null)
            {
                Controller.setRumble(0, strength);
                Thread.Sleep(duration);
                Controller.setRumble(0, 0);
            }
        }

        public void RumbleSmall(byte strength, int duration)
        {
            if (Controller != null)
            {
                Controller.setRumble(strength, 0);
                Thread.Sleep(duration);
                Controller.setRumble(0, 0);
            }
        }

        public void SetThreshold(DS4Axis Axis, float Threshold)
        {
            axisThresholds[(int)Axis] = Threshold;
        }

        public void Suspend()
        {
            Suspended = true;
        }

        #endregion Public Methods

        #region Private Methods

        private void DoButtonDown(DS4Button Button)
        {
            // if button already pressed, ignore
            if (buttonStates[(int)Button] == true) return;

            buttonStates[(int)Button] = true;
            Thread buttWatcher = new Thread(() =>
            {
                // Handle button events
                ButtonDown(Button);
                while (buttonStates[(int)Button])
                {
                    Thread.Sleep(5);
                }
                buttonStates[(int)Button] = false;
                ButtonUp(Button);
                return;
            });

            buttWatcher.Start();
        }

        private void DoButtonUp(DS4Button Button)
        {
            buttonStates[(int)Button] = false;
        }

        private bool EvaluateThreshold(float AxisValue, float Threshold)
        {
            if (AxisValue < 0) AxisValue = -AxisValue;
            return AxisValue > Threshold;
        }

        /// <summary>
        /// Raised whenever the specified <code>Axis</code> reaches the defined threshold
        /// </summary>
        /// <param name="Axis"></param>
        private void OnAxisOver(DS4Axis Axis)
        {
        }

        /// <summary>
        /// Raised whenever the specified <code>Axis</code> falls below the defined threshold
        /// </summary>
        /// <param name="Axis"></param>
        private void OnAxisUnder(DS4Axis Axis)
        {
        }

        /// <summary>
        /// Raised whenever a button is pressed.
        /// </summary>
        /// <param name="Button">The DS4Button that was pressed.</param>
        private void OnButtonDown(DS4Button Button)
        {
        }

        /// <summary>
        /// Raised whenever a button is released.
        /// </summary>
        /// <param name="Button">The DS4Button that was released.</param>
        private void OnButtonUp(DS4Button Button)
        {
        }
        /// <summary>
        /// Raised whenever a controller is disconnected.
        /// </summary>
        private void OnControllerDisconnected()
        {
        }
        private void StateMonitor()
        {
            DS4State state = new DS4State();
            while (true)
            {
                if (Controller != null && !Suspended)
                {
                    try
                    {
                        // Update controller state
                        state = Controller.getCurrentState();
                        var lState = Controller.getPreviousState();

                        // Face Buttons
                        if (state.Circle && lState.Circle)
                            DoButtonDown(DS4Button.Circle);
                        if (!state.Circle && buttonStates[(int)DS4Button.Circle])
                            DoButtonUp(DS4Button.Circle);

                        if (state.Cross && lState.Cross)
                            DoButtonDown(DS4Button.Cross);
                        if (!state.Cross && buttonStates[(int)DS4Button.Cross])
                            DoButtonUp(DS4Button.Cross);

                        if (state.Triangle && lState.Triangle)
                            DoButtonDown(DS4Button.Triangle);
                        if (!state.Triangle && buttonStates[(int)DS4Button.Triangle])
                            DoButtonUp(DS4Button.Triangle);

                        if (state.Square && lState.Square)
                            DoButtonDown(DS4Button.Square);
                        if (!state.Square && buttonStates[(int)DS4Button.Square])
                            DoButtonUp(DS4Button.Square);

                        // D-Pad Directions
                        if (state.DpadDown && lState.DpadDown)
                            DoButtonDown(DS4Button.DpadDown);
                        if (!state.DpadDown && buttonStates[(int)DS4Button.DpadDown])
                            DoButtonUp(DS4Button.DpadDown);

                        if (state.DpadUp && lState.DpadUp)
                            DoButtonDown(DS4Button.DpadUp);
                        if (!state.DpadUp && buttonStates[(int)DS4Button.DpadUp])
                            DoButtonUp(DS4Button.DpadUp);

                        if (state.DpadLeft && lState.DpadLeft)
                            DoButtonDown(DS4Button.DpadLeft);
                        if (!state.DpadLeft && buttonStates[(int)DS4Button.DpadLeft])
                            DoButtonUp(DS4Button.DpadLeft);

                        if (state.DpadRight && lState.DpadRight)
                            DoButtonDown(DS4Button.DpadRight);
                        if (!state.DpadRight && buttonStates[(int)DS4Button.DpadRight])
                            DoButtonUp(DS4Button.DpadRight);

                        // L1/R1 triggers
                        if (state.L1 && lState.L1)
                            DoButtonDown(DS4Button.L1);
                        if (!state.L1 && buttonStates[(int)DS4Button.L1])
                            DoButtonUp(DS4Button.L1);

                        if (state.R1 && lState.R1)
                            DoButtonDown(DS4Button.R1);
                        if (!state.R1 && buttonStates[(int)DS4Button.R1])
                            DoButtonUp(DS4Button.R1);

                        // Handle L2/R2 triggers as buttons
                        if (TriggerMode == TriggerMode.Buttons)
                        {
                            if (AxisToFloat(state.L2) > (TriggerSensitivity.L2 / 100))
                                DoButtonDown(DS4Button.L2);
                            if (!(AxisToFloat(state.L2) > (TriggerSensitivity.L2 / 100)) && buttonStates[(int)DS4Button.L2])
                                DoButtonUp(DS4Button.L2);
                            if (AxisToFloat(state.R2) > (TriggerSensitivity.R2 / 100))
                                DoButtonDown(DS4Button.R2);
                            if (!(AxisToFloat(state.R2) > (TriggerSensitivity.R2 / 100)) && buttonStates[(int)DS4Button.R2])
                                DoButtonUp(DS4Button.R2);
                        }

                        // L3/R3
                        if (state.L3 && lState.L3)
                            DoButtonDown(DS4Button.L3);
                        if (!state.L3 && buttonStates[(int)DS4Button.L3])
                            DoButtonUp(DS4Button.L3);

                        if (state.R3 && lState.R3)
                            DoButtonDown(DS4Button.R3);
                        if (!state.R3 && buttonStates[(int)DS4Button.R3])
                            DoButtonUp(DS4Button.R3);

                        // Share, Options, PS
                        if (state.Share && lState.Share)
                            DoButtonDown(DS4Button.Share);
                        if (!state.Share && buttonStates[(int)DS4Button.Share])
                            DoButtonUp(DS4Button.Share);

                        if (state.Options && lState.Options)
                            DoButtonDown(DS4Button.Options);
                        if (!state.Options && buttonStates[(int)DS4Button.Options])
                            DoButtonUp(DS4Button.Options);

                        if (state.PS && lState.PS)
                            DoButtonDown(DS4Button.PS);
                        if (!state.PS && buttonStates[(int)DS4Button.PS])
                            DoButtonUp(DS4Button.PS);

                        bool didTouch = false;

                        if (Controller.Touchpad.lastTouchPadY1 < 200 && state.TouchButton)
                        { DoButtonDown(DS4Button.TouchUpper); didTouch = true; }
                        if (!state.TouchButton && buttonStates[(int)DS4Button.TouchUpper])
                        { DoButtonUp(DS4Button.TouchUpper); didTouch = true; }

                        if (!didTouch)
                        {
                            // Touchpad click left/right
                            if (state.TouchLeft && state.TouchButton)
                                DoButtonDown(DS4Button.TouchLeft);
                            if (!state.TouchButton && buttonStates[(int)DS4Button.TouchLeft])
                                DoButtonUp(DS4Button.TouchLeft);

                            if (state.TouchRight && state.TouchButton)
                                DoButtonDown(DS4Button.TouchRight);
                            if (!state.TouchButton && buttonStates[(int)DS4Button.TouchRight])
                                DoButtonUp(DS4Button.TouchRight);
                        }

                        // Process axis threshold states
                        if (EvaluateThreshold(GetStickAxis(state.LX), axisThresholds[(int)DS4Axis.Lx]) && !axisStates[(int)DS4Axis.Lx])
                        {
                            AxisThresholdReached(DS4Axis.Lx);
                            axisStates[(int)DS4Axis.Lx] = true;
                        }

                        if (!EvaluateThreshold(GetStickAxis(state.LX), axisThresholds[(int)DS4Axis.Lx]) && axisStates[(int)DS4Axis.Lx])
                        {
                            AxisThresholdDropped(DS4Axis.Lx);
                            axisStates[(int)DS4Axis.Lx] = false;
                        }

                        if (EvaluateThreshold(GetStickAxis(state.LY), axisThresholds[(int)DS4Axis.Ly]) && !axisStates[(int)DS4Axis.Ly])
                        {
                            AxisThresholdReached(DS4Axis.Ly);
                            axisStates[(int)DS4Axis.Ly] = true;
                        }

                        if (!EvaluateThreshold(GetStickAxis(state.LY), axisThresholds[(int)DS4Axis.Ly]) && axisStates[(int)DS4Axis.Ly])
                        {
                            AxisThresholdDropped(DS4Axis.Ly);
                            axisStates[(int)DS4Axis.Ly] = false;
                        }

                        if (EvaluateThreshold(GetStickAxis(state.RX), axisThresholds[(int)DS4Axis.Rx]) && !axisStates[(int)DS4Axis.Rx])
                        {
                            AxisThresholdReached(DS4Axis.Rx);
                            axisStates[(int)DS4Axis.Rx] = true;
                        }

                        if (!EvaluateThreshold(GetStickAxis(state.RX), axisThresholds[(int)DS4Axis.Rx]) && axisStates[(int)DS4Axis.Rx])
                        {
                            AxisThresholdDropped(DS4Axis.Rx);
                            axisStates[(int)DS4Axis.Rx] = false;
                        }

                        if (EvaluateThreshold(GetStickAxis(state.RY), axisThresholds[(int)DS4Axis.Ry]) && !axisStates[(int)DS4Axis.Ry])
                        {
                            AxisThresholdReached(DS4Axis.Ry);
                            axisStates[(int)DS4Axis.Ry] = true;
                        }

                        if (!EvaluateThreshold(GetStickAxis(state.RY), axisThresholds[(int)DS4Axis.Ry]) && axisStates[(int)DS4Axis.Ry])
                        {
                            AxisThresholdDropped(DS4Axis.Ry);
                            axisStates[(int)DS4Axis.Ry] = false;
                        }
                    }
                    catch
                    {
                        // the controller was probably removed
                    }
                }

                Thread.Sleep(5);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    controllerThread.Abort();
                    stateThread.Abort();

                    // Disconnect controller and abort thread
                    if (Controller != null)
                    {
                        Controller.pushHapticState(new DS4HapticState()
                        {
                            LightBarExplicitlyOff = true,
                            RumbleMotorsExplicitlyOff = true
                        });
                        Controller.LightBarColor = new DS4Color(Color.Black);

                        Controller.StopUpdate();
                        if (Controller.ConnectionType == ConnectionType.BT)
                            Controller.DisconnectBT();

                        Controller = null;
                    }
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #endregion Private Methods
    }
}