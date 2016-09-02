using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using J2i.Net.XInputWrapper;

namespace WoWmapper.Controllers.Xbox
{
    public class XInput9 : IXInput
    {
        [DllImport("xinput9_1_0.dll")]
        private static extern int XInputGetState(int dwUserIndex, ref XInputState pState);

        [DllImport("xinput9_1_0.dll")]
        private static extern int XInputSetState(int dwUserIndex, ref XInputVibration pVibration);

        [DllImport("xinput9_1_0.dll")]
        private static extern int XInputGetCapabilities(int dwUserIndex, int dwFlags,
            ref XInputCapabilities pCapabilities);

        public int GetState(int dwUserIndex, ref XInputState pState)
        {
            return XInputGetState(dwUserIndex, ref pState);
        }

        public int GetStateSecret(int dwUserIndex, out XInputStateSecret pStateSecret)
        {
            pStateSecret = new XInputStateSecret();
            return 0;
        }

        public int SetState(int dwUserIndex, ref XInputVibration pVibration)
        {
            return XInputSetState(dwUserIndex, ref pVibration);
        }

        public int GetCapabilities(int dwUserIndex, int dwFlags, ref XInputCapabilities pCapabilities)
        {
            return XInputGetCapabilities(dwUserIndex, dwFlags, ref pCapabilities);
        }

        public int GetBatteryInformation(int dwUserIndex, byte devType, ref XInputBatteryInformation pBatteryInformation)
        {
            // This method is not available in this version of XInput
            return -1;
        }
    }
}