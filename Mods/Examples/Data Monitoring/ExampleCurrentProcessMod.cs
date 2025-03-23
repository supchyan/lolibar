// DEPRECATED.
//using LolibarApp.Source;
//using LolibarApp.Source.Tools;
//using System.Diagnostics;

//// This mod is outside of the Mods namespace, so it won't be loaded
//// You can uncomment namespace to enable (load) it

////namespace LolibarApp.Mods;

//class ExampleCurrentProcessMod : LolibarMod
//{
//    LolibarContainer CurrentProcessContainer = new();

//    public override void PreInitialize() { }
//    public override void Initialize()
//    {
//        CurrentProcessContainer = new()
//        {
//            Name = "ExampleCurrentProcessContainer",
//            Parent = Lolibar.BarRightContainer,
//            Text = LolibarDefaults.GetCurrentApplicationInfo(),
//            Icon = LolibarDefaults.GetCurProcIcon(),
//            MouseLeftButtonUpEvent = OpenTaskManagerEvent,
//        };
//        CurrentProcessContainer.Create();
//    }
//    public override void Update()
//    {
//        CurrentProcessContainer.Text = LolibarDefaults.GetCurrentApplicationInfo();
//        CurrentProcessContainer.Update();
//    }
//    void OpenTaskManagerEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
//    {
//        new Process
//        {
//            StartInfo = new()
//            {
//                FileName = "powershell.exe",
//                Arguments = "Start-Process taskmgr.exe",
//                UseShellExecute = false,
//                CreateNoWindow = true,
//            }
//        }.Start();
//    }
//}

