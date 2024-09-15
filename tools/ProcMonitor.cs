﻿using System.Diagnostics;

namespace lolibar.tools
{
    public class ProcMonitor
    {
        // Counters
        public static readonly PerformanceCounter CPU_Time_Total = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public static readonly PerformanceCounter CPU_Time_Process = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
        public static readonly PerformanceCounter RAM_Left_MB = new PerformanceCounter("Memory", "Available MBytes");
        public static readonly PerformanceCounter RAM_Commited_B = new PerformanceCounter("Memory", "Committed Bytes");

        // https://stackoverflow.com/questions/97283/how-can-i-determine-the-name-of-the-currently-focused-process-in-c-sharp
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern nint GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(nint hwnd, out uint lpdwProcessId);
        /// <summary>
        /// Process ID [0]; Process Name [1]; Process Info (name: id) [2];
        /// </summary>
        /// <returns></returns>
        public string[] GetForegroundProcessInfo()
        {
            nint hwnd = GetForegroundWindow();

            GetWindowThreadProcessId(hwnd, out uint pid);

            foreach (var p in Process.GetProcesses())
            {
                if (p.Id == pid)
                {
                    return
                    [
                        $"{pid}",
                        $"{p.ProcessName}",
                    ];
                }
            }

            return new string[2];
        }
    }
}
