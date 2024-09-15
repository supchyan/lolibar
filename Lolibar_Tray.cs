using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Tray [ Notify Icon ]
        NotifyIcon notifyIcon;
        void GenerateTrayMenu()
        {
            notifyIcon = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "lolibar",
                Visible = true,
                ContextMenuStrip = new()
                {
                    Items =
                    {
                        new ToolStripMenuItem("GitHub", null, OnGitHubSelected),
                        new ToolStripMenuItem("Exit", null, OnExitSelected)
                    }
                }
            };
        }
        // Tray Content
        void OnGitHubSelected(object? sender, EventArgs e)
        {
            Process.Start("explorer", "https://github.com/supchyan/lolibar");
        }
        void OnExitSelected(object? sender, EventArgs e)
        {
            foreach(Window window in App.Current.Windows)
            {
                window.Close();
            }
        }
        #endregion
    }
}
