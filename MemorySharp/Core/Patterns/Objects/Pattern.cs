namespace Binarysharp.MemoryManagement.Core.Patterns.Objects
{
    /// <summary>
    ///     A class that represents basic pattrn scanning properties.
    /// </summary>
    public class Pattern
    {
        #region Constructors, Destructors

        /// <summary>
        ///     Creates a new instance of <see cref="Pattern" />.
        /// </summary>
        /// <param name="pattern">The patterns Dword formatted text pattern.</param>
        /// <param name="offseToAdd">The offset to add to the result found before returning the value.</param>
        /// <param name="isOffsetMode">If we should return the address or offset in the result if this is a xml patternscan.</param>
        /// <param name="rebase">If the address should be rebased to a process module.</param>
        public Pattern(string pattern, int offseToAdd, bool isOffsetMode, bool rebase)
        {
            TextPattern = pattern;
            OffsetToAdd = offseToAdd;
            IsOffsetMode = isOffsetMode;
            RebaseAddress = rebase;
        }

        #endregion Constructors, Destructors

        #region Public Properties, Indexers

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
        ///     Gets the mask from the text pattern in this instance.
        /// </summary>
        /// <returns>The mask from the pattern.</returns>
        public string Mask => PatternCore.GetMaskFromDwordPattern(TextPattern);

        /// <summary>
        ///     Gets the <code>byte[]</code> pattern from text pattern in this instance.
        /// </summary>
        /// <returns>An array of bytes.</returns>
        public byte[] PatternBytes => PatternCore.GetBytesFromDwordPattern(TextPattern);

        #endregion Public Properties, Indexers
    }
}