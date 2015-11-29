using Binarysharp.MemoryManagement.Core.Memory;
using Binarysharp.MemoryManagement.Core.Memory.Objects;
using Binarysharp.MemoryManagement.Core.Native.Structs;
using System;

namespace Binarysharp.MemoryManagement.Core.Native.Objects
{
    /// <summary>
    ///     Class representing the ProcessUpdateData Environment Block of a remote process.
    /// </summary>
    public class ManagedPeb64 : ProxyPointer
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ManagedPeb32" /> class.
        /// </summary>
        /// am>
        /// <param name="processHandle">The open handle to the process.</param>
        public ManagedPeb64(IntPtr processHandle)
            : base(
                processHandle,
                ExternalMemoryCore.NtQueryInformationProcess(processHandle).PebBaseAddress)
        {
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     Returns the read <see cref="Peb64" /> structure.
        ///     <remarks>
        ///         Currently, this is not a fully managed peb like the x32 bit version. However, for now until updated it
        ///         still provides a lot of information that is easily accessible about the peb for 64 bit and releated values.
        ///     </remarks>
        /// </summary>
        public Peb64 NativePeb => Read<Peb64>();

        /// <summary>
        ///     Returns the read <see cref="PebLdrData64" /> structure.
        ///     <remarks>
        ///         Currently, this is not a fully managed peb like the x32 bit version. However, for now until updated it
        ///         still provides a lot of information that is easily accessible about the peb for 64 bit and releated values.
        ///     </remarks>
        /// </summary>
        public PebLdrData64 NativePebLdrData => Read<PebLdrData64>((int)NativePeb.pLdr);

        #endregion Public Properties, Indexers
    }
}