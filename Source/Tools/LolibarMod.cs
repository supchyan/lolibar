using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using LolibarApp.Mods;  

namespace LolibarApp.Source.Tools;

public abstract class LolibarMod
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
    /// Lolibar's width property. [ U - Updatable ]
    /// </summary>
    public static double U_BarWidth { get; set; } = 300.0;
    /// <summary>
    /// Lolibar's height property. [ U - Updatable ]
    /// </summary>
    public static double U_BarHeight { get; set; } = 42.0;
    /// <summary>
    /// Lolibar's Left padding property. [ U - Updatable ]
    /// </summary>
    public static double U_BarLeft { get; set; } = 0.0;
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



    public static readonly LolibarContainer BarUserContainer = new() 
    {
        Name = "BarUserContainer",
        Text = LolibarDefaults.UserInfo(),
        MouseLeftButtonUpEvent = LolibarEvents.OpenUserSettingsEvent,
    };
    public static readonly LolibarContainer BarCurProcContainer = new()
    {
        Name = "BarCurProcContainer",
        Text = LolibarDefaults.CurProcInfo(),
        Icon = LolibarDefaults.CurProcIcon(),
        MouseLeftButtonUpEvent = LolibarEvents.OpenTaskManagerEvent,
    };
    public static readonly LolibarContainer BarCpuContainer = new()
    {
        Name = "BarCpuContainer",
        Text = LolibarDefaults.CpuInfo(),
        Icon = LolibarDefaults.CpuIcon()
    };
    public static readonly LolibarContainer BarRamContainer = new()
    {
        Name = "BarRamContainer",
        Text = LolibarDefaults.RamInfo(),
        Icon = LolibarDefaults.RamIcon(),
        MouseRightButtonUpEvent = LolibarEvents.SwapRamInfoEvent
    };
    public static readonly LolibarContainer BarDiskContainer = new()
    {
        Name = "BarDiskContainer",
        Text = LolibarDefaults.DiskInfo(),
        Icon = LolibarDefaults.DiskIcon(),
        MouseRightButtonUpEvent = LolibarEvents.SwapDiskInfoEvent
    };
    public static readonly LolibarContainer BarNetworkContainer = new()
    {
        Name = "BarNetworkContainer",
        MouseRightButtonUpEvent = LolibarEvents.SwapNetworkInfoEvent
    };
    public static readonly LolibarContainer BarTimeContainer = new()
    {
        Name = "BarTimeContainer",
        Text = LolibarDefaults.TimeInfo(),
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
        Text = LolibarDefaults.PowerInfo(),
        Icon = LolibarDefaults.PowerIcon(),
        MouseLeftButtonUpEvent = LolibarEvents.OpenPowerSettingsEvent,
    };



    /// <summary>
    /// Creates User Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateUserContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreateCurProcContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreateCpuContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreateRamContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreateDiskContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreateNetworkContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
    {
        LolibarPerfMon.InitializeNetworkCounters();

        BarNetworkContainer.Text = LolibarDefaults.NetworkInfo();
        BarNetworkContainer.Icon = LolibarDefaults.NetworkIcon();

        BarNetworkContainer.Parent = parent;
        BarNetworkContainer.SeparatorPosition = sepPos;
        BarNetworkContainer.Create();
    }
    /// <summary>
    /// Creates Time Container in specified parent.
    /// </summary>
    /// <param name="parent">Specified parent [ Lolibar.BarLeftContainer as for an example ]</param>
    /// <param name="sepPos">Should container generate separator as well?</param>
    public virtual void CreateTimeContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreateWorkspacesContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
    public virtual void CreatePowerContainer(StackPanel? parent, LolibarEnums.SeparatorPosition? sepPos)
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
        CreateUserContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.None);
        CreateCurProcContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.Both);
        CreateCpuContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.None);
        CreateRamContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.None);
        CreateDiskContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.None);
        CreateNetworkContainer(Lolibar.BarLeftContainer, LolibarEnums.SeparatorPosition.None);

        // --- Center containers ---
        CreateTimeContainer(Lolibar.BarCenterContainer, LolibarEnums.SeparatorPosition.None);

        // --- Right containers ---
        CreateWorkspacesContainer(Lolibar.BarRightContainer, LolibarEnums.SeparatorPosition.Left);
        CreatePowerContainer(Lolibar.BarRightContainer, LolibarEnums.SeparatorPosition.None);

    }
    /// <summary>
    /// Update method. Invokes repeatedly every "UpdateDelay".
    /// </summary>
    public virtual void Update()
    {
        LolibarVirtualDesktop.WorkspaceTabsListener(Config.BarWorkspacesContainer.Border);

        BarUserContainer.Text = LolibarDefaults.UserInfo();
        BarUserContainer.Update();

        BarCurProcContainer.Text = LolibarDefaults.CurProcInfo();
        BarCurProcContainer.Icon = LolibarDefaults.CurProcIcon();
        BarCurProcContainer.Update();

        BarCpuContainer.Text = LolibarDefaults.CpuInfo();
        BarCpuContainer.Icon = LolibarDefaults.CpuIcon();
        BarCpuContainer.Update();

        BarRamContainer.Text = LolibarDefaults.RamInfo();
        BarRamContainer.Icon = LolibarDefaults.RamIcon();
        BarRamContainer.Update();

        BarDiskContainer.Text = LolibarDefaults.DiskInfo();
        BarDiskContainer.Icon = LolibarDefaults.DiskIcon();
        BarDiskContainer.Update();

        BarNetworkContainer.Text = LolibarDefaults.NetworkInfo();
        BarNetworkContainer.Icon = LolibarDefaults.NetworkIcon();
        BarNetworkContainer.Update();

        BarTimeContainer.Text = LolibarDefaults.TimeInfo();
        BarTimeContainer.Update();

        BarPowerContainer.Text = LolibarDefaults.PowerInfo();
        BarPowerContainer.Icon = LolibarDefaults.PowerIcon();
        BarPowerContainer.Update();

        U_BarWidth = LolibarHelper.Inch_ScreenWidth - 2 * BarMargin;
        U_BarHeight = 42.0;

        U_BarLeft = (LolibarHelper.Inch_ScreenWidth - U_BarWidth) / 2;
    }
}