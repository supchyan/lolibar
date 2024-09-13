using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Ikst.MouseHook;

namespace lolibar
{
    public partial class MainWindow : Window
    {
        // Misc
        private readonly MouseHook mouseHook = new();

        // Screen coordinates
        Matrix transformToDevice;
        Size screenSize;

        readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
        readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;

        // Styles
        double BarWidth = 1500;
        double BarHeight = 36;
        double BarBorderRadius = 6;
        double BarMargin = 8;

        Thickness ElementMargin = new Thickness(8, 0, 8, 0);

        SolidColorBrush TextColor = new SolidColorBrush(Color.FromRgb(120, 120, 120));

        // Drawing conditions
        bool IsHidden, oldIsHidden;

        // Info
        string BarUser = $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
        string BarTime = DateTime.Now.ToString();
        string BarCPU = "wait";
        string BarRAM = "wait";

        // Counters
        PerformanceCounter CPU_Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        PerformanceCounter RAM_Counter = new PerformanceCounter("Memory", "Available MBytes");

        public MainWindow()
        {
            InitializeComponent();

            SetBarSizeRelatedToWindow();

            ContentRendered += MainWindow_ContentRendered;

            Resources.Add("BarWidth", BarWidth);
            Resources.Add("BarHeight", BarHeight);
            Resources.Add("BarBorderRadius", BarBorderRadius);

            Resources.Add("ElementMargin", ElementMargin);
            Resources.Add("TextColor", TextColor);

            Resources.Add("BarUser", BarUser);
            Resources.Add("BarTime", BarTime);
            Resources.Add("BarCPU", BarCPU);
            Resources.Add("BarRAM", BarRAM);

            mouseHook.MouseMove += MouseHook_MouseMove;
            mouseHook.Start();

            Update();
        }

        async void Update()
        {
            while (true)
            {
                await Task.Delay(1000);
                Resources["BarTime"] = DateTime.Now.ToString();
                Resources["BarCPU"] = $"{Math.Round(CPU_Counter.NextValue(), 2)}%";
                Resources["BarRAM"] = $"{Math.Round(RAM_Counter.NextValue() / 1024, 2)}GB";
            }
        }

        void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            transformToDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            screenSize = (Size)transformToDevice.Transform(new Point((float)Inch_ScreenWidth, (float)Inch_ScreenHeight));

            Left = (Inch_ScreenWidth - Width) / 2;
            Top = Inch_ScreenHeight;
        }

        void MouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            var Show_Trigger = screenSize.Height;
            var Hide_Trigger = screenSize.Height - Height - 4 * BarMargin;

            var StartPosition = Inch_ScreenHeight - Height - BarMargin;
            var EndPosition = Inch_ScreenHeight;

            var ShowAnimation = new DoubleAnimation
            {
                From = EndPosition,
                To = StartPosition,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            var HideAnimation = new DoubleAnimation
            {
                From = StartPosition,
                To = EndPosition,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };

            Storyboard SB_Show = new();
            SB_Show.Children.Add(ShowAnimation);

            Storyboard SB_Hide = new();
            SB_Hide.Children.Add(HideAnimation);

            if (mouseStruct.pt.y >= Show_Trigger)
            {
                IsHidden = true;
            }
            else if (mouseStruct.pt.y < Hide_Trigger)
            {
                IsHidden = false;
            }

            if (oldIsHidden != IsHidden)
            {
                if (IsHidden)
                {
                    Storyboard.SetTarget(ShowAnimation, this);
                    Storyboard.SetTargetProperty(ShowAnimation, new PropertyPath(TopProperty));
                    SB_Show.Begin(this);
                }
                else
                {
                    Storyboard.SetTarget(HideAnimation, this);
                    Storyboard.SetTargetProperty(HideAnimation, new PropertyPath(TopProperty));
                    SB_Hide.Begin(this);
                }
                oldIsHidden = IsHidden;
            }

            
        }

        void SetBarSizeRelatedToWindow()
        {
            BarWidth = Inch_ScreenWidth - 2 * BarMargin;
        }
    }
}