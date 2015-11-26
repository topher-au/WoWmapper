using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace ConsolePort.WoWData
{
    public class DataReader : IDisposable
    {
        MemorySharp wowMemory;
        int pidWow;
        IntPtr PlayerBase;

        public bool Attached { get; private set; }
        Thread watcher;

        public DataReader()
        {
            watcher = new Thread(WoWWatcher);
            watcher.Start();
            
        }

        public void Dispose()
        {
            watcher.Abort();
            if(wowMemory != null)
            {
                wowMemory.Dispose();
            }
        }

        public void WoWWatcher()
        {
            while(true && watcher.IsAlive)
            {
                if (wowMemory == null)
                {
                    Attached = TryAttach();
                } else if(!wowMemory.IsRunning)
                {
                    Attached = TryAttach();
                }                    

                Thread.Sleep(1000);
            }
        }

        public WoWState ReadState()
        {
            if(wowMemory != null && wowMemory.IsRunning)
            {
                try {
                    byte b = wowMemory.Read<byte>(Offsets.GameState, true);
                    return (WoWState)b;
                } catch { return WoWState.NotRunning; }
                
            } else
            {
                return WoWState.NotRunning;
            }
        }

        public PlayerInfo GetPlayerInfo()
        {
            if (wowMemory != null && wowMemory.IsRunning)
            {
                try
                {
                    // Read player info from memory
                    PlayerBase = wowMemory.Read<IntPtr>(Offsets.Player.LocalPlayer, true);
                    var playerName = wowMemory.ReadString(Offsets.Player.Name, true);
                    var playerClass = (Constants.WoWClass)wowMemory.Read<byte>(Offsets.Player.Class, true);
                    var playerLevel = ReadPlayerData<uint>(Constants.PlayerDataType.Level);
                    var playerCurrentHP = ReadPlayerData<uint>(Constants.PlayerDataType.Health);
                    var playerMaxHP = ReadPlayerData<uint>(Constants.PlayerDataType.MaxHealth);
                    var playerRealm = wowMemory.ReadString(wowMemory.Read<IntPtr>(Offsets.Player.RealmName, true)+8);
                    var playerAccount = wowMemory.ReadString(Offsets.Player.AccountName, true);
                    return new PlayerInfo()
                    {
                        Name = playerName,
                        Class = playerClass,
                        Level = playerLevel,
                        CurrentHP = playerCurrentHP,
                        MaxHP = playerMaxHP,
                        AccountName = playerAccount,
                        RealmName = playerRealm
                    };
                } catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Exception reading memory: {0}", ex.Message));
                }
                
            }

            return default(PlayerInfo);
        }


        public T ReadPlayerData<T>(Constants.PlayerDataType PlayerData)
        {
            if (wowMemory != null && wowMemory.IsRunning)
            {
                try
                {
                    T ReadData = wowMemory.Read<T>(wowMemory.Read<IntPtr>(PlayerBase + 0x08) + (int)PlayerData);
                    return ReadData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Exception reading memory: {0}", ex.Message));
                }
            }
            return default(T);
        }

        public byte[] GetTargetGuid()
        {
            if (wowMemory != null && wowMemory.IsRunning)
            {
                var bytes = new byte[16];
                try
                {
                    bytes = wowMemory.ReadBytes(wowMemory.Read<IntPtr>(PlayerBase + 0x08) + (int)Constants.PlayerDataType.Target, 16);
                } catch {
                    
                }
                
                return bytes;
            } else
            {
                return null;
            }
        }

        private bool TryAttach()
        {
            var WoWProcesses = Process.GetProcessesByName("WoW-64");
            if (WoWProcesses.Length > 0)
            {
                wowMemory = new MemorySharp(WoWProcesses.First());
                pidWow = wowMemory.Pid;
                return true;
            }
            return false;
        }
    }

    public enum WoWState : int
    {
        LoggedOut = 0x00,
        LoggedIn = 0x01,
        NotRunning = 0xFF
    }

    public struct PlayerInfo
    {
        public string Name;
        public Constants.WoWClass Class;
        public uint Level;
        public uint CurrentHP;
        public uint MaxHP;
        public string RealmName;
        public string AccountName;
    }
}
