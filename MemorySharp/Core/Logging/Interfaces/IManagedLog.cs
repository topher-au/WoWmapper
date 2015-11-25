using Binarysharp.MemoryManagement.Core.Managment.Interfaces;

namespace Binarysharp.MemoryManagement.Core.Logging.Interfaces
{
    /// <summary>
    ///     Interface IManagedLog defines a log which is part of a pool of other <see cref="ILog" /> members.
    /// </summary>
    public interface IManagedLog : ILog, INamedElement
    {
    }
}