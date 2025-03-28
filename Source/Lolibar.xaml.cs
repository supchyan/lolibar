﻿using System.Windows;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using System.Windows.Controls;
using System.Diagnostics;
using System.Reflection;
using System.Numerics;
using System.IO;

namespace LolibarApp.Source;

public partial class Lolibar : Window
{
    // --- Misc ---
    MouseHook               MouseHandler            { get; set; } = new();
    LolibarPublicMod        PublicMod               { get; set; } = new();

    // --- Links to the root containers ---
    public static StackPanel BarLeftContainer       { get; private set; } = new StackPanel();
    public static StackPanel BarCenterContainer     { get; private set; } = new StackPanel();
    public static StackPanel BarRightContainer      { get; private set; } = new StackPanel();

    // --- Screen calculation properties ---
    public static Vector2 Inch_Screen               { get; private set; }
    public static Vector2 ScreenSize                { get; private set; }

    public static double StatusBarVisiblePosY       { get; private set; }
    public static double StatusBarHidePosY          { get; private set; }


    // --- Drawing triggers ---
    bool IsHidden                                   { get; set; }
    bool OldIsHidden                                { get; set; }

    // Null window to prevent lolibar's appearing inside alt+tab menu
    readonly Window nullWin = new()
    {
        Visibility          = Visibility.Hidden,
        WindowStyle         = WindowStyle.ToolWindow,
        ShowInTaskbar       = false,
        Width               = 0,
        Height              = 0,
        Left                = -100 // to open the null_window outside of the screen 
    };

    // Trigger to prevent different job before...
    // ...application's window actually rendered
    bool            IsRendered                      { get; set; }

    // --- Cursor velocity calculation ---
    static Vector2  OldCursorPosition               { get; set; }
    static Vector2  CursorPosition                  { get; set; }
    static float    CursorVelocity                  { get; set; }
    static DateTime OldTime                         { get; set; }

    // --- LolibarVirtualDesktop update trigger on lolibar's opening ---
    static bool ShouldManuallyUpdateDynamicLibs { get; set; }

    public Lolibar()
    {
        InitializeComponent();

        ContentRendered     += Lolibar_ContentRendered;
        Closing             += Lolibar_Closing;
        Closed              += Lolibar_Closed;

        // --- Moves lolibar into the null window ---
        nullWin.Show();
        Owner = GetWindow(nullWin);

        // --- Writes main containers into accessable types ---
        BarCenterContainer  = _BarCenterContainer;
        BarLeftContainer    = _BarLeftContainer;
        BarRightContainer   = _BarRightContainer;

        // ---

        InitializeCycle();
        UpdateCycle();

        CheeseUpdateCycle();  // For dynamic libs Update

        // Should be below Initialize and Update calls, because it has Resources[] dependency
        MouseHandler.MouseMove += MouseHandler_MouseMove;
        MouseHandler.Start();

        LolibarAudio.Start();

        SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
    }

    void SystemParameters_StaticPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        UpdateScreenParameters();
    }

    static void UpdateScreenParameters()
    {
        // These applies to your primary screen, so statusbar will be drawn in it only.
        Inch_Screen = new((float)SystemParameters.PrimaryScreenWidth, (float)SystemParameters.PrimaryScreenHeight);
        ScreenSize  = new(LolibarExtern.GetDeviceCaps(LolibarExtern.GetDC(IntPtr.Zero), 118), LolibarExtern.GetDeviceCaps(LolibarExtern.GetDC(IntPtr.Zero), 117));
    }
    static void PreUpdateSnapping()
    {
        if (!LolibarMod.BarSnapToTop)
        {
            StatusBarVisiblePosY = Inch_Screen.Y - LolibarMod.BarHeight - LolibarMod.BarMargin;
            StatusBarHidePosY    = Inch_Screen.Y;
        }
        else
        {
            StatusBarVisiblePosY = LolibarMod.BarMargin;
            StatusBarHidePosY    = -LolibarMod.BarHeight - LolibarMod.BarMargin;
        }
    }

    /// <summary>
    /// Updates root properties.
    /// </summary>
    void PostUpdateRootProperties()
    {
        Width               = LolibarMod.BarWidth;
        Height              = LolibarMod.BarHeight;

        Left                = LolibarMod.BarLeft;
        
        FontSize            = LolibarMod.BarFontSize;

        RootGrid.Opacity    = LolibarMod.BarOpacity;

        Bar.Background      = LolibarMod.BarColor;
        Bar.CornerRadius    = LolibarMod.BarCornerRadius;
        Bar.BorderThickness = LolibarMod.BarStrokeThickness;
        Bar.BorderBrush     = LolibarMod.BarContainersColor;

        _BarLeftContainer.Margin = _BarCenterContainer.Margin = _BarRightContainer.Margin = LolibarMod.BarContainerMargin;
    }

    #region Lifecycle
    void InitializeCycle()
    {
        // --- PreInitialize ---
        UpdateScreenParameters();
        LolibarModLoader.LoadMods();

        // --- Mods PreInitialize ---
        PublicMod.PreInitialize();

        // --- Mods Initialize --
        PublicMod.Initialize();
    }
    async void UpdateCycle()
    {
        while (true)
        {
            await Task.Delay(LolibarMod.BarUpdateDelay);

            // --- PreUpdate ---
            PreUpdateSnapping();

            // --- Update ---
            PublicMod.Update();

            // --- PostUpdate ---
            PostUpdateRootProperties();
        }
    }
    async static void CheeseUpdateCycle() {
        while (true)
        {
            await Task.Delay(100);

            OldCursorPosition = CursorPosition;

            if (ShouldManuallyUpdateDynamicLibs)
            {
                LolibarVirtualDesktop.UpdateInitializedDesktops();
                LolibarProcess.UpdateInitializedPinnedApps();
                LolibarProcess.FetchPinnedAppsContainers();
                ShouldManuallyUpdateDynamicLibs = false;
            }
        }
    }
    #endregion

    #region Events
    void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        if (!IsRendered) return;

        bool ShowTrigger, HideTrigger, IsCursorInDesktopsMenuPosition;

        bool IsMouseMinY = mouseStruct.pt.y <= 0;
        bool IsMouseMaxY = mouseStruct.pt.y >= ScreenSize.Y;

        bool IsMouseMinX = mouseStruct.pt.x <= 0;
        bool IsMouseMaxX = mouseStruct.pt.x >= ScreenSize.X;

        var BarVisibleY = LolibarMod.BarHeight + 2 * LolibarMod.BarMargin;

        if (!LolibarMod.BarSnapToTop)
        {
            ShowTrigger = (IsMouseMinX || IsMouseMaxX) && IsMouseMaxY;
            HideTrigger = mouseStruct.pt.y < ScreenSize.Y - BarVisibleY;
        }
        else
        {
            ShowTrigger = (IsMouseMinX || IsMouseMaxX) && IsMouseMinY;
            HideTrigger = mouseStruct.pt.y > BarVisibleY;
        }

        if (ShowTrigger)
        {
            IsHidden = false;
        }
        else if (HideTrigger)
        {
            IsHidden = true;
        }

        if (OldIsHidden != IsHidden)
        {
            if (!IsHidden)
            {
                LolibarAnimator.BeginStatusBarShowAnimation(this);
                ShouldManuallyUpdateDynamicLibs = true;
            }
            else
            {
                LolibarAnimator.BeginStatusBarHideAnimation(this);
            }
            OldIsHidden = IsHidden;
        }

        // Logic for opening all apps and desktops view (WIN + TAB)
        if (LolibarMod.BarCornersInvokesDesktopsMenu)
        {
            IsCursorInDesktopsMenuPosition =
                    (LolibarMod.BarTargetCorner == LolibarEnums.BarTargetCorner.Left ? IsMouseMinX : IsMouseMaxX) &&
                    (!LolibarMod.BarSnapToTop ? IsMouseMaxY : IsMouseMinY);

            CursorPosition = new Vector2(mouseStruct.pt.x, mouseStruct.pt.y);

            // Prevents CursorPosition get out of bounds values:
            if (CursorPosition.X <= -1f) CursorPosition = new Vector2(-1f, CursorPosition.Y);
            if (CursorPosition.Y <= -1f) CursorPosition = new Vector2(CursorPosition.X, -1f);

            if (CursorPosition.X >= ScreenSize.X) CursorPosition = new Vector2(ScreenSize.X, CursorPosition.Y);
            if (CursorPosition.Y >= ScreenSize.Y) CursorPosition = new Vector2(CursorPosition.X, ScreenSize.Y);
            //

            CursorVelocity = (OldCursorPosition - CursorPosition).Length();
            
            if (IsCursorInDesktopsMenuPosition && CursorVelocity >= 2f && (DateTime.Now - OldTime).Milliseconds > 500)
            {
                LolibarHelper.OpenWindowsDesktopsUI();

                // Prevnts multiple calls of the statement above
                // Also prevents possible WIN+TAB spam, which is breaks Windows OS (lol)
                OldTime = DateTime.Now;
            }
        }
    }
    void Lolibar_ContentRendered(object? sender, EventArgs e)
    {
        IsRendered = true;
    }
    void Lolibar_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (System.Windows.Forms.Control.ModifierKeys.HasFlag(Keys.Alt))
        {
            e.Cancel = true;
        }
    }
    void Lolibar_Closed(object? sender, EventArgs e)
    {
        LolibarHelper.ShowWindowsTaskbar();

        // Clear tray icon
        TrayIcon.Icon       = null;
        TrayIcon.Visible    = false;
        TrayIcon.Dispose();
        System.Windows.Forms.Application.DoEvents();
    }
    #endregion

    #region Tray [ Notify Icon ]
    readonly static ToolStripMenuItem AutorunTrayItem = new(AutorunTrayItemContent(), null, OnAutorunSelected);
    readonly static ToolStripMenuItem RestartTrayItem = new("Restart", null, OnRestartSelected);
    readonly static ToolStripMenuItem GitHubTrayItem = new("GitHub", null, OnGitHubSelected);
    readonly static ToolStripMenuItem CloseTrayItem = new("Close Lolibar", null, OnExitSelected);

    readonly static NotifyIcon TrayIcon = new()
    {
        Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
        Text = "Lolibar In Tray",
        Visible = true,
        ContextMenuStrip = new()
        {
            Items =
            {
                AutorunTrayItem,
                RestartTrayItem,
                GitHubTrayItem,
                CloseTrayItem
            }
        }
    };
    static bool IsAutorunPathExist()
    {
        var PathExists = Path.Exists("C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Startup\\lolibar.lnk");
        
        if (PathExists)
            return true;

        else 
            return false;
    }
    static string AutorunTrayItemContent()
    {
        return IsAutorunPathExist() ? "Autorun (On)" : "Autorun (Off)";
    }
    /// <summary>
    /// Updates the whole Lolibar's Tray Menu, when Autorun property has changed
    /// </summary>
    static void UpdateTrayItems()
    {
        AutorunTrayItem.Text = AutorunTrayItemContent();

        TrayIcon.ContextMenuStrip?.Items.Remove(AutorunTrayItem);
        TrayIcon.ContextMenuStrip?.Items.Add(AutorunTrayItem);

        TrayIcon.ContextMenuStrip?.Items.Remove(RestartTrayItem);
        TrayIcon.ContextMenuStrip?.Items.Add(RestartTrayItem);

        TrayIcon.ContextMenuStrip?.Items.Remove(GitHubTrayItem);
        TrayIcon.ContextMenuStrip?.Items.Add(GitHubTrayItem);

        TrayIcon.ContextMenuStrip?.Items.Remove(CloseTrayItem);
        TrayIcon.ContextMenuStrip?.Items.Add(CloseTrayItem);
    } 
    private static void OnAutorunSelected(object? sender, EventArgs e)
    {
        Process proc = new();
        proc.StartInfo.FileName = @"Autorun\autorun.exe";
        proc.StartInfo.UseShellExecute = true;
        proc.Start();
        proc.WaitForExit();

        UpdateTrayItems();
    }

    // Tray Content
    static void OnRestartSelected(object? sender, EventArgs e)
    {
        LolibarHelper.RestartApplicationGently();
    }
    static void OnGitHubSelected(object? sender, EventArgs e)
    {
        Process.Start("explorer", "https://github.com/supchyan/lolibar");
    }
    static void OnExitSelected(object? sender, EventArgs e)
    {
        LolibarHelper.CloseApplicationGently();
    }
    #endregion
}
