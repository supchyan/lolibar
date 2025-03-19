<div align=center><img src=https://github.com/user-attachments/assets/8acf0034-5860-440f-a814-02f5cabfa94e width=200 height=auto /></div>
<br>
<div align=center><a href=https://github.com/supchyan/lolibar/blob/master/Mods/SupchyanMod.cs><img src=https://github.com/user-attachments/assets/b01b9b95-d8d6-4293-b5f2-324dea26af9f /></a></div>
<div align=center><a href=https://github.com/supchyan/lolibar/blob/master/Mods/ShowcaseMod.cs><img src=https://github.com/user-attachments/assets/07662aad-f4d6-4f20-9a34-6a0ab7aca995 /></a></div>


#### <div align=center>lolibar | statusbar for Windows [ 10, 11 ] | C#</div>

## 🌸Introduction
This project is the **toolkit** set for modders, which allow to create statusbars for Windows. There're **NO** `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page, so if you want to gain one, you can build it using this toolkit's source! Since this is C# Project, there is no complicated stuff in building procedure. Following a guide below will help you to get into it ASAP.

## 🌸Similar Projects
- **[yasb](https://github.com/da-rth/yasb) (Cross platform, Python)**
- **[polybar](https://github.com/polybar/polybar) (Linux, C++)**
- **[eww](https://github.com/elkowar/eww) (Linux, Rust)**
- **[ironbar](https://github.com/JakeStanger/ironbar) (Linux, Rust)**

## 🌸Average PC Usage
<div align=center><img src=https://github.com/user-attachments/assets/1deb7840-e859-4944-8464-441ff86af89c width=800 height=auto /></div>

## 🌸Pre-requirements
All modding operations is highly recommended to do in `Visual Studio 2022+`. Moreover, to build this project, you have to install `.NET 8.0 SDK`. Alternatively, you can use other `.NET SDK` versions as well, but `master` branch targets to `.NET 8.0`, so any issues with different SDK versions you have to solve locally.

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
> All mods is highly recommended to be stored in **[Mods](https://github.com/supchyan/lolibar/tree/master/Mods/)** folder.

The first step of your modding journey - **create a mod class**. Let me explain basics on `MyFirstMod.cs` example:
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
`PreInitialize()` hook useful to initialize something before `Initialize()` hook invoked, because calls before initialization process started. I recommend you to setup all properties in there. What is `properties`? Let's talk about them, referencing to **[LolibarProperties](https://github.com/supchyan/lolibar/blob/master/Source/Tools/LolibarProperties.cs)** class:
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
`Initialize()` hook is about to **initialize** something on application's launch. The most common usage is **initialization of the containers**. What is `container`? Let's get into it, referencing to **[LolibarContainer](https://github.com/supchyan/lolibar/blob/master/Source/Tools/LolibarContainer.cs)** class:
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
        Name          = "HelloContainer",
        Parent        = Lolibar.BarCenterContainer,
        Text          = "Hello!",
        HasBackground = true
    };
    HelloContainer.Create();
}
```
</br>
<div align=center><img src=https://github.com/user-attachments/assets/160c0d5f-7628-42c8-aeeb-8b2ae089f372 /></div>

*<div align=center>Result of the initialization process we can observe after Lolibar's launch.</div>*
</br>

Here we can see a new object instance, that has a couple of local properties inside. Let me explain about those, which certain example has:
* `Name` - Initial container name. Uses for automatic resources initialization;
* `Parent` - Any other container, where **your container** should be placed;
* `Text` - Text content of the container;
* `HasBackground` - Trigger to draw border around the container. It's semi-transparent and fits well with the whole statusbar theme.
</br></br>

I used `Lolibar.BarCenterContainer` here, which is tricky part to put `HelloContainer` into the one of default containers. Lolibar has `3` default containers to place custom containers inside:
* `BarLeftContainer` - Most left side of the statusbar;
* `BarRightContainer` - Most right side of the statusbar.
* `BarCenterContainer` - This container is centered relative to `BarLeftContainer` and `BarRightContainer`. This is not a center of the status bar! 
</br></br>

We've drawn a static container. Now, let's update it's content, using `Update()` hook:
```cs
public override void Update() { }
```
This hook is about to **update** something along statusbar's execution process. `Update()` hook has an **Update Delay** - time span, between loop iterations. Yes, `Update()` is an infinite loop hook, so keep it in mind. The interations delay can be modified by `BarUpdateDelay` property, which can be defined in `PreInitialize()` hook as well. To understand `Update()`'s principles better, check **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section. By the way, **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section is **great start point** in your modding journey, because it has various examples of how to create one or other thing. Anyway, let's get back to updating info in `HelloContainer`:
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
            Name           = "HelloContainer",
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
<div align=center><img src=https://github.com/user-attachments/assets/0b5f5253-ff5e-4c94-82d9-7b07559e82f7 /></div>

*<div align=center>`HelloContainer` shows current time (every 1000ms by default).</div>*
</br>

To build 'n run `Lolibar` project, you need to select preferred profile at the top of the VS and push any of `▶` `▷` buttons.</br>
<div align=center><img src=https://github.com/user-attachments/assets/6128d51e-2de1-4d7a-9db2-2cb0e2fbf404 /></div>

## 🌸Next steps
Inspired enough to start modding? Then, get into **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section to learn more about Lolibar's capabilities. As I mentioned before, **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section is **great start point** in your modding journey. Especially **[Basics](https://github.com/supchyan/lolibar/tree/master/Mods/Examples/Basics)** section. Good luck!

## 🌸Special thanks
- **[VirtualDesktop](https://github.com/MScholtes/VirtualDesktop) by @MScholtes**

## 🌸At the end...
<div align=center><img src=https://github.com/user-attachments/assets/791250c1-112e-47e0-8756-284e36194162 /></div>

##### <div align=center>😎My lolibar's <a href=https://github.com/supchyan/lolibar/blob/master/Mods/SupchyanMod.cs>mod</a> showcase</div>

---
##### <div align=center> ☕Have any questions or suggestions? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
