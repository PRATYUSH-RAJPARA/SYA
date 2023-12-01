using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SYA
{
    public partial class addgold : Form
    {
        private SQLiteConnection connection;
        private const int ItemNameColumnIndex = 2;
        private const int ItemCaretColumnIndex = 3;

        public addgold()
        {
            InitializeComponent();
            InitializeDatabaseConnection();

            // Manually add columns to the DataGridView
            dataGridView1.AutoGenerateColumns = false;
            // Manually add columns to the DataGridView
            dataGridView1.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderText = "PR_CODE";
            textBoxColumn.Name = "prcode";
            dataGridView1.Columns.Add(textBoxColumn);
            dataGridView1.Columns["prcode"].Visible = false;
            InitializeComboBoxColumns();

            // Set the DataGridView DataSource to the new DataTable
            dataGridView1.DataSource = GetEmptyDataTable();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new SQLiteConnection("Data Source=C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\SYADataBase.db;Version=3;");
        }

        private void InitializeComboBoxColumns()
        {
            // Load TYPE values
            LoadComboBoxValues("G", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)dataGridView1.Columns["type"]);

            // Load CARET values
            LoadComboBoxValues("GQ", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)dataGridView1.Columns["caret"]);
        }

        private void LoadComboBoxValues(string itemType, string columnName, string displayMember, DataGridViewComboBoxColumn comboBoxColumn)
        {
            using (SQLiteConnection con = new SQLiteConnection(connection.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand($"SELECT DISTINCT {columnName} FROM ITEM_MASTER WHERE IT_TYPE = '{itemType}'", con))
                {
                    con.Open();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxColumn.Items.Add(reader[displayMember].ToString());
                        }
                    }
                }
            }
        }

        private void addgold_Load(object sender, EventArgs e)
        {
            // Code for form load event
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //  if (e.ColumnIndex == ItemCaretColumnIndex)
            //  {
            //      UpdateTagNo(e.RowIndex);
            //  }
        }

        // Modify the UpdateTagNo method
        private void UpdateTagNo(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dataGridView1.Rows.Count)
            {
                if (dataGridView1.Columns.Count > ItemNameColumnIndex)
                {
                    object itemNameObject = dataGridView1.Rows[rowIndex].Cells[ItemNameColumnIndex].Value;

                    if (itemNameObject != null)
                    {
                        string itemName = itemNameObject.ToString();
                        string caret = dataGridView1.Rows[rowIndex].Cells["caret"].Value?.ToString();
                        string prCode = GetPRCode(itemName);

                        string prefix = "SYA";
                        int newSequenceNumber = GetNextSequenceNumber(prefix, prCode, caret);

                        if (!string.IsNullOrEmpty(caret) && !string.IsNullOrEmpty(prCode))
                        {
                            string newTagNo = $"{prefix}{caret}{prCode}{newSequenceNumber:D5}";
                            dataGridView1.Rows[rowIndex].Cells["tagno"].Value = newTagNo;
                            dataGridView1.Rows[rowIndex].Cells["prcode"].Value = prCode; // Set PR_CODE in the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("CARET or PR_CODE is empty. Cannot generate Tag No.");
                        }
                    }
                }
            }
        }


        private int GetNextSequenceNumber(string prefix, string prCode, string caret)
        {
            int prefixLength = prefix.Length + prCode.Length + caret.Length;

            using (SQLiteConnection con = new SQLiteConnection(connection.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand($"SELECT MAX(CAST(SUBSTR(TAG_NO, {prefixLength + 1}) AS INTEGER)) FROM MAIN_DATA WHERE ITEM_CODE = '{prCode}'", con))
                {
                    con.Open();
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        return Convert.ToInt32(result) + 1;
                    }
                    return 1;
                }
            }
        }

        private string GetPRCode(string itemName)
        {
            using (SQLiteConnection con = new SQLiteConnection(connection.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand($"SELECT PR_CODE FROM ITEM_MASTER WHERE IT_NAME = '{itemName}' AND IT_TYPE = 'G'", con))
                {
                    con.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                    return null;
                }
            }
        }

        private DataTable GetEmptyDataTable()
        {
            DataTable dataTable = new DataTable();

            // Add columns to match your DataGridView
            dataTable.Columns.Add("select", typeof(bool));
            dataTable.Columns.Add("tagno", typeof(string));
            dataTable.Columns.Add("type", typeof(string));
            dataTable.Columns.Add("caret", typeof(string));
            dataTable.Columns.Add("gross", typeof(decimal));
            dataTable.Columns.Add("net", typeof(decimal));
            dataTable.Columns.Add("labour", typeof(decimal));
            dataTable.Columns.Add("other", typeof(decimal));
            dataTable.Columns.Add("huid1", typeof(string));
            dataTable.Columns.Add("huid2", typeof(string));
            dataTable.Columns.Add("size", typeof(string));
            dataTable.Columns.Add("comment", typeof(string));
            dataTable.Columns.Add("prcode", typeof(string));

            return dataTable;
        }

        private void btnAddGoldSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        // Modify the SaveData method to call UpdateTagNo for each row before saving
        private void SaveData()
        {
            // Check if there are rows in the DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("DataGridView is empty. Check your data population logic.");
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection(connection.ConnectionString))
            {
                con.Open();

                using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW, LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, COMMENT, ITEM_CODE) VALUES ( @tagNo, @type, @caret, @gross, @net, @labour, @other, @huid1, @huid2, @size, @comment, @prCode)", con))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        UpdateTagNo(row.Index); // Update Tag No for each row

                        // Rest of the code remains the same...
                        string tagNo = row.Cells["tagno"].Value?.ToString();
                        string type = row.Cells["type"].Value?.ToString();
                        string caret = row.Cells["caret"].Value?.ToString();
                        decimal gross = Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value);
                        decimal net = Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value);
                        decimal labour = Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value);
                        decimal other = Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value);
                        string huid1 = row.Cells["huid1"].Value?.ToString();
                        string huid2 = row.Cells["huid2"].Value?.ToString();
                        string size = row.Cells["size"].Value?.ToString();
                        string comment = row.Cells["comment"].Value?.ToString();
                        string prCode = row.Cells["prcode"].Value?.ToString();

                        // Insert new data into the database
                        insertCommand.Parameters.AddWithValue("@tagNo", tagNo);
                        insertCommand.Parameters.AddWithValue("@type", type);
                        insertCommand.Parameters.AddWithValue("@caret", caret);
                        insertCommand.Parameters.AddWithValue("@gross", gross);
                        insertCommand.Parameters.AddWithValue("@net", net);
                        insertCommand.Parameters.AddWithValue("@labour", labour);
                        insertCommand.Parameters.AddWithValue("@other", other);
                        insertCommand.Parameters.AddWithValue("@huid1", huid1);
                        insertCommand.Parameters.AddWithValue("@huid2", huid2);
                        insertCommand.Parameters.AddWithValue("@size", size);
                        insertCommand.Parameters.AddWithValue("@comment", comment);
                        insertCommand.Parameters.AddWithValue("@prCode", prCode);

                        insertCommand.ExecuteNonQuery();
                        insertCommand.Parameters.Clear();
                    }
                }
            }
        }

        private void btnAddGoldPrintTag_Click(object sender, EventArgs e)
        {
            PrintData();
        }

        private void PrintData()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells["select"].Value)
                {
                    // Print the selected row data
                    string itemCode = row.Cells["ItemCode"].Value.ToString();
                    string tagNo = row.Cells["tagno"].Value.ToString();

                    // Perform printing logic here
                    MessageBox.Show($"Printing: Item Code - {itemCode}, Tag No - {tagNo}");
                }
            }
        }
    }
}
