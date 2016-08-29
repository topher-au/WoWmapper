using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WoWmapper.Controllers.DS4;
using WoWmapper.Controllers.Xbox;
using WoWmapper.Keybindings;

namespace WoWmapper.Controllers
{
    public static class ControllerManager
    {
        #region Public Fields

        public static IController ActiveController;
        public static readonly List<IController> Controllers = new List<IController>();
        public static readonly DS4Interface DS4 = new DS4Interface();
        public static readonly XInputInterface XInput = new XInputInterface();

        #endregion

        #region Private Fields

        private static Thread _watcherThread = new Thread(ControllerWatcher);
        private static bool _threadRunning;

        #endregion

        #region Public Delegates

        public delegate void ControllerButtonStateChangedHandler(GamepadButton button, bool state);

        public delegate void ActiveControllerChangedHandler(IController controller);

        public delegate void ControllersChangedHandler();

        #endregion

        #region Public Events

        public static event ControllerButtonStateChangedHandler ControllerButtonStateChanged;
        public static event ControllersChangedHandler ControllersChanged;
        public static event ActiveControllerChangedHandler ActiveControllerChanged;

        #endregion

        #region Public Methods

        public static void Start()
        {
            _threadRunning = true;
            _watcherThread.Start();
        }

        public static void Stop()
        {
            _threadRunning = false;
            DS4.Shutdown();
            XInput.Shutdown();
        }

        public static BitmapImage GetButtonIcon(GamepadButton button)
        {
            switch (Properties.Settings.Default.ButtonStyle)
            {
                case 0:
                    if (ActiveController != null)
                    {
                        switch (ActiveController.Type)
                        {
                            case GamepadType.PlayStation:
                                return DS4Data.GetButtonImage(button);
                            case GamepadType.Xbox:
                                return XboxData.GetButtonImage(button);
                        }
                    }
                    goto default;
                case 1:
                    return DS4Data.GetButtonImage(button);
                case 2:
                    return XboxData.GetButtonImage(button);
                default:
                    return XboxData.GetButtonImage(button);
            }
        }

        public static string GetButtonName(GamepadButton button)
        {
            switch (Properties.Settings.Default.ButtonStyle)
            {
                case 0:
                    if (ActiveController != null)
                    {
                        switch (ActiveController.Type)
                        {
                            case GamepadType.PlayStation:
                                return DS4Data.GetButtonName(button);
                            case GamepadType.Xbox:
                                return XboxData.GetButtonName(button);
                        }
                    }
                    goto default;
                case 1:
                    return DS4Data.GetButtonName(button);
                case 2:
                    return XboxData.GetButtonName(button);
                default:
                    return XboxData.GetButtonName(button);
            }
        }

        public static void RefreshControllers()
        {
            var controllerList = new List<IController>();

            if (DS4 != null)
                controllerList.AddRange(DS4.Controllers);
            if (XInput != null)
                controllerList.AddRange(XInput.Controllers);

            foreach (var controller in controllerList)
            {
                if (!Controllers.Contains(controller) && controller != null)
                {
                    Controllers.Add(controller);
                    if (ControllersChanged != null)
                        Application.Current.Dispatcher.Invoke(ControllersChanged);
                }
            }
        }

        public static void SetController(IController controller)
        {
            if (ActiveController != null)
                ActiveController.ButtonStateChanged -= ActiveControllerOnButtonStateChanged;
            ActiveController = controller;

            if (controller != null)
            {
                ActiveController.ButtonStateChanged += ActiveControllerOnButtonStateChanged;
                Log.WriteLine("Selected controller: " + controller.Name);
            }

            if (ActiveControllerChanged != null)
                Application.Current.Dispatcher.Invoke(() => { ActiveControllerChanged?.Invoke(controller); });

            ConsolePort.BindWriter.WriteBinds();

            MainWindow.UpdateButtonStyle();
        }

        public static Point GetLeftAxis()
        {
            if (ActiveController == null) return new Point(0, 0);
            var x = ActiveController.GetAxis(GamepadAxis.StickLeftX);
            var y = ActiveController.GetAxis(GamepadAxis.StickLeftY);
            return new Point(x, y);
        }

        public static Point GetRightAxis()
        {
            if (ActiveController == null) return new Point(0, 0);
            var x = ActiveController.GetAxis(GamepadAxis.StickRightX);
            var y = ActiveController.GetAxis(GamepadAxis.StickRightY);
            return new Point(x, y);
        }

        public static void SendRumble(byte left, byte right, int duration)
        {
            ActiveController?.SendRumble(left, right, duration);
        }

        #endregion

        #region Private Methods

        private static void ActiveControllerOnButtonStateChanged(GamepadButton button, bool state)
        {
            ControllerButtonStateChanged?.Invoke(button, state);
        }

        private static void ControllerWatcher()
        {
            while (_threadRunning)
            {
                // Check validity of connected controllers
                var deadControllers = new List<IController>();

                foreach (var controller in Controllers)
                {
                    if (controller.IsAlive()) continue;
                    
                    deadControllers.Add(controller);
                }

                if (deadControllers.Contains(ActiveController))
                {
                    SetController(null);
                    Application.Current.Dispatcher.Invoke(() => { ActiveControllerChanged?.Invoke(null); });
                }


                // Remove disconnected devices
                for (int i = 0; i < deadControllers.Count; i++)
                {
                    deadControllers[i].Stop();
                    Controllers.Remove(deadControllers[i]);
                    deadControllers[i] = null;
                }

                if (deadControllers.Count > 0)
                {
                    if (ControllersChanged != null)
                        Application.Current.Dispatcher.Invoke(ControllersChanged);
                }


                if (ActiveController == null)
                {
                    RefreshControllers();
                    if (Controllers.Count > 0)
                    {
                        try
                        {
                            var firstActive = Controllers.First(device => device.IsAlive());
                            SetController(firstActive);
                        }
                        catch
                        {
                        }
                    }
                }

                Thread.Sleep(500);
            }
        }

        #endregion
    }
}