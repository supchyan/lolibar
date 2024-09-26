using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace LolibarApp.Source.Tools;

public abstract class ModLolibar
{
    /// <summary>
    /// When Lolibar should be snapped to top of the screen. (True by default)
    /// </summary>
    public static bool BarSnapToTop { get; set; } = true;

    /// <summary>
    /// Time between update loop iterations in milliseconds. (1000ms by default)
    /// </summary>
    public static int BarUpdateDelay { get; set; } = 1000;

    /// <summary>
    /// Margin between screen edges and the Lolibar.
    /// </summary>
    public static double BarMargin { get; set; } = 8.0;
    /// <summary>
    /// Lolibar's width property.
    /// </summary>
    public static double BarWidth { get; set; } = LolibarHelper.Inch_ScreenWidth - 2 * BarMargin; // Fit to screen width as default
    /// <summary>
    /// Lolibar's height property.
    /// </summary>
    public static double BarHeight { get; set; } = 42.0;
    /// <summary>
    /// Lolibar's opacity property. (From 0 to 1)
    /// </summary>
    public static double BarOpacity { get; set; } = 1.0;
    /// <summary>
    /// Lolibar's font size property.
    /// </summary>
    public static double BarFontSize { get; set; } = 12.0;
    /// <summary>
    /// Lolibar's icon size property.
    /// </summary>
    public static double BarIconSize { get; set; } = 16.0;
    /// <summary>
    /// Separator - that bar between elements in lolibar's containers. This is it's width property.
    /// </summary>
    public static double BarSeparatorWidth { get; set; } = 4.0;
    /// <summary>
    /// Separator - that bar between elements in lolibar's containers. This is it's height property.
    /// </summary>
    public static double BarSeparatorHeight { get; set; } = 16.0;
    /// <summary>
    /// Separator - that bar between elements in lolibar's containers. This is it's border radius property.
    /// </summary>
    public static double BarSeparatorRadius { get; set; } = BarSeparatorWidth / 2.0;

    /// <summary>
    /// Lolibar's stroke size property.
    /// </summary>
    public static Thickness BarStrokeThickness { get; set; } = new Thickness(0.0, 0.0, 0.0, 0.0);
    /// <summary>
    /// Margin between containers in lolibar's containers.
    /// </summary>
    public static Thickness BarContainerMargin { get; set; } = new Thickness(0.0, 0.0, 0.0, 0.0);
    /// <summary>
    /// Gap between Border and it's content inside. ( Padding of the text inside it's box, if it's more accurate )
    /// </summary>
    public static Thickness BarContainerInnerMargin { get; set; } = new Thickness(9.0, 5.0, 9.0, 5.0);
    /// <summary>
    /// Gap between elements inside border. ( Padding between the text and the icon inside their box [ border ], if it's more accurate )
    /// </summary>
    public static Thickness BarContainersContentMargin { get; set; } = new Thickness(5.0, 0.0, 5.0, 0.0);
    /// <summary>
    /// Gap between workspaces.
    /// </summary>
    public static Thickness BarWorkspacesMargin { get; set; } = new Thickness(8.0, 0.0, 8.0, 0.0);


    /// <summary>
    /// Lolibar's containers color. ( That boxes, where sort of content draws )
    /// </summary>
    public static SolidColorBrush BarContainerColor { get; set; } = System.Windows.Media.Brushes.Transparent;
    /// <summary>
    /// Lolibar's main color.
    /// </summary>
    public static SolidColorBrush BarColor { get; set; } = LolibarHelper.SetColor("#2f2f2f");
    /// <summary>
    /// Lolibar's elements color. (Icons / Text / etc.)
    /// </summary>
    public static SolidColorBrush BarContainersContentColor { get; set; } = LolibarHelper.SetColor("#55b456");

    /// <summary>
    /// Lolibar's border radius property.
    /// </summary>
    public static CornerRadius BarCornerRadius { get; set; } = new CornerRadius(6.0);
    /// <summary>
    /// Cornder radius of the elements containers.
    /// </summary>
    public static CornerRadius BarContainersCornerRadius { get; set; } = new CornerRadius(3.0);
    /// <summary>
    /// Content of the User Container.
    /// </summary>
    public static string BarUserText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the Current Process Container.
    /// </summary>
    public static string BarCurProcText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the CPU Container.
    /// </summary>
    public static string BarCpuText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the RAM Container.
    /// </summary>
    public static string BarRamText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the Disk Container.
    /// </summary>
    public static string BarDiskText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the Network Container.
    /// </summary>
    public static string BarNetworkText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the Power Container.
    /// </summary>
    public static string BarPowerText { get; set; } = string.Empty;
    /// <summary>
    /// Content of the Time Container.
    /// </summary>
    public static string BarTimeText { get; set; } = string.Empty;

    /// <summary>
    /// Icon of the Network Container.
    /// </summary>
    public static Geometry BarNetworkIcon { get; set; } = Geometry.Empty;
    /// <summary>
    /// Icon of the CPU Container.
    /// </summary>
    public static Geometry BarCpuIcon { get; set; } = Geometry.Empty;
    /// <summary>
    /// Icon of the Current Process Container.
    /// </summary>
    public static Geometry BarCurProcIcon { get; set; } = Geometry.Empty;
    /// <summary>
    /// Icon of the Disk Container.
    /// </summary>
    public static Geometry BarDiskIcon { get; set; } = Geometry.Empty;
    /// <summary>
    /// Icon of the RAM Container.
    /// </summary>
    public static Geometry BarRamIcon { get; set; } = Geometry.Empty;
    /// <summary>
    /// Icon of the Power Container.
    /// </summary>
    public static Geometry BarPowerIcon { get; set; } = Geometry.Empty;



    public static readonly LolibarContainer BarUserContainer = new() 
    {
        Name = "BarUserContainer",
        Text = BarUserText,
        MouseLeftButtonUpEvent = LolibarEvents.OpenUserSettingsEvent,
    };
    public static readonly LolibarContainer BarCurProcContainer = new()
    {
        Name = "BarCurProcContainer",
        Text = BarCurProcText,
        Icon = BarCurProcIcon,
        MouseLeftButtonUpEvent = LolibarEvents.OpenTaskManagerEvent,
    };
    public static readonly LolibarContainer BarCpuContainer = new()
    {
        Name = "BarCpuContainer",
        Text = BarCpuText,
        Icon = BarCpuIcon
    };
    public static readonly LolibarContainer BarRamContainer = new()
    {
        Name = "BarRamContainer",
        Text = BarRamText,
        Icon = BarRamIcon,
        MouseRightButtonUpEvent = LolibarEvents.SwapRamInfoEvent
    };
    public static readonly LolibarContainer BarDiskContainer = new()
    {
        Name = "BarDiskContainer",
        Text = BarDiskText,
        Icon = BarDiskIcon,
        MouseRightButtonUpEvent = LolibarEvents.SwapDiskInfoEvent
    };
    public static readonly LolibarContainer BarNetworkContainer = new()
    {
        Name = "BarNetworkContainer",
        Text = BarNetworkText,
        Icon = BarNetworkIcon,
        MouseRightButtonUpEvent = LolibarEvents.SwapNetworkInfoEvent
    };
    public static readonly LolibarContainer BarTimeContainer = new()
    {
        Name = "BarTimeContainer",
        Text = BarTimeText,
        MouseLeftButtonUpEvent = LolibarEvents.OpenTimeSettingsEvent
    };
    public static readonly LolibarContainer BarWorkspacesContainer = new()
    {
        Name = "BarWorkspacesContainer",
        UseWorkspaceSwapEvents = true,
    };
    public static readonly LolibarContainer BarPowerContainer = new()
    {
        Name = "BarPowerContainer",
        Icon = BarPowerIcon,
        Text = BarPowerText,
        MouseLeftButtonUpEvent = LolibarEvents.OpenPowerSettingsEvent,
    };



    /// <summary>
    /// Creates User Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateUserContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarUserContainer.Parent = parent;
        BarUserContainer.SeparatorPosition = sepPos;
        BarUserContainer.Create();
    }
    /// <summary>
    /// Creates Current Process Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateCurProcContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarCurProcContainer.Parent = parent;
        BarCurProcContainer.SeparatorPosition = sepPos;
        BarCurProcContainer.Create();
    }
    /// <summary>
    /// Creates CPU Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateCpuContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarCpuContainer.Parent = parent;
        BarCpuContainer.SeparatorPosition = sepPos;
        BarCpuContainer.Create();
    }
    /// <summary>
    /// Creates RAM Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateRamContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarRamContainer.Parent = parent;
        BarRamContainer.SeparatorPosition = sepPos;
        BarRamContainer.Create();
    }
    /// <summary>
    /// Creates Disk Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateDiskContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarDiskContainer.Parent = parent;
        BarDiskContainer.SeparatorPosition = sepPos;
        BarDiskContainer.Create();
    }
    /// <summary>
    /// Creates Network Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateNetworkContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarNetworkContainer.Parent = parent;
        BarNetworkContainer.SeparatorPosition = sepPos;
        BarNetworkContainer.Create();
    }
    /// <summary>
    /// Creates Time Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateTimeContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarTimeContainer.Parent = parent;
        BarTimeContainer.SeparatorPosition = sepPos;
        BarTimeContainer.Create();
    }
    /// <summary>
    /// Creates Workspaces Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateWorkspacesContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarWorkspacesContainer.Parent = parent;
        BarWorkspacesContainer.SeparatorPosition = sepPos;
        BarWorkspacesContainer.Create();
    }
    /// <summary>
    /// Creates Power Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreatePowerContainer(StackPanel? parent = null, LolibarEnums.SeparatorPosition? sepPos = null)
    {
        BarPowerContainer.Parent = parent;
        BarPowerContainer.SeparatorPosition = sepPos;
        BarPowerContainer.Create();
    }
    /// <summary>
    /// Initialization method. Invokes once at lolibar's launch.
    /// </summary>
    public virtual void Initialize()
    {
        // --- Left containers ---
        CreateUserContainer(Lolibar.BarLeftContainer);
        CreateCurProcContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.Both);
        CreateCpuContainer(Lolibar.BarLeftContainer);
        CreateRamContainer(Lolibar.BarLeftContainer);
        CreateDiskContainer(Lolibar.BarLeftContainer);
        CreateNetworkContainer(Lolibar.BarLeftContainer);

        // --- Center containers ---
        CreateTimeContainer(Lolibar.BarCenterContainer);

        // --- Right containers ---
        CreateWorkspacesContainer(Lolibar.BarRightContainer, LolibarEnums.SeparatorPosition.Left);
        CreatePowerContainer(Lolibar.BarRightContainer);

    }
    /// <summary>
    /// Update method. Invokes repeatedly every "UpdateDelay".
    /// </summary>
    public virtual void Update()
    {
        // --- Left containers ---
        BarUserText          = LolibarDefaults.UserInfo()            ?? string.Empty;

        BarCurProcText       = LolibarDefaults.CurProcInfo()         ?? string.Empty;
        BarCurProcIcon       = LolibarDefaults.CurProcIcon()         ?? Geometry.Empty;

        // --- Center containers ---
        BarCpuText           = LolibarDefaults.CpuInfo()             ?? string.Empty;
        BarCpuIcon           = LolibarDefaults.CpuIcon()             ?? Geometry.Empty;

        BarRamText           = LolibarDefaults.RamInfo()             ?? string.Empty;
        BarRamIcon           = LolibarDefaults.RamIcon()             ?? Geometry.Empty;

        BarDiskText          = LolibarDefaults.DiskInfo()            ?? string.Empty;
        BarDiskIcon          = LolibarDefaults.DiskIcon()            ?? Geometry.Empty;

        BarNetworkText       = LolibarDefaults.NetworkInfo()         ?? string.Empty;
        BarNetworkIcon       = LolibarDefaults.NetworkIcon()         ?? Geometry.Empty;

        // --- Right containers ---
        BarPowerText         = LolibarDefaults.PowerInfo()           ?? string.Empty;
        BarPowerIcon         = LolibarDefaults.PowerIcon()           ?? Geometry.Empty;

        BarTimeText          = LolibarDefaults.TimeInfo()            ?? string.Empty;
    }
}
