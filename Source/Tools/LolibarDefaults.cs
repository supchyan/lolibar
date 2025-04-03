using Microsoft.VisualBasic.Devices;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Media;
using System.IO;
using System.Globalization;
using System.Windows.Input;

namespace LolibarApp.Source.Tools;

public class LolibarDefaults
{
    static bool ShowRamInPercent    = true;
    static int  DiskInfoState       = 0;
    static int  NetworkInfoState    = 0;
    /// <summary>
    /// Returns focusted application ID.
    /// </summary>
    public static string  CurrentApplicationId   { get; private set; }   = string.Empty;
    /// <summary>
    /// Returns focusted application process name.
    /// </summary>
    public static string  CurrentApplicationName { get; private set; }   = string.Empty;
    /// <summary>
    /// Returns current input language.
    /// </summary>
    public static string? CurrentInputLanguage
    {
        get
        {
            // https://stackoverflow.com/questions/26617159/hook-detect-windows-language-change-even-when-app-not-focused
            var layout = LolibarExtern.GetKeyboardLayout(LolibarExtern.GetWindowThreadProcessId(LolibarExtern.GetForegroundWindow(), out uint _));
            try
            {
                return new CultureInfo((short)layout.ToInt64()).NativeName;
            }
            catch
            {

            }
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
    //    if (LolibarPerfMon.WiFiAdapters == null) return null;

    //    foreach (var adapter in LolibarPerfMon.WiFiAdapters)
    //    {
    //        // TODO:
    //        // AvailableNetworks[0] here is not a connected network, so this is wrong info.
    //        return adapter.NetworkReport.AvailableNetworks[0].Ssid;
    //    }

    //    return null;
    //}
    //static Geometry? GetWiFiIcon()
    //{
    //    if (LolibarPerfMon.WiFiAdapters == null) return null;

    //    foreach (var adapter in LolibarPerfMon.WiFiAdapters)
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
    /// <summary>
    /// Current Windows account username.
    /// </summary>
    /// <returns>Your account username.</returns>
    public static string? GetUserInfo()
    {
        return $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
    }
    #endregion

    #region CurProc
    /// <summary>
    /// Show current selected / running application info in a view like: `proc_name [proc_id]`
    /// </summary>
    /// <returns></returns>
    public static string? GetCurrentApplicationInfo()
    {
        CurrentApplicationId = LolibarProcess.ForegroundProcess.Id.ToString();
        CurrentApplicationName = LolibarProcess.ForegroundProcess.Name;

        var nameAndId = string.Empty;

        nameAndId += $"{CurrentApplicationName} [{CurrentApplicationId}]";

        if (nameAndId == string.Empty) return "No running application is active";
        return nameAndId;

    }
    #endregion

    #region Cpu
    /// <summary>
    /// CPU Info.
    /// </summary>
    /// <returns>Current total CPU load in percent.</returns>
    public static string? GetCpuInfo()
    {
        return $"{String.Format("{0:0.0}", Math.Round(LolibarPerfMon.CPU_Total.NextValue(), 1))}%";
    }
    #endregion

    #region Ram
    /// <summary>
    /// Swaps RAM load info. Get info with `GetRamInfo()`.
    /// </summary>
    public static void SwapRamInfo()
    {
        ShowRamInPercent = !ShowRamInPercent;
    }
    /// <summary>
    /// RAM Info.
    /// </summary>
    /// <returns>Current total RAM load in percent / Gbytes. Swap it with `SwapRamInfo()`.</returns>
    public static string? GetRamInfo()
    {
        var computerInfo            = new ComputerInfo();
        var RamUsedInPercentInfo    = $"{String.Format("{0:0.0}", Math.Round(100.0 * (1.0 - ((double)computerInfo.AvailablePhysicalMemory / (double)computerInfo.TotalPhysicalMemory)), 1))}%";
        var RamUsedInGbInfo         = $"{String.Format("{0:0.0}", Math.Round((double)computerInfo.TotalPhysicalMemory - (double)computerInfo.AvailablePhysicalMemory) / 1024.0 / 1024.0 / 1024.0)}Gb";

        if (ShowRamInPercent)   return RamUsedInPercentInfo ?? string.Empty;
        else                    return RamUsedInGbInfo      ?? string.Empty;
    }
    public static Geometry? GetRamIcon()
    {
        return LolibarIcon.ParseSVG("Defaults\\ram.svg");
    }
    #endregion

    #region Disk
    /// <summary>
    /// Swaps disk info. Get info with `GetDiskInfo()`.
    /// </summary>
    public static void SwapDiskInfo()
    {
        if (DiskInfoState < 2) DiskInfoState++;
        else DiskInfoState = 0;
    }
    /// <summary>
    /// Disk load info. (Total / Read / Write)
    /// </summary>
    /// <returns>Disk load info in percent. Can be swaped via `SwapDiskInfo`.</returns>
    public static string? GetDiskInfo()
    {
        switch (DiskInfoState)
        {
            case 0: // read + write average usage
                return $"{String.Format("{0:0.0}", Math.Round(LolibarPerfMon.Disk_Total.NextValue(), 1))}%";

            case 1: // only read average usage
                return $"{String.Format("{0:0.0}", Math.Round(LolibarPerfMon.Disk_Read_Total.NextValue(), 1))}%";

            case 2: // only write average usage
                return $"{String.Format("{0:0.0}", Math.Round(LolibarPerfMon.Disk_Write_Total.NextValue(), 1))}%";
            
            default:
                break;
        }
        return null;
    }
    public static Geometry? GetDiskIcon()
    {
        switch (DiskInfoState)
        {
            case 0:     return LolibarIcon.ParseSVG("Defaults\\disk.svg"      );    // read + write average usage
            case 1:     return LolibarIcon.ParseSVG("Defaults\\disk_read.svg" );   // only read average usage
            case 2:     return LolibarIcon.ParseSVG("Defaults\\disk_write.svg");  // only write average usage
            default:    break;
        }
        return null;
    }
    #endregion

    #region Network
    /// <summary>
    /// Swaps Network info. Get info with `GetNetworkInfo()`.
    /// </summary>
    public static void SwapNetworkInfo()
    {
        if (NetworkInfoState < 2) NetworkInfoState++;
        else NetworkInfoState = 0;
    }
    /// <summary>
    /// Network info (Total / Sent / Received).
    /// </summary>
    /// <returns>Network info in percent. Can be swaped via `SwapNetworkInfo`.</returns>
    public static string? GetNetworkInfo()
    {
        if (!LolibarPerfMon.IsNetworkCountersInitialized)
        {
            LolibarPerfMon.InitializeNetworkCounters();
        }

        switch (NetworkInfoState)
        {
            case 0: // total kbps usage
                if (LolibarPerfMon.Network_Bytes_Total == null) return "No data";
                return $"{Math.Round(LolibarPerfMon.Network_Bytes_Total.NextValue() / 1024)}Kbps";

            case 1: // sent kbps usage
                if (LolibarPerfMon.Network_Bytes_Sent == null) return "No data";
                return $"{Math.Round(LolibarPerfMon.Network_Bytes_Sent.NextValue() / 1024)}Kbps";

            case 2: // received kbps usage
                if (LolibarPerfMon.Network_Bytes_Received == null) return "No data";
                return $"{Math.Round(LolibarPerfMon.Network_Bytes_Received.NextValue() / 1024)}Kbps";

            default:
                break;
        }
        return null;
    }
    public static Geometry? GetNetworkIcon()
    {
        switch (NetworkInfoState)
        {
            case 0:     return LolibarIcon.ParseSVG("Defaults\\network.svg"        );    // total kbps usage
            case 1:     return LolibarIcon.ParseSVG("Defaults\\network_send.svg"   );   // sent kbps usage
            case 2:     return LolibarIcon.ParseSVG("Defaults\\network_receive.svg");  // received kbps usage
            default:    break;
        }
        return null;
    }
    #endregion

    #region Power
    static double GetPowerPercent()
    {
        return Math.Round(100.0 * SystemInformation.PowerStatus.BatteryLifePercent);
    }
    /// <summary>
    /// Power (Battery) info.
    /// </summary>
    /// <returns>Current battery status in percent.</returns>
    public static string? GetPowerInfo()
    {
        return $"{GetPowerPercent()}%";
    }
    public static Geometry? GetPowerIcon()
    {
        var powerStatus = SystemInformation.PowerStatus;

        // Power Icon handling
        if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging))
        {
            return LolibarIcon.ParseSVG("./Defaults/power_charge.svg");
        }
        if (GetPowerPercent() >= 80)
        {
            return LolibarIcon.ParseSVG("./Defaults/power_high.svg");
        }
        if (GetPowerPercent() >= 30)
        {
            return LolibarIcon.ParseSVG("./Defaults/power_low.svg");
        }
        if (GetPowerPercent() < 30)
        {
            return LolibarIcon.ParseSVG("./Defaults/power_crit.svg");
        }

        return LolibarIcon.ParseSVG("./Defaults/power_error.svg");
    }
    #endregion
}
