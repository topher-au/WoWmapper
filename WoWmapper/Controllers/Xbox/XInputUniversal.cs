using System;
using System.Runtime.InteropServices;
using J2i.Net.XInputWrapper;
using WoWmapper.Properties;

namespace WoWmapper.Controllers.Xbox
{
    public class XInputUniversal : IXInput
    {
        private readonly int dllIndex = -1;

        public XInputUniversal()
        {
            if (Settings.Default.XinputOverride)
            {
                dllIndex = Settings.Default.XinputDll;
                switch (dllIndex)
                {
                    case 0:
                        Log.WriteLine("Forcing Xinput library: xinput1_4.dll");
                        break;
                    case 1:
                        Log.WriteLine("Forcing Xinput library: xinput1_3.dll");
                        break;
                    case 2:
                        Log.WriteLine("Forcing Xinput library: xinput9_1_0.dll");
                        break;
                }
            }
            else
            {
                Log.WriteLine("Attempting to auto-detect Xinput library...");
                var x14exists = LibraryExists("xinput1_4.dll");
                var x13exists = LibraryExists("xinput1_3.dll");
                var x9exists = LibraryExists("xinput9_1_0.dll");


                if (x14exists)
                {
                    Log.WriteLine("Found xinput1_4.dll!");
                    dllIndex = 0;
                }
                else if (x13exists)
                {
                    Log.WriteLine("Found xinput1_3.dll!");
                    dllIndex = 1;
                }
                else if (x9exists)
                {
                    Log.WriteLine("Found xinput9_1_0.dll!");
                    dllIndex = 2;
                }
                else
                    Log.WriteLine("Unable to find a compatible Xinput library on this system. Xinput will not function.");
            }
        }

        public int GetState(int dwUserIndex, ref XInputState pState)
        {
            switch (dllIndex)
            {
                case 0:
                    return XInputGetState14(dwUserIndex, ref pState);
                case 1:
                    return XInputGetState13(dwUserIndex, ref pState);
                case 2:
                    return XInputGetState9(dwUserIndex, ref pState);
                default:
                    return 0;
            }
        }

        public int GetStateSecret(int dwUserIndex, out XInputStateSecret pStateSecret)
        {
            switch (dllIndex)
            {
                case 0:
                    return XInputGetStateSecret14(dwUserIndex, out pStateSecret);
                case 1:
                    return XInputGetStateSecret13(dwUserIndex, out pStateSecret);
                default:
                    pStateSecret = new XInputStateSecret();
                    return -1;
            }
        }

        public int SetState(int dwUserIndex, ref XInputVibration pVibration)
        {
            switch (dllIndex)
            {
                case 0:
                    return XInputSetState14(dwUserIndex, ref pVibration);
                case 1:
                    return XInputSetState13(dwUserIndex, ref pVibration);
                case 2:
                    return XInputSetState9(dwUserIndex, ref pVibration);
                default:
                    return 0;
            }
        }

        public int GetCapabilities(int dwUserIndex, int dwFlags, ref XInputCapabilities pCapabilities)
        {
            switch (dllIndex)
            {
                case 0:
                    return XInputGetCapabilities14(dwUserIndex, dwFlags, ref pCapabilities);
                case 1:
                    return XInputGetCapabilities13(dwUserIndex, dwFlags, ref pCapabilities);
                case 2:
                    return XInputGetCapabilities9(dwUserIndex, dwFlags, ref pCapabilities);
                default:
                    return 0;
            }
        }

        public int GetBatteryInformation(int dwUserIndex, byte devType, ref XInputBatteryInformation pBatteryInformation)
        {
            switch (dllIndex)
            {
                case 0:
                    return XInputGetBatteryInformation14(dwUserIndex, devType, ref pBatteryInformation);
                case 1:
                    return XInputGetBatteryInformation13(dwUserIndex, devType, ref pBatteryInformation);
                default:
                    return -1;
            }
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static bool LibraryExists(string fileName)
        {
            var result = LoadLibrary(fileName);
            if (result != IntPtr.Zero)
            {
                FreeLibrary(result);
                return true;
            }
            return false;
        }

        #region xinput1_3.dll

        [DllImport("xinput1_3.dll", EntryPoint = "XInputGetState")]
        private static extern int XInputGetState13(int dwUserIndex, ref XInputState pState);

        [DllImport("xinput1_3.dll", EntryPoint = "#100")]
        private static extern int XInputGetStateSecret13(int playerIndex, out XInputStateSecret struc);

        [DllImport("xinput1_3.dll", EntryPoint = "XInputSetState")]
        private static extern int XInputSetState13(int dwUserIndex, ref XInputVibration pVibration);

        [DllImport("xinput1_3.dll", EntryPoint = "XInputGetCapabilities")]
        private static extern int XInputGetCapabilities13(int dwUserIndex, int dwFlags,
            ref XInputCapabilities pCapabilities);

        [DllImport("xinput1_3.dll", EntryPoint = "XInputGetBatteryInformation")]
        public static extern int XInputGetBatteryInformation13(int dwUserIndex, byte devType,
            ref XInputBatteryInformation pBatteryInformation);

        #endregion

        #region xinput9_1_0.dll

        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputGetState")]
        private static extern int XInputGetState9(int dwUserIndex, ref XInputState pState);

        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputSetState")]
        private static extern int XInputSetState9(int dwUserIndex, ref XInputVibration pVibration);

        [DllImport("xinput9_1_0.dll", EntryPoint = "XInputGetCapabilities")]
        private static extern int XInputGetCapabilities9(int dwUserIndex, int dwFlags,
            ref XInputCapabilities pCapabilities);

        #endregion

        #region xinput1_4.dll

        [DllImport("xinput1_4.dll", EntryPoint = "XInputGetState")]
        private static extern int XInputGetState14(int dwUserIndex, ref XInputState pState);

        [DllImport("xinput1_4.dll", EntryPoint = "#100")]
        private static extern int XInputGetStateSecret14(int playerIndex, out XInputStateSecret struc);

        [DllImport("xinput1_4.dll", EntryPoint = "XInputSetState")]
        private static extern int XInputSetState14(int dwUserIndex, ref XInputVibration pVibration);

        [DllImport("xinput1_4.dll", EntryPoint = "XInputGetCapabilities")]
        private static extern int XInputGetCapabilities14(int dwUserIndex, int dwFlags,
            ref XInputCapabilities pCapabilities);

        [DllImport("xinput1_4.dll", EntryPoint = "XInputGetBatteryInformation")]
        public static extern int XInputGetBatteryInformation14(int dwUserIndex, byte devType,
            ref XInputBatteryInformation pBatteryInformation);

        #endregion
    }
}