using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using WoWmapper.Properties;

namespace WoWmapper.WoWInfoReader
{
    public static class WoWReader
    {
        private static IntPtr _handle;
        public static IntPtr offsetAoeState;
        public static IntPtr offsetGameState;
        public static IntPtr offsetMouselookState;
        public static IntPtr offsetPlayerBase;
        public static IntPtr offsetWalkState;
        private static Process _process;
        private static bool _offsetsLoaded;

        public static bool IsAttached => Settings.Default.EnableMemoryReading &&
                                         (_handle != IntPtr.Zero) &&
                                         (_process != null) && _offsetsLoaded;

        public static Tuple<long, long> PlayerHealth => ReadPlayerHealth();

        public static bool MouselookState => ReadMouselook();

        public static bool GameState => ReadGameState();

        public static int MovementState => ReadMovementState();

        public static bool AoeState => ReadAoeState();

        public static void Open(Process wowProcess)
        {
            if (IsAttached) Close();

            // Test process architecture
            bool isWin32;
            IsWow64Process(wowProcess.Handle, out isWin32);

            if (isWin32)
                throw new Exception("The selected process must be 64-bit.");

            // Attempt to open for memory reading
            var hProc = OpenProcess(ProcessAccessFlags.VirtualMemoryRead, false, wowProcess.Id);
            if (hProc == IntPtr.Zero)
                throw new Exception("Unable to open process for memory reading.");
            
            _process = wowProcess;
            _handle = hProc;

            LoadOffsets(wowProcess);
        }

        public static void Clear()
        {
            offsetMouselookState = IntPtr.Zero;
            offsetWalkState = IntPtr.Zero;
            offsetAoeState = IntPtr.Zero;
            offsetGameState  = IntPtr.Zero;
            offsetPlayerBase = IntPtr.Zero;
            _offsetsLoaded = false;
        }

        public static void Close()
        {
            Clear();
            CloseHandle(_handle);
            _handle = IntPtr.Zero;
            _process = null;
        }

        private static void LoadOffsets(Process process)
        {
            var scanner = new SigScan(process, process.MainModule.BaseAddress,
                process.MainModule.ModuleMemorySize);

            try
            {
                offsetGameState = scanner.FindPattern(OffsetPattern.GameState.Pattern, OffsetPattern.GameState.Offset);
                if (offsetGameState == IntPtr.Zero) throw new Exception("Unable to match pattern for GameState");
                offsetGameState = ReadPointer(offsetGameState, 1);

                offsetAoeState = scanner.FindPattern(OffsetPattern.AoeState.Pattern);
                if (offsetAoeState == IntPtr.Zero) throw new Exception("Unable to match pattern for AoeState");
                offsetAoeState = offsetAoeState + OffsetPattern.AoeState.Offset;

                offsetMouselookState = scanner.FindPattern(OffsetPattern.MouselookState.Pattern);
                if (offsetMouselookState == IntPtr.Zero)
                    throw new Exception("Unable to match pattern for MouselookState");
                offsetMouselookState = ReadPointer(offsetMouselookState + OffsetPattern.MouselookState.Offset, 4);

                offsetWalkState = scanner.FindPattern(OffsetPattern.WalkState.Pattern);
                if (offsetWalkState == IntPtr.Zero) throw new Exception("Unable to match pattern for WalkState");
                offsetWalkState = offsetWalkState + OffsetPattern.WalkState.Offset;

                offsetPlayerBase = scanner.FindPattern(OffsetPattern.PlayerBase.Pattern);
                if (offsetPlayerBase == IntPtr.Zero) throw new Exception("Unable to match pattern for PlayerBase");
                offsetPlayerBase = ReadPointer(offsetPlayerBase + OffsetPattern.PlayerBase.Offset);

                _offsetsLoaded = true;
                
                Log.WriteLine($"Offset scan was successful!\n" +
                              $"GameState:      {offsetGameState.ToString("X2")}\n" +
                              $"AoeState:       {offsetAoeState.ToString("X2")}\n" +
                              $"MouselookState: {offsetMouselookState.ToString("X2")}\n" +
                              $"WalkState:      {offsetWalkState.ToString("X2")}\n" +
                              $"PlayerBase:     {offsetPlayerBase.ToString("X2")}\n");
            }
            catch (Exception ex)
            {
                Log.WriteLine("Offset scan failed: " + ex.Message);
            }
            
        }

        private static Tuple<long, long> ReadPlayerHealth()
        {
            if (!IsAttached || !_offsetsLoaded) return null;
            try
            {
                var playerBase = Read<IntPtr>(offsetPlayerBase);
                var playerBase2 = Read<IntPtr>(playerBase + 0x10);
                var currentHealth = Read<long>(playerBase2 + 0xE0);
                var maxHealth = Read<long>(playerBase2 + 0x100);
                return new Tuple<long, long>(currentHealth, maxHealth);
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return null;
            }
        }

        private static bool ReadMouselook()
        {
            if (!IsAttached || !_offsetsLoaded) return false;
            try
            {
                var mouselookState = Read<byte>(offsetMouselookState);

                return (mouselookState & 1) == 1;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return false;
            }
        }

        private static byte lastState = 0;
        private static bool ReadGameState()
        {
            if (!IsAttached || !_offsetsLoaded) return false;

            try
            {
                var gameState = Read<byte>(offsetGameState);
                if(gameState != 0 && gameState != 1) Log.WriteLine("GameState unexpected value: {gameState}");
                if (lastState != gameState)
                {
                    Log.WriteLine($"GameState changed: {gameState}");
                    lastState = gameState;
                }
                return gameState != 0;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return false;
            }
        }

        private static int ReadMovementState()
        {
            if (!IsAttached || !_offsetsLoaded) return 0;
            try
            {
                var secondOffset = ReadPointer(offsetWalkState);
                var finalOffset = Read<IntPtr>(secondOffset)+0x1599;

                var walkState = Read<byte>(finalOffset);

                return walkState;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return 0;
            }
        }

        private static bool ReadAoeState()
        {
            if (!IsAttached || !_offsetsLoaded) return false;
            try
            {
                var baseOffset = offsetAoeState + Read<int>(offsetAoeState) + 4;
                var bigOffset = Read<IntPtr>(baseOffset);
                var smallOffset = Read<int>(offsetAoeState + 6);
                var aoeState = Read<byte>(bigOffset + smallOffset);

                return aoeState == 1;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return false;
            }
        }

        private static T Read<T>(IntPtr offset)
        {
            var typeLength = Marshal.SizeOf<T>();
            var bytes = new byte[typeLength];
            IntPtr bytesRead;

            var readSuccess = ReadProcessMemory(_handle, offset, bytes, bytes.Length, out bytesRead);

            if (!readSuccess) return default(T);

            var typePtr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                Marshal.Copy(bytes, 0, typePtr, bytes.Length);
                var returnType = Marshal.PtrToStructure<T>(typePtr);
                return returnType;
            }
            finally
            {
                Marshal.FreeHGlobal(typePtr);
            }
        }

        private static T[] Read<T>(IntPtr offset, int count)
        {
            var typeLength = Marshal.SizeOf<T>();
            var bytes = new byte[typeLength*count];
            var bytesRead = IntPtr.Zero;

            var readSuccess = ReadProcessMemory(_handle, offset, bytes, bytes.Length, out bytesRead);

            if (!readSuccess) return default(T[]);

            var typePtr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                var returnType = new T[count];
                for (var i = 0; i < count; i++)
                {
                    var outPtr = Marshal.AllocHGlobal(typeLength);
                    try
                    {
                        Marshal.Copy(bytes, typeLength*i, outPtr, typeLength);
                        var outType = (T) Marshal.PtrToStructure(outPtr, typeof(T));
                        returnType[i] = outType;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(outPtr);
                    }
                }
                Marshal.Copy(bytes, 0, typePtr, bytes.Length);
                return returnType;
            }
            catch (Exception ex)
            {
                return default(T[]);
            }
            finally
            {
                Marshal.FreeHGlobal(typePtr);
            }
        }

        private static IntPtr ReadPointer(IntPtr startAddress, IReadOnlyList<int> pointerOffsets, int staticOffset = 0)
        {
            var currentAddress = startAddress;

            foreach (var pointerOffset in pointerOffsets)
            {
                var pointer = Read<int>(currentAddress + pointerOffset);
                currentAddress += 4 + pointer + pointerOffset; // int size = 4
            }

            return currentAddress + staticOffset;
        }

        private static IntPtr ReadPointer(IntPtr startAddress, int staticOffset = 0)
        {
            return ReadPointer(startAddress, new int[1], staticOffset);
        }

        #region Native Methods and Enums

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
            [Out] [MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(
            ProcessAccessFlags processAccess,
            bool bInheritHandle,
            int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead);

        [Flags]
        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        #endregion
    }
}