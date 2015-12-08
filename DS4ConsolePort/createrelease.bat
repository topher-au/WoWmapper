@echo off
del /q DS4ConsolePort_*.zip
"C:\Program Files\WinRAR\WinRAR.exe" a -afzip -ep DS4ConsolePort.zip DS4ConsolePort.exe DS4ConsolePort.exe.config CPAdvancedHaptics.dll DS4Driver.dll ICSharpCode.SharpZipLib.dll Newtonsoft.Json.dll offsets.xml ..\..\..\DS4CP_Updater\bin\Release\DS4CP_Updater.exe