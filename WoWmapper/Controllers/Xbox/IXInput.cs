using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J2i.Net.XInputWrapper;
using Octokit;

namespace WoWmapper.Controllers.Xbox
{
    public interface IXInput
    {
        int GetState(int dwUserIndex, ref XInputState pState);
        int GetStateSecret(int dwUserIndex, out XInputStateSecret pStateSecret);
        int SetState(int dwUserIndex, ref XInputVibration pVibration);
        int GetCapabilities(int dwUserIndex, int dwFlags, ref XInputCapabilities pCapabilities);
        int GetBatteryInformation(int dwUserIndex, byte devType,
            ref XInputBatteryInformation pBatteryInformation);
    }
}
