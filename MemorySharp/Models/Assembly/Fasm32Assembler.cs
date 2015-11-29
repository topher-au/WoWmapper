//using Binarysharp.Assemblers.Fasm;
using Binarysharp.MemoryManagement.Core.CallingConvention.Interfaces;
using System;

namespace Binarysharp.MemoryManagement.Models.Assembly
{
    /// <summary>
    ///     Implement Fasm.NET compiler for 32-bit development.
    ///     More info: https://github.com/ZenLulz/Fasm.NET
    /// </summary>
    public class Fasm32Assembler : IAssembler
    {
        #region Interface Implementations

        /// <summary>
        ///     Assemble the specified assembly code.
        /// </summary>
        /// <param name="asm">The assembly code.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble(string asm)
        {
            // Assemble and return the code
            return Assemble(asm, IntPtr.Zero);
        }

        /// <summary>
        ///     Assemble the specified assembly code at a base address.
        /// </summary>
        /// <param name="asm">The assembly code.</param>
        /// <param name="baseAddress">The address where the code is rebased.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble(string asm, IntPtr baseAddress)
        {
            // Rebase the code
            asm = $"use32\norg 0x{baseAddress.ToInt64():X8}\n" + asm;
            // Assemble and return the code
            return new byte[1]; // FasmNet.Assemble(asm);
        }

        #endregion Interface Implementations
    }
}