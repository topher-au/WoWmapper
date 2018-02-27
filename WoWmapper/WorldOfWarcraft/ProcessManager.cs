using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreadState = System.Threading.ThreadState;

namespace WoWmapper.WorldOfWarcraft
{
    internal static class ProcessManager
    {
        #region Natives

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; // x position of upper-left corner
            public int Top; // y position of upper-left corner
            public int Right; // x position of lower-right corner
            public int Bottom; // y position of lower-right corner
        }

        #endregion

        private static readonly string[] WoWProcessNames = new[] {"wow", "wow-64", "wowt", "wowt-64", "wowb", "wowb-64"};
        private static bool _threadRunning = false;
        private static readonly Thread ProcessThread = new Thread(ProcessThreadMethod);

        public static Process GameProcess { get; private set; }
        public static bool GameRunning => GameProcess != null;

        public static Rectangle GetClientRectangle()
        {
            RECT clientRect, windowRect;
            GetClientRect(GameProcess.MainWindowHandle, out clientRect);
            GetWindowRect(GameProcess.MainWindowHandle, out windowRect);

            var borderWidth = (windowRect.Right - windowRect.Left - clientRect.Right)/2;
            var titleHeight = (windowRect.Bottom - windowRect.Top) - clientRect.Bottom - borderWidth;

            var outRectangle = new Rectangle(windowRect.Left + borderWidth, windowRect.Top + titleHeight,
                clientRect.Right, clientRect.Bottom);
            return outRectangle;
        }

        private static void ProcessThreadMethod()
        {
            // Process watcher thread
            while (ProcessThread.ThreadState == ThreadState.Running)
            {
                if (GameProcess == null)
                {
                    try
                    {
                        // Acquire a list of all processes
                        var wowProcess =
                            Process.GetProcesses()
                                .FirstOrDefault(
                                    process =>
                                        WoWProcessNames.Contains(process.ProcessName.ToLower()) &&
                                        process.HasExited == false);

                        if (wowProcess != null)
                        {
                            GameProcess = wowProcess;

                            Log.WriteLine($"Found game process: [{GameProcess.Id}: {GameProcess.ProcessName}]");

                            // Attempt to export bindings
                            ConsolePort.BindWriter.WriteBinds();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine($"Exception occurred: {ex.Message}");
                    }
                }

                if (GameProcess != null)
                {
                    // Test process validity
                    if (GameProcess.HasExited)
                    {
                        Log.WriteLine($"Process [{GameProcess.Id}: {GameProcess.ProcessName}] has exited");

                        GameProcess.Dispose();
                        GameProcess = null;
                        continue;
                    }
                }

                Thread.Sleep(500);
            }
        }

        internal static void Start()
        {
            _threadRunning = true;
            ProcessThread.Start();
        }

        internal static void Stop()
        {
            _threadRunning = false;
            ProcessThread.Abort();
        }
    }

    internal enum ProcessType
    {
        None,
        WoW32,
        WoW64,
        WoWT,
        WoWT64
    }
}