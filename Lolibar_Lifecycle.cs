using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Ikst.MouseHook;
using LolibarApp.Tools;

namespace LolibarApp
{
    partial class Lolibar : Window
    {
        #region Lifecycle
        void __PreInitialize()
        {
            LolibarDefaults.Initialize();
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
            LolibarDefaults.Update();
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
