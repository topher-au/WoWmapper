using System;

namespace Binarysharp.MemoryManagement.Core.Extensions
{
    /// <summary>
    ///     Class EnumExtensions.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     Gets the value from an enum.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="e">The enum.</param>
        /// <returns>T.</returns>
        public static T GetValue<T>(this Enum e)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return (T) (object) e;
        }

        /// <summary>
        ///     Convers an enum to a <see cref="int" /> value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        ///     Convers an enum to a <see cref="uint" /> value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.UInt32.</returns>
        public static uint ToUint(this Enum enumValue)
        {
            return Convert.ToUInt32(enumValue);
        }

        /// <summary>
        ///     Convers an enum to a <see cref="long" /> value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.Int64.</returns>
        public static long ToLong(this Enum enumValue)
        {
            return Convert.ToInt64(enumValue);
        }

        /// <summary>
        ///     Convers an enum to a <see cref="ulong" /> value.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.UInt64.</returns>
        public static ulong ToUlong(this Enum enumValue)
        {
            return Convert.ToUInt64(enumValue);
        }
    }
}