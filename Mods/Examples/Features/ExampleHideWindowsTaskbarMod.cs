using LolibarApp.Source.Tools;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleHideWindowsTaskbarMod : LolibarMod
{
    public override void PreInitialize() { }
    public override void Initialize() { }
    public override void Update()
    {
        // So... What is taskbar in a nutshell?
        // Taskbar is a `window`, which is a part of some default Windows software.
        // You can manually hide it completely, getting it's `WindowHandle`.
        // What's `WindowHandle`? Microsoft wiki will explain it better.
        // (See Windows HWND Reference)

        // Lolibar has a taskbar hide implementation, that you can enable in your mod.

        // Just call this in `Update()`:
        LolibarHelper.HideWindowsTaskbar();

        // Update() call is important, because taskbar window likes to show itself automatically,
        // so we want to close it right after that will :d

        // DON'T WORRY, YOUR OS WILL BE FINE!

        // Taskbar will return in a normal state
        // after small amount of time when lolibar has closed.

        // Potential FAQ:
        //
        // Q:
        // I've hidden taskbar with a method above. But I see an odd transparent bar,
        // witch prevent my applications to draw in fullscreen. What's the problem?
        //
        // A:
        // Before hiding a default windows taskbar, make sure,
        // you enabled `Automatically hide Taskbar` in Windows Taskbar settings.
        // That option won't hide taskbar completely,
        // but this option will remove mentioned "transparent" bar,
        // when lolibar hides it completely. That's, how windows works ha...
    }
}