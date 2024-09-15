﻿using Ikst.MouseHook;
using System.Diagnostics;
using System.Windows;

namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Events
        void Container_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Hand;
            BeginDecOpacityAnimation((UIElement)sender);
        }
        void Container_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = System.Windows.Input.Cursors.Arrow;
            BeginIncOpacityAnimation((UIElement)sender);
        }

        void BarCurProcContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new Process
            {
                StartInfo = new()
                {
                    FileName        = "powershell.exe",
                    Arguments       = "taskmgr.exe",
                    UseShellExecute = false,
                    CreateNoWindow  = true,
                }
            }.Start();
        }

        void BarSoundContainer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("Rundll32.exe","shell32.dll,Control_RunDLL Mmsys.cpl,,0");
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
