using LML.Core.Filters;
using LML.Core.Models;
using LML.Core.Services;
using LML.Infrastructure.Services;

namespace LML.GUI
{
    public partial class Form_Overview : Form
    {
        #region Constants
        private static readonly List<string> DefaultFilterOptions = new List<string> { "All", "Unknown Artist", "Not Local" };
        private readonly List<ToolStripItem> ToolStripsToEnableOnOpen;
        #endregion

        #region Properties
        private IMediaLibraryService? mediaLibraryService;

        private IFilter? filter;
        private List<MediaFile>? mediaFiles;
        private List<MediaFile>? filteredMediaFiles;
        private List<MediaFile>? sortedMediaFiles;

        private List<DataGridViewRow> selectedItems
        {
            get => dgv_Media.SelectedRows.Cast<DataGridViewRow>().ToList();
        }

        private List<MediaFile> selectedMediaFiles
        {
            get => selectedItems.ConvertAll(i => (MediaFile)i.DataBoundItem!);
        }
        #endregion

        #region Create
        public Form_Overview()
        {
            InitializeComponent();
            InitializeDataGridView();

            ToolStripsToEnableOnOpen = new List<ToolStripItem>
            {
                organizeAndDeleteToolStripMenuItem,
                organizeToolStripMenuItem,
                saveToolStripMenuItem,
                mediaToolStripMenuItem,
                metaToolStripMenuItem,
                infoToolStripMenuItem,
                playlistToolStripMenuItem,
                filterToolStripMenuItem,
                filesToolStripMenuItem
            };

            pb_Progress.Size = dgv_Media.Size;
            cmb_Filter.SelectedItem = cmb_Filter.Items.Cast<string>().ToList().Find(f => f == "All");
            cmb_sort.SelectedItem = cmb_sort.Items.Cast<string>().ToList().Find(f => f == "Artist");
        }

        private void InitializeDataGridView()
        {
            dgv_Media.AutoGenerateColumns = false;

            // column databindings
            col_Title.DataPropertyName = "Title";
            col_Artists.DataPropertyName = "Stringified_Artists";
            col_Genres.DataPropertyName = "Stringified_Genres";
            col_Album.DataPropertyName = "Album";
            col_Track.DataPropertyName = "Stringified_AlbumTrack";
            col_Tags.DataPropertyName = "Stringified_Tags";
        }
        #endregion

        #region Functions
        #region Progress
        private void ShowProgressBar()
        {
            pb_Progress.Value = 0;
            pb_Progress.Visible = true;
            pb_Progress.BringToFront();
        }

        private void HideProgressBar()
        {
            pb_Progress.Visible = false;
        }

        private void UpdateProgress(int value, int maximum)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProgress(value, maximum)));
                return;
            }

            pb_Progress.Maximum = maximum;
            pb_Progress.Value = value;
        }
        #endregion

        private async Task LoadMediaFiles()
        {
            await FilterMedia();
        }

        private async Task OpenLibrary(bool _open)
        {
            string path;
            if (!_open)
            {
                // save file dialog
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.RestoreDirectory = true;
                sfd.AddExtension = true;
                sfd.DefaultExt = "json";
                sfd.Filter = "json|*.json";
                if (sfd.ShowDialog() != DialogResult.OK) return;
                path = sfd.FileName;
            }
            else
            {
                // open file dialog
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.RestoreDirectory = true;
                ofd.Multiselect = false;
                ofd.Filter = "json|*.json";
                if (ofd.ShowDialog() != DialogResult.OK) return;
                path = ofd.FileName;
            }

            // open
            mediaLibraryService = new MediaLibraryService(path);

            // load
            await mediaLibraryService.LoadLibraryAsync();
            await ReloadFilters();
            await LoadMediaFiles();

            // enable menu items
            ToolStripsToEnableOnOpen.ForEach(i => i.Enabled = true);
        }

        private async Task ReloadFilters(string? newSelection = null)
        {
            List<INamedFilter> filters = (await mediaLibraryService!.GetFiltersAsync()).ToList();

            string selection = newSelection ?? (string)cmb_Filter.SelectedItem!;
            cmb_Filter.Items.Clear();
            DefaultFilterOptions.ForEach(f => cmb_Filter.Items.Add(f));
            filters.ForEach(f => cmb_Filter.Items.Add($"[{f.Name}]"));

            // select last or All
            cmb_Filter.SelectedItem = cmb_Filter.Items.Cast<string>().ToList().Find(f => f == selection) ?? cmb_Filter.Items.Cast<string>().ToList().Find(f => f == "All");
        }

        private async Task AddMedia(string path)
        {
            await mediaLibraryService!.AddMediaFileAsync(path);
            await LoadMediaFiles();
        }

        private async Task AddMediaDirectory(string path)
        {
            try
            {
                ShowProgressBar();
                var progress = new Progress<(int current, int total)>(update =>
                {
                    UpdateProgress(update.current, update.total);
                });

                IEnumerable<MediaFile> ms = await mediaLibraryService!.AddMediaFilesFromDirectoryAsync(path, true, true, progress);
                await LoadMediaFiles();

                MessageBox.Show("Files added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideProgressBar();
            }
        }

        private async Task RemoveSelectedMedias(bool delete)
        {
            // get selected items
            List<MediaFile> medias = selectedMediaFiles;

            // remove
            await mediaLibraryService!.RemoveMediaFilesAsync(medias, delete);
            await LoadMediaFiles();
        }

        private async Task Organize(bool delete)
        {
            try
            {
                ShowProgressBar();
                var progress = new Progress<(int current, int total)>(update =>
                {
                    UpdateProgress(update.current, update.total);
                });

                await mediaLibraryService!.OrganizeFilesAsync(delete, progress);
                await LoadMediaFiles();

                MessageBox.Show("Files organized successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error organizing files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideProgressBar();
            }
        }

        private async Task FilterMedia()
        {
            mediaFiles = (await mediaLibraryService!.GetMediaFilesAsync()).ToList();
            filteredMediaFiles = (filter != null
                ? (await mediaLibraryService!.FilterMediaFilesAsync(filter)).ToList()
                : mediaFiles
            );

            await SortMedia();
        }

        private async Task ReadSelectedMetaData()
        {
            if (MessageBox.Show("Are you sure you want to read the metadata of the selected files?", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            // get selected items
            List<MediaFile> medias = selectedMediaFiles;

            // read
            try
            {
                ShowProgressBar();
                var progress = new Progress<(int current, int total)>(update =>
                {
                    UpdateProgress(update.current, update.total);
                });

                medias.ForEach(m =>
                {
                    m.ExtractMetadata();
                    UpdateProgress(medias.IndexOf(m) + 1, medias.Count);
                });

                // refresh
                await LoadMediaFiles();
                MessageBox.Show("Metadata read successfully!", "LML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading metadata: {ex.Message}", "LML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideProgressBar();
            }
        }

        private async Task WriteSelectedMetaData()
        {
            if (MessageBox.Show("Are you sure you want to write the metadata of the selected files?", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            // get selected items
            List<MediaFile> medias = selectedMediaFiles;

            // write
            try
            {
                ShowProgressBar();
                var progress = new Progress<(int current, int total)>(update =>
                {
                    UpdateProgress(update.current, update.total);
                });

                medias.ForEach(m =>
                {
                    m.SaveMetadata();
                    UpdateProgress(medias.IndexOf(m) + 1, medias.Count);
                });

                await Task.Delay(0);
                MessageBox.Show("Metadata written successfully!", "LML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing metadata: {ex.Message}", "LML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideProgressBar();
            }
        }

        private async Task SortMedia()
        {
            string s = (string?)(cmb_sort.SelectedItem) ?? "";
            switch (s)
            {
                case "Random":
                    {
                        Random rnd = new Random();
                        List<MediaFile> toSort = filteredMediaFiles!.ToList();
                        sortedMediaFiles = new List<MediaFile>();
                        while (toSort.Count > 0)
                        {
                            int r = rnd.Next(0, toSort.Count);
                            sortedMediaFiles.Add(toSort[r]);
                            toSort.Remove(toSort[r]);
                        }
                    }
                    break;

                case "Title":
                    {
                        sortedMediaFiles = filteredMediaFiles!.ToList();
                        sortedMediaFiles.Sort(Sort_ByTitle);
                    }
                    break;

                case "Artist":
                    {
                        sortedMediaFiles = filteredMediaFiles!.ToList();
                        sortedMediaFiles.Sort(Sort_ByArtist);
                    }
                    break;

                case "Album":
                    {
                        sortedMediaFiles = filteredMediaFiles!.ToList();
                        sortedMediaFiles.Sort(Sort_ByAlbum);
                    }
                    break;

                default:
                    // unsorted
                    sortedMediaFiles = filteredMediaFiles!.ToList();
                    break;
            }
            await Task.Delay(0);

            // remember selection
            List<MediaFile> selected = selectedMediaFiles;

            // new datasource
            dgv_Media.DataSource = new BindingSource(sortedMediaFiles, "");

            // restore selection
            dgv_Media.ClearSelection();
            dgv_Media.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(r => r.Selected = selected.Contains((MediaFile)r.DataBoundItem!));
            if (selected.Count == 1)
                dgv_Media.FirstDisplayedScrollingRowIndex = sortedMediaFiles.IndexOf(selected[0]);

            RefreshInfo();
        }

        private void RefreshInfo()
        {
            lbl_Info.Text = $"[Files: {mediaFiles!.Count}] [Filter: {filteredMediaFiles!.Count}] [Selected: {selectedItems.Count}]";
        }

        #region Sort Functions
        public static int Sort_ByTitle(MediaFile a, MediaFile b)
        {
            return string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase);
        }

        public static int Sort_ByAlbum(MediaFile a, MediaFile b)
        {
            int c1 = string.Compare(a.Album, b.Album, StringComparison.OrdinalIgnoreCase);
            int c2 = ((int?)a.AlbumTrack ?? 0) - ((int?)b.AlbumTrack ?? 0);
            return (c1 == 0
                ? (c2 == 0
                    ? Sort_ByTitle(a, b)
                    : c2)
                : c1);
        }

        public static int Sort_ByArtist(MediaFile a, MediaFile b)
        {
            int c = string.Compare(a.Stringified_Artists, b.Stringified_Artists, StringComparison.OrdinalIgnoreCase);
            return c == 0 ? Sort_ByAlbum(a, b) : c;
        }
        #endregion

        #region Lookup
        private async Task<string?> Lookup(Dictionary<string, int> data)
        {
            Form_Lookup lookup = new Form_Lookup(data);
            Hide();
            DialogResult r = lookup.ShowDialog();
            Show();

            return await Task.FromResult(r != DialogResult.OK
                ? null
                : lookup.FilterResult
            );
        }

        private async Task LookupArtist()
        {
            string? result = await Lookup(await mediaLibraryService!.GetArtistsAsync());
            if (result == null) return;

            // filter
            cmb_Filter.SelectedItem = null;
            filter = new Filter_StringList_contains(MediaProperty.Artists, result!, true);
            await FilterMedia();
        }

        private async Task LookupAlbum()
        {
            string? result = await Lookup(await mediaLibraryService!.GetAlbumsAsync());
            if (result == null) return;

            // filter
            cmb_Filter.SelectedItem = null;
            filter = new Filter_String_equal(MediaProperty.Album, result!, true);
            await FilterMedia();
        }

        private async Task LookupGenre()
        {
            string? result = await Lookup(await mediaLibraryService!.GetGenresAsync());
            if (result == null) return;

            // filter
            cmb_Filter.SelectedItem = null;
            filter = new Filter_StringList_contains(MediaProperty.Genres, result!, true);
            await FilterMedia();
        }

        private async Task LookupTag()
        {
            string? result = await Lookup(await mediaLibraryService!.GetTagsAsync());
            if (result == null) return;

            // filter
            cmb_Filter.SelectedItem = null;
            filter = new Filter_StringList_contains(MediaProperty.Tags, result!, true);
            await FilterMedia();
        }
        #endregion

        private async Task BatchEdit()
        {
            List<MediaFile> selected = selectedMediaFiles;
            if (selected.Count == 0)
            {
                MessageBox.Show("No files selected", "LML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form_BatchEdit f = new Form_BatchEdit();
            if (f.ShowDialog() != DialogResult.OK) return;

            // artists
            if (f.Result_Artists != ""
                && MessageBox.Show($"Are you sure you want to change the artists of the selected files to [{f.Result_Artists!}]?", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes
            )
            {
                selected.ForEach(m => m.Stringified_Artists = f.Result_Artists!);
            }

            // album
            if (f.Result_Album != ""
                && MessageBox.Show($"Are you sure you want to change the album of the selected files to [{f.Result_Album!}]?", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes
            )
            {
                selected.ForEach(m => m.Album = f.Result_Album!);
            }

            // genres
            if (f.Result_Genres != ""
                && MessageBox.Show($"Are you sure you want to change the genres of the selected files to [{f.Result_Genres!}]?", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes
            )
            {
                selected.ForEach(m => m.Stringified_Genres = f.Result_Genres!);
            }

            // tags 
            if (f.Result_Tags != ""
                && MessageBox.Show($"Are you sure you want to change the tags of the selected files to [{f.Result_Tags!}]?", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes
            )
            {
                selected.ForEach(m => m.Stringified_Tags = f.Result_Tags!);
            }

            // refresh
            await LoadMediaFiles();
        }

        private async Task CreatePlaylist(bool fromFilter)
        {
            // get files
            List<MediaFile> files = fromFilter ? filteredMediaFiles! : selectedMediaFiles;
            if (files.Count == 0)
            {
                MessageBox.Show("No files selected", "LML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // save file dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.AddExtension = true;
            sfd.DefaultExt = "m3u";
            sfd.Filter = "m3u|*.m3u";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // create
                _ = mediaLibraryService!.CreatePlaylist_M3U(files, fromFilter ? filter : null, sfd.FileName);

                // message
                MessageBox.Show("Playlist created successfully!", "LML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            await Task.Delay(0);
        }

        private async Task EditFilter(string editName)
        {
            IFilter? editFilter = null;
            if (editName.StartsWith("[") && editName.EndsWith("]"))
            {
                string name = editName.Substring(1, editName.Length - 2);
                INamedFilter? namedFilter = (await mediaLibraryService!.GetFiltersAsync()).ToList().Find(f => f.Name == name);
                filter = namedFilter?.Filter;
            }

            // open form
            Form_Filter f = new Form_Filter(editName, editFilter);
            if (f.ShowDialog() != DialogResult.OK) return;

            // set filter
            filter = f.Filter;
            if (editFilter == null && f.FilterName != "")
            {
                await mediaLibraryService!.AddFilterAsync(f.FilterName, f.Filter!);
                await ReloadFilters($"[{f.FilterName}]");
            }
            await FilterMedia();

            await Task.Delay(0);
        }

        private async Task DeleteFilter(string name)
        {
            await mediaLibraryService!.DeleteFilterAsync(name);
            await ReloadFilters();
        }

        private async Task SelectFilter()
        {
            if (mediaLibraryService == null) return;

            bool filterFound = false;
            string filterName = (string)cmb_Filter.SelectedItem!;
            switch (filterName)
            {
                case "All":
                    filter = null;
                    filterFound = true;
                    break;

                case "Unknown Artist":
                    filter = new Filter_Bool(MediaProperty.UnknownArtist);
                    filterFound = true;
                    break;

                case "Not Local":
                    filter = new Filter_not(new Filter_Bool(MediaProperty.Local));
                    filterFound = true;
                    break;

                default:
                    {
                        if (filterName.StartsWith("[") && filterName.EndsWith("]"))
                        {
                            string name = filterName.Substring(1, filterName.Length - 2);
                            INamedFilter? namedFilter = (await mediaLibraryService!.GetFiltersAsync()).ToList().Find(f => f.Name == name);
                            filter = namedFilter?.Filter;
                            filterFound = true;
                        }
                    }
                    break;
            }

            if (filterFound)
                await FilterMedia();
        }

        private async Task RebuildPlaylist()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Multiselect = true;
            ofd.Filter = "m3u|*.m3u";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _ = mediaLibraryService!.RebuildPlaylist(ofd.FileNames.ToList());
                MessageBox.Show("Playlist rebuilt successfully!", "LML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            await Task.Delay(0);
        }

        private async Task ExportLibrary()
        {
            // get selection
            List<MediaFile> files = selectedMediaFiles.ToList();
            if (files.Count == 0)
            {
                MessageBox.Show("No files selected", "LML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // save file dialog
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.AddExtension = true;
            sfd.DefaultExt = "json";
            sfd.Filter = "json|*.json";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            // export
            try
            {
                await mediaLibraryService!.ExportAsync(files, sfd.FileName);
                MessageBox.Show("Library exported successfully!", "LML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting library: {ex.Message}", "LML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region UI Events
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = OpenLibrary(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = OpenLibrary(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaLibraryService?.SaveLibraryAsync();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _ = AddMedia(ofd.FileName);
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                _ = AddMediaDirectory(fbd.SelectedPath);
            }
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = RemoveSelectedMedias(false);
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = RemoveSelectedMedias(true);
        }

        private void organizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = Organize(false);
        }
        private void organizeAndDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Organize and delete", "LML", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                _ = Organize(true);
        }

        private void cmb_sort_SelectedValueChanged(object sender, EventArgs e)
        {
            _ = SortMedia();
        }

        private void readSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = ReadSelectedMetaData();
        }

        private void writeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = WriteSelectedMetaData();
        }

        private void showAllArtistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = LookupArtist();
        }

        private void showAllAlbumsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = LookupAlbum();
        }

        private void showAllGenresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = LookupGenre();
        }

        private void showAllTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = LookupTag();
        }

        private void dgv_Media_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != dgv_Media.Columns.IndexOf(col_State))
                return;

            MediaFile media = (MediaFile)dgv_Media.Rows[e.RowIndex].DataBoundItem!;
            media.Play();
        }

        private void dgv_Media_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            MediaFile media = (MediaFile)dgv_Media.Rows[e.RowIndex].DataBoundItem!;
            if (e.ColumnIndex == dgv_Media.Columns.IndexOf(col_State))
            {
                // state
                Color clr = Color.Green;
                if (!media.FileExists) clr = Color.Red;
                else if (!media.IsInLibrary) clr = Color.Orange;

                e.CellStyle!.BackColor = clr;
                e.CellStyle!.ForeColor = clr;
            }
        }

        private void dgv_Media_SelectionChanged(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void batchEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = BatchEdit();
        }

        private void createFromSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = CreatePlaylist(false);
        }

        private void createFromFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = CreatePlaylist(true);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = EditFilter("");
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filterName = (string)cmb_Filter.SelectedItem!;
            if (filterName.StartsWith("[") && filterName.EndsWith("]"))
            {
                string name = filterName.Substring(1, filterName.Length - 2);
                _ = EditFilter(name);
            }
            else
            {
                MessageBox.Show("No filter selected", "LML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filterName = (string)cmb_Filter.SelectedItem!;
            if (filterName.StartsWith("[") && filterName.EndsWith("]"))
            {
                string name = filterName.Substring(1, filterName.Length - 2);
                _ = DeleteFilter(name);
            }
            else
            {
                MessageBox.Show("No filter selected", "LML", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmb_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ = SelectFilter();
        }

        private void rebuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = RebuildPlaylist();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ = ExportLibrary();
        }

        private void Form_Overview_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mediaLibraryService?.UnsavedChanges == true)
            {
                DialogResult r = MessageBox.Show("Do you want to save the changes?", "LML", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (r == DialogResult.Yes)
                {
                    mediaLibraryService?.SaveLibraryAsync();
                }
                else if (r == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void timer_Unsaved_Tick(object sender, EventArgs e)
        {
            if (mediaLibraryService != null)
            {
                lbl_Unsaved.Visible = mediaLibraryService.UnsavedChanges;
            }
        }
        #endregion
    }
}
