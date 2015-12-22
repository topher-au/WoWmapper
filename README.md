![WoWmapper Logo](https://github.com/topher-au/WoWmapper/raw/master/WoWmapper/Resources/WoWmapperLogo.png)

# WoWmapper

WoWmapper is a game controller input mapper for World of Warcraft, designed for use with the addon [ConsolePort by MunkDev](http://www.wowinterface.com/downloads/info23536-ConsolePort-GameControllerAddon.html).

## Getting Started

### Requirements

- [Microsoft .NET Framework v4.5.2 (or later)](https://www.microsoft.com/en-au/download/details.aspx?id=42643)
- [SlimDX Redistributable](https://slimdx.org/download.php)
- [World of Warcraft](http://www.worldofwarcraft.com/)
- [ConsolePort by MunkDev](http://www.wowinterface.com/downloads/info23536-ConsolePort-GameControllerAddon.html)

### Installing WoWmapper

Easy to set up and use, simply download the latest release (or build your own) and run it. WoWmapper will start with Dualshock4 selected by default, select another input plugin if you are using a different controller.

If you do not have ConsolePort installed, you can install it from the main application window, or by downloading it manually (see links below).

Buttons can be rebound as needed from the Keybinds tab. The default configuration uses F1-F12, Numpad *, -, +. At any time you may reset your keybinds by clicking the button in the upper right corner of the keybinds tab.

From the Settings tab, you can change some application settings or enable the Advanced Haptics module.

## Enhanced Interaction

The Enhanced Interaction module uses passive memory reading to obtain some basic information about your character. This information is used to provide haptic feedback such as controller vibration and lightbar controls.

Before using the Enhanced Interaction module, you should be aware of the risks associated with this kind of interaction with the game.

**[The World of Warcraft Terms of Use may be viewed in full here.](http://us.blizzard.com/en-us/company/legal/wow_tou.html)**

As per *Section 2 - Additional License Limitations*:

> *(...)* You agree that you will not, under any circumstances:
- A. use cheats, automation software (bots), hacks, mods or any other unauthorized third-party software designed to modify the World of Warcraft experience;
- C. use any unauthorized third-party software that intercepts, “mines,” or otherwise collects information from or through the Game or the Service, including without limitation any software that reads areas of RAM used by the Game to store information about a character or the game environment; provided, however, that Blizzard may, at its sole and absolute discretion, allow the use of certain third party user interfaces;

The technique used by WoWmapper is a standard windows API call and does not modify the game memory in any way. This is a similar technique used by anti-virus and anti-malware software, but targeted to a specific part of the memory.

The World of Warcraft anti-cheat system typically looks for specific, known illegal processes (bots, hacks) and parts of the game memory that have been tampered with (injected Lua code and such), and since WoWmapper does neither, it's not likely to trigger any alarms unless Blizzard tells it to.

WoWmapper is a game-enhancing addon that does not provide any additional features or automation beyond what is available with a mouse and keyboard. It is for this reason that I believe it is unlikely Blizzard would take any action on the software or it's users.
