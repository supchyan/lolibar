using System.Diagnostics;
using System.Reflection;
using System.Windows;
using LolibarApp.Source.Tools;

namespace LolibarApp.Source;

partial class Lolibar : Window
{
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
