using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks if a file is not in any playlist.
    /// </summary>
    public class Filter_noPlaylist : Filter_Base
    {
        /// <summary>
        /// Creates a new no playlist filter.
        /// </summary>
        public Filter_noPlaylist()
        {
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return !mediaFile.IsInPlaylist;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.NoPlaylist;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription(IFilter? parentFilter = null)
        {
            return "noPlaylist";
        }
    }
}