using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace WoWmapper.MapperModule
{
    internal class WindowScanner : IDisposable
    {
        private Thread scannerThread;
        private Process wowProcess;
        private bool attachedState;

        public WindowScanner()
        {
            scannerThread = new Thread(WindowScannerThread);
            scannerThread.Start();
        }

        public bool IsAttached
        {
            get
            {
                return attachedState;
            }
        }

        public IntPtr WindowHandle
        {
            get
            {
                if (attachedState)
                {
                    return wowProcess.MainWindowHandle;
                }
                return IntPtr.Zero;
            }
        }

        public WinApi.RECT WoWWindow
        {
            get
            {
                if (wowProcess != null)
                    if (wowProcess.MainWindowHandle != IntPtr.Zero)
                    {
                        WinApi.RECT wowRect = new WinApi.RECT();
                        var b = WinApi.GetWindowRect(new HandleRef(this, wowProcess.MainWindowHandle), out wowRect);
                        return wowRect;
                    }
                return default(WinApi.RECT);
            }
        }

        public Process WoWProcess
        {
            get
            {
                if (attachedState)
                {
                    return wowProcess;
                }
                return null;
            }
        }

        private bool FindWoWProcess(out Process WoWProcess)
        {
            var processList32 = Process.GetProcessesByName("WoW");
            var processList64 = Process.GetProcessesByName("WoW-64");

            if (processList32.Length > 0)
            {
                // find first non-exited wow process
                WoWProcess = processList32.First(process => process.HasExited == false);
                return true;
            }

            if (processList64.Length > 0)
            {
                // find first non-exited wow process
                WoWProcess = processList64.First(process => process.HasExited == false);
                return true;
            }

            WoWProcess = null;
            return false;
        }

        private string GetWoWImageName()
        {
            switch (IntPtr.Size)
            {
                case 4: // OS is 32-bit
                    return "WoW";

                case 8: // OS is 64-bit
                    return "WoW-64";

                default:
                    return string.Empty;
            }
        }

        private void WindowScannerThread()
        {
            while (true)
            {
                // Check if we already have WoW process
                if (wowProcess != null)
                    if (wowProcess.HasExited || !WinApi.IsWindow(wowProcess.MainWindowHandle))
                    {
                        // Process has exited, clean up
                        wowProcess.Dispose();
                        wowProcess = null;
                        attachedState = false;
                    }

                // Check if we need to find a WoW process
                if (wowProcess == null)
                {
                    Process newProcess = null;
                    bool foundProcess = FindWoWProcess(out newProcess);
                    if (foundProcess)
                    {
                        wowProcess = newProcess;
                        attachedState = true;
                    }
                    else
                    {
                        attachedState = false;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            if (scannerThread != null) scannerThread.Abort();
        }
    }
}