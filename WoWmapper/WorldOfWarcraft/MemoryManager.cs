using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WoWmapper.WorldOfWarcraft
{
    public static class MemoryManager
    {
        #region Native Methods and Enums

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr processHandle,
            [Out, MarshalAs(UnmanagedType.Bool)] out bool wow64Process);

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
        static extern bool CloseHandle(IntPtr hObject);

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

        #region Public Properties

        public static bool IsAttached => ProcessHandle != IntPtr.Zero && Process != null;

        #endregion

        public static IntPtr ProcessHandle;
        public static Process Process;
        private static IntPtr ignoreHandle = IntPtr.Zero;

        public static bool Attach(Process process)
        {
            // Check if a process is already attached and detach
            if (ProcessHandle != IntPtr.Zero) Detach();

            // Check if we are ignoring a handle
            if (process.Handle == ignoreHandle) return false;

            // Wow64 process is a 32-bit process running on a 64-bit system
            bool isWow64 = false;
            IsWow64Process(process.Handle, out isWow64);

            if (isWow64) // Process is Wow64
            {
                ignoreHandle = process.Handle;
                Log.WriteLine($"Failed to attach: Process {process.ProcessName} is 32-bit");
                return false;
            }

            // Check process validity
            if (process.HasExited ||
                process.Id == 0)
            {
                Log.WriteLine($"Failed to attach: Process {process.ProcessName} was invalid");
                return false;
            }

            // Attempt to open process and acquire handle
            Log.WriteLine($"Attaching memory reader to {process.ProcessName}");
            var handleProcess = OpenProcess(ProcessAccessFlags.VirtualMemoryRead, false, process.Id);

            if (handleProcess != IntPtr.Zero)
            {
                // Successfully acquired handle
                ProcessHandle = handleProcess;
                Process = process;

                Log.WriteLine($"Memory reader attached: OpenProcess handle [{ProcessHandle}]");
                return true;
            }
            else
            {
                // Unable to acquire handle
                ProcessHandle = IntPtr.Zero;
                Process = null;
                Log.WriteLine($"Failed to attach: OpenProcess failed to acquire handle");
                return false;
            }
        }

        public static void Detach()
        {
            Log.WriteLine("Closing OpenProcess handle {0}", ProcessHandle.ToString());
            CloseHandle(ProcessHandle);
            ProcessHandle = IntPtr.Zero;
        }

        public static Tuple<long, long> ReadPlayerHealth()
        {
            if (!IsAttached) return null;

            try
            {
                var offset = Offsets.GetStaticOffset(Offset.PlayerBase, Process, true);
                if (offset == IntPtr.Zero) return null; // Failed to locate offset
                var playerBase = Read<IntPtr>(offset);
                var playerBase2 = Read<IntPtr>(playerBase + 0x10);
                var currentHealth = Read<long>(playerBase2 + (int) PlayerInfoOffset.HealthCurrent);
                var maxHealth = Read<long>(playerBase2 + (int)PlayerInfoOffset.HealthMax);
                return new Tuple<long, long>(currentHealth, maxHealth);
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool ReadMouselook()
        {
            if (!IsAttached) return false;

            try
            {
                var offset = Offsets.GetStaticOffset(Offset.MouselookState, Process, true);
                if (offset == IntPtr.Zero) return false; // Failed to locate offset
                var mouselookState = Read<byte>(offset);

                return (mouselookState & 1) == 1;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return false;
            }
        }

        public static byte ReadGameState()
        {
            if (!IsAttached) return 0;

            try
            {
                var offset = Offsets.GetStaticOffset(Offset.GameState, Process, true);
                if (offset == IntPtr.Zero) return 0; // Failed to locate offset

                var gameState = Read<byte>(offset);

                return gameState;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return 0;
            }
        }

        public static int ReadMovementState()
        {
            if (!IsAttached) return 0;

            try
            {
                var offset = Offsets.GetStaticOffset(Offset.WalkRunState, Process, true);

                if (offset == IntPtr.Zero) return 0; // Failed to locate offset

                var secondOffset = MemoryManager.ReadPointer(offset);
                var finalOffset = MemoryManager.Read<IntPtr>(secondOffset) + 0x15e1;

                var walkState = Read<byte>(finalOffset);

                return walkState;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return 0;
            }
        }

        public static bool ReadAoeState()
        {
            if (!IsAttached) return false;

            try
            {
                var offset = Offsets.GetStaticOffset(Offset.PlayerAoeState, Process, true);
                if (offset == IntPtr.Zero) return false; // Failed to locate offset

                var baseOffset = offset + Read<int>(offset) + 4;
                var bigOffset = Read<IntPtr>(baseOffset);
                var smallOffset = MemoryManager.Read<int>(offset + 6);
                var aoeState = Read<byte>(bigOffset + smallOffset);

                return aoeState  == 1;
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
                return false;
            }
        }

        public enum PlayerInfoOffset
        {
            HealthCurrent = 0xF0,
            HealthMax = 0x110,
        }

        public static T Read<T>(IntPtr offset, bool isRelative = false)
        {
            var typeLength = Marshal.SizeOf<T>();
            var bytes = new byte[typeLength];
            IntPtr bytesRead;

            var readSuccess = ReadProcessMemory(ProcessHandle,
                isRelative ? Process.MainModule.BaseAddress + (int) offset : offset, bytes, bytes.Length, out bytesRead);

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

        public static T[] Read<T>(IntPtr offset, int count, bool isRelative = false)
        {
            var typeLength = Marshal.SizeOf<T>();
            var bytes = new byte[typeLength*count];
            var bytesRead = IntPtr.Zero;

            var readSuccess = ReadProcessMemory(ProcessHandle,
                isRelative ? Process.MainModule.BaseAddress + (int) offset : offset, bytes, bytes.Length, out bytesRead);

            if (!readSuccess) return default(T[]);

            var typePtr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                var returnType = new T[count];
                for (int i = 0; i < count; i++)
                {
                    var outPtr = Marshal.AllocHGlobal(typeLength);
                    try
                    {
                        Marshal.Copy(bytes, typeLength*i, outPtr, typeLength);
                        var outType = (T) Marshal.PtrToStructure(outPtr, typeof (T));
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

        public static IntPtr ReadPointer(IntPtr startAddress, IReadOnlyList<int> pointerOffsets, int staticOffset = 0)
        {
            var currentAddress = startAddress;

            foreach (var pointerOffset in pointerOffsets)
            {
                var pointer = MemoryManager.Read<int>(currentAddress + pointerOffset);
                currentAddress += 4 + pointer + pointerOffset; // int size = 4
            }

            return currentAddress + staticOffset;
        }

        public static IntPtr ReadPointer(IntPtr startAddress, int staticOffset = 0)
        {
            return ReadPointer(startAddress, new int[1], staticOffset);
        }
    }
}