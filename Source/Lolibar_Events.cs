using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using System.Windows;
using LolibarApp.Mods;

namespace LolibarApp.Source;

partial class Lolibar
{
    #region Events
    
    void Lolibar_ContentRendered(object? sender, EventArgs e)
    {
        TransformToDevice   = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
        ScreenSize          = (System.Windows.Size)TransformToDevice.Transform(new System.Windows.Point((float)LolibarHelper.Inch_ScreenWidth, (float)LolibarHelper.Inch_ScreenHeight));

        Left                = (LolibarHelper.Inch_ScreenWidth - Width) / 2;
        Top                 = LolibarHelper.Inch_ScreenHeight;

        IsRendered          = true;
    }

    void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        if (!IsRendered) return;

        bool Show_Trigger, Hide_Trigger;
        
        bool MouseMinY = mouseStruct.pt.y <= 0;
        bool MouseMaxY = mouseStruct.pt.y >= ScreenSize.Height;

        bool MouseMinX = mouseStruct.pt.x <= 0;
        bool MouseMaxX = mouseStruct.pt.x >= ScreenSize.Width;

        var BarSizeY = Height + 4 * Config.BarMargin;

        if (!Config.BarSnapToTop)
        {
            Show_Trigger = MouseMaxY && (MouseMinX || MouseMaxX);
            Hide_Trigger = mouseStruct.pt.y < ScreenSize.Height - BarSizeY;
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

        if (OldIsHidden != IsHidden)
        {
            if (!IsHidden)
            {
                LolibarAnimator.BeginStatusBarShowAnimation(this);
            }
            else
            {
                LolibarAnimator.BeginStatusBarHideAnimation(this);
            }
            OldIsHidden = IsHidden;
        }
    }
    void Lolibar_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (Control.ModifierKeys.HasFlag(Keys.Alt))
        {
            e.Cancel = true;
        }
    }
    void Lolibar_Closed(object? sender, EventArgs e)
    {
        // Should dispose tray icon [ it don't ]
        trayIcon.Icon       = null;
        trayIcon.Visible    = false;
        trayIcon.Dispose();
        System.Windows.Forms.Application.DoEvents();
    }
    #endregion
}
