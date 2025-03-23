using System.Diagnostics;

namespace LolibarApp.Source.Tools;

// https://stackoverflow.com/questions/2315561/correct-way-in-net-to-switch-the-focus-to-another-application
public class LolibarProcess
{
    /// <summary>
    /// Array of all running applications (which are in the Apps category in Task Manager).
    /// </summary>
    public static Process[] ActiveApplications
    { 
        get
        {
            return Process.GetProcesses().Where(p => p.MainWindowHandle != 0).ToArray();
        } 
    }
    static Dictionary<string, LolibarContainer> ApplicationsContainers { get; set; } = new();
    /// <summary>
    /// Returns array-like current foreground process info. [Id, Name]
    /// </summary>
    /// <returns></returns>
    public class ForegroundProcess
    {
        public static int Id { 
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
    public static void TranslateApplicationStateToContainer(string applicationPath, LolibarContainer applicationContainer)
    {
        ApplicationsContainers.Add(applicationPath, applicationContainer);
    }
    public static void FetchTranslatableApplications()
    {
        foreach (var application in ApplicationsContainers)
        {
            var definedProcesses = Process.GetProcessesByName(ProcessNameByPath(application.Key));
            if (definedProcesses.Length > 0)
            {
                application.Value.HasBackground = definedProcesses[0].MainWindowHandle == LolibarExtern.GetForegroundWindow();
                application.Value.Text = $"{application.Value.RefText} •";
            }
            else
            {
                application.Value.Text = application.Value.RefText;
            }
            application.Value.Update();
        }
    }
    /// <summary>
    /// Try to switch to selected application window, otherwise start the process,
    /// if it's not running yet, or it's in the background. 
    /// </summary>
    /// <param name="applicationPath">App execution path.</param>
    /// <param name="processName">App process name (usually is the same as executable file name).</param>
    public static void InvokeApplicationByPath (string applicationPath)
    {
        var definedProcesses = Process.GetProcessesByName(ProcessNameByPath(applicationPath));

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
    public static Icon? GetAssociatedIcon(string applicationPath)
    {        
        return Icon.ExtractAssociatedIcon(applicationPath);
    }
    static string ProcessNameByPath(string processPath)
    {
        return processPath.Split("\\").Last().Split(".")[0];
    }
}