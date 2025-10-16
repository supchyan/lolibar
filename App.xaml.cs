using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Forms;

namespace LolibarApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    App()
    {
        AppDomain.CurrentDomain.UnhandledException  += CurrentDomain_UnhandledException;
    }

    void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception exception = (Exception)e.ExceptionObject;

        // Idk, strange bug with async lolibar stuff, i don't care to fix it now.
        // Just for note: Threading.Dispatcher causes this.
        if (exception.Source != null && exception.Source.Contains("WindowsBase"))
        {
            return;
        }

        DialogResult result = LolibarMessageBox.Show
                (
                    text: $"{exception.Source}.exe caught an error.\n\nError: {exception.Message}\n{exception.InnerException}\n{exception.StackTrace}\n\nDo you want to restart the application?",
                    title: "Lolibar Alert",
                    MessageBoxButtons.YesNo
                );

        if (result == DialogResult.Yes)
        {
            LolibarHelper.RestartApplicationGently();
        }
        if (result == DialogResult.No)
        {
            //LolibarHelper.CloseApplicationGently();
            Process.GetCurrentProcess().Kill();
        }
    }
}
