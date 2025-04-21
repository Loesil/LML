using LML.Core.Models;

namespace LML.Core.Filters
{
    /// <summary>
    /// Base class for media file filters with properties.
    /// </summary>
    public abstract class Filter_Property : Filter_Base
    {
        public readonly MediaProperty Property;
        public readonly MediaPropertyType SupportedType;

        public Filter_Property(MediaProperty property, MediaPropertyType supportedType)
        {
            Property = property;
            SupportedType = supportedType;
            if (!IsSupportedType()) throw new ArgumentException($"Property {property} is not of type {supportedType}");
        }

        /// <summary>
        /// Checks if the property type is supported.
        /// </summary>
        /// <param name="type">The property type</param>
        /// <returns>True if the property type is supported</returns>
        protected bool IsSupportedType()
        {
            return GetPropertyType() == SupportedType;
        }

        #region Property Info
        /// <summary>
        /// Gets the value of a property from a media file.
        /// </summary>
        /// <param name="mediaFile">The media file</param>
        /// <returns>The property value</returns>
        protected object? GetPropertyValue(MediaFile mediaFile)
        {
            return Property switch
            {
                // string
                MediaProperty.Path => mediaFile.FilePath,
                MediaProperty.Title => mediaFile.Title,
                MediaProperty.Album => mediaFile.Album,

                // string list
                MediaProperty.Artists => mediaFile.Artists,
                MediaProperty.Genres => mediaFile.Genres,
                MediaProperty.Tags => mediaFile.Tags,

                // uint
                MediaProperty.Track => mediaFile.AlbumTrack,

                // bool
                MediaProperty.Exists => mediaFile.FileExists,
                MediaProperty.Local => mediaFile.IsInLibrary,
                MediaProperty.Audio => mediaFile.Type == MediaType.Audio,
                MediaProperty.Video => mediaFile.Type == MediaType.Video,
                MediaProperty.UnknownArtist => mediaFile.Artists.Count == 0 || mediaFile.Artists[0] == "" || mediaFile.Artists[0] == "???",
                MediaProperty.InPlaylist => mediaFile.IsInPlaylist,

                _ => null
            };
        }

        /// <summary>
        /// Gets the type of a property from a media file.
        /// </summary>
        /// <returns>The property type</returns>
        protected MediaPropertyType? GetPropertyType()
        {
            return PropertyToPropertyType(Property);
        }

        public static MediaPropertyType? PropertyToPropertyType(MediaProperty property)
        {
            return property switch
            {
                // string
                MediaProperty.Path => MediaPropertyType.String,
                MediaProperty.Title => MediaPropertyType.String,
                MediaProperty.Album => MediaPropertyType.String,

                // string list
                MediaProperty.Artists => MediaPropertyType.StringList,
                MediaProperty.Genres => MediaPropertyType.StringList,
                MediaProperty.Tags => MediaPropertyType.StringList,

                // uint
                MediaProperty.Track => MediaPropertyType.Uint,

                // bool
                MediaProperty.Exists => MediaPropertyType.Boolean,
                MediaProperty.Local => MediaPropertyType.Boolean,
                MediaProperty.Audio => MediaPropertyType.Boolean,
                MediaProperty.Video => MediaPropertyType.Boolean,
                MediaProperty.UnknownArtist => MediaPropertyType.Boolean,
                MediaProperty.InPlaylist => MediaPropertyType.Boolean,

                _ => null
            };
        }

        /// <summary>
        /// Gets the name of a property.
        /// </summary>
        /// <returns>The property name</returns>
        public static string? PropertyToName(MediaProperty property)
        {
            return property switch
            {
                // string
                MediaProperty.Path => "path",
                MediaProperty.Title => "title",
                MediaProperty.Album => "album",

                // string list
                MediaProperty.Artists => "artists",
                MediaProperty.Genres => "genres",
                MediaProperty.Tags => "tags",

                // uint
                MediaProperty.Track => "track",

                // bool
                MediaProperty.Exists => "exists",
                MediaProperty.Local => "local",
                MediaProperty.Audio => "audio",
                MediaProperty.Video => "video",
                MediaProperty.UnknownArtist => "unknownArtist",
                MediaProperty.InPlaylist => "inPlaylist",

                _ => null
            };
        }

        /// <summary>
        /// Converts a property name to a property.
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>The property</returns> 
        public static MediaProperty? NameToProperty(string name)
        {
            return name switch
            {
                // string
                "path" => MediaProperty.Path,
                "title" => MediaProperty.Title,
                "album" => MediaProperty.Album,

                // string list
                "artists" => MediaProperty.Artists,
                "genres" => MediaProperty.Genres,
                "tags" => MediaProperty.Tags,

                // uint
                "track" => MediaProperty.Track,

                // bool
                "exists" => MediaProperty.Exists,
                "local" => MediaProperty.Local,
                "audio" => MediaProperty.Audio,
                "video" => MediaProperty.Video,
                "unknownArtist" => MediaProperty.UnknownArtist,
                "inPlaylist" => MediaProperty.InPlaylist,

                _ => null
            };
        }
        #endregion
    }
}