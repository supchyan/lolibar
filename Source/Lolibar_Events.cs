using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows;

namespace LolibarApp.Source
{
    partial class Lolibar : Window
    {
        #region Events
        void Container_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Hand;
            LolibarAnimator.BeginDecOpacityAnimation((UIElement)sender);
        }
        void Container_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Arrow;
            LolibarAnimator.BeginIncOpacityAnimation((UIElement)sender);
        }

        void BarUserContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName = "powershell.exe",
                    Arguments = "Start-Process ms-settings:accounts",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            }.Start();
        }

        void BarCurProcContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName        = "powershell.exe",
                    Arguments       = "Start-Process taskmgr.exe",
                    UseShellExecute = false,
                    CreateNoWindow  = true,
                }
            }.Start();
        }
        void BarCurProcContainer_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LolibarDefaults.ChangeCurProcInfo();
        }

        void BarRamContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LolibarDefaults.ChangeRamInfo();
        }

        void BarSoundContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName = "powershell.exe",
                    Arguments = "Start-Process ms-settings:sound",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            }.Start();
        }

        void BarPowerContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName = "powershell.exe",
                    Arguments = "Start-Process ms-settings:batterysaver",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            }.Start();
        }

        void BarTimeContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName = "powershell.exe",
                    Arguments = "Start-Process ms-settings:dateandtime",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            }.Start();
        }

        void Lolibar_ContentRendered(object? sender, EventArgs e)
        {
            transformToDevice = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            screenSize = (System.Windows.Size)transformToDevice.Transform(new System.Windows.Point((float)LolibarHelper.Inch_ScreenWidth, (float)LolibarHelper.Inch_ScreenHeight));

            Left = (LolibarHelper.Inch_ScreenWidth - Width) / 2;
            Top  = LolibarHelper.Inch_ScreenHeight;

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
                    LolibarAnimator.BeginStatusBarShowAnimation(this);
                }
                else
                {
                    LolibarAnimator.BeginStatusBarHideAnimation(this);
                }
                oldIsHidden = IsHidden;
            }
        }
        void Lolibar_Closed(object? sender, EventArgs e)
        {
            if (!LolibarHelper.CanBeClosed)
            {
                LolibarHelper.RestartApplicationGently();
            }
        }
        #endregion
    }
}
