using Shell32;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

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
    static Dictionary<ShellLinkObject, LolibarContainer> InitializedApps            { get; set; } = new();
    static StackPanel?                          InitializedParent                   { get; set; }
    static int                                  InitializedAppTitleMaxLength        { get; set; }
    static LolibarEnums.AppContainerTitleState  InitializedAppContainerTitleState   { get; set; }

    const string AppActiveSymbol = "•";

    static string PinnedAppsPath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar";

    static List<ShellLinkObject> UserPinnedTargetLinks
    {
        get
        {
            List<ShellLinkObject> TargetLinks = new();

            var ShellInstance       = new Shell();
            Folder UserPinnedFolder = ShellInstance.NameSpace(PinnedAppsPath);

            foreach (FolderItem item in UserPinnedFolder.Items())
            {
                if (item.IsLink)
                {
                    ShellLinkObject lnk = (ShellLinkObject)item.GetLink;
                    // Skip URL type shortcuts, which don't end up with .exe, like P5R: steam://rungameid/1687950
                    if (lnk.Path.EndsWith(".exe"))
                    {
                        TargetLinks.Add(lnk);
                    }
                }
            }

            return TargetLinks;
        }
    }
    /// <summary>
    /// Returns probable process name to be invoked / fetched by Process.GetProcessesByName() for example.
    /// </summary>
    /// <param name="TargetLink"></param>
    /// <returns></returns>
    static string GetProcessName(ShellLinkObject TargetLink)
    {
        // discord issue, may not work for different type of windows shortcuts
        var probableName = TargetLink.Target.Name;
        // the cause of issue is: ...\Discord\Update.exe --processStart Discord.exe,
        // where --processStart calls different binary to execute after update.exe did a trick,
        // so we handle this and try to invoke discord.exe binary instead of update.exe
        if (TargetLink.Arguments.Contains(".exe"))
        {
            probableName = TargetLink.Arguments.Split(" ").Last((e) => e.Contains(".exe")).Replace(".exe", "");
        }
        return probableName;
    }
    /// <summary>
    /// Invokes application's instance by specified path / starts a new one,
    /// if current application isn't running, or running at the background.
    /// </summary>
    public static void InvokeApplicationByPath(ShellLinkObject TargetLink)
    {
        var definedProcesses = Process.GetProcessesByName(GetProcessName(TargetLink));

        var process = definedProcesses.Length > 0 ? definedProcesses[0] : null;

        if (process != null)
        {
            if (process.MainWindowHandle != 0)
            {
                LolibarExtern.SwitchToThisWindow(process.MainWindowHandle, true);
            }
            else
            {
                // invoke existing process
                if (process.MainModule != null)
                {
                    Process.Start(process.MainModule.FileName);
                }
            }
        }
        else
        {
            // start new process using shortcut path and arguments
            process = new Process()
            {
                StartInfo =
                {
                    FileName    = "cmd",
                    Arguments   = $"/C call \"{TargetLink.Path}\" {TargetLink.Arguments}",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            process.Start();
        }

        FetchPinnedAppsLogicDelayed(process);
    }
    /// <summary>
    /// Starts a new application instance by specified path.
    /// </summary>
    /// <param name="applicationPath">App execution path.</param>
    public static void StartApplicationByPath(ShellLinkObject TargetLink)
    {
        new Process()
        {
            StartInfo =
                {
                    FileName    = "cmd",
                    Arguments   = $"/C call \"{TargetLink.Path}\" {TargetLink.Arguments}",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
        }.Start();

        // Fetch apps' containers
        FetchPinnedAppsContainers();
    }
    static string? GetProcessMainWindowTitle(ShellLinkObject TargetLink)
    {
        Process[]? procs = Process.GetProcessesByName(GetProcessName(TargetLink));
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

        foreach(var TargetLink in UserPinnedTargetLinks)
        {
            TargetLink.GetIconLocation(out string pbs);

            // Icon can be embedded into .exe file, so predict it
            if (pbs == "")
            {
                pbs = TargetLink.Target.Path;
            }

            // Create pinned app container:
            var PinContainer        = new LolibarContainer()
            {
                Icon                = LolibarIcon.GetApplicationIcon(pbs),
                Parent              = parent,
                LeftMarginOffset   = 5.0,
                RightMarginOffset  = 5.0,

                MouseRightButtonUp  = (e) =>
                {
                    GenerateContextMenu(TargetLink);
                    return 0;
                },
                MouseMiddleButtonUp = (e) =>  
                {
                    // Starts a new application instance
                    StartApplicationByPath(TargetLink);
                    return 0;
                },
                MouseLeftButtonUp   = (e) =>
                {
                    // Invokes application instance / starts a new one,
                    // if specified application isn't running, or running at the background
                    InvokeApplicationByPath(TargetLink);
                    return 0;
                }
            };
            PinContainer.Create();

            // Store a child into a initialized dict
            InitializedApps.Add(TargetLink, PinContainer);
        }
    }
    static void GenerateContextMenu(ShellLinkObject TargetLink)
    {
        /* OPEN CONTEXT MENU */
        LolibarContextMenu hwndContextMenu = new()
        {
            ChildMargin = 10
        };

        TargetLink.GetIconLocation(out string pbs);

        // Icon can be embedded into .exe file, so predict it
        if (pbs == "")
        {
            pbs = TargetLink.Path;
        }

        // Add interactable menu header
        hwndContextMenu.Children.Add(new()
        {
            Text = TargetLink.Target.Name,
            Icon = LolibarIcon.GetApplicationIcon(pbs),
            MouseLeftButtonUp = (e) =>
            {
                InvokeApplicationByPath(TargetLink);
                hwndContextMenu.Close();
                return 0;
            }
        });

        Process[]? procs = null;

        procs = Process.GetProcessesByName(GetProcessName(TargetLink));

        foreach (var proc in procs)
        {
            if (proc.MainWindowTitle == "") continue;

            hwndContextMenu.Children.Add(new()
            {
                Text = proc.MainWindowTitle.Truncate(24), // only MainWindowHandle has a name, lame ;v;
                Icon = LolibarIcon.GetApplicationIcon(pbs),
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
    /// <summary>
    /// Updates state of pinned apps containers in lolibar + updates virtual desktops if initialized.
    /// Desktops updates, because opnening app window may cause redirect to the different desktop,
    /// so we need to update their state in lolibar as well.
    /// This method awaits before MainWindowHandle actually appeared and then does fetch logic.
    /// This method has 5s await time before fetch forcibly.
    /// </summary>
    /// <param name="proc"></param>
    static async void FetchPinnedAppsLogicDelayed(Process proc)
    {
        var time = DateTime.Now.Ticks + 5000 * 10000; // 5s

        // wait for 5s, before fetch or fetch after window appeared
        while (proc.MainWindowHandle == 0 && time > DateTime.Now.Ticks)
        {
            await Task.Delay(1);
        }

        FetchPinnedAppsContainers();
        LolibarVirtualDesktop.UpdateInitializedDesktops();
    }
    public static void FetchPinnedAppsContainers()
    {
        foreach ((var TargetLink, var Container) in InitializedApps)
        {
            var proc = Process.GetProcessesByName(GetProcessName(TargetLink)).ToList().FirstOrDefault();

            if (proc != null)
            {
                var isActive = proc.MainWindowHandle == LolibarExtern.GetForegroundWindow();

                Container.HasBackground = isActive;

                switch (InitializedAppContainerTitleState)
                {
                    case LolibarEnums.AppContainerTitleState.Always:
                        
                        Container.Text = GetProcessMainWindowTitle(TargetLink)?.Truncate(InitializedAppTitleMaxLength);
                        break;

                    case LolibarEnums.AppContainerTitleState.OnlyActive:

                        Container.Text = isActive ? GetProcessMainWindowTitle(TargetLink)?.Truncate(InitializedAppTitleMaxLength) : AppActiveSymbol;
                        break;

                    case LolibarEnums.AppContainerTitleState.Never:

                        Container.Text = AppActiveSymbol;
                        break;
                }
            }
            else
            {
                switch (InitializedAppContainerTitleState)
                {
                    case LolibarEnums.AppContainerTitleState.Always:
                        
                        Container.Text = TargetLink.Target.Name.Truncate(InitializedAppTitleMaxLength);
                        break;

                    case LolibarEnums.AppContainerTitleState.OnlyActive:
                        
                        Container.Text = null;
                        break;

                    case LolibarEnums.AppContainerTitleState.Never:

                        Container.Text = null;
                        break;
                }
            }

            Container.Update();
        }
    }
}