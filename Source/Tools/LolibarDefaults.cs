using Microsoft.VisualBasic.Devices;
using System.Security.Principal;
using System.Windows.Media;

namespace LolibarApp.Source.Tools
{
    public partial class LolibarDefaults
    {
        static bool ShowRamInPercent    = true;
        static int  CurProcInfoState    = 0;
        static int  DiskInfoState       = 0;
        static int  NetworkInfoState    = 0;

        #region Ram
        public static void ChangeRamInfo()
        {
            ShowRamInPercent = !ShowRamInPercent;
        }
        public static string GetRamInfo()
        {
            var computerInfo            = new ComputerInfo();
            var RamUsedInPercentInfo    = $"{String.Format("{0:0.0}", Math.Round(100.0 * (1.0 - ((double)computerInfo.AvailablePhysicalMemory / (double)computerInfo.TotalPhysicalMemory)), 1))}%";
            var RamUsedInGbInfo         = $"{String.Format("{0:0.0}", Math.Round((double)computerInfo.TotalPhysicalMemory - (double)computerInfo.AvailablePhysicalMemory) / 1024.0 / 1024.0 / 1024.0)}Gb";

            if (ShowRamInPercent)   return RamUsedInPercentInfo ?? "";
            else                    return RamUsedInGbInfo ?? "";
        }
        #endregion

        #region CurProc
        public static void ChangeCurProcInfo()
        {
            if (CurProcInfoState < 2)   CurProcInfoState++;
            else                        CurProcInfoState = 0;
        }
        public static string? GetCurProcInfo()
        {
            var CurProcIdInfo   = $"{PerfMonitor.GetForegroundProcessInfo()[0]}";
            var CurProcNameInfo = $"{PerfMonitor.GetForegroundProcessInfo()[1]}";

            switch (CurProcInfoState)
            {
                case 0: // name + id
                    var nameAndId = "";

                    if (CurProcNameInfo != null)    nameAndId += $"{CurProcNameInfo} : ";
                    if (CurProcIdInfo   != null)    nameAndId += $"{CurProcIdInfo}";

                    return nameAndId;

                case 1: // only name // TODO Replace this by more useful info about proc
                    return CurProcNameInfo ?? "";

                case 2: // only id  // TODO Replace this by more useful info about proc
                    return CurProcIdInfo ?? "";

            }
            return null;
        }
        #endregion

        #region Disk
        public static void ChangeDiskInfo()
        {
            if (DiskInfoState < 2) DiskInfoState++;
            else DiskInfoState = 0;
        }
        public static string? GetDiskInfo()
        {
            switch (DiskInfoState)
            {
                case 0: // read + write average usage
                    return $"{String.Format("{0:0.0}", Math.Round(PerfMonitor.Disk_Total.NextValue(), 1))}%";

                case 1: // only read average usage
                    return $"{String.Format("{0:0.0}", Math.Round(PerfMonitor.Disk_Read_Total.NextValue(), 1))}%";

                case 2: // only write average usage
                    return $"{String.Format("{0:0.0}", Math.Round(PerfMonitor.Disk_Write_Total.NextValue(), 1))}%";

            }
            return null;
        }
        public static Geometry? GetDiskIcon()
        {
            switch (DiskInfoState)
            {
                case 0: return DiskNormalIcon;  // read + write average usage
                case 1: return DiskReadIcon;    // only read average usage
                case 2: return DiskWriteIcon;   // only write average usage
            }
            return null;
        }
        #endregion

        #region Power
        public static string? GetPowerInfo()
        {
            return $"{Math.Round(100.0 * SystemInformation.PowerStatus.BatteryLifePercent)}%";
        }
        public static Geometry? GetPowerIcon()
        {
            var powerStatus = SystemInformation.PowerStatus;
            // Power Icon handling
            if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.High))
            {
                // High Power Icon
                return powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging :
                    PowerIconHigh;
            }
            else if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Low))
            {
                // Low Power Icon
                return powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging :
                    PowerIconLow;
            }
            else if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Critical))
            {
                // Critical Power Icon
                return powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging :
                    PowerIconCritical;
            }
            else
            {
                // Empty Power Icon
                return PowerIconEmpty;
            }
        }
        #endregion

        #region Sound
        public static string? GetSoundInfo()
        {
            return $"Sound Properties";
        }
        #endregion

        #region User
        public static string? GetUserInfo()
        {
            return $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
        }
        #endregion

        #region Time
        public static string? GetTimeInfo()
        {
            return $"{DateTime.Now}";
        }
        #endregion

        #region Cpu
        public static string? GetCpuInfo()
        {
            return $"{String.Format("{0:0.0}", Math.Round(PerfMonitor.CPU_Total.NextValue(), 1))}%";
        }
        #endregion

        #region Network
        public static void ChangeNetworkInfo()
        {
            if (NetworkInfoState < 2) NetworkInfoState++;
            else NetworkInfoState = 0;
        }
        public static string? GetNetworkInfo()
        {
            switch (NetworkInfoState)
            {
                case 0: // total kbps usage
                    return $"{Math.Round(PerfMonitor.Network_Bytes_Total.NextValue()    / 1024)}Kbps";

                case 1: // sent kbps usage
                    return $"{Math.Round(PerfMonitor.Network_Bytes_Sent.NextValue()     / 1024)}Kbps";

                case 2: // received kbps usage
                    return $"{Math.Round(PerfMonitor.Network_Bytes_Received.NextValue() / 1024)}Kbps";

            }
            return null;
        }
        public static Geometry? GetNetworkIcon()
        {
            switch (NetworkInfoState)
            {
                case 0: return NetworkNormalIcon;       // total kbps usage
                case 1: return NetworkSentIcon;         // sent kbps usage
                case 2: return NetworkReceivedIcon;     // received kbps usage
            }
            return null;
        }
        #endregion
    }
}
