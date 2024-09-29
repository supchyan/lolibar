using System.Reflection;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows;

//////////////////////////
///////////  TEST FEATURES
namespace LolibarApp.Source.Tools
{
    public class LolibarEffects : ShaderEffect
    {
        private static PixelShader _pixelShader =
            new PixelShader() { UriSource = MakePackUri("Effects/ThresholdEffect.fx.ps") };

        public LolibarEffects()
        {
            PixelShader = _pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ThresholdProperty);
            UpdateShaderValue(BlankColorProperty);
        }

        // MakePackUri is a utility method for computing a pack uri
        // for the given resource. 
        public static Uri MakePackUri(string relativeFile)
        {
            Assembly a = typeof(LolibarEffects).Assembly;

            // Extract the short name.
            string assemblyShortName = a.ToString().Split(',')[0];

            string uriString = "pack://application:,,,/" +
                assemblyShortName +
                ";component/" +
                relativeFile;

            return new Uri(uriString);
        }

        ///////////////////////////////////////////////////////////////////////
        #region Input dependency property

        public System.Windows.Media.Brush Input
        {
            get { return (System.Windows.Media.Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(LolibarEffects), 0);

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region Threshold dependency property

        public double Threshold
        {
            get { return (double)GetValue(ThresholdProperty); }
            set { SetValue(ThresholdProperty, value); }
        }

        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register("Threshold", typeof(double), typeof(LolibarEffects),
                    new UIPropertyMetadata(0.5, PixelShaderConstantCallback(0)));

        #endregion

        ///////////////////////////////////////////////////////////////////////
        #region BlankColor dependency property

        public System.Windows.Media.Color BlankColor
        {
            get { return (System.Windows.Media.Color)GetValue(BlankColorProperty); }
            set { SetValue(BlankColorProperty, value); }
        }

        public static readonly DependencyProperty BlankColorProperty =
            DependencyProperty.Register("BlankColor", typeof(System.Windows.Media.Color), typeof(LolibarEffects),
                    new UIPropertyMetadata(Colors.Transparent, PixelShaderConstantCallback(1)));

        #endregion
    }
}
