using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     Class providing extension methods for <see cref="Delegate" /> types.
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        ///     Shortcut method to convert a delegate to a native function pointer. Simple wrapper around
        ///     <see cref="Marshal.GetFunctionPointerForDelegate" />.
        /// </summary>
        /// <param name="del">Delegate to convert</param>
        /// <returns>Pointer to the function that can be accessed from native code.</returns>
        public static IntPtr ToFunctionPointer(this Delegate del)
        {
            return Marshal.GetFunctionPointerForDelegate(del);
        }
    }
}