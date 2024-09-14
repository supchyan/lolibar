using lolibar.tools;
using System.Security.Principal;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

// This is config part of the Lolibar Class. You can freely customize statusbar appearance here.
namespace lolibar
{
    partial class Lolibar : Window
    {
        // Runs once after launch.
        void Awake()
        {
            // If it isn't set, lolibar's width will be [ screen_width - bar_margin ]
            //Resources["BarWidth"]         = 800.0;
            Resources["BarHeight"]          = 40.0;
            Resources["BarMargin"]          = 8.0;
            Resources["BarBorderRadius"]    = 6.0;
            Resources["BarOpacity"]         = 1.0;
            Resources["BarColor"]           = new SolidColorBrush(System.Windows.Media.Color.FromRgb(3, 7, 33));

            Resources["ElementMargin"]      = new Thickness(16.0, 0.0, 16.0, 0.0);
            Resources["ElementColor"]       = new SolidColorBrush(System.Windows.Media.Color.FromRgb(122, 169, 180));
            Resources["IconSize"]           = 16.0;
            Resources["FontSize"]           = 12.0;

            BarTimeIcon.Visibility          = Visibility.Collapsed;
            BarCurProcIcon.Visibility       = Visibility.Collapsed;
            BarUserIcon.Visibility          = Visibility.Collapsed;
        }

        // Updates every second.
        void Update()
        {
            // Here you can set anything you want to show in Lolibar.
            Resources["BarUser_Text"]       = $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
            Resources["BarUser_Icon"]       = Geometry.Parse("M4 4C4 2.89543 4.89543 2 6 2C7.10457 2 8 2.89543 8 4C8 5.10457 7.10457 6 6 6C4.89543 6 4 5.10457 4 4ZM6 0C3.79086 0 2 1.79086 2 4C2 6.20914 3.79086 8 6 8C8.20914 8 10 6.20914 10 4C10 1.79086 8.20914 0 6 0ZM9.3 11.6C9.57713 11.8078 9.98351 12.2839 9.99951 13.9005C9.80004 13.9235 9.53047 13.9452 9.16895 13.9613C8.40139 13.9954 7.38536 14 6 14C4.61464 14 3.59861 13.9954 2.83105 13.9613C2.46953 13.9452 2.19996 13.9235 2.00049 13.9005C2.01649 12.2839 2.42287 11.8078 2.7 11.6C3.13836 11.2712 4.04939 11 6 11C7.95061 11 8.86164 11.2712 9.3 11.6ZM12 14C12 15.933 11.5 16 6 16C0.5 16 0 15.933 0 14C0 10 2 9 6 9C10 9 12 10 12 14Z");

            Resources["BarTime_Text"]       = $"{DateTime.Now}";
            Resources["BarTime_Icon"]       = Geometry.Parse("M14 8C14 11.3137 11.3137 14 8 14C4.68629 14 2 11.3137 2 8C2 4.68629 4.68629 2 8 2C11.3137 2 14 4.68629 14 8ZM16 8C16 12.4183 12.4183 16 8 16C3.58172 16 0 12.4183 0 8C0 3.58172 3.58172 0 8 0C12.4183 0 16 3.58172 16 8ZM9 5.5C9 4.94772 8.55229 4.5 8 4.5C7.44772 4.5 7 4.94772 7 5.5V8C7 8.26522 7.10536 8.51957 7.29289 8.70711L8.79289 10.2071C9.18342 10.5976 9.81658 10.5976 10.2071 10.2071C10.5976 9.81658 10.5976 9.18342 10.2071 8.79289L9 7.58579V5.5Z");
            
            Resources["BarCpu_Text"]        = $"{Math.Round(ProcMonitor.CPU_Counter.NextValue(), 2)}%";
            Resources["BarCpu_Icon"]        = Geometry.Parse("M4 1C4 0.447715 4.44772 0 5 0C5.55228 0 6 0.447715 6 1V2H10V1C10 0.447715 10.4477 0 11 0C11.5523 0 12 0.447715 12 1V2.17071C12.8524 2.47199 13.528 3.14759 13.8293 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H14V10H15C15.5523 10 16 10.4477 16 11C16 11.5523 15.5523 12 15 12H13.8293C13.528 12.8524 12.8524 13.528 12 13.8293V15C12 15.5523 11.5523 16 11 16C10.4477 16 10 15.5523 10 15V14H6V15C6 15.5523 5.55228 16 5 16C4.44772 16 4 15.5523 4 15V13.8293C3.14759 13.528 2.47199 12.8524 2.17071 12H1C0.447715 12 0 11.5523 0 11C0 10.4477 0.447715 10 1 10H2V6H1C0.447715 6 0 5.55228 0 5C0 4.44772 0.447715 4 1 4H2.17071C2.47199 3.14759 3.14759 2.47199 4 2.17071V1ZM12 5C12 4.44772 11.5523 4 11 4H5C4.44772 4 4 4.44772 4 5V11C4 11.5523 4.44772 12 5 12H11C11.5523 12 12 11.5523 12 11V5Z");

            Resources["BarRam_Text"]        = $"{Math.Round(ProcMonitor.RAM_Counter.NextValue() / 1024.0, 2)}GB";
            Resources["BarRam_Icon"]        = Geometry.Parse("M3 0C1.34315 0 0 1.34315 0 3V5C0 6.65685 1.34315 8 3 8V11C3 11.5523 3.44772 12 4 12C4.55228 12 5 11.5523 5 11V8H7V11C7 11.5523 7.44772 12 8 12C8.55228 12 9 11.5523 9 11V8H11V11C11 11.5523 11.4477 12 12 12C12.5523 12 13 11.5523 13 11V8C14.6569 8 16 6.65685 16 5V3C16 1.34315 14.6569 0 13 0H3ZM2 3C2 2.44772 2.44772 2 3 2H13C13.5523 2 14 2.44772 14 3V5C14 5.55228 13.5523 6 13 6H3C2.44772 6 2 5.55228 2 5V3Z");

            Resources["BarPower_Text"]      = $"{SystemInformation.PowerStatus.BatteryLifePercent * 100.0}%";
            Resources["BarPower_Icon"]      = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4H9C9.55228 4 10 4.44772 10 5V9C10 9.55228 9.55228 10 9 10H5C4.44772 10 4 9.55228 4 9V5Z");

            Resources["BarCurProc_Text"]    = procMonitor.GetForegroundProcessInfo()[2];
            Resources["BarCurProc_Icon"]    = Geometry.Parse("M4 1C4 0.447715 4.44772 0 5 0C5.55228 0 6 0.447715 6 1V2H10V1C10 0.447715 10.4477 0 11 0C11.5523 0 12 0.447715 12 1V2.17071C12.8524 2.47199 13.528 3.14759 13.8293 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H14V10H15C15.5523 10 16 10.4477 16 11C16 11.5523 15.5523 12 15 12H13.8293C13.528 12.8524 12.8524 13.528 12 13.8293V15C12 15.5523 11.5523 16 11 16C10.4477 16 10 15.5523 10 15V14H6V15C6 15.5523 5.55228 16 5 16C4.44772 16 4 15.5523 4 15V13.8293C3.14759 13.528 2.47199 12.8524 2.17071 12H1C0.447715 12 0 11.5523 0 11C0 10.4477 0.447715 10 1 10H2V6H1C0.447715 6 0 5.55228 0 5C0 4.44772 0.447715 4 1 4H2.17071C2.47199 3.14759 3.14759 2.47199 4 2.17071V1ZM12 5C12 4.44772 11.5523 4 11 4H5C4.44772 4 4 4.44772 4 5V11C4 11.5523 4.44772 12 5 12H11C11.5523 12 12 11.5523 12 11V5Z");
        }
    }
}
