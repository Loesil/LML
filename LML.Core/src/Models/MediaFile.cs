using System.Text.Json.Serialization;
using TagLibFile = TagLib.File;
using System.Diagnostics;
using LML.Core.Services;

namespace LML.Core.Models
{
    #region Enums
    /// <summary>
    /// Represents the type of media file.
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// Unknown media type.
        /// </summary>
        Unknown,
        /// <summary>
        /// Audio media type.
        /// </summary>
        Audio,
        /// <summary>
        /// Video media type.
        /// </summary>
        Video
    }

    /// <summary>
    /// Represents the properties that can be used for filtering media files.
    /// </summary>
    public enum MediaProperty
    {
        // string
        /// <summary>
        /// The file path property.
        /// </summary>
        Path,
        /// <summary>
        /// The title property.
        /// </summary>
        Title,
        /// <summary>
        /// The album property.
        /// </summary>
        Album,

        // string list
        /// <summary>
        /// The artists property.
        /// </summary>
        Artists,
        /// <summary>
        /// The genres property.
        /// </summary>
        Genres,
        /// <summary>
        /// The tags property.
        /// </summary>
        Tags,

        // uint
        /// <summary>
        /// The track number property.
        /// </summary>
        Track,

        // bool
        /// <summary>
        /// Whether the file is local.
        /// </summary>
        Local,
        /// <summary>
        /// Whether the file exists.
        /// </summary>
        Exists,
        /// <summary>
        /// Whether the file is an audio file.
        /// </summary>
        Audio,
        /// <summary>
        /// Whether the file is a video file.
        /// </summary>
        Video,
        /// <summary>
        /// Whether the artist is unknown.
        /// </summary>
        UnknownArtist
    }

    /// <summary>
    /// Represents the type of a media property.
    /// </summary>
    public enum MediaPropertyType
    {
        /// <summary>
        /// String property type.
        /// </summary>
        String,
        /// <summary>
        /// String list property type.
        /// </summary>
        StringList,
        /// <summary>
        /// Unsigned integer property type.
        /// </summary>
        Uint,
        /// <summary>
        /// Boolean property type.
        /// </summary>
        Boolean
    }
    #endregion

    #region Json Type
    /// <summary>
    /// Represents the JSON data structure for a media file.
    /// </summary>
    public class MediaFileJsonData
    {
        #region Properties
        #region Base
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [JsonPropertyName("path")]
        public string FilePath { get; set; }
        #endregion

        #region Metadata
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [JsonPropertyName("title")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the artists.
        /// </summary>
        [JsonPropertyName("artists")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Artists { get; set; }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        [JsonPropertyName("album")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Album { get; set; }

        /// <summary>
        /// Gets or sets the album track number.
        /// </summary>
        [JsonPropertyName("track")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public uint? AlbumTrack { get; set; }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        [JsonPropertyName("genres")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Genres { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        [JsonPropertyName("tags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Tags { get; set; }
        #endregion
        #endregion

        #region Create
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFileJsonData"/> class.
        /// </summary>
        public MediaFileJsonData()
        {
            FilePath = "";
            Title = "";
            Artists = "";
            Album = "";
            AlbumTrack = 0;
            Genres = "";
            Tags = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFileJsonData"/> class from a <see cref="MediaFile"/>.
        /// </summary>
        /// <param name="m">The media file to create the JSON data from.</param>
        public MediaFileJsonData(MediaFile m)
        {
            FilePath = m.FilePath;
            Title = m.Title;
            Artists = CheckEmpty(m.Artists);
            Album = CheckEmpty(m.Album);
            AlbumTrack = m.AlbumTrack == 0 ? null : m.AlbumTrack;
            Genres = CheckEmpty(m.Genres);
            Tags = CheckEmpty(m.Tags);
        }
        #endregion

        #region Helper
        /// <summary>
        /// Checks if a string value is empty or "???" and returns null if so.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>Null if the value is empty or "???", otherwise the value.</returns>
        public static string? CheckEmpty(string value)
        {
            return (value == "" || value == "???" ? null : value);
        }

        /// <summary>
        /// Checks if a list of strings is empty and returns null if so.
        /// </summary>
        /// <param name="value">The list of strings to check.</param>
        /// <returns>Null if the list is empty, otherwise the joined string.</returns>
        public static string? CheckEmpty(List<string> value)
        {
            return CheckEmpty(string.Join(", ", value));
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Represents a media file in the library.
    /// </summary>
    public class MediaFile
    {
        #region Static
        /// <summary>
        /// List of supported audio file extensions.
        /// </summary>
        public static List<string> AudioExtensions = new List<string> { ".mp3", ".wav", ".flac", ".m4a", ".wma", ".aac", ".ogg", ".wav" };

        /// <summary>
        /// List of supported video file extensions.
        /// </summary>
        public static List<string> VideoExtensions = new List<string> { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".m4v" };
        #endregion

        #region Properties
        #region Base
        /// <summary>
        /// Gets or sets the media library service.
        /// </summary>
        public IMediaLibraryService MediaLibraryService { get; set; }

        /// <summary>
        /// Gets the unique identifier of the media file.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value.Replace("\\", "/");
                UpdateFileInfo();
            }
        }

        /// <summary>
        /// Gets the media type.
        /// </summary>
        public MediaType Type { get; private set; }
        #endregion

        #region Metadata
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                MediaLibraryService.NotifyUnsavedChanges();
            }
        }

        /// <summary>
        /// Gets or sets the artists.
        /// </summary>
        public List<string> Artists
        {
            get => _artists;
            set
            {
                MediaLibraryService.UpdateCollections(this, false);
                _artists = value;
                MediaLibraryService.UpdateCollections(this, true);
                MediaLibraryService.NotifyUnsavedChanges();
            }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        public string Album
        {
            get => _album;
            set
            {
                MediaLibraryService.UpdateCollections(this, false);
                _album = value;
                MediaLibraryService.UpdateCollections(this, true);
                MediaLibraryService.NotifyUnsavedChanges();
            }
        }

        /// <summary>
        /// Gets or sets the album track number.
        /// </summary>
        public uint? AlbumTrack
        {
            get => _albumTrack;
            set
            {
                _albumTrack = value;
                MediaLibraryService.NotifyUnsavedChanges();
            }
        }

        /// <summary>
        /// Gets or sets the genres.
        /// </summary>
        public List<string> Genres
        {
            get => _genres;
            set
            {
                MediaLibraryService.UpdateCollections(this, false);
                _genres = value;
                MediaLibraryService.UpdateCollections(this, true);
                MediaLibraryService.NotifyUnsavedChanges();
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public List<string> Tags
        {
            get => _tags;
            set
            {
                MediaLibraryService.UpdateCollections(this, false);
                _tags = value;
                MediaLibraryService.UpdateCollections(this, true);
                MediaLibraryService.NotifyUnsavedChanges();
            }
        }
        #endregion

        #region Relations
        /// <summary>
        /// Gets or sets the list of variations of this media file.
        /// </summary>
        public List<MediaFile> Variations { get; set; } = new List<MediaFile>();

        /// <summary>
        /// Gets or sets whether this file is a duplicate.
        /// </summary>
        public bool IsDuplicate { get; set; }

        /// <summary>
        /// Gets or sets whether this file is in a playlist.
        /// </summary>
        public bool IsInPlaylist { get; set; }

        /// <summary>
        /// Gets whether the file exists on disk.
        /// </summary>
        public bool FileExists { get; private set; }

        /// <summary>
        /// Gets whether the file is in the library.
        /// </summary>
        public bool IsInLibrary { get; private set; }
        #endregion

        #region Calculated
        /// <summary>
        /// Gets the full file path.
        /// </summary>
        public string FullFilePath
        {
            get
            {
                string p = FilePath;
                if (p.StartsWith("./"))
                    p = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(MediaLibraryService.Path)!, p)).Replace("\\", "/");
                return p;
            }
        }

        /// <summary>
        /// Gets or sets the stringified artists.
        /// </summary>
        public string Stringified_Artists
        {
            get
            {
                return Artists.Count == 0 ? "???" : string.Join(", ", Artists);
            }

            set
            {
                Artists = value.Replace(";", ",").Split(',').ToList().Select(a => a.Trim()).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the stringified genres.
        /// </summary>
        public string Stringified_Genres
        {
            get
            {
                return string.Join(", ", Genres);
            }

            set
            {
                Genres = value.Replace(";", ",").Split(',').ToList().Select(g => g.Trim()).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the stringified tags.
        /// </summary>
        public string Stringified_Tags
        {
            get
            {
                return string.Join(", ", Tags);
            }

            set
            {
                Tags = value.Replace(";", ",").Split(',').ToList().Select(t => t.Trim()).ToList();
            }
        }

        /// <summary>
        /// Gets or sets the stringified album track.
        /// </summary>
        public string Stringified_AlbumTrack
        {
            get
            {
                if (AlbumTrack == 0) return "";
                return AlbumTrack?.ToString() ?? "";
            }

            set
            {
                if (value != ""
                    && uint.TryParse(value, out uint r)
                    && r != 0)
                {
                    AlbumTrack = r;
                }
                else
                    AlbumTrack = null;
            }
        }
        #endregion
        #endregion

        #region Private Properties
        private string _filePath = "";
        private string _title = "";
        private List<string> _artists = new List<string>();
        private string _album = "";
        private uint? _albumTrack = null;
        private List<string> _genres = new List<string>();
        private List<string> _tags = new List<string>();
        #endregion

        #region Create
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFile"/> class.
        /// </summary>
        /// <param name="service">The media library service.</param>
        /// <param name="filePath">The file path.</param>
        public MediaFile(IMediaLibraryService service, string filePath)
        {
            MediaLibraryService = service;

            // Base
            Id = Guid.NewGuid();
            FilePath = filePath;
            Type = DetermineMediaTypeFromFile(FullFilePath);

            // Metadata
            _title = "???";
            _album = "";
            _albumTrack = null;

            // Relations
            Variations = new List<MediaFile>();
            IsDuplicate = false;
            IsInPlaylist = false;
        }
        #endregion

        #region Methods
        #region Metadata
        /// <summary>
        /// Saves the metadata to the file.
        /// </summary>
        public void SaveMetadata()
        {
            using var file = TagLibFile.Create(FullFilePath);
            file.Tag.Title = Title;
            file.Tag.AlbumArtists = Artists.ToArray();
            file.Tag.Album = Album;
            if (AlbumTrack != null) file.Tag.Track = (uint)AlbumTrack!;
            file.Tag.Genres = Genres.ToArray();
            //file.Tag.Year = (uint)DateTime.Now.Year;
            file.Tag.Comment = "#" + string.Join(", ", Tags);
            file.Save();

            if (!VerifyMetadata())
                throw new Exception($"Metadata verification failed for [{FilePath}]");
        }

        /// <summary>
        /// Verifies that the metadata in the file matches the current metadata.
        /// </summary>
        /// <returns>True if the metadata matches, false otherwise.</returns>
        public bool VerifyMetadata()
        {
            try
            {
                using var file = TagLibFile.Create(FullFilePath);

                // Extract metadata and compare
                if (Title != (string.IsNullOrEmpty(file.Tag.Title)
                    ? Path.GetFileNameWithoutExtension(FilePath)
                    : file.Tag.Title)) return false;

                if (string.Join(", ", Artists) != string.Join(", ", file.Tag.AlbumArtists.ToList())) return false;
                if (Album != (file.Tag.Album ?? Album)) return false;
                if (AlbumTrack != (file.Tag.Track == 0 ? null : file.Tag.Track)) return false;
                if (string.Join(", ", Genres) != string.Join(", ", file.Tag.Genres.ToList())) return false;
                if (file.Tag.Comment != null && file.Tag.Comment.StartsWith("#"))
                    if (string.Join(", ", Tags) != string.Join(", ", file.Tag.Comment.Substring(1).Split(',').ToList().Select(t => t.Trim()))) return false;

                // mediaFile.Metadata["Year"] = file.Tag.Year.ToString();

                return true;
            }
            catch (Exception ex)
            {
                // Log error and set basic metadata
                Console.WriteLine($"Error extracting metadata for {FilePath}: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Extracts metadata from the file.
        /// </summary>
        public void ExtractMetadata()
        {
            try
            {
                using var file = TagLibFile.Create(FullFilePath);

                // Extract metadata
                Title = string.IsNullOrEmpty(file.Tag.Title) ?
                    Path.GetFileNameWithoutExtension(FilePath) : file.Tag.Title;
                Artists = file.Tag.AlbumArtists.ToList();
                Album = file.Tag.Album ?? Album;
                AlbumTrack = file.Tag.Track == 0 ? null : file.Tag.Track;
                Genres = file.Tag.Genres.ToList();
                if (file.Tag.Comment != null && file.Tag.Comment.StartsWith("#"))
                    Tags = file.Tag.Comment.Substring(1).Split(',').ToList().Select(t => t.Trim()).ToList();

                // mediaFile.Metadata["Year"] = file.Tag.Year.ToString();
            }
            catch (Exception ex)
            {
                // Log error and set basic metadata
                Console.WriteLine($"Error extracting metadata for {FilePath}: {ex.Message}");
            }
        }
        #endregion

        #region MediaType
        /// <summary>
        /// Determines the media type from the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The media type.</returns>
        public static MediaType DetermineMediaTypeFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return MediaType.Unknown;

            // MIME type
            using var file = TagLibFile.Create(filePath);
            string mimeType = file.MimeType.ToLower();
            MediaType type = mimeType switch
            {
                var mt when mt.StartsWith("audio/") => MediaType.Audio,
                var mt when mt.StartsWith("video/") => MediaType.Video,
                _ => MediaType.Unknown
            };

            // extension
            if (type == MediaType.Unknown)
            {
                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                type = MediaType.Unknown;
                if (AudioExtensions.Contains(extension)) type = MediaType.Audio;
                if (VideoExtensions.Contains(extension)) type = MediaType.Video;
            }
            return type;
        }
        #endregion

        #region Playlist
        /// <summary>
        /// Gets the M3U entry for this media file.
        /// </summary>
        /// <returns>The M3U entry.</returns>
        public string GetM3UEntry()
        {
            // Format: #EXTINF:duration,artist - title
            //         file_path
            int duration = -1; // Duration.TotalSeconds == 0 ? -1 : (int)Duration.TotalSeconds;
            string info = MediaFileJsonData.CheckEmpty(Artists) == null
                ? Title
                : $"{Stringified_Artists} - {Title}";

            return $"#EXTINF:{duration},{info}\n{FilePath}";
        }
        #endregion

        /// <summary>
        /// Updates the file information.
        /// </summary>
        public void UpdateFileInfo()
        {
            // exists
            FileExists = File.Exists(FullFilePath);

            // is in lib
            string rel = Path.GetRelativePath(Path.GetDirectoryName(MediaLibraryService.Path)!, FullFilePath);
            string relBase = MediaLibraryService.Pattern.Substring(0, MediaLibraryService.Pattern.IndexOf("{"));
            IsInLibrary = !rel.StartsWith("..") && rel[1] != ':' && !rel.StartsWith("\\\\") && rel.Replace("\\", "/").StartsWith(relBase);
        }

        /// <summary>
        /// Checks if this file is the preferred variation.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        /// <param name="caseSensitive">Whether to perform case-sensitive comparison.</param>
        /// <returns>True if this file is the preferred variation.</returns>
        public bool IsPreferredVariation(string value, bool caseSensitive = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Variations.Count == 1;
            }

            var comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            return FilePath.Contains(value, comparison);
        }
        #endregion

        #region Playback
        /// <summary>
        /// Plays the media file.
        /// </summary>
        public void Play()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = FullFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing file {FilePath}: {ex.Message}");
            }
        }
        #endregion

        /// <summary>
        /// Creates a clone of this media file.
        /// </summary>
        /// <param name="service">The media library service.</param>
        /// <param name="useFullPath">Whether to use the full file path.</param>
        /// <returns>The cloned media file.</returns>
        public MediaFile Clone(IMediaLibraryService service, bool useFullPath = false)
        {
            MediaFile clone = new MediaFile(service, useFullPath ? FullFilePath : FilePath);
            clone.Title = Title;
            clone.Artists = Artists.ToList();
            clone.Album = Album;
            clone.AlbumTrack = AlbumTrack;
            clone.Genres = Genres.ToList();
            //clone.Duration = Duration;
            clone.Tags = Tags.ToList();
            return clone;
        }
    }
}