namespace Binarysharp.MemoryManagement.Core.Helpers
{
    /// <summary>
    ///     Static helper used to create or get a singleton from another class.
    /// </summary>
    /// <typeparam name="T">The type to create or get a singleton.</typeparam>
    public static class Singleton<T> where T : new()
    {
        #region Fields, Private Properties

        /// <summary>
        ///     Gets the singleton of the given type.
        /// </summary>
        public static readonly T Instance = new T();

        #endregion Fields, Private Properties
    }
}