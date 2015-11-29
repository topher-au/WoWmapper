namespace Binarysharp.MemoryManagement.Core.Managment.Interfaces
{
    /// <summary>
    ///     Defines a element with a name.
    /// </summary>
    public interface INamedElement : IApplicableElement
    {
        #region Public Properties, Indexers

        /// <summary>
        ///     The name of the element.
        /// </summary>
        string Name { get; }

        #endregion Public Properties, Indexers
    }
}