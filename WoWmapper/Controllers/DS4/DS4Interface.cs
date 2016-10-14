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

        public DS4Interface()
        {
            Scan();
        }

        public void Scan()
        {
            //Log.WriteLine("Searching for DS4 controllers...");
            // Update controller list
            DS4Devices.findControllers();

            // Check validity of connected controllers
            var deadControllers = new List<IController>();

            foreach (var controller in Controllers)
            {
                if (controller.IsAlive()) continue;

                Log.WriteLine($"DS4 device {controller.Name} was disconnected.");
                controller.Stop();
                deadControllers.Add(controller);
            }

            // Remove disconnected devices
            if (deadControllers.Count > 0)
                Controllers.RemoveAll(c => deadControllers.Contains(c));

            
            var ds4Devices = DS4Devices.getDS4Controllers().ToArray();

            for (var i = 0; i < ds4Devices.Length; i++)
            {
                if (ds4Devices[i] != null &&
                    Controllers.Count(controller => controller.UnderlyingController == ds4Devices[i]) == 0)
                {
                    var controller = new DS4Controller(ds4Devices[i]);
                    Controllers.Add(controller);
                    Log.WriteLine($"DS4 device {ds4Devices[i].MacAddress} was connected.");
                }
            }
        }

        public void Shutdown()
        {
            Log.WriteLine
                ("Shutting down DS4 controllers...");

            DS4Devices.stopControllers();

            foreach (var c in Controllers)
            {
                c.Stop();
            }
        }
    }
}