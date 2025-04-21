using System.ComponentModel;
using LML.Core.Filters;
using LML.Core.Models;

namespace LML.GUI
{
    public partial class Form_Filter : Form
    {
        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilterName { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFilter? Filter { get; set; }

        #endregion

        #region Create
        public Form_Filter()
        {
            FilterName = "";
            InitializeComponent();
        }

        public Form_Filter(string filterName, IFilter? filter)
            : this()
        {
            FilterName = filterName;
            Filter = filter;

            // name
            tb_Name.Text = filterName;
            if (filterName != "") tb_Name.ReadOnly = true;

            // filter
            if (Filter != null) tb_Filter.Text = Filter.GetFilterDescription();
        }
        #endregion

        #region UI Events
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                FilterName = tb_Name.Text;
                Filter = FilterParser.ParseFilter(tb_Filter.Text);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form_Filter_Load(object sender, EventArgs e)
        {
            Dictionary<MediaPropertyType, string> allowedOperators = new Dictionary<MediaPropertyType, string>()
            {
                { MediaPropertyType.Boolean, "" },
                { MediaPropertyType.String, "== <= <" },
                { MediaPropertyType.StringList, "== <= <" },
                { MediaPropertyType.Uint, "== != < <= > >=" },
            };

            lv_Info.Items.Clear();
            foreach (MediaProperty property in Enum.GetValues(typeof(MediaProperty)))
            {
                MediaPropertyType type = (MediaPropertyType)Filter_Property.PropertyToPropertyType(property)!;
                lv_Info.Items.Add(new ListViewItem(new[]
                {
                    type.ToString(),
                    Filter_Property.PropertyToName(property)!,
                    allowedOperators[type].ToString()
                }));
            }
        }

        private void lv_Info_DoubleClick(object sender, EventArgs e)
        {
            if (lv_Info.SelectedItems.Count != 1) return;
            tb_Filter.AppendText(lv_Info.SelectedItems[0].SubItems[1].Text);
        }
        #endregion
    }
}
