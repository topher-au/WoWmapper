using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WoWmapper.Classes;
using WoWmapper.Input;
using WoWmapper.MemoryReader;
using WoWmapper.Offsets;
using WoWmapper.Properties;
using WoWmapper.WorldOfWarcraft.AddOns;

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
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, Int32 wParam, Int32 lParam);

        private static int WM_CLOSE = 0x10;
        private static int WM_LBUTTONDOWN = 0x201;
        private static int WM_LBUTTONUP = 0x202;
        private static int WM_RBUTTONDOWN = 0x204;
        private static int WM_RBUTTONUP = 0x205;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; // x position of upper-left corner
            public int Top; // y position of upper-left corner
            public int Right; // x position of lower-right corner
            public int Bottom; // y position of lower-right corner
        }

        #endregion Native Methods and Structs

        public static Process Process; // Current instance of WoW
        private static Thread _watcherThread; // Thread for monitoring WoW instances
        private static bool _watching; // Current state of the watcher

        public static bool GameRunning => Process != null;
        public static bool IsRunning => _watching;

        public static Enums.GameArchitecture GameArchitecture
        {
            get
            {
                if (Process == null) return Enums.GameArchitecture.None;
                try
                {
                    bool proc64;
                    var x = IsWow64Process(Process.Handle, out proc64);
                    return proc64 ? Enums.GameArchitecture.X86 : Enums.GameArchitecture.X64;
                }
                catch
                {
                }
                return Enums.GameArchitecture.None;
            }
        }

        public static void Start()
        {
            if (_watching || _watcherThread != null) return;
            Logger.Write("Starting process watcher...");

            _watching = true;

            _watcherThread = new Thread(WatcherThread) {IsBackground = true};
            _watcherThread.Start();
        }

        public static void Stop()
        {
            if (!_watching || _watcherThread == null) return;
            Logger.Write("Stopping process watcher...");
            MemoryManager.Close();
            _watcherThread?.Abort();
            Process?.Dispose();
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
            OffsetManager.Clear();
            Process?.Dispose();
            Process = null;
        }

        private static void WatcherThread()
        {
            while (_watching)
            {
                // Check current process
                if (Process != null)
                {
                    try
                    {
                        if (Process.HasExited || !IsWindow(Process.MainWindowHandle))
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
                if (Process == null)
                {
                    var wowProcess = GetWowProcesses();
                    Process activeProcess = null;
                    try
                    {
                        activeProcess =
                            wowProcess.FirstOrDefault(
                                proc => proc.HasExited == false && proc.MainWindowHandle != IntPtr.Zero);
                        if (activeProcess != null)
                            Logger.Write("Found WoW process! Handle is {0}", activeProcess.Handle);
                        
                    }
                    catch (Exception ex)
                    {
                    }


                    if (activeProcess != null)
                    {
                        if (Settings.Default.EnableAdvancedFeatures)
                            MemoryManager.Attach(activeProcess);
                        var consolePortLuaFile = Path.Combine(Path.GetDirectoryName(activeProcess.MainModule.FileName), "Interface\\AddOns\\ConsolePort\\Controllers\\WoWmapper.lua");
                        if (File.Exists(consolePortLuaFile))
                            ConsolePortBindWriter.WriteBindFile(consolePortLuaFile);

                        Process = activeProcess;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public static RECT GameWindowRect
        {
            get
            {
                if (Process == null || !IsWindow(Process.MainWindowHandle)) return new RECT();

                RECT windowRect;
                var gotRect = GetWindowRect(Process.MainWindowHandle, out windowRect);
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

                    var foregroundResult = SendMessage(hWndFg, WM_KEYDOWN, (IntPtr) KeyInterop.VirtualKeyFromKey(key),
                        IntPtr.Zero);
                    if (foregroundResult != 0)
                        Logger.Write("SendMessage WM_KEYDOWN returned error code: {0}", foregroundResult);
                    return;
                }

                if (!ProcessWatcher.GameRunning) return;

                var sendResult = SendMessage(Process.MainWindowHandle, WM_KEYDOWN,
                    (IntPtr) KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
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

                    var foregroundResult = SendMessage(hWndFg, WM_KEYUP, (IntPtr) KeyInterop.VirtualKeyFromKey(key),
                        IntPtr.Zero);
                    if (foregroundResult != 0)
                        Logger.Write("SendMessage WM_KEYUP returned error code: {0}", foregroundResult);
                    return;
                }

                if (!ProcessWatcher.GameRunning) return;

                var sendResult = SendMessage(Process.MainWindowHandle, WM_KEYUP,
                    (IntPtr) KeyInterop.VirtualKeyFromKey(key), IntPtr.Zero);
                if (sendResult != 0)
                    Logger.Write("SendMessage WM_KEYUP returned error code: {0}", sendResult);
            }

            public static void SendRightClick()
            {
                SendMessage(Process.MainWindowHandle, WM_RBUTTONDOWN, 0, 0);
                SendMessage(Process.MainWindowHandle, WM_RBUTTONUP, 0, 0);
            }
            public static void SendLeftClick()
            {
                SendMessage(Process.MainWindowHandle, WM_LBUTTONDOWN, 0, 0);
                SendMessage(Process.MainWindowHandle, WM_LBUTTONUP, 0, 0);
            }
        }

        private static Process[] GetWowProcesses()
        {
            // Build list of process names to search
            var searchNames = new List<string>();

            if (Settings.Default.ForceArchitecture != 64) searchNames.Add("wow");
            if (Settings.Default.ForceArchitecture != 64) searchNames.Add("wowt");
            if (Settings.Default.ForceArchitecture != 64) searchNames.Add("wowb");
            if (Settings.Default.ForceArchitecture != 32) searchNames.Add("wow-64");
            if (Settings.Default.ForceArchitecture != 32) searchNames.Add("wowt-64");
            if (Settings.Default.ForceArchitecture != 32) searchNames.Add("wowb-64");

            // Build list of matching processes
            var processes = Process.GetProcesses();
            return processes.Where(proc => searchNames.Contains(proc.ProcessName.ToLower())).ToArray();
        }
    }
}