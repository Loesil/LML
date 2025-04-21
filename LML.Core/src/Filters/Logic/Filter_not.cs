using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that negates another filter.
    /// </summary>
    public class Filter_not : Filter_Base
    {
        private readonly IFilter _filter;

        /// <summary>
        /// Creates a new NOT filter.
        /// </summary>
        /// <param name="filter">The filter to negate</param>
        public Filter_not(IFilter filter)
        {
            _filter = filter;
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return !_filter.Apply(mediaFile);
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.Not;
        }

        /// <inheritdoc/>
        public override bool ContainsFilterType(FilterType type)
        {
            return type == FilterType.Not || _filter.ContainsFilterType(type);
        }

        /// <inheritdoc/>
        public override string GetFilterDescription(IFilter? parentFilter = null)
        {
            return $"!{_filter.GetFilterDescription(this)}";
        }
    }
}