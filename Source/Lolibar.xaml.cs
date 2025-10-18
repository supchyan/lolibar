using Ikst.MouseHook;
using IWshRuntimeLibrary;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

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

    // --- Lolibar drop shadow effect ---
    static DropShadowEffect DropShadow { get; set; } = new();

    /// <summary>
    /// Returns screen mouse position, when mouse is over lolibar.
    /// </summary>
    public static System.Windows.Point InWindowScreenMousePosition { get; private set; }


    // --- Drawing triggers ---
    bool IsHidden       { get; set; }
    bool OldIsHidden    { get; set; }

    /// <summary>
    /// DANGEROUS PARAMETER! RECOMMENDED TO AVOID IT! 
    /// Add value if Lolibar need to be shown regardless of default conditions. 
    /// Empty list means, nothing prevents lolibar from hiding under default conditions.
    /// </summary>
    public static List<byte> HideInterruptions { get; set; } = new();

    public static double GetStatusBarVisiblePosY(Window wnd)
    {
        return !LolibarMod.BarSnapToTop ? Inch_Screen.Y - wnd.Height : 0;
    }
    public static double GetStatusBarHidePosY(Window wnd)
    {
        return !LolibarMod.BarSnapToTop ? Inch_Screen.Y : -wnd.Height;
    }

    /// <summary>
    /// Null window to prevent other windows appearing inside Alt+Tab UI
    /// </summary>
    static readonly Window NullWindow = new()
    {
        Visibility          = Visibility.Hidden,
        WindowStyle         = WindowStyle.ToolWindow,
        ShowInTaskbar       = false,
        Width               = 0,
        Height              = 0,
        Left                = -100 // to open the null_window outside of the screen 
    };
    /// <summary>
    /// Prevents some window being drawn in Alt+Tab UI
    /// </summary>
    /// <param name="wnd"></param>
    public static void HideFromAltTab(Window wnd)
    {
        wnd.Owner = GetWindow(NullWindow);
    }

    // --- Cursor params ---
    public static bool MouseLeftDown    { get; private set; }
    public static bool MouseRightDown   { get; private set; }

    /// <summary>
    /// Cursor screen position in pixels.
    /// </summary>
    public static Vector2   CursorPosition          { get; private set; }
    static Vector2          OldCursorPosition       { get; set; }
    static float            CursorVelocity          { get; set; }
    static DateTime         OldTime                 { get; set; }

    // --- LolibarVirtualDesktop update trigger on lolibar's opening ---
    static bool ShouldManuallyUpdateDynamicLibs { get; set; }
    static bool IsClosing { get; set; }

    public Lolibar()
    {
        InitializeComponent();

        Closed += Lolibar_Closed;
        Closing += Lolibar_Closing;

        // Show null window
        NullWindow.Show();

        // --- Moves lolibar into the null window ---
        HideFromAltTab(this);

        // --- Writes main containers into accessable types ---
        BarCenterContainer  = _BarCenterContainer;
        BarLeftContainer    = _BarLeftContainer;
        BarRightContainer   = _BarRightContainer;

        // ---

        InitializeCycle();
        VanillaTaskBarLurker();
        UpdateCycle();

        CheeseUpdateCycle();  // For dynamic libs Update

        // Should be below Initialize and Update calls, because it has Resources[] dependency
        MouseHandler.MouseMove      += MouseHandler_MouseMove;

        MouseHandler.LeftButtonUp   += MouseHandler_LeftButtonUp;
        MouseHandler.LeftButtonDown += MouseHandler_LeftButtonDown;

        MouseHandler.RightButtonUp      += MouseHandler_RightButtonUp;
        MouseHandler.RightButtonDown    += MouseHandler_RightButtonDown;

        MouseHandler.Start();

        LolibarAudio.Start();

        // Create .lolibar folder in user directory + Set environment var. according to user
        CreateLolibarCliEnvironment();

        SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
    }

    void Lolibar_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        IsClosing = true;
    }

    void MouseHandler_RightButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        MouseRightDown = false;
    }
    void MouseHandler_RightButtonUp(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        MouseRightDown = true;
    }

    void MouseHandler_LeftButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        MouseLeftDown = true;
    }

    void MouseHandler_LeftButtonUp(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        MouseLeftDown = false;
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

    /// <summary>
    /// Updates root properties.
    /// </summary>
    void PostUpdateRootProperties()
    {
        Width               = Inch_Screen.X;
        Height              = LolibarMod.BarHeight;

        FontSize            = LolibarMod.BarFontSize;

        DropShadow.ShadowDepth    = 0;
        DropShadow.Opacity        = 1;
        DropShadow.BlurRadius     = LolibarMod.BarShadowBlurRadius;
        DropShadow.Color          = LolibarMod.BarShadowColor.Color;

        Bar.Background      = LolibarMod.BarColor;
        Bar.BorderThickness = LolibarMod.BarStrokeThickness;
        Bar.BorderBrush     = LolibarMod.BarStrokeColor;
        Bar.CornerRadius    = LolibarMod.BarCornerRadius;

        Bar.Width = LolibarMod.BarWidth == -1 ? // -1 for fit to screen width
            Inch_Screen.X - LolibarMod.BarMargin.Left - LolibarMod.BarMargin.Right :
            LolibarMod.BarWidth - LolibarMod.BarMargin.Left - LolibarMod.BarMargin.Right;

        Bar.Height = LolibarMod.BarHeight - LolibarMod.BarMargin.Top - LolibarMod.BarMargin.Bottom;

        if (LolibarMod.BarScreenPosition == LolibarEnums.BarScreenPosition.Left)
        {
            Bar.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Left = LolibarMod.BarMargin.Left; // margins fix
        }

        if (LolibarMod.BarScreenPosition == LolibarEnums.BarScreenPosition.Center)
        {
            Bar.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        }

        if (LolibarMod.BarScreenPosition == LolibarEnums.BarScreenPosition.Right)
        {
            Bar.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            Left = -LolibarMod.BarMargin.Left; // margins fix
        }

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

        // Add shadow effect to main bar
        Bar.Effect = DropShadow;

        // On initialize hook this parameter is -100 to prevent null window from spawning in visible screen area.
        // Remove the old -100 value.
        Left = 0;

        // Move lolibar to proper hidden position
        Top = GetStatusBarHidePosY(this);
    }
    async void UpdateCycle()
    {
        while (true)
        {
            await Task.Delay(LolibarMod.BarUpdateDelay);

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
                LolibarProcess.FetchPinnedAppsContainers();
                ShouldManuallyUpdateDynamicLibs = false;
            }
        }
    }
    /// <summary>
    /// Hides vanilla windows taskbar if BarHideVanillaTaskBar flag enabled
    /// </summary>
    async static void VanillaTaskBarLurker()
    {
        if (!LolibarMod.BarHideVanillaTaskBar) return;

        // Primary monitor
        var hwnd = LolibarExtern.FindWindow("Shell_TrayWnd", "");
        // Secondary monitor(s)
        var hwndSecondary = LolibarExtern.FindWindow("Shell_SecondaryTrayWnd", "");
        var startButtonHandle = LolibarExtern.FindWindowEx(LolibarExtern.GetDesktopWindow(), 0, "button", 0);

        while (!IsClosing)
        {
            LolibarExtern.ShowWindow(hwnd, LolibarEnums.WindowStateEnum.Hide);
            LolibarExtern.ShowWindow(hwndSecondary, LolibarEnums.WindowStateEnum.Hide);
            LolibarExtern.ShowWindow(startButtonHandle, LolibarEnums.WindowStateEnum.Hide);

            await Task.Delay(1);
        }

        LolibarExtern.ShowWindow(hwnd, LolibarEnums.WindowStateEnum.Show);
        LolibarExtern.ShowWindow(hwndSecondary, LolibarEnums.WindowStateEnum.Show);
        LolibarExtern.ShowWindow(startButtonHandle, LolibarEnums.WindowStateEnum.Show);
    }
    #endregion

    #region Events
    void MouseHandler_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
        try
        {
            InWindowScreenMousePosition = PointToScreen(Mouse.GetPosition(this));

            bool ShowTrigger, HideTrigger, IsCursorInDesktopsMenuPosition;

            bool IsMouseMinY = mouseStruct.pt.y <= 0;
            bool IsMouseMaxY = mouseStruct.pt.y >= ScreenSize.Y;

            bool IsMouseMinX = mouseStruct.pt.x <= 0;
            bool IsMouseMaxX = mouseStruct.pt.x >= ScreenSize.X;

            var BarVisibleY = 2 * Bar.ActualHeight + Bar.Margin.Top + Bar.Margin.Bottom;

            if (!LolibarMod.BarSnapToTop)
            {
                ShowTrigger = ((IsMouseMinX || IsMouseMaxX) && IsMouseMaxY) || HideInterruptions.Count > 0;
                HideTrigger = mouseStruct.pt.y < ScreenSize.Y - BarVisibleY && HideInterruptions.Count == 0;
            }
            else
            {
                ShowTrigger = ((IsMouseMinX || IsMouseMaxX) && IsMouseMinY) || HideInterruptions.Count > 0;
                HideTrigger = mouseStruct.pt.y > BarVisibleY && HideInterruptions.Count == 0;
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
                    LolibarAnimator.Core.ShowLolibar(this);
                    ShouldManuallyUpdateDynamicLibs = true;
                }
                else
                {
                    LolibarAnimator.Core.HideLolibar(this);
                }
                OldIsHidden = IsHidden;
            }

            CursorPosition = new Vector2(mouseStruct.pt.x, mouseStruct.pt.y);

            // Logic for opening all apps and desktops view (WIN + TAB)
            if (LolibarMod.BarCornersInvokesDesktopsMenu)
            {
                IsCursorInDesktopsMenuPosition =
                        (LolibarMod.BarTargetCorner == LolibarEnums.BarTargetCorner.Left ? IsMouseMinX : IsMouseMaxX) &&
                        (!LolibarMod.BarSnapToTop ? IsMouseMaxY : IsMouseMinY);

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
        catch
        {
            // Can't reach mouse handler server...
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
    readonly static ToolStripMenuItem AutorunTrayItem   = new(AutorunTrayItemContent(), null, OnAutorunSelected);
    readonly static ToolStripMenuItem RestartTrayItem   = new("Restart", null, OnRestartSelected);
    readonly static ToolStripMenuItem GitHubTrayItem    = new("GitHub", null, OnGitHubSelected);
    readonly static ToolStripMenuItem CloseTrayItem     = new("Close Lolibar", null, OnExitSelected);

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
    public static bool IsAutorunPathExist()
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
    static void OnAutorunSelected(object? sender, EventArgs e)
    {
        new Process()
        {
            StartInfo =
            {
                FileName = @"Autorun\autorun.exe",
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            },
        }.Start();

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

    #region Overrides
    // Protects lolibar from being closed by some keybindings
    protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
    {
        if (
            (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space) ||
            (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.F4)
            )
        {
            e.Handled = true;
        }
        else
        {
            base.OnKeyDown(e);
        }
    }
    #endregion

    #region Cli
    /// <summary>
    /// Creates .lolibar folder in the $User directory + Sets environment var. (Path) according to this user.
    /// Grants access to lolibar via cli, using `lolibar`.
    /// </summary>
    static void CreateLolibarCliEnvironment()
    {
        var execPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var localLolibarPath = $"C:\\Users\\{LolibarStats.UserInfo}\\.lolibar";
        var cmdFileRefPath = $"{execPath}\\Scripts\\lolibar.cmd";
        var cmdFilePath = $"C:\\Users\\{LolibarStats.UserInfo}\\.lolibar\\lolibar.cmd";
        var lnkFilePath = $"C:\\Users\\{LolibarStats.UserInfo}\\.lolibar\\lolibar.lnk";
        var enviromentValue = System.Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);

        // Create `.lolibar` folder in user directory if not exist
        if (!Directory.Exists(localLolibarPath))
        {
            Directory.CreateDirectory(localLolibarPath);
        }

        // Remove old .cmd file 
        try { System.IO.File.Delete(cmdFilePath); }
        catch { /* File is not exist */ }
        // Copy a new one
        System.IO.File.Copy(cmdFileRefPath, cmdFilePath);

        // Remove old .lnk file 
        try { System.IO.File.Delete(lnkFilePath); }
        catch { /* File is not exist */ }
        
        // Create new .lnk file 
        WshShell shell = new();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(lnkFilePath);

        shortcut.TargetPath = $"{execPath}\\lolibar.exe";
        shortcut.IconLocation = $"{execPath}\\lolibar.exe";

        shortcut.Save();

        // Add `.lolibar` folder to PATH
        if (enviromentValue != null && !enviromentValue.Contains(localLolibarPath))
        {
            System.Environment.SetEnvironmentVariable("Path", $"{enviromentValue}{localLolibarPath};", EnvironmentVariableTarget.User);
        }
    }
    #endregion
}
