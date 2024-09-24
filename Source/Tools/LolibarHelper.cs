﻿using LolibarApp.Mods;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LolibarApp.Source.Tools
{
    public static partial class LolibarHelper
    {
        public static bool CanBeClosed { get; private set; }
        public static readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
        public static readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;

        /// <summary>
        /// Generates `SolidColorBrush` object, by getting HEX Color value.
        /// </summary>
        public static SolidColorBrush SetColor(string color)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(color);
        }

        public static void CloseApplicationGently()
        {
            CanBeClosed = true;
            System.Windows.Application.Current.Shutdown();
        }
        // https://stackoverflow.com/questions/3895188/restart-application-using-c-sharp
        public static void RestartApplicationGently()
        {
            CanBeClosed = true;
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Simplifies initialize of the default events for `containers`.
        /// </summary>
        /// <param name="element">Actual container</param>
        /// <param name="mouseEnterEvent">Set 'null', if you don't need it</param>
        /// <param name="mouseLeaveEvent">Set 'null', if you don't need it</param>
        /// <param name="leftMouseClickEvent">Set 'null', if you don't need it</param>
        /// <param name="rightMouseClickEvent">Set 'null', if you don't need it</param>
        public static void SetContainerEvents(this UIElement element, System.Windows.Input.MouseEventHandler? mouseEnterEvent, System.Windows.Input.MouseEventHandler? mouseLeaveEvent, System.Windows.Input.MouseButtonEventHandler? leftMouseClickEvent, System.Windows.Input.MouseButtonEventHandler? rightMouseClickEvent)
        {
            if (mouseEnterEvent != null)        element.MouseEnter          += mouseEnterEvent;
            if (mouseLeaveEvent != null)        element.MouseLeave          += mouseLeaveEvent;
            if (leftMouseClickEvent != null)    element.MouseLeftButtonUp   += leftMouseClickEvent;
            if (rightMouseClickEvent != null)   element.MouseRightButtonUp  += rightMouseClickEvent;
        }
    }
}
