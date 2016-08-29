using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace J2i.Net.XInputWrapper
{
    [StructLayout(LayoutKind.Explicit)]
    public struct  XInputState
    {
         [FieldOffset(0)]
        public int PacketNumber;

         [FieldOffset(4)]
        public XInputGamepad Gamepad;

         public void Copy(XInputState source)
         {
             PacketNumber = source.PacketNumber;
             Gamepad.Copy(source.Gamepad);
         }

         public override bool Equals(object obj)
         {
             if ((obj == null) || (!(obj is XInputState)))
                 return false;
             XInputState source = (XInputState)obj;

             return ((PacketNumber == source.PacketNumber)
                 && (Gamepad.Equals(source.Gamepad)));
         }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XInputStateSecret
    {
        public uint eventCount;
        public ushort wButtons;
        public byte bLeftTrigger;
        public byte bRightTrigger;
        public short sThumbLX;
        public short sThumbLY;
        public short sThumbRX;
        public short sThumbRY;
    }
}
