namespace LML.GUI
{
    partial class Form_Filter
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
            lbl_Name = new Label();
            tb_Name = new TextBox();
            tb_Filter = new TextBox();
            lbl_Filter = new Label();
            lv_Info = new ListView();
            col_Type = new ColumnHeader();
            col_Property = new ColumnHeader();
            col_Operators = new ColumnHeader();
            lbl_Info = new Label();
            SuspendLayout();
            // 
            // btn_OK
            // 
            btn_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btn_OK.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn_OK.Location = new Point(12, 415);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(393, 23);
            btn_OK.TabIndex = 0;
            btn_OK.Text = "OK";
            btn_OK.UseVisualStyleBackColor = true;
            btn_OK.Click += btn_OK_Click;
            // 
            // lbl_Name
            // 
            lbl_Name.AutoSize = true;
            lbl_Name.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Name.Location = new Point(12, 9);
            lbl_Name.Name = "lbl_Name";
            lbl_Name.Size = new Size(99, 15);
            lbl_Name.TabIndex = 1;
            lbl_Name.Text = "Name: (optional)";
            // 
            // tb_Name
            // 
            tb_Name.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_Name.Location = new Point(12, 27);
            tb_Name.Name = "tb_Name";
            tb_Name.Size = new Size(393, 23);
            tb_Name.TabIndex = 2;
            // 
            // tb_Filter
            // 
            tb_Filter.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_Filter.Location = new Point(12, 95);
            tb_Filter.Multiline = true;
            tb_Filter.Name = "tb_Filter";
            tb_Filter.PlaceholderText = "Enter filter expression (e.g. genres <=\"Rock\" & artists <= \"Gorillaz\")";
            tb_Filter.Size = new Size(393, 111);
            tb_Filter.TabIndex = 3;
            // 
            // lbl_Filter
            // 
            lbl_Filter.AutoSize = true;
            lbl_Filter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Filter.Location = new Point(12, 77);
            lbl_Filter.Name = "lbl_Filter";
            lbl_Filter.Size = new Size(39, 15);
            lbl_Filter.TabIndex = 4;
            lbl_Filter.Text = "Filter:";
            // 
            // lv_Info
            // 
            lv_Info.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lv_Info.Columns.AddRange(new ColumnHeader[] { col_Type, col_Property, col_Operators });
            lv_Info.FullRowSelect = true;
            lv_Info.GridLines = true;
            lv_Info.Location = new Point(12, 241);
            lv_Info.Name = "lv_Info";
            lv_Info.Size = new Size(393, 168);
            lv_Info.TabIndex = 5;
            lv_Info.UseCompatibleStateImageBehavior = false;
            lv_Info.View = View.Details;
            lv_Info.DoubleClick += lv_Info_DoubleClick;
            // 
            // col_Type
            // 
            col_Type.Text = "Type";
            col_Type.Width = 75;
            // 
            // col_Property
            // 
            col_Property.Text = "Property";
            col_Property.Width = 150;
            // 
            // col_Operators
            // 
            col_Operators.Text = "Operators";
            col_Operators.Width = 150;
            // 
            // lbl_Info
            // 
            lbl_Info.AutoSize = true;
            lbl_Info.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl_Info.Location = new Point(12, 223);
            lbl_Info.Name = "lbl_Info";
            lbl_Info.Size = new Size(33, 15);
            lbl_Info.TabIndex = 6;
            lbl_Info.Text = "Info:";
            // 
            // Form_Filter
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(417, 450);
            Controls.Add(lbl_Info);
            Controls.Add(lv_Info);
            Controls.Add(lbl_Filter);
            Controls.Add(tb_Filter);
            Controls.Add(tb_Name);
            Controls.Add(lbl_Name);
            Controls.Add(btn_OK);
            Name = "Form_Filter";
            Text = "Filter";
            Load += Form_Filter_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_OK;
        private Label lbl_Name;
        private TextBox tb_Name;
        private TextBox tb_Filter;
        private Label lbl_Filter;
        private ListView lv_Info;
        private ColumnHeader col_Type;
        private ColumnHeader col_Property;
        private ColumnHeader col_Operators;
        private Label lbl_Info;
    }
}