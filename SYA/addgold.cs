using Serilog;
using System;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using QRCoder;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

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
                dataGridView1.Rows[e.RowIndex].Cells["labour"].Value = "650";
                dataGridView1.Rows[e.RowIndex].Cells["wholeLabour"].Value = "0";
                dataGridView1.Rows[e.RowIndex].Cells["other"].Value = "0";
                // Copy values from the previous row's combo boxes
                if (dataGridView1.Rows.Count > 1)
                {
                    DataGridViewRow previousRow = dataGridView1.Rows[dataGridView1.Rows.Count - 2];

                    // Set the combo box values in the current row
                    DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["type"];
                    DataGridViewComboBoxCell caretCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["caret"];

                    typeCell.Value = previousRow.Cells["type"].Value;
                    caretCell.Value = previousRow.Cells["caret"].Value;
                    dataGridView1.Rows[e.RowIndex].Cells["labour"].Value = (previousRow.Cells["labour"].Value ?? "0").ToString();
                    dataGridView1.Rows[e.RowIndex].Cells["wholeLabour"].Value = (previousRow.Cells["wholeLabour"].Value ?? "0").ToString();
                    dataGridView1.Rows[e.RowIndex].Cells["other"].Value = (previousRow.Cells["other"].Value ?? "0").ToString();
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

                .WriteTo.File("C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\LOG\\logs_tagno.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
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


            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(114, 131, 89); // Color for row headers

            //  dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 245, 219);
            dataGridView1.Columns["select"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124);
            dataGridView1.Columns["tagno"].HeaderCell.Style.BackColor = Color.FromArgb(166, 185, 139);
            dataGridView1.Columns["type"].HeaderCell.Style.BackColor = Color.FromArgb(181, 201, 154);
            dataGridView1.Columns["caret"].HeaderCell.Style.BackColor = Color.FromArgb(194, 213, 170);
            dataGridView1.Columns["gross"].HeaderCell.Style.BackColor = Color.FromArgb(207, 225, 185);
            dataGridView1.Columns["net"].HeaderCell.Style.BackColor = Color.FromArgb(220, 235, 202);
            dataGridView1.Columns["labour"].HeaderCell.Style.BackColor = Color.FromArgb(233, 245, 219);
            dataGridView1.Columns["wholeLabour"].HeaderCell.Style.BackColor = Color.FromArgb(220, 235, 202);
            dataGridView1.Columns["other"].HeaderCell.Style.BackColor = Color.FromArgb(207, 225, 185);
            dataGridView1.Columns["huid1"].HeaderCell.Style.BackColor = Color.FromArgb(194, 213, 170);
            dataGridView1.Columns["huid2"].HeaderCell.Style.BackColor = Color.FromArgb(181, 201, 154);
            dataGridView1.Columns["size"].HeaderCell.Style.BackColor = Color.FromArgb(166, 185, 139);
            dataGridView1.Columns["comment"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124);// Color for Column1
            // Customize DataGridView appearance
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Set cell alignment to middle center
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for cells
                column.DefaultCellStyle.Font = new Font("Arial", (float)12.5); // Adjust the font and size as needed

                // Set column width
                if (column.Name == "select") // Adjust the column name
                {
                    column.Width = 40; // Adjust the width as needed
                }
                else if (column.Name == "type") // Adjust the column name
                {
                    column.Width = 225; // Adjust the width as needed
                }
                else if (column.Name == "tagno") // Adjust the column name
                {
                    column.Width = 200; // Adjust the width as needed
                }
                else if (column.Name == "caret") // Adjust the column name
                {
                    column.Width = 75; // Adjust the width as needed
                }
                else if (column.Name == "gross") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                else if (column.Name == "net") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                else if (column.Name == "size") // Adjust the column name
                {
                    column.Width = 80; // Adjust the width as needed
                }
                // Add more conditions for other columns as needed
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for column headers
                column.HeaderCell.Style.Font = new Font("Arial", (float)12.5, FontStyle.Bold); // Adjust the font and size as needed

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

            using (SQLiteDataReader reader = helper.FetchDFromSYADataBase($"SELECT DISTINCT {columnName} FROM ITEM_MASTER WHERE IT_TYPE = '{itemType}'"))
            {
                while (reader.Read())
                {
                    comboBoxColumn.Items.Add(reader[displayMember].ToString());
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
        private bool SaveData()
        {
            try
            {
                // Check if there are rows in the DataGridView
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("DataGridView is empty. Check your data population logic.");
                    return false;
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Check if the row is not empty
                    if (!row.IsNewRow)
                    {


                        // Check if the "tagno" cell is not null or empty
                        if (row.Cells["tagno"].Value != null && !string.IsNullOrEmpty(row.Cells["tagno"].Value.ToString()) && row.Cells["tagno"].Value.ToString() != "0")
                        {
                            // If tagno is generated, update the existing entry in the database
                            UpdateData(row);
                        }
                        else
                        {
                            // If tagno is not generated, insert a new entry in the database
                            InsertData(row);
                        }
                    }
                }

                // If execution reaches here, the save was successful
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // If there's an exception, return false
                return false;
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
            if (!helper.validateType(row.Cells["type"].Value.ToString()))
            {
                MessageBox.Show($"Please add a valid type for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "type");
                return false;
            }

            if (!helper.validateWeight(row.Cells["gross"].Value?.ToString()))
            {
                MessageBox.Show($"Gross weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "gross");
                return false;
            }
            if (!helper.validateWeight(row.Cells["net"].Value?.ToString()))
            {
                MessageBox.Show($"Net weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "net");
                return false;
            }



            if (!helper.validateWeight(row.Cells["gross"].Value?.ToString(), row.Cells["net"].Value?.ToString()))
            {
                MessageBox.Show($"Gross weight should be greater than or equal to net weight for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "gross");
                return false;
            }

            if (!helper.validateLabour(row.Cells["labour"].Value?.ToString()))
            {
                MessageBox.Show($"Labour should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "labour");
                return false;
            }

            if (!helper.validateOther(row.Cells["other"].Value?.ToString()))
            {
                MessageBox.Show($"Other should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(dataGridView1, row.Index, "other");
                return false;
            }

            string huid1 = row.Cells["huid1"].Value?.ToString();
            string huid2 = row.Cells["huid2"].Value?.ToString();

            helper.validateHUID(huid1, huid2);




            return true; // All data is valid
        }

        private void UpdateData(DataGridViewRow row)
        {
            if (!ValidateData(row))
            {
                // Validation failed, return or handle accordingly
                return;
            }

            string updateQuery = "UPDATE MAIN_DATA SET ITEM_DESC = @type, ITEM_PURITY = @caret, GW = @gross, NW = @net, " +
                                 "LABOUR_AMT = @labour, WHOLE_LABOUR_AMT = @wholeLabour, OTHER_AMT = @other, HUID1 = @huid1, HUID2 = @huid2, SIZE = @size, " +
                                 "COMMENT = @comment, ITEM_CODE = @prCode WHERE TAG_NO = @tagNo";

            SQLiteParameter[] parameters = new SQLiteParameter[]
            {
        new SQLiteParameter("@tagNo", row.Cells["tagno"].Value?.ToString()),
        new SQLiteParameter("@type", row.Cells["type"].Value?.ToString()),
        new SQLiteParameter("@caret", row.Cells["caret"].Value?.ToString()),
        new SQLiteParameter("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value)),
        new SQLiteParameter("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value)),
        new SQLiteParameter("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value)),
        new SQLiteParameter("@wholeLabour", Convert.IsDBNull(row.Cells["wholeLabour"].Value) ? 0 : Convert.ToDecimal(row.Cells["wholeLabour"].Value)),
        new SQLiteParameter("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value)),
        new SQLiteParameter("@huid1", row.Cells["huid1"].Value?.ToString()),
        new SQLiteParameter("@huid2", row.Cells["huid2"].Value?.ToString()),
        new SQLiteParameter("@size", row.Cells["size"].Value?.ToString()),
        new SQLiteParameter("@comment", row.Cells["comment"].Value?.ToString()),
        new SQLiteParameter("@prCode", row.Cells["prcode"].Value?.ToString())
            };

            helper.RunQueryWithParametersSYADataBase(updateQuery, parameters);

            // Continue with your logic after the update
        }
        private void InsertData(DataGridViewRow row)
        {
            try
            {
                string InsertQuery = "INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW, LABOUR_AMT,WHOLE_LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, COMMENT,IT_TYPE, ITEM_CODE, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, PRICE, STATUS, AC_CODE, AC_NAME) VALUES ( @tagNo, @type, @caret, @gross, @net, @labour,@wholeLabour, @other, @huid1, @huid2, @size, @comment,@ittype, @prCode, @coYear, @coBook, @vchNo, @vchDate, @price, @status, @acCode, @acName)";
                {
                    // Call UpdateTagNo for each row
                    UpdateTagNo(row.Index);

                    if (!ValidateData(row))
                    {
                        row.Cells["tagno"].Value = null;
                        // Validation failed, return or handle accordingly
                        return;
                    }

                    // Add parameters for fixed data
                    int currentYear = DateTime.Now.Year;

                    // Initialize the array with the fixed parameters
                    SQLiteParameter[] parameters = new SQLiteParameter[]
                    {
                new SQLiteParameter("@tagNo", row.Cells["tagno"].Value?.ToString()),
                new SQLiteParameter("@type", row.Cells["type"].Value?.ToString()),
                new SQLiteParameter("@caret", row.Cells["caret"].Value?.ToString()),
                new SQLiteParameter("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value)),
                new SQLiteParameter("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value)),
                new SQLiteParameter("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value)),
                new SQLiteParameter("@wholeLabour", Convert.IsDBNull(row.Cells["wholeLabour"].Value) ? 0 : Convert.ToDecimal(row.Cells["wholeLabour"].Value)),
                new SQLiteParameter("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value)),
                new SQLiteParameter("@huid1", row.Cells["huid1"].Value?.ToString()),
                new SQLiteParameter("@huid2", row.Cells["huid2"].Value?.ToString()),
                new SQLiteParameter("@size", row.Cells["size"].Value?.ToString()),
                new SQLiteParameter("@comment", row.Cells["comment"].Value?.ToString()),
                new SQLiteParameter("@prCode", row.Cells["prcode"].Value?.ToString()),
                new SQLiteParameter("@ittype", "G"),
                new SQLiteParameter("@coYear", $"{currentYear}-{currentYear + 1}"),
                new SQLiteParameter("@coBook", "015"),
                new SQLiteParameter("@vchNo", "SYA01"),
                new SQLiteParameter("@vchDate", DateTime.Now),
                new SQLiteParameter("@price", 0),
                new SQLiteParameter("@status", "INSTOCK"),
                new SQLiteParameter("@acCode", null),
                new SQLiteParameter("@acName", null)
                    };

                    // Execute the insert query with parameters
                    helper.RunQueryWithParametersSYADataBase(InsertQuery, parameters);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error inserting data: {ex.Message}");
                MessageBox.Show($"Error inserting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --------------------------------------------------------------------------------------------
        // Printing Tags
        // --------------------------------------------------------------------------------------------

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            // Iterate through all rows except the last one and set the value of the "select" column to true
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                dataGridView1.Rows[i].Cells["select"].Value = true;
            }
        }

        // --------------------------------------------------------------------------------------------
        // Fetch data from access to sqlite
        // --------------------------------------------------------------------------------------------
        private int totalRows;
        private int insertedCount;
        private int updatedCount;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable data = e.Argument as DataTable;
            if (data != null)
            {
                InsertDataIntoSQLite(data);
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Update progress bar with the percentage
            progressBar1.Value = e.ProgressPercentage;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show($"Error inserting/updating data into SQLite: {e.Error.Message}.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show($"Data fetched from Access and inserted/updated in SQLite successfully.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnFetch_Click(object sender, EventArgs e)
        {
            FetchDataFromMSAccessAndInsertIntoSQLite();
        }

        private void FetchDataFromMSAccessAndInsertIntoSQLite()
        {
            try
            {
                // Query to select data from your Access table
                string query = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '015' OR CO_BOOK = '15'";
                DataTable accessData = helper.FetchFromDataCareDataBase(query);

                // Set totalRows for progress calculation
                totalRows = accessData.Rows.Count;

                // Start background worker to perform insertion in the background
                backgroundWorker1.RunWorkerAsync(accessData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting into SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDataIntoSQLite(DataTable data)
        {
            List<int> errorRows = new List<int>();
            int insertedCount = 0;
            int updatedCount = 0;

            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                {
                    sqliteConnection.Open();

                    backgroundWorker1.ReportProgress(0); // Initialize progress bar

                    for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
                    {
                        DataRow row = data.Rows[rowIndex];

                        try
                        {
                            using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM MAIN_DATA WHERE TAG_NO = @TAG_NO", sqliteConnection))
                            {
                                checkCommand.Parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
                                int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                                if (rowCount > 0)
                                {
                                    using (SQLiteCommand updateCommand = new SQLiteCommand("UPDATE MAIN_DATA SET CO_YEAR = @CO_YEAR, CO_BOOK = @CO_BOOK, VCH_NO = @VCH_NO, VCH_DATE = @VCH_DATE, GW = @GW, NW = @NW, LABOUR_AMT = @LABOUR_AMT, OTHER_AMT = @OTHER_AMT,IT_TYPE = @IT_TYPE, ITEM_CODE = @ITEM_CODE, ITEM_PURITY = @ITEM_PURITY, ITEM_DESC = @ITEM_DESC, SIZE = @SIZE, PRICE = @PRICE, STATUS = @STATUS, AC_CODE = @AC_CODE, AC_NAME = @AC_NAME, COMMENT = @COMMENT WHERE TAG_NO = @TAG_NO", sqliteConnection))
                                    {
                                        MapParameters(updateCommand.Parameters, row);
                                        updateCommand.ExecuteNonQuery();
                                        updatedCount++;
                                    }
                                }
                                else
                                {
                                    using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, OTHER_AMT,IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT) VALUES (@CO_YEAR, @CO_BOOK, @VCH_NO, @VCH_DATE, @TAG_NO, @GW, @NW, @LABOUR_AMT, @OTHER_AMT, @IT_TYPE, @ITEM_CODE, @ITEM_PURITY, @ITEM_DESC, @SIZE, @PRICE, @STATUS, @AC_CODE, @AC_NAME, @COMMENT)", sqliteConnection))
                                    {
                                        MapParameters(insertCommand.Parameters, row);
                                        insertCommand.ExecuteNonQuery();
                                        insertedCount++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorRows.Add(rowIndex + 1);
                            MessageBox.Show($"Error in row {rowIndex + 1}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // Update progress bar based on the percentage completed
                        int percentage = (int)(((double)rowIndex / data.Rows.Count) * 100);
                        backgroundWorker1.ReportProgress(percentage);
                    }
                }

                if (errorRows.Count > 0)
                {
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


        private void MapParameters(SQLiteParameterCollection parameters, DataRow row)
        {
            // Map MS Access column values to SQLite parameters
            parameters.AddWithValue("@CO_YEAR", row["CO_YEAR"]);
            parameters.AddWithValue("@CO_BOOK", row["CO_BOOK"]);
            parameters.AddWithValue("@VCH_NO", "SYA00");
            parameters.AddWithValue("@VCH_DATE", row["VCH_DATE"]);
            parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
            parameters.AddWithValue("@GW", row["ITM_GWT"]);
            parameters.AddWithValue("@NW", row["ITM_NWT"]);
            // REMOVE BELOW 2 PRATYUSH
            parameters.AddWithValue("@HUID1", row["KR_NAME"]);
            parameters.AddWithValue("@HUID2", row["RATE_TYPE"]);
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
            // UNCOMMENT BELOW
            // PRATYUSH  parameters.AddWithValue("@HUID1", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            // PRATYUSH  parameters.AddWithValue("@HUID2", DBNull.Value); // Set to DBNull since it's NULL in MS Access
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

            string query = "SELECT IT_NAME FROM ITEM_MASTER WHERE PR_CODE = " + prCode + " AND IT_TYPE = " + itemType;



            object result = helper.RunQueryWithoutParametersSYADataBase(query, "ExecuteScalar");
            if (result != null)
            {
                return result.ToString();
            }
            return string.Empty;

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
        private void btnAddGoldPrintTag_Click(object sender, EventArgs e)
        {
            PrintLabels();
        }
        private void PrintLabels()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "TSC_TE244";


                pd.PrintPage += new PrintPageEventHandler(PrintPageGold);


                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PrintPageGold(object sender, PrintPageEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;
            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                if (tagNumber.Length > 1)
                {

                    Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Set the starting position for printing
                    float xPos = 0; // Adjust the starting X position
                    float yPos = 0; // Adjust the starting Y position

                    // Get the printer DPI
                    float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;

                    float rectX = 4; // Adjust the X position of the rectangle
                    float rectY = 4; // Adjust the Y position of the rectangle
                    float rectWidth = 211; // Adjust the width of the rectangle
                    float rectHeight = 45; // Adjust the height of the rectangle
                                           //gross weight
                                           //net weight
                    if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                    {
                        e.Graphics.DrawString((selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }

                    //logo
                    Image logoImage = Image.FromFile("C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\SYA\\Image\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 

                    if (selectedRow.Cells["type"].Value.ToString() == "BANGAL")
                    {
                        e.Graphics.DrawString((selectedRow.Cells[10].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {

                        e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //caret
                    e.Graphics.DrawString((selectedRow.Cells["caret"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw the QR code rectangle
                    RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);

                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        // Draw the QR code onto the printing surface
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }

                    //labour                
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)16, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //Tag number
                    string firstPart = null;
                    string secondPart = null;
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 5;
                        firstPart = tagNumber.Substring(lastIndex);
                        secondPart = tagNumber.Substring(0, lastIndex);
                        e.Graphics.DrawString(secondPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Graphics.DrawString(firstPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString(tagNumber, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)30, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //huid
                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        e.Graphics.DrawString("HUID", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    Log.Information(" TagNo : " + tagNumber);
                    //outside box
                    //e.Graphics.DrawRectangle(Pens.Red, 4, 4, 211, 45);
                    //first part
                    //e.Graphics.DrawRectangle(Pens.Red, 4, 4, (float)105.5, 45);
                    //Second Part
                    //e.Graphics.DrawRectangle(Pens.Red, (float)109.5, 4, (float)105.5, 45);
                    //gross weight
                    //e.Graphics.DrawRectangle(Pens.Red, 4, 4, 75, (float)22.5);
                    //net weight
                    //e.Graphics.DrawRectangle(Pens.Red, 4, (float)26.5, 75, (float)22.5);
                    //logo
                    //e.Graphics.DrawRectangle(Pens.Red, 83, 4, (float)22.5, (float)22.5);
                    //logo name 
                    //e.Graphics.DrawRectangle(Pens.Red, 79, (float)26.5, (float)30.5, (float)11.25);
                    //caret
                    //e.Graphics.DrawRectangle(Pens.Red, 79, (float)37.75, (float)30.5, (float)11.25);
                    // Draw the QR code rectangle
                    //RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                    //e.Graphics.DrawRectangle(Pens.Red, qrCodeRect.X, qrCodeRect.Y, qrCodeRect.Width, qrCodeRect.Height);
                    //labour                
                    //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)4, (float)56.5, (float)11);
                    //other
                    //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)15, (float)56.5, (float)11);
                    //Tag number
                    //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)26, (float)56.5, (float)11);
                    //huid
                    //e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)40, (float)37, (float)9);
                }
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            // if (e.KeyCode == Keys.Tab)
            if (false)
            {

                string currentColumnName = dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name;
                int currentRowIndex = dataGridView1.CurrentCell.RowIndex;
                // MessageBox.Show("pratyush1 : " + currentColumnName);
                // Assuming "ColumnName" is the name of the last column
                if (currentColumnName == "comment")
                {
                    // MessageBox.Show("in comment");
                    // You are moving to the next row in the last column
                    // Call your save and/or print function here
                    SaveData();
                    PrintLabels();

                }

            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.PreviewKeyDown -= dataGridView_EditingControl_PreviewKeyDown;
            e.Control.PreviewKeyDown += dataGridView_EditingControl_PreviewKeyDown;

        }
        private void dataGridView_EditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (checkBoxAddGold1.Checked == true)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    if (editingControl != null)
                    {
                        DataGridView dataGridView = dataGridView1;
                        string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                        int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                        //  MessageBox.Show("pratyush1: " + currentColumnName);

                        // Assuming "comment" is the name of the last column
                        if (currentColumnName == "comment")
                        {
                            // MessageBox.Show("in comment");
                            // You are moving to the next row in the last column
                            // Call your save and/or print function here
                            if (SaveData())
                            {
                                DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                                if (tagNumber.Length > 1)
                                {
                                    PrintLabels();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void panel31_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel30_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel29_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel28_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel32_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel33_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel21_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel23_Paint(object sender, PaintEventArgs e)
        {
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell is a header cell
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                //  MessageBox.Show("pratyush");
                // Set the background color for the top-left cell
                e.CellStyle.BackColor = Color.FromArgb(233, 245, 219);
                // You can customize other properties as needed
                e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Check if it is the top-left cell (1, 1)
            if (e.RowIndex == -1 && e.ColumnIndex == -1)
            {
                // Set the background color for the top-left cell
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(114, 131, 89)), e.CellBounds);



                // Prevent default painting
                e.Handled = true;
            }
        }


    }
}
