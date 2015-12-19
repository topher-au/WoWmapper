using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WoWmapper.Input;

namespace WoWmapper
{
    class MapperModule
    {
        Thread windowScanner;
        Process wowProcess;
        bool attachedState;

        IInputPlugin inputDevice;

        public bool IsAttached
        {
            get
            {
                return attachedState;
            }
        }

        public MapperModule(IInputPlugin InputDevice)
        {
            attachedState = false;
            inputDevice = InputDevice;
            windowScanner = new Thread(WindowScannerThread);
            windowScanner.Start();
        }

        private void WindowScannerThread()
        {
            while(true)
            {
                // Check if we already have WoW process
                if(wowProcess != null)
                    if(wowProcess.HasExited)
                    {
                        // Process has exited, clean up
                        wowProcess.Dispose();
                        wowProcess = null;
                        attachedState = false;
                    }

                // Check if we need to find a WoW process
                if(wowProcess == null)
                {
                    Process newProcess = null;
                    bool foundProcess = FindWoWProcess(out newProcess);
                    if(foundProcess)
                    {
                        wowProcess = newProcess;
                        attachedState = true;
                    } else
                    {
                        attachedState = false;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private bool FindWoWProcess(out Process WoWProcess)
        {
            var processList = Process.GetProcessesByName(GetWoWImageName());
            if(processList.Length > 0)
            {
                // find first non-exited wow process
                WoWProcess = processList.First(process => process.HasExited == false);
                return true;
            }
            WoWProcess = null;
            return false;
        }

        /// <summary>
        /// Returns the appropriate image name for World of Warcraft based on process architecture
        /// </summary>
        /// <returns>The process image name</returns>
        private string GetWoWImageName()
        {
            switch(IntPtr.Size)
            {
                case 4:
                    return "WoW";
                case 8:
                    return "WoW-64";
                default:
                    return string.Empty;
            }
        }

        
    }
}
