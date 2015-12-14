using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using WoWmapper.Input;
using WoWmapper.WoWData;

namespace WoWmapper.AdvancedHaptics
{
    public class HapticsModule : IDisposable
    {
        public bool Enabled { get; set; } = false;
        public WoWReader wowReader = new WoWReader();
        private IInputPlugin hapticDevice;
        private Thread lightbarThread, rumbleThread;

        public bool LedClass { get; set; } = true;
        public bool LedHealth { get; set; } = true;
        public bool RumbleDamage { get; set; } = true;
        public bool RumbleTarget { get; set; } = true;

        public bool LightbarOverride { get; set; } = false;

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

        public HapticsModule(IInputPlugin Device)
        {
            hapticDevice = Device;
            lightbarThread = new Thread(LightbarThread);
            lightbarThread.Start();
            rumbleThread = new Thread(RumbleThread);
            rumbleThread.Start();
        }

        public void Dispose()
        {
            hapticDevice.SetLEDOff();
            lightbarThread.Abort();
            rumbleThread.Abort();
            wowReader.Dispose();
        }

        private void RumbleThread()
        {
            // Load initial data
            byte[] lastTarget = new byte[16];
            if (wowReader.IsAttached && wowReader.GameState == WoWState.LoggedIn)
                lastTarget = wowReader.TargetGuid;

            float lastHealthPercent = 100;

            while (true)
            {
                if (wowReader.IsAttached && wowReader.GameState == WoWState.LoggedIn && Enabled)
                {
                    // Buzz on change target
                    if (RumbleTarget)
                    {
                        var target = wowReader.TargetGuid;

                        if (lastTarget != null && target != null) // if previous target exists
                            if (!target.SequenceEqual(lastTarget) && !target.SequenceEqual(new byte[16])) // ignore same target or null target
                            {
                                hapticDevice.SendRumble(InputRumbleMotor.Right, 200, 150);
                            }

                        lastTarget = target; // set last target
                    }
                    else
                    {
                        lastTarget = new byte[16];
                    }

                    if (RumbleDamage)
                    {
                        PlayerInfo playerInfo = wowReader.ReadPlayerInfo();

                        float currentHealthPct = ((float)playerInfo.CurrentHP / (float)playerInfo.MaxHP) * 100;

                        // Rumble controller on health loss
                        var healthLoss = lastHealthPercent - currentHealthPct;

                        if (healthLoss > 50)
                        {
                            hapticDevice.SendRumble(InputRumbleMotor.Both, 255, 1200);
                        }
                        else if (healthLoss > 30)
                        {
                            hapticDevice.SendRumble(InputRumbleMotor.Both, 220, 950);
                        }
                        else if (healthLoss > 10)
                        {
                            hapticDevice.SendRumble(InputRumbleMotor.Both, 180, 700);
                        }
                        else if (healthLoss > 0)
                        {
                            hapticDevice.SendRumble(InputRumbleMotor.Both, 100, 500);
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
                Thread.Sleep(100);
            }
        }

        private void LightbarThread()
        {
            byte[] lastTarget = new byte[16];

            while (true)
            {
                if (wowReader.IsAttached && 
                    wowReader.GameState == WoWState.LoggedIn && 
                    Enabled &&
                    !LightbarOverride)
                {
                    if (LedHealth)
                    {
                        // Read player data from memory
                        PlayerInfo playerInfo = wowReader.ReadPlayerInfo();

                        float currentHealthPct = ((float)playerInfo.CurrentHP / (float)playerInfo.MaxHP) * 100;

                        // Update lightbar based on player health
                        if (currentHealthPct > 90)
                        {
                            if (LedClass)
                            {
                                var classColor = ClassColors.GetClassColor(playerInfo.Class);
                                hapticDevice.SetLEDColor(classColor.R, classColor.G, classColor.B);
                            }
                            else
                            {
                                hapticDevice.SetLEDColor(HealthColors.High.R, HealthColors.High.G, HealthColors.High.B);
                            }
                        }
                        else
                        if (currentHealthPct > 50)
                        {
                            hapticDevice.SetLEDColor(HealthColors.Medium.R, HealthColors.Medium.G, HealthColors.Medium.B);
                        }
                        else
                        if (currentHealthPct > 20)
                        {
                            hapticDevice.SetLEDColor(HealthColors.Low.R, HealthColors.Low.G, HealthColors.Low.B);
                        }
                        else if (currentHealthPct < 20)
                        {
                            hapticDevice.SetLEDFlash(HealthColors.Critical.R, HealthColors.Critical.G, HealthColors.Critical.B, 100, 100);
                        }
                    }
                    else if (LedClass)
                    {
                        PlayerInfo playerInfo = wowReader.ReadPlayerInfo();
                        var classColor = ClassColors.GetClassColor(playerInfo.Class);
                        hapticDevice.SetLEDColor(classColor.R, classColor.B, classColor.G);
                    }
                    else
                    {
                        hapticDevice.SetLEDOff();
                    }
                }
                else
                {
                }

                Thread.Sleep(100);
            }
        }
    }
}