using LML.Core.Models;
using LML.Core.Filters;

namespace LML.Core.Services
{
    /// <summary>
    /// Represents a named filter with its string representation and compiled filter.
    /// </summary>
    public interface INamedFilter
    {
        /// <summary>
        /// Gets or sets the name of the filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the filter.
        /// </summary>
        public string FilterString { get; set; }

        /// <summary>
        /// Gets or sets the compiled filter instance.
        /// </summary>
        public IFilter? Filter { get; set; }
    }

    /// <summary>
    /// Defines the interface for managing a media library.
    /// </summary>
    public interface IMediaLibraryService
    {
        #region Properties
        /// <summary>
        /// Gets the path to the library file.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the pattern used for file organization.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Gets whether there are unsaved changes in the library.
        /// </summary>
        public bool UnsavedChanges { get; }
        #endregion

        #region Functions
        #region Media Files
        /// <summary>
        /// Adds a single media file to the library.
        /// </summary>
        /// <param name="filePath">The path to the media file.</param>
        /// <param name="autoExtractMetadata">Whether to automatically extract metadata.</param>
        /// <returns>The added media file, or null if the file couldn't be added.</returns>
        Task<MediaFile?> AddMediaFileAsync(string filePath, bool autoExtractMetadata = true);

        /// <summary>
        /// Adds multiple media files from a directory to the library.
        /// </summary>
        /// <param name="directoryPath">The path to the directory.</param>
        /// <param name="recursive">Whether to include subdirectories.</param>
        /// <param name="autoExtractMetadata">Whether to automatically extract metadata.</param>
        /// <param name="progress">Optional progress reporter.</param>
        /// <returns>The added media files.</returns>
        Task<IEnumerable<MediaFile>> AddMediaFilesFromDirectoryAsync(string directoryPath, bool recursive = true, bool autoExtractMetadata = true, IProgress<(int current, int total)>? progress = null);

        /// <summary>
        /// Removes media files from the library.
        /// </summary>
        /// <param name="mediaFiles">The media files to remove.</param>
        /// <param name="deleteFile">Whether to delete the physical files.</param>
        Task RemoveMediaFilesAsync(List<MediaFile> mediaFiles, bool deleteFile = false);

        /// <summary>
        /// Gets all media files in the library.
        /// </summary>
        /// <returns>All media files in the library.</returns>
        Task<IEnumerable<MediaFile>> GetMediaFilesAsync();
        #endregion

        #region Filters
        /// <summary>
        /// Filters media files using the specified filter.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The filtered media files.</returns>
        Task<IEnumerable<MediaFile>> FilterMediaFilesAsync(IFilter filter);

        /// <summary>
        /// Adds a new named filter to the library.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="filter">The filter to add.</param>
        /// <returns>The created named filter.</returns>
        Task<INamedFilter> AddFilterAsync(string name, IFilter filter);

        /// <summary>
        /// Gets all named filters in the library.
        /// </summary>
        /// <returns>All named filters in the library.</returns>
        Task<List<INamedFilter>> GetFiltersAsync();

        /// <summary>
        /// Gets all named filters in the library.
        /// </summary>
        /// <returns>All named filters in the library.</returns>
        List<INamedFilter> GetFilters();

        /// <summary>
        /// Deletes a named filter from the library.
        /// </summary>
        /// <param name="name">The name of the filter to delete.</param>
        Task DeleteFilterAsync(string name);
        #endregion

        #region Getters
        /// <summary>
        /// Gets a dictionary of artists and their occurrence count.
        /// </summary>
        /// <returns>Dictionary of artists and their count.</returns>
        Task<Dictionary<string, int>> GetArtistsAsync();

        /// <summary>
        /// Gets a dictionary of albums and their occurrence count.
        /// </summary>
        /// <returns>Dictionary of albums and their count.</returns>
        Task<Dictionary<string, int>> GetAlbumsAsync();

        /// <summary>
        /// Gets a dictionary of genres and their occurrence count.
        /// </summary>
        /// <returns>Dictionary of genres and their count.</returns>
        Task<Dictionary<string, int>> GetGenresAsync();

        /// <summary>
        /// Gets a dictionary of tags and their occurrence count.
        /// </summary>
        /// <returns>Dictionary of tags and their count.</returns>
        Task<Dictionary<string, int>> GetTagsAsync();
        #endregion

        #region Load / Save
        /// <summary>
        /// Saves the current state of the library.
        /// </summary>
        Task SaveLibraryAsync();

        /// <summary>
        /// Loads the library from disk.
        /// </summary>
        Task LoadLibraryAsync();

        /// <summary>
        /// Exports selected media files to a specified path.
        /// </summary>
        /// <param name="mediaFiles">The media files to export.</param>
        /// <param name="outputPath">The path to export to.</param>
        Task ExportAsync(List<MediaFile> mediaFiles, string outputPath);
        #endregion

        /// <summary>
        /// Finds duplicate media files in the library.
        /// </summary>
        /// <returns>The duplicate media files.</returns>
        Task<IEnumerable<MediaFile>> FindDuplicatesAsync();

        /// <summary>
        /// Finds media files that are not in any playlist.
        /// </summary>
        /// <returns>The media files without playlists.</returns>
        Task<IEnumerable<MediaFile>> FindFilesWithoutPlaylistAsync();

        /// <summary>
        /// Organizes files according to the library pattern.
        /// </summary>
        /// <param name="deleteOriginals">Whether to delete original files after organization.</param>
        /// <param name="progress">Optional progress reporter.</param>
        Task OrganizeFilesAsync(bool deleteOriginals = false, IProgress<(int current, int total)>? progress = null);

        #region Playlists
        /// <summary>
        /// Rebuilds playlists from the specified paths.
        /// </summary>
        /// <param name="playlistPathes">The paths of the playlists to rebuild.</param>
        Task RebuildPlaylist(List<string> playlistPathes);

        /// <summary>
        /// Creates an M3U playlist from the specified media files.
        /// </summary>
        /// <param name="mediaFiles">The media files to include.</param>
        /// <param name="filter">Optional filter to apply to the files.</param>
        /// <param name="outputPath">The path to save the playlist.</param>
        /// <param name="playlistName">Optional name for the playlist.</param>
        Task CreatePlaylist_M3U(IEnumerable<MediaFile> mediaFiles, IFilter? filter, string outputPath, string? playlistName = null);
        #endregion

        /// <summary>
        /// Updates the collection counts for a media file.
        /// </summary>
        /// <param name="mediaFile">The media file to update.</param>
        /// <param name="increment">Whether to increment or decrement the counts.</param>
        void UpdateCollections(MediaFile mediaFile, bool increment);

        /// <summary>
        /// Notifies that there are unsaved changes in the library.
        /// </summary>
        void NotifyUnsavedChanges();
        #endregion
    }
}