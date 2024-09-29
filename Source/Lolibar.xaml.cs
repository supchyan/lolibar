using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Ikst.MouseHook;
using LolibarApp.Source.Tools;
using LolibarApp.Mods;
using System.Windows.Controls;

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
    System.Windows.Size ScreenSize              { get; set; }
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

        // --- Move lolibar into null window ---
        nullWin.Show();
        Owner = GetWindow(nullWin);

        // --- Write main containers into accessable types ---
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
    static void PreUpdateInchScreenSize()
    {
        Inch_ScreenWidth    = SystemParameters.PrimaryScreenWidth;
        Inch_ScreenHeight   = SystemParameters.PrimaryScreenHeight;
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
    /// Reloads root properties.
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
            PreUpdateInchScreenSize();
            PreUpdateSnapping();

            // --- Update ---
            config.Update();

            // --- PostUpdate ---
            PostUpdateRootProperties();
        }
    }
    #endregion
}
