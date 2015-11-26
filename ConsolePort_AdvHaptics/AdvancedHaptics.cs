using ConsolePort.WoWData;
using DS4Wrapper;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace ConsolePort_AdvHaptics
{
    public class Haptics : IDisposable
    {
        private DataReader wowDataReader;
        private DS4 hapticDevice;
        private Thread hapticThread;

        public bool LightbarClass { get; set; } = true;
        public bool LightbarHealth { get; set; } = true;
        public bool RumbleOnDamage { get; set; } = true;

        private byte[] lastTarget;

        public WoWState GameState
        {
            get
            {
                return wowDataReader.ReadState();
            }
        }

        public bool IsWoWAttached
        {
            get
            {
                return wowDataReader.Attached;
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
            hapticThread.Priority = ThreadPriority.Highest;
            hapticThread.Start();
        }

        public void Dispose()
        {
            wowDataReader.Dispose();
            hapticThread.Abort();
        }

        private void HapticThread()
        {
            // Thread for processing advanced haptic feedback
            if (wowDataReader == null) wowDataReader = new DataReader();
            // Retrieve data about game
            var playerMaxHealth = wowDataReader.ReadPlayerData<uint>(Constants.PlayerDataType.MaxHealth);
            var playerHealth = wowDataReader.ReadPlayerData<uint>(Constants.PlayerDataType.Health);

            float healthPercent = ((float)playerHealth / (float)playerMaxHealth) * 100;

            while (true)
            {
                if (wowDataReader.Attached)
                {
                    if (wowDataReader.ReadState() == WoWState.LoggedIn)
                    {
                        // Buzz on change target
                        var target = wowDataReader.GetTargetGuid();

                        if (lastTarget != null)
                            if (!target.SequenceEqual(lastTarget) && !target.SequenceEqual(new byte[16])) // same target or null target
                            {
                                hapticDevice.RumbleSmall(200, 200);
                            }
                        lastTarget = target;

                        // Retrieve data about game
                        playerMaxHealth = wowDataReader.ReadPlayerData<uint>(Constants.PlayerDataType.MaxHealth);
                        playerHealth = wowDataReader.ReadPlayerData<uint>(Constants.PlayerDataType.Health);

                        float currentHealthPct = ((float)playerHealth / (float)playerMaxHealth) * 100;

                        if (healthPercent != currentHealthPct)
                        {
                            // Rumble controller on health loss
                            var healthLoss = healthPercent - currentHealthPct;
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

                            // Update lightbar based on player health
                            if (currentHealthPct > 90)
                            {
                                if (LightbarClass)
                                {
                                    // TODO: colour lightbar by class
                                    hapticDevice.LightBarOn(this.HealthColors.High);
                                }
                                else
                                {
                                    hapticDevice.LightBarOn(this.HealthColors.High);
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

                            healthPercent = currentHealthPct;
                        }
                    }
                }
                else
                {
                    hapticDevice.LightBarOff();
                }
                Thread.Sleep(5);
            }
        }
    }
}