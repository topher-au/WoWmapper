using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Helpers
{
    /// <summary>
    ///     Static class providing tools for extracting information related to types.
    /// </summary>
    /// <typeparam name="T">Type to analyse.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static class TypeData<T>
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes static information related to the specified type.
        /// </summary>
        static TypeData()
        {
            // Gather information related to the provided type
            IsIntPtr = typeof(T) == typeof(IntPtr);
            RealType = typeof(T);
            Size = TypeCode == TypeCode.Boolean ? 1 : Marshal.SizeOf(RealType);
            TypeCode = Type.GetTypeCode(RealType);
            // Check if the type can be stored in registers
            CanBeStoredInRegisters =
                IsIntPtr ||
                TypeCode == TypeCode.Int64 ||
                TypeCode == TypeCode.UInt64 ||
                TypeCode == TypeCode.Boolean ||
                TypeCode == TypeCode.Byte ||
                TypeCode == TypeCode.Char ||
                TypeCode == TypeCode.Int16 ||
                TypeCode == TypeCode.Int32 ||
                TypeCode == TypeCode.Int64 ||
                TypeCode == TypeCode.SByte ||
                TypeCode == TypeCode.Single ||
                TypeCode == TypeCode.UInt16 ||
                TypeCode == TypeCode.UInt32;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Gets if the type can be stored in a registers (for example ACX, ECX, ...).
        /// </summary>
        public static bool CanBeStoredInRegisters { get; }

        /// <summary>
        ///     State if the type is <see cref="IntPtr" />.
        /// </summary>
        public static bool IsIntPtr { get; }

        /// <summary>
        ///     The real type.
        /// </summary>
        public static Type RealType { get; }

        /// <summary>
        ///     The size of the type.
        /// </summary>
        public static int Size { get; }

        /// <summary>
        ///     The typecode of the type.
        /// </summary>
        public static TypeCode TypeCode { get; }

        #endregion Public Properties, Indexers
    }
}