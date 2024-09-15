using lolibar.tools;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Animtaions
        void BeginDecOpacityAnimation(UIElement _)
        {
            Storyboard SB = new();
            var Animation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 0.5,
                Duration = el_duration,
                EasingFunction = easing
            };
            SB.Children.Add(Animation);
            Storyboard.SetTarget(Animation, _);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(OpacityProperty));
            SB.Begin((FrameworkElement)_);
        }
        void BeginIncOpacityAnimation(UIElement _)
        {
            Storyboard SB = new();
            var Animation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 1,
                Duration = el_duration,
                EasingFunction = easing
            };
            SB.Children.Add(Animation);
            Storyboard.SetTarget(Animation, _);
            Storyboard.SetTargetProperty(Animation, new PropertyPath(OpacityProperty));
            SB.Begin((FrameworkElement)_);
        }
        #endregion
    }
}
