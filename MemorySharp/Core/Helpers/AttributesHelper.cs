using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Helpers
{
    /// <summary>
    ///     Static class for extracting and verifying  Attribute information.
    /// </summary>
    public class AttributesHelper
    {
        /// <summary>
        ///     Determines whether the specified item has attrib.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     <c>true</c> if the specified item has attrib; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Created 2012-01-16 19:28 by Nesox.</remarks>
        public static bool HasAttribute<T>(Type item)
        {
            return item.GetCustomAttributes(typeof (T), true).Length != 0;
        }


        /// <summary>
        ///     Determines whether [has UFP attribute] [the specified d].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns>
        ///     <c>true</c> if [has UFP attribute] [the specified d]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Created 2012-01-16 19:28 by Nesox.</remarks>
        public static bool HasUfpAttribute(Delegate d)
        {
            return HasUfpAttribute(d.GetType());
        }

        /// <summary>
        ///     Determines whether [has UFP attribute] [the specified t].
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>
        ///     <c>true</c> if [has UFP attribute] [the specified t]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Created 2012-01-16 19:28 by Nesox.</remarks>
        public static bool HasUfpAttribute(Type t)
        {
            return HasAttribute<UnmanagedFunctionPointerAttribute>(t);
        }
    }
}