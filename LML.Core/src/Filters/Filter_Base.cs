using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Base class for media file filters.
    /// </summary>
    public abstract class Filter_Base : IFilter
    {
        /// <summary>
        /// Checks if a media file matches the filter criteria.
        /// </summary>
        /// <param name="mediaFile">The media file to check</param>
        /// <returns>True if the file matches the filter criteria</returns>
        public abstract bool Apply(MediaFile mediaFile);

        /// <summary>
        /// Gets the type of the filter.
        /// </summary>
        /// <returns>The filter type</returns>
        public abstract FilterType GetFilterType();

        /// <summary>
        /// Checks if the filter contains a specific filter type.
        /// </summary>
        /// <param name="type">The filter type to check for</param>
        /// <returns>True if the filter contains the specified type</returns>
        public virtual bool ContainsFilterType(FilterType type)
        {
            return GetFilterType() == type;
        }

        /// <summary>
        /// Gets a human-readable description of the filter.
        /// </summary>
        /// <returns>The filter description</returns>
        public abstract string GetFilterDescription();
    }
}