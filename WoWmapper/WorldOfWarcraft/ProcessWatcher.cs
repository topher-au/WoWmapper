using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WoWmapper.Classes;
using WoWmapper.MemoryReader;
using WoWmapper.Offsets;
using WoWmapper.Properties;

namespace WoWmapper.WorldOfWarcraft
{
    public static class ProcessWatcher
    {
#region Native Methods and Structs

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
#endregion Native Methods and Structs
        private static Process _gameProcess; // Current instance of WoW
        private static Thread _watcherThread; // Thread for monitoring WoW instances
        private static bool _watching; // Current state of the watcher

        // public static bool CanReadMemory => _gameHandle != IntPtr.Zero && GameArchitecture == Enums.GameArchitecture.X64 && Settings.Default.EnableAdvancedFeatures && OffsetManager.OffsetsAvailable;
        public static bool GameRunning => _gameProcess != null;
        public static bool IsRunning => _watching;

        public static Enums.GameArchitecture GameArchitecture
        {
            get
            {
                if (_gameProcess == null) return Enums.GameArchitecture.None;
                try
                {
                    bool proc64;
                    var x = IsWow64Process(_gameProcess.Handle, out proc64);
                    return proc64 ? Enums.GameArchitecture.X86 : Enums.GameArchitecture.X64;
                } catch { }
                return Enums.GameArchitecture.None;
            }
        }

        public static void Start()
        {
            if (_watching || _watcherThread != null) return;
            Logger.Write("Starting process watcher...");

            _watching = true;

            _watcherThread = new Thread(WatcherThread) { IsBackground = true };
            _watcherThread.Start();
        }

        public static void Stop()
        {
            if (!_watching || _watcherThread == null) return;
            Logger.Write("Stopping process watcher...");
            MemoryManager.Close();
            _watcherThread?.Abort();
            _gameProcess?.Dispose();
            _watcherThread = null;
            _watching = false;
        }

        public static void Restart()
        {
            Stop();
            Start();
        }

        private static void ResetProcess()
        {
            MemoryManager.Close();
            _gameProcess?.Dispose();
            _gameProcess = null;
        }

        private static void WatcherThread()
        {
            while (_watching)
            {
                // Check current process
                if (_gameProcess != null)
                {
                    try
                    {
                        if (_gameProcess.HasExited || !IsWindow(_gameProcess.MainWindowHandle))
                        {
                            Logger.Write("WoW process invalidated, clearing!");
                            ResetProcess();
                        }
                    }
                    catch
                    {
                        ResetProcess();
                    }
                }

                // Find game process
                if (_gameProcess == null)
                {
                    var wowProcess = GetWowProcesses();
                    Process activeProcess = null;
                    try
                    {
                        activeProcess =
                        wowProcess.FirstOrDefault(proc => proc.HasExited == false && proc.MainWindowHandle != IntPtr.Zero);
                        if(activeProcess != null) Logger.Write("Found WoW process! Handle is {0}", activeProcess.Handle);
                    } catch { }
                    

                    if (activeProcess != null)
                    {
                        if (Settings.Default.EnableAdvancedFeatures)
                            MemoryManager.Attach(activeProcess);
                        
                        
                        _gameProcess = activeProcess;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public static RECT GameWindowRect
        {
            get
            {
                if (_gameProcess == null || !IsWindow(_gameProcess.MainWindowHandle)) return new RECT();

                RECT windowRect;
                var gotRect = GetWindowRect(_gameProcess.MainWindowHandle, out windowRect);
                return gotRect ? windowRect : new RECT();
            }
        }

        public static class Interaction
        {
            public static void SendKeyDown(Key key)
            {
                if (Settings.Default.SendForegroundKeys)
                {
                    // Get foreground window and send message
                    var hWndFg = GetForegroundWindow();
                    if (hWndFg == IntPtr.Zero)
                    {
                        Logger.Write("Failed to get foreground window handle");
                        return;
                    }

                    var foregroundResult = SendMessage(hWndFg, WM_KEYDOWN, (IntPtr)KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
                    if (foregroundResult != 0)
                        Logger.Write("SendMessage WM_KEYDOWN returned error code: {0}", foregroundResult);
                    return;
                }

                if (!ProcessWatcher.GameRunning) return;

                var sendResult = SendMessage(_gameProcess.MainWindowHandle, WM_KEYDOWN, (IntPtr)KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
                if (sendResult != 0)
                    Logger.Write("SendMessage WM_KEYDOWN returned error code: {0}", sendResult);
            }

            public static void SendKeyUp(Key key)
            {
                if (Settings.Default.SendForegroundKeys)
                {
                    // Get foreground window and send message
                    var hWndFg = GetForegroundWindow();
                    if (hWndFg == IntPtr.Zero)
                    {
                        Logger.Write("Failed to get foreground window handle");
                        return;
                    }

                    var foregroundResult = SendMessage(hWndFg, WM_KEYUP, (IntPtr)KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
                    if (foregroundResult != 0)
                        Logger.Write("SendMessage WM_KEYUP returned error code: {0}", foregroundResult);
                    return;
                }

                if (!ProcessWatcher.GameRunning) return;

                var sendResult = SendMessage(_gameProcess.MainWindowHandle, WM_KEYUP, (IntPtr)KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
                if (sendResult != 0)
                    Logger.Write("SendMessage WM_KEYUP returned error code: {0}", sendResult);
            }
        }

        private static Process[] GetWowProcesses()
        {
            // Build list of process names to search
            var searchNames = new List<string>();
            if(Settings.Default.ForceArchitecture != 64) searchNames.Add("wow");
            if(Settings.Default.ForceArchitecture != 32) searchNames.Add("wow-64");
            if (Settings.Default.ForceArchitecture != 32) searchNames.Add("wowt-64");

            // Build list of matching processes
            var processes = Process.GetProcesses();
            return processes.Where(proc => searchNames.Contains(proc.ProcessName.ToLower())).ToArray();
        }
    }
}
