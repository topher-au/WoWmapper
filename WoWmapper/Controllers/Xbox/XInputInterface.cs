using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DS4Windows;
using J2i.Net.XInputWrapper;
using WoWmapper.Controllers.Xbox;

namespace WoWmapper.Controllers.DS4
{
    public class XInputInterface
    {
        public List<IController> Controllers = new List<IController>();

        private bool _active;
        private readonly Thread _controllerThread;

        public XInputInterface()
        {
            Log.WriteLine("XInput interface starting up");
            XboxController.StartPolling();
            _active = true;
            _controllerThread = new Thread(ControllerScan) { IsBackground = true };
            _controllerThread.Start();
        }

        public void ControllerScan()
        {
            while (_active)
            {
                // Check validity of connected controllers
                var deadControllers = new List<IController>();

                foreach (var controller in Controllers)
                {
                    if (controller.IsAlive()) continue;

                    Log.WriteLine($"XInput device {controller.Name} was disconnected.");
                    controller.Stop();
                    deadControllers.Add(controller);
                }

                // Remove disconnected devices
                if (deadControllers.Count > 0)
                    Controllers.RemoveAll(c => deadControllers.Contains(c));

                // Update controller list
                for (int i = 0; i < 4; i++)
                {
                    
                    var controller = XboxController.RetrieveController(i);
                    if (!controller.IsConnected || Controllers.Any(device => device.UnderlyingController == controller)) continue;
                    var xdevice = new XInputController(i);
                    Log.WriteLine($"XInput device {xdevice.Name} was connected.");
                    Controllers.Add(xdevice);
                }

                Thread.Sleep(2000);
            }

        }

        public void Shutdown()
        {
            Log.WriteLine("Shutting down XInput controllers...");
            XboxController.StopPolling();
            _active = false;

            foreach (var c in Controllers)
            {
                c.Stop();
            }
        }
    }

}