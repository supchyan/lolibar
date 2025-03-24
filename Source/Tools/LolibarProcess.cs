using Shell32;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

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
    static string PinnedAppsPath { get; set; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar";

    /// <summary>
    /// Stores initialized applications' containers and paths to their executable target.
    /// </summary>
    static Dictionary<string, LolibarContainer> InitializedApplications { get; set; } = [];
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

                    var TargetPath = lnk.Target.Path;
                    var Arguments = lnk.Arguments;

                    while (TargetPath.EndsWith(".lnk") && !string.IsNullOrEmpty(Arguments))
                    {
                        ShellLinkObject linkedLnk = (ShellLinkObject)ShellInstance.NameSpace(TargetPath).Items().Item().GetLink;
                        TargetPath = linkedLnk.Target.Path;
                        Arguments = linkedLnk.Arguments;
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
    public static void InvokeApplicationByPath (string applicationPath)
    {
        var definedProcesses = Process.GetProcessesByName(GetProcessNameByPath(applicationPath));

        var process = definedProcesses.Length > 0 ? definedProcesses[0] : null;

        if (process != null)
        {
            if (process.MainWindowHandle != 0)
            {
                LolibarExtern.SwitchToThisWindow(process.MainWindowHandle, true);

                // here a problem, when switching apps between multiple desktops,
                // desktops instance of LolibarVirtualDesktop doensn't upadte itself.
                // so here probably we should check whenever SwitchToThisWindow() has finished
                // to update LolibarVirtualDesktop right after that.
            }
            else
            {
                Process.Start(applicationPath);
            }
        }
        else
        {
            Process.Start(applicationPath);
        }
    }
    /// <summary>
    /// Starts a new application instance by specified path.
    /// </summary>
    /// <param name="applicationPath">App execution path.</param>
    public static void StartApplicationByPath(string applicationPath)
    {
        Process.Start(applicationPath);
    }
    static string GetProcessNameByPath(string processPath)
    {
        return processPath.Split("\\").Last().Split(".")[0];
    }
    
    /// <summary>
    /// Generates interactable apps' containers, which are pinned to windows dockbar.
    /// </summary>
    /// <param name="parent">Target parent container.</param>
    public static void AddPinnedAppsToContainer(StackPanel? parent)
    {
        if (parent == null) return;

        // Clear old initialized dict
        InitializedApplications.Clear();

        // Clear all children in parent container:
        parent.Children.Clear();

        foreach (var TargetPath in UserPinnedTargetPaths)
        {
            try
            {
                // Create pinned app container:
                var PinContainer = new LolibarContainer()
                {
                    Name = $"{GetProcessNameByPath(TargetPath)}ApplicationContainer",
                    Icon = LolibarIcon.GetApplicationIcon(TargetPath),
                    Parent = parent,
                    MouseRightButtonUpEvent = (object sender,  MouseButtonEventArgs e) =>
                    { 
                        /* OPEN CONTEXT MENU */  
                    },
                    MouseMiddleButtonUpFunc = () =>  
                    {
                        // Starts a new application instance
                        StartApplicationByPath(TargetPath);
                        return 0;
                    },
                    MouseLeftButtonUpEvent  = (object sender,  MouseButtonEventArgs e) =>
                    {
                        // Invokes application instance / starts a new one,
                        // if specified application isn't running, or running at the background
                        InvokeApplicationByPath(TargetPath);
                    }
                };
                PinContainer.Create();

                // Store a child into a initialized dict
                InitializedApplications.Add(TargetPath, PinContainer);
            }
            catch
            {
                continue;
            }
        }
    }

    //bad
    public static void FetchPinnedAppsContainersState()
    {
        foreach (var application in InitializedApplications)
        {
            var definedProcesses = Process.GetProcessesByName(GetProcessNameByPath(application.Key));
            if (definedProcesses.Length > 0)
            {
                application.Value.HasBackground = definedProcesses[0].MainWindowHandle == LolibarExtern.GetForegroundWindow();
                application.Value.Text          = $"{application.Value.RefText} •";
            }
            else
            {
                application.Value.Text          = application.Value.RefText;
            }
            // baad bad
            application.Value.Update();
        }
    }
}