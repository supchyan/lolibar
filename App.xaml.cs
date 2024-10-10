namespace LolibarApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    App()
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception exception = (Exception)e.ExceptionObject;
        System.Windows.MessageBox.Show($"{exception.Source}.exe caught an error.\n\nError: {exception.Message}\n{exception.InnerException}\n{exception.StackTrace}");
    }
}
