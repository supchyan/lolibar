using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LolibarApp.Source.Tools;

static class LolibarIcon
{
    static string IcoIconsFolderPath
    {
        get
        {
            return $"{LolibarDefaults.ExecutionPath}\\Icons\\ico\\";
        }
    }
    static string SvgIconsFolderPath
    {
        get
        {
            return $"{LolibarDefaults.ExecutionPath}\\Icons\\svg\\";
        }
    }
    /// <summary>
    /// Icon parser.
    /// </summary>
    /// <param name="iconLocation">Path to your icon. Should be in `svg` file format.</param>
    /// <returns>Geometry class object. Returns NULL if originally received unsupported file format (different from `svg`).</returns>
    public static Geometry ParseSVG(string pathToSvg)
    {
        if (pathToSvg.Split(".").Last() == "svg")
        {
            pathToSvg = pathToSvg.StandardizePath();

            string fileContent = File.ReadAllText($"{SvgIconsFolderPath}{pathToSvg}");

            fileContent = fileContent.Split(" d=\"")[1].Split("\" ")[0];
            return Geometry.Parse(fileContent);
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
                pathToIco = pathToIco.StandardizePath();

                string fileContent = File.ReadAllText($"{IcoIconsFolderPath}{pathToIco}");

                return new Icon(pathToIco).ToBitmapSource();
            }
        }
        catch
        {

        }
        pathToIco = ".\\Defaults\\empty.ico".StandardizePath();
        return new Icon($"{IcoIconsFolderPath}{pathToIco}").ToBitmapSource();
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
    /// <summary>
    /// Converts input path into proper format, by tuncating first ".\" or "\".
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static string StandardizePath(this string path)
    {
        path = path[0] == '\\' || path[0] == '/' ? path.Substring(1) : path;
        path = path[0..1] == ".\\" || path[0..1] == "./" ? path.Substring(2) : path;

        return path;
    }
}
