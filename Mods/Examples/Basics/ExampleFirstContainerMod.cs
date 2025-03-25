using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Windows.Media;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to enable (load) it

//namespace LolibarApp.Mods;

class ExampleFirstContainerMod : LolibarMod
{
    // This mod explains, how to create, customize and update containers.

    // Let's define external Container type:
    LolibarContainer MyFirstContainer = new LolibarContainer();

    public override void PreInitialize() 
    {
        // We don't use this override in container's initialization.
        // So let's go to the next override.
    }
    public override void Initialize()
    {
        // Now, let's setup our container with explanations:


        // Container's name. You have to set it up,
        // unless you want to overlap its resources with other containers.
        MyFirstContainer.Name = "MyCoolContainer";


        // Text content:
        MyFirstContainer.Text = "Hello world";

        // Icon context.
        // You can use two icon formats: `svg` and `ico`.
        // Each icon have to be stored inside specified directory. But which one?
        // The project has `Icons` directory and `./svg` and `./ico` inside as well.
        // So put your icons there. After that, you can call them like:
        MyFirstContainer.Icon = LolibarIcon.ParseSVG("./Examples/example_first_container_icon.svg");

        // As you can see, you don't need to write a whole path to the icon.
        // Calling `ParseSVG()` already moves you inside `./Icons/svg/` directory,
        // so only path you need to specify - the path from `.../svg/` to your icon location.

        // [ You can check `./Icons/svg/` folder manually for better understanding ]

        // The same trick can be released for the ico images in the way like:
        MyFirstContainer.Icon = LolibarIcon.ParseICO("./Examples/example_first_container_icon.ico");

        // Btw, they are different containers, so defining both type of icons will draw both containers as well.
        // But you can't draw two `svg` icons or two `ico` icons in the same time.


        // Let's go forward:


        // Parent contaienr where your container have to be drawn.
        // If it's `null`, your container won't be drawn at all.
        MyFirstContainer.Parent = null;

        // Lolibar has 3 default containers to draw content inside:
        //
        // 1. Lolibar.BarLeftContainer - Left aligned container;
        // 2. Lolibar.BarRightContainer - Right aligned container;
        // 3. Lolibar.BarCenterContainer - Centered relative to previous both containers;

        // Getting into info above, let's set valid parent:
        MyFirstContainer.Parent = Lolibar.BarCenterContainer;

        // This will draw separator line at specified size of the container.
        // It's useful to separate different categories of the containers in statusbar.
        MyFirstContainer.SeparatorPosition = null;

        // `LolibarEnums` has Enum called `SeparatorPosition`
        // So let's use it to select separators' lines position at `Both` sides of the container:
        MyFirstContainer.SeparatorPosition = LolibarEnums.SeparatorPosition.Both;

        // To draw a background behind the container,
        // you can set `HasBackground` to true.
        // It's semi-transparent and relatives to `BarContainerColor` property.
        MyFirstContainer.HasBackground = true;

        // I mentioned `BarContainerColor`, so what about custom container's color.
        MyFirstContainer.Color = default;

        // Well, a `default` value equal to `BarContainerColor` property.
        // For custom colors, you can use `LolibarColor` lib:
        MyFirstContainer.Color = LolibarColor.FromHEX("#ff0000");

        // `#ff0000` is HEX representation of RGB Color.
        // So, you can use any color you want.

        // --- Events ---
        //
        // Container can listen 4 types of events:
        //
        // 1. MouseLeftButtonUp     - left mouse button up
        // 2. MouseRightButtonUp    - right mouse button up
        // 3. MouseMiddleButtonUp   - middle mouse button up
        // 4. MouseWheelDelta       - mouse wheel scrolling (up or down)
        //
        // You don't need to register on events, Lolibar does it automatically,
        // Just tell to Lolibar, what should happen upon event's invoke.
        MyFirstContainer.MouseLeftButtonUp = MyLeftMouseUpHook;

        // Now, when we have got into parameters, let's create our container:
        MyFirstContainer.Create();

        // Done! Now let's go to `Update()` section.
    }

    public override void Update()
    {
        // In `Update()` section you can update container's parameters.
        // Let's update a text content:
        MyFirstContainer.Text = DateTime.Now.ToString();

        // ...And we need to update container after changes:
        MyFirstContainer.Update();

        // `MyFirstContainer` will show current OS time after that.

        // That's it, have fun~
    }

    // --- Addition to Events section ---
    int MyLeftMouseUpHook(MouseButtonEventArgs e)
    {
        System.Windows.MessageBox.Show("Hello there :)");

        // Because this is Func<int>
        return 0;
    }
}