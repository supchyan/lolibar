using System.Windows;
using System.Windows.Media.Animation;

namespace LolibarApp.Source.Tools;

partial class LolibarAnimator
{
    public const double TIME_QUICK = 0.12;
    public const double TIME_LONG = 0.22;
    static readonly Duration duration           = new(TimeSpan.FromSeconds(TIME_LONG));
    static readonly Duration elementDuration    = new(TimeSpan.FromSeconds(TIME_QUICK));
    static readonly CubicEase easingFunction    = new() { EasingMode = EasingMode.EaseInOut };

    public class ContextMenu : Window
    {
        /// <summary>
        /// OLD: BeginContextMenuShowAnimation
        /// </summary>
        /// <param name="_"></param>
        public static void Show(Window _)
        {
            Storyboard SB = new();

            var direction = LolibarMod.BarSnapToTop ? -1 : 1;

            var ShowAnimation = new DoubleAnimation
            {
                From = _.Top + 10.0 * direction,
                To = _.Top,
                Duration = elementDuration,
                EasingFunction = easingFunction,
            };
            var OpacityOnAnimation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 1,
                Duration = elementDuration,
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
        /// <summary>
        /// OLD: BeginContextMenuHideAnimation
        /// </summary>
        /// <param name="_"></param>
        public static void Hide(Window _)
        {
            Storyboard SB = new();

            var direction = LolibarMod.BarSnapToTop ? -1 : 1;

            var HideAnimation = new DoubleAnimation
            {
                From = _.Top,
                To = _.Top + 10.0 * direction,
                Duration = elementDuration,
                EasingFunction = easingFunction,
            };
            var OpacityOnAnimation = new DoubleAnimation
            {
                From = _.Opacity,
                To = 0,
                Duration = elementDuration,
                EasingFunction = easingFunction,
            };

            SB.Children.Add(HideAnimation);
            SB.Children.Add(OpacityOnAnimation);

            Storyboard.SetTarget(HideAnimation, _);
            Storyboard.SetTargetProperty(HideAnimation, new PropertyPath(TopProperty));

            Storyboard.SetTarget(OpacityOnAnimation, _);
            Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

            // Close the window that became transparent above
            SB.Completed += (s, e) => {
                try
                {
                    _.Close();
                }
                catch { /* it already closed */ }
            };

            SB.Begin(_);
        }
    }
    public class Core : Window
    {
        /// <summary>
        /// OLD: BeginStatusBarShowAnimation
        /// </summary>
        /// <param name="_"></param>
        public static void ShowLolibar(Window _)
        {
            Storyboard SB = new();

            var ShowAnimation = new DoubleAnimation
            {
                From = _.Top,
                To = Lolibar.GetStatusBarVisiblePosY(_),
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
        /// <summary>
        /// OLD: BeginStatusBarHideAnimation
        /// </summary>
        /// <param name="_"></param>
        public static void HideLolibar(Window _)
        {
            Storyboard SB = new();

            var HideAnimation = new DoubleAnimation
            {
                From = _.Top,
                To = Lolibar.GetStatusBarHidePosY(_),
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

            SB.Children.Add(HideAnimation);
            SB.Children.Add(OpacityOnAnimation);

            Storyboard.SetTarget(HideAnimation, _);
            Storyboard.SetTargetProperty(HideAnimation, new PropertyPath(TopProperty));

            Storyboard.SetTarget(OpacityOnAnimation, _);
            Storyboard.SetTargetProperty(OpacityOnAnimation, new PropertyPath(OpacityProperty));

            SB.Begin(_);
        }
    }
    public class Common : Window
    {
        /// <summary>
        /// OLD: BeginDecOpacityAnimation
        /// </summary>
        /// <param name="_"></param>
        public static void DecreaseTransparency(UIElement _)
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
        /// <summary>
        /// OLD: BeginIncOpacityAnimation
        /// </summary>
        /// <param name="_"></param>
        public static void IncreaseTransparency(UIElement _)
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
        public static void Appear(UIElement _)
        {
            Storyboard SB = new();
            var Animation = new DoubleAnimation
            {
                From = 0,
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
}
