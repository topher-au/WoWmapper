namespace Binarysharp.MemoryManagement.Core.CallingConvention.Enums
{
    /// <summary>
    ///     A list of calling conventions.
    /// </summary>
    public enum CallingConventions
    {
        /// <summary>
        ///     Name       : C Declaration Calling Convention
        ///     Clean-up   : Caller
        ///     Parameters : Passed on the stack in reverse order
        ///     Ret. value : Returned in the EAX register
        ///     Notes      : Widely used by the compiler GCC
        /// </summary>
        Cdecl,

        /// <summary>
        ///     Name       : Standard Calling Convention
        ///     Clean-up   : Callee
        ///     Parameters : Passed on the stack in reverse order
        ///     Ret. value : Returned in the EAX register
        ///     Notes      : Convention created by Microsoft, used in the Win32 API
        /// </summary>
        Stdcall,

        /// <summary>
        ///     Name       : Fast Calling Convention (aka __msfastcall)
        ///     Clean-up   : Callee
        ///     Parameters : The first two parameters are placed in the ECX and EDX registers respectively.
        ///     Any remaining parameters are placed on the stack in reverse order.
        ///     Ret. Value : Returned in the EAX register
        ///     Notes      : A variation of the stdcall convention
        /// </summary>
        Fastcall,

        /// <summary>
        ///     Name       : This Calling Convention
        ///     Clean-up   : Callee
        ///     Parameters : The 'this' pointer is placed in the ECX register.
        ///     Parameters are placed on the stack in reverse order.
        ///     Ret. Value : Returned in the EAX register
        ///     Notes      : Used for object-oriented programming by Microsoft Visual C++
        /// </summary>
        Thiscall
    }
}