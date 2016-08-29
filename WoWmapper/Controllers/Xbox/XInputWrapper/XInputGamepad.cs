using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace J2i.Net.XInputWrapper
{
    [StructLayout(LayoutKind.Explicit)]
    public struct  XInputGamepad
    {
        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(0)]
        public short wButtons;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(2)]
        public byte bLeftTrigger;

        [MarshalAs(UnmanagedType.I1)]
        [FieldOffset(3)]
        public byte bRightTrigger;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(4)]
        public short sThumbLX;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(6)]
        public short sThumbLY;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(8)]
        public short sThumbRX;

        [MarshalAs(UnmanagedType.I2)]
        [FieldOffset(10)]
        public short sThumbRY;


        public bool IsButtonPressed(int buttonFlags)
        {
            return (wButtons & buttonFlags) == buttonFlags;
        }

        public bool IsButtonPresent(int buttonFlags)
        {
            return (wButtons & buttonFlags) == buttonFlags;
        }



        public void Copy(XInputGamepad source)
        {
            sThumbLX = source.sThumbLX;
            sThumbLY = source.sThumbLY;
            sThumbRX = source.sThumbRX;
            sThumbRY = source.sThumbRY;
            bLeftTrigger = source.bLeftTrigger;
            bRightTrigger = source.bRightTrigger;
            wButtons = source.wButtons;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is XInputGamepad))
                return false;
            XInputGamepad source = (XInputGamepad)obj;
            return ((sThumbLX == source.sThumbLX) 
            && (sThumbLY == source.sThumbLY)
            && (sThumbRX == source.sThumbRX)
            && (sThumbRY == source.sThumbRY)
            && (bLeftTrigger == source.bLeftTrigger)
            && (bRightTrigger == source.bRightTrigger)
            && (wButtons == source.wButtons)); 
        }
    }
}
