using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Tray [ Notify Icon ]
        void GenerateTrayMenu()
        {
            new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "lolibar",
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
            System.Windows.Forms.Application.Restart();
            CloseApplicationGently();
        }
        void OnGitHubSelected(object? sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/supchyan/lolibar");
        }
        void OnExitSelected(object? sender, EventArgs e)
        {
            CloseApplicationGently();
        }
        #endregion
    }
}
