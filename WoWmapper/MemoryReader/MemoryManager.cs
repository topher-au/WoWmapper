using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Farplane.Memory;
using WoWmapper.Classes;
using WoWmapper.Offsets;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.MemoryReader
{
    public static class MemoryManager
    {
        #region Windows Native Methods

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess,
            IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private const int PROCESS_WM_READ = 0x10;

        #endregion

        private const int PlayerDataLength = 0x2E03;

        public static Process Process = null; // A Process object that represents the current instance of the game
        public static IntPtr MemoryHandle = IntPtr.Zero; // The OpenProcess handle used to read memory from the game
        private static IntPtr _moduleBase = IntPtr.Zero;
        private static bool _attached = false;
        private static byte[] _playerData = null;

        public static bool Attached => _attached;

        public static bool Attach(Process process)
        {
            if (_attached) return false;

            try
            {
                Process = process;

                var openProcessHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
                if (openProcessHandle != IntPtr.Zero)
                {
                    Process = process;
                    MemoryHandle = openProcessHandle;
                    _moduleBase = Process.MainModule.BaseAddress;

                    OffsetManager.InitializeOffsets();
                    
                    _attached = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            Process = null;
            MemoryHandle = IntPtr.Zero;
            _moduleBase = IntPtr.Zero;
            _attached = false;

            return false;
        }

        public static void Close()
        {
            if (!_attached) return;

            Process.Dispose();
            MemoryHandle = IntPtr.Zero;
            _moduleBase = IntPtr.Zero;
            _attached = false;
        }

        public static T Read<T>(IntPtr offset, bool isRelative = true)
        {
            var readOffset = offset;
            if (isRelative) readOffset += (int) _moduleBase;

            var readBuffer = new byte[Marshal.SizeOf<T>()];
            var bytesRead = 0;
            var success = ReadProcessMemory(MemoryHandle,
                readOffset, readBuffer, readBuffer.Length, ref bytesRead);
            if (success)
            {
                IntPtr outPtr = Marshal.AllocHGlobal(readBuffer.Length);
                try
                {
                    Marshal.Copy(readBuffer, 0, outPtr, readBuffer.Length);
                    var outT = Marshal.PtrToStructure<T>(outPtr);
                    return outT;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    Marshal.FreeHGlobal(outPtr);
                }
            }
            return default(T);
        }

        public static T[] Read<T>(IntPtr offset, int count, bool isRelative = true)
        {
            var tLength = Marshal.SizeOf<T>();
            var tArray = new T[count];

            for (int i = 0; i < count; i++)
            {
                var t = Read<T>(offset + tLength * i, isRelative);
                tArray[i] = t;
            }
            return tArray;
        }

        public static byte[] ReadBytes(IntPtr offset, int count, bool isRelative = true)
        {
            var readOffset = offset;
            if (isRelative) readOffset += (int)_moduleBase;

            var readBuffer = new byte[count];
            var bytesRead = 0;
            var success = ReadProcessMemory(MemoryHandle,
                readOffset, readBuffer, readBuffer.Length, ref bytesRead);
            if (success)
                return readBuffer;
            return default(byte[]);
        }

        private static IntPtr _base;
        public static PlayerInfo GetPlayerInfo()
        {
            if (!_attached) return null;
            try
            {
                if(_base == IntPtr.Zero)
                    _base = Read<IntPtr>(OffsetManager.Offset.PlayerBase, false);
                var playerData = ReadBytes(_base + 0xF0, 0x80, false);
                if (playerData == null) return null;
                var playerInfo = new PlayerInfo()
                {
                    CurrentHealth = BitConverter.ToUInt32(playerData, 0x00),
                    MaxHealth = BitConverter.ToUInt32(playerData, 0x20),
                    Level = (int)playerData[0x70]
                };
                return playerInfo;
            }
            catch (Exception ex)
            {
                Logger.Write($"GetPlayerInfo Exception: {ex}");
                return null;
            }
        }

        public static GameState GetGameState()
        {
            if (!_attached) return GameState.NotRunning;

            try
            {
                var bGameState = new byte[1];
                var bytesRead = 0;

                var success = ReadProcessMemory(MemoryHandle, OffsetManager.Offset.GameState,
                    bGameState, 1, ref bytesRead);

                if(success && bGameState[0] != (byte)GameState.LoggedIn) _base = IntPtr.Zero;
                
                return success ? (GameState) bGameState[0] : GameState.NotRunning;
            }
            catch (Exception ex)
            {
                Logger.Write($"GetGameState Exception: {ex}");
                return GameState.NotRunning;
            }
        }

        public static byte[] GetTargetGuid()
        {
            if (!_attached) return null;

            try
            {
                var oTargetGuid = OffsetManager.Offset.PlayerTarget;
                var bTargetGuid = new byte[16];
                var bytesRead = 0;

                var success = ReadProcessMemory(MemoryHandle, oTargetGuid,
                    bTargetGuid, 16, ref bytesRead);

                return success ? bTargetGuid : null;
            }
            catch (Exception ex)
            {
                Logger.Write($"GetTargetGuid Exception: {ex}");
                return null;
            }
        }

        public static byte[] GetMouseGuid()
        {
            return null;
            if (!_attached) return null;

            try
            {
                var bMouseGuid = new byte[16];
                var bytesRead = 0;

                //var success = ReadProcessMemory(MemoryHandle, OffsetManager.GetOffset(OffsetType.MouseGuid),
                //    bMouseGuid, 16, ref bytesRead);

                //return success ? bMouseGuid : null;
            }
            catch (Exception ex)
            {
                Logger.Write($"GetMouseGuid Exception: {ex}");
                return null;
            }
        }

        public static bool GetMouselooking()
        {
            if (!_attached) return false;

            try
            {
                var bMouseState = new byte[1];
                var bytesRead = 0;


                // Read the location of the mouse info from memory
                ReadProcessMemory(MemoryHandle,
                    OffsetManager.Offset.MouseLook, bMouseState,
                    bMouseState.Length, ref bytesRead);

                return (((int) bMouseState[0] & 1) == 1);
            }
            catch (Exception ex)
            {
                Logger.Write("GetMouselooking Exception: {0}", ex);
                return false;
            }
        }

        public static GameInfo.Classes GetPlayerClass()
        {
            if (!_attached) return GameInfo.Classes.None;
            

            try
            {
                var bPlayerClass = new byte[1];
                var bytesRead = 0;

                var success = ReadProcessMemory(MemoryHandle, OffsetManager.Offset.PlayerClass,
                    bPlayerClass, 1, ref bytesRead);


                return success ? (GameInfo.Classes) bPlayerClass[0] : GameInfo.Classes.None;
            }
            catch (Exception ex)
            {
                Logger.Write($"GetPlayerClass Exception: {ex}");
            }

            return GameInfo.Classes.None;
        }

        

        public static bool GetPlayerWalk()
        {
            if (!_attached) return false;

            try
            {
                var offsetPtr = MemoryManager.Read<IntPtr>(OffsetManager.Offset.PlayerWalk, false);
                var walkPtr = MemoryManager.Read<IntPtr>(offsetPtr + 0x210, false);
                var oPlayerWalk = walkPtr + 0x58;
                var bPlayerWalk = new byte[4];
                var bytesRead = 0;

                var success = ReadProcessMemory(MemoryHandle, oPlayerWalk,
                    bPlayerWalk, 4, ref bytesRead);


                return bPlayerWalk[1] == 1;

            }
            catch (Exception ex)
            {
                Logger.Write($"GetPlayerWalk Exception: {ex}");
            }

            return false;
        }

        public static bool GetPlayerAOE()
        {
            if (!_attached) return false;

            try
            {
                var offset = OffsetManager.Offset.PlayerAOE;
                var aoeOffset = MemoryManager.Read<IntPtr>(offset, false) + 0xC60;
                var aoeByte = Read<byte>(aoeOffset, false);
                
                return aoeByte == 1;

            }
            catch (Exception ex)
            {
                Logger.Write($"GetPlayerWalk Exception: {ex}");
            }

            return false;
        }

        internal static string GetPlayerName()
        {
            if (!_attached) return string.Empty;

            try
            {
                var bName = new byte[12];
                var bytesRead = 0;

                ReadProcessMemory(MemoryHandle,
                    OffsetManager.Offset.PlayerName, bName,
                    bName.Length, ref bytesRead);

                var nameLength = Array.FindIndex(bName, item => item == 0x00);

                if (nameLength > 0)
                    return Encoding.Default.GetString(bName, 0, nameLength);
                else
                    return Encoding.Default.GetString(bName, 0, bytesRead);
            }
            catch (Exception ex)
            {
                Logger.Write("GetPlayerName Exception: {0}", ex);
            }

            return string.Empty;
        }
    }
}