namespace LolibarApp.Source.Tools;

public class LolibarMessageBox
{
    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
    {
        return MessageBox.Show(text, caption, buttons);
    }
}
