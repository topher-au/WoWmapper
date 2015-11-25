using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement.Core.Native;

namespace Binarysharp.MemoryManagement.Core.Marshaling
{
    /// <summary>
    ///     Static class providing tools for extracting information related to types in an unsafe manner.
    /// </summary>
    /// <typeparam name="T">Type to analyse.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
    public static class UnsafeMarshal<T>
    {
        #region Fields, Private Properties
        /// <summary> The size of the Type </summary>
        public static int Size;

        /// <summary> The size of the Type </summary>
        public static uint SizeU;

        /// <summary> The real, underlying type. </summary>
        public static Type RealType;

        /// <summary> The type code </summary>
        public static TypeCode TypeCode;

        /// <summary> True if this type requires the Marshaler to map variables. (No direct pointer dereferencing) </summary>
        public static bool TypeRequiresMarshal;

        /// <summary>
        ///     Determines if the value is an <see cref="IntPtr" /> or not.
        /// </summary>
        public static bool IsIntPtr;

        /// <summary>
        ///     The get unsafe PTR.
        /// </summary>
        internal static readonly GetUnsafePtrDelegate GetUnsafePtr;
        #endregion

        #region Constructors, Destructors
        /// <summary>
        ///     Initializes static members of the <see cref="UnsafeMarshal{T}" /> class.
        /// </summary>
        static UnsafeMarshal()
        {
            TypeCode = Type.GetTypeCode(typeof (T));

            // Bools = 1 char.
            if (typeof (T) == typeof (bool))
            {
                Size = 1;
                RealType = typeof (T);
            }
            else if (typeof (T).IsEnum)
            {
                var underlying = typeof (T).GetEnumUnderlyingType();
                Size = Marshal.SizeOf(underlying);
                RealType = underlying;
                TypeCode = Type.GetTypeCode(underlying);
            }
            else
            {
                Size = Marshal.SizeOf(typeof (T));
                RealType = typeof (T);
            }

            IsIntPtr = RealType == typeof (IntPtr);

            SizeU = (uint) Size;

            Debug.Write("[SafeMarshal] " + typeof (T) + " Size: " + SizeU);

            // Basically, if any members of the type have a MarshalAs attrib, then we can't just pointer deref. :(
            // This literally means any kind of MarshalAs. Strings, arrays, custom type sizes, etc.
            // Ideally, we want to avoid the Marshaler as much as possible. It causes a lot of overhead, and for a memory reading
            // lib where we need the best speed possible, we do things manually when possible!
            TypeRequiresMarshal =
                RealType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any(
                    m => m.GetCustomAttributes(typeof (MarshalAsAttribute), true).Any());

            // Generate a method to get the address of a generic type. We'll be using this for RtlMoveMemory later for much faster structure reads.
            var method = new DynamicMethod($"GetPinnedPtr<{typeof (T).FullName.Replace(".", "<>")}>",
                typeof (void*), new[] {typeof (T).MakeByRefType()}, typeof (UnsafeMarshal<>).Module);
            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Conv_U);
            generator.Emit(OpCodes.Ret);
            GetUnsafePtr = (GetUnsafePtrDelegate) method.CreateDelegate(typeof (GetUnsafePtrDelegate));
        }
        #endregion

        /// <summary>
        ///     Converts a <see cref="IntPtr" /> to a structure.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>T.</returns>
        /// <exception cref="InvalidOperationException">Cannot retrieve a value at address 0</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static unsafe T PointerToStructure(IntPtr address)
        {
            try
            {
                if (address == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Cannot retrieve a value at address 0");
                }

                object ptrToStructure;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (TypeCode)
                {
                    case TypeCode.Object:

                        if (IsIntPtr)
                        {
                            return (T) (object) *(IntPtr*) address;
                        }

                        // If the type doesn't require an explicit Marshal call, then ignore it and memcpy the fuckin thing.
                        if (!TypeRequiresMarshal)
                        {
                            var o = default(T);
                            var ptr = GetUnsafePtr(ref o);

                            UnsafeNativeMethods.MoveMemory(ptr, (void*) address, Size);

                            return o;
                        }

                        // All System.Object's require marshaling!
                        ptrToStructure = Marshal.PtrToStructure(address, typeof (T));
                        break;
                    case TypeCode.Boolean:
                        ptrToStructure = *(byte*) address != 0;
                        break;
                    case TypeCode.Char:
                        ptrToStructure = *(char*) address;
                        break;
                    case TypeCode.SByte:
                        ptrToStructure = *(sbyte*) address;
                        break;
                    case TypeCode.Byte:
                        ptrToStructure = *(byte*) address;
                        break;
                    case TypeCode.Int16:
                        ptrToStructure = *(short*) address;
                        break;
                    case TypeCode.UInt16:
                        ptrToStructure = *(ushort*) address;
                        break;
                    case TypeCode.Int32:
                        ptrToStructure = *(int*) address;
                        break;
                    case TypeCode.UInt32:
                        ptrToStructure = *(uint*) address;
                        break;
                    case TypeCode.Int64:
                        ptrToStructure = *(long*) address;
                        break;
                    case TypeCode.UInt64:
                        ptrToStructure = *(ulong*) address;
                        break;
                    case TypeCode.Single:
                        ptrToStructure = *(float*) address;
                        break;
                    case TypeCode.Double:
                        ptrToStructure = *(double*) address;
                        break;
                    case TypeCode.Decimal:
                        ptrToStructure = *(decimal*) address;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return (T) ptrToStructure;
            }
            catch (AccessViolationException)
            {
                Trace.WriteLine("Access Violation on " + address + " with type " + typeof (T).Name);
                return default(T);
            }
        }

        /// <summary>
        ///     Structures to pointer.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="defaultValue">The value.</param>
        public static unsafe void StructureToPointer(IntPtr address, T defaultValue = default(T))
        {
            var pointer = address.ToPointer();

            object structureToPtr = defaultValue;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (TypeCode)
            {
                case TypeCode.Boolean:
                    *(bool*) pointer = (bool) structureToPtr;
                    break;

                case TypeCode.Byte:
                    *(byte*) pointer = (byte) structureToPtr;
                    break;

                case TypeCode.SByte:
                    *(sbyte*) pointer = (sbyte) structureToPtr;
                    break;

                case TypeCode.Char:
                    *(char*) pointer = (char) structureToPtr;
                    break;

                case TypeCode.Int16:
                    *(short*) pointer = (short) structureToPtr;
                    break;

                case TypeCode.UInt16:
                    *(ushort*) pointer = (ushort) structureToPtr;
                    break;

                case TypeCode.Int32:
                    *(int*) pointer = (int) structureToPtr;
                    break;

                case TypeCode.UInt32:
                    *(uint*) pointer = (uint) structureToPtr;
                    break;

                case TypeCode.Int64:
                    *(long*) pointer = (long) structureToPtr;
                    break;

                case TypeCode.UInt64:
                    *(ulong*) pointer = (ulong) structureToPtr;
                    break;

                case TypeCode.Single:
                    *(float*) pointer = (float) structureToPtr;
                    break;

                case TypeCode.Double:
                    *(double*) pointer = (double) structureToPtr;
                    break;

                default:
                    // Assume the pointer is a custom structure.
                    // https://msdn.microsoft.com/en-us/library/vstudio/4ca6d5z7(v=vs.100).aspx for more dtails on PtrToStructure.
                    Marshal.StructureToPtr(structureToPtr, address, true);
                    break;
            }
        }

        /// <summary>
        ///     Converts the given array of bytes to the specified type.
        ///     Uses either marshalling or unsafe code, depending on UseUnsafeReadWrite
        /// </summary>
        /// <param name="data">Array of bytes</param>
        /// <param name="defVal">The default value of this operation (which is returned in case the Read-operation fails)</param>
        /// <returns></returns>
        public static unsafe T ByteArrayToObject(byte[] data, T defVal = default(T))
        {
            T structure;
            fixed (byte* b = data)
                structure = (T) Marshal.PtrToStructure((IntPtr) b, typeof (T));
            return structure;
        }

        /// <summary>
        ///     Converts the given array of bytes to the specified type.
        ///     Uses either marshalling or unsafe code, depending on UseUnsafeReadWrite
        /// </summary>
        /// <param name="data">Array of bytes</param>
        /// <param name="index">Index of the data to convert</param>
        /// <param name="defVal">The default value of this operation (which is returned in case the Read-operation fails)</param>
        /// <returns></returns>
        public static T ByteArrayToObject(byte[] data, int index, T defVal = default(T))
        {
            var size = Marshal.SizeOf(typeof (T));
            var tmp = new byte[size];
            Array.Copy(data, index, tmp, 0, size);
            return ByteArrayToObject(tmp, defVal);
        }

        /// <summary>
        ///     Converts the given struct to a byte-array
        /// </summary>
        /// <param name="value">Value to conver to bytes</param>
        /// <returns></returns>
        public static unsafe byte[] ObjectToByteArray(T value)
        {
            var size = Marshal.SizeOf(typeof (T));
            var data = new byte[size];
            fixed (byte* b = data)
                Marshal.StructureToPtr(value, (IntPtr) b, true);
            return data;
        }

        /// <summary>
        ///     Delegate GetUnsafePtrDelegate{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Void*.</returns>
        internal unsafe delegate void* GetUnsafePtrDelegate(ref T value);
    }
}