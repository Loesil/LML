using LML.Core.Models;
using LML.Core.Filters;
using LML.Core.Services;
using System.Text.Json;
using File = System.IO.File;
using System.Text.Json.Serialization;

namespace LML.Infrastructure.Services
{
    #region Filter
    /// <summary>
    /// Represents a named filter with its string representation and compiled filter.
    /// </summary>
    public class NamedFilter : INamedFilter
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the filter.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the filter.
        /// </summary>
        [JsonPropertyName("filter")]
        public string FilterString { get; set; }

        /// <summary>
        /// Gets or sets the compiled filter instance.
        /// </summary>
        [JsonIgnore]
        public IFilter? Filter { get; set; }
        #endregion

        #region Create
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedFilter"/> class.
        /// </summary>
        public NamedFilter()
        {
            Name = "";
            FilterString = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedFilter"/> class with the specified name and filter string.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="filterString">The string representation of the filter.</param>
        public NamedFilter(string name, string filterString)
        {
            Name = name;
            FilterString = filterString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedFilter"/> class with the specified name and filter.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="filter">The filter instance.</param>
        public NamedFilter(string name, IFilter filter)
        {
            Name = name;
            Filter = filter;
            FilterString = Filter.GetFilterDescription();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Loads the filter from its string representation.
        /// </summary>
        public void LoadFilter()
        {

        }
        #endregion
    }
    #endregion

    #region Json Type
    /// <summary>
    /// Represents the JSON data structure for a media library.
    /// </summary>
    class MediaLibraryJsonData
    {
        #region Properties
        /// <summary>
        /// Gets or sets the file organization pattern.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? pattern { get; set; }

        /// <summary>
        /// Gets or sets the list of named filters.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<NamedFilter>? filter { get; set; }

        /// <summary>
        /// Gets or sets the list of media files.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<MediaFileJsonData>? media { get; set; }
        #endregion

        #region Create
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibraryJsonData"/> class.
        /// </summary>
        public MediaLibraryJsonData()
        {
            pattern = "";
            filter = new List<NamedFilter>();
            media = new List<MediaFileJsonData>();
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Implements the media library service for managing media files and their metadata.
    /// </summary>
    public class MediaLibraryService : IMediaLibraryService
    {
        #region Const
        /// <summary>
        /// The default file organization pattern.
        /// </summary>
        public const string DefaultPattern = "music/{Artist}/{Album}/{Title}";
        #endregion

        #region Properties
        /// <inheritdoc/>
        public string Pattern { get; private set; }

        /// <inheritdoc/>
        public string Path { get; private set; }

        /// <inheritdoc/>
        public bool UnsavedChanges { get; private set; }

        #region Metadata Lists
        private readonly Dictionary<string, int> _tags;
        private readonly Dictionary<string, int> _artists;
        private readonly Dictionary<string, int> _albums;
        private readonly Dictionary<string, int> _genres;
        #endregion
        #endregion

        #region Private Properties
        private readonly List<MediaFile> _mediaFiles;

        private readonly List<NamedFilter> _filters;

        private bool _temporarilyDisableUpdate = false;
        #endregion

        #region Create
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLibraryService"/> class.
        /// </summary>
        /// <param name="path">The path to the library file.</param>
        public MediaLibraryService(string path)
        {
            Path = path;
            Pattern = DefaultPattern;
            _mediaFiles = new List<MediaFile>();
            _filters = new List<NamedFilter>();
            _tags = new Dictionary<string, int>();
            _artists = new Dictionary<string, int>();
            _albums = new Dictionary<string, int>();
            _genres = new Dictionary<string, int>();
        }
        #endregion

        #region Methods
        #region Media Files
        /// <inheritdoc/>
        public async Task<MediaFile?> AddMediaFileAsync(string filePath, bool autoExtractMetadata = true)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Media file not found", filePath);

            // create media file
            _temporarilyDisableUpdate = true;
            var mediaFile = new MediaFile(this, filePath);

            // extract metadata if requested
            if (autoExtractMetadata) await ExtractMetadataAsync(mediaFile);

            // add to library
            if (_mediaFiles.Find(m => m.FullFilePath == mediaFile.FullFilePath) != null)
                return null;

            _mediaFiles.Add(mediaFile);
            _temporarilyDisableUpdate = false;
            NotifyUnsavedChanges();
            UpdateCollections(mediaFile, true);
            return mediaFile;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MediaFile>> AddMediaFilesFromDirectoryAsync(string directoryPath, bool recursive = true, bool autoExtractMetadata = true, IProgress<(int current, int total)>? progress = null)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException("Directory not found");

            // get all files in the directory
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(directoryPath, "*.*", searchOption)
                .Where(f => IsMediaFile(f));

            // iterate over files and add them to the library
            var totalFiles = files.Count();
            var processedCount = 0;
            var addedFiles = new List<MediaFile>();
            foreach (var file in files)
            {
                addedFiles.Add(await AddMediaFileAsync(file, autoExtractMetadata));
                processedCount++;
                progress?.Report((processedCount, totalFiles));
            }

            return addedFiles;
        }

        /// <inheritdoc/>
        public async Task RemoveMediaFilesAsync(List<MediaFile> mediaFiles, bool deleteFile = false)
        {
            // delete files if requested
            if (deleteFile)
                mediaFiles.FindAll(f => File.Exists(f.FilePath)).ForEach(f => File.Delete(f.FilePath));

            // remove from library
            mediaFiles.ForEach(f =>
            {
                UpdateCollections(f, false);
                _mediaFiles.Remove(f);
            });
            await Task.Delay(0);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<MediaFile>> GetMediaFilesAsync()
        {
            return Task.FromResult(_mediaFiles.AsEnumerable());
        }
        #endregion

        #region Filter  
        /// <inheritdoc/>
        public Task<IEnumerable<MediaFile>> FilterMediaFilesAsync(IFilter filter)
        {
            var filteredFiles = _mediaFiles.Where(f => filter.Apply(f));
            return Task.FromResult(filteredFiles);
        }

        /// <inheritdoc/>
        public Task<INamedFilter> AddFilterAsync(string name, IFilter filter)
        {
            NamedFilter f = new NamedFilter(name, filter);
            _filters.Add(f);
            return Task.FromResult((INamedFilter)f);
        }

        /// <inheritdoc/>
        public Task DeleteFilterAsync(string name)
        {
            _filters.RemoveAll(f => f.Name == name);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<IEnumerable<INamedFilter>> GetFiltersAsync()
        {
            return Task.FromResult(_filters.ConvertAll(f => (INamedFilter)f).AsEnumerable());
        }
        #endregion

        #region Getters
        /// <inheritdoc/>
        public Task<Dictionary<string, int>> GetArtistsAsync()
        {
            return Task.FromResult(new Dictionary<string, int>(_artists));
        }

        /// <inheritdoc/>
        public Task<Dictionary<string, int>> GetAlbumsAsync()
        {
            return Task.FromResult(new Dictionary<string, int>(_albums));
        }

        /// <inheritdoc/>
        public Task<Dictionary<string, int>> GetGenresAsync()
        {
            return Task.FromResult(new Dictionary<string, int>(_genres));
        }

        /// <inheritdoc/>
        public Task<Dictionary<string, int>> GetTagsAsync()
        {
            return Task.FromResult(new Dictionary<string, int>(_tags));
        }
        #endregion

        #region Save / Load
        /// <inheritdoc/>
        public async Task ExportAsync(List<MediaFile> mediaFiles, string outputPath)
        {
            MediaLibraryService export = new MediaLibraryService(outputPath);
            export._temporarilyDisableUpdate = true;
            List<MediaFile> exportedFiles = mediaFiles.ConvertAll(m => m.Clone(export, true));
            exportedFiles.ForEach(m => export._mediaFiles.Add(m));
            export._temporarilyDisableUpdate = false;
            await export.SaveLibraryAsync();
        }

        /// <inheritdoc/>
        public async Task SaveLibraryAsync()
        {
            var libraryData = new MediaLibraryJsonData()
            {
                filter = _filters,
                media = _mediaFiles.ConvertAll(m => new MediaFileJsonData(m)),
                pattern = string.IsNullOrEmpty(Pattern) ? DefaultPattern : Pattern,
            };

            var json = JsonSerializer.Serialize(libraryData, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(Path, json);
            UnsavedChanges = false;
        }

        /// <inheritdoc/>
        public async Task LoadLibraryAsync()
        {
            if (!File.Exists(Path))
                return;

            // Read the library data
            var json = await File.ReadAllTextAsync(Path);
            var libraryData = JsonSerializer.Deserialize<MediaLibraryJsonData>(json);

            // Clear all collections
            _mediaFiles.Clear();
            _tags.Clear();
            _artists.Clear();
            _albums.Clear();
            _genres.Clear();
            if (libraryData == null) return;

            // library data
            Pattern = string.IsNullOrEmpty(libraryData.pattern) ? DefaultPattern : libraryData.pattern;

            // Add filters
            if (libraryData.filter != null)
            {
                foreach (var filter in libraryData.filter)
                {
                    try
                    {
                        _filters.Add(new NamedFilter(filter.Name, FilterParser.ParseFilter(filter.FilterString)));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing filter {filter.FilterString}: {ex.Message}");
                    }
                }
            }

            // Add all files and update collections 
            if (libraryData.media != null)
            {
                foreach (var file in libraryData.media)
                {
                    _temporarilyDisableUpdate = true;
                    var mf = new MediaFile(this, file.FilePath)
                    {
                        Title = file.Title,
                        Artists = file.Artists == null ? new List<string>() : file.Artists!.Replace(";", ",").Split(",").ToList().ConvertAll(s => s.Trim()),
                        Album = file.Album ?? "",
                        AlbumTrack = file.AlbumTrack,
                        Genres = file.Genres == null ? new List<string>() : file.Genres!.Replace(";", ",").Split(",").ToList().ConvertAll(s => s.Trim()),
                        Tags = file.Tags == null ? new List<string>() : file.Tags!.Replace(";", ",").Split(",").ToList().ConvertAll(s => s.Trim())
                    };
                    _mediaFiles.Add(mf);
                    _temporarilyDisableUpdate = false;
                    UpdateCollections(mf, true);
                }
            }

            UnsavedChanges = false;
        }
        #endregion

        #region Find
        /// <inheritdoc/>
        public Task<IEnumerable<MediaFile>> FindDuplicatesAsync()
        {
            var duplicates = _mediaFiles
                .GroupBy(f => new { f.Title, f.Artists, f.Album })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            return Task.FromResult(duplicates.AsEnumerable());
        }

        /// <inheritdoc/>
        public Task<IEnumerable<MediaFile>> FindFilesWithoutPlaylistAsync()
        {
            var filesWithoutPlaylist = _mediaFiles.Where(f => !f.IsInPlaylist);
            return Task.FromResult(filesWithoutPlaylist);
        }
        #endregion

        #region Organize
        /// <inheritdoc/>
        public async Task OrganizeFilesAsync(bool deleteOriginals = false, IProgress<(int current, int total)>? progress = null)
        {
            string pattern = string.IsNullOrEmpty(Pattern) ? DefaultPattern : Pattern;
            var processedCount = 0;
            var totalFiles = _mediaFiles.Count;
            string baseDir = System.IO.Path.GetDirectoryName(Path)!;

            foreach (var file in _mediaFiles.ToList())
            {
                try
                {
                    string newPath = BuildOrganizedPath(file, pattern);
                    string relPath = "./" + System.IO.Path.GetRelativePath(baseDir, newPath).Replace("\\", "/");
                    if (relPath != file.FilePath)
                    {
                        // ensure directly exists
                        string? directory = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(newPath));
                        if (directory != null
                            && !Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        if (File.Exists(newPath))
                        {
                            // handle duplicates by appending a number
                            var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(newPath);
                            var extension = System.IO.Path.GetExtension(newPath);
                            var counter = 1;

                            while (File.Exists(newPath))
                            {
                                newPath = System.IO.Path.Combine(
                                   directory ?? "",
                                    $"{fileNameWithoutExt} ({counter}){extension}"
                                );
                                counter++;
                            }
                        }

                        // move or copy file
                        string fullPath = System.IO.Path.GetFullPath(newPath);
                        if (deleteOriginals)
                            File.Move(file.FullFilePath, fullPath);
                        else
                            File.Copy(file.FullFilePath, fullPath);

                        // update file path 
                        file.FilePath = relPath;
                    }

                    // update progress
                    processedCount++;
                    progress?.Report((processedCount, totalFiles));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error organizing file {file.FilePath}: {ex.Message}");
                }

                await Task.Yield(); // allow UI to remain responsive
            }

            NotifyUnsavedChanges();
            await SaveLibraryAsync();
        }
        #endregion

        #region Playlists
        /// <inheritdoc/>
        public async Task RebuildPlaylist(List<string> playlistPathes)
        {
            foreach (string f in playlistPathes)
            {
                string ext = System.IO.Path.GetExtension(f);

                // read filter string
                string? filterString = null;
                switch (ext)
                {
                    case ".m3u":
                        {
                            string content = await File.ReadAllTextAsync(f);
                            filterString = content.Replace("\r\n", "\n").Split("\n").ToList().Find(l => l.StartsWith("#LML:"))?.Split(":")[1].Trim();
                        }
                        break;

                    default:
                        throw new Exception("Invalid playlist file extension");
                }

                // rebuild playlist
                try
                {
                    if (filterString == null)
                        throw new Exception("Filter string not found");

                    IFilter filter = FilterParser.ParseFilter(filterString);
                    List<MediaFile> mediaFiles = (await FilterMediaFilesAsync(filter)).ToList();
                    switch (ext)
                    {
                        case ".m3u":
                            await CreatePlaylist_M3U(mediaFiles, filter, f);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error rebuilding playlist {f}: {ex.Message}");
                }
            }
        }

        /// <inheritdoc/>
        public async Task CreatePlaylist_M3U(IEnumerable<MediaFile> mediaFiles, IFilter? filter, string outputPath, string? playlistName = null)
        {
            try
            {
                // Filter files
                var filteredFiles = mediaFiles.Where(file => filter?.Apply(file) ?? true).ToList();
                if (!filteredFiles.Any())
                    return;

                // Create output directory if it doesn't exist
                string outputDir = System.IO.Path.GetDirectoryName(outputPath)!;
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                // Create M3U file
                using (var writer = new StreamWriter(outputPath))
                {
                    // Write M3U header
                    await writer.WriteLineAsync("#EXTM3U");
                    if (!string.IsNullOrEmpty(playlistName))
                        await writer.WriteLineAsync($"#PLAYLIST:{playlistName}");

                    // Write Playlist Metadata
                    if (filter != null)
                        await writer.WriteLineAsync($"#LML:{filter.GetFilterDescription()}");

                    // Write each file entry
                    foreach (var file in filteredFiles)
                    {
                        await writer.WriteLineAsync(file.GetM3UEntry());
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Builds the organized path for a media file based on the pattern.
        /// </summary>
        /// <param name="file">The media file.</param>
        /// <param name="pattern">The organization pattern.</param>
        /// <returns>The organized path.</returns>
        private string BuildOrganizedPath(MediaFile file, string pattern)
        {
            var basePath = System.IO.Path.GetDirectoryName(Path)!;
            var fileName = System.IO.Path.GetFileName(file.FilePath);
            var extension = System.IO.Path.GetExtension(file.FilePath);

            string path = pattern
                .Replace("{Type}", file.Type.ToString())
                .Replace("{Artist}", SafeFileName(file.Artists.Count != 0 ? string.Join(", ", file.Artists) : "Unknown Artist"))
                .Replace("{Album}", SafeFileName(file.Album ?? "Unknown Album"))
                .Replace("{Title}", SafeFileName(System.IO.Path.GetFileNameWithoutExtension(file.Title ?? fileName)))
                .Replace("\\", "/")
                .Replace("//", "/");
            if (path.StartsWith("/"))
                path = "." + path;

            return System.IO.Path.GetFullPath(System.IO.Path.Combine(basePath, path + extension)).Replace("\\", "/");
        }

        /// <summary>
        /// Creates a safe file name by removing invalid characters.
        /// </summary>
        /// <param name="fileName">The original file name.</param>
        /// <returns>The safe file name.</returns>
        private string SafeFileName(string fileName)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries))
                .TrimEnd('.');
        }

        /// <summary>
        /// Checks if a file is a media file based on its extension.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>True if the file is a media file.</returns>
        private bool IsMediaFile(string filePath)
        {
            var extension = System.IO.Path.GetExtension(filePath).ToLower();
            return MediaFile.AudioExtensions.Contains(extension) || MediaFile.VideoExtensions.Contains(extension);
        }

        /// <summary>
        /// Extracts metadata from a media file asynchronously.
        /// </summary>
        /// <param name="mediaFile">The media file.</param>
        private async Task ExtractMetadataAsync(MediaFile mediaFile)
        {
            await Task.Run(() =>
            {
                mediaFile.ExtractMetadata();
                NotifyUnsavedChanges();
            });
        }

        /// <inheritdoc/>
        public void UpdateCollections(MediaFile mediaFile, bool increment)
        {
            if (_temporarilyDisableUpdate) return;
            UpdateMetaInfoDictionary(_albums, mediaFile.Album, increment);
            mediaFile.Artists.ForEach(i => UpdateMetaInfoDictionary(_artists, i, increment));
            mediaFile.Genres.ForEach(i => UpdateMetaInfoDictionary(_genres, i, increment));
            mediaFile.Tags.ForEach(i => UpdateMetaInfoDictionary(_tags, i, increment));
        }

        /// <inheritdoc/>
        public void NotifyUnsavedChanges()
        {
            if (_temporarilyDisableUpdate) return;
            UnsavedChanges = true;
        }

        /// <summary>
        /// Updates a metadata dictionary with the specified key.
        /// </summary>
        /// <param name="dictionary">The dictionary to update.</param>
        /// <param name="key">The key to update.</param>
        /// <param name="increment">Whether to increment or decrement the count.</param>
        private void UpdateMetaInfoDictionary(Dictionary<string, int> dictionary, string key, bool increment)
        {
            if (increment)
                if (dictionary.ContainsKey(key))
                    dictionary[key]++;
                else
                    dictionary.Add(key, 1);
            else
                if (dictionary.ContainsKey(key))
                dictionary[key]--;

            if (dictionary[key] == 0)
                dictionary.Remove(key);
        }
        #endregion
        #endregion
    }
}