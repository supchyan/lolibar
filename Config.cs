using lolibar.tools;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;

// This is config part of the Lolibar Class. You can freely customize statusbar appearance here
namespace lolibar
{
    partial class Lolibar : Window
    {
        // Runs once after launch
        void Initialize()
        {
            Resources["UpdateDelay"]        = 1000; // Delay for Update() method. Low delay affects performance!

            // If it's not set, lolibar's size and margin will be automatically initialized.
            // Resources["BarMargin"]       = 4.0;
            // Resources["BarWidth"]        = 800.0; 
            // Resources["BarHeight"]       = 48.0;

            Resources["BarBorderRadius"]    = 6.0;
            Resources["BarOpacity"]         = 1.0;
            Resources["BarStrokeThickness"] = 0.0;
            Resources["BarColor"]           = new SolidColorBrush(System.Windows.Media.Color.FromRgb(3, 7, 33));
            
            Resources["ElementMargin"]      = new Thickness(16.0, 0.0, 16.0, 0.0);
            Resources["ElementColor"]       = new SolidColorBrush(System.Windows.Media.Color.FromRgb(122, 169, 180));
            Resources["IconSize"]           = 16.0;
            Resources["FontSize"]           = 12.0;

            Resources["BarUser_Text"]       = "";
            Resources["BarUser_Icon"]       = LolibarDefaults.UserIcon;

            Resources["BarCurProc_Text"]    = "";
            Resources["BarCurProc_Icon"]    = LolibarDefaults.CurrentProcessIcon;

            Resources["BarRam_Text"]        = "";
            Resources["BarRam_Icon"]        = LolibarDefaults.RamIcon;

            Resources["BarCpu_Text"]        = "";
            Resources["BarCpu_Icon"]        = LolibarDefaults.CpuIcon;

            Resources["BarPower_Text"]      = "";
            Resources["BarPower_Icon"]      = LolibarDefaults.PowerIcon;

            Resources["BarTime_Text"]       = "";
            Resources["BarTime_Icon"]       = LolibarDefaults.TimeIcon;


            BarTimeIcon.Visibility          = Visibility.Collapsed;
            BarCurProcIcon.Visibility       = Visibility.Collapsed;
            BarUserIcon.Visibility          = Visibility.Collapsed;
        }

        // Updates every "UpdateDelay".
        void Update()
        {
            Resources["BarUser_Text"]       = LolibarDefaults.UserInfo;

            Resources["BarCurProc_Text"]    = LolibarDefaults.CurrentProcessInfo;

            Resources["BarRam_Text"]        = LolibarDefaults.RamInfo;

            Resources["BarCpu_Text"]        = LolibarDefaults.CpuInfo;

            Resources["BarPower_Text"]      = LolibarDefaults.PowerInfo;
            Resources["BarPower_Icon"]      = LolibarDefaults.PowerIcon;

            Resources["BarTime_Text"]       = LolibarDefaults.TimeInfo;

        }
    }
}
