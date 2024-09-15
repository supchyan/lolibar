﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Ikst.MouseHook;
using lolibar.tools;

namespace lolibar
{

    public partial class Lolibar : Window
    {
        // Misc
        MouseHook MouseHandler = new();

        // For screen coordinates calculation
        Matrix transformToDevice;
        System.Windows.Size screenSize;
        readonly double Inch_ScreenWidth = SystemParameters.PrimaryScreenWidth;
        readonly double Inch_ScreenHeight = SystemParameters.PrimaryScreenHeight;
        double StatusBarVisiblePosY, StatusBarHidePosY;

        // Drawing conditions
        bool IsHidden, oldIsHidden;

        // Null window to prevent lolibar's appearing inside alt+tab menu
        Window nullWin = new()
        {
            Visibility = Visibility.Hidden,
            WindowStyle = WindowStyle.ToolWindow,
            ShowInTaskbar = false,
            Width = 0, Height = 0,
        };

        // Trigger to prevent different job before... window rendered
        bool IsRendered;

        public Lolibar()
        {
            InitializeComponent();

            // Move lolibar into null window
            nullWin.Show();
            Owner = GetWindow(nullWin);

            ContentRendered += Lolibar_ContentRendered;

            BarUserContainer.MouseEnter += BarUserContainer_MouseEnter;
            BarUserContainer.MouseLeave += BarUserContainer_MouseLeave;

            BarCurProcContainer.MouseEnter += BarCurProcContainer_MouseEnter;
            BarCurProcContainer.MouseLeave += BarCurProcContainer_MouseLeave;
            BarCurProcContainer.MouseLeftButtonUp += BarCurProcContainer_MouseLeftButtonUp;

            BarRamContainer.MouseEnter += BarRamContainer_MouseEnter;
            BarRamContainer.MouseLeave += BarRamContainer_MouseLeave;

            BarCpuContainer.MouseEnter += BarCpuContainer_MouseEnter;
            BarCpuContainer.MouseLeave += BarCpuContainer_MouseLeave;

            BarPowerContainer.MouseEnter += BarPowerContainer_MouseEnter;
            BarPowerContainer.MouseLeave += BarPowerContainer_MouseLeave;

            BarSoundContainer.MouseEnter += BarSoundContainer_MouseEnter;
            BarSoundContainer.MouseLeave += BarSoundContainer_MouseLeave;
            BarSoundContainer.MouseLeftButtonUp += BarSoundContainer_MouseLeftButtonUp;

            BarTimeContainer.MouseEnter += BarTimeContainer_MouseEnter;
            BarTimeContainer.MouseLeave += BarTimeContainer_MouseLeave;

            _PreInitialize();
            _Initialize();
            _PostInitialize();

            _PreUpdate();
            _Update();
            _PostUpdate();

            MouseHandler.MouseMove += MouseHandler_MouseMove;
            MouseHandler.Start();

            GenerateTrayMenu();
        }

        // Misc
        void SetOptimalBarSize()
        {
            Resources["BarMargin"] = Resources["BarMargin"] != null ? Resources["BarMargin"] : 8.0;
            Resources["BarHeight"] = Resources["BarHeight"] != null ? Resources["BarHeight"] : 42.0;
            Resources["BarWidth"]  = Resources["BarWidth"]  != null ? Resources["BarWidth"]  : Inch_ScreenWidth - 2 * (double)Resources["BarMargin"];
        }

        #region Lifecycle Methods
        void _PreInitialize()
        {
            LolibarDefaults.Initialize();
        }
        void _Initialize()
        {
            // From Lolibar_Config.cs
            Initialize();
        }
        void _PostInitialize()
        {
            SetOptimalBarSize();
        }

        async void _PreUpdate()
        {
            while (true)
            {
                await Task.Delay(1000);
                LolibarDefaults.Update();
                if (!(bool)Resources["SnapToTop"])
                {
                    StatusBarVisiblePosY = Inch_ScreenHeight - Height - (double)Resources["BarMargin"];
                    StatusBarHidePosY = Inch_ScreenHeight;
                }
                else
                {
                    StatusBarVisiblePosY = (double)Resources["BarMargin"];
                    StatusBarHidePosY = - Height - (double)Resources["BarMargin"];
                }
            }
        }
        async void _Update()
        {
            while (true)
            {
                await Task.Delay((int)Resources["UpdateDelay"]);

                // From Lolibar_Config.cs
                Update();
            }
        }
        async void _PostUpdate()
        {
            while (true)
            {
                await Task.Delay(1000);
            }
        }
        #endregion
    }
}