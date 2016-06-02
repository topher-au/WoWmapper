using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WoWmapper.Classes;
using WoWmapper.Offsets;
using WoWmapper.WorldOfWarcraft;

namespace WoWmapper.MemoryReader
{
    public static class MemoryManager
    {
        #region Windows Native Methods

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess,
            IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private const int PROCESS_WM_READ = 0x10;

        #endregion

        private const int PlayerDataLength = 0x2E03;

        private static Process _gameProcess = null;             // A Process object that represents the current instance of the game
        private static IntPtr _memoryHandle = IntPtr.Zero;      // The OpenProcess handle used to read memory from the game
        private static IntPtr _moduleBase = IntPtr.Zero;
        private static IntPtr _playerBase = IntPtr.Zero;
        private static bool _attached = false;
        private static byte[] _playerData = null;

        public static bool Attached => _attached;

        public static bool Attach(Process process)
        {
            if (_attached) return false;

            try
            {
                _gameProcess = process;
                var offsetsLoaded =
                    OffsetManager.InitializeOffsets(process.MainModule.FileVersionInfo.ProductPrivatePart);

                if (offsetsLoaded)
                {
                    var openProcessHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
                    if (openProcessHandle != IntPtr.Zero)
                    {
                        _gameProcess = process;
                        _memoryHandle = openProcessHandle;
                        _moduleBase = _gameProcess.MainModule.BaseAddress;
                        _attached = true;
                        return true;
                    }
                }
                

            }
            catch { }

            _gameProcess = null;
            _memoryHandle = IntPtr.Zero;
            _moduleBase = IntPtr.Zero;
            _attached = false;

            return false;
        }

        public static void Close()
        {
            if (!_attached) return;

            _gameProcess.Dispose();
            _memoryHandle = IntPtr.Zero;
            _moduleBase = IntPtr.Zero;
            _attached = false;
        }

        private static bool UpdatePlayerBase()
        {
            try
            {
                var bPlayerPointer = new byte[8];
                var bytesRead = 0;

                var success = ReadProcessMemory(_memoryHandle,
                    _moduleBase + OffsetManager.GetOffset(OffsetType.PlayerBase), bPlayerPointer,
                    bPlayerPointer.Length, ref bytesRead);

                if (!success) throw new InvalidOperationException("Unable to read player pointer");

                var playerPointer = (IntPtr) BitConverter.ToInt64(bPlayerPointer, 0);

                success = ReadProcessMemory(_memoryHandle, playerPointer + 0x08, bPlayerPointer, bPlayerPointer.Length, ref bytesRead);

                if (success)
                {
                    _playerBase = (IntPtr) BitConverter.ToInt64(bPlayerPointer, 0);
                    return true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                _playerBase = IntPtr.Zero;
                Logger.Write($"UpdatePlayerBase Exception: {ex}");
                return false;
            }
        }

        public static bool UpdatePlayerData()
        {
            if (!_attached) return false;

            // Update player base pointer
            var update = UpdatePlayerBase();

            if (!update) return false;

            try
            {
                var bPlayerInfo = new byte[PlayerDataLength];
                var bytesRead = 0;

                var success = ReadProcessMemory(_memoryHandle, _playerBase, bPlayerInfo, PlayerDataLength, ref bytesRead);

                if (!success) return false;

                _playerData = bPlayerInfo;
                return true;
            }
            catch (Exception ex)
            {
                Logger.Write($"UpdatePlayerData Exception: {ex}");
                _playerData = null;
                return false;
            }
            
        }

        public static GameState GetGameState()
        {
            if (!_attached) return GameState.NotRunning;

            try
            {
                var bGameState = new byte[1];
                var bytesRead = 0;

                var success = ReadProcessMemory(_memoryHandle, _moduleBase + OffsetManager.GetOffset(OffsetType.GameState),
                    bGameState, 1, ref bytesRead);


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
                var bTargetGuid = new byte[16];
                var bytesRead = 0;

                var success = ReadProcessMemory(_memoryHandle, _moduleBase + OffsetManager.GetOffset(OffsetType.TargetGuid),
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
            if (!_attached) return null;

            try
            {
                var bMouseGuid = new byte[16];
                var bytesRead = 0;

                var success = ReadProcessMemory(_memoryHandle, _moduleBase + OffsetManager.GetOffset(OffsetType.MouseGuid),
                    bMouseGuid, 16, ref bytesRead);

                return success ? bMouseGuid : null;
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
                ReadProcessMemory(_memoryHandle,
                    _moduleBase + OffsetManager.GetOffset(OffsetType.MouseLook), bMouseState,
                    bMouseState.Length, ref bytesRead);

                return (((int)bMouseState[0] & 1) == 1);
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

                var success = ReadProcessMemory(_memoryHandle, _moduleBase + OffsetManager.GetOffset(OffsetType.PlayerClass),
                    bPlayerClass, 1, ref bytesRead);


                return success ? (GameInfo.Classes) bPlayerClass[0] : GameInfo.Classes.None;
            }
            catch (Exception ex)
            {
                Logger.Write($"GetPlayerClass Exception: {ex}");
            }

            return GameInfo.Classes.None;
        }

        public static bool GetPlayerInfo(out PlayerInfo playerInfo)
        {
            if (_playerData == null)
            {
                var update = UpdatePlayerData();

                if (!update)
                {
                    playerInfo = null;
                    return false;
                }
            }
            else
            {
                playerInfo = new PlayerInfo()
                {
                    CurrentHealth = BitConverter.ToUInt32(_playerData, OffsetManager.GetOffset(OffsetType.PlayerHealthCurrent)),
                    MaxHealth = BitConverter.ToUInt32(_playerData, OffsetManager.GetOffset(OffsetType.PlayerHealthMax)),
                    Level = (int)_playerData[OffsetManager.GetOffset(OffsetType.PlayerLevel)]
                };

                return true;
            }
            playerInfo = null;
            return false;
        }

        internal static string GetPlayerName()
        {
            if (!_attached) return string.Empty;

            try
            {
                var bName = new byte[12];
                var bytesRead = 0;

                ReadProcessMemory(_memoryHandle,
                    _moduleBase + OffsetManager.GetOffset(OffsetType.PlayerName), bName,
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
