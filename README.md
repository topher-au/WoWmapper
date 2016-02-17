![WoWmapper](https://github.com/topher-au/WoWmapper/raw/master/WoWmapper/Resources/WoWmapper_Logo.png)

WoWmapper is a game controller input mapper for World of Warcraft, designed for use with the addon [ConsolePort by MunkDev](http://www.curse.com/addons/wow/console-port). Basically, it takes the buttons you press and sends them as keyboard input that ConsolePort can recognize, along with providing a few other features.

## Getting Started

### Requirements
- [Microsoft DirectX 9.0c Runtime](https://www.microsoft.com/en-us/download/confirmation.aspx?id=8109)
- [Microsoft .NET Framework v4.5.2 (or later)](https://www.microsoft.com/en-au/download/details.aspx?id=42643)
- [World of Warcraft](http://www.worldofwarcraft.com/)
- [ConsolePort by MunkDev](http://www.curse.com/addons/wow/console-port)

### Installing WoWmapper

Before downloading WoWmapper, please ensure you have all the requirements listed above. ***DirectX 9.0c is not included with Windows 10 by default, you will need to install the file from the link above for Xbox controller support on Windows 10.***

After you have installed the required software, download WoWmapper from [here](https://github.com/topher-au/WoWmapper/releases/latest). Typically, only the latest major release will be posted with an installer, and minor releases will be pushed through WoWmapper directly. The installer will usually be called *WoWmapper_1.2.3.4_Setup.msi* (for example). Once you have downloaded and installed the application, start it by selecting WoWmapper from the Start menu, or by running *WoWmapper.exe* from the application folder.

The first time you run WoWmapper, it will attempt to locate your World of Warcraft installation directory. If this is unsuccessful, you can select your World of Warcraft directory from the Settings window. If you do not select a directory, WoWmapper will be unable to update ConsolePort.

WoWmapper comes pre-configured for use with ConsolePort. If you wish to change the bindings, they can be accessed from the Settings window, or by right-clicking the system tray icon.

### Selecting a controller

WoWmapper supports DualShock 4 and Xbox 360 controllers, and will automatically bind to the first available if one is detected. You can manually select the controller from the Settings menu, under Controllers.

### WoWmapper says I am missing xinput1_3.dll, help!

[You need to install DirectX 9.0c - this is not installed by default with Windows 10.](https://www.microsoft.com/en-us/download/confirmation.aspx?id=8109)

## Advanced Features
WoWmapper can optionally provide some extra features, such as vibration and lightbar feedback, by reading some of the game's memory. For more details on this feature, please see [here](https://github.com/topher-au/WoWmapper/wiki/Advanced-Features).
