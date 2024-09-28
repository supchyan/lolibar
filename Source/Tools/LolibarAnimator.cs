using System.Windows;
using System.Windows.Media.Animation;

namespace LolibarApp.Source.Tools;

partial class LolibarAnimator : Window
{
    static readonly Duration duration           = new(TimeSpan.FromSeconds(0.3));
    static readonly Duration elementDuration    = new(TimeSpan.FromSeconds(0.2));
    static readonly CubicEase easingFunction    = new() { EasingMode = EasingMode.EaseInOut };

    public static void BeginStatusBarShowAnimation(Window _)
    {
        Storyboard SB = new();

        var ShowAnimation = new DoubleAnimation
        {
            From = _.Top,
            To = Lolibar.StatusBarVisiblePosY,
            Duration = duration,
            EasingFunction = easingFunction,
        };
        var OpacityOnAnimation = new DoubleAnimation
        {
            From = _.Opacity,
            To = 1,
            Duration = duration,
            EasingFunction = easingFunction,
        };

        SB.Children.Add(ShowAnimation);
        SB.Children.Add(OpacityOnAnimation);

        Storyboard.SetTarget(ShowAnimation, _);
        Storyboard.SetTargetProperty(ShowAnimation, new PropertyPath(TopProperty));

        Storyboard.SetTarget(OpacityOnAnimation, _);
        Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

        SB.Begin(_);
    }
    public static void BeginStatusBarHideAnimation(Window _)
    {
        Storyboard SB = new();

        var ShowAnimation = new DoubleAnimation
        {
            From = _.Top,
            To = Lolibar.StatusBarHidePosY,
            Duration = duration,
            EasingFunction = easingFunction,
        };
        var OpacityOnAnimation = new DoubleAnimation
        {
            From = _.Opacity,
            To = 0,
            Duration = duration,
            EasingFunction = easingFunction,
        };

        SB.Children.Add(ShowAnimation);
        SB.Children.Add(OpacityOnAnimation);

        Storyboard.SetTarget(ShowAnimation, _);
        Storyboard.SetTargetProperty(ShowAnimation, new PropertyPath(TopProperty));

        Storyboard.SetTarget(OpacityOnAnimation, _);
        Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

        SB.Begin(_);
    }
    public static void BeginDecOpacityAnimation(UIElement _)
    {
        Storyboard SB = new();
        var Animation = new DoubleAnimation
        {
            From = _.Opacity,
            To = 0.5,
            Duration = elementDuration,
            EasingFunction = easingFunction,
        };
        SB.Children.Add(Animation);
        Storyboard.SetTarget(Animation, _);
        Storyboard.SetTargetProperty(Animation, new PropertyPath(OpacityProperty));
        SB.Begin((FrameworkElement)_);
    }
    public static void BeginIncOpacityAnimation(UIElement _)
    {
        Storyboard SB = new();
        var Animation = new DoubleAnimation
        {
            From = _.Opacity,
            To = 1,
            Duration = elementDuration,
            EasingFunction = easingFunction,
        };
        SB.Children.Add(Animation);
        Storyboard.SetTarget(Animation, _);
        Storyboard.SetTargetProperty(Animation, new PropertyPath(OpacityProperty));
        SB.Begin((FrameworkElement)_);
    }
}
