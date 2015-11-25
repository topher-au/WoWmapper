namespace Binarysharp.MemoryManagement.Core.Managment.Interfaces
{
    /// <summary>
    ///     Defines an element able to be activated in the remote process.
    /// </summary>
    public interface IApplicableElement : IDisposableState
    {
        #region Public Properties, Indexers
        /// <summary>
        ///     States if the element is enabled.
        /// </summary>
        bool IsEnabled { get; }
        #endregion

        /// <summary>
        ///     Disables the element.
        /// </summary>
        void Disable();

        /// <summary>
        ///     Enables the element.
        /// </summary>
        void Enable();
    }
}