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
    // Static container's links
    public static StackPanel BarLeftContainer   { get; private set; } = new StackPanel();
    public static StackPanel BarCenterContainer { get; private set; } = new StackPanel();
    public static StackPanel BarRightContainer  { get; private set; } = new StackPanel();

    // Misc
    readonly MouseHook MouseHandler = new();
    readonly Config config = new(); // We use Config's object to invoke Update() and Initialize() methods.
    readonly LolibarVirtualDesktop lolibarVirtualDesktop = new();

    // Screen coordinates calculation properties
    Matrix TransformToDevice                    { get; set; }
    System.Windows.Size ScreenSize              { get; set; }
    public static double StatusBarVisiblePosY   { get; private set; }
    public static double StatusBarHidePosY      { get; private set; }

    // Drawing conditions
    bool IsHidden                               { get; set; }
    bool OldIsHidden                            { get; set; }

    // Null window to prevent lolibar's appearing inside alt+tab menu
    Window nullWin = new()
    {
        Visibility = Visibility.Hidden,
        WindowStyle = WindowStyle.ToolWindow,
        ShowInTaskbar = false,
        Width = 0, Height = 0,
        Top = LolibarHelper.Inch_ScreenWidth // to move it outside the screen 
    };

    // A trigger to prevent different app's job before... it's window actually rendered
    bool IsRendered;

    // System theme check
    [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
    public static extern bool ShouldSystemUseDarkMode();

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

        GenerateTrayMenu();
    }

    void PostInitializeSnapping()
    {
        if (!Config.BarSnapToTop)
        {
            StatusBarVisiblePosY = LolibarHelper.Inch_ScreenHeight - Config.BarHeight - Config.BarMargin;
            StatusBarHidePosY    = LolibarHelper.Inch_ScreenHeight;
        }
        else
        {
            StatusBarVisiblePosY = Config.BarMargin;
            StatusBarHidePosY    = -Config.BarHeight - Config.BarMargin;
        }
    }

    /// <summary>
    /// Reloads resources in Lolibar.xaml.
    /// </summary>
    void ReloadResources()
    {
        // --- Global UI properties ---
        Resources["BarMargin"]                  = Config.BarMargin;
        Resources["BarHeight"]                  = Config.BarHeight;
        Resources["BarWidth"]                   = Config.BarWidth;
        Resources["BarFontSize"]                = Config.BarFontSize;

        Resources["BarColor"]                   = Config.BarColor;
        Resources["BarCornerRadius"]            = Config.BarCornerRadius;
        Resources["BarOpacity"]                 = Config.BarOpacity;
        Resources["BarStrokeThickness"]         = Config.BarStrokeThickness;

        Resources["BarContainersContentColor"]  = Config.BarContainersContentColor;

        Resources["BarContainerMargin"]         = Config.BarContainerMargin;
        Resources["BarContainerInnerMargin"]    = Config.BarContainerInnerMargin;
        Resources["BarContainersContentMargin"] = Config.BarContainersContentMargin;

        Resources["BarWorkspacesMargin"]        = Config.BarWorkspacesMargin;

        Resources["BarAddWorkspaceText"]        = Config.BarAddWorkspaceText;
    }
}
