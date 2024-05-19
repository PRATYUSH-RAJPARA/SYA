using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
namespace SYA
{
    public partial class RTGSList : Form
    {
        private DataTable originalRTGSList;
        private string selectedID;
        public RTGSList()
        {
            InitializeComponent();
        }
        private void RTGSList_Load(object sender, EventArgs e)
        {
            originalRTGSList = helper.FetchDataTableFromSYADataBase("SELECT ID,BName,BAcNo,BAddress FROM RTGSData ORDER BY BName ASC");
            dataGridView1.DataSource = originalRTGSList;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.ReadOnly = true;
            dataGridView1.Focus();
        }
        private void TextFilter(char keyPressed)
        {
            textBoxFilter.Text += keyPressed;
        }
        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            string filterText = textBoxFilter.Text.ToLower();
            DataTable filteredData = originalRTGSList.Clone();
            foreach (DataRow row in originalRTGSList.Rows)
            {
                if (row.ItemArray.Any(field => field.ToString().ToLower().Contains(filterText)))
                {
                    filteredData.ImportRow(row);
                }
            }
            dataGridView1.DataSource = filteredData;
            dataGridView1.Focus();
        }
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                if (!string.IsNullOrEmpty(textBoxFilter.Text))
                {
                    textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.Length - 1);
                    textBoxFilter_TextChanged(sender, e);
                }
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                    selectedID = dataGridView1.CurrentRow.Cells[0].Value.ToString(); ;
                    this.Close();
            }
            else
            {
                char keyPressed = e.KeyChar;
                TextFilter(keyPressed);
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Prevent the default behavior
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    selectedID = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();
                    this.Close();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (!string.IsNullOrEmpty(textBoxFilter.Text))
                {
                    textBoxFilter.Text = textBoxFilter.Text.Remove(textBoxFilter.Text.Length - 1);
                    textBoxFilter_TextChanged(sender, e);
                }
            }
        }
        public string GetSelectedID()
        {
            return selectedID;
        }
    }
}
