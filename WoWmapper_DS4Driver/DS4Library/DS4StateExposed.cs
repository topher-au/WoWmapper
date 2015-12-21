using System;

namespace DS4Windows
{
    public class DS4StateExposed
    {
        private DS4State _state;

        private byte[] accel = new byte[] { 0, 0, 0, 0, 0, 0 },
            gyro = new byte[] { 0, 0, 0, 0, 0, 0 };

        public DS4StateExposed()
        {
            _state = new DS4State();
        }

        public DS4StateExposed(DS4State state)
        {
            _state = state;
        }

        private bool Square
        { get { return _state.Square; } }
        private bool Triangle
        { get { return _state.Triangle; } }
        private bool Circle
        { get { return _state.Circle; } }
        private bool Cross
        { get { return _state.Cross; } }
        private bool DpadUp
        { get { return _state.DpadUp; } }
        private bool DpadDown
        { get { return _state.DpadDown; } }
        private bool DpadLeft
        { get { return _state.DpadLeft; } }
        private bool DpadRight
        { get { return _state.DpadRight; } }
        private bool L1
        { get { return _state.L1; } }
        private bool L3
        { get { return _state.L3; } }
        private bool R1
        { get { return _state.R1; } }
        private bool R3
        { get { return _state.R3; } }
        private bool Share
        { get { return _state.Share; } }
        private bool Options
        { get { return _state.Options; } }
        private bool PS
        { get { return _state.PS; } }
        private bool Touch1
        { get { return _state.Touch1; } }
        private bool Touch2
        { get { return _state.Touch2; } }
        private bool TouchButton
        { get { return _state.TouchButton; } }
        private byte LX
        { get { return _state.LX; } }
        private byte RX
        { get { return _state.RX; } }
        private byte LY
        { get { return _state.LY; } }
        private byte RY
        { get { return _state.RY; } }
        private byte L2
        { get { return _state.L2; } }
        private byte R2
        { get { return _state.R2; } }
        private int Battery
        { get { return _state.Battery; } }

        /// <summary> Holds raw DS4 input data from 14 to 19 </summary>
        public byte[] Accel { set { accel = value; } }

        /// <summary> Holds raw DS4 input data from 20 to 25 </summary>
        public byte[] Gyro { set { gyro = value; } }

        /// <summary> Yaw leftward/counter-clockwise/turn to port or larboard side </summary>
        /// <remarks> Add double the previous result to this delta and divide by three.</remarks>
        public int AccelX { get { return (Int16)((UInt16)(accel[2] << 8) | accel[3]) / 256; } }

        /// <summary> Pitch upward/backward </summary>
        /// <remarks> Add double the previous result to this delta and divide by three.</remarks>
        public int AccelY { get { return (Int16)((UInt16)(accel[0] << 8) | accel[1]) / 256; } }

        /// <summary> roll left/L side of controller down/starboard raising up </summary>
        /// <remarks> Add double the previous result to this delta and divide by three.</remarks>
        public int AccelZ { get { return (Int16)((UInt16)(accel[4] << 8) | accel[5]) / 256; } }

        /// <summary> R side of controller upward </summary>
        /// <remarks> Add double the previous result to this delta and divide by three.</remarks>
        public int GyroX { get { return (Int16)((UInt16)(gyro[0] << 8) | gyro[1]) / 64; } }

        /// <summary> touchpad and button face side of controller upward </summary>
        /// <remarks> Add double the previous result to this delta and divide by three.</remarks>
        public int GyroY { get { return (Int16)((UInt16)(gyro[2] << 8) | gyro[3]) / 64; } }

        /// <summary> Audio/expansion ports upward and light bar/shoulders/bumpers/USB port downward </summary>
        /// <remarks> Add double the previous result to this delta and divide by three.</remarks>
        public int GyroZ { get { return (Int16)((UInt16)(gyro[4] << 8) | gyro[5]) / 64; } }
    }
}