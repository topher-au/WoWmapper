using System;
using System.Drawing;
using System.Windows.Forms;
using WoWmapper.Input;

namespace WoWmapper.MapperModule
{
    internal class WoWMapper : IDisposable
    {
        private WindowScanner windowScanner;
        private InputMapper inputMapper;
        private InputMonitor inputMonitor;
        private int touchState, lastTouchX, lastTouchY;

        private IInputPlugin inputDevice;

        public WoWMapper(IInputPlugin InputDevice)
        {
            inputDevice = InputDevice;

            windowScanner = new WindowScanner(); // Watches for WoW window/process
            inputMapper = new InputMapper(windowScanner); // Handles input conversion to the game
            inputMonitor = new InputMonitor(inputDevice, inputMapper); // Watches for controller input

            inputDevice.Settings.Settings.Read("TouchMode", out touchState);

            inputDevice.OnButtonDown += DoButtonDown;
            inputDevice.OnButtonUp += DoButtonUp;

            if (inputDevice.Peripherals.Touchpad)
                inputDevice.OnTouchpadMoved += DoTouchPad;

            int tLeft, tRight;
            inputDevice.Settings.Settings.Read("TriggerLeft", out tLeft);
            inputDevice.Settings.Settings.Read("TriggerRight", out tRight);

            inputDevice.Thresholds = new InputThresholds()
            {
                TriggerLeft = tLeft,
                TriggerRight = tRight
            };
            inputDevice.Enabled = true;
        }

        private void DoTouchPad(InputTouchpadEventArgs e)
        {
            if (touchState == 0 && windowScanner.IsAttached) // Mouse Control
            {
                var x = e.touches[0].DeltaX;
                var y = e.touches[0].DeltaY;
                var curPos = Cursor.Position;
                if (x != lastTouchX)
                {
                    curPos.X += x;
                    lastTouchX = x;
                }
                if (y != lastTouchY)
                {
                    curPos.Y += y;
                    lastTouchY = y;
                }
                Cursor.Position = curPos;
            }
        }

        public void Dispose()
        {
            windowScanner.Dispose();
            inputMonitor.Dispose();
        }

        private void DoButtonDown(InputButton Button)
        {
            if (windowScanner.IsAttached || !Properties.Settings.Default.InactiveDisable)
                switch (Button)
                {
                    case InputButton.RFaceDown:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("RFaceDown").Key.Value);
                        break;

                    case InputButton.RFaceRight:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("RFaceRight").Key.Value);
                        break;

                    case InputButton.RFaceUp:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("RFaceUp").Key.Value);
                        break;

                    case InputButton.RFaceLeft:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("RFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceDown:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("LFaceDown").Key.Value);
                        break;

                    case InputButton.LFaceLeft:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("LFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceRight:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("LFaceRight").Key.Value);
                        break;

                    case InputButton.LFaceUp:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("LFaceUp").Key.Value);
                        break;

                    case InputButton.BumperLeft:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("BumperLeft").Key.Value);
                        break;

                    case InputButton.BumperRight:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("BumperRight").Key.Value);
                        break;

                    case InputButton.TriggerLeft:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("TriggerLeft").Key.Value);
                        break;

                    case InputButton.TriggerRight:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("TriggerRight").Key.Value);
                        break;

                    case InputButton.CenterMiddle:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("CenterMiddle").Key.Value);
                        break;

                    case InputButton.CenterLeft:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                        break;

                    case InputButton.CenterRight:
                        inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                        break;

                    case InputButton.Extra1:
                        DoTouchDown(Button);
                        break;

                    case InputButton.Extra2:
                        DoTouchDown(Button);
                        break;

                    case InputButton.Extra3:
                        DoTouchUpper();
                        break;

                    case InputButton.StickLeft:
                        inputMapper.SendMouseDown(MouseButtons.Left);
                        break;

                    case InputButton.StickRight:
                        if (inputMapper.IsKeyDown(Keys.LControlKey))
                        {
                            var wowWidth = windowScanner.WoWWindow.Right - windowScanner.WoWWindow.Left;
                            var wowHeight = windowScanner.WoWWindow.Bottom - windowScanner.WoWWindow.Top;

                            Cursor.Position = new Point(windowScanner.WoWWindow.Left + (wowWidth / 2), windowScanner.WoWWindow.Top + (wowHeight / 2));
                        }
                        else
                        {
                            inputMapper.SendMouseDown(MouseButtons.Right);
                        }
                        break;
                }
        }

        public bool IsAttached
        {
            get
            {
                return windowScanner.IsAttached;
            }
        }

        private void DoButtonUp(InputButton Button)
        {
            if (windowScanner.IsAttached || !Properties.Settings.Default.InactiveDisable)
                switch (Button)
                {
                    case InputButton.RFaceDown:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("RFaceDown").Key.Value);
                        break;

                    case InputButton.RFaceRight:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("RFaceRight").Key.Value);
                        break;

                    case InputButton.RFaceUp:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("RFaceUp").Key.Value);
                        break;

                    case InputButton.RFaceLeft:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("RFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceDown:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("LFaceDown").Key.Value);
                        break;

                    case InputButton.LFaceLeft:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("LFaceLeft").Key.Value);
                        break;

                    case InputButton.LFaceRight:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("LFaceRight").Key.Value);
                        break;

                    case InputButton.LFaceUp:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("LFaceUp").Key.Value);
                        break;

                    case InputButton.BumperLeft:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("BumperLeft").Key.Value);
                        break;

                    case InputButton.BumperRight:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("BumperRight").Key.Value);
                        break;

                    case InputButton.TriggerLeft:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("TriggerLeft").Key.Value);
                        break;

                    case InputButton.TriggerRight:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("TriggerRight").Key.Value);
                        break;

                    case InputButton.CenterMiddle:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("CenterMiddle").Key.Value);
                        break;

                    case InputButton.CenterLeft:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                        break;

                    case InputButton.CenterRight:
                        inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                        break;

                    case InputButton.Extra1:
                        DoTouchUp(Button);
                        break;

                    case InputButton.Extra2:
                        DoTouchUp(Button);
                        break;

                    case InputButton.Extra3:
                        DoTouchUpper();
                        break;

                    case InputButton.StickLeft:
                        inputMapper.SendMouseUp(MouseButtons.Left);
                        break;

                    case InputButton.StickRight:
                        inputMapper.SendMouseUp(MouseButtons.Right);
                        break;
                }
        }

        private void DoTouchDown(InputButton Button)
        {
            switch (Button)
            {
                case InputButton.Extra1:
                    switch (touchState)
                    {
                        case 0:
                            inputMapper.SendMouseDown(MouseButtons.Left);
                            break;

                        case 1:
                            inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("Extra1").Key.Value);
                            break;

                        case 2:
                            inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                            break;
                    }
                    break;

                case InputButton.Extra2:
                    switch (touchState)
                    {
                        case 0:
                            inputMapper.SendMouseDown(MouseButtons.Right);
                            break;

                        case 1:
                            inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("Extra2").Key.Value);
                            break;

                        case 2:
                            inputMapper.SendKeyDown(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                            break;
                    }
                    break;
            }
        }

        private void DoTouchUp(InputButton Button)
        {
            switch (Button)
            {
                case InputButton.Extra1:
                    switch (touchState)
                    {
                        case 0:
                            inputMapper.SendMouseUp(MouseButtons.Left);
                            break;

                        case 1:
                            inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("Extra1").Key.Value);
                            break;

                        case 2:
                            inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("CenterLeft").Key.Value);
                            break;
                    }
                    break;

                case InputButton.Extra2:
                    switch (touchState)
                    {
                        case 0:
                            inputMapper.SendMouseUp(MouseButtons.Right);
                            break;

                        case 1:
                            inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("Extra2").Key.Value);
                            break;

                        case 2:
                            inputMapper.SendKeyUp(inputDevice.Keybinds.FromName("CenterRight").Key.Value);
                            break;
                    }
                    break;
            }
        }

        private void DoTouchUpper()
        {
            if (touchState != 0)
            {
                touchState = 0;
            }
            else
            {
                inputDevice.Settings.Settings.Read("TouchMode", out touchState);
            }
        }
    }
}