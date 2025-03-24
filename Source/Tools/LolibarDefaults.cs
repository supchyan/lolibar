using Microsoft.VisualBasic.Devices;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Media;
using System.IO;

namespace LolibarApp.Source.Tools;

public partial class LolibarDefaults
{
    static bool ShowRamInPercent    = true;
    static int  DiskInfoState       = 0;
    static int  NetworkInfoState    = 0;

    public static string CurrentApplicationId   { get; private set; }   = string.Empty;
    public static string CurrentApplicationName { get; private set; }   = string.Empty;

    /// <summary>
    /// Current execution path.
    /// </summary>
    public static string ExecutionPath
    {
        get
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".\\";
        }
    }

    #region User
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
    public static Geometry? GetCurProcIcon()
    {
        return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\process_sine.svg");
    }
    #endregion

    #region Cpu
    public static string? GetCpuInfo()
    {
        return $"{String.Format("{0:0.0}", Math.Round(LolibarPerfMon.CPU_Total.NextValue(), 1))}%";
    }
    public static Geometry? GetCpuIcon()
    {
        return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\cpu.svg");
    }
    #endregion

    #region Ram
    public static void SwapRamInfo()
    {
        ShowRamInPercent = !ShowRamInPercent;
    }
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
        return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\ram.svg");
    }
    #endregion

    #region Disk
    public static void SwapDiskInfo()
    {
        if (DiskInfoState < 2) DiskInfoState++;
        else DiskInfoState = 0;
    }
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
            case 0:     return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\disk.svg"        );    // read + write average usage
            case 1:     return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\disk_read.svg"   );   // only read average usage
            case 2:     return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\disk_write.svg"  );  // only write average usage
            default:    break;
        }
        return null;
    }
    #endregion

    #region Network
    public static void SwapNetworkInfo()
    {
        if (NetworkInfoState < 2) NetworkInfoState++;
        else NetworkInfoState = 0;
    }
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
            case 0:     return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\network.svg"         );    // total kbps usage
            case 1:     return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\network_send.svg"    );   // sent kbps usage
            case 2:     return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\network_receive.svg" );  // received kbps usage
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
            return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\power_charge.svg");
        }
        if (GetPowerPercent() >= 80)
        {
            return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\power_high.svg");
        }
        if (GetPowerPercent() >= 30)
        {
            return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\power_low.svg");
        }
        if (GetPowerPercent() < 30)
        {
            return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\power_crit.svg");
        }

        return LolibarIcon.ParseSVG(@".\Icons\Defaults\svg\power_error.svg");
    }
    #endregion

    #region Time
    public static string? GetTimeInfo()
    {
        return $"{DateTime.Now}";
    }
    #endregion
}
