using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Base class for media file filters.
    /// </summary>
    public abstract class Filter_Base : IFilter
    {
        /// <inheritdoc/>
        public abstract bool Apply(MediaFile mediaFile);

        /// <inheritdoc/>
        public abstract FilterType GetFilterType();

        /// <inheritdoc/>
        public virtual bool ContainsFilterType(FilterType type)
        {
            return GetFilterType() == type;
        }

        /// <inheritdoc/>
        public abstract string GetFilterDescription(IFilter? parentFilter = null);
    }
}