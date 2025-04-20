using System.ComponentModel;
using System.Runtime.InteropServices;

namespace LML.GUI
{
    public partial class Form_Lookup : Form
    {
        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilterResult { get; set; }

        private Dictionary<string, int> lookupData { get; set; }

        private List<string> sortedLookupData { get; set; }
        #endregion

        #region Create
        public Form_Lookup()
        {
            lookupData = new Dictionary<string, int>();
            sortedLookupData = new List<string>();
            FilterResult = "";
            InitializeComponent();
        }

        public Form_Lookup(Dictionary<string, int> data)
            : this()
        {
            lookupData = new Dictionary<string, int>();
            List<KeyValuePair<string, int>> d = data.ToList();
            d.Sort((a, b) => string.Compare(a.Key, b.Key));
            d.ForEach(d => lookupData.Add(d.Key, d.Value));
            sortedLookupData = d.ConvertAll(d => d.Key);

            // add items
            foreach (var kvp in lookupData)
            {
                ListViewItem i = lv_Lookup.Items.Add(kvp.Value.ToString());
                i.SubItems.Add(kvp.Key);
            }
        }
        #endregion

        #region UI Events

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (!lookupData.ContainsKey(tb_Search.Text))
            {
                MessageBox.Show("Value not found");
                return;
            }

            FilterResult = tb_Search.Text;
            DialogResult = DialogResult.OK;
        }
        private void lv_Lookup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_Lookup.SelectedItems.Count == 1)
                tb_Search.Text = lv_Lookup.SelectedItems[0].SubItems[1].Text;
        }

        private void tb_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            // lookup after keypress
            string t = tb_Search.Text;
            string? selected = lv_Lookup.SelectedItems.Count > 0 ? lv_Lookup.SelectedItems[0].SubItems[1].Text : null;

            if (selected != null && selected.StartsWith(t, StringComparison.OrdinalIgnoreCase)) return; // already selected
            string? match = sortedLookupData.Find(s => s.StartsWith(t, StringComparison.OrdinalIgnoreCase));
            if (match == null) return; // no match

            // select match
            int index = sortedLookupData.IndexOf(match);
            lv_Lookup.SelectedItems.Clear();
            lv_Lookup.Items[index].Selected = true;
            lv_Lookup.EnsureVisible(lv_Lookup.Items[index].Index);
        }
        #endregion
    }
}
