using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that combines multiple filters using logical OR.
    /// </summary>
    public class Filter_or : Filter_Base
    {
        private readonly List<IFilter> _filters;

        /// <summary>
        /// Creates a new OR filter.
        /// </summary>
        /// <param name="filters">The filters to combine</param>
        public Filter_or(params IFilter[] filters)
        {
            _filters = filters.ToList();
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return _filters.Any(filter => filter.Apply(mediaFile));
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.Or;
        }

        /// <inheritdoc/>
        public override bool ContainsFilterType(FilterType type)
        {
            return type == FilterType.Or || _filters.Any(filter => filter.ContainsFilterType(type));
        }

        /// <inheritdoc/>
        public override string GetFilterDescription()
        {
            if (_filters.Count == 0)
            {
                return "";
            }
            else if (_filters.Count == 1)
            {
                return _filters[0].GetFilterDescription();
            }

            return "(" + string.Join(" | ", _filters.Select(f => f.GetFilterDescription())) + ")";
        }
    }
}