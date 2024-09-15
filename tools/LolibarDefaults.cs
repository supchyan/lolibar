using System.Security.Principal;
using System.Windows.Media;
using Microsoft.VisualBasic.Devices;

namespace lolibar.tools
{
    public partial class LolibarDefaults
    {
        static ProcMonitor      procMonitor         = new();

        public static string?   UserInfo            { get; private set; }

        public static string?   CurProcInfo         { get; private set; }

        public static string?   RamInfo             { get; private set; }
        public static string?   VRamInfo            { get; private set; }
        public static string?   CpuInfo             { get; private set; }
        public static string?   DiskInfo            { get; private set; }
        public static string?   NetworkInfo         { get; private set; }

        public static string?   SoundInfo           { get; private set; }
        public static string?   PowerInfo           { get; private set; }
        public static string?   TimeInfo            { get; private set; }

        public static Geometry? UserIcon            { get; private set; }
        public static Geometry? CurProcIcon         { get; private set; }

        public static Geometry? RamIcon             { get; private set; }
        public static Geometry? VRamIcon            { get; private set; }
        public static Geometry? CpuIcon             { get; private set; }
        public static Geometry? DiskIcon            { get; private set; }
        public static Geometry? NetworkIcon         { get; private set; }

        public static Geometry? SoundIcon           { get; private set; }
        public static Geometry? PowerIcon           { get; private set; }
        public static Geometry? TimeIcon            { get; private set; }

        // Instantiate Method
        public static void Initialize()
        {
            UserIcon            = UserIcon_Reference;
            CurProcIcon         = CurProcIcon_Reference;

            RamIcon             = RamIcon_Reference;
            VRamIcon            = VRamIcon_Reference;
            CpuIcon             = CpuIcon_Reference;
            DiskIcon            = DiskIcon_Reference;
            NetworkIcon         = NetworkIcon_Reference;

            SoundIcon           = SoundIcon_Reference;
            PowerIcon           = PowerIconEmpty_Reference;
            TimeIcon            = TimeIcon_Reference;
        }
        // Update Method
        public static void Update()
        {
            var computerInfo = new ComputerInfo();

            UserInfo            = $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";

            CurProcInfo         = $"{procMonitor.GetForegroundProcessInfo()[2]}";

            RamInfo             = $"{Math.Round(100.0 * (1.0 - ((double)computerInfo.AvailablePhysicalMemory / (double)computerInfo.TotalPhysicalMemory)), 2)}%";
            VRamInfo            = $"No Info";
            CpuInfo             = $"{Math.Round(100.0 * ProcMonitor.CPU_Time_Total.NextValue() / Environment.ProcessorCount, 2)}%";
            DiskInfo            = $"No Info";
            NetworkInfo         = $"No Info";

            SoundInfo           = $"Sound Properties";
            PowerInfo           = $"{Math.Round(100.0 * SystemInformation.PowerStatus.BatteryLifePercent, 2)}%";
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
