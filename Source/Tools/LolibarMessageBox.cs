namespace LolibarApp.Source.Tools;

public class LolibarMessageBox
{
    public static DialogResult Show(string text, string title = "Alert", MessageBoxButtons buttons = MessageBoxButtons.OK)
    {
        return MessageBox.Show(text, title, buttons);
    }
}
