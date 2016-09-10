## What is WoWmapper

WoWmapper is part of a project aimed at bringing true controller functionality to World of Warcraft.
It handles connections from DualShock 4 or Xbox/Xinput devices and converts them into button presses and mouse movements,
which are then sent to WoW and processed by [ConsolePort, an addon for World of Warcraft](http://www.wowinterface.com/downloads/info23536-ConsolePort.html).
ConsolePort binds each key to an action within World of Warcraft, and features a full UI and many features for
enhanced gameplay with a controller.

## What do I need?

- A system running Windows 7, 8, 10 or higher
- DirectX 9 or DirectX 10
- Microsoft .NET Framework 4.5.2
- A DualShock 4 or Xbox compatible controller
- World of Warcraft retail edition (no support for old/modified clients)

## How much effort is required to set it up?
 
Practically none at all. Once you have started WoWmapper, and loaded World of Warcraft, it will automatically export your configuration to ConsolePort, setting up the appropriate keybinds and modifier keys. Once in-game, you can simply load the default controller configuration for a quick and simple set up, or customize the controls further as you feel necessary.

## Configuration instructions

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

Further instructions for configuring the above applications with ConsolePort can be found in the ConsolePort documentation
