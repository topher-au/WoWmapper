using DS4Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace DS4Wrapper
{
    public class DS4 : IDisposable
    {
        // Events
        public event ControllerConnectedHandler ControllerConnected;
        public delegate void ControllerConnectedHandler();

        public event ControllerDisconnectedHandler ControllerDisconnected;
        public delegate void ControllerDisconnectedHandler();

        public event ButtonDownHandler ButtonDown;
        public delegate void ButtonDownHandler(DS4Button Button);

        public event ButtonDownHandler ButtonUp;
        public delegate void ButtonUpHandler(DS4Button Button);

        public event AxisThresholdHandler AxisThresholdReached;
        public delegate void AxisThresholdHandler(DS4Axis Axis);

        public event AxisThresholdDroppedHandler AxisThresholdDropped;
        public delegate void AxisThresholdDroppedHandler(DS4Axis Axis);

        // Public objects
        public DS4Device Controller; // the active controller
        public bool IsConnected; // is a controller connected?

        // Settings
        public TriggerMode TriggerMode { get; set; } // Triggers analog/digital
        public float TriggerSensitivity { get; set; } // Trigger sensitivity

        Thread controllerThread, stateThread;
        bool[] buttonStates, axisStates;
        float[] axisThresholds;
        

        public DS4()
        {
            // Start a thread to monitor the DS4 connection state
            controllerThread = new Thread(ControllerThread);
            controllerThread.Priority = ThreadPriority.AboveNormal;
            controllerThread.IsBackground = true;
            controllerThread.Start();

            // Monitor button presses
            stateThread = new Thread(StateMonitor);
            stateThread.Priority = ThreadPriority.AboveNormal;
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

            for(int i = 0; i<axisThresholds.Length;i++)
            {
                axisThresholds[i] = 0.8f; // default threshold
            }

            TriggerMode = TriggerMode.Buttons;
            TriggerSensitivity = 0.5f;
        }

        /// <summary>
        /// Raised whenever a button is released.
        /// </summary>
        /// <param name="Button">The DS4Button that was released.</param>
        private void OnButtonUp(DS4Button Button)
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
        /// Raised whenever a controller is connected.
        /// </summary>
        public void OnControllerConnected()
        {

        }

        /// <summary>
        /// Raised whenever a controller is disconnected.
        /// </summary>
        private void OnControllerDisconnected()
        {
            
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

        public void SetThreshold(DS4Axis Axis, float Threshold)
        {
            axisThresholds[(int)Axis] = Threshold;
        }

        public float GetThreshold(DS4Axis Axis)
        {
            return axisThresholds[(int)Axis];
        }

        public bool GetButtonState(DS4Button Button)
        {
            return buttonStates[(int)Button];
        }

        private void StateMonitor()
        {
            DS4State state = new DS4State();
            while(true)
            {
                if(Controller != null)
                {
                    try
                    {
                        // Update controller state
                        state = Controller.getCurrentState();

                        // Face Buttons
                        if (state.Circle)
                            DoButtonDown(DS4Button.Circle);
                        if (!state.Circle && buttonStates[(int)DS4Button.Circle])
                            DoButtonUp(DS4Button.Circle);

                        if (state.Cross)
                            DoButtonDown(DS4Button.Cross);
                        if (!state.Cross && buttonStates[(int)DS4Button.Cross])
                            DoButtonUp(DS4Button.Cross);

                        if (state.Triangle)
                            DoButtonDown(DS4Button.Triangle);
                        if (!state.Triangle && buttonStates[(int)DS4Button.Triangle])
                            DoButtonUp(DS4Button.Triangle);

                        if (state.Square)
                            DoButtonDown(DS4Button.Square);
                        if (!state.Square && buttonStates[(int)DS4Button.Square])
                            DoButtonUp(DS4Button.Square);

                        // D-Pad Directions
                        if (state.DpadDown)
                            DoButtonDown(DS4Button.DpadDown);
                        if (!state.DpadDown && buttonStates[(int)DS4Button.DpadDown])
                            DoButtonUp(DS4Button.DpadDown);

                        if (state.DpadUp)
                            DoButtonDown(DS4Button.DpadUp);
                        if (!state.DpadUp && buttonStates[(int)DS4Button.DpadUp])
                            DoButtonUp(DS4Button.DpadUp);

                        if (state.DpadLeft)
                            DoButtonDown(DS4Button.DpadLeft);
                        if (!state.DpadLeft && buttonStates[(int)DS4Button.DpadLeft])
                            DoButtonUp(DS4Button.DpadLeft);

                        if (state.DpadRight)
                            DoButtonDown(DS4Button.DpadRight);
                        if (!state.DpadRight && buttonStates[(int)DS4Button.DpadRight])
                            DoButtonUp(DS4Button.DpadRight);

                        // L1/R1 triggers
                        if (state.L1)
                            DoButtonDown(DS4Button.L1);
                        if (!state.L1 && buttonStates[(int)DS4Button.L1])
                            DoButtonUp(DS4Button.L1);

                        if (state.R1)
                            DoButtonDown(DS4Button.R1);
                        if (!state.R1 && buttonStates[(int)DS4Button.R1])
                            DoButtonUp(DS4Button.R1);

                        // Handle L2/R2 triggers as buttons
                        if (TriggerMode == TriggerMode.Buttons)
                        {
                            if (GetTriggerAxis(state.L2) > TriggerSensitivity)
                                DoButtonDown(DS4Button.L2);
                            if (!(GetTriggerAxis(state.L2) > TriggerSensitivity) && buttonStates[(int)DS4Button.L2])
                                DoButtonUp(DS4Button.L2);
                            if (GetTriggerAxis(state.R2) > TriggerSensitivity)
                                DoButtonDown(DS4Button.R2);
                            if (!(GetTriggerAxis(state.R2) > TriggerSensitivity) && buttonStates[(int)DS4Button.R2])
                                DoButtonUp(DS4Button.R2);
                        }

                        // L3/R3
                        if (state.L3)
                            DoButtonDown(DS4Button.L3);
                        if (!state.L3 && buttonStates[(int)DS4Button.L3])
                            DoButtonUp(DS4Button.L3);

                        if (state.R3)
                            DoButtonDown(DS4Button.R3);
                        if (!state.R3 && buttonStates[(int)DS4Button.R3])
                            DoButtonUp(DS4Button.R3);

                        // Share, Options, PS
                        if (state.Share)
                            DoButtonDown(DS4Button.Share);
                        if (!state.Share && buttonStates[(int)DS4Button.Share])
                            DoButtonUp(DS4Button.Share);

                        if (state.Options)
                            DoButtonDown(DS4Button.Options);
                        if (!state.Options && buttonStates[(int)DS4Button.Options])
                            DoButtonUp(DS4Button.Options);

                        if (state.PS)
                            DoButtonDown(DS4Button.PS);
                        if (!state.PS && buttonStates[(int)DS4Button.PS])
                            DoButtonUp(DS4Button.PS);

                        // Touchpad click left/right
                        if (state.TouchLeft && state.TouchButton)
                            DoButtonDown(DS4Button.TouchClickLeft);
                        if (!state.TouchButton && buttonStates[(int)DS4Button.TouchClickLeft])
                            DoButtonUp(DS4Button.TouchClickLeft);

                        if (state.TouchRight && state.TouchButton)
                            DoButtonDown(DS4Button.TouchClickRight);
                        if (!state.TouchButton && buttonStates[(int)DS4Button.TouchClickRight])
                            DoButtonUp(DS4Button.TouchClickRight);

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
                    } catch
                    {
                        // the controller was probably removed
                    }
                    
                }
                
                Thread.Sleep(5);
            }
        }

        public Point GetStickPoint(DS4Stick Stick)
        {
            if(Controller != null)
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
            
            return new Point(0,0);
        }

        public float GetStickAxis(byte Axis)
        {
            float StickAxis = Axis - 127;
            StickAxis *= 2;
            StickAxis /= 255;
            return StickAxis;
        }

        public float GetTriggerAxis(byte Axis)
        {
            float TriggerAxis = Axis / 255;
            return TriggerAxis;
        }

        private bool EvaluateThreshold(float AxisValue, float Threshold)
        {
            if (AxisValue < 0) AxisValue = -AxisValue;
            return AxisValue > Threshold;
        }

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

        public void Dispose()
        {
            // Disconnect controller and abort thread
            if(Controller != null)
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
            
            controllerThread.Abort();
            stateThread.Abort();
        }

        public void LightBarOn(Color c)
        {
            if (Controller != null && !Controller.LightBarColor.Equals(new DS4Color(c)))
            {
                    Controller.LightBarColor = new DS4Color(c);
                    Controller.LightBarOffDuration = 0;
                    Controller.LightBarOnDuration = 255;
            }
        }

        public void LightBarFlash(Color c, byte On, byte Off)
        {
            if (Controller != null && !(
                Controller.LightBarColor.Equals(new DS4Color(c)) &&
                Controller.LightBarOffDuration == Off &&
                Controller.LightBarOnDuration == On
                ))
            {
                Controller.LightBarColor = new DS4Color(c);
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
                Controller.LightBarOffDuration = 0xFF;
            }
        }

        public void Rumble(byte strengthL, byte strengthR, int duration)
        {
            if(Controller != null)
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
                    ControllerConnected();
                }
                Thread.Sleep(5);
            }
        }
    }
    public enum TriggerMode
    {
        Analog,
        Buttons
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
        TouchClickLeft,
        TouchClickRight
    }

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

    public enum DS4Stick
    {
        Left,
        Right
    }
}