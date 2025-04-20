namespace LML.Core.Models
{
    /// <summary>
    /// Represents the type of a filter.
    /// </summary>
    public enum FilterType
    {
        // Logical operators
        And,
        Or,
        Not,

        // String filters
        StringEqual,
        StringContains,

        // String list filters
        StringListEqual,
        StringListContains,
        StringListContainsPart,

        // Uint filters
        UintEqual,
        UintNotEqual,
        UintGreater,
        UintGreaterEqual,
        UintLess,
        UintLessEqual,

        // Other filters
        MediaType,
        FileInfo,
        Duplicate,
        NoPlaylist,
        HasVariations,
        OneVariation,
        HasSubTracks,

        // Bool filters
        Bool
    }
}