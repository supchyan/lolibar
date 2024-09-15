using Ikst.MouseHook;
using System.Windows;

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

            bool Show_Trigger, Hide_Trigger;
            
            bool MouseMinY = mouseStruct.pt.y <= 0;
            bool MouseMaxY = mouseStruct.pt.y >= screenSize.Height;

            bool MouseMinX = mouseStruct.pt.x <= 0;
            bool MouseMaxX = mouseStruct.pt.x >= screenSize.Width;

            var BarSizeY = Height + 4 * (double)Resources["BarMargin"];

            if (!(bool)Resources["SnapToTop"])
            {
                Show_Trigger = MouseMaxY && (MouseMinX || MouseMaxX);
                Hide_Trigger = mouseStruct.pt.y < screenSize.Height - BarSizeY;
            }
            else
            {
                Show_Trigger = MouseMinY && (MouseMinX || MouseMaxX);
                Hide_Trigger = mouseStruct.pt.y > BarSizeY;
            }

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
                    BeginStatusBarShowAnimation(this);
                }
                else
                {
                    BeginStatusBarHideAnimation(this);
                }
                oldIsHidden = IsHidden;
            }
        }
        #endregion
    }
}
