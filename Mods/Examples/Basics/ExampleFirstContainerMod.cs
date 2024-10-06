using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Windows.Input;
using System.Windows.Media;

// This mod is outside of the Mods namespace, so it won't be loaded
// You can uncomment namespace to make in loadable

//namespace LolibarApp.Mods;

class ExampleFirstContainerMod : LolibarMod
{
    // This mod exmplains, how to create, customize and update containers.

    // Let's define external Container type:
    LolibarContainer MyFirstContainer = new LolibarContainer();

    public override void PreInitialize() 
    {
        // We don't use this override in container's initialization.
        // So let's go to the next override.
    }
    public override void Initialize()
    {
        // Now let's setup our container with explanations:


        // Container's name. You have to choose it,
        // unless you won't overlap it's resources with other containers.
        MyFirstContainer.Name = "MyCoolContainer";


        // Text content. Won't be drawn, if you won't set it.
        MyFirstContainer.Text = "Hello world";


        // Container's icon. Won't be drawn, if you won't set it.
        MyFirstContainer.Icon = Geometry.Empty;

        // Let me exmplain, what Icon stores inside:
        //
        // Icon is `Geometry` class instance, which is sort of SVG icons representation.
        // So, if you have a svg icon, you want to use as the container one, you have to do:
        //
        // 1. Get it's data. Just open svg icon file in text editor and copy it's value.
        // This will be long odd line of symbols, goes inside d="" but it's ok.
        // That line is coordinates of icon's verticles, whitch uses to draw svg exactly.
        //
        // 2. Paste copied data to Icon property like:
        MyFirstContainer.Icon = Geometry.Parse("M16 8C16 12.4183 12.4183 16 8 16C3.58172 16 0 12.4183 0 8C0 3.58172 3.58172 0 8 0C12.4183 0 16 3.58172 16 8ZM7 7.5C7 8.88071 5.88071 10 4.5 10C3.11929 10 2 8.88071 2 7.5C2 6.11929 3.11929 5 4.5 5C5.88071 5 7 6.11929 7 7.5ZM11.5 10C12.8807 10 14 8.88071 14 7.5C14 6.11929 12.8807 5 11.5 5C10.1193 5 9 6.11929 9 7.5C9 8.88071 10.1193 10 11.5 10Z");

        // Geometry.Parse("") gets data string and tries to parse it as svg image.
        // So, after pasting there your data, you have to see your icon inside the container!
        //
        // Important here. All default icons is relative to 16x16 grid in pixels,
        // so if you want to make a custom additional icon, draw it as svg in mentioned border restrictions.


        // Let's go forward:

        
        // Parent contaienr where your container have to be drawn.
        // If it's set to `null`, your container won't draw anywhere.
        MyFirstContainer.Parent = null;

        // Lolibar has 3 default container to draw content inside:
        //
        // 1. Lolibar.BarLeftContainer - Left aligned container;
        // 2. Lolibar.BarRightContainer - Right aligned container;
        // 3. Lolibar.BarCenterContainer - Centered relative to previous both containers, NOT TO SCREEN's center;

        // Getting this info let's set valid parent:
        MyFirstContainer.Parent = Lolibar.BarCenterContainer;


        // This will draw separator line at specified size of the container.
        // It's useful to separate different categories of the containers in statusbar.
        MyFirstContainer.SeparatorPosition = null;

        // Lolibar has Enum for this called LolibarEnums.SeparatorPosition
        // So let's use it and draw separator lines at `None` sides of the container:
        MyFirstContainer.SeparatorPosition = LolibarEnums.SeparatorPosition.None;


        // Draws the background of the container.
        // It's semi-transparent and relatives to `BarContainerColor`
        MyFirstContainer.HasBackground = true;


        // I mentioned `BarContainerColor`, so what about container's color.
        MyFirstContainer.Color = default;

        // Well, as a `default` value it equals to `BarContainerColor` property,
        // so you even can ignore this property, unless you want to create container
        // with a custom color. In that case, let me show, how to do it:
        MyFirstContainer.Color = LolibarHelper.SetColor("#ff0000");

        // `#ff0000` here is HEX representation of the RGB Color.
        // You can use any color you wish.


        // --- Events ---
        //
        // Container can listen 3 types of events:
        //
        // 1. MouseLeftButtonUpEvent - left mouse click to it
        // 2. MouseRightButtonUpEvent - right mouse click to it
        // 3. MouseWheelEvent - mouse wheel scrolling (up / down)
        //
        // You have to know what are events in C# is, to handle it,
        // but let me show `MouseLeftButtonUpEvent` as an example:
        MyFirstContainer.MouseLeftButtonUpEvent = MyLeftMouseEvent;


        // Now, when we have got into parameters, let's create our container:
        MyFirstContainer.Create();

        // Done! Now let's go to `Update()` section.
    }

    public override void Update()
    {
        // In `Update()` section you can update container's parameters.
        // Let's update it's text content to something else:
        MyFirstContainer.Text = DateTime.Now.Millisecond.ToString();

        // Now our text have to be show current time milliseconds.

        // But if we'll compile or app now, this text won't be updated,
        // so let's call the update method for the container:
        MyFirstContainer.Update();

        // Now everything have to be fine. Have fun~
    }

    // --- Addition to Events section ---
    private void MyLeftMouseEvent(object sender, MouseButtonEventArgs e)
    {
        System.Windows.MessageBox.Show("Hello there :)");
    }
}