using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Ikst.MouseHook;
using lolibar.tools;

namespace lolibar
{
    public partial class Lolibar : Window
    {
        // Misc
        MouseHook MouseHandler = new();
        Info _Info = new();
        Styles _Styles = new();
        NotifyIcon notifyIcon;

        // Screen coordinates
        Matrix transformToDevice;
        System.Windows.Size screenSize;

        readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
        readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;

        // Drawing conditions
        bool IsHidden, oldIsHidden;

        public Lolibar()
        {
            InitializeComponent();

            SetBarSizeRelatedToWindow();

            ContentRendered += Lolibar_ContentRendered;

            Resources.Add("BarWidth",        _Styles.BarWidth);
            Resources.Add("BarHeight",       _Styles.BarHeight);
            Resources.Add("BarBorderRadius", _Styles.BarBorderRadius);
            Resources.Add("BarOpacity",      _Styles.BarOpacity);
            Resources.Add("ElementMargin",   _Styles.ElementMargin);
            Resources.Add("TextColor",       _Styles.TextColor);

            Resources.Add("BarUser",         _Info.BarUser);
            Resources.Add("BarTime",         _Info.BarTime);
            Resources.Add("BarCPU",          _Info.BarCPU);
            Resources.Add("BarRAM",          _Info.BarRAM);
            Resources.Add("BarCurProcName",  _Info.BarCurProcName);
            Resources.Add("BarCurProcID",    _Info.BarCurProcID);
            Resources.Add("BarCurProc",      _Info.BarCurProc);

            MouseHandler.MouseMove += MouseHandler_MouseMove;
            MouseHandler.Start();

            Update();

            notifyIcon = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "lolibar",
                Visible = true,
                ContextMenuStrip = new()
                {
                    Items = 
                    { 
                        new ToolStripMenuItem("Exit", null, Exit)
                    }
                }
            };
        }
        void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Close();
        }

        async void Update()
        {
            while (true)
            {
                await Task.Delay(1000);

                _Info.GetForegroundProcessInfo();

                Resources["BarUser"]        = $"{WindowsIdentity.GetCurrent().Name.Split('\\')[1]}";
                Resources["BarTime"]        = $"{DateTime.Now}";
                Resources["BarCPU"]         = $"{Math.Round(Info.CPU_Counter.NextValue(), 2)}%";
                Resources["BarRAM"]         = $"{Math.Round(Info.RAM_Counter.NextValue() / 1024.0, 2)}GB";
                Resources["BarPower"]       = $"{SystemInformation.PowerStatus.BatteryLifePercent * 100.0}%";
                
                Resources["BarCurProcName"] = _Info.BarCurProcName;
                Resources["BarCurProcID"]   = _Info.BarCurProcID;
                Resources["BarCurProc"]     = _Info.BarCurProc;
            }
        }

        void Lolibar_ContentRendered(object? sender, EventArgs e)
        {
            transformToDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            screenSize = (System.Windows.Size)transformToDevice.Transform(new System.Windows.Point((float)Inch_ScreenWidth, (float)Inch_ScreenHeight));

            Left = (Inch_ScreenWidth - Width) / 2;
            Top = Inch_ScreenHeight;
        }

        void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            var Show_Trigger = screenSize.Height;
            var Hide_Trigger = screenSize.Height - Height - 4 * _Styles.BarMargin;

            var StartPosition = Inch_ScreenHeight - Height - _Styles.BarMargin;
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
            _Styles.BarWidth = Inch_ScreenWidth - 2 * _Styles.BarMargin;
        }
    }
}