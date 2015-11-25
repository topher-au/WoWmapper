/*
 * MemorySharp Library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2012-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/

namespace Binarysharp.MemoryManagement.Core.CallingConvention.Enums
{
    /// <summary>
    ///     A list of type of clean-up available in calling conventions.
    /// </summary>
    public enum CleanupTypes
    {
        /// <summary>
        ///     The clean-up task is performed by the called function.
        /// </summary>
        Callee,

        /// <summary>
        ///     The clean-up task is performed by the caller function.
        /// </summary>
        Caller
    }
}