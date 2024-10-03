using LolibarApp.Source.Tools;

namespace LolibarApp.Modding.Examples.ExampleGnomeLinuxLeftCornerSimulation
{
    public class ExampleGnomeLinuxLeftCornerSimulation
    {
        public static void Apply()
        {
            // Set it to true, to enable feature:
            LolibarProperties.BarCornersInvokesDesktopsMenu = true;
            
            // Now you can simulate WIN+TAB key press,
            // quick moving your mouse cursor to left corner of the statusbar.


            // You can swap corner to `Right` one:
            LolibarProperties.BarTargetCorner = LolibarEnums.BarTargetCorner.Right;

            // So now, this feature linked to Right corner of the screen!

            // But wait, which corner? Top Right? Bottom Right? Both???

            // 😎

            // Depends on which side of the screen your statusbar has been snapped.

            // You can modify snapping with `BarSnapToTop` property, like:
            LolibarProperties.BarSnapToTop = false;

            // Now bar will be drawn at the bottom of the screen,
            // so Right corner now is Bottom Right corner.

            // That's it!
        }
    }
}
