using System.Diagnostics;

namespace lolibar.tools
{
    public class Info
    {
        // Info
        public string BarUser        = "wait";
        public string BarTime        = "wait";
        public string BarCPU         = "wait";
        public string BarRAM         = "wait";
        public string BarPower       = "wait";
        public string BarCurProcName = "wait";
        public string BarCurProcID   = "wait";
        public string BarCurProc     = "wait";

        // Counters
        public static readonly PerformanceCounter CPU_Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        public static readonly PerformanceCounter RAM_Counter = new PerformanceCounter("Memory", "Available MBytes");

        // https://stackoverflow.com/questions/97283/how-can-i-determine-the-name-of-the-currently-focused-process-in-c-sharp
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern nint GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(nint hwnd, out uint lpdwProcessId);

        public void GetForegroundProcessInfo()
        {
            nint hwnd = GetForegroundWindow();

            if (hwnd == null) return;

            GetWindowThreadProcessId(hwnd, out uint pid);

            foreach (var p in Process.GetProcesses())
            {
                if (p.Id == pid)
                {
                    BarCurProcID    = pid.ToString();
                    BarCurProcName  = p.ProcessName;
                    BarCurProc = $"{BarCurProcName}: {BarCurProcID}";
                }
            }
        }
    }
}
