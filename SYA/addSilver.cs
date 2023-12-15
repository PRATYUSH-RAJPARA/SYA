using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public partial class addSilver : Form
    {
        public addSilver()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            // Manually add columns to the DataGridView
            addSilverDataGridView.AutoGenerateColumns = false;
            gridviewstyle();
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderText = "PR_CODE";
            textBoxColumn.Name = "prcode";
            addSilverDataGridView.Columns.Add(textBoxColumn);
            addSilverDataGridView.Columns["prcode"].Visible = false;
            InitializeComboBoxColumns();

            // Set the DataGridView DataSource to an empty DataTable
            addSilverDataGridView.DataSource = GetEmptyDataTable();


            // Enable row headers for DataGridView to display row numbers
            addSilverDataGridView.RowHeadersVisible = true;



            // Add an event handler to update row numbers when rows are added or removed
            addSilverDataGridView.RowsAdded += (s, args) => UpdateRowNumbers();
            addSilverDataGridView.RowsRemoved += (s, args) => UpdateRowNumbers();
        }

        private void addSilver_Load(object sender, EventArgs e)
        {
            InitializeLogging();
            // Update row numbers initially
            UpdateRowNumbers();
        }

        private int pratyushcount = 0;
        private SQLiteConnection connectionToSYADatabase;
        private SQLiteConnection connectionToDatacare;
        private const int ItemNameColumnIndex = 2;
        private string previousTypeValue = string.Empty;
        private string previousCaretValue = string.Empty;
        private int itemCount = 0;
        private decimal grossWeightSum = 0;


        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection(helper.SYAConnectionString);
            connectionToDatacare = new SQLiteConnection(helper.accessConnectionString);

        }
        private void InitializeComboBoxColumns()
        {
            // Load TYPE values
            LoadComboBoxValues("S", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)addSilverDataGridView.Columns["type"]);

            // Load CARET values
            LoadComboBoxValues("SQ", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)addSilverDataGridView.Columns["caret"]);
        }
        // --------------------------------------------------------------------------------------------
        // Events
        // --------------------------------------------------------------------------------------------
        private void addSilverDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (addSilverDataGridView.CurrentCell is DataGridViewTextBoxCell)
            {
                addSilverDataGridView.BeginEdit(true);

                // Select all text in the cell
                if (addSilverDataGridView.EditingControl is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }
            // Check if entering the first column of a new row
            if (e.ColumnIndex == 0 && e.RowIndex == addSilverDataGridView.Rows.Count - 1)
            {
                UpdateItemCountAndGrossWeight();
                // Copy values from the previous row's combo boxes
                if (addSilverDataGridView.Rows.Count > 1)
                {
                    DataGridViewRow previousRow = addSilverDataGridView.Rows[addSilverDataGridView.Rows.Count - 2];

                    // Set the combo box values in the current row
                    DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)addSilverDataGridView.Rows[e.RowIndex].Cells["type"];
                    DataGridViewComboBoxCell caretCell = (DataGridViewComboBoxCell)addSilverDataGridView.Rows[e.RowIndex].Cells["caret"];

                    typeCell.Value = previousRow.Cells["type"].Value;
                    caretCell.Value = previousRow.Cells["caret"].Value;

                    // Save the values for future reference
                    previousTypeValue = previousRow.Cells["type"].Value?.ToString();
                    previousCaretValue = previousRow.Cells["caret"].Value?.ToString();
                }
            }
        }
        private void addSilverDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Check if the edited cell is in the "gross" column
                if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "gross")
                {
                    // Copy the value from the "gross" column to the corresponding "net" column
                    addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value = addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value;

                    // Update the item count and gross weight sum
                    UpdateItemCountAndGrossWeight();
                }
                if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "caret")
                {
                    caretValueChanged(e);
                }
                else if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "gross")
                {
                    grossValueChanged(e);
                }
                else if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "net")
                {
                    netValueChanged(e);
                }
                else if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "labour")
                {
                    pglValueChanged(e);
                }
                else if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "wholeLabour")
                {
                    wlValueChanged(e);
                }
                else if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "other")
                {
                    otherValueChanged(e);
                }

            }
        }
        private void caretValueChanged(DataGridViewCellEventArgs e)
        {
            grossValueChanged(e);
        }
        private void grossValueChanged(DataGridViewCellEventArgs e)
        {
            string caret = addSilverDataGridView.Rows[e.RowIndex].Cells["caret"].Value?.ToString();
            decimal? gross = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value?.ToString());
            decimal? net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value?.ToString());
            decimal? pgl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value?.ToString());
            decimal? wl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value?.ToString());
            decimal? other = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value?.ToString());


            if (net == null || net == 0)
            {
                net = gross;
            }
            if (gross != null || gross != 0)
            {
                addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value = net;
            }
        }
        private void netValueChanged(DataGridViewCellEventArgs e)
        {
            string caret = addSilverDataGridView.Rows[e.RowIndex].Cells["caret"].Value?.ToString();
            decimal? gross = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value?.ToString());
            decimal? net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value?.ToString());
            decimal? pgl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value?.ToString());
            decimal? wl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value?.ToString());
            decimal? other = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value?.ToString());

            if (caret == "SLO")
            {
                if (net < 10)
                {
                    if (pgl == null || pgl == 0)
                    {
                        pgl = 0;
                        if (wl == null || wl == 0)
                        {
                            wl = 200;

                        }
                    }
                    else if (pgl != null)
                    {
                        if (pgl * net < 200)
                        {
                            pgl = 0;
                            if (wl == null || wl == 0)
                            {
                                wl = 200;

                            }
                        }
                    }
                }
                else if (net > 10)
                {
                    if (pgl == null || pgl == 0)
                    {
                        pgl = 20;
                        wl = 0;
                    }
                    else if (pgl != null)
                    {
                        if (pgl * net < 200)
                        {
                            pgl = 20;
                            wl = 0;
                        }
                    }
                }
            }
            else if (caret == "925")
            {

                pgl = 300;

                if (wl == null)
                {
                    wl = 0;
                }
            }
            addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = pgl;
            addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = wl;
        }
        private void pglValueChanged(DataGridViewCellEventArgs e)
        {

            calculatePrice(e);
        }
        private void wlValueChanged(DataGridViewCellEventArgs e) { calculatePrice(e); }
        private void otherValueChanged(DataGridViewCellEventArgs e) { calculatePrice(e); }
        private void calculatePrice(DataGridViewCellEventArgs e)
        {

            string caret = addSilverDataGridView.Rows[e.RowIndex].Cells["caret"].Value?.ToString();

            decimal gross = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value?.ToString() ?? "0");
            decimal net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value?.ToString() ?? "0");
            decimal pgl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value?.ToString() ?? "0");
            decimal wl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value?.ToString() ?? "0");
            decimal other = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value?.ToString() ?? "0");

            decimal price = 0;
            if (caret == "SLO")
            {
                // pratyush check if current price is null
                if (gross - net < 5)
                {
                    price = (gross * (Convert.ToDecimal(txtCurrentPrice.Text) + pgl) + wl + other);

                }
                else { price = (net * (Convert.ToDecimal(txtCurrentPrice.Text) + pgl) + wl + other); }
            }
            else if (caret == "925")
            {
                if (gross - net < 5)
                {
                    price = ((gross * pgl) + wl + other);

                }
                else { price = ((net * pgl) + wl + other); }
            }
            // Round to the nearest upper bound based on your custom ranges (e.g., 50)
            decimal step = 50;
            decimal roundedPrice = CustomRound(price, step);
            addSilverDataGridView.Rows[e.RowIndex].Cells["price"].Value = roundedPrice;
        }

        private void updatelabourandprice(DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == 0)
                {
                    // Calculate values for the first row
                    var caretValue = addSilverDataGridView.Rows[e.RowIndex].Cells["caret"].Value?.ToString();
                    if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "gross" || addSilverDataGridView.Columns[e.ColumnIndex].Name == "net")

                    {
                        if ("SLO".Equals(caretValue, StringComparison.OrdinalIgnoreCase))
                        {
                            decimal gross = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value);
                            decimal net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value);

                            if ((gross - net) < 5)
                            {
                                if (gross < 10)
                                {
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = 200;
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = 0;
                                }
                                else
                                {
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = 0;
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = 20;
                                }
                            }
                            else
                            {
                                if (net < 10)
                                {
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = 200;
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = 0;
                                }
                                else
                                {
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = 0;
                                    addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = 20;
                                }
                            }
                        }
                        else
                        {
                            addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = 300;
                            addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = 0;
                            CalculatePrice(e.RowIndex);
                        }
                    }

                }
                else
                {
                    // For subsequent rows
                    var currentRow = addSilverDataGridView.Rows[e.RowIndex];
                    var previousRow = addSilverDataGridView.Rows[e.RowIndex - 1];

                    // Check if type and caret values are the same as the previous row
                    if (currentRow.Cells["type"].Value == previousRow.Cells["type"].Value &&
                        currentRow.Cells["caret"].Value == previousRow.Cells["caret"].Value)
                    {
                        // Copy values from the previous row
                        currentRow.Cells["wholeLabour"].Value = previousRow.Cells["wholeLabour"].Value;
                        currentRow.Cells["labour"].Value = previousRow.Cells["labour"].Value;
                    }
                    else
                    {
                        // Calculate values for the current row if type and caret are different
                        var caretValue = currentRow.Cells["caret"].Value?.ToString();

                        if ("SLO".Equals(caretValue, StringComparison.OrdinalIgnoreCase))
                        {
                            decimal gross = Convert.ToDecimal(currentRow.Cells["gross"].Value);
                            decimal net = Convert.ToDecimal(currentRow.Cells["net"].Value);

                            if ((gross - net) < 5)
                            {
                                if (gross < 10)
                                {
                                    currentRow.Cells["wholeLabour"].Value = 200;
                                    currentRow.Cells["labour"].Value = 0;
                                }
                                else
                                {
                                    currentRow.Cells["wholeLabour"].Value = 0;
                                    currentRow.Cells["labour"].Value = 20;
                                }
                            }
                            else
                            {
                                if (net < 10)
                                {
                                    currentRow.Cells["wholeLabour"].Value = 200;
                                    currentRow.Cells["labour"].Value = 0;
                                }
                                else
                                {
                                    currentRow.Cells["wholeLabour"].Value = 0;
                                    currentRow.Cells["labour"].Value = 20;
                                }
                            }
                        }
                        else
                        {
                            currentRow.Cells["wholeLabour"].Value = 300;
                            currentRow.Cells["labour"].Value = 0;
                        }
                    }

                    CalculatePrice(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void CalculatePrice(int rowIndex)
        {
            try
            {
                decimal wholeLabour = Convert.ToDecimal(addSilverDataGridView.Rows[rowIndex].Cells["wholeLabour"].Value);
                decimal gross = Convert.ToDecimal(addSilverDataGridView.Rows[rowIndex].Cells["gross"].Value);
                decimal net = Convert.ToDecimal(addSilverDataGridView.Rows[rowIndex].Cells["net"].Value);
                decimal other = Convert.ToDecimal(addSilverDataGridView.Rows[rowIndex].Cells["other"].Value ?? 0);

                if ((gross - net) < 5)
                {
                    // Calculate the price
                    decimal rawPrice = (gross * wholeLabour) + other;

                    // Round to the nearest upper bound based on your custom ranges (e.g., 50)
                    decimal step = 50;
                    decimal roundedPrice = CustomRound(rawPrice, step);

                    // Set the rounded price to the DataGridView
                    addSilverDataGridView.Rows[rowIndex].Cells["price"].Value = roundedPrice;
                }
                else
                {
                    // Calculate the price
                    decimal rawPrice = (net * wholeLabour) + other;

                    // Round to the nearest upper bound based on your custom ranges (e.g., 50)
                    decimal step = 50;
                    decimal roundedPrice = CustomRound(rawPrice, step);

                    // Set the rounded price to the DataGridView
                    addSilverDataGridView.Rows[rowIndex].Cells["price"].Value = roundedPrice;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while calculating price: {ex.Message}");
            }
        }

        private decimal CustomRound(decimal value, decimal step)
        {
            decimal remainder = value % step;
            if (remainder == 0)
            {
                // No rounding needed
                return value;
            }
            else
            {
                // Round up to the nearest multiple of step
                return value + (step - remainder);
            }
        }




        private void addSilverDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //updatelabourandprice(e);
            }
            // Update item count and gross weight sum when cell values change
            // UpdateItemCountAndGrossWeight();
        }
        // --------------------------------------------------------------------------------------------
        // Logging
        // --------------------------------------------------------------------------------------------
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()

                .WriteTo.File(helper.LogsFolder + "\\logs.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
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
            foreach (DataGridViewColumn column in addSilverDataGridView.Columns)
            {
                // Set cell alignment to middle center
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for cells
                column.DefaultCellStyle.Font = new Font("Arial", 10); // Adjust the font and size as needed


                // Add more conditions for other columns as needed
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for column headers
                column.HeaderCell.Style.Font = new Font("Arial", 10, FontStyle.Bold); // Adjust the font and size as needed

            }
        }
        private void UpdateItemCountAndGrossWeight()
        {
            // Reset counts
            itemCount = 0;
            grossWeightSum = 0;

            foreach (DataGridViewRow row in addSilverDataGridView.Rows)
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
            foreach (DataGridViewRow row in addSilverDataGridView.Rows)
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
            dataTable.Columns.Add("wholeLabour", typeof(decimal));
            dataTable.Columns.Add("other", typeof(decimal));
            dataTable.Columns.Add("size", typeof(string));
            dataTable.Columns.Add("comment", typeof(string));
            dataTable.Columns.Add("prcode", typeof(string));

            return dataTable;
        }
        // --------------------------------------------------------------------------------------------
        // Save Data
        // --------------------------------------------------------------------------------------------
        private void btnAddSilverSave_Click(object sender, EventArgs e)
        {
            UpdateItemCountAndGrossWeight();
            SaveData();
        }
        private void SaveData()
        {
            // Check if there are rows in the DataGridView
            if (addSilverDataGridView.Rows.Count == 0)
            {
                MessageBox.Show("DataGridView is empty. Check your data population logic.");
                return;
            }

            using (SQLiteConnection con = new SQLiteConnection(connectionToSYADatabase.ConnectionString))
            {
                con.Open();

                foreach (DataGridViewRow row in addSilverDataGridView.Rows)
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
            if (rowIndex >= 0 && rowIndex < addSilverDataGridView.Rows.Count)
            {
                if (addSilverDataGridView.Columns.Count > ItemNameColumnIndex)
                {
                    object itemNameObject = addSilverDataGridView.Rows[rowIndex].Cells[ItemNameColumnIndex].Value;

                    if (itemNameObject != null)
                    {
                        string itemName = itemNameObject.ToString();
                        string caret = addSilverDataGridView.Rows[rowIndex].Cells["caret"].Value?.ToString();
                        string prCode = GetPRCode(itemName);

                        string prefix = "SYA";
                        int newSequenceNumber = GetNextSequenceNumber(prefix, prCode, caret);

                        if (!string.IsNullOrEmpty(caret) && !string.IsNullOrEmpty(prCode))
                        {
                            string newTagNo = $"{prefix}{caret}{prCode}{newSequenceNumber:D5}";
                            addSilverDataGridView.Rows[rowIndex].Cells["tagno"].Value = newTagNo;
                            addSilverDataGridView.Rows[rowIndex].Cells["prcode"].Value = prCode; // Set PR_CODE in the DataGridView
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
            int prefixLength = (prefix ?? string.Empty).Length + (prCode ?? string.Empty).Length + (caret ?? string.Empty).Length;

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
                using (SQLiteCommand command = new SQLiteCommand($"SELECT PR_CODE FROM ITEM_MASTER WHERE IT_NAME = '{itemName}' AND IT_TYPE = 'S'", con))
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
                SelectCell(addSilverDataGridView, row.Index, "type");
                return false;
            }

            if (!decimal.TryParse(row.Cells["gross"].Value?.ToString(), out decimal grossWeight) || grossWeight < 0)
            {
                MessageBox.Show($"Gross weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "gross");
                return false;
            }

            if (!decimal.TryParse(row.Cells["net"].Value?.ToString(), out decimal netWeight) || netWeight < 0)
            {
                MessageBox.Show($"Net weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "net");
                return false;
            }

            if (grossWeight < netWeight)
            {
                MessageBox.Show($"Gross weight should be greater than or equal to net weight for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "gross");
                return false;
            }

            if (!decimal.TryParse(row.Cells["labour"].Value?.ToString(), out decimal labour) || labour < 0)
            {
                MessageBox.Show($"Labour should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "labour");
                return false;
            }
            if (!decimal.TryParse(row.Cells["wholeLabour"].Value?.ToString(), out decimal wholeLabour) || wholeLabour < 0)
            {
                MessageBox.Show($"Whole Labour should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "wholeLabour");
                return false;
            }
            if (row.Cells["price"].Value != null && (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out decimal price) || price < 0))
            {
                MessageBox.Show($"Price should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "price");
                return false;
            }

            if (row.Cells["other"].Value != null && (!decimal.TryParse(row.Cells["other"].Value?.ToString(), out decimal other) || other < 0))
            {
                MessageBox.Show($"Other should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "other");
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
            using (SQLiteCommand updateCommand = new SQLiteCommand("UPDATE MAIN_DATA SET ITEM_DESC = @type, ITEM_PURITY = @caret, GW = @gross, NW = @net, LABOUR_AMT = @labour," +
                "WHOLE_LABOUR_AMT = @wholelable, OTHER_AMT = @other, HUID1 = @huid1, HUID2 = @huid2,PRICE= @price, SIZE = @size, COMMENT = @comment, ITEM_CODE = @prCode WHERE TAG_NO = @tagNo", con))
            {
                // Set parameters for the update query
                updateCommand.Parameters.AddWithValue("@tagNo", row.Cells["tagno"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@type", row.Cells["type"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@caret", row.Cells["caret"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value));
                updateCommand.Parameters.AddWithValue("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value));
                updateCommand.Parameters.AddWithValue("@wholelable", Convert.IsDBNull(row.Cells["wholeLabour"].Value) ? 0 : Convert.ToDecimal(row.Cells["wholeLabour"].Value));
                updateCommand.Parameters.AddWithValue("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value));
                updateCommand.Parameters.AddWithValue("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value));
                updateCommand.Parameters.AddWithValue("@huid1", null);
                updateCommand.Parameters.AddWithValue("@huid2", null);
                updateCommand.Parameters.AddWithValue("@price", row.Cells["price"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@size", row.Cells["size"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@comment", row.Cells["comment"].Value?.ToString());
                updateCommand.Parameters.AddWithValue("@prCode", row.Cells["prcode"].Value?.ToString());

                // Execute the update query
                updateCommand.ExecuteNonQuery();
            }
        }
        private void InsertData(DataGridViewRow row, SQLiteConnection con)
        {
            using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW,WHOLE_LABOUR_AMT, LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, COMMENT,IT_TYPE, ITEM_CODE, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, PRICE, STATUS, AC_CODE, AC_NAME) VALUES ( @tagNo, @type, @caret, @gross, @net,@wholelabouramt, @labour, @other, @huid1, @huid2, @size, @comment,@ittype, @prCode, @coYear, @coBook, @vchNo, @vchDate, @price, @status, @acCode, @acName)", con))
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
                insertCommand.Parameters.AddWithValue("@wholelabouramt", Convert.IsDBNull(row.Cells["wholeLabour"].Value) ? 0 : Convert.ToDecimal(row.Cells["wholeLabour"].Value));
                insertCommand.Parameters.AddWithValue("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value));
                insertCommand.Parameters.AddWithValue("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value));
                insertCommand.Parameters.AddWithValue("@huid1", null);
                insertCommand.Parameters.AddWithValue("@huid2", null);
                insertCommand.Parameters.AddWithValue("@size", row.Cells["size"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@comment", row.Cells["comment"].Value?.ToString());
                insertCommand.Parameters.AddWithValue("@prCode", row.Cells["prcode"].Value?.ToString());

                // Add parameters for fixed data
                // (these might need to be adjusted based on your actual requirements)
                insertCommand.Parameters.AddWithValue("@coYear", DateTime.Now.ToString("yyyy") + "-" + (DateTime.Now.Year + 1).ToString("yyyy"));
                insertCommand.Parameters.AddWithValue("@coBook", "015");
                insertCommand.Parameters.AddWithValue("@vchNo", "SYA00");
                insertCommand.Parameters.AddWithValue("@ittype", "S");
                insertCommand.Parameters.AddWithValue("@vchDate", DateTime.Now);
                insertCommand.Parameters.AddWithValue("@price", row.Cells["price"].Value?.ToString());
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
        private void btnAddSilverPrintTag_Click(object sender, EventArgs e)
        {
            PrintData();
        }
        private void btnAddSiilverSelectAll_Click(object sender, EventArgs e)
        {
            // Iterate through all rows except the last one and set the value of the "select" column to true
            for (int i = 0; i < addSilverDataGridView.Rows.Count - 1; i++)
            {
                addSilverDataGridView.Rows[i].Cells["select"].Value = true;
            }
        }
        private void PrintData()
        {
            foreach (DataGridViewRow row in addSilverDataGridView.Rows)
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





    }
}
