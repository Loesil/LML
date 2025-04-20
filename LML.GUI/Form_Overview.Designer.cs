namespace LML.GUI;

partial class Form_Overview
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStrip = new MenuStrip();
        libraryToolStripMenuItem = new ToolStripMenuItem();
        createToolStripMenuItem = new ToolStripMenuItem();
        openToolStripMenuItem = new ToolStripMenuItem();
        saveToolStripMenuItem = new ToolStripMenuItem();
        mediaToolStripMenuItem = new ToolStripMenuItem();
        addToolStripMenuItem = new ToolStripMenuItem();
        addFolderToolStripMenuItem = new ToolStripMenuItem();
        removeSelectedToolStripMenuItem = new ToolStripMenuItem();
        deleteSelectedToolStripMenuItem = new ToolStripMenuItem();
        filterToolStripMenuItem = new ToolStripMenuItem();
        newToolStripMenuItem = new ToolStripMenuItem();
        editToolStripMenuItem = new ToolStripMenuItem();
        deleteToolStripMenuItem = new ToolStripMenuItem();
        playlistToolStripMenuItem = new ToolStripMenuItem();
        createFromSelectionToolStripMenuItem = new ToolStripMenuItem();
        createFromFilterToolStripMenuItem = new ToolStripMenuItem();
        rebuildToolStripMenuItem = new ToolStripMenuItem();
        metaToolStripMenuItem = new ToolStripMenuItem();
        readSelectedToolStripMenuItem = new ToolStripMenuItem();
        writeSelectedToolStripMenuItem = new ToolStripMenuItem();
        infoToolStripMenuItem = new ToolStripMenuItem();
        batchEditToolStripMenuItem = new ToolStripMenuItem();
        showAllArtistsToolStripMenuItem = new ToolStripMenuItem();
        showAllAlbumsToolStripMenuItem = new ToolStripMenuItem();
        showAllGenresToolStripMenuItem = new ToolStripMenuItem();
        showAllTagsToolStripMenuItem = new ToolStripMenuItem();
        filesToolStripMenuItem = new ToolStripMenuItem();
        organizeToolStripMenuItem = new ToolStripMenuItem();
        organizeAndDeleteToolStripMenuItem = new ToolStripMenuItem();
        exportToolStripMenuItem = new ToolStripMenuItem();
        dgv_Media = new DataGridView();
        col_Title = new DataGridViewTextBoxColumn();
        col_Artists = new DataGridViewTextBoxColumn();
        col_Album = new DataGridViewTextBoxColumn();
        col_Track = new DataGridViewTextBoxColumn();
        col_State = new DataGridViewButtonColumn();
        col_Genres = new DataGridViewTextBoxColumn();
        col_Tags = new DataGridViewTextBoxColumn();
        cmb_sort = new ComboBox();
        lbl_Info = new Label();
        pb_Progress = new ProgressBar();
        cmb_Filter = new ComboBox();
        timer_Unsaved = new System.Windows.Forms.Timer(components);
        lbl_Unsaved = new Label();
        menuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv_Media).BeginInit();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { libraryToolStripMenuItem, mediaToolStripMenuItem, filterToolStripMenuItem, playlistToolStripMenuItem, metaToolStripMenuItem, infoToolStripMenuItem, filesToolStripMenuItem });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1415, 24);
        menuStrip.TabIndex = 1;
        menuStrip.Text = "menuStrip";
        // 
        // libraryToolStripMenuItem
        // 
        libraryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem });
        libraryToolStripMenuItem.Name = "libraryToolStripMenuItem";
        libraryToolStripMenuItem.Size = new Size(55, 20);
        libraryToolStripMenuItem.Text = "Library";
        // 
        // createToolStripMenuItem
        // 
        createToolStripMenuItem.Name = "createToolStripMenuItem";
        createToolStripMenuItem.Size = new Size(148, 22);
        createToolStripMenuItem.Text = "Create";
        createToolStripMenuItem.Click += createToolStripMenuItem_Click;
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        openToolStripMenuItem.Size = new Size(148, 22);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += openToolStripMenuItem_Click;
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.Enabled = false;
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        saveToolStripMenuItem.Size = new Size(148, 22);
        saveToolStripMenuItem.Text = "Save";
        saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
        // 
        // mediaToolStripMenuItem
        // 
        mediaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addToolStripMenuItem, addFolderToolStripMenuItem, removeSelectedToolStripMenuItem, deleteSelectedToolStripMenuItem });
        mediaToolStripMenuItem.Enabled = false;
        mediaToolStripMenuItem.Name = "mediaToolStripMenuItem";
        mediaToolStripMenuItem.Size = new Size(52, 20);
        mediaToolStripMenuItem.Text = "Media";
        // 
        // addToolStripMenuItem
        // 
        addToolStripMenuItem.Name = "addToolStripMenuItem";
        addToolStripMenuItem.Size = new Size(168, 22);
        addToolStripMenuItem.Text = "Add File";
        addToolStripMenuItem.Click += addToolStripMenuItem_Click;
        // 
        // addFolderToolStripMenuItem
        // 
        addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
        addFolderToolStripMenuItem.Size = new Size(168, 22);
        addFolderToolStripMenuItem.Text = "Add Folder";
        addFolderToolStripMenuItem.Click += addFolderToolStripMenuItem_Click;
        // 
        // removeSelectedToolStripMenuItem
        // 
        removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
        removeSelectedToolStripMenuItem.Size = new Size(168, 22);
        removeSelectedToolStripMenuItem.Text = "Remove Selection";
        removeSelectedToolStripMenuItem.Click += removeSelectedToolStripMenuItem_Click;
        // 
        // deleteSelectedToolStripMenuItem
        // 
        deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
        deleteSelectedToolStripMenuItem.Size = new Size(168, 22);
        deleteSelectedToolStripMenuItem.Text = "Delete Selection";
        deleteSelectedToolStripMenuItem.Click += deleteSelectedToolStripMenuItem_Click;
        // 
        // filterToolStripMenuItem
        // 
        filterToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, editToolStripMenuItem, deleteToolStripMenuItem });
        filterToolStripMenuItem.Enabled = false;
        filterToolStripMenuItem.Name = "filterToolStripMenuItem";
        filterToolStripMenuItem.Size = new Size(45, 20);
        filterToolStripMenuItem.Text = "Filter";
        // 
        // newToolStripMenuItem
        // 
        newToolStripMenuItem.Name = "newToolStripMenuItem";
        newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
        newToolStripMenuItem.Size = new Size(140, 22);
        newToolStripMenuItem.Text = "New";
        newToolStripMenuItem.Click += newToolStripMenuItem_Click;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(140, 22);
        editToolStripMenuItem.Text = "Edit";
        editToolStripMenuItem.Click += editToolStripMenuItem_Click;
        // 
        // deleteToolStripMenuItem
        // 
        deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
        deleteToolStripMenuItem.Size = new Size(140, 22);
        deleteToolStripMenuItem.Text = "Delete";
        deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
        // 
        // playlistToolStripMenuItem
        // 
        playlistToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { createFromSelectionToolStripMenuItem, createFromFilterToolStripMenuItem, rebuildToolStripMenuItem });
        playlistToolStripMenuItem.Enabled = false;
        playlistToolStripMenuItem.Name = "playlistToolStripMenuItem";
        playlistToolStripMenuItem.Size = new Size(56, 20);
        playlistToolStripMenuItem.Text = "Playlist";
        // 
        // createFromSelectionToolStripMenuItem
        // 
        createFromSelectionToolStripMenuItem.Name = "createFromSelectionToolStripMenuItem";
        createFromSelectionToolStripMenuItem.Size = new Size(153, 22);
        createFromSelectionToolStripMenuItem.Text = "From Selection";
        createFromSelectionToolStripMenuItem.Click += createFromSelectionToolStripMenuItem_Click;
        // 
        // createFromFilterToolStripMenuItem
        // 
        createFromFilterToolStripMenuItem.Name = "createFromFilterToolStripMenuItem";
        createFromFilterToolStripMenuItem.Size = new Size(153, 22);
        createFromFilterToolStripMenuItem.Text = "From Filter";
        createFromFilterToolStripMenuItem.Click += createFromFilterToolStripMenuItem_Click;
        // 
        // rebuildToolStripMenuItem
        // 
        rebuildToolStripMenuItem.Name = "rebuildToolStripMenuItem";
        rebuildToolStripMenuItem.Size = new Size(153, 22);
        rebuildToolStripMenuItem.Text = "Rebuild";
        rebuildToolStripMenuItem.Click += rebuildToolStripMenuItem_Click;
        // 
        // metaToolStripMenuItem
        // 
        metaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { readSelectedToolStripMenuItem, writeSelectedToolStripMenuItem });
        metaToolStripMenuItem.Enabled = false;
        metaToolStripMenuItem.Name = "metaToolStripMenuItem";
        metaToolStripMenuItem.Size = new Size(46, 20);
        metaToolStripMenuItem.Text = "Meta";
        // 
        // readSelectedToolStripMenuItem
        // 
        readSelectedToolStripMenuItem.Name = "readSelectedToolStripMenuItem";
        readSelectedToolStripMenuItem.Size = new Size(153, 22);
        readSelectedToolStripMenuItem.Text = "Read Selection";
        readSelectedToolStripMenuItem.Click += readSelectedToolStripMenuItem_Click;
        // 
        // writeSelectedToolStripMenuItem
        // 
        writeSelectedToolStripMenuItem.Name = "writeSelectedToolStripMenuItem";
        writeSelectedToolStripMenuItem.Size = new Size(153, 22);
        writeSelectedToolStripMenuItem.Text = "Write Selection";
        writeSelectedToolStripMenuItem.Click += writeSelectedToolStripMenuItem_Click;
        // 
        // infoToolStripMenuItem
        // 
        infoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { batchEditToolStripMenuItem, showAllArtistsToolStripMenuItem, showAllAlbumsToolStripMenuItem, showAllGenresToolStripMenuItem, showAllTagsToolStripMenuItem });
        infoToolStripMenuItem.Enabled = false;
        infoToolStripMenuItem.Name = "infoToolStripMenuItem";
        infoToolStripMenuItem.Size = new Size(40, 20);
        infoToolStripMenuItem.Text = "Info";
        // 
        // batchEditToolStripMenuItem
        // 
        batchEditToolStripMenuItem.Name = "batchEditToolStripMenuItem";
        batchEditToolStripMenuItem.Size = new Size(162, 22);
        batchEditToolStripMenuItem.Text = "Batch edit";
        batchEditToolStripMenuItem.Click += batchEditToolStripMenuItem_Click;
        // 
        // showAllArtistsToolStripMenuItem
        // 
        showAllArtistsToolStripMenuItem.Name = "showAllArtistsToolStripMenuItem";
        showAllArtistsToolStripMenuItem.Size = new Size(162, 22);
        showAllArtistsToolStripMenuItem.Text = "Show all Artists";
        showAllArtistsToolStripMenuItem.Click += showAllArtistsToolStripMenuItem_Click;
        // 
        // showAllAlbumsToolStripMenuItem
        // 
        showAllAlbumsToolStripMenuItem.Name = "showAllAlbumsToolStripMenuItem";
        showAllAlbumsToolStripMenuItem.Size = new Size(162, 22);
        showAllAlbumsToolStripMenuItem.Text = "Show all Albums";
        showAllAlbumsToolStripMenuItem.Click += showAllAlbumsToolStripMenuItem_Click;
        // 
        // showAllGenresToolStripMenuItem
        // 
        showAllGenresToolStripMenuItem.Name = "showAllGenresToolStripMenuItem";
        showAllGenresToolStripMenuItem.Size = new Size(162, 22);
        showAllGenresToolStripMenuItem.Text = "Show all Genres";
        showAllGenresToolStripMenuItem.Click += showAllGenresToolStripMenuItem_Click;
        // 
        // showAllTagsToolStripMenuItem
        // 
        showAllTagsToolStripMenuItem.Name = "showAllTagsToolStripMenuItem";
        showAllTagsToolStripMenuItem.Size = new Size(162, 22);
        showAllTagsToolStripMenuItem.Text = "Show all Tags";
        showAllTagsToolStripMenuItem.Click += showAllTagsToolStripMenuItem_Click;
        // 
        // filesToolStripMenuItem
        // 
        filesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { organizeToolStripMenuItem, organizeAndDeleteToolStripMenuItem, exportToolStripMenuItem });
        filesToolStripMenuItem.Enabled = false;
        filesToolStripMenuItem.Name = "filesToolStripMenuItem";
        filesToolStripMenuItem.Size = new Size(42, 20);
        filesToolStripMenuItem.Text = "Files";
        // 
        // organizeToolStripMenuItem
        // 
        organizeToolStripMenuItem.Name = "organizeToolStripMenuItem";
        organizeToolStripMenuItem.Size = new Size(179, 22);
        organizeToolStripMenuItem.Text = "Organize";
        organizeToolStripMenuItem.Click += organizeToolStripMenuItem_Click;
        // 
        // organizeAndDeleteToolStripMenuItem
        // 
        organizeAndDeleteToolStripMenuItem.Name = "organizeAndDeleteToolStripMenuItem";
        organizeAndDeleteToolStripMenuItem.Size = new Size(179, 22);
        organizeAndDeleteToolStripMenuItem.Text = "Organize and delete";
        organizeAndDeleteToolStripMenuItem.Click += organizeAndDeleteToolStripMenuItem_Click;
        // 
        // exportToolStripMenuItem
        // 
        exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        exportToolStripMenuItem.Size = new Size(179, 22);
        exportToolStripMenuItem.Text = "Export";
        exportToolStripMenuItem.Click += exportToolStripMenuItem_Click;
        // 
        // dgv_Media
        // 
        dgv_Media.AllowUserToAddRows = false;
        dgv_Media.AllowUserToDeleteRows = false;
        dgv_Media.AllowUserToResizeRows = false;
        dgv_Media.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv_Media.Columns.AddRange(new DataGridViewColumn[] { col_Title, col_Artists, col_Album, col_Track, col_State, col_Genres, col_Tags });
        dgv_Media.Location = new Point(12, 56);
        dgv_Media.Name = "dgv_Media";
        dgv_Media.Size = new Size(1391, 606);
        dgv_Media.TabIndex = 4;
        dgv_Media.CellContentClick += dgv_Media_CellContentClick;
        dgv_Media.CellFormatting += dgv_Media_CellFormatting;
        dgv_Media.SelectionChanged += dgv_Media_SelectionChanged;
        // 
        // col_Title
        // 
        col_Title.HeaderText = "Title";
        col_Title.MaxInputLength = 100;
        col_Title.Name = "col_Title";
        col_Title.Width = 300;
        // 
        // col_Artists
        // 
        col_Artists.HeaderText = "Artists";
        col_Artists.MaxInputLength = 100;
        col_Artists.Name = "col_Artists";
        col_Artists.Width = 250;
        // 
        // col_Album
        // 
        col_Album.HeaderText = "Album";
        col_Album.MaxInputLength = 100;
        col_Album.Name = "col_Album";
        col_Album.Width = 150;
        // 
        // col_Track
        // 
        col_Track.HeaderText = "#";
        col_Track.MaxInputLength = 3;
        col_Track.Name = "col_Track";
        col_Track.Width = 40;
        // 
        // col_State
        // 
        col_State.FlatStyle = FlatStyle.Flat;
        col_State.HeaderText = "!";
        col_State.Name = "col_State";
        col_State.Width = 25;
        // 
        // col_Genres
        // 
        col_Genres.HeaderText = "Genres";
        col_Genres.MaxInputLength = 100;
        col_Genres.Name = "col_Genres";
        col_Genres.Width = 150;
        // 
        // col_Tags
        // 
        col_Tags.HeaderText = "Tags";
        col_Tags.MaxInputLength = 160;
        col_Tags.Name = "col_Tags";
        col_Tags.Width = 400;
        // 
        // cmb_sort
        // 
        cmb_sort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cmb_sort.FormattingEnabled = true;
        cmb_sort.Items.AddRange(new object[] { "Unsorted", "Random", "Title", "Artist", "Album" });
        cmb_sort.Location = new Point(1282, 27);
        cmb_sort.Name = "cmb_sort";
        cmb_sort.Size = new Size(121, 23);
        cmb_sort.TabIndex = 3;
        cmb_sort.SelectedValueChanged += cmb_sort_SelectedValueChanged;
        // 
        // lbl_Info
        // 
        lbl_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lbl_Info.AutoSize = true;
        lbl_Info.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lbl_Info.Location = new Point(12, 665);
        lbl_Info.Name = "lbl_Info";
        lbl_Info.Size = new Size(22, 15);
        lbl_Info.TabIndex = 5;
        lbl_Info.Text = "???";
        // 
        // pb_Progress
        // 
        pb_Progress.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pb_Progress.Location = new Point(12, 56);
        pb_Progress.Name = "pb_Progress";
        pb_Progress.Size = new Size(168, 0);
        pb_Progress.TabIndex = 6;
        pb_Progress.Visible = false;
        // 
        // cmb_Filter
        // 
        cmb_Filter.FormattingEnabled = true;
        cmb_Filter.Items.AddRange(new object[] { "All" });
        cmb_Filter.Location = new Point(12, 27);
        cmb_Filter.Name = "cmb_Filter";
        cmb_Filter.Size = new Size(121, 23);
        cmb_Filter.TabIndex = 7;
        cmb_Filter.SelectedIndexChanged += cmb_Filter_SelectedIndexChanged;
        // 
        // timer_Unsaved
        // 
        timer_Unsaved.Enabled = true;
        timer_Unsaved.Interval = 250;
        timer_Unsaved.Tick += timer_Unsaved_Tick;
        // 
        // lbl_Unsaved
        // 
        lbl_Unsaved.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lbl_Unsaved.AutoSize = true;
        lbl_Unsaved.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lbl_Unsaved.Location = new Point(1271, 665);
        lbl_Unsaved.Name = "lbl_Unsaved";
        lbl_Unsaved.Size = new Size(132, 15);
        lbl_Unsaved.TabIndex = 8;
        lbl_Unsaved.Text = "[UNSAVED CHANGES!]";
        lbl_Unsaved.Visible = false;
        // 
        // Form_Overview
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1415, 689);
        Controls.Add(lbl_Unsaved);
        Controls.Add(cmb_Filter);
        Controls.Add(pb_Progress);
        Controls.Add(lbl_Info);
        Controls.Add(cmb_sort);
        Controls.Add(dgv_Media);
        Controls.Add(menuStrip);
        Name = "Form_Overview";
        Text = "LML";
        FormClosing += Form_Overview_FormClosing;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv_Media).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private MenuStrip menuStrip;
    private ToolStripMenuItem libraryToolStripMenuItem;
    private ToolStripMenuItem createToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private DataGridView dgv_Media;
    private ToolStripMenuItem mediaToolStripMenuItem;
    private ToolStripMenuItem addToolStripMenuItem;
    private ToolStripMenuItem addFolderToolStripMenuItem;
    private ToolStripMenuItem removeSelectedToolStripMenuItem;
    private ToolStripMenuItem deleteSelectedToolStripMenuItem;
    private ToolStripMenuItem organizeToolStripMenuItem;
    private ComboBox cmb_sort;
    private ToolStripMenuItem metaToolStripMenuItem;
    private ToolStripMenuItem readSelectedToolStripMenuItem;
    private ToolStripMenuItem writeSelectedToolStripMenuItem;
    private ToolStripMenuItem infoToolStripMenuItem;
    private ToolStripMenuItem showAllArtistsToolStripMenuItem;
    private ToolStripMenuItem showAllAlbumsToolStripMenuItem;
    private ToolStripMenuItem showAllGenresToolStripMenuItem;
    private ToolStripMenuItem showAllTagsToolStripMenuItem;
    private ToolStripMenuItem organizeAndDeleteToolStripMenuItem;
    private Label lbl_Info;
    private ProgressBar pb_Progress;
    private ToolStripMenuItem batchEditToolStripMenuItem;
    private ComboBox cmb_Filter;
    private ToolStripMenuItem playlistToolStripMenuItem;
    private ToolStripMenuItem createFromSelectionToolStripMenuItem;
    private ToolStripMenuItem createFromFilterToolStripMenuItem;
    private ToolStripMenuItem filterToolStripMenuItem;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem deleteToolStripMenuItem;
    private DataGridViewTextBoxColumn col_Title;
    private DataGridViewTextBoxColumn col_Artists;
    private DataGridViewTextBoxColumn col_Album;
    private DataGridViewTextBoxColumn col_Track;
    private DataGridViewButtonColumn col_State;
    private DataGridViewTextBoxColumn col_Genres;
    private DataGridViewTextBoxColumn col_Tags;
    private ToolStripMenuItem rebuildToolStripMenuItem;
    private ToolStripMenuItem filesToolStripMenuItem;
    private ToolStripMenuItem exportToolStripMenuItem;
    private System.Windows.Forms.Timer timer_Unsaved;
    private Label lbl_Unsaved;
}
