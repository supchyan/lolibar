using Microsoft.VisualBasic.Devices;

namespace LolibarApp.Tools
{
    public partial class LolibarDefaults
    {
        static bool             ShowRamInPercent        = true;

        public static string?   CurProcIdInfo           { get; private set; }
        public static string?   CurProcNameInfo         { get; private set; }

        public static string?   CpuInfo                 { get; private set; }

        public static string?   RamUsedInPercentInfo    { get; private set; }
        public static string?   RamUsedInGbInfo         { get; private set; }

        public static string?   GpuInfo                 { get; private set; }
        public static string?   DiskInfo                { get; private set; }
        public static string?   NetworkInfo             { get; private set; }

        public static string?   SoundInfo               { get; private set; }
        public static string?   PowerInfo               { get; private set; }
        public static string?   TimeInfo                { get; private set; }


        public static void ChangeRamInfo()
        {
            ShowRamInPercent = !ShowRamInPercent;
        }
        public static string RamInfo()
        {
            if (ShowRamInPercent)
            {
                 return RamUsedInPercentInfo == null ? "" : RamUsedInPercentInfo;
            }
            else return RamUsedInGbInfo      == null ? "" : RamUsedInGbInfo;
        }

        // Instantiate Method
        public static void Initialize()
        {

        }
        // Update Method
        public static void Update()
        {
            var computerInfo = new ComputerInfo();
            var powerStatus = SystemInformation.PowerStatus;

            CurProcIdInfo       = $"{PerfMonitor.GetForegroundProcessInfo()[0]}";
            CurProcNameInfo     = $"{PerfMonitor.GetForegroundProcessInfo()[1]}";
            
            CpuInfo             = $"{String.Format("{0:0.0}", Math.Round(PerfMonitor.CPU_Total.NextValue(), 1))}%";
            
            RamUsedInPercentInfo  = $"{String.Format("{0:0.0}", Math.Round(100.0 * (1.0 - ((double)computerInfo.AvailablePhysicalMemory / (double)computerInfo.TotalPhysicalMemory)), 1))}%";
            RamUsedInGbInfo       = $"{String.Format("{0:0.0}", Math.Round((double)computerInfo.TotalPhysicalMemory - (double)computerInfo.AvailablePhysicalMemory) / 1024.0 / 1024.0 / 1024.0)}GB";
            
            GpuInfo             = $"No Data";
            DiskInfo            = $"No Data";
            NetworkInfo         = $"No Data";

            SoundInfo           = $"Sound Properties";
            PowerInfo           = $"{Math.Round(100.0 * SystemInformation.PowerStatus.BatteryLifePercent)}%"; 
            TimeInfo            = $"{DateTime.Now}";

            // Power Icon handling
            if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.High))
            {
                // High Power Icon
                PowerIcon = powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging :
                    PowerIconHigh;
            }
            else if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Low))
            {
                // Low Power Icon
                PowerIcon = powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging :
                    PowerIconLow;
            }
            else if (powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Critical))
            {
                // Critical Power Icon
                PowerIcon = powerStatus.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging :
                    PowerIconCritical ;
            }
            else
            {
                // Empty Power Icon
                PowerIcon = PowerIconEmpty;
            }
        }
    }
}
