## What is WoWmapper

WoWmapper is part of a project aimed at bringing true controller functionality to World of Warcraft.

It's primary purpose is to handle DualShock 4 or Xbox/Xinput controller input and convert it into button presses and mouse movements which are then sent to WoW and processed by [ConsolePort, an addon for World of Warcraft](http://www.wowinterface.com/downloads/info23536-ConsolePort.html). ConsolePort binds each key to an action within World of Warcraft, and features a full UI and many features for enhanced gameplay with a controller. WoWmapper also includes force feedback and input assistance utilites such as vibration, automatic cursor centering and pseudo-analog-sensitive movement as well as many more features designed to make efficient controller gameplay a reality in the World of Warcraft.

## What do I need?

- A system running Windows 7, 8, 10 or higher
- DirectX 9 or DirectX 10
- Microsoft .NET Framework 4.5.2
- A DualShock 4 or Xbox/Xinput compatible controller
- World of Warcraft retail edition *(WoWmapper does not support unofficial clients)*

**Before you download WoWmapper, please ensure that you have updated DirectX to the latest version available for your system, and that you meet the other requirements for running the application.**

### How much effort is required to get it set up?
 
WoWmapper and ConsolePort are designed to work together to make the installation as simple as possible. Once ConsolePort and WoWmapper are installed, launching World of Warcraft will export a keybinding configuration file to the ConsolePort folder that will be loaded while ConsolePort is active, meaning that ConsolePort will not need to be calibrated in-game, and when you disable ConsolePort, your regular bindings are preserved underneath allowing you to easily switch between keyboard and mouse or controller gameplay simply by toggling the ConsolePort addon and reloading the user interface.

### Configuration instructions

**WoWmapper features an automatic configuration system that will set up your keybindings and icons in-game without any input. We recommend leaving WoWmapper's keybindings as their defaults and only changing the modifier layout.**

By default, temporary keybindings will be exported to ConsolePort that will not make any permanent changes to your standard keybindings. These bindings are controlled by ConsolePort and you can switch from controller to keyboard and mouse by simply disabling ConsolePort, and back again by re-enabling it. Additionally, WoWmapper will configure the button icons within ConsolePort to match the currently connected controller.

To change your controller layout, open the WoWmapper configuration and select *Key Bindings*. From here you can change which of the shoulder buttons will be used as modifiers, and select which button icons will be shown in WoWmapper and ConsolePort. Additionally, you may override the default WoWmapper bindings (this is not recommended).

If you make any changes to the controller layout or bindings, you must type `/reload` in-game or restart World of Warcraft for the changes to take effect.

## Non-Windows systems and WoWmapper alternatives

If you're not running Windows, or WoWmapper isn't suitable for your setup, there are several alternatives available. These alternatives only provide base input mapping - no advanced features or haptic feedback.

### Mac OS X
- ControllerMate
- Joystick Mapper

### Windows alternatives

- DS4Windows
- Xpadder
- JoyToKey
- Keysticks