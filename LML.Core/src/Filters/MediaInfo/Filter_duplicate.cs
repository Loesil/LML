using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a file is a duplicate.
    /// </summary>
    public class Filter_duplicate : Filter_Base
    {
        /// <summary>
        /// Creates a new duplicate filter.
        /// </summary>
        public Filter_duplicate()
        {
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return mediaFile.IsDuplicate;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.Duplicate;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription(IFilter? parentFilter = null)
        {
            return "duplicate";
        }
    }
}