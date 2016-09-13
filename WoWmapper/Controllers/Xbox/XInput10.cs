using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using J2i.Net.XInputWrapper;

namespace WoWmapper.Controllers.Xbox
{
    public class XInput10 : IXInput
    {
        [DllImport("xinput1_4.dll")]
        private static extern int XInputGetState(int dwUserIndex, ref XInputState pState);

        [DllImport("xinput1_4.dll", EntryPoint = "#100")]
        private static extern int XInputGetStateSecret(int playerIndex, out XInputStateSecret struc);

        [DllImport("xinput1_4.dll")]
        private static extern int XInputSetState(int dwUserIndex, ref XInputVibration pVibration);

        [DllImport("xinput1_4.dll")]
        private static extern int XInputGetCapabilities(int dwUserIndex, int dwFlags,
            ref XInputCapabilities pCapabilities);

        [DllImport("xinput1_4.dll")]
        public static extern int XInputGetBatteryInformation(int dwUserIndex, byte devType,
            ref XInputBatteryInformation pBatteryInformation);

        public int GetState(int dwUserIndex, ref XInputState pState)
        {
            return XInputGetState(dwUserIndex, ref pState);
        }

        public int GetStateSecret(int dwUserIndex, out XInputStateSecret pStateSecret)
        {
            return XInputGetStateSecret(dwUserIndex, out pStateSecret);
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
            return XInputGetBatteryInformation(dwUserIndex, devType, ref pBatteryInformation);
        }
    }
}