using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Interface for media file filters.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Checks if a media file matches the filter criteria.
        /// </summary>
        /// <param name="mediaFile">The media file to check</param>
        /// <returns>True if the file matches the filter criteria</returns>
        bool Apply(MediaFile mediaFile);

        /// <summary>
        /// Gets the type of the filter.
        /// </summary>
        /// <returns>The filter type</returns>
        FilterType GetFilterType();

        /// <summary>
        /// Checks if the filter contains a specific filter type.
        /// </summary>
        /// <param name="type">The filter type to check for</param>
        /// <returns>True if the filter contains the specified type</returns>
        bool ContainsFilterType(FilterType type);

        /// <summary>
        /// Gets a human-readable description of the filter.
        /// </summary>
        /// <returns>The filter description</returns>
        string GetFilterDescription();
    }
}