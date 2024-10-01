using System.Windows;
using System.Windows.Media;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using LolibarApp.Mods;
using System.Windows.Controls;
using System.Diagnostics;
using System.Reflection;

namespace LolibarApp.Source;

public partial class Lolibar : Window
{
    // --- Links to the root containers ---
    public static StackPanel BarLeftContainer   { get; private set; } = new StackPanel();
    public static StackPanel BarCenterContainer { get; private set; } = new StackPanel();
    public static StackPanel BarRightContainer  { get; private set; } = new StackPanel();

    // Misc
    readonly MouseHook MouseHandler = new();
    readonly Config config = new(); // We use Config's object to invoke Update() and Initialize() methods.
    readonly LolibarVirtualDesktop lolibarVirtualDesktop = new();

    // --- Screen calculation properties ---
    Matrix TransformToDevice                    { get; set; }
    static System.Windows.Size ScreenSize       { get; set; }
    public static double StatusBarVisiblePosY   { get; private set; }
    public static double StatusBarHidePosY      { get; private set; }

    public static double Inch_ScreenWidth       { get; private set; }
    public static double Inch_ScreenHeight      { get; private set; }

    // --- Drawing triggers ---
    bool IsHidden                               { get; set; }
    bool OldIsHidden                            { get; set; }

    // Null window to prevent lolibar's appearing inside alt+tab menu
    Window nullWin = new()
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
    }
    void PreUpdateScreenSize()
    {
        Inch_ScreenWidth    = SystemParameters.PrimaryScreenWidth;
        Inch_ScreenHeight   = SystemParameters.PrimaryScreenHeight;
        TransformToDevice   = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
        ScreenSize          = (System.Windows.Size)TransformToDevice.Transform(new System.Windows.Point((float)Inch_ScreenWidth, (float)Inch_ScreenHeight));
    }
    static void PreUpdateSnapping()
    {
        if (!Config.BarSnapToTop)
        {
            StatusBarVisiblePosY = Inch_ScreenHeight - Config.BarHeight - Config.BarMargin;
            StatusBarHidePosY    = Inch_ScreenHeight;
        }
        else
        {
            StatusBarVisiblePosY = Config.BarMargin;
            StatusBarHidePosY    = -Config.BarHeight - Config.BarMargin;
        }
    }

    /// <summary>
    /// Updates root properties.
    /// </summary>
    void PostUpdateRootProperties()
    {
        Width               = Config.U_BarWidth;
        Height              = Config.BarHeight;

        Left                = Config.U_BarLeft;
        
        FontSize            = Config.BarFontSize;

        RootGrid.Opacity    = Config.BarOpacity;

        Bar.Background      = Config.BarColor;
        Bar.CornerRadius    = Config.BarCornerRadius;
        Bar.BorderThickness = Config.BarStrokeThickness;
        Bar.BorderBrush     = Config.BarContainersContentColor;

        _BarLeftContainer.Margin = _BarCenterContainer.Margin = _BarRightContainer.Margin = Config.BarContainerMargin;
    }

    #region Lifecycle
    void InitializeCycle()
    {
        // --- Initialize ---
        config.Initialize();
    }
    async void UpdateCycle()
    {
        while (true)
        {
            await Task.Delay(Config.BarUpdateDelay);

            // --- PreUpdate ---
            PreUpdateScreenSize();
            PreUpdateSnapping();

            // --- Update ---
            config.Update();

            // --- PostUpdate ---
            PostUpdateRootProperties();
        }
    }
    #endregion

    #region Events
    void Lolibar_ContentRendered(object? sender, EventArgs e)
    {
        IsRendered = true;
    }

    void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        if (!IsRendered) return;

        bool ShowTrigger, HideTrigger;

        bool MouseMinY = mouseStruct.pt.y <= 0;
        bool MouseMaxY = mouseStruct.pt.y >= ScreenSize.Height;

        bool MouseMinX = mouseStruct.pt.x <= 0;
        bool MouseMaxX = mouseStruct.pt.x >= ScreenSize.Width;

        var BarSizeY = Height + 4 * Config.BarMargin;

        if (!Config.BarSnapToTop)
        {
            ShowTrigger = MouseMaxY && (MouseMinX || MouseMaxX);
            HideTrigger = mouseStruct.pt.y < ScreenSize.Height - BarSizeY;
        }
        else
        {
            ShowTrigger = MouseMinY && (MouseMinX || MouseMaxX);
            HideTrigger = mouseStruct.pt.y > BarSizeY;
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
