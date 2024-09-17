
using System.Windows;
using System.Windows.Media;

namespace LolibarApp.Source.Tools
{
    public static class LolibarHelper
    {
        public static bool CanBeClosed { get; private set; }
        public static readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
        public static readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;

        /// <summary>
        /// Generates `SolidColorBrush` object, by getting HEX Color value.
        /// </summary>
        public static SolidColorBrush? SetColor(string color)
        {
            return (SolidColorBrush?)new BrushConverter().ConvertFrom(color);
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
    }
}
