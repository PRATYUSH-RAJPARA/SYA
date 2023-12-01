using Serilog;
using System;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SYA
{
    public partial class addgold : Form
    {
        private SQLiteConnection connection;
        private const int ItemNameColumnIndex = 2;
        private const int ItemCaretColumnIndex = 3;
        private void addgold_Load(object sender, EventArgs e)
        {

            InitializeLogging();



            // Update row numbers initially
            UpdateRowNumbers();



        }
        public addgold()
        {
            InitializeComponent();
            InitializeDatabaseConnection();

            // Manually add columns to the DataGridView
            dataGridView1.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderText = "PR_CODE";
            textBoxColumn.Name = "prcode";
            dataGridView1.Columns.Add(textBoxColumn);
            dataGridView1.Columns["prcode"].Visible = false;
            InitializeComboBoxColumns();

            // Set the DataGridView DataSource to an empty DataTable
            dataGridView1.DataSource = GetEmptyDataTable();


            // Enable row headers for DataGridView to display row numbers
            dataGridView1.RowHeadersVisible = true;



            // Add an event handler to update row numbers when rows are added or removed
            dataGridView1.RowsAdded += (s, args) => UpdateRowNumbers();
            dataGridView1.RowsRemoved += (s, args) => UpdateRowNumbers();
        }
        private string previousTypeValue = string.Empty;
        private string previousCaretValue = string.Empty;

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Check if entering the first column of a new row
            if (e.ColumnIndex == 0 && e.RowIndex == dataGridView1.Rows.Count - 1)
            {
                // Copy values from the previous row's combo boxes
                if (dataGridView1.Rows.Count > 1)
                {
                    DataGridViewRow previousRow = dataGridView1.Rows[dataGridView1.Rows.Count - 2];

                    // Set the combo box values in the current row
                    DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["type"];
                    DataGridViewComboBoxCell caretCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["caret"];

                    typeCell.Value = previousRow.Cells["type"].Value;
                    caretCell.Value = previousRow.Cells["caret"].Value;

                    // Save the values for future reference
                    previousTypeValue = previousRow.Cells["type"].Value?.ToString();
                    previousCaretValue = previousRow.Cells["caret"].Value?.ToString();
                }
            }
        }
        private void UpdateRowNumbers()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = $"{row.Index + 1}";
            }
        }

        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()

                .WriteTo.File("C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\LOG\\logs.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
                .CreateLogger();
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

        private bool ValidateData()
        {

            for (int i = 0; i < (dataGridView1.Rows.Count); i++)
            {

                if (dataGridView1.Rows[i].Cells["tagno"].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells["tagno"].Value.ToString()))
                {

                    // Validate each column's data in the row
                    if (dataGridView1.Rows[i].Cells["type"].Value == null || string.IsNullOrWhiteSpace(dataGridView1.Rows[i].Cells["type"].Value.ToString()))
                    {
                        MessageBox.Show($"Please add a valid type for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "type");
                        return false;
                    }

                    if (!decimal.TryParse(dataGridView1.Rows[i].Cells["gross"].Value?.ToString(), out decimal grossWeight) || grossWeight < 0)
                    {
                        MessageBox.Show($"Gross weight should be a non-negative numeric value for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "gross");
                        return false;
                    }

                    if (!decimal.TryParse(dataGridView1.Rows[i].Cells["net"].Value?.ToString(), out decimal netWeight) || netWeight < 0)
                    {
                        MessageBox.Show($"Net weight should be a non-negative numeric value for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "net");
                        return false;
                    }

                    if (grossWeight < netWeight)
                    {
                        MessageBox.Show($"Gross weight should be greater than or equal to net weight for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "gross");
                        return false;
                    }

                    if (!decimal.TryParse(dataGridView1.Rows[i].Cells["labour"].Value?.ToString(), out decimal labour) || labour < 0)
                    {
                        MessageBox.Show($"Labour should be a non-negative numeric value for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "labour");
                        return false;
                    }

                    if (dataGridView1.Rows[i].Cells["other"].Value != null && (!decimal.TryParse(dataGridView1.Rows[i].Cells["other"].Value?.ToString(), out decimal other) || other < 0))
                    {
                        MessageBox.Show($"Other should be a non-negative numeric value for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "other");
                        return false;
                    }

                    string huid1 = dataGridView1.Rows[i].Cells["huid1"].Value?.ToString();
                    string huid2 = dataGridView1.Rows[i].Cells["huid2"].Value?.ToString();

                    if (!string.IsNullOrEmpty(huid1) && huid1.Length != 6)
                    {
                        MessageBox.Show($"HUID1 should contain exactly 6 characters for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "huid1");
                        return false;
                    }

                    if (!string.IsNullOrEmpty(huid2) && huid2.Length != 6)
                    {
                        MessageBox.Show($"HUID2 should contain exactly 6 characters for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "huid2");
                        return false;
                    }

                    if (string.IsNullOrEmpty(huid1) && !string.IsNullOrEmpty(huid2))
                    {
                        MessageBox.Show($"If HUID1 is null, please add HUID in the correct column for Row {i + 1}.");
                        SelectCell(dataGridView1, i, "huid1");
                        return false;
                    }
                }

            }

            return true; // All data is valid
        }

        private void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
        }




        private void SaveData()
        {
            // Check if there are rows in the DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("DataGridView is empty. Check your data population logic.");
                return;
            }
            // Call the ValidateData function
            if (!ValidateData())
            {
                // Validation failed, return or handle accordingly
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection(connection.ConnectionString))
            {
                con.Open();

                using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW, LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, COMMENT, ITEM_CODE, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, PRICE, STATUS, AC_CODE, AC_NAME) VALUES ( @tagNo, @type, @caret, @gross, @net, @labour, @other, @huid1, @huid2, @size, @comment, @prCode, @coYear, @coBook, @vchNo, @vchDate, @price, @status, @acCode, @acName)", con))
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Call UpdateTagNo for each row
                        UpdateTagNo(row.Index);

                        // Check if the row is not empty and the "tagno" cell is not null or empty
                        if (!row.IsNewRow && row.Cells["tagno"].Value != null && !string.IsNullOrEmpty(row.Cells["tagno"].Value.ToString()))
                        {
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

                            // Add the fixed data
                            string coYear = "2023-2024";
                            string coBook = "015";
                            string vchNo = "00000";
                            DateTime vchDate = DateTime.Now; // Current date and time
                            decimal price = 0;
                            string status = "INSTOCK";
                            string acCode = null; // Blank
                            string acName = null; // Blank

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

                            // Add parameters for fixed data
                            insertCommand.Parameters.AddWithValue("@coYear", coYear);
                            insertCommand.Parameters.AddWithValue("@coBook", coBook);
                            insertCommand.Parameters.AddWithValue("@vchNo", vchNo);
                            insertCommand.Parameters.AddWithValue("@vchDate", vchDate);
                            insertCommand.Parameters.AddWithValue("@price", price);
                            insertCommand.Parameters.AddWithValue("@status", status);
                            insertCommand.Parameters.AddWithValue("@acCode", acCode);
                            insertCommand.Parameters.AddWithValue("@acName", acName);

                            string executedQuery = insertCommand.CommandText;
                            string parameterValues = string.Join(", ", insertCommand.Parameters.Cast<SQLiteParameter>().Select(p => $"{p.ParameterName} = {p.Value}"));
                            Log.Information("Executed Query: {ExecutedQuery}, Parameters: {ParameterValues}", executedQuery, parameterValues);


                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
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
