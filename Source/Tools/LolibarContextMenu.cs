using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace LolibarApp.Source.Tools;

/// <summary>
/// Class, which provides capabilities to create context menus for lolibar.
/// </summary>
public class LolibarContextMenu
{
    Window              ContextMenuWnd      { get; set; }           = new();

    /// <summary>
    /// Context menu child margin. (Affects left, top, right, bottom at once)
    /// </summary>
    public double       ChildMargin         { get; set; }

    /// <summary>
    /// Margin offset will be increased by children left/right margin offsets 
    /// to properly calculate width of context menu in horizontal orientation.
    /// </summary>
    //double              MarginOffset        { get; set; }

    /// <summary>
    /// List of context menu children. Add your clickable or not containers to here, which will be shown on context menu.
    /// </summary>
    public List<LolibarContainer> Children { get; set; } = new();

    /// <summary>
    /// Set to false, it you want to prevent context menu from closing after mouse left clicked somewhere. (True by default)
    /// </summary>
    public bool CloseOnMouseLeftClicked { get; set; } = true;

    /// <summary>
    /// Set to false, it you want to prevent context menu from closing after mouse right clicked somewhere. (True by default)
    /// </summary>
    public bool CloseOnMouseRightClicked { get; set; } = true;
    /// <summary>
    /// Context menu orientation. (Vertical by default)
    /// </summary>
    public System.Windows.Controls.Orientation Orientation { get; set; } = System.Windows.Controls.Orientation.Vertical;
    /// <summary>
    /// True if context menu window is already closing
    /// </summary>
    bool IsClosing { get; set; }

    /// <summary>
    /// Closes context menu if shown.
    /// </summary>
    public void Close()
    {
        // Prevent close dublicate
        if (IsClosing) return;

        LolibarAnimator.ContextMenu.Hide(ContextMenuWnd);
        // Remove lolbar hide interruption to make hiding logic behave normally
        Lolibar.HideInterruptions.Remove(0);
        
        IsClosing = true;
    }
    public void Create()
    {
        // Context menu is empty, so we won't show it
        if (Children.Count == 0) return;

        // Display Scale setting offset. (i. e. "125% Recommended" stuff fix)
        var scaleOffset = Lolibar.ScreenSize.X / Lolibar.Inch_Screen.X;

        ContextMenuWnd          = new()
        {
            Name                = LolibarHelper.GetRandomString(10),

            Opacity             = 0,

            WindowStyle         = WindowStyle.None,
            ResizeMode          = ResizeMode.NoResize,
            Visibility          = Visibility.Collapsed,

            SizeToContent       = SizeToContent.WidthAndHeight,

            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = VerticalAlignment.Center,

            AllowsTransparency  = true,
            Topmost             = true,
            UseLayoutRounding   = true,
            ShowInTaskbar       = false,
            IsTabStop           = false,

            Background          = LolibarColor.FromHEX("#00000000"),

            FontSize            = LolibarMod.BarFontSize,
            FontFamily          = (System.Windows.Media.FontFamily)App.Current.Resources["mononoki"]
        };

        ContextMenuWnd.MouseLeave   += ContextMenu_MouseLeave;

        // set menu orientation (vertical / horizontal)
        var StackPanelContainer = new StackPanel()
        {
            Orientation = Orientation,
            Margin      = new Thickness(ChildMargin),
        };

        var DropShadowEffect = new DropShadowEffect()
        {
            ShadowDepth    = 0,
            Opacity        = 1,
            BlurRadius     = LolibarMod.BarShadowBlurRadius,
            Color          = LolibarMod.BarShadowColor.Color,
        };

        var BorderContainer = new Border()
        {
            Background      = LolibarMod.BarColor, // LolibarColor.FromHEX("#ffffff");
            BorderThickness = LolibarMod.BarStrokeThickness,
            BorderBrush     = LolibarMod.BarStrokeColor,
            CornerRadius    = LolibarMod.BarCornerRadius,
            Effect          = DropShadowEffect,
            Margin          = new Thickness(LolibarMod.BarShadowBlurRadius),
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            VerticalAlignment   = VerticalAlignment.Center
        };

        BorderContainer.Child   = StackPanelContainer;
        ContextMenuWnd.Content  = BorderContainer;

        foreach (var child in Children)
        {
            // Initialize inner layout without drawing anywhere
            child.Initialize();

            // Set horizontal alignment
            child.GetBody().HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            child.GetRoot().HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            // We don't want to draw margin for the last child, so prevent it
            if (child != Children.Last())
            {
                // Also add margins between children.
                if (Orientation == System.Windows.Controls.Orientation.Vertical)
                {
                    // bottom margin
                    child.GetRoot().Margin = new Thickness(
                        left: child.GetRoot().Margin.Left,
                        top: child.GetRoot().Margin.Top,
                        right: child.GetRoot().Margin.Right,
                        bottom: child.GetRoot().Margin.Bottom + ChildMargin
                    );
                }
                else
                {
                    //MarginOffset += child.LeftMarginOffset + child.RightMarginOffset;

                    // right margin
                    child.GetRoot().Margin = new Thickness(
                        left: child.GetRoot().Margin.Left,
                        top: child.GetRoot().Margin.Top,
                        right: child.GetRoot().Margin.Right + ChildMargin,
                        bottom: child.GetRoot().Margin.Bottom
                    );
                }
            }
            
            // Draw child in StackPanelContainer
            StackPanelContainer.Children.Add(child.GetRoot());
        }

        if (CloseOnMouseRightClicked)
        {
            // Close this menu, when mouse right clicked somewhere
            CloseOnRightMouseClick();
        }
        if (CloseOnMouseLeftClicked)
        {
            // Close this menu, when mouse left clicked somewhere
            CloseOnLeftMouseClick();
        }

        // Prevent context menu window appearing in Alt+Tab UI
        Lolibar.HideFromAltTab(ContextMenuWnd);

        ContextMenuWnd.Show();

        // Add interruption for lolibar to prevent its close before this menu is opened
        Lolibar.HideInterruptions.Add(0);

        // Set menu spawn position
        if (LolibarMod.BarSnapToTop)
        {
            ContextMenuWnd.Top = LolibarMod.BarHeight;
        }
        else
        {
            ContextMenuWnd.Top = Lolibar.Inch_Screen.Y - LolibarMod.BarHeight - ContextMenuWnd.Height;
        }

        ContextMenuWnd.Left = Lolibar.CursorPosition.X / scaleOffset - ContextMenuWnd.Width;

        if (ContextMenuWnd.Left < 0)
        {
            ContextMenuWnd.Left = LolibarMod.BarMargin.Left;
        }

        LolibarAnimator.ContextMenu.Show(ContextMenuWnd);
    }

    void ContextMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        Close();
    }

    async void CloseOnRightMouseClick()
    {
        while (!Lolibar.MouseRightDown)
        {
            await Task.Delay(100);
        }
        while (Lolibar.MouseRightDown)
        {
            await Task.Delay(100);
        }
        Close();
    }
    async void CloseOnLeftMouseClick()
    {
        while (!Lolibar.MouseLeftDown)
        {
            await Task.Delay(100);
        }
        while (Lolibar.MouseLeftDown)
        {
            await Task.Delay(100);
        }
        if (ContextMenuWnd.IsFocused)
        {
            CloseOnLeftMouseClick();
            return;
        }
        Close();
    }
}
