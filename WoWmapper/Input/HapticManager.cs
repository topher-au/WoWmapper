using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using WoWmapper.Controllers;
using WoWmapper.MemoryReader;
using WoWmapper.WorldOfWarcraft;
using WoWmapper.Properties;

namespace WoWmapper.Input
{
    public static class HapticManager
    {
        private const int MouselookDelay = 20;
        private static int _playerHealthCurrent, _playerHealthMax;
        private static byte[] _targetGuid = new byte[16];
        private static byte[] _mouseGuid;
        private static bool _leftGrip, _rightGrip;
        private static Thread _hapticThread;

        public static void Start()
        {
            if (_hapticThread != null) return;

            _hapticThread = new Thread(HapticThread) { IsBackground = true };
            _hapticThread.Start();
        }

        public static void Stop()
        {
            if (_hapticThread == null) return;
            _hapticThread.Abort();
            _hapticThread = null;
        }

        private static void HapticThread()
        {
            var mouseLookTimer = 0;
            var wasMouseLooking = false;

            _playerHealthCurrent = int.MaxValue;
            _playerHealthMax = 0;
            while (true)
            {
                var setLightbar = false;
                var controller = ControllerManager.GetActiveController();
                if (controller == null) continue;
                lock (controller)
                {
                    var state = controller.GetControllerState();

                    // Color lightbar on low battery
                    if (Settings.Default.EnableColorLightbarBatteryLow)
                    {
                        if (controller.Battery < Settings.Default.LightbarBatteryThreshold &&
                            controller.BatteryState == ControllerBatteryState.Discharging)
                        {
                            var color = Settings.Default.LightbarColorBatteryLow;
                            controller.SetLightbar(color.R, color.G, color.B, 1000, 1000);
                            setLightbar = true;
                        }
                    }

                    // Read memory if enabled and update haptics
                    if (Settings.Default.EnableAdvancedFeatures && MemoryManager.Attached && MemoryManager.GetGameState() == GameState.LoggedIn)
                    {
                        var updatedPlayerInfo = MemoryManager.UpdatePlayerData();

                        if (updatedPlayerInfo)
                        {
                            PlayerInfo playerInfo;
                            var success = MemoryManager.GetPlayerInfo(out playerInfo);

                            //var targetGuid = MemoryManager.GetTargetGuid();
                            //var mouseGuid = MemoryManager.GetMouseGuid();

                            float previousPercent = (float)_playerHealthCurrent / playerInfo.MaxHealth;
                            float currentPercent = (float)playerInfo.CurrentHealth / playerInfo.MaxHealth;
                            var healthDiff = previousPercent - currentPercent;

                            // Vibrate when taking damage
                            if (Settings.Default.EnableVibrateDamage && playerInfo?.CurrentHealth < _playerHealthCurrent && playerInfo.MaxHealth >= _playerHealthMax && playerInfo?.CurrentHealth > 0)
                            {
                                if (healthDiff <= 0.10f) controller.SendRumble(0x60, 0x60, 250);
                                if (healthDiff <= 0.25f && healthDiff > 0.10f) controller.SendRumble(0x90, 0x90, 400);
                                if (healthDiff <= 0.50f && healthDiff > 0.25f) controller.SendRumble(0xB0, 0xB0, 650);
                                if (healthDiff <= 0.75f && healthDiff > 0.50f) controller.SendRumble(0xD0, 0xD0, 950);
                                if (healthDiff <= 1.00f && healthDiff > 0.75f) controller.SendRumble(0xFF, 0xFF, 1200);
                            }

                            _playerHealthCurrent = (int)playerInfo.CurrentHealth;
                            _playerHealthMax = (int)playerInfo.MaxHealth;

                            // Vibrate when changing target
                            //if (Settings.Default.EnableVibrateTarget && targetGuid != null && !_targetGuid.SequenceEqual(targetGuid) && !targetGuid.SequenceEqual(new byte[16]))
                            //{
                            //    controller.SendRumble(0, 255, 150);
                            //    _targetGuid = targetGuid;
                            //}

                            // Color lightbar by health
                            if (Settings.Default.EnableLightbarHealth && !setLightbar)
                            {
                                var critColor = Settings.Default.LightbarHealthCritical;
                                var lowColor = Settings.Default.LightbarHealthLow;
                                var medColor = Settings.Default.LightbarHealthMedium;
                                var highColor = Settings.Default.LightbarHealthHigh;

                                // Color by health
                                if (currentPercent <= 0.90)
                                {
                                    if (currentPercent <= 0.20) controller.SetLightbar(critColor.R, critColor.G, critColor.B);
                                    if (currentPercent <= 0.50 && currentPercent > 0.20) controller.SetLightbar(lowColor.R, lowColor.G, lowColor.B);
                                    if (currentPercent <= 0.90 && currentPercent > 0.50) controller.SetLightbar(medColor.R, medColor.G, medColor.B);
                                    setLightbar = true;
                                }

                                // High health color if not class color
                                if (currentPercent > 0.90f && !Settings.Default.EnableLightbarClass)
                                {
                                    controller.SetLightbar(highColor.R, highColor.G, highColor.B);
                                    setLightbar = true;
                                }
                            }

                            // Color lightbar by class
                            if (Settings.Default.EnableLightbarClass && !setLightbar)
                            {
                                Color color = new Color();
                                try
                                {
                                    color = GameInfo.RaidClassColors[MemoryManager.GetPlayerClass()];
                                }
                                catch { }


                                controller.SetLightbar(color.R, color.G, color.B);
                                setLightbar = true;
                            }

                            var isMouseLooking = MemoryManager.GetMouselooking();

                            if (isMouseLooking &&
                                ProcessWatcher.GetForegroundWindow() != ProcessWatcher.Process.MainWindowHandle)
                            {
                                Keymapper.DoMouseDown(MouseButtons.Right);
                                Keymapper.DoMouseDown(MouseButtons.Right);
                            }

                            // Auto-center cursor
                            if (Settings.Default.MouselookCenterCursor)
                            {
                                
                                if (isMouseLooking && !wasMouseLooking)
                                {
                                    if (mouseLookTimer > MouselookDelay)
                                    {
                                        wasMouseLooking = true;
                                        mouseLookTimer = 0;
                                        Console.WriteLine("Mouselook Set");
                                    }
                                    else
                                    {
                                        mouseLookTimer += 1;
                                    }

                                }
                                else if (!isMouseLooking)
                                {
                                    if (wasMouseLooking)
                                    {
                                        // Center cursor
                                        var winRect = ProcessWatcher.GameWindowRect;
                                        var cur = Cursor.Position;

                                        Console.WriteLine($"{cur.X}, {cur.Y}");

                                        var winWidth = winRect.Right - winRect.Left;
                                        var winHeight = winRect.Bottom - winRect.Top;

                                        cur.X = winRect.Left + (winWidth / 2);
                                        cur.Y = winRect.Top + (winHeight / 2);

                                        Console.WriteLine($"{cur.X}, {cur.Y}");

                                        Cursor.Position = cur;
                                    }
                                    wasMouseLooking = false;
                                    mouseLookTimer = 0;
                                }
                            }
                        }
                    }

                    // Vibrate on trigger grip
                    if (Settings.Default.EnableVibrateTriggerGrip && Settings.Default.EnableTriggerGrip)
                    {
                        if (state.TriggerLeft >= Settings.Default.ThresholdLeftClick && !_leftGrip)
                        {
                            controller.SendRumble(255, 255, 200);
                            _leftGrip = true;
                        }
                        else if (state.TriggerLeft < Settings.Default.ThresholdLeftClick && _leftGrip)
                        {
                            _leftGrip = false;
                        }

                        if (state.TriggerRight >= Settings.Default.ThresholdRightClick && !_rightGrip)
                        {
                            controller.SendRumble(255, 255, 200);
                            _rightGrip = true;
                        }
                        else if (state.TriggerRight < Settings.Default.ThresholdRightClick && _rightGrip)
                        {
                            _rightGrip = false;
                        }
                    }

                    if (Settings.Default.EnableColorLightbar && !setLightbar)
                    {
                        var color = Settings.Default.LightbarColorDefault;
                        controller.SetLightbar(color.R, color.G, color.B);
                        setLightbar = true;
                    }

                    if (!setLightbar)
                    {
                        controller.SetLightbar(0, 0, 0);
                    }
                }
                
                Thread.Sleep(10);
            }
        }
    }
}
