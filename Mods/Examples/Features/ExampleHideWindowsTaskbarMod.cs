using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleHideWindowsTaskbarMod : LolibarMod
{
    public override void PreInitialize() 
    {
        // IMPORTANT!
        // This trick works bad on Windows 11 24H2 patch.
        // So if you face visual glitches with trick below,
        // I can recommend you to find third-party software,
        // which modifies explorer.exe and "softlocks" vanilla taskbar.
        // Lolibar doesn't do this due OS stability issues.
        // ...
        // I can suggest to use Windhawk software with `Windows 11 Taskbar Styler` mod.
        // In mod settings you can modify visibility of vanilla taskbar's root node like:
        // Target: Windows.UI.Xaml.Controls.Grid
        // Styles: Visibility=Collapsed
        // It should hide vanilla taskbar permanently.

        // So... What is taskbar in a nutshell?
        // Taskbar is a `window`, which is a part of some default Windows software.
        // You can manually hide it completely, getting it's `WindowHandle`.
        // What's `WindowHandle`? Microsoft wiki will explain it better.
        // (See Windows HWND Reference)

        // But you can do just this:
        BarHideVanillaTaskBar = true;

        // Vanilla taskbar will return in a normal state
        // on first update right after lolibar close.

        // Potential FAQ:
        //
        // Q:
        // I've hidden taskbar with a flag above. But I see an odd transparent bar,
        // witch prevent my applications to draw in fullscreen. What's the problem?
        //
        // A:
        // Before hiding a default windows taskbar, make sure,
        // you enabled `Automatically hide Taskbar` in Windows Taskbar settings.
        // `Automatically hide Taskbar` option won't hide taskbar completely,
        // but will remove mentioned "transparent" bar,
        // when lolibar hides it completely. That's, how windows works ha...
    }
    public override void Initialize() { }
    public override void Update() { }
}
