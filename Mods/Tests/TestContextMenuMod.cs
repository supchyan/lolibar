using LolibarApp.Source;
using LolibarApp.Source.Tools;
using System.Diagnostics;
using System.Windows.Input;


// TESTING FEATURES, DON'T TRY TO IMPLEMENT IT IN YOUR MOD
 //namespace LolibarApp.Mods;

class TestContextMenuMod : LolibarMod
{
    LolibarContainer testContextMenuContainer = new();
    public override void PreInitialize() { }
    public override void Initialize()
    {
        testContextMenuContainer    = new()
        {
            Parent                  = Lolibar.BarRightContainer,
            Text                    = "CMT",
            HasBackground           = true,
            RightMarginOffset       = 10.0,
            MouseRightButtonUp      = OpenContextMenu
        };
        testContextMenuContainer.Create();
    }
    public override void Update() { }
    int OpenContextMenu(MouseButtonEventArgs e)
    {
        LolibarContextMenu test = new()
        {
            ChildMargin = 10,
            Orientation = System.Windows.Controls.Orientation.Vertical,

            Children = new()
            {
                new LolibarContainer()
                {
                    Text = "1",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
                new LolibarContainer()
                {
                    Text = "2",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
                new LolibarContainer()
                {
                    Text = "3",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
                new LolibarContainer()
                {
                    Text = "44444444444",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
                new LolibarContainer()
                {
                    Text = "5",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
                new LolibarContainer()
                {
                    Text = "6",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
                new LolibarContainer()
                {
                    Text = "7",
                    HasBackground = true,
                    MouseLeftButtonUp = testContextClick
                },
            }
        };
        test.Create();

        return 0;
    }

    private int testContextClick(MouseButtonEventArgs args)
    {
        Process.Start("cmd.exe");
        return 0;
    }
}