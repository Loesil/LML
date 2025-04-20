namespace LML.GUI
{
    partial class Form_Lookup
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
            lv_Lookup = new ListView();
            col_Number = new ColumnHeader();
            col_Name = new ColumnHeader();
            btn_OK = new Button();
            tb_Search = new TextBox();
            SuspendLayout();
            // 
            // lv_Lookup
            // 
            lv_Lookup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lv_Lookup.Columns.AddRange(new ColumnHeader[] { col_Number, col_Name });
            lv_Lookup.FullRowSelect = true;
            lv_Lookup.GridLines = true;
            lv_Lookup.Location = new Point(12, 12);
            lv_Lookup.Name = "lv_Lookup";
            lv_Lookup.Size = new Size(476, 368);
            lv_Lookup.TabIndex = 0;
            lv_Lookup.UseCompatibleStateImageBehavior = false;
            lv_Lookup.View = View.Details;
            lv_Lookup.SelectedIndexChanged += lv_Lookup_SelectedIndexChanged;
            // 
            // col_Number
            // 
            col_Number.Text = "#";
            // 
            // col_Name
            // 
            col_Name.Text = "Name";
            col_Name.Width = 350;
            // 
            // btn_OK
            // 
            btn_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_OK.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_OK.Location = new Point(12, 415);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(476, 23);
            btn_OK.TabIndex = 1;
            btn_OK.Text = "OK";
            btn_OK.UseVisualStyleBackColor = true;
            btn_OK.Click += btn_OK_Click;
            // 
            // tb_Search
            // 
            tb_Search.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_Search.Location = new Point(12, 386);
            tb_Search.Name = "tb_Search";
            tb_Search.Size = new Size(476, 23);
            tb_Search.TabIndex = 2;
            tb_Search.KeyPress += tb_Search_KeyPress;
            // 
            // Form_Lookup
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 450);
            Controls.Add(tb_Search);
            Controls.Add(btn_OK);
            Controls.Add(lv_Lookup);
            Name = "Form_Lookup";
            ShowInTaskbar = false;
            Text = "Lookup";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lv_Lookup;
        private Button btn_OK;
        private TextBox tb_Search;
        private ColumnHeader col_Number;
        private ColumnHeader col_Name;
    }
}