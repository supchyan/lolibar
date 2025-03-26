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
        // Taskbar is a `window`, part of some default Windows software,
        // that you can manually hide, getting it's `WindowHandle`.
        // What's window handle? Microsoft wiki will explain it better. (C# hWnd Reference)

        // Lolibar has taskbar hide implementation, that you can call right under your mod.

        // Just call this in `Update()`:
        LolibarHelper.HideWindowsTaskbar();

        // Update() call is important, because taskbar window like to show itself automatically,
        // so we want to close it right after :d

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
// LolibarHelper.HideWindowsTaskbar();