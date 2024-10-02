using System.Windows;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using LolibarApp.Modding;
using System.Windows.Controls;
using System.Diagnostics;
using System.Reflection;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LolibarApp.Source;

public partial class Lolibar : Window
{
    // --- Links to the root containers ---
    public static StackPanel BarLeftContainer   { get; private set; } = new StackPanel();
    public static StackPanel BarCenterContainer { get; private set; } = new StackPanel();
    public static StackPanel BarRightContainer  { get; private set; } = new StackPanel();

    // Misc
    readonly MouseHook MouseHandler = new();
    readonly ModClass modClass = new(); // We use Config's object to invoke Update() and Initialize() methods.
    readonly LolibarVirtualDesktop lolibarVirtualDesktop = new();

    // --- Screen calculation properties ---
    public static Vector2 Inch_Screen           { get; private set; }
    public static Vector2 ScreenSize            { get; private set; }

    public static double StatusBarVisiblePosY   { get; private set; }
    public static double StatusBarHidePosY      { get; private set; }


    // --- Drawing triggers ---
    bool IsHidden                               { get; set; }
    bool OldIsHidden                            { get; set; }

    // Null window to prevent lolibar's appearing inside alt+tab menu
    readonly Window nullWin = new()
    {
        Visibility = Visibility.Hidden,
        WindowStyle = WindowStyle.ToolWindow,
        ShowInTaskbar = false,
        Width = 0, Height = 0,
        Left = -100 // to open the null_window outside of the screen 
    };

    // A trigger to prevent different app's job before... it's window actually rendered
    bool IsRendered;

    public Lolibar()
    {
        InitializeComponent();

        ContentRendered += Lolibar_ContentRendered;
        Closing         += Lolibar_Closing;
        Closed          += Lolibar_Closed;

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

        // Should be below Initialize and Update calls, because it has Resources[] dependency
        MouseHandler.MouseMove += MouseHandler_MouseMove;
        MouseHandler.Start();

        SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
    }

    private void SystemParameters_StaticPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        UpdateScreenParameters();
    }

    void UpdateScreenParameters()
    {
        // These applies to your primary screen, so statusbar will be drawn in it only.
        Inch_Screen = new((float)SystemParameters.WorkArea.Width, (float)SystemParameters.WorkArea.Height);
        ScreenSize  = new(LolibarExtern.GetDeviceCaps(LolibarExtern.GetDC(IntPtr.Zero), 118), LolibarExtern.GetDeviceCaps(LolibarExtern.GetDC(IntPtr.Zero), 117));
    }
    static void PreUpdateSnapping()
    {
        if (!ModClass.BarSnapToTop)
        {
            StatusBarVisiblePosY = Inch_Screen.Y - ModClass.BarHeight - ModClass.BarMargin;
            StatusBarHidePosY    = Inch_Screen.Y;
        }
        else
        {
            StatusBarVisiblePosY = ModClass.BarMargin;
            StatusBarHidePosY    = -ModClass.BarHeight - ModClass.BarMargin;
        }
    }

    /// <summary>
    /// Updates root properties.
    /// </summary>
    void PostUpdateRootProperties()
    {
        Width               = ModClass.U_BarWidth;
        Height              = ModClass.BarHeight;

        Left                = ModClass.U_BarLeft;
        
        FontSize            = ModClass.BarFontSize;

        RootGrid.Opacity    = ModClass.BarOpacity;

        Bar.Background      = ModClass.BarColor;
        Bar.CornerRadius    = ModClass.BarCornerRadius;
        Bar.BorderThickness = ModClass.BarStrokeThickness;
        Bar.BorderBrush     = ModClass.BarContainersContentColor;

        _BarLeftContainer.Margin = _BarCenterContainer.Margin = _BarRightContainer.Margin = ModClass.BarContainerMargin;
    }

    #region Lifecycle
    void InitializeCycle()
    {
        // --- PreInitialize ---
        LolibarAudio.InitializeStreamEvents();
        UpdateScreenParameters();

        // --- Initialize ---
        modClass.Initialize();
    }
    async void UpdateCycle()
    {
        while (true)
        {
            await Task.Delay(ModClass.BarUpdateDelay);

            // --- PreUpdate ---
            PreUpdateSnapping();

            // --- Update ---
            modClass.Update();

            // --- PostUpdate ---
            PostUpdateRootProperties();
        }
    }
    #endregion

    #region Events
    void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        if (!IsRendered) return;
        
        bool ShowTrigger, HideTrigger;

        bool MouseMinY = mouseStruct.pt.y <= 0;
        bool MouseMaxY = mouseStruct.pt.y >= ScreenSize.Y;

        bool MouseMinX = mouseStruct.pt.x <= 0;
        bool MouseMaxX = mouseStruct.pt.x >= ScreenSize.X;

        var BarVisibleY = ModClass.BarHeight + 2 * ModClass.BarMargin;

        if (!ModClass.BarSnapToTop)
        {
            ShowTrigger = MouseMaxY && (MouseMinX || MouseMaxX);
            HideTrigger = mouseStruct.pt.y < ScreenSize.Y - BarVisibleY;
        }
        else
        {
            ShowTrigger = MouseMinY && (MouseMinX || MouseMaxX);
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
            }
            else
            {
                LolibarAnimator.BeginStatusBarHideAnimation(this);
            }
            OldIsHidden = IsHidden;
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
        // Should dispose tray icon [ but doesn't ]
        TrayIcon.Icon = null;
        TrayIcon.Visible = false;
        TrayIcon.Dispose();
        System.Windows.Forms.Application.DoEvents();
    }
    #endregion

    #region Tray [ Notify Icon ]
    readonly NotifyIcon TrayIcon = new()
    {
        Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
        Text = "Lolibar Menu",
        Visible = true,
        ContextMenuStrip = new()
        {
            Items =
            {
                new ToolStripMenuItem("Restart", null, OnRestartSelected),
                new ToolStripMenuItem("GitHub",  null, OnGitHubSelected),
                new ToolStripMenuItem("Exit",    null, OnExitSelected)
            }
        }
    };
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
