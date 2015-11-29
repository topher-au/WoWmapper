using Binarysharp.MemoryManagement.Core.Managment.Interfaces;
using Binarysharp.MemoryManagement.Models.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Models.Detours
{
    /// <summary>
    ///     A manager class to handle function detours, and hooks.
    ///     <remarks>All credits to the GreyMagic library written by Apoc @ www.ownedcore.com</remarks>
    /// </summary>
    public class Detour : INamedElement
    {
        #region Fields, Private Properties

        /// <summary>
        ///     This var is not used within the detour itself. It is only here
        ///     to keep a reference, to avoid the GC from collecting the delegate instance!
        /// </summary>
        // ReSharper disable once NotAccessedField.Local
        private readonly Delegate _hookDelegate;

        /// <summary>
        ///     The reference of the <see cref="ProcessMemory" /> object.
        /// </summary>
        private ProcessMemory ProcessMemory { get; }

        #endregion Fields, Private Properties

        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Detour" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hook">The hook.</param>
        /// <param name="name">The name.</param>
        /// <param name="memory">The memory.</param>
        public Detour(Delegate target, Delegate hook, string name, ProcessMemory memory)
        {
            ProcessMemory = memory;
            Name = name;
            TargetDelegate = target;
            Target = Marshal.GetFunctionPointerForDelegate(target);
            _hookDelegate = hook;
            Hook = Marshal.GetFunctionPointerForDelegate(hook);

            //Store the orginal bytes
            Orginal = new List<byte>();
            Orginal.AddRange(memory.ReadBytes(Target, 6));

            //Setup the detour bytes
            New = new List<byte> { 0x68 };
            var tmp = BitConverter.GetBytes(Target.ToInt32());
            New.AddRange(tmp);
            New.Add(0xC3);
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets the hook.
        /// </summary>
        /// <value>The hook.</value>
        public IntPtr Hook { get; }

        /// <summary>
        ///     Gets the new bytes.
        /// </summary>
        /// <value>The new bytes.</value>
        public List<byte> New { get; }

        /// <summary>
        ///     Gets the orginal bytes.
        /// </summary>
        /// <value>The orginal bytes.</value>
        public List<byte> Orginal { get; }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        /// <value>The target.</value>
        public IntPtr Target { get; }

        /// <summary>
        ///     Gets the target delegate.
        /// </summary>
        /// <value>The target delegate.</value>
        public Delegate TargetDelegate { get; }

        /// <summary>
        ///     States if the detour is currently enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     The name of the detour.
        /// </summary>
        /// <value>The name of the detour.</value>
        public string Name { get; }

        /// <summary>
        ///     Gets a value indicating whether the element is disposed.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        /// <summary>
        ///     Gets a value indicating whether the element must be disposed when the Garbage Collector collects the object.
        /// </summary>
        public bool MustBeDisposed { get; set; } = true;

        #endregion Public Properties, Indexers

        #region Interface Implementations

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (IsEnabled && MustBeDisposed)
            {
                Disable();
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Removes this Detour from memory. (Reverts the bytes back to their originals.)
        /// </summary>
        /// <returns></returns>
        public void Disable()
        {
            ProcessMemory.WriteBytes(Target, Orginal.ToArray());
            IsEnabled = false;
        }

        /// <summary>
        ///     Applies this Detour to memory. (Writes new bytes to memory)
        /// </summary>
        /// <returns></returns>
        public void Enable()
        {
            ProcessMemory.WriteBytes(Target, New.ToArray());
            IsEnabled = true;
        }

        #endregion Interface Implementations

        /// <summary>
        ///     Calls the original function, and returns a return value.
        /// </summary>
        /// <param name="args">
        ///     The arguments to pass. If it is a 'void' argument list,
        ///     you MUST pass 'null'.
        /// </param>
        /// <returns>An object containing the original functions return value.</returns>
        public object CallOriginal(params object[] args)
        {
            Disable();
            var ret = TargetDelegate.DynamicInvoke(args);
            Enable();
            return ret;
        }
    }
}