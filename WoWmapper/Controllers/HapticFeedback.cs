using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DS4Windows;
using WoWmapper.Controllers.DS4;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft;
using WoWmapper.WoWInfoReader;

namespace WoWmapper.Controllers
{
    public class HapticFeedback
    {
        private static Thread _feedbackThread;

        public HapticFeedback()
        {
            _feedbackThread = new Thread(FeedbackThread);
            _feedbackThread.Start();
        }

        public void Abort()
        {
            _feedbackThread.Abort();
        }

        private void FeedbackThread()
        {
            long lastHealth = 0;
            long lastMax = 0;
            byte lastLevel = 0;

            while (_feedbackThread.ThreadState != ThreadState.Aborted)
            {
                if (WoWReader.IsAttached && 
                    WoWReader.GameState && 
                    ControllerManager.ActiveController != null)
                {
                    var playerHealth = WoWReader.PlayerHealth;
                    if (playerHealth != null)
                    {
                        // Check player health defecit
                        var maxHealth = playerHealth.Item2;
                        var currentHealth = playerHealth.Item1;
                        var healthLost = lastHealth - currentHealth;

                        if (Settings.Default.MemoryVibrationDamage && healthLost > 0 && maxHealth <= lastMax && maxHealth > 0)
                        {
                            // Player has taken damage
                            var damagePercent = (float)healthLost / lastHealth;

                            var leftStrength = (byte)(damagePercent * 200 + 55);
                            var rightStrength = (byte)(damagePercent * 200 + 55);
                            var duration = (int)(damagePercent * 2500 + 500);
                            Console.WriteLine($"Rumble {leftStrength} {rightStrength} {duration}");
                            ControllerManager.SendRumble(leftStrength, rightStrength, duration);
                        }
                        else if (Settings.Default.MemoryVibrationHealing && healthLost < 0 && lastHealth > 0)
                        {
                            if (-healthLost > (maxHealth/25))
                            {
                                healthLost = -(maxHealth/25);
                            }
                            // Player was healed
                            var damagePercent = (float)-healthLost / maxHealth;
                            var leftStrength = (byte)(damagePercent * 100 + 20);
                            var rightStrength = (byte)(damagePercent * 100 + 35);
                            var duration = (int)(damagePercent * 1500 + 500);

                            Console.WriteLine($"Rumble {leftStrength} {rightStrength} {duration}");
                            ControllerManager.SendRumble(leftStrength, rightStrength, duration);
                        }
                        lastHealth = currentHealth;
                        lastMax = maxHealth;

                        // Lightbar control
                        //var ds4 = ControllerManager.ActiveController.UnderlyingController as DS4Device;
                        //if (ds4 != null)
                        //{

                        //    var healthPercent = ((float)currentHealth / maxHealth) * 100;
                        //    if (healthPercent >= 90)
                        //    {
                        //        ds4.LightBarColor = new DS4Color(Color.Green);
                        //        ds4.LightBarOnDuration = 255;
                        //        ds4.LightBarOffDuration = 0;
                        //    }
                        //    else
                        //    {
                        //        ds4.LightBarColor = new DS4Color(Color.Red);
                        //        ds4.LightBarOnDuration = 255;
                        //        ds4.LightBarOffDuration = 0;
                        //    }
                        //}
                    }
                }
                
                Thread.Sleep(5);
            }
        }
    }
}