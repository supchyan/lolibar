using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using VirtualDesktop;
using Windows.Devices.WiFi;
using Windows.Foundation;

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
    
    public static bool IsNetworkCountersInitialized             { get; private set; }

    public static IReadOnlyList<WiFiAdapter>? WiFiAdapters { get { return WiFiAdapter.FindAllAdaptersAsync().GetAwaiter().GetResult();  } }
    
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