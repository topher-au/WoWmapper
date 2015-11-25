# MemorySharp
## MemorySharp's author and information ##
This project is a modifed(by lolp1) version of ZenLulz the original MemorySharp library, to whom should get all credits for his nice work. Although currently not actively being updated by him, you can find his information here:
http://binarysharp.com/
https://github.com/ZenLulz

**This version of MemorySharp information**
The reason for making modifications and changes to the original library are mainly for me to learn as a beginner, and also to provide some useful functionality that can range from syntax sugar and minor convenience to full implementation of useful new features.

Basic Features Added
------------------------------------------------------------------------
 **Support for 'in-process' Memory operations, aka injected.**

- Apocs GreyMagic detour/patch classes converted to work with this lib.
- Fast internal reads with apocs marshaling tricks  - see `Unsafe/Marshal<T>` class.
- Extension methods of all kinds for delegates, functions, and value types.
- Virtual table class and methods.
- Basic WndProc hook and manager examples, with a custom  engine interface support.
- More..

**Shared features for the library added**
 
 - Pattern scanning - supporting various formats including patter scanning from xml/json files.
 - Some x64 bit Peb data support, and added x64 support in general.
 - 3D math structures and utilities, such as world to screen etc. Thanks Zat @ unknowncheats.
 - Generic useful extension methods and helper classes.
 - Basic ILog Interface and default implementation for easy and effective logging.
 - Generic plugin interface and example manager. *Thanks coolfish on github, I think.

A few examples of features added being used
--------------

**Pattern scanning**

**Standard pattern scan**

 ```csharp
    private static byte[] PatternBytes { get; } = {1, 2, 3, 0, 0, 5};
    private static string PatternMask { get; } = "xxx??x";
    private static int OffsetLocation { get; } = 0xC;
    private static bool IsOffsetResult { get; } = false;
    private static bool RebaseAddress { get; } = true;
    public static void TestStandardPatternScan()
    {
        var memory = new MemorySharp("MyGameProcessName");
        var scanResult = memory.MainModulePatterns.Find(PatternBytes, PatternMask, OffsetLocation, IsOffsetResult,
            RebaseAddress);
        Console.WriteLine(scanResult.Address.ToString("X"));
        Console.WriteLine(scanResult.Offset.ToString("X"));
    }
```
**From a xml or json file**

 ```csharp
          // This will write the local SerializablePattern object to a xml file, then serialize it from the file.
        // It will then pattern scan it with the paramters from the xml file, and add its scanned results to the dictionary's.
        public static Dictionary<string, IntPtr> XmlPatternScanDictionary { get; } = new Dictionary<string, IntPtr>();
        public static SerializablePattern SerializablePattern { get; set; }
        public static IntPtr TestXmlPatternScanDictionary()
        {
            var memory = new MemorySharp("MyGameProcessName");
            // The object we're going to load from an xml file to scan.
            SerializablePattern = new SerializablePattern("CustomTestPattern",
                "55 8b ?? a1 ?? ?? ?? ?? 8b ?? ?? ?? ?? ?? 56 57 33 ?? 47", 4, false, true,
                "This is a comment that resides in the xml file the pattern is stored in.");
            // Write to to the file.
            // Note the same can be done with json patterns, simply replace 'XmlHelper' with 'JsonHelper' and you are set.
            XmlHelper.ExportToFile(SerializablePattern, "TestPattern.xml");
            // Now we can use this file to do our pattern scans and add it to our dictionary of choice like this as long as the file exist.
            memory.MainModulePatterns.CollectXmlScanResults("TestPattern.xml", XmlPatternScanDictionary);
            // The key to the scanned results is the description of the pattern in the xml file.
            Console.WriteLine(@"Found pattern address: " + XmlPatternScanDictionary["CustomTestPattern"].ToString("X"));
            // The resilt.
            return XmlPatternScanDictionary["CustomTestPattern"];
        }
```

**More traditional pattern scan**

 ```csharp
        // This will return a scan result from a regular pattern struct.
        public static Pattern CustomTestPattern { get; set; }
        public static ScanResult TestRegularPatternScan()
        {
            var memory = new MemorySharp("MyGameProcessName");
            // Note: pattern scans from regular byte/mask patterns work as well.
            CustomTestPattern = new Pattern("55 8b ?? a1 ?? ?? ?? ?? 8b ?? ?? ?? ?? ?? 56 57 33 ?? 47", 4, false, true);
            var scanResult = memory.MainModulePatterns.Find(CustomTestPattern);
            Console.WriteLine(@"The rebased address found was: " + scanResult.Address.ToString("X"));
            return scanResult;
        }
```
    
**Basic WndProc hook example - credits to jadd@ownedcore entirely.**

 ```csharp
       public static class WndProcHook
	    {
        // The MemoryPlus instance, not needed but used in this case.
        public static MemoryPlus MemoryPlus { get; private set; } 
        // The window hook instance.
        private static WindowHook Window { get; set; }
        // The custom engine defined by the user. Default one exist as "WindowHookEngine", if desired.
        private static IWindowEngine _msgBoxPulser;
        // The updater tool to invoke our engine start up at a set interval.
        private static Updater Updater { get; set; }

        // Install the hook.
        public static void Attach()
        {
            // We're injected so this should be valid.
            MemoryPlus = new MemoryPlus(Process.GetCurrentProcess());
            // Our instance of the custom nested engine.
            _msgBoxPulser = new MsgBoxPulse();
            // We use the MemoryPlus data to create the instance.
            // However you could simply use the direct handle of the window you choose if desired.
            Window = new WindowHook(MemoryPlus.Process.MainWindowHandle, "WndProc", ref _msgBoxPulser);
            Window.Enable();
            // Invoke our event every 1000 ms.
            Updater = new Updater(1000);
            Updater.OnUpdate += ShowMessageBoxInvoke;
            Updater.Enable();

        }

        // Our event handler for when the Updater.OnUpdate event is raised.
        private static void ShowMessageBoxInvoke(object sender, Updater.DeltaEventArgs e)
        {
            Window.InvokeUserMessage(UserMessage.StartUp);
        }

        // Nested engine example class. You would design this to fit your needs. A default class "WindowHookEngine" exist, if desired.
        private class MsgBoxPulse : IWindowEngine
        {
            public void StartUp()
            {
                MessageBox.Show(@"Hi from: " + Process.GetCurrentProcess().ProcessName +
                                @". The engine start up method has been called a total of: " + Updater.TickCount +
                                @"times.");
            }

            public void ShutDown()
            {
                // Shut down logic.
                // Make sure to call Window.Disable();
            }
        }
    }
```

**Reading values with 3DMath structures and helpers**

```csharp
			    // Get the location of an in-game object and comparing a distance
                var sharp = new MemorySharp("ProcessName");
                IntPtr objectOneXyz = new IntPtr(500);
                IntPtr objectTwoXyz = new IntPtr(500);
    
                float[] xyz = sharp.ReadArray<float>(objectOneXyz, 3);
                float[] otherXyz = sharp.ReadArray<float>(objectTwoXyz, 3);
    
                Vector3 vector3 = new Vector3(xyz);
                Vector3 vector3Two = new Vector3(otherXyz);
               
                Console.WriteLine(vector3.DistanceTo(vector3Two));
```
**World To Screen with extension methods(thanks Zatt@unknowncheats):**

  ```csharp
		        int matrixRows = 1;
                int matrixCocolumns = 1;
                Matrix viewMatrix = new Matrix(matrixRows, matrixCocolumns);
                Vector2 screenSize = new Vector2(500,500);
                Vector3 pointToConvert = new Vector3(1,1,1);
                Vector2 clientCordinates = viewMatrix.WorldToScreen(screenSize, pointToConvert);
                Console.WriteLine(clientCordinates.X + " " + clientCordinates.Y);
```
**Anyways, that should be enough to get you started peaking around the library if you like the original MemorySharp like a beginner like myself does :).

**Credits**
-------
I can't possible remember all of them - but I fully support giving all credits to the proper people for any code used. I put together this lib mostly from writing out other peoples code by hand and then implementing them in my application to learn how stuff works, so I missed a lot I am sure. Simply message me if you want to be added if I stole your code ^_^.

**People**
----------

 - ZenLulz for MemorySharp
 - Jadd @ ownedcore.
 - Zat @ unknowncheats for his ExternalUtilsCSharp - where the math/updated classes mostly come from.
 - aganonki @ unknowncheats for spoon feeding all my noob questions and cool ideas like the VirtualClass.
 - Apoc for his Detour/Patch/MarshalCache 
 - jeffora for his cool extension methods for internal reads, and his Marshaling examples.
 - Torpedos @ ownedcore for giving me really solid advice in general.
 - More..

**Sites**
-----
www.blizzhackers.cc
www.ownedcore.com
www.unknowncheats.me
https://github.com/jeffora/extemory/tree/master/src/Extemory
https://github.com/BigMo/ExternalUtilsCSharp
https://github.com/aganonki/HackTools
https://github.com/miceiken/IceFlake (Apocs GreyMagic is here as well)
https://github.com/Dramacydal/DirtyDeeds/tree/master/DirtyDeeds - cool hack and great learning material.
http://blog.ntoskr.nl/hooking-threads-without-detours-or-patches/ - WndProc hook blog by Jadd. 

