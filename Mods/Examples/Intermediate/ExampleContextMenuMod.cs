using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Input;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;
class ExampleContextMenuMod : LolibarMod
{
    // Define context menu children containers:
    LolibarContainer updChild = new();
    LolibarContainer child = new();
    public override void PreInitialize() { }
    public override void Initialize()
    {
        // Create some container to open context menu:
        LolibarContainer contextMenuContainer = new()
        {
            Text                = "Right click me",
            HasBackground       = true,
            Parent              = Lolibar.BarCenterContainer,
            MouseRightButtonUp  = OpenContextMenu // <- set right click event
        };
        contextMenuContainer.Create();
    }
    public override void Update() 
    {
        // Let's update this container's text property to show current time
        updChild.Text = $"{DateTime.Now}";
        updChild.Update();
    }
    private int OpenContextMenu(MouseButtonEventArgs args)
    {
        // You MUST redefine containers in context menu
        // each time it's being generated.
        updChild = new() 
        { 
            Text = "I will be overwritten ;ccccc;"
            // We don't need a `Parent` property here,
            // because context menu handles it's children manually
        };

        // container.Create(); <- DON'T DO THIS FOR CONTEXT MENU CHILDREN,
        // USE `Initialize()` INSTEAD:
        updChild.Initialize();

        // And this one as well:
        child = new()
        {
            // We don't need a `Parent` property here.
            Text = "Context menu child",
            HasBackground = true,
            MouseLeftButtonUp = OpenCmd
        };
        // Initialize this container as well:
        child.Initialize();

        // Create context menu object
        LolibarContextMenu contextMenu = new();

        // Add children in `Children` property:
        contextMenu.Children.Add(updChild);
        contextMenu.Children.Add(child);

        // Create context menu like:
        contextMenu.Create();

        return 0;
    }

    private int OpenCmd(MouseButtonEventArgs args)
    {
        // Open cmd.exe
        Process.Start("cmd.exe");
        return 0;
    }
}