using System.ComponentModel;

namespace LML.GUI
{
    public partial class Form_BatchEdit : Form
    {
        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Result_Artists { get => tb_Artists.Text; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Result_Album { get => tb_Album.Text; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Result_Genres { get => tb_Genres.Text; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? Result_Tags { get => tb_Tags.Text; }
        #endregion

        public Form_BatchEdit()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
