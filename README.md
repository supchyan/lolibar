<div align=center><img src="https://github.com/user-attachments/assets/e53fa816-ef14-4d8a-b14b-7e16ab67fede" /></div>
<div align=center><img src="https://github.com/user-attachments/assets/d1fef496-d9d9-4bf7-a092-3fbae6ccbef2" /></div>

#### <div align=center>lolibar | statusbar for Windows 10 / 11 | C#</div>

## 🪼Introduction
This project is **toolkit** for modders, which grants capabilities to create statusbars. There're no `ready-to-use` executable on **[Releases](https://github.com/supchyan/lolibar/releases)** page, so if you want to gain one, you can configure it using this toolkit!

## 🪼Alternatives
- **[yasb](https://github.com/da-rth/yasb) (Cross platform, Python)**
- **[polybar](https://github.com/polybar/polybar) (Linux, C++)**
- **[eww](https://github.com/elkowar/eww) (Linux, Rust)**
- **[ironbar](https://github.com/JakeStanger/ironbar) (Linux, Rust)**

## 🪼Pre-requirements
All modding operations is highly recommended to do in `Visual Studio 2022`. Moreover, to build this project, you have to install `.NET8.0 SDK`. Actually, you can use other `.net sdks` as well, but `main branch` targets to `.net8.0`, so any issues with different `.net` versions you have to solve locally.

## 🪼Modding Basics
Have you ever tried to write mods for video games? So, this toolkit provides the same vibes:
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
As you can see, this file looks similar to any other mod body. You can handle every single part of Lolibar's libraries here!

## 🪼Your First mod
> [!TIP]
> All mods is highly recommended to be stored in **[Mods](https://github.com/supchyan/lolibar/tree/master/Mods/)** folder.

The first step of your modding journey is **creating a mod class**. Let's get into `MyFirstMod.cs`:
```cs
namespace LolibarApp.Mods;

class MyFirstMod : LolibarMod
{
    public override void PreInitialize() { }
    public override void Initialize() { }
    public override void Update() { }
}
```
All you can see above is **absolute minimum** your mod must contain. Without that, Lolibar won't compile properly. Let's talk about every part in details. </br></br>

```cs
namespace LolibarApp.Mods;
```
`namespace` have to be set as `LolibarApp.Mods` unless you want to prevent your mod from being detected by `ModLoader`. </br></br>

```cs
public override void PreInitialize() { }
```
`PreInitialize()` hook useful to initialize something before `Initialize()` hook invoked. I recommend you to setup all properties in there. What is `properties`? Let's talk about **[LolibarProperties](https://github.com/supchyan/lolibar/blob/master/Source/Tools/LolibarProperties.cs)** class:
```cs
// Properties stores values, which uses in Lolibar's resources.
// It can be anything, starting from styles, such as Main Color (BarColor),
// ending with triggers, simulating or providing something.

// Basic example:
public override void PreInitialize()
{
    BarUpdateDelay = 250;
    BarHeight = 36;
    BarColor = LolibarHelper.SetColor("#2a3247");
    BarContainersContentColor = LolibarHelper.SetColor("#6f85bd");
}
```
This example overrides `4` different properties, which will be applied to all Lolibar's components including Lolibar itself during initialization process. </br></br>

```cs
public override void Initialize() { }
```
`Initialize()` hook is about **initialize** something on application's launch. The most common usage is containers initialization. What is `container`? Let's get into the **[LolibarContainer](https://github.com/supchyan/lolibar/blob/master/Source/Tools/LolibarContainer.cs)** class:
```cs
// All "Buttons" / "Data Fields" inside your statusbar generated by LolibarContainer class
// and named simply - Containers.

// How to create one:
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

Here we can see a new object instance, which has a couple of data inside. Let me explain, which of them certain example has:
* `Name` - Initial container name. Uses for automatic resources initialization;
* `Parent` - Any other container, where this should be placed;
* `Text` - Text content of the container;
* `HasBackground` - Trigger to draw border around the container. It's semi-transparent and fits well with the whole statusbar theme. 

We've drawn static container. Now, let's update it's content, using `Update()` hook:
```cs
public override void Update() { }
```
This hook is about to **update** something while statusbar execution process. `Update()` has **Update delay** - time span, between update loop iterations. This delay can be modified by `BarUpdateDelay` property. To understand `Update()`'s principles and restictions, check **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section. By the way, **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section is **great start point** in your modding journey, because it has various examples of how to create one or other thing. Anyway, let's get back into updating info in `HelloContainer`:
```cs
namespace LolibarApp.Mods;

class MyFirstMod : LolibarMod
{
    // I've made it external to get access to it from other hook.
    LolibarContainer HelloContainer;

    public override void PreInitialize() { }
    public override void Initialize()
    {
        HelloContainer = new()
        {
            Name = "HelloContainer",
            Parent = Lolibar.BarCenterContainer,
            Text = "Hello!",
            HasBackground = true
        };
        HelloContainer.Create();
    }
    public override void Update() 
    {
        HelloContainer.Text = "I like whales!"; // Update instance's text content ...
        HelloContainer.Update();                // ... And load it into resources
    }
}
// Simple enough, isn't it? 🐳
```
</br>
<div align=center><img src=https://github.com/user-attachments/assets/ba51d8a4-79ec-46db-9320-6ab4dad612cc /></div>

*<div align=center>Now `HelloContainer` has a brand new text content!</div>*

## 🪼Next steps
Inspired enough to start modding? Then, get into **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section to learn more about Lolibar's capabilities. As I mentioned before, **[Examples](https://github.com/supchyan/lolibar/tree/master/Mods/Examples)** section is **great start point** in your modding journey. Especially **[Basics](https://github.com/supchyan/lolibar/tree/master/Mods/Examples/Basics)** section.

## 🪼Something about `LolibarApp.csproj`
To compile this `Lolibar Project`, you have to set valid `TargetFramework` for your system in **[LolibarApp.csproj](https://github.com/supchyan/lolibar/blob/master/LolibarApp.csproj)** </br>
As an example of this source, for Windows 11, it should be set to `net8.0-windows10.0.22000.0`. </br>
You can read **[this](https://learn.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-enhance)** article on Microsoft Learn to find out, which `TargetFramework` is for you.
```csproj
﻿<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>net8.0-windows10.0.22000.0</TargetFramework>
        ...other tags
    </PropertyGroup>
</Project>
```

## 🪼Special thanks
- **[VirtualDesktop@MScholtes](https://github.com/MScholtes/VirtualDesktop)**
- **[WindowsMediaController@DubyaDude](https://github.com/DubyaDude/WindowsMediaController)**

## 🪼At the end...
<div align=center><img src=https://github.com/user-attachments/assets/8d64d34a-0368-4f3d-a561-b66a02f6992a /></div>

##### <div align=center>😎My lolibar's <a href=https://github.com/supchyan/lolibar/blob/master/Mods/SupchyanMod.cs>mod</a> showcase</div>

---
##### <div align=center> ☕Have any questions or suggestions? Feel free to contact me on my [Discord](https://discord.gg/dGF8p9UGyM) Server!</div>
