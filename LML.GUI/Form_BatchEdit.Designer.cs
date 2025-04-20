namespace LML.GUI
{
    partial class Form_BatchEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_OK = new Button();
            lbl_Artist = new Label();
            tb_Artists = new TextBox();
            tb_Album = new TextBox();
            lbl_Album = new Label();
            tb_Genres = new TextBox();
            lbl_Genre = new Label();
            tb_Tags = new TextBox();
            lbl_Tags = new Label();
            SuspendLayout();
            // 
            // btn_OK
            // 
            btn_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_OK.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_OK.Location = new Point(12, 128);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(369, 23);
            btn_OK.TabIndex = 0;
            btn_OK.Text = "OK";
            btn_OK.UseVisualStyleBackColor = true;
            btn_OK.Click += btn_OK_Click;
            // 
            // lbl_Artist
            // 
            lbl_Artist.AutoSize = true;
            lbl_Artist.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Artist.Location = new Point(12, 15);
            lbl_Artist.Name = "lbl_Artist";
            lbl_Artist.Size = new Size(43, 15);
            lbl_Artist.TabIndex = 1;
            lbl_Artist.Text = "Artists";
            // 
            // tb_Artists
            // 
            tb_Artists.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_Artists.Location = new Point(108, 12);
            tb_Artists.Name = "tb_Artists";
            tb_Artists.Size = new Size(273, 23);
            tb_Artists.TabIndex = 2;
            // 
            // tb_Album
            // 
            tb_Album.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_Album.Location = new Point(108, 41);
            tb_Album.Name = "tb_Album";
            tb_Album.Size = new Size(273, 23);
            tb_Album.TabIndex = 4;
            // 
            // lbl_Album
            // 
            lbl_Album.AutoSize = true;
            lbl_Album.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Album.Location = new Point(12, 44);
            lbl_Album.Name = "lbl_Album";
            lbl_Album.Size = new Size(43, 15);
            lbl_Album.TabIndex = 3;
            lbl_Album.Text = "Album";
            // 
            // tb_Genres
            // 
            tb_Genres.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_Genres.Location = new Point(108, 70);
            tb_Genres.Name = "tb_Genres";
            tb_Genres.Size = new Size(273, 23);
            tb_Genres.TabIndex = 6;
            // 
            // lbl_Genre
            // 
            lbl_Genre.AutoSize = true;
            lbl_Genre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Genre.Location = new Point(12, 73);
            lbl_Genre.Name = "lbl_Genre";
            lbl_Genre.Size = new Size(47, 15);
            lbl_Genre.TabIndex = 5;
            lbl_Genre.Text = "Genres";
            // 
            // tb_Tags
            // 
            tb_Tags.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_Tags.Location = new Point(108, 99);
            tb_Tags.Name = "tb_Tags";
            tb_Tags.Size = new Size(273, 23);
            tb_Tags.TabIndex = 8;
            // 
            // lbl_Tags
            // 
            lbl_Tags.AutoSize = true;
            lbl_Tags.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Tags.Location = new Point(12, 102);
            lbl_Tags.Name = "lbl_Tags";
            lbl_Tags.Size = new Size(31, 15);
            lbl_Tags.TabIndex = 7;
            lbl_Tags.Text = "Tags";
            // 
            // Form_BatchEdit
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(393, 163);
            Controls.Add(tb_Tags);
            Controls.Add(lbl_Tags);
            Controls.Add(tb_Genres);
            Controls.Add(lbl_Genre);
            Controls.Add(tb_Album);
            Controls.Add(lbl_Album);
            Controls.Add(tb_Artists);
            Controls.Add(lbl_Artist);
            Controls.Add(btn_OK);
            Name = "Form_BatchEdit";
            Text = "Batch Edit";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_OK;
        private Label lbl_Artist;
        private TextBox tb_Artists;
        private TextBox tb_Album;
        private Label lbl_Album;
        private TextBox tb_Genres;
        private Label lbl_Genre;
        private TextBox tb_Tags;
        private Label lbl_Tags;
    }
}