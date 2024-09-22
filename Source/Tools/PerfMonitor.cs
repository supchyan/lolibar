using System.Diagnostics;

namespace LolibarApp.Source.Tools
{
    public class PerfMonitor
    {
        // Counters
        public static readonly PerformanceCounter CPU_Total         = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        
        public static readonly PerformanceCounter RAM_Left_MB       = new PerformanceCounter("Memory", "Available MBytes");

        public static readonly PerformanceCounter Disk_Total        = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        public static readonly PerformanceCounter Disk_Read_Total   = new PerformanceCounter("PhysicalDisk", "% Disk Read Time", "_Total");
        public static readonly PerformanceCounter Disk_Write_Total  = new PerformanceCounter("PhysicalDisk", "% Disk Write Time", "_Total");

        public static PerformanceCounter? Network_Bytes_Total       { get; private set; }
        public static PerformanceCounter? Network_Bytes_Sent        { get; private set; }
        public static PerformanceCounter? Network_Bytes_Received    { get; private set; }

        // https://stackoverflow.com/questions/97283/how-can-i-determine-the-name-of-the-currently-focused-process-in-c-sharp
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern nint GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(nint hwnd, out uint lpdwProcessId);
        /// <summary>
        /// Process ID [0]; Process Name [1]; Process Info (name: id) [2];
        /// </summary>
        /// <returns></returns>
        public static string[] GetForegroundProcessInfo()
        {
            nint hwnd = GetForegroundWindow();

            GetWindowThreadProcessId(hwnd, out uint pid);

            if (pid == Process.GetCurrentProcess().Id)
            {
                return
                [
                    LolibarDefaults.CurProcIdInfo,
                    LolibarDefaults.CurProcNameInfo
                ];
            }
                

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
        public static void InitializeNetworkCounters()
        {
            var category = new PerformanceCounterCategory("Network Interface");
            foreach (var instance in category.GetInstanceNames())
            {
                if (instance.Contains("802.11ac"))
                {
                    Network_Bytes_Total     = new PerformanceCounter($"Network Interface", "Bytes Total/sec", instance);
                    Network_Bytes_Sent      = new PerformanceCounter($"Network Interface", "Bytes Sent/sec", instance);
                    Network_Bytes_Received  = new PerformanceCounter($"Network Interface", "Bytes Received/sec", instance);
                    return;
                }
            }
        }
    }
}