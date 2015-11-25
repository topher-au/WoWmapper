using System;
using System.Collections.Generic;

namespace Binarysharp.MemoryManagement.Core.Helpers
{
    /// <summary>
    ///     Runtime class to assert input values, and throws exceptions if the requirements aren't met.
    /// </summary>
    public static class Requires
    {
        /// <summary>
        ///     Requires the specified value to be non-null, and throws an ArgumentNullException if this requirement is not met.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull<T>(T value, string parameterName) where T : class
            // We don't want to compare value types with null if we can avoid it.
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        ///     Requires the specified two values to not be equal, and throws an ArgumentException if they are.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="compareToValue">The compare to value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotEqual<T>(T value, T compareToValue, string parameterName)
        {
            if (EqualityComparer<T>.Default.Equals(value, compareToValue))
                throw new ArgumentException(parameterName);
        }

        /// <summary>
        ///     Requires the specified condition to evaluate to true, and throws an ArgumentException if it doesn't.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void Condition(Func<bool> condition, string parameterName)
        {
            if (!condition())
                throw new ArgumentException(parameterName);
        }

        /// <summary>
        ///     Requires the specified member to be non-null, and throws an InvalidOperationException if it is.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member">The member.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void MemberNotNull<T>(T member, string message)
        {
            if (member == null)
                throw new InvalidOperationException(message);
        }
    }
}