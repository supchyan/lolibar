using Shell32;
using System.Diagnostics;
using System.Windows.Controls;

namespace LolibarApp.Source.Tools;

// https://stackoverflow.com/questions/2315561/correct-way-in-net-to-switch-the-focus-to-another-application
// https://learn.microsoft.com/en-us/answers/questions/1297602/how-to-get-the-real-target-of-a-shortcut
public class LolibarProcess
{
    /// <summary>
    /// Returns array-like current foreground process info. [Id, Name]
    /// </summary>
    /// <returns></returns>
    public class ForegroundProcess
    {
        public static int Id
        {
            get
            {
                LolibarExtern.GetWindowThreadProcessId(LolibarExtern.GetForegroundWindow(), out uint pid);
                return (int)pid;
            }
        }
        public static string Name
        {
            get
            {
                LolibarExtern.GetWindowThreadProcessId(LolibarExtern.GetForegroundWindow(), out uint pid);
                return Process.GetProcessById((int)pid).ProcessName;
            }
        }
    }
    /// <summary>
    /// Stores initialized applications' containers and paths to their executable target.
    /// </summary>
    static Dictionary<string, LolibarContainer> InitializedApps                     { get; set; } = [];
    static StackPanel?                          InitializedParent                   { get; set; }
    static int                                  InitializedAppTitleMaxLength        { get; set; }
    static LolibarEnums.AppContainerTitleState  InitializedAppContainerTitleState   { get; set; }

    const string AppActiveSymbol = "•";

    static string PinnedAppsPath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar";

    static List<string> UserPinnedTargetPaths
    {
        get
        {
            List<string> TargetPathArr = [];

            var ShellInstance       = new Shell();
            Folder UserPinnedFolder = ShellInstance.NameSpace(PinnedAppsPath);

            foreach (FolderItem item in UserPinnedFolder.Items())
            {
                if (item.IsLink)
                {
                    ShellLinkObject lnk = (ShellLinkObject)item.GetLink;

                    var TargetPath  = lnk.Target.Path;
                    var Arguments   = lnk.Arguments;

                    while (TargetPath.EndsWith(".lnk") && !string.IsNullOrEmpty(Arguments))
                    {
                        ShellLinkObject linkedLnk = (ShellLinkObject)ShellInstance.NameSpace(TargetPath).Items().Item().GetLink;
                        TargetPath  = linkedLnk.Target.Path;
                        Arguments   = linkedLnk.Arguments;
                    }
                    TargetPathArr.Add(TargetPath);
                }
            }

            return TargetPathArr;
        }
    }
    /// <summary>
    /// Invokes application's instance by specified path / starts a new one,
    /// if current application isn't running, or running at the background.
    /// </summary>
    /// <param name="applicationPath">App execution path.</param>
    public static void InvokeApplicationByPath(string applicationPath)
    {
        var definedProcesses = Process.GetProcessesByName(GetProcessNameByPath(applicationPath));

        var process = definedProcesses.Length > 0 ? definedProcesses[0] : null;

        if (process != null)
        {
            if (process.MainWindowHandle != 0)
            {
                LolibarExtern.SwitchToThisWindow(process.MainWindowHandle, true);
            }
            else
            {
                process = Process.Start(applicationPath);
            }
        }
        else
        {
            process = Process.Start(applicationPath);
        }

        // TODO:
        // This should be called right after window actually started and MainWindow handled.
        // At this moment, it can be occured before ``MainWindowHandle` became reachable.
        FetchPinnedAppsContainers();

        // TODO:
        // This should be called right after window actually switched.
        // At this moment, it can be occured before `SwitchToThisWindow()` finished it's job.
        // Update workspaces state
        LolibarVirtualDesktop.UpdateInitializedDesktops();
    }
    /// <summary>
    /// Starts a new application instance by specified path.
    /// </summary>
    /// <param name="applicationPath">App execution path.</param>
    public static void StartApplicationByPath(string applicationPath)
    {
        Process.Start(applicationPath);

        // Fetch apps' containers
        FetchPinnedAppsContainers();
    }
    static string GetProcessNameByPath(string processPath)
    {
        return processPath.Split("\\").Last().Split(".")[0];
    }
    static string? GetProcessMainWindowTitleByPath(string processPath)
    {
        Process[]? procs = Process.GetProcessesByName(GetProcessNameByPath(processPath));
        return procs[0]?.MainWindowTitle;
    }

    /// <summary>
    /// Generates interactable apps' containers, which are pinned to windows dockbar.
    /// </summary>
    /// <param name="parent">Target parent container.</param>
    /// <param name="appContainerTitleState">Whenever apps' titles have to be drawn.</param>
    /// <param name="appTitleMaxLength">
    /// Each app has a name, isn't it?
    /// This determines, how long app's name should be drawn in the container.
    /// </param>
    public static void AddPinnedAppsToContainer(StackPanel? parent, LolibarEnums.AppContainerTitleState appContainerTitleState = LolibarEnums.AppContainerTitleState.Never, int appTitleMaxLength = 16)
    {
        if (parent == null) return;

        // Clear old initialized dict
        InitializedApps.Clear();

        // Clear all children in parent container:
        parent.Children.Clear();

        InitializedParent                   = parent;
        InitializedAppTitleMaxLength        = appTitleMaxLength;
        InitializedAppContainerTitleState   = appContainerTitleState;

        foreach (var TargetPath in UserPinnedTargetPaths)
        {
            try
            {
                // Create pinned app container:
                var PinContainer        = new LolibarContainer()
                {
                    Icon                = LolibarIcon.GetApplicationIcon(TargetPath),
                    Parent              = parent,
                    LeftMarginOffset   = 5.0,
                    RightMarginOffset  = 5.0,

                    MouseRightButtonUp  = (e) =>
                    {
                        GenerateContextMenu(TargetPath);
                        return 0;
                    },
                    MouseMiddleButtonUp = (e) =>  
                    {
                        // Starts a new application instance
                        StartApplicationByPath(TargetPath);
                        return 0;
                    },
                    MouseLeftButtonUp   = (e) =>
                    {
                        // Invokes application instance / starts a new one,
                        // if specified application isn't running, or running at the background
                        InvokeApplicationByPath(TargetPath);
                        return 0;
                    }
                };
                PinContainer.Create();

                // Store a child into a initialized dict
                InitializedApps.Add(TargetPath, PinContainer);
            }
            catch
            {
                continue;
            }
        }
    }
    static void GenerateContextMenu(string TargetPath)
    {
        /* OPEN CONTEXT MENU */
        LolibarContextMenu hwndContextMenu = new()
        {
            ChildMargin = 10
        };

        hwndContextMenu.Children.Add(new()
        {
            Text = GetProcessNameByPath(TargetPath),
            Icon = LolibarIcon.GetApplicationIcon(TargetPath),
            MouseLeftButtonUp = (e) =>
            {
                InvokeApplicationByPath(TargetPath);
                hwndContextMenu.Close();
                return 0;
            }
        });

        Process[]? procs = null;

        procs = Process.GetProcessesByName(GetProcessNameByPath(TargetPath));

        foreach (var proc in procs)
        {
            if (proc.MainWindowTitle == "") continue;

            hwndContextMenu.Children.Add(new()
            {
                Text = proc.MainWindowTitle.Truncate(24), // only MainWindowHandle has a name, lame ;v;
                Icon = LolibarIcon.GetApplicationIcon(TargetPath),
                HasBackground = true,
                MouseLeftButtonUp = (e) =>
                {
                    LolibarExtern.SwitchToThisWindow(proc.MainWindowHandle, true);
                    hwndContextMenu.Close();
                    return 0;
                }
            });
        }
        hwndContextMenu.Create();
    }
    public static void UpdateInitializedPinnedApps()
    {
        AddPinnedAppsToContainer(InitializedParent, InitializedAppContainerTitleState, InitializedAppTitleMaxLength);
    }
    public static void FetchPinnedAppsContainers()
    {
        foreach (var application in InitializedApps)
        {
            var proc = Process.GetProcessesByName(GetProcessNameByPath(application.Key)).ToList().FirstOrDefault();

            if (proc != null)
            {
                var isActive = proc.MainWindowHandle == LolibarExtern.GetForegroundWindow();

                application.Value.HasBackground = isActive;

                switch (InitializedAppContainerTitleState)
                {
                    case LolibarEnums.AppContainerTitleState.Always:
                        
                        application.Value.Text = GetProcessMainWindowTitleByPath(application.Key)?.Truncate(InitializedAppTitleMaxLength);
                        break;

                    case LolibarEnums.AppContainerTitleState.OnlyActive:

                        application.Value.Text = isActive ? GetProcessMainWindowTitleByPath(application.Key)?.Truncate(InitializedAppTitleMaxLength) : AppActiveSymbol;
                        break;

                    case LolibarEnums.AppContainerTitleState.Never:

                        application.Value.Text = AppActiveSymbol;
                        break;
                }
            }
            else
            {
                switch (InitializedAppContainerTitleState)
                {
                    case LolibarEnums.AppContainerTitleState.Always:
                        
                        application.Value.Text = GetProcessNameByPath(application.Key).Truncate(InitializedAppTitleMaxLength);
                        break;

                    case LolibarEnums.AppContainerTitleState.OnlyActive:
                        
                        application.Value.Text = null;
                        break;

                    case LolibarEnums.AppContainerTitleState.Never:

                        application.Value.Text = null;
                        break;
                }
            }

            application.Value.Update();
        }
    }
}