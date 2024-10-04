using System.Diagnostics;
using System.Runtime.InteropServices;
using VirtualDesktop;

namespace LolibarApp.Source.Tools;

public class LolibarPerfMon
{
    // Counters
    public static readonly PerformanceCounter CPU_Total         = new("Processor", "% Processor Time", "_Total");
    
    public static readonly PerformanceCounter RAM_Left_MB       = new("Memory", "Available MBytes");

    public static readonly PerformanceCounter Disk_Total        = new("PhysicalDisk", "% Disk Time", "_Total");
    public static readonly PerformanceCounter Disk_Read_Total   = new("PhysicalDisk", "% Disk Read Time", "_Total");
    public static readonly PerformanceCounter Disk_Write_Total  = new("PhysicalDisk", "% Disk Write Time", "_Total");

    public static PerformanceCounter? Network_Bytes_Total       { get; private set; }
    public static PerformanceCounter? Network_Bytes_Sent        { get; private set; }
    public static PerformanceCounter? Network_Bytes_Received    { get; private set; }
    
    static nint oldhwnd { get; set; }

    public static bool IsNetworkCountersInitialized              { get; private set; }

    /// <summary>
    /// Process ID [0]; Process Name [1]; Process Info (name: id) [2];
    /// </summary>
    public static string[] GetForegroundProcessInfo()
    {
        var defaults = new string[]
        {
            LolibarDefaults.CurProcIdInfo,
            LolibarDefaults.CurProcNameInfo
        };

        nint hwnd = LolibarExtern.GetForegroundWindow();

        if (oldhwnd == hwnd) return defaults;

        LolibarExtern.GetWindowThreadProcessId(hwnd, out uint pid);

        // Prevent info update when statusbar gets focus
        if (pid == Process.GetCurrentProcess().Id) return defaults;

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

        oldhwnd = hwnd;

        return new string[2];
    }
    public static void InitializeNetworkCounters()
    {
        var category = new PerformanceCounterCategory("Network Interface");
        foreach (var instance in category.GetInstanceNames())
        {
            if (instance.Contains("802.11ac"))
            {
                Network_Bytes_Total     = new($"Network Interface", "Bytes Total/sec", instance);
                Network_Bytes_Sent      = new($"Network Interface", "Bytes Sent/sec", instance);
                Network_Bytes_Received  = new($"Network Interface", "Bytes Received/sec", instance);
                return;
            }
        }
        IsNetworkCountersInitialized = true;
    }
}