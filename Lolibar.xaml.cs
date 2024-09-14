using System.Diagnostics;
using System.Reflection;
using System.Windows;
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
        NotifyIcon notifyIcon;

        // For screen coordinates calculation
        Matrix transformToDevice;
        System.Windows.Size screenSize;
        readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
        readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;

        // Drawing conditions
        bool IsHidden, oldIsHidden;

        // Null window to prevent lolibar's appearing inside alt+tab menu
        Window nullWin = new()
        {
            Visibility = Visibility.Hidden,
            WindowStyle = WindowStyle.ToolWindow,
            ShowInTaskbar = false,
            Width = 0, Height = 0,
        };

        // Trigger to prevent different job before... window rendered
        bool IsRendered;

        // Properties for animations
        Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
        Duration el_duration = new Duration(TimeSpan.FromSeconds(0.1));
        CubicEase easing = new CubicEase { EasingMode = EasingMode.EaseInOut };

        public Lolibar()
        {
            InitializeComponent();

            // Move lolibar into null window
            nullWin.Show();
            Owner = GetWindow(nullWin);

            ContentRendered += Lolibar_ContentRendered;

            BarUserContainer.MouseEnter += BarUserContainer_MouseEnter;
            BarUserContainer.MouseLeave += BarUserContainer_MouseLeave;

            BarCurProcContainer.MouseEnter += BarCurProcContainer_MouseEnter;
            BarCurProcContainer.MouseLeave += BarCurProcContainer_MouseLeave;

            BarRamContainer.MouseEnter += BarRamContainer_MouseEnter;
            BarRamContainer.MouseLeave += BarRamContainer_MouseLeave;

            BarCpuContainer.MouseEnter += BarCpuContainer_MouseEnter;
            BarCpuContainer.MouseLeave += BarCpuContainer_MouseLeave;

            BarPowerContainer.MouseEnter += BarPowerContainer_MouseEnter;
            BarPowerContainer.MouseLeave += BarPowerContainer_MouseLeave;

            BarTimeContainer.MouseEnter += BarTimeContainer_MouseEnter;
            BarTimeContainer.MouseLeave += BarTimeContainer_MouseLeave;

            _Awake();
            _Update();

            MouseHandler.MouseMove += MouseHandler_MouseMove;
            MouseHandler.Start();

            notifyIcon = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "lolibar",
                Visible = true,
                ContextMenuStrip = new()
                {
                    Items = 
                    {
                        new ToolStripMenuItem("GitHub", null, GitHubRef),
                        new ToolStripMenuItem("Exit", null, ExitRef)
                    }
                }
            };
        }

        void BarUserContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseIncOpacityAnimation(BarUserContainer);
        }
        void BarUserContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseDecOpacityAnimation(BarUserContainer);
        }

        void BarCurProcContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseIncOpacityAnimation(BarCurProcContainer);
        }
        void BarCurProcContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseDecOpacityAnimation(BarCurProcContainer);
        }

        void BarRamContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseIncOpacityAnimation(BarRamContainer);
        }
        void BarRamContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseDecOpacityAnimation(BarRamContainer);
        }

        void BarTimeContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseIncOpacityAnimation(BarTimeContainer);
        }
        void BarTimeContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseDecOpacityAnimation(BarTimeContainer);
        }

        void BarPowerContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseIncOpacityAnimation(BarPowerContainer);
        }
        void BarPowerContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseDecOpacityAnimation(BarPowerContainer);
        }

        void BarCpuContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseDecOpacityAnimation(BarCpuContainer);
        }
        void BarCpuContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            UseIncOpacityAnimation(BarCpuContainer);
        }

        void Lolibar_ContentRendered(object? sender, EventArgs e)
        {
            transformToDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            screenSize = (System.Windows.Size)transformToDevice.Transform(new System.Windows.Point((float)Inch_ScreenWidth, (float)Inch_ScreenHeight));

            Left = (Inch_ScreenWidth - Width) / 2;
            Top = Inch_ScreenHeight;

            IsRendered = true;
        }

        void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            if (!IsRendered) return;

            var Show_Trigger = mouseStruct.pt.y >= screenSize.Height && (mouseStruct.pt.x <= 0 || mouseStruct.pt.x >= screenSize.Width);
            var Hide_Trigger = mouseStruct.pt.y < screenSize.Height - Height - 4 * (double)Resources["BarMargin"];

            Storyboard SB_Show = new();
            Storyboard SB_Hide = new();

            var ShowAnimation = new DoubleAnimation
            {
                From = Top,
                To = Inch_ScreenHeight - Height - (double)Resources["BarMargin"],
                Duration = duration,
                EasingFunction = easing
            };
            var HideAnimation = new DoubleAnimation
            {
                From = Top,
                To = Inch_ScreenHeight,
                Duration = duration,
                EasingFunction = easing
            };
            var OpacityOnAnimation = new DoubleAnimation
            {
                From = Opacity,
                To = 1,
                Duration = duration,
                EasingFunction = easing
            };
            var OpacityOffAnimation = new DoubleAnimation
            {
                From = Opacity,
                To = 0,
                Duration = duration,
                EasingFunction = easing
            };

            SB_Show.Children.Add(ShowAnimation);
            SB_Show.Children.Add(OpacityOnAnimation);

            SB_Hide.Children.Add(HideAnimation);
            SB_Hide.Children.Add(OpacityOffAnimation);

            if (Show_Trigger)
            {
                IsHidden = false;
            }
            else if (Hide_Trigger)
            {
                IsHidden = true;
            }

            if (oldIsHidden != IsHidden)
            {
                if (!IsHidden)
                {
                    Storyboard.SetTarget(ShowAnimation, this);
                    Storyboard.SetTargetProperty(ShowAnimation, new PropertyPath(TopProperty));

                    Storyboard.SetTarget(OpacityOnAnimation, this);
                    Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

                    SB_Show.Begin(this);
                }
                else
                {
                    Storyboard.SetTarget(HideAnimation, this);
                    Storyboard.SetTargetProperty(HideAnimation, new PropertyPath(TopProperty));

                    Storyboard.SetTarget(OpacityOffAnimation, this);
                    Storyboard.SetTargetProperty(OpacityOffAnimation, new PropertyPath(OpacityProperty));

                    SB_Hide.Begin(this);
                }
                oldIsHidden = IsHidden;
            }
        }

        // Misc
        void SetOptimalBarSize()
        {
            Resources["BarMargin"] = Resources["BarMargin"] == null ? 8 : Resources["BarMargin"];
            Resources["BarHeight"] = Resources["BarHeight"] == null ? 40 : Resources["BarHeight"];
            Resources["BarWidth"] = Inch_ScreenWidth - 2 * (double)Resources["BarMargin"];
        }
        void UseDecOpacityAnimation(UIElement _)
        {
            Storyboard SB = new();
            var Animation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 0.5,
                Duration = el_duration,
                EasingFunction = easing
            };
            SB.Children.Add(Animation);
            Storyboard.SetTarget(Animation, _);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(OpacityProperty));
            SB.Begin((FrameworkElement)_);
        }
        void UseIncOpacityAnimation(UIElement _)
        {
            Storyboard SB = new();
            var Animation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 1,
                Duration = el_duration,
                EasingFunction = easing
            };
            SB.Children.Add(Animation);
            Storyboard.SetTarget(Animation, _);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(OpacityProperty));
            SB.Begin((FrameworkElement)_);
        }

        // Config Methods Calls
        void _Awake()
        {
            // Sets in Config.cs
            Awake();

            SetOptimalBarSize();
        }
        async void _Update()
        {
            while (true)
            {
                await Task.Delay((int)Resources["UpdateDelay"]);

                LolibarDefaults.Listen();

                // Sets in Config.cs
                Update();
            }
        }

        // Tray Content
        void GitHubRef(object? sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/supchyan/lolibar");
        }
        void ExitRef(object? sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            // Close container and the lolibar
            nullWin.Close();
            Close();
        }
    }
}