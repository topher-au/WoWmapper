using System;

namespace Binarysharp.MemoryManagement.Core.Managment.Interfaces
{
    /// <summary>
    ///     Define a factory for the library.
    /// </summary>
    /// <remarks>At the moment, the factories are just disposable.</remarks>
    public interface IFactory : IDisposable
    {
        //TODO: Investigate possible useful additons to this interface.
    }
}