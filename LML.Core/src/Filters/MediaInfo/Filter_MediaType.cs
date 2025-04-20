using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Filter that checks the media type of a file.
    /// </summary>
    public class Filter_MediaType : Filter_Base
    {
        private readonly bool _isAudio;

        /// <summary>
        /// Creates a new media type filter.
        /// </summary>
        /// <param name="isAudio">True to filter for audio files, false for video files</param>
        public Filter_MediaType(bool isAudio)
        {
            _isAudio = isAudio;
        }

        /// <inheritdoc/>
        public override bool Apply(MediaFile mediaFile)
        {
            return _isAudio ? mediaFile.Type == MediaType.Audio : mediaFile.Type == MediaType.Video;
        }

        /// <inheritdoc/>
        public override FilterType GetFilterType()
        {
            return FilterType.MediaType;
        }

        /// <inheritdoc/>
        public override string GetFilterDescription()
        {
            return _isAudio ? "audio" : "video";
        }
    }
}