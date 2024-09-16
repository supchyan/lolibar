using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Media;
using Microsoft.VisualBasic.Devices;

namespace lolibar.tools
{
    public partial class LolibarDefaults
    {
        static bool             ShowRamInPercent    = true;

        public static string?   CurProcIdInfo       { get; private set; }
        public static string?   CurProcNameInfo     { get; private set; }

        public static string?   CpuInfo             { get; private set; }

        public static string?   RamInfoUsedPercent  { get; private set; }
        public static string?   RamInfoUsedGB       { get; private set; }

        public static string?   GpuInfo             { get; private set; }
        public static string?   DiskInfo            { get; private set; }
        public static string?   NetworkInfo         { get; private set; }

        public static string?   SoundInfo           { get; private set; }
        public static string?   PowerInfo           { get; private set; }
        public static string?   TimeInfo            { get; private set; }

        public static Geometry? CpuIcon             { get; private set; }
        public static Geometry? RamIcon             { get; private set; }
        public static Geometry? GpuIcon             { get; private set; }
        public static Geometry? DiskIcon            { get; private set; }
        public static Geometry? NetworkIcon         { get; private set; }

        public static Geometry? SoundIcon           { get; private set; }
        public static Geometry? PowerIcon           { get; private set; }


        public static void ChangeRamInfo()
        {
            ShowRamInPercent = !ShowRamInPercent;
        }
        public static string RamInfo()
        {
            if (ShowRamInPercent)
            {
                return RamInfoUsedPercent;
            }
            else return RamInfoUsedGB;
        }

        // Instantiate Method
        public static void Initialize()
        {
            CpuIcon             = CpuIcon_Reference;
            RamIcon             = RamIcon_Reference;
            GpuIcon             = GpuIcon_Reference;
            DiskIcon            = DiskIcon_Reference;
            NetworkIcon         = NetworkIcon_Reference;

            SoundIcon           = SoundIcon_Reference;
            PowerIcon           = PowerIconEmpty_Reference;
        }
        // Update Method
        public static void Update()
        {
            var computerInfo = new ComputerInfo();

            CurProcIdInfo       = $"{PerfMonitor.GetForegroundProcessInfo()[0]}";
            CurProcNameInfo     = $"{PerfMonitor.GetForegroundProcessInfo()[1]}";
            
            CpuInfo             = $"{String.Format("{0:0.0}", Math.Round(PerfMonitor.CPU_Total.NextValue(), 1))}%";
            
            RamInfoUsedPercent  = $"{String.Format("{0:0.0}", Math.Round(100.0 * (1.0 - ((double)computerInfo.AvailablePhysicalMemory / (double)computerInfo.TotalPhysicalMemory)), 1))}%";
            RamInfoUsedGB       = $"{String.Format("{0:0.0}", Math.Round((double)computerInfo.TotalPhysicalMemory - (double)computerInfo.AvailablePhysicalMemory) / 1024.0 / 1024.0 / 1024.0)}GB";
            
            GpuInfo             = $"No Data";
            DiskInfo            = $"No Data";
            NetworkInfo         = $"No Data";

            SoundInfo           = $"Sound Properties";
            PowerInfo           = $"{Math.Round(100.0 * SystemInformation.PowerStatus.BatteryLifePercent)}%"; 
            TimeInfo            = $"{DateTime.Now}";

            // Power Icon handling
            PowerStatus PowerStat = SystemInformation.PowerStatus;
            if (PowerStat.BatteryChargeStatus.HasFlag(BatteryChargeStatus.High))
            {
                // High Power Icon
                PowerIcon = PowerStat.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging_Reference :
                    PowerIconHigh_Reference;
            }
            else if (PowerStat.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Low))
            {
                // Low Power Icon
                PowerIcon = PowerStat.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging_Reference :
                    PowerIconLow_Reference;
            }
            else if (PowerStat.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Critical))
            {
                // Critical Power Icon
                PowerIcon = PowerStat.BatteryChargeStatus.HasFlag(BatteryChargeStatus.Charging) ?
                    PowerIconCharging_Reference :
                    PowerIconCritical_Reference ;
            }
            else
            {
                // Empty Power Icon
                PowerIcon = PowerIconEmpty_Reference;
            }
        }
    }
}
