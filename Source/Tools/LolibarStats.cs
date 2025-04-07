using Microsoft.VisualBasic.Devices;
using System.Security.Principal;
using System.Windows.Media;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Windows.Devices.WiFi;

namespace LolibarApp.Source.Tools;

public class LolibarStats
{
    // Counters
    static readonly PerformanceCounter CPU_Total        = new("Processor", "% Processor Time", "_Total");

    static readonly PerformanceCounter Disk_Total       = new("PhysicalDisk", "% Disk Time", "_Total");
    static readonly PerformanceCounter Disk_Read_Total  = new("PhysicalDisk", "% Disk Read Time", "_Total");
    static readonly PerformanceCounter Disk_Write_Total = new("PhysicalDisk", "% Disk Write Time", "_Total");

    static PerformanceCounter? Network_Bytes_Total      { get; set; }
    static PerformanceCounter? Network_Bytes_Sent       { get; set; }
    static PerformanceCounter? Network_Bytes_Received   { get; set; }
    static bool NetworkInterfaceInitialized             { get; set; }
    static PerformanceCounterCategory? NetworkInterface
    {
        get
        {
            return new PerformanceCounterCategory("Network Interface");
        }
    }
    static void InitializeNetworkCounters()
    {
        if (NetworkInterface == null || NetworkInterfaceInitialized) 
        { 
            return;
        }

        foreach (var instance in NetworkInterface.GetInstanceNames())
        {
            if (instance.Contains("802.11ac"))
            {
                Network_Bytes_Total         = new($"Network Interface", "Bytes Total/sec", instance);
                Network_Bytes_Sent          = new($"Network Interface", "Bytes Sent/sec", instance);
                Network_Bytes_Received      = new($"Network Interface", "Bytes Received/sec", instance);
                NetworkInterfaceInitialized = true;
                return;
            }
        }
    }
    static IReadOnlyList<WiFiAdapter>? WiFiAdapters
    {
        get
        {
            return WiFiAdapter.FindAllAdaptersAsync().GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// Returns focusted application ID.
    /// </summary>
    public static int? CurrentApplicationId
    { 
        get
        {
            return LolibarProcess.ForegroundProcess.Id;
        }
    }
    /// <summary>
    /// Returns focusted application process name.
    /// </summary>
    public static string? CurrentApplicationName
    { 
        get
        {
            return LolibarProcess.ForegroundProcess.Name;
        }
    }
    /// <summary>
    /// Returns current input language.
    /// </summary>
    public static string? CurrentInputLanguage
    {
        get
        {
            // https://stackoverflow.com/questions/26617159/hook-detect-windows-language-change-even-when-app-not-focused
            try
            {
                var layout = LolibarExtern.GetKeyboardLayout(LolibarExtern.GetWindowThreadProcessId(LolibarExtern.GetForegroundWindow(), out uint _));
                return new CultureInfo((short)layout.ToInt64()).NativeName;
            }
            catch { }

            return "Unknown";
        }
    }

    /// <summary>
    /// Current lolibar's execution path.
    /// </summary>
    public static string ExecutionPath
    {
        get
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".\\";
        }
    }
    #region WiFi
    //static string? GetWiFiName()
    //{
    //    if (WiFiAdapters == null) return null;

    //    foreach (var adapter in WiFiAdapters)
    //    {
    //        // TODO:
    //        // AvailableNetworks[0] here is not a connected network, so this is wrong info.
    //        return adapter.NetworkReport.AvailableNetworks[0].Ssid;
    //    }

    //    return null;
    //}
    //static Geometry? GetWiFiIcon()
    //{
    //    if (WiFiAdapters == null) return null;

    //    foreach (var adapter in WiFiAdapters)
    //    {
    //        // TODO:
    //        // AvailableNetworks[0] here is not a connected network, so this is wrong info.
    //        var signalBars = adapter.NetworkReport.AvailableNetworks[0].SignalBars;

    //        switch (signalBars)
    //        {
    //            case 4:
    //                return LolibarIcon.ParseSVG("./Defaults/wifi_4.svg");

    //            case 3:
    //                return LolibarIcon.ParseSVG("./Defaults/wifi_3.svg");

    //            case 2:
    //                return LolibarIcon.ParseSVG("./Defaults/wifi_2.svg");

    //            case 1:
    //                return LolibarIcon.ParseSVG("./Defaults/wifi_1.svg");
    //        }
    //    }

    //    return null;
    //}
    #endregion
    #region User
    public static string? UserInfo
    {
        get
        {
            return $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
        }
    }
    #endregion

    #region Cpu
    public static string? CpuTotalInPercent
    {
        get
        {
            return $"{String.Format("{0:0.0}", Math.Round(CPU_Total.NextValue(), 1))}%";
        }
    }
    #endregion

    #region Ram
    public static string? RamUsedInPercent
    {
        get
        {
            var computerInfo = new ComputerInfo();
            return $"{String.Format("{0:0.0}", Math.Round(100.0 * (1.0 - ((double)computerInfo.AvailablePhysicalMemory / (double)computerInfo.TotalPhysicalMemory)), 1))}%";
        }
    }
    public static string? RamUsedInGigabytes
    {
        get
        {
            var computerInfo = new ComputerInfo();
            return $"{String.Format("{0:0.0}", Math.Round((double)computerInfo.TotalPhysicalMemory - (double)computerInfo.AvailablePhysicalMemory) / 1024.0 / 1024.0 / 1024.0)}Gb";
        }
    }
    #endregion

    #region Disk
    public static string? DiskReadInPercent
    {
        get
        {
            return $"{String.Format("{0:0.0}", Math.Round(Disk_Read_Total.NextValue(), 1))}%";
        }
    }
    public static string? DiskWriteInPercent
    {
        get
        {
            return $"{String.Format("{0:0.0}", Math.Round(Disk_Write_Total.NextValue(), 1))}%";
        }
    }
    public static string? DiskTotalInPercent
    {
        get
        {
            return $"{String.Format("{0:0.0}", Math.Round(Disk_Total.NextValue(), 1))}%";
        }
    }
    #endregion

    #region Network
    /// <summary>
    /// Network info (Total / Sent / Received).
    /// </summary>
    /// <returns>Network info in percent. Can be swaped via `SwapNetworkInfo`.</returns>
    public static string? NetworkBytesSent
    {
        get
        {
            InitializeNetworkCounters();
            if (Network_Bytes_Sent == null) return "No data";
            return $"{Math.Round(Network_Bytes_Sent.NextValue() / 1024)}Kbps";
        }
    }
    public static string? NetworkBytesReceived
    {
        get
        {
            InitializeNetworkCounters();
            if (Network_Bytes_Received == null) return "No data";
            return $"{Math.Round(Network_Bytes_Received.NextValue() / 1024)}Kbps";
        }
    }
    public static string? NetworkBytesTotal
    {
        get
        {
            InitializeNetworkCounters();
            if (Network_Bytes_Total == null) return "No data";
            return $"{Math.Round(Network_Bytes_Total.NextValue() / 1024)}Kbps";
        }
    }
    #endregion

    #region Power
    static double PrivatePowerInPercent
    {
        get
        {
            return Math.Round(100.0 * SystemInformation.PowerStatus.BatteryLifePercent);
        }
    }
    public static string PowerInPercent
    {
        get
        {
            return $"{PrivatePowerInPercent}%";
        }
    }
    public static Geometry? SmartPowerIcon
    {
        get
        {
            var powerStatus = SystemInformation.PowerStatus;

            // Power Icon handling
            if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging))
            {
                return LolibarIcon.ParseSVG("./Defaults/power_charge.svg");
            }
            if (PrivatePowerInPercent >= 80)
            {
                return LolibarIcon.ParseSVG("./Defaults/power_high.svg");
            }
            if (PrivatePowerInPercent >= 30)
            {
                return LolibarIcon.ParseSVG("./Defaults/power_low.svg");
            }
            if (PrivatePowerInPercent < 30)
            {
                return LolibarIcon.ParseSVG("./Defaults/power_crit.svg");
            }

            return LolibarIcon.ParseSVG("./Defaults/power_error.svg");
        }
    }
    /// <summary>
    /// Returns true, whenever PC's battery is charging.
    /// </summary>
    public static bool IsBatteryCharging
    {
        get
        {
            return SystemInformation.PowerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging);
        }
    }
    #endregion
}
