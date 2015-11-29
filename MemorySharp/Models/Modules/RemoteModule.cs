using Binarysharp.MemoryManagement.Core.Memory.Objects;
using Binarysharp.MemoryManagement.Core.Modules;
using Binarysharp.MemoryManagement.Core.Native.Objects;
using Binarysharp.MemoryManagement.Models.Calls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Binarysharp.MemoryManagement.Models.Modules
{
    /// <summary>
    ///     Class repesenting a module in the remote process.
    /// </summary>
    public class RemoteModule : ProxyRegion
    {
        #region Fields, Private Properties

        /// <summary>
        ///     The dictionary containing all cached functions of the remote module.
        /// </summary>
        internal static readonly IDictionary<Tuple<string, SafeMemoryHandle>, GenericRemoteFunction> CachedFunctions =
            new Dictionary<Tuple<string, SafeMemoryHandle>, GenericRemoteFunction>();

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteModule" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        /// <param name="module">The native <see cref="ProcessModule" /> object corresponding to this module.</param>
        internal RemoteModule(MemorySharp memorySharp, ProcessModule module)
            : base(memorySharp.Handle, module.BaseAddress)
        {
            MemorySharp = memorySharp;
            // Save the parameter
            Native = module;
            Patterns = new PatternScanner(MemorySharp, module);
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the memory sharp reference for this instance.
        /// </summary>
        /// <value>The memory sharp instance.</value>
        protected MemorySharp MemorySharp { get; }

        /// <summary>
        ///     Gets the pattern scannning instance for the remote module.
        /// </summary>
        /// <value>A pattern scanning instance for the module..</value>
        public PatternScanner Patterns { get; }

        /// <summary>
        ///     State if this is the main module of the remote process.
        /// </summary>
        public bool IsMainModule => MemorySharp.ImageBase == BaseAddress;

        /// <summary>
        ///     Gets if the <see cref="RemoteModule" /> is valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return base.IsValid &&
                       MemorySharp.Process.Modules.Cast<ProcessModule>()
                                  .Any(m => m.BaseAddress == BaseAddress && m.ModuleName == Name);
            }
        }

        /// <summary>
        ///     The name of the module.
        /// </summary>
        public string Name => Native.ModuleName;

        /// <summary>
        ///     The native <see cref="ProcessModule" /> object corresponding to this module.
        /// </summary>
        public ProcessModule Native { get; }

        /// <summary>
        ///     The full path of the module.
        /// </summary>
        public string Path => Native.FileName;

        /// <summary>
        ///     The size of the module in the memory of the remote process.
        /// </summary>
        public int Size => Native.ModuleMemorySize;

        /// <summary>
        ///     Gets the specified function in the remote module.
        /// </summary>
        /// <param name="functionName">The name of the function.</param>
        /// <returns>A new instance of a <see cref="RemoteFunction" /> class.</returns>
        public GenericRemoteFunction this[string functionName] => FindFunction(functionName);

        #endregion Public Properties, Indexers

        /// <summary>
        ///     Ejects the loaded dynamic-link library (DLL) module.
        /// </summary>
        public void Eject()
        {
            // Eject the module
            MemorySharp.Factories.ModuleFactory.Eject(this);
            // Remove the pointer
            BaseAddress = IntPtr.Zero;
        }

        /// <summary>
        ///     Finds the specified function in the remote module.
        /// </summary>
        /// <param name="functionName">The name of the function (case sensitive).</param>
        /// <returns>A new instance of a <see cref="RemoteFunction" /> class.</returns>
        /// <remarks>
        ///     Interesting article on how DLL loading works: http://msdn.microsoft.com/en-us/magazine/bb985014.aspx
        /// </remarks>
        public GenericRemoteFunction FindFunction(string functionName)
        {
            // Create the tuple
            var tuple = Tuple.Create(functionName, MemorySharp.SafeHandle);

            // Check if the function is already cached
            if (CachedFunctions.ContainsKey(tuple))
                return CachedFunctions[tuple];

            // If the function is not cached
            // Check if the local process has this module loaded
            var localModule =
                Process.GetCurrentProcess()
                       .Modules.Cast<ProcessModule>()
                       .FirstOrDefault(m => m.FileName.ToLower() == Path.ToLower());
            var isManuallyLoaded = false;

            try
            {
                // If this is not the case, load the module inside the local process
                if (localModule == null)
                {
                    isManuallyLoaded = true;
                    localModule = ModuleCore.LoadLibrary(Native.FileName);
                }

                // Get the offset of the function
                var offset = ModuleCore.GetProcAddress(localModule, functionName).ToInt64() -
                             localModule.BaseAddress.ToInt64();

                // Rebase the function with the remote module
                var function = new GenericRemoteFunction(MemorySharp, functionName,
                    new IntPtr(Native.BaseAddress.ToInt64() + offset));

                // Store the function in the cache
                CachedFunctions.Add(tuple, function);

                // Return the function rebased with the remote module
                return function;
            }
            finally
            {
                // Free the module if it was manually loaded
                if (isManuallyLoaded)
                    ModuleCore.FreeLibrary(localModule);
            }
        }

        /// <summary>
        ///     Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        /// <param name="module">The module to eject.</param>
        internal static void InternalEject(MemorySharp memorySharp, RemoteModule module)
        {
            // Call FreeLibrary remotely
            memorySharp.Factories.ThreadFactory.CreateAndJoin(memorySharp["kernel32"]["FreeLibrary"].BaseAddress,
                module.BaseAddress);
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"BaseAddress = 0x{BaseAddress.ToInt64():X} Name = {Name}";
        }
    }
}