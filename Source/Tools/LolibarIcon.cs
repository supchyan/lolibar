using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LolibarApp.Source.Tools;

class LolibarIcon
{
    /// <summary>
    /// Icon parser.
    /// </summary>
    /// <param name="iconLocation">Path to your icon. Should be in `svg` file format.</param>
    /// <returns>Geometry class object. Returns NULL if originally received unsupported file format (different from `svg`).</returns>
    public static Geometry ParseSVG(string pathToSvg)
    {
        string content = File.ReadAllText(pathToSvg);
        
        if (pathToSvg.Split(".").Last() == "svg")
        {
            content = content.Split(" d=\"")[1].Split("\" ")[0];
            return Geometry.Parse(content);
        }

        return Geometry.Empty;
    }
    /// <summary>
    /// Icon parser.
    /// </summary>
    /// <param name="iconLocation">Path to your icon. Should be in `ico` file format.</param>
    /// <returns>BitmapSource class object. Returns NULL if originally received unsupported file format (different from `ico`).</returns>
    public static BitmapSource ParseICO(string pathToIco)
    {
        try
        {
            if (pathToIco.Split(".").Last() == "ico")
            {
                return new Icon(pathToIco).ToBitmapSource();
            }
        }
        catch
        {
            return new Icon(@".\Icons\Defaults\ico\empty.ico").ToBitmapSource();
        }

        return new Icon(@".\Icons\Defaults\ico\empty.ico").ToBitmapSource();
    }
    /// <summary>
    /// Attempts to return an Icon object of the specified application, received from it's location.
    /// </summary>
    /// <param name="applicationPath">Path to your application.</param>
    /// <returns></returns>
    public static Icon? GetApplicationIcon(string applicationPath)
    {
        return Icon.ExtractAssociatedIcon(applicationPath);
    }
}
