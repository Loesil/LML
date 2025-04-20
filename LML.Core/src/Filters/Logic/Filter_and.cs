using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that combines multiple filters using logical AND.
    /// </summary>
    public class Filter_and : Filter_Base
    {
        private readonly List<IFilter> _filters;

        /// <summary>
        /// Creates a new AND filter.
        /// </summary>
        /// <param name="filters">The filters to combine</param>
        public Filter_and(params IFilter[] filters)
        {
            _filters = filters.ToList();
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return _filters.All(filter => filter.Apply(mediaFile));
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.And;
        }

        /// <inheritdoc/>
        public override bool ContainsFilterType(FilterType type)
        {
            return type == FilterType.And || _filters.Any(filter => filter.ContainsFilterType(type));
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

            return "(" + string.Join(" & ", _filters.Select(f => f.GetFilterDescription())) + ")";
        }
    }
}