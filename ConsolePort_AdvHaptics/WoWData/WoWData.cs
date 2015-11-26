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

        public uint rsdgf()
        {

            uint result = 0;
            if (wowMemory != null && wowMemory.IsRunning)
            {
                
                
                

            }

            return result;
        }

        public T ReadPlayerData<T>(Constants.PlayerDataType PlayerData)
        {
            try
            {
                T ReadData = wowMemory.Read<T>(wowMemory.Read<IntPtr>(PlayerBase + 0x08) + (int)PlayerData);
                return ReadData;
                
            } catch
            {
                return default(T);
            }
            
            
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
                PlayerBase = wowMemory.Read<IntPtr>(Offsets.Player.LocalPlayer, true);
                //var MyHealth = wowMemory.Read<uint>(wowMemory.Read<IntPtr>(MyBase + 0x8) + 0xF0);
                //var MaxHealth = wowMemory.Read<uint>(wowMemory.Read<IntPtr>(MyBase + 0x8) + 0x10C);
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
        int CurrentHP;
        int MaxHP;
    }
}
