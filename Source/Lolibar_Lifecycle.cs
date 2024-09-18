using System.Windows;
using LolibarApp.Source.Tools;

namespace LolibarApp.Source
{
    partial class Lolibar : Window
    {
        #region Lifecycle
        void __PreInitialize()
        {
            SetDefaults();
        }
        void __PostInitialize()
        {
            PostInitializeContainersVisibility();
            PostInitializeSnapping();
        }
        void _Initialize()
        {
            __PreInitialize();

            Initialize(); // From Lolibar_Config.cs

            __PostInitialize();
        }

        void __PreUpdate()
        {
            UpdateDefaults();
        }
        async void _Update()
        {
            while (true)
            {
                await Task.Delay((int)Resources["UpdateDelay"]);

                __PreUpdate();

                Update(); // From Lolibar_Config.cs
            }
        }
        #endregion
    }
}
