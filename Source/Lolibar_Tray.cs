using System.Diagnostics;
using System.Reflection;
using System.Windows;
using LolibarApp.Source.Tools;

namespace LolibarApp.Source;

partial class Lolibar : Window
{
    #region Tray [ Notify Icon ]
    NotifyIcon? trayIcon;
    void GenerateTrayMenu()
    {
        trayIcon = new NotifyIcon
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
    }
    // Tray Content
    void OnRestartSelected(object? sender, EventArgs e)
    {
        LolibarHelper.RestartApplicationGently();
    }
    void OnGitHubSelected(object? sender, EventArgs e)
    {
        Process.Start("explorer", "https://github.com/supchyan/lolibar");
    }
    void OnExitSelected(object? sender, EventArgs e)
    {
        LolibarHelper.CloseApplicationGently();
    }
    #endregion
}
