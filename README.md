<div align=center>
    <a href="#">
        <img src=https://github.com/user-attachments/assets/8acf0034-5860-440f-a814-02f5cabfa94e width=200 height=auto />
    </a>
</div>

</br>

<div align=center>
    <a href="#">
        <img src=https://github.com/user-attachments/assets/1536e5e4-4888-4350-a165-b9332de24881 />
    </a>
</div>

#### <div align=center>lolibar | statusbar lib for Windows [ 10, 11 ] | C#</div>

## 🌸Introduction
This project is the **toolkit lib** for modders, which allows you to create statusbars for Windows. There is **NO** ready-to-use executable on *Releases* page, so if you want to get one, you can build it, using source! This is a .NET project, so there is no complicated stuff in building procedure. Following the [guide](#pre-requirements) below will help you to understand basics of modding and building lolibar.

## 🌸Similar Projects
- **[yasb](https://github.com/da-rth/yasb) (Cross platform, Python)**
- **[polybar](https://github.com/polybar/polybar) (Linux, C++)**
- **[eww](https://github.com/elkowar/eww) (Linux, Rust)**
- **[ironbar](https://github.com/JakeStanger/ironbar) (Linux, Rust)**

## 🌸Contact me
If you have any questions or suggestions, you can always ping me on my **[Discord](https://discord.gg/dGF8p9UGyM)** server! I'll be glad to help you and improve my project.

## 🌸Average PC Usage

<div align=center>
    <a href="#">
        <img src=https://github.com/user-attachments/assets/18f4fe8c-3f8b-4540-bd77-5175a0243b87 width=800 height=auto />
    </a>
</div>

## 🌸CLI Support
```
C:\Users\supchyan>lolibar

Lolibar cli usage:
   lolibar [/k | /r | /s]

Options:
   /k      Terminates all lolibar.exe processes,
   /r      Restarts lolibar.exe process,
   /s      Starts lolibar.exe process

C:\Users\supchyan>
```

## 🌸Customizable context menus and toast messages
<div align=center>
    <img width="345" height="auto" alt="2" src="https://github.com/user-attachments/assets/cc239c59-7f02-4947-ae90-4df4c7810130" />
    <div align=center>
        <img width="auto" height="59" src="https://github.com/user-attachments/assets/295d77f2-c20c-48d9-a656-7b3a7a30164e" />
        <img width="auto" height="59" src="https://github.com/user-attachments/assets/a5ef1f5b-4ff5-4794-9d89-419ff81fff63" />
    </div>
</div>

## 🌸Pre-requirements
All modding operations is highly recommended to do in `Visual Studio 2022+`. Moreover, to build this project, you have to install `.NET 9.0 SDK`. Alternatively, you can use other `.NET SDK` versions as well, but `stable` branch targets to `.NET 9.0`, so any issues with different SDK versions you have to solve locally.

## 🌸Modding Basics
Have you ever tried to write mods for video games? So, this toolkit provide the same vibe:

```csharp
namespace LolibarApp.Mods;

class ExampleEmptyMod : LolibarMod
{
    public override void PreInitialize()
    {
        // Put your Pre-Initialization code here...
    }
    public override void Initialize()
    {
        // Put your Initialization code here...
    }
    public override void Update()
    {
        // Put your Updatable code here...
    }
}
```

As you can see, this code looks familiar with any other mod body. You can handle every single part of Lolibar's libraries here!

## 🌸Your First mod
> [!TIP]
> It's highly recommended to store all mods inside **[Mods](https://github.com/supchyan/lolibar/tree/stable/Mods/)** folder.

The first step of your modding journey - **create a mod class**. Let me explain basics on pseudo `MyFirstMod.cs` example:

```cs
namespace LolibarApp.Mods;

class MyFirstMod : LolibarMod
{
    public override void PreInitialize() { }
    public override void Initialize() { }
    public override void Update() { }
}
```

Code you can see above is **absolute minimum** your mod must contain. Without that, Lolibar won't compile properly. Now, talk about every part in details ↓↓↓ </br></br>

```cs
namespace LolibarApp.Mods;
```

`namespace` have to be set as `LolibarApp.Mods` unless you want to prevent your mod from being detected by `ModLoader`. </br></br>

```cs
public override void PreInitialize() { }
```

`PreInitialize()` hook useful to initialize something before `Initialize()` hook invoked, because calls before initialization process started. I recommend you to setup all properties in there. What are `properties`? Let's talk about them, referencing to **[LolibarProperties](https://github.com/supchyan/lolibar/blob/stable/Source/Tools/LolibarProperties.cs)** class:

```cs
// Properties stores values, which uses in Lolibar's resources.
// It can be anything, starting from styles, such as Main Color (BarColor),
// ending by triggers, which simulates or provides something.

// Basic example:
public override void PreInitialize()
{
    BarUpdateDelay            = 250;
    BarHeight                 = 36;
    BarColor                  = LolibarHelper.SetColor("#2a3247");
    BarContainersContentColor = LolibarHelper.SetColor("#6f85bd");
}
```

This example overrides `4` different properties, which will be applied to all Lolibar's components including Lolibar itself during initialization process. </br></br>

```cs
public override void Initialize() { }
```

`Initialize()` hook is about to **initialize** something on application's launch. The most common usage is **initialization of the containers**. What is `container`? Let's get into it, referencing to **[LolibarContainer](https://github.com/supchyan/lolibar/blob/stable/Source/Tools/LolibarContainer.cs)** class:

```cs
// All "Buttons" / "Data Fields" inside your statusbar
// have to be generated by LolibarContainer class.
// This class automatically does job of formatting,
// structing and comparing content inside specified components.
// So that component's Frankenstein can be called simply - `Container`.

// How to create one, using LolibarContainer class:
public override void Initialize()
{
    LolibarContainer HelloContainer = new()
    {
        Parent        = Lolibar.BarCenterContainer,
        Text          = "Hello!",
        HasBackground = true
    };
    HelloContainer.Create();
}
```

</br>

<div align=center>
    <a href="#">
        <img src=https://github.com/user-attachments/assets/160c0d5f-7628-42c8-aeeb-8b2ae089f372 />
    </a>
    <i><div align=center>Result of the initialization process we can observe after Lolibar's launch.</div></i>
</div>

</br>

Here we can see a new object instance, that has a couple of local properties inside. Let me explain about those, which certain example has:
* `Parent` - Any other container, where **your container** should be placed;
* `Text` - Text content of the container;
* `HasBackground` - Trigger to draw border around the container. It's semi-transparent and fits well with the whole statusbar theme.

</br>

I used `Lolibar.BarCenterContainer` here, which is tricky part to put `HelloContainer` into the one of default containers. Lolibar has `3` default containers to place custom containers inside:
* `BarLeftContainer` - Most left side of the statusbar;
* `BarRightContainer` - Most right side of the statusbar.
* `BarCenterContainer` - This container is centered relative to `BarLeftContainer` and `BarRightContainer`. This is not a center of the status bar! 

</br>

We've drawn a static container. Now, let's update it's content, using `Update()` hook:

```cs
public override void Update() { }
```

This hook is about to **update** something along statusbar's execution process. `Update()` hook has an **Update Delay** - time span, between loop iterations. Yes, `Update()` is an infinite loop hook, so keep it in mind. The interations delay can be modified by `BarUpdateDelay` property, which can be defined in `PreInitialize()` hook as well. To understand `Update()`'s principles better, check **[Examples](https://github.com/supchyan/lolibar/tree/stable/Mods/Examples)** section. By the way, **[Examples](https://github.com/supchyan/lolibar/tree/stable/Mods/Examples)** section is **great start point** in your modding journey, because it has various examples of how to create one or other thing. Anyway, let's get back to updating info in `HelloContainer`:

```cs
using LolibarApp.Source.Tools;
using LolibarApp.Source;

namespace LolibarApp.Mods;

class MyFirstMod : LolibarMod
{
    // I made this container as external var to get access to it under different hooks.
    LolibarContainer HelloContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        HelloContainer     = new()
        {
            Parent         = Lolibar.BarCenterContainer,
            Text           = "Hello!",
            HasBackground  = true
        };
        HelloContainer.Create();
    }
    public override void Update() 
    {
        HelloContainer.Text = DateTime.Now.ToString();   // Change instance's text content ...
        HelloContainer.Update();                         // ... And update it in resources

        // Now, text inside `HelloContainer` equals current OS time after every `BarUpdateDelay`.
    }
}
// Simple enough, isn't it? 🐳
```

</br>

<div align=center>
    <a href="#">
        <img src=https://github.com/user-attachments/assets/0b5f5253-ff5e-4c94-82d9-7b07559e82f7 />
    </a>
    <i><div align=center><b>HelloContainer</b> shows current time (every 1000ms by default).</div></i>
</div>

</br>

To save changes, compile and run `Lolibar` project, you need to select preferred profile at the top of the VS and push any of `▶` `▷` buttons:

<div align=center>
    <a href="#">
        <img src=https://github.com/user-attachments/assets/6128d51e-2de1-4d7a-9db2-2cb0e2fbf404 />
    </a>
</div>

## 🌸Next steps
Inspired enough to start modding? Then, get into **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section to learn more about Lolibar's capabilities. As I mentioned before, **[Examples](https://github.com/supchyan/lolibar/tree/stable/Mods/Examples)** section is **great start point** in your modding journey. Especially **[Basics](https://github.com/supchyan/lolibar/tree/stable/Mods/Examples/Basics)** section. Good luck!

## 🌸Special thanks
- **[MouseHook](https://github.com/ikst/Ikst.MouseHook) by @ikst**
- **[VirtualDesktop](https://github.com/MScholtes/VirtualDesktop) by @MScholtes**
- **[WindowsMediaController](https://github.com/DubyaDude/WindowsMediaController) by @DubyaDude**
- **[The best music covers I've ever heard](https://www.youtube.com/@vallyexe) by @vally.exe**

## 🌸In the end...
<div align=center>
    <a href="#">
        <img src="https://github.com/user-attachments/assets/77cfc1ec-5dfd-4cb9-92a7-9b703c5dab05" />
    </a>
</div>

##### <div align=center>🐳 Maintainer showcase [mod](https://github.com/supchyan/lolibar/blob/stable/Mods/MaintainerShowcaseMod.cs). Oh, also a [wallpaper](https://wallhaven.cc/w/jeeokp) source!</div>

---
##### <div align=center>☕Have any questions or suggestions? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) server!</div>
