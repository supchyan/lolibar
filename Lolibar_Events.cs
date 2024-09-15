using Ikst.MouseHook;
using lolibar.tools;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Events
        void BarUserContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginIncOpacityAnimation(BarUserContainer);
        }
        void BarUserContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginDecOpacityAnimation(BarUserContainer);
        }

        void BarCurProcContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginIncOpacityAnimation(BarCurProcContainer);
        }
        void BarCurProcContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginDecOpacityAnimation(BarCurProcContainer);
        }

        void BarRamContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginIncOpacityAnimation(BarRamContainer);
        }
        void BarRamContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginDecOpacityAnimation(BarRamContainer);
        }

        void BarTimeContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginIncOpacityAnimation(BarTimeContainer);
        }
        void BarTimeContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginDecOpacityAnimation(BarTimeContainer);
        }

        void BarPowerContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginIncOpacityAnimation(BarPowerContainer);
        }
        void BarPowerContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginDecOpacityAnimation(BarPowerContainer);
        }

        void BarCpuContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginDecOpacityAnimation(BarCpuContainer);
        }
        void BarCpuContainer_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            BeginIncOpacityAnimation(BarCpuContainer);
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
        #endregion
    }
}
