namespace Binarysharp.MemoryManagement.Core.Patterns.Objects
{
    /// <summary>
    ///     A class that represents basic pattern scanning properties.
    /// </summary>
    public class SerializablePattern
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Creates a new instance of <see cref="SerializablePattern" />.
        /// </summary>
        /// <param name="description">A description of the pattern being created.</param>
        /// <param name="pattern">The patterns Dword formatted text pattern.</param>
        /// <param name="offseToAdd">The offset to add to the result found before returning the value.</param>
        /// <param name="isOffsetMode">If we should return the address or offset in the result if this is a xml patternscan.</param>
        /// <param name="rebase">If the address should be rebased to a process module.</param>
        /// <param name="comment">Any comments about the pattern. Useful for xml files.</param>
        public SerializablePattern(string description, string pattern, int offseToAdd, bool isOffsetMode, bool rebase,
                                   string comment)
        {
            Description = description;
            TextPattern = pattern;
            OffsetToAdd = offseToAdd;
            IsOffsetMode = isOffsetMode;
            RebaseAddress = rebase;
            Comments = comment;
        }

        /// <summary>
        ///     Paramerterless constructor for xml serialization/deserialization simplicity.
        /// </summary>
        public SerializablePattern()
        {
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

        /// <summary>
        ///     A description of the pattern being scanned.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The Dword format text of the pattern.
        ///     <example>A2 5B ?? ?? ?? A2</example>
        /// </summary>
        public string TextPattern { get; set; }

        /// <summary>
        ///     The value to add to the offset result when the pattern is first found.
        /// </summary>
        public int OffsetToAdd { get; set; }

        /// <summary>
        ///     If the result should be from the address or offset.
        /// </summary>
        public bool IsOffsetMode { get; set; }

        /// <summary>
        ///     If the address should be rebased to a process modules address or not.
        /// </summary>
        public bool RebaseAddress { get; set; }

        /// <summary>
        ///     Tyhe type of pointer the scan results in.
        /// </summary>
        public string Comments { get; set; }

        #endregion Public Properties, Indexers
    }
}