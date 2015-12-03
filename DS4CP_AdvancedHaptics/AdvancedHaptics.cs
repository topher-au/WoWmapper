using DS4ConsolePort;
using DS4ConsolePort.WoWData;
using DS4Wrapper;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace DS4ConsolePort.AdvancedHaptics
{
    public class Haptics : IDisposable
    {
        public bool Enabled { get; set; } = false;
        //public DataReader wowDataReader;
        public WoWReader wowReader = new WoWReader();
        private DS4 hapticDevice;
        private Thread hapticThread, rumbleThread;

        public bool LightbarClass { get; set; } = true;
        public bool LightbarHealth { get; set; } = true;
        public bool RumbleOnDamage { get; set; } = true;
        public bool RumbleOnTarget { get; set; } = true;

        public WoWState GameState
        {
            get
            {
                    return wowReader.GameState;
            }
        }

        public bool IsWoWAttached
        {
            get
            {
                    return wowReader.IsAttached;
            }
        }

        public Colors HealthColors { get; set; } = new Colors() { High = Color.Green, Medium = Color.Yellow, Low = Color.Red, Critical = Color.Red };

        public struct Colors
        {
            public Color High;
            public Color Medium;
            public Color Low;
            public Color Critical;
        }

        public Haptics(DS4 Device)
        {
            hapticDevice = Device;
            hapticThread = new Thread(HapticThread);
            hapticThread.Start();
            rumbleThread = new Thread(RumbleThread);
            rumbleThread.Priority = ThreadPriority.Highest;
            rumbleThread.Start();
        }

        public void Dispose()
        {
            hapticThread.Abort();
            rumbleThread.Abort();
            wowReader.Dispose();
        }

        private void RumbleThread()
        {
            byte[] lastTarget = new byte[16];
            if (wowReader.IsAttached && wowReader.GameState == WoWState.LoggedIn)
                lastTarget = wowReader.CurrentTarget;

            float lastHealthPercent = 100;

            while (rumbleThread.ThreadState == System.Threading.ThreadState.Running)
            {
                if (wowReader.IsAttached && wowReader.GameState == WoWState.LoggedIn)
                {
                    // Buzz on change target
                    if (RumbleOnTarget)
                    {
                        var target = wowReader.CurrentTarget;

                        if (lastTarget != null && target != null) // if previous target exists
                            if (!target.SequenceEqual(lastTarget) && !target.SequenceEqual(new byte[16])) // ignore same target or null target
                            {
                                hapticDevice.RumbleSmall(200, 200);
                            }

                        lastTarget = target; // set last target
                    }
                    else
                    {
                        lastTarget = new byte[16];
                    }

                    if (RumbleOnDamage)
                    {
                        PlayerInfo playerInfo = wowReader.ReadPlayerInfo();

                        float currentHealthPct = ((float)playerInfo.CurrentHP / (float)playerInfo.MaxHP) * 100;

                        // Rumble controller on health loss
                        var healthLoss = lastHealthPercent - currentHealthPct;

                        if (healthLoss > 50)
                        {
                            hapticDevice.Rumble(255, 255, 1200);
                        }
                        else if (healthLoss > 30)
                        {
                            hapticDevice.Rumble(150, 150, 800);
                        }
                        else if (healthLoss > 10)
                        {
                            hapticDevice.Rumble(100, 100, 500);
                        }
                        else if (healthLoss > 0)
                        {
                            hapticDevice.Rumble(50, 50, 500);
                        }

                        lastHealthPercent = currentHealthPct;
                    }
                    else
                    {
                        lastHealthPercent = 100;
                    }
                }
                else
                {
                    lastHealthPercent = 100;
                }
                Thread.Sleep(1);
            }
        }

        private void HapticThread()
        {
            byte[] lastTarget = new byte[16];

            while (hapticThread.ThreadState == System.Threading.ThreadState.Running)
            {
                if (hapticDevice != null && Enabled)
                {
                    if (wowReader.IsAttached)
                    {
                        // Check if player is logged in
                        if (wowReader.GameState == WoWState.LoggedIn)
                        {
                            if (LightbarHealth)
                            {
                                // Read player data from memory
                                PlayerInfo playerInfo = wowReader.ReadPlayerInfo();

                                float currentHealthPct = ((float)playerInfo.CurrentHP / (float)playerInfo.MaxHP) * 100;

                                // Update lightbar based on player health
                                if (currentHealthPct > 90)
                                {
                                    if (LightbarClass)
                                    {
                                        var classColor = ClassColors.GetClassColor(playerInfo.Class);
                                        hapticDevice.LightBarOn(classColor);
                                    }
                                    else
                                    {
                                        hapticDevice.LightBarOn(HealthColors.High);
                                    }
                                }
                                else
                                if (currentHealthPct > 50)
                                {
                                    hapticDevice.LightBarOn(HealthColors.Medium);
                                }
                                else
                                if (currentHealthPct > 20)
                                {
                                    hapticDevice.LightBarOn(HealthColors.Low);
                                }
                                else if (currentHealthPct < 20)
                                {
                                    hapticDevice.LightBarFlash(HealthColors.Critical, 50, 50);
                                }
                            }
                            else if (!LightbarHealth && LightbarClass)
                            {
                                PlayerInfo playerInfo = wowReader.ReadPlayerInfo();
                                var classColor = ClassColors.GetClassColor(playerInfo.Class);
                                hapticDevice.LightBarOn(classColor);
                            }
                            else
                            {
                                hapticDevice.LightBarOff();
                            }
                        }
                        else
                        {
                            hapticDevice.LightBarOff();
                        }
                    }
                }
                else
                {
                    if (hapticDevice != null)
                        hapticDevice.LightBarOff();
                }

                Thread.Sleep(5);
            }
        }
    }
}