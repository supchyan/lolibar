using lolibar.tools;
using System.Windows;

// This is config part of the Lolibar Class. You can handle statusbar's content logic here
namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Config
        // Runs once after launch
        void Initialize()
        {
            Resources["SnapToTop"]          = false; // If true, snaps Lolibar to top of the screen
            Resources["UpdateDelay"]        = 1000; // Delay for Update() method. Low delay affects performance!

            // If it's not set, lolibar's size and margin will be automatically initialized.
            // Resources["BarMargin"]       = 4.0;
            // Resources["BarWidth"]        = 800.0; 
            // Resources["BarHeight"]       = 48.0;

            Resources["BarBorderRadius"]    = 6.0;
            Resources["BarOpacity"]         = 1.0;
            Resources["BarStroke"]          = 0.0;
            Resources["BarColor"]           = LolibarHelper.SetColor("#121e46");

            Resources["ElementPadding"]     = new Thickness(8.0,  8.0, 8.0,  8.0); // Gaps inside containers [ such as statistics container ]
            Resources["ElementMargin"]      = new Thickness(16.0, 0.0, 16.0, 0.0);
            Resources["ElementColor"]       = LolibarHelper.SetColor("#8981bd");
            Resources["IconSize"]           = 16.0;
            Resources["FontSize"]           = 12.0;

            Resources["SeparatorWidth"]     = 4.0;

            Resources["BarCurProcIdText"]   = ""; // No icon slot for this

            Resources["BarCurProcNameText"] = ""; // No icon slot for this

            Resources["BarRamText"]         = "";
            Resources["BarRamIcon"]         = LolibarDefaults.RamIcon;

            Resources["BarVRamText"]        = "";
            Resources["BarVRamIcon"]        = LolibarDefaults.VRamIcon;

            Resources["BarCpuText"]         = "";
            Resources["BarCpuIcon"]         = LolibarDefaults.CpuIcon;

            Resources["BarDiskText"]        = "";
            Resources["BarDiskIcon"]        = LolibarDefaults.DiskIcon;

            Resources["BarNetworkText"]     = "";
            Resources["BarNetworkIcon"]     = LolibarDefaults.NetworkIcon;

            Resources["BarSoundText"]       = "";
            Resources["BarSoundIcon"]       = LolibarDefaults.SoundIcon;

            Resources["BarPowerText"]       = "";
            Resources["BarPowerIcon"]       = LolibarDefaults.PowerIcon;

            Resources["BarTimeText"]        = "";
            Resources["BarTimeIcon"]        = LolibarDefaults.TimeIcon;

            BarTimeIcon.Visibility          = Visibility.Collapsed;

            BarCenterContainer.Visibility  = Visibility.Collapsed;
        }

        // Updates every "UpdateDelay".
        void Update()
        {
            Resources["BarCurProcIdText"]   = LolibarDefaults.CurProcIdInfo;
            Resources["BarCurProcNameText"] = LolibarDefaults.CurProcNameInfo;

            Resources["BarRamText"]         = LolibarDefaults.RamInfo;

            Resources["BarVRamText"]        = LolibarDefaults.VRamInfo;

            Resources["BarCpuText"]         = LolibarDefaults.CpuInfo;

            Resources["BarDiskText"]        = LolibarDefaults.DiskInfo;

            Resources["BarNetworkText"]     = LolibarDefaults.NetworkInfo;

            Resources["BarSoundText"]       = LolibarDefaults.SoundInfo;

            Resources["BarPowerText"]       = LolibarDefaults.PowerInfo;
            Resources["BarPowerIcon"]       = LolibarDefaults.PowerIcon;

            Resources["BarTimeText"]        = LolibarDefaults.TimeInfo;
        }
        #endregion
    }
}
