using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace J2i.Net.XInputWrapper
{
    [StructLayout(LayoutKind.Explicit)]
    public struct  XInputCapabilities
    {
        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(0)]
        byte Type;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(1)]
        public byte SubType;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(2)]
        public short Flags;

        
        [FieldOffset(4)]
        public XInputGamepad Gamepad;

        [FieldOffset(16)]
        public XInputVibration Vibration;
    }
}
