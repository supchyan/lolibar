using lolibar.tools;
using System.Windows;
using System.Windows.Media;

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
            Resources["BarOpacity"]         = 1;
            Resources["BarStrokeThickness"] = 0.0;
            Resources["BarColor"]           = new SolidColorBrush(System.Windows.Media.Color.FromRgb(3, 7, 33));
            
            Resources["ElementMargin"]      = new Thickness(16.0, 0.0, 16.0, 0.0);
            Resources["ElementColor"]       = new SolidColorBrush(System.Windows.Media.Color.FromRgb(122, 169, 180));
            Resources["IconSize"]           = 16.0;
            Resources["FontSize"]           = 12.0;

            Resources["BarUserText"]       = "";
            Resources["BarUserIcon"]       = LolibarDefaults.UserIcon;

            Resources["BarCurProcText"]    = "";
            Resources["BarCurProcIcon"]    = LolibarDefaults.CurProcIcon;

            Resources["BarRamText"]        = "";
            Resources["BarRamIcon"]        = LolibarDefaults.RamIcon;

            Resources["BarCpuText"]        = "";
            Resources["BarCpuIcon"]        = LolibarDefaults.CpuIcon;

            Resources["BarPowerText"]      = "";
            Resources["BarPowerIcon"]      = LolibarDefaults.PowerIcon;

            Resources["BarTimeText"]       = "";
            Resources["BarUserIcon"]       = LolibarDefaults.TimeIcon;


            BarTimeIcon.Visibility          = Visibility.Collapsed;
            BarCurProcIcon.Visibility       = Visibility.Collapsed;
            BarUserIcon.Visibility          = Visibility.Collapsed;
        }

        // Updates every "UpdateDelay".
        void Update()
        {
            Resources["BarUserText"]       = LolibarDefaults.UserInfo;

            Resources["BarCurProcText"]    = LolibarDefaults.CurProcInfo;

            Resources["BarRamText"]        = LolibarDefaults.RamInfo;

            Resources["BarCpuText"]        = LolibarDefaults.CpuInfo;

            Resources["BarPowerText"]      = LolibarDefaults.PowerInfo;
            Resources["BarPowerIcon"]      = LolibarDefaults.PowerIcon;

            Resources["BarTimeText"]       = LolibarDefaults.TimeInfo;
        }
        #endregion
    }
}
