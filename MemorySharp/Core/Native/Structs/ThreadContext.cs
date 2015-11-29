using Binarysharp.MemoryManagement.Core.Native.Enums;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Represents a thread context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadContext
    {
        /// <summary>
        ///     Determines which registers are returned or set when using <see cref="SafeNativeMethods.GetThreadContext" /> or
        ///     <see cref="SafeNativeMethods.SetThreadContext" />.
        ///     If the context record is used as an INPUT parameter, then for each portion of the context record controlled by a
        ///     flag whose value is set, it is assumed that portion of the
        ///     context record contains valid context. If the context record is being used to modify a threads context, then only
        ///     that portion of the threads context will be modified.
        ///     If the context record is used as an INPUT/OUTPUT parameter to capture the context of a thread, then only those
        ///     portions of the thread's context corresponding to set flags will be returned.
        ///     The context record is never used as an OUTPUT only parameter.
        /// </summary>
        public ThreadContextFlags ContextFlags;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr0;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr1;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr2;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr3;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr6;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr7;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Eax;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint Ebp;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Ebx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Ecx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Edi;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Edx;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint EFlags;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint Eip;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Esi;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint Esp;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.ExtendedRegisters" />.
        ///     The format and contexts are processor specific.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] ExtendedRegisters;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.FloatingPoint" />.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)]
        public FloatingSaveArea FloatingSave;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint SegCs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegDs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegEs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegFs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegGs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint SegSs;
    }
}