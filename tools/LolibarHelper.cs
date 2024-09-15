
using System.Windows.Media;

namespace lolibar.tools
{
    public static class LolibarHelper
    {
        /// <summary>
        /// Generates `SolidColorBrush` object, by getting HEX Color value.
        /// </summary>
        public static SolidColorBrush? SetColor(string color)
        {
            return (SolidColorBrush?) new BrushConverter().ConvertFrom(color);
        }
    }
}
