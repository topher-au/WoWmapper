using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DS4Windows;

namespace WoWmapper.Controllers.DS4
{
    public class DS4Interface
    {
        public List<IController> Controllers = new List<IController>();

        private bool _active;
        private readonly Thread _controllerThread;

        public DS4Interface()
        {
            Log.WriteLine("DualShock 4 interface starting up");
            _active = true;
            _controllerThread = new Thread(ControllerScan) {IsBackground = true};
            _controllerThread.Start();
        }

        public void ControllerScan()
        {
            while (_active)
            {
                if(ControllerManager.ActiveController != null)
                    Thread.Sleep(2500);
                else
                    Thread.Sleep(500);
                
                // Check validity of connected controllers
                var deadControllers = new List<IController>();

                foreach (var controller in Controllers)
                {
                    if (controller.IsAlive()) continue;

                    Log.WriteLine($"DS4 disconnected: {controller.Name}");
                    controller.Stop();
                    deadControllers.Add(controller);
                }

                // Remove disconnected devices
                if(deadControllers.Count > 0)
                    Controllers.RemoveAll(c => deadControllers.Contains(c));

                // Update controller list
                DS4Devices.findControllers();
                var ds4Devices = DS4Devices.getDS4Controllers().ToArray();

                for (var i = 0; i < ds4Devices.Length; i++)
                {
                    if (ds4Devices[i] != null && Controllers.Count(controller => controller.UnderlyingController == ds4Devices[i]) == 0)
                    {
                        var controller = new DS4Controller(ds4Devices[i]);
                        Controllers.Add(controller);
                        Log.WriteLine($"DS4 connected: {ds4Devices[i].MacAddress}");
                    }
                }
            }

        }

        public void Shutdown()
        {
            Log.WriteLine("DualShock 4 interface shutting down");
            _active = false;

            foreach (var c in Controllers)
            {
                c.Stop();
            }
            DS4Devices.stopControllers();
        }
    }

}