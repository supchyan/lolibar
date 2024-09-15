using System.Windows;
using System.Windows.Media.Animation;

namespace lolibar
{
    partial class Lolibar : Window
    {
        #region Animtaions

        // Global animation parameters
        Duration  duration      = new Duration(TimeSpan.FromSeconds(0.3));
        Duration  el_duration   = new Duration(TimeSpan.FromSeconds(0.1));
        CubicEase easing        = new CubicEase { EasingMode = EasingMode.EaseInOut };

        void BeginStatusBarShowAnimation(Window _)
        {
            Storyboard SB = new();
            
            var ShowAnimation = new DoubleAnimation
            {
                From = _.Top,
                To = StatusBarVisiblePosY,
                Duration = duration,
                EasingFunction = easing
            };
            var OpacityOnAnimation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 1,
                Duration = duration,
                EasingFunction = easing
            };

            SB.Children.Add(ShowAnimation);
            SB.Children.Add(OpacityOnAnimation);

            Storyboard.SetTarget(ShowAnimation, _);
            Storyboard.SetTargetProperty(ShowAnimation, new PropertyPath(TopProperty));

            Storyboard.SetTarget(OpacityOnAnimation, _);
            Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

            SB.Begin(_);
        }
        void BeginStatusBarHideAnimation(Window _)
        {
            Storyboard SB = new();

            var ShowAnimation = new DoubleAnimation
            {
                From = _.Top,
                To = StatusBarHidePosY,
                Duration = duration,
                EasingFunction = easing
            };
            var OpacityOnAnimation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 0,
                Duration = duration,
                EasingFunction = easing
            };

            SB.Children.Add(ShowAnimation);
            SB.Children.Add(OpacityOnAnimation);

            Storyboard.SetTarget(ShowAnimation, _);
            Storyboard.SetTargetProperty(ShowAnimation, new PropertyPath(TopProperty));

            Storyboard.SetTarget(OpacityOnAnimation, _);
            Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

            SB.Begin(_);
        }
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
