﻿using Serilog;
using System;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace SYA
{
    public partial class addgold : Form
    {
        private SQLiteConnection connectionToSYADatabase;
        private SQLiteConnection connectionToDatacare;
        private const int ItemNameColumnIndex = 2;
        private string previousTypeValue = string.Empty;
        private string previousCaretValue = string.Empty;
        private int itemCount = 0;
        private decimal grossWeightSum = 0;
        public addgold()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            // Manually add columns to the DataGridView
            dataGridView1.AutoGenerateColumns = false;
            gridviewstyle();
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
        private void addgold_Load(object sender, EventArgs e)
        {

            InitializeLogging();



            // Update row numbers initially
            UpdateRowNumbers();



        }
        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection("Data Source=C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\SYADataBase.db;Version=3;");
            connectionToDatacare = new SQLiteConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\pvraj\\OneDrive\\Desktop\\DataCare23 - Copy.mdb\";");

        }
        private void InitializeComboBoxColumns()
        {
            // Load TYPE values
            LoadComboBoxValues("G", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)dataGridView1.Columns["type"]);

            // Load CARET values
            LoadComboBoxValues("GQ", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)dataGridView1.Columns["caret"]);
        }
        // --------------------------------------------------------------------------------------------
        // Events
        // --------------------------------------------------------------------------------------------
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell is DataGridViewTextBoxCell)
            {
                dataGridView1.BeginEdit(true);

                // Select all text in the cell
                if (dataGridView1.EditingControl is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }
            // Check if entering the first column of a new row
            if (e.ColumnIndex == 0 && e.RowIndex == dataGridView1.Rows.Count - 1)
            {
                UpdateItemCountAndGrossWeight();
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
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Check if the edited cell is in the "gross" column
                if (dataGridView1.Columns[e.ColumnIndex].Name == "gross")
                {
                    // Copy the value from the "gross" column to the corresponding "net" column
                    dataGridView1.Rows[e.RowIndex].Cells["net"].Value = dataGridView1.Rows[e.RowIndex].Cells["gross"].Value;

                    // Update the item count and gross weight sum
                    UpdateItemCountAndGrossWeight();
                }
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update item count and gross weight sum when cell values change
            // UpdateItemCountAndGrossWeight();
        }
        // --------------------------------------------------------------------------------------------
        // Logging
        // --------------------------------------------------------------------------------------------
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()

                .WriteTo.File("C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\LOG\\logs.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
                .CreateLogger();
        }
        // --------------------------------------------------------------------------------------------
        // Styling Handling
        // --------------------------------------------------------------------------------------------
        private void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
        }
        private void gridviewstyle()
        {
            // Customize DataGridView appearance
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Set cell alignment to middle center
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for cells
                column.DefaultCellStyle.Font = new Font("Arial", 10); // Adjust the font and size as needed

                // Set column width
                if (column.Name == "select") // Adjust the column name
                {
                    column.Width = 40; // Adjust the width as needed
                }
                else if (column.Name == "type") // Adjust the column name
                {
                    column.Width = 250; // Adjust the width as needed
                }
                else if (column.Name == "tagno") // Adjust the column name
                {
                    column.Width = 150; // Adjust the width as needed
                }
                // Add more conditions for other columns as needed
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for column headers
                column.HeaderCell.Style.Font = new Font("Arial", 12, FontStyle.Bold); // Adjust the font and size as needed

            }
        }
        private void UpdateItemCountAndGrossWeight()
        {
            // Reset counts
            itemCount = 0;
            grossWeightSum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the row is not empty and not in edit mode
                //if (!row.IsNewRow && !row.DataGridView.IsCurrentRowDirty)
                if (!row.IsNewRow)
                {
                    itemCount++;

                    // Check if the "gross" cell is not null or empty
                    if (row.Cells["gross"].Value != null && decimal.TryParse(row.Cells["gross"].Value.ToString(), out decimal gross))
                    {
                        grossWeightSum += gross;
                    }
                }
            }

            // Update the TextBox with the latest counts
            UpdateItemCountAndGrossWeightTextBox();
        }
        private void UpdateItemCountAndGrossWeightTextBox()
        {
            // Assuming you have a TextBox named textBoxItemCountGrossWeight on your form
            itemcountandgrossweight.Text = $"Total Item Count: {itemCount}, Total Gross Weight: {grossWeightSum}";
        }
        private void UpdateRowNumbers()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = $"{row.Index + 1}";
            }
        }
        private void LoadComboBoxValues(string itemType, string columnName, string displayMember, DataGridViewComboBoxColumn comboBoxColumn)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionToSYADatabase.ConnectionString))
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
        // --------------------------------------------------------------------------------------------
        // Save Data
        // --------------------------------------------------------------------------------------------
        private void btnAddGoldSave_Click(object sender, EventArgs e)
        {
            UpdateItemCountAndGrossWeight();
            SaveData();
        }
        private void SaveData()
        {
            // Check if there are rows in the DataGridView
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("DataGridView is empty. Check your data population logic.");
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection(connectionToSYADatabase.ConnectionString))
            {
                con.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Check if the row is not empty
                    if (!row.IsNewRow)
                    {


                        // Check if the "tagno" cell is not null or empty
                        if (row.Cells["tagno"].Value != null && !string.IsNullOrEmpty(row.Cells["tagno"].Value.ToString()))
                        {
                            // If tagno is generated, update the existing entry in the database
                            UpdateData(row, con);
                        }
                        else
                        {
                            // If tagno is not generated, insert a new entry in the database
                            InsertData(row, con);
                        }
                    }
                }
            }
        }
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

            using (SQLiteConnection con = new SQLiteConnection(connectionToSYADatabase.ConnectionString))
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
            using (SQLiteConnection con = new SQLiteConnection(connectionToSYADatabase.ConnectionString))
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
        private bool ValidateData(DataGridViewRow row)
        {
            // Validate each column's data in the row
            // The validation logic can be customized based on your requirements
            // For example, you can check if the "type" column is not null or empty



            // Validate each column's data in the row
            if (row.Cells["type"].Value == null || string.IsNullOrWhiteSpace(row.Cells["type"].Value.ToString()))
            {
                MessageBox.Show($"Please add a valid type for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "type");
                return false;
            }

            if (!decimal.TryParse(row.Cells["gross"].Value?.ToString(), out decimal grossWeight) || grossWeight < 0)
            {
                MessageBox.Show($"Gross weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "gross");
                return false;
            }

            if (!decimal.TryParse(row.Cells["net"].Value?.ToString(), out decimal netWeight) || netWeight < 0)
            {
                MessageBox.Show($"Net weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "net");
                return false;
            }

            if (grossWeight < netWeight)
            {
                MessageBox.Show($"Gross weight should be greater than or equal to net weight for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "gross");
                return false;
            }

            if (!decimal.TryParse(row.Cells["labour"].Value?.ToString(), out decimal labour) || labour < 0)
            {
                MessageBox.Show($"Labour should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "labour");
                return false;
            }

            if (row.Cells["other"].Value != null && (!decimal.TryParse(row.Cells["other"].Value?.ToString(), out decimal other) || other < 0))
            {
                MessageBox.Show($"Other should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "other");
                return false;
            }

            string huid1 = row.Cells["huid1"].Value?.ToString();
            string huid2 = row.Cells["huid2"].Value?.ToString();

            if (!string.IsNullOrEmpty(huid1) && huid1.Length != 6)
            {
                MessageBox.Show($"HUID1 should contain exactly 6 characters for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "huid1");
                return false;
            }

            if (!string.IsNullOrEmpty(huid2) && huid2.Length != 6)
            {
                MessageBox.Show($"HUID2 should contain exactly 6 characters for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "huid2");
                return false;
            }

            if (string.IsNullOrEmpty(huid1) && !string.IsNullOrEmpty(huid2))
            {
                MessageBox.Show($"If HUID1 is null, please add HUID in the correct column for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "huid1");
                return false;
            }




            return true; // All data is valid
        }
        private void UpdateData(DataGridViewRow row, SQLiteConnection con)
        {
            if (!ValidateData(row))
            {
                // Validation failed, return or handle accordingly
                return;
            }
            using (SQLiteCommand updateCommand = new SQLiteCommand("UPDATE MAIN_DATA SET ITEM_DESC = @type, ITEM_PURITY = @caret, GW = @gross, NW = @net, LABOUR_AMT = @labour, OTHER_AMT = @other, HUID1 = @huid1, HUID2 = @huid2, SIZE = @size, COMMENT = @comment, ITEM_CODE = @prCode WHERE TAG_NO = @tagNo", con))
            {
                // Set parameters for the update query
                updateCommand.Parameters.AddWithValue("@tagNo", row.Cells["tagno"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@type", row.Cells["type"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@caret", row.Cells["caret"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value));
                updateCommand.Parameters.AddWithValue("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value));
                updateCommand.Parameters.AddWithValue("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value));
                updateCommand.Parameters.AddWithValue("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value));
                updateCommand.Parameters.AddWithValue("@huid1", row.Cells["huid1"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@huid2", row.Cells["huid2"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@size", row.Cells["size"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@comment", row.Cells["comment"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@prCode", row.Cells["prcode"].Value?.ToString());

                // Execute the update query
                updateCommand.ExecuteNonQuery();
            }
        }
        private void InsertData(DataGridViewRow row, SQLiteConnection con)
        {
            using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW, LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, COMMENT,IT_TYPE, ITEM_CODE, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, PRICE, STATUS, AC_CODE, AC_NAME) VALUES ( @tagNo, @type, @caret, @gross, @net, @labour, @other, @huid1, @huid2, @size, @comment,@ittype, @prCode, @coYear, @coBook, @vchNo, @vchDate, @price, @status, @acCode, @acName)", con))
            {
                // Call UpdateTagNo for each row
                UpdateTagNo(row.Index);
                if (!ValidateData(row))
                {
                    row.Cells["tagno"].Value = null;
                    // Validation failed, return or handle accordingly
                    return;
                }
                // Set parameters for the insert query
                insertCommand.Parameters.AddWithValue("@tagNo", row.Cells["tagno"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@type", row.Cells["type"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@caret", row.Cells["caret"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value));
                insertCommand.Parameters.AddWithValue("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value));
                insertCommand.Parameters.AddWithValue("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value));
                insertCommand.Parameters.AddWithValue("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value));
                insertCommand.Parameters.AddWithValue("@huid1", row.Cells["huid1"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@huid2", row.Cells["huid2"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@size", row.Cells["size"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@comment", row.Cells["comment"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@prCode", row.Cells["prcode"].Value?.ToString());

                // Add parameters for fixed data
                // (these might need to be adjusted based on your actual requirements)
                int currentYear = DateTime.Now.Year;
                insertCommand.Parameters.AddWithValue("@coYear", $"{currentYear}-{currentYear + 1}");



                insertCommand.Parameters.AddWithValue("@coBook", "015");
                insertCommand.Parameters.AddWithValue("@vchNo", "SYA00");
                insertCommand.Parameters.AddWithValue("@ittype", "G");
                insertCommand.Parameters.AddWithValue("@vchDate", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@price", 0);
                insertCommand.Parameters.AddWithValue("@status", "INSTOCK");
                insertCommand.Parameters.AddWithValue("@acCode", null);
                insertCommand.Parameters.AddWithValue("@acName", null);

                // Execute the insert query
                insertCommand.ExecuteNonQuery();
            }
        }
        // --------------------------------------------------------------------------------------------
        // Printing Tags
        // --------------------------------------------------------------------------------------------
        private void btnAddGoldPrintTag_Click(object sender, EventArgs e)
        {
            PrintData();
        }
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            // Iterate through all rows except the last one and set the value of the "select" column to true
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                dataGridView1.Rows[i].Cells["select"].Value = true;
            }
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
        // --------------------------------------------------------------------------------------------
        // Fetch data from access to sqlite
        // --------------------------------------------------------------------------------------------
        private void btnFetch_Click(object sender, EventArgs e)
        {
            FetchDataFromMSAccessAndInsertIntoSQLite();
        }

        private void FetchDataFromMSAccessAndInsertIntoSQLite()
        {
            try
            {
                // Connection string for MS Access (.mdb)
                string accessConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\pvraj\\OneDrive\\Desktop\\DataCare23Copy.mdb\"";

                // Query to select data from your Access table
                string query = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '015'";

                using (OleDbConnection accessConnection = new OleDbConnection(accessConnectionString))
                {
                    accessConnection.Open();

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, accessConnection))
                    {
                        DataTable accessData = new DataTable();
                        adapter.Fill(accessData);

                        // Insert data into SQLite
                        InsertDataIntoSQLite(accessData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data from Access and inserting into SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDataIntoSQLite(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                List<int> errorRows = new List<int>();
                int insertedCount = 0;
                int updatedCount = 0;
                try
                {
                    using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                    {
                        sqliteConnection.Open();

                        for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
                        {
                            DataRow row = data.Rows[rowIndex];
                            try
                            {
                                // Check if a row with the same TAG_NO already exists in MAIN_DATA
                                using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM MAIN_DATA WHERE TAG_NO = @TAG_NO", sqliteConnection))
                                {
                                    checkCommand.Parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
                                    int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                                    if (rowCount > 0)
                                    {
                                        // Row with the same TAG_NO already exists, perform an update
                                        using (SQLiteCommand updateCommand = new SQLiteCommand("UPDATE MAIN_DATA SET CO_YEAR = @CO_YEAR, CO_BOOK = @CO_BOOK, VCH_NO = @VCH_NO, VCH_DATE = @VCH_DATE, GW = @GW, NW = @NW, LABOUR_AMT = @LABOUR_AMT, OTHER_AMT = @OTHER_AMT,IT_TYPE = @IT_TYPE, ITEM_CODE = @ITEM_CODE, ITEM_PURITY = @ITEM_PURITY, ITEM_DESC = @ITEM_DESC, HUID1 = @HUID1, HUID2 = @HUID2, SIZE = @SIZE, PRICE = @PRICE, STATUS = @STATUS, AC_CODE = @AC_CODE, AC_NAME = @AC_NAME, COMMENT = @COMMENT WHERE TAG_NO = @TAG_NO", sqliteConnection))
                                        {
                                            // Map MS Access column values to SQLite parameters for update
                                            MapParameters(updateCommand.Parameters, row);

                                            // Execute the update query
                                            updateCommand.ExecuteNonQuery();
                                            updatedCount++;
                                        }
                                    }
                                    else
                                    {
                                        // Row with the same TAG_NO doesn't exist, perform an insert
                                        using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, OTHER_AMT,IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT) VALUES (@CO_YEAR, @CO_BOOK, @VCH_NO, @VCH_DATE, @TAG_NO, @GW, @NW, @LABOUR_AMT, @OTHER_AMT, @IT_TYPE, @ITEM_CODE, @ITEM_PURITY, @ITEM_DESC, @HUID1, @HUID2, @SIZE, @PRICE, @STATUS, @AC_CODE, @AC_NAME, @COMMENT)", sqliteConnection))
                                        {
                                            // Map MS Access column values to SQLite parameters for insert
                                            MapParameters(insertCommand.Parameters, row);

                                            // Execute the insert query
                                            insertCommand.ExecuteNonQuery();
                                            insertedCount++;

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // If an error occurs, add the current row index to the list of error rows
                                errorRows.Add(rowIndex + 1); // Adding 1 to make it 1-based index for display
                                MessageBox.Show($"Error in row {rowIndex + 1}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                    if (errorRows.Count > 0)
                    {
                        // If there are error rows, show them in a DataGridView in a separate dialog box
                        ShowErrorRowsDialog(errorRows, data);
                    }
                    else
                    {
                        MessageBox.Show($"Data fetched from Access and inserted/updated in SQLite successfully.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting/updating data into SQLite: {ex.Message}.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No data to insert/update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MapParameters(SQLiteParameterCollection parameters, DataRow row)
        {
            // Map MS Access column values to SQLite parameters
            parameters.AddWithValue("@CO_YEAR", row["CO_YEAR"]);
            parameters.AddWithValue("@CO_BOOK", row["CO_BOOK"]);
            parameters.AddWithValue("@VCH_NO", row["VCH_NO"]);
            parameters.AddWithValue("@VCH_DATE", row["VCH_DATE"]);
            parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
            parameters.AddWithValue("@GW", row["ITM_GWT"]);
            parameters.AddWithValue("@NW", row["ITM_NWT"]);
            parameters.AddWithValue("@LABOUR_AMT", row["LBR_RATE"]);
            parameters.AddWithValue("@OTHER_AMT", row["OTH_AMT"]);
            parameters.AddWithValue("@IT_TYPE", row["IT_TYPE"]);
            parameters.AddWithValue("@ITEM_CODE", row["PR_CODE"]);
            string prCode = row["PR_CODE"].ToString();
            parameters.AddWithValue("@ITEM_PURITY", GetItemPurity(row["IT_CODE"].ToString(), prCode));

            // Extract ITEM_DESC based on mappings
            string itemType = row["IT_TYPE"].ToString(); // Assuming this is a constant value
            string itemDesc = GetItemDescFromSQLite(prCode, itemType);
            parameters.AddWithValue("@ITEM_DESC", itemDesc);

            parameters.AddWithValue("@HUID1", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@HUID2", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@SIZE", row["ITM_SIZE"]);
            parameters.AddWithValue("@PRICE", row["MRP"]);
            parameters.AddWithValue("@STATUS", "INSTOCK"); // Assuming this is a constant value
            parameters.AddWithValue("@AC_CODE", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@AC_NAME", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@COMMENT", row["DESIGN"]);
        }

        private string GetItemPurity(string itCode, string prcode)
        {
            // Assuming itCode has a format like "PR_CODEXXX" where XXX is the item purity
            return itCode.Replace(prcode, "");
        }

        private string GetItemDescFromSQLite(string prCode, string itemType)
        {
            // Implement logic to fetch ITEM_DESC from SQLite based on PR_CODE and IT_TYPE
            // You can modify this method based on your database schema and logic
            // For example, you might need to query the ITEM_MASTER table

            // Sample logic (replace with actual query and logic)
            using (SQLiteConnection con = new SQLiteConnection("Data Source=C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\SYADataBase.db;Version=3;"))
            {
                using (SQLiteCommand command = new SQLiteCommand($"SELECT IT_NAME FROM ITEM_MASTER WHERE PR_CODE = @prCode AND IT_TYPE = @itemType", con))
                {
                    command.Parameters.AddWithValue("@prCode", prCode);
                    command.Parameters.AddWithValue("@itemType", itemType);

                    con.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return result.ToString();
                    }
                    return string.Empty;
                }
            }
        }

        private void ShowErrorRowsDialog(List<int> errorRows, DataTable data)
        {
            // Create a new form to display error rows
            Form errorForm = new Form();
            errorForm.Text = "Error Rows";

            // Set the height of the form based on the number of rows
            int rowHeight = 22; // Adjust this value based on the actual row height
            int formHeight = Math.Min(errorRows.Count * rowHeight + 100, Screen.PrimaryScreen.WorkingArea.Height);

            // Set the width of the form to the full available width
            errorForm.Width = Screen.PrimaryScreen.WorkingArea.Width;

            // Center the form on the screen
            errorForm.StartPosition = FormStartPosition.CenterScreen;

            // Create a DataGridView to display error rows along with original data
            DataGridView errorGridView = new DataGridView();
            errorGridView.Dock = DockStyle.Fill;
            errorGridView.AllowUserToAddRows = false;
            errorGridView.ReadOnly = true;

            // Add columns to display both error information and relevant original data
            errorGridView.Columns.Add("RowNumber", "Row Number");
            errorGridView.Columns.Add("ErrorMessage", "Error Message");

            // Add columns relevant to insertion
            foreach (string columnName in new[] { "TAG_NO", "VCH_DATE", "ITM_GWT", "ITM_NWT", "LBR_RATE", "OTH_AMT", "PR_CODE", "IT_CODE", "IT_DESC", "ITM_SIZE", "MRP", "DESIGN" })
            {
                errorGridView.Columns.Add(columnName, columnName);
            }

            // Populate the DataGridView with error row numbers and error messages
            foreach (int rowNum in errorRows)
            {
                errorGridView.Rows.Add(rowNum, "Error occurred in this row");

                // Add the original data from the row with an error
                DataRow errorRow = data.Rows[rowNum - 1]; // Adjusting to 0-based index

                // Populate relevant data columns
                object[] rowData = new object[]
                {
                    "","",
            errorRow["TAG_NO"],
            errorRow["VCH_DATE"],
            errorRow["ITM_GWT"],
            errorRow["ITM_NWT"],
            errorRow["LBR_RATE"],
            errorRow["OTH_AMT"],
            errorRow["PR_CODE"],
            errorRow["IT_CODE"],
            errorRow["IT_DESC"],
            errorRow["ITM_SIZE"],
            errorRow["MRP"],
            errorRow["DESIGN"],
                };

                errorGridView.Rows.Add(rowData);
            }

            // Add the DataGridView to the form
            errorForm.Controls.Add(errorGridView);

            // Calculate the height of the form title bar and add it to the form height
            int titleBarHeight = errorForm.Height - errorForm.ClientRectangle.Height;
            errorForm.Height = formHeight + titleBarHeight;

            // Show the form
            errorForm.ShowDialog();
        }


        private void panel37_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
