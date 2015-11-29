using System;
using System.Runtime.InteropServices;

namespace Binarysharp.MemoryManagement.Core.Native.Structs
{
    /// <summary>
    ///     Struct for marsahling unicode strings.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UnicodeString : IDisposable
    {
        /// <summary>
        ///     The buffer{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public IntPtr Buffer;

        /// <summary>
        ///     The length{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ushort Length;

        /// <summary>
        ///     The maximum length{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        public ushort MaximumLength;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnicodeString" /> struct.
        /// </summary>
        /// <param name="s">The s.</param>
        public UnicodeString(string s = null)
        {
            if (s != null)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                Buffer = Marshal.StringToHGlobalUni(s);
            }
            else
            {
                Length = 0;
                MaximumLength = 0;
                Buffer = IntPtr.Zero;
            }
        }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (Buffer != IntPtr.Zero) Marshal.FreeHGlobal(Buffer);
            Buffer = IntPtr.Zero;
        }

        /// <summary>
        ///     To the string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string ToString()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return Buffer != IntPtr.Zero ? Marshal.PtrToStringUni(Buffer) : string.Empty;
        }
    }
}