using System;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;

namespace SYA
{
    public partial class SaleItem : Form
    {
        // Declare string variables for each field
        public string ID { get; set; }
        public string CO_YEAR { get; set; }
        public string CO_BOOK { get; set; }
        public string VCH_NO { get; set; }
        public string VCH_DATE { get; set; }
        public string TAG_NO { get; set; }
        public string GW { get; set; }
        public string NW { get; set; }
        public string LABOUR_AMT { get; set; }
        public string WHOLE_LABOUR_AMT { get; set; }
        public string OTHER_AMT { get; set; }
        public string IT_TYPE { get; set; }
        public string ITEM_CODE { get; set; }
        public string ITEM_PURITY { get; set; }
        public string ITEM_DESC { get; set; }
        public string HUID1 { get; set; }
        public string HUID2 { get; set; }
        public string SIZE { get; set; }
        public string PRICE { get; set; }
        public string STATUS { get; set; }
        public string AC_CODE { get; set; }
        public string AC_NAME { get; set; }
        public string COMMENT { get; set; }
        public string PRINT { get; set; }
        public string Item { get; set; }

        public SaleItem()
        {
            InitializeComponent();
        }

        private void SaleItem_Load(object sender, EventArgs e)
        {
        }

        private void LoadData(string query)
        {
            try
            {
                using (SQLiteDataReader reader = helper.FetchDataFromSYADataBase(query))
                {
                    if (reader.HasRows && reader.Read())
                    {
                        DataTable transposedTable = TransposeDataReader(reader);

                        LoadDataIntoDataGridView(transposedTable);

                        // Set SaleItem properties based on transposed data
                        SetSaleItemProperties(this, transposedTable);
                    }
                    else
                    {
                        MessageBox.Show("No rows found in the result set.", "Information");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void LoadDataIntoDataGridView(DataTable dataTable)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.RowHeadersVisible = false;

            foreach (DataColumn col in dataTable.Columns)
                dataGridView1.Columns.Add(col.ColumnName, col.ColumnName);

            dataGridView1.Columns[0].Width = 198;
            dataGridView1.Columns[1].Width = 198;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataRow row in dataTable.Rows)
                dataGridView1.Rows.Add(row.ItemArray);
        }

        private DataTable TransposeDataReader(SQLiteDataReader reader)
        {
            DataTable transposedTable = new DataTable();
            transposedTable.Columns.Add("Field", typeof(string));
            transposedTable.Columns.Add("Value", typeof(object));

            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    transposedTable.Rows.Add(reader.GetName(i), reader[i]);
            }

            return transposedTable;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string query = "SELECT * FROM MAIN_DATA_VERIFIED WHERE TAG_NO = '" + textBox1.Text + "'";
                LoadData(query);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Additional logic for button click if needed
        }

        private void SetSaleItemProperties(SaleItem saleItem, DataTable transposedTable)
        {
            foreach (DataRow row in transposedTable.Rows)
            {
                string columnName = row["Field"].ToString();
                var property = typeof(SaleItem).GetProperty(columnName);

                if (property != null)
                {
                    object value = Convert.ChangeType(row["Value"], property.PropertyType);
                    property.SetValue(saleItem, value);
                }
            }
        }
    }
}
