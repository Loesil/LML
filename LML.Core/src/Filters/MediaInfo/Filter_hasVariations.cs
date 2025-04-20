using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a file has variations.
    /// </summary>
    public class Filter_hasVariations : Filter_Base
    {
        /// <summary>
        /// Creates a new has variations filter.
        /// </summary>
        public Filter_hasVariations()
        {
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return mediaFile.Variations?.Count > 0;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.HasVariations;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription()
        {
            return "hasVariations";
        }
    }
}