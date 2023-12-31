﻿using QRCoder;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
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
        bool quickSave = false;
        bool quickSaveAndPrint = false;
        string tagtype = "weight";

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
                addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = "650";
                addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = "0";
                addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value = "0";
                // Copy values from the previous row's combo boxes
                if (addSilverDataGridView.Rows.Count > 1)
                {
                    DataGridViewRow previousRow = addSilverDataGridView.Rows[addSilverDataGridView.Rows.Count - 2];

                    // Set the combo box values in the current row
                    DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)addSilverDataGridView.Rows[e.RowIndex].Cells["type"];
                    DataGridViewComboBoxCell caretCell = (DataGridViewComboBoxCell)addSilverDataGridView.Rows[e.RowIndex].Cells["caret"];

                    typeCell.Value = previousRow.Cells["type"].Value;
                    caretCell.Value = previousRow.Cells["caret"].Value;
                    addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = (previousRow.Cells["labour"].Value ?? "0").ToString();
                    addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = (previousRow.Cells["wholeLabour"].Value ?? "0").ToString();
                    addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value = (previousRow.Cells["other"].Value ?? "0").ToString();
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
                    addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);


                    // Update the item count and gross weight sum
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
                    addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

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
                if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "huid1")
                {
                    DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
                    addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }
                else if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "huid2")
                {
                    DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
                    addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }

            }
        }
        private void caretValueChanged(DataGridViewCellEventArgs e)
        {
            grossValueChanged(e);
        }
        private void grossValueChanged(DataGridViewCellEventArgs e)
        {
            try
            {
                string caret = addSilverDataGridView.Rows[e.RowIndex].Cells["caret"].Value?.ToString();
                //pratyush check
                decimal? gross = Convert.ToDecimal((addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value ?? "0")?.ToString() ?? "0");
                decimal? net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value?.ToString() ?? "0");
                decimal? pgl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value?.ToString() ?? "0");
                decimal? wl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value?.ToString() ?? "0");
                decimal? other = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value?.ToString() ?? "0");
                if (net == null || net == 0)
                {
                    net = gross;
                }
                if (gross != null || gross != 0)
                {
                    addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value = net;
                }
            }
            catch
            {
                MessageBox.Show("error is gross please check again");
            }


        }
        private void netValueChanged(DataGridViewCellEventArgs e)
        {
            try
            {
                string caret = addSilverDataGridView.Rows[e.RowIndex].Cells["caret"].Value?.ToString();
                decimal? gross = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value?.ToString() ?? "0");
                decimal? net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value?.ToString() ?? "0");
                decimal? pgl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value?.ToString() ?? "0");
                decimal? wl = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value?.ToString() ?? "0");
                decimal? other = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value?.ToString() ?? "0");


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
            catch { MessageBox.Show("Please check net weight"); }
        }
        private void pglValueChanged(DataGridViewCellEventArgs e)
        {

            calculatePrice(e);
        }
        private void wlValueChanged(DataGridViewCellEventArgs e) { calculatePrice(e); }
        private void otherValueChanged(DataGridViewCellEventArgs e) { calculatePrice(e); }
        private void calculatePrice(DataGridViewCellEventArgs e)
        {
            try
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
                decimal roundedFinalPrice = Math.Truncate(roundedPrice);
                addSilverDataGridView.Rows[e.RowIndex].Cells["price"].Value = roundedFinalPrice;
            }
            catch
            {
                MessageBox.Show("error in caret");
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
            //   if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            //   {
            //updatelabourandprice(e);
            //   }
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
            addSilverDataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(114, 131, 89); // Color for row headers
            addSilverDataGridView.Columns["select"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124);
            addSilverDataGridView.Columns["tagno"].HeaderCell.Style.BackColor = Color.FromArgb(166, 185, 139);
            addSilverDataGridView.Columns["type"].HeaderCell.Style.BackColor = Color.FromArgb(181, 201, 154);
            addSilverDataGridView.Columns["caret"].HeaderCell.Style.BackColor = Color.FromArgb(194, 213, 170);
            addSilverDataGridView.Columns["gross"].HeaderCell.Style.BackColor = Color.FromArgb(207, 225, 185);
            addSilverDataGridView.Columns["net"].HeaderCell.Style.BackColor = Color.FromArgb(220, 235, 202);
            addSilverDataGridView.Columns["labour"].HeaderCell.Style.BackColor = Color.FromArgb(233, 245, 219);
            addSilverDataGridView.Columns["wholeLabour"].HeaderCell.Style.BackColor = Color.FromArgb(220, 235, 202);
            addSilverDataGridView.Columns["other"].HeaderCell.Style.BackColor = Color.FromArgb(207, 225, 185);
            addSilverDataGridView.Columns["price"].HeaderCell.Style.BackColor = Color.FromArgb(181, 201, 154);
            addSilverDataGridView.Columns["size"].HeaderCell.Style.BackColor = Color.FromArgb(166, 185, 139);
            addSilverDataGridView.Columns["comment"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124);// Color for Column1
            // Customize DataGridView appearance
            foreach (DataGridViewColumn column in addSilverDataGridView.Columns)
            {
                // Set cell alignment to middle center
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for cells
                column.DefaultCellStyle.Font = new Font("Arial", (float)12.5); // Adjust the font and size as needed


                // Add more conditions for other columns as needed
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for column headers
                column.HeaderCell.Style.Font = new Font("Arial", (float)12.5, FontStyle.Bold); // Adjust the font and size as needed

            }

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
            DataGridViewRow empty = new DataGridViewRow();

            SaveData(empty, 0);
        }
        private bool SaveData(DataGridViewRow selectedRow, int check)
        {
            if (check == 0)
            {
                try
                {
                    // Check if there are rows in the DataGridView
                    if (addSilverDataGridView.Rows.Count == 0)
                    {
                        MessageBox.Show("DataGridView is empty. Check your data population logic.");
                        return false;
                    }



                    foreach (DataGridViewRow row in addSilverDataGridView.Rows)
                    {
                        // Check if the row is not empty
                        if (!row.IsNewRow)
                        {


                            // Check if the "tagno" cell is not null or empty
                            if (row.Cells["tagno"].Value != null && !string.IsNullOrEmpty(row.Cells["tagno"].Value.ToString()))
                            {
                                // If tagno is generated, update the existing entry in the database
                                // If tagno is generated, update the existing entry in the database
                                if (UpdateData(row))
                                {
                                    txtMessageBox.Text = "Data Updated Successfully for " + row.Cells["tagno"].Value.ToString() + ".";
                                    messageBoxTimer.Start();
                                }
                            }
                            else
                            {
                                // If tagno is not generated, insert a new entry in the database
                                if (InsertData(row))
                                {
                                    txtMessageBox.Text = "Data Added Successfully for " + row.Cells["tagno"].Value.ToString() + ".";
                                    messageBoxTimer.Start();
                                }
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // If there's an exception, return false
                    return false;
                }
            }
            else if (check == 1)
            {
                try
                {
                    // Check if the "tagno" cell is not null or empty
                    if (selectedRow.Cells["tagno"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["tagno"].Value.ToString()))
                    {
                        // If tagno is generated, update the existing entry in the database
                        // If tagno is generated, update the existing entry in the database
                        if (UpdateData(selectedRow))
                        {
                            txtMessageBox.Text = "Data Updated Successfully for " + selectedRow.Cells["tagno"].Value.ToString() + ".";
                            messageBoxTimer.Start();
                        }
                    }
                    else
                    {
                        // If tagno is not generated, insert a new entry in the database
                        if (InsertData(selectedRow))
                        {
                            txtMessageBox.Text = "Data Added Successfully for " + selectedRow.Cells["tagno"].Value.ToString() + ".";
                            messageBoxTimer.Start();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // If there's an exception, return false
                    return false;
                }
            }
            return false;
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
        private bool UpdateData(DataGridViewRow row)
        {
            if (!ValidateData(row))
            {
                // Validation failed, return or handle accordingly
                return false;
            }
            string updateQuery = "UPDATE MAIN_DATA SET ITEM_DESC = @type, ITEM_PURITY = @caret, GW = @gross, NW = @net, LABOUR_AMT = @labour," +
                "WHOLE_LABOUR_AMT = @wholelable, OTHER_AMT = @other, HUID1 = @huid1, HUID2 = @huid2,PRICE= @price, SIZE = @size, COMMENT = @comment, ITEM_CODE = @prCode WHERE TAG_NO = @tagNo";
            {
                // Set parameters for the update query
                SQLiteParameter[] parameters = new SQLiteParameter[]
{
    new SQLiteParameter("@tagNo", row.Cells["tagno"].Value?.ToString()),
    new SQLiteParameter("@type", row.Cells["type"].Value?.ToString()),
    new SQLiteParameter("@caret", row.Cells["caret"].Value?.ToString()),
    new SQLiteParameter("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value)),
    new SQLiteParameter("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value)),
    new SQLiteParameter("@wholelable", Convert.IsDBNull(row.Cells["wholeLabour"].Value) ? 0 : Convert.ToDecimal(row.Cells["wholeLabour"].Value)),
    new SQLiteParameter("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value)),
    new SQLiteParameter("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value)),
    new SQLiteParameter("@huid1", null),
    new SQLiteParameter("@huid2", null),
    new SQLiteParameter("@price", row.Cells["price"].Value?.ToString()),
    new SQLiteParameter("@size", row.Cells["size"].Value?.ToString()),
    new SQLiteParameter("@comment", row.Cells["comment"].Value?.ToString()),
    new SQLiteParameter("@prCode", row.Cells["prcode"].Value?.ToString())
};
                if (helper.RunQueryWithParametersSYADataBase(updateQuery, parameters))
                {
                    return true;
                }
                return false;

            }
        }
        private bool InsertData(DataGridViewRow row)
        {
            try
            {
                string InsertQuery = "INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW,WHOLE_LABOUR_AMT, LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, COMMENT,IT_TYPE, ITEM_CODE, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, PRICE, STATUS, AC_CODE, AC_NAME) VALUES ( @tagNo, @type, @caret, @gross, @net,@wholelabouramt, @labour, @other, @huid1, @huid2, @size, @comment,@ittype, @prCode, @coYear, @coBook, @vchNo, @vchDate, @price, @status, @acCode, @acName)";
                {
                    // Call UpdateTagNo for each row
                    UpdateTagNo(row.Index);
                    if (!ValidateData(row))
                    {
                        row.Cells["tagno"].Value = null;
                        // Validation failed, return or handle accordingly
                        return false;
                    }
                    SQLiteParameter[] parameters = new SQLiteParameter[]
    {
    new SQLiteParameter("@tagNo", row.Cells["tagno"].Value?.ToString()),
    new SQLiteParameter("@type", row.Cells["type"].Value?.ToString()),
    new SQLiteParameter("@caret", row.Cells["caret"].Value?.ToString()),
    new SQLiteParameter("@gross", Convert.IsDBNull(row.Cells["gross"].Value) ? 0 : Convert.ToDecimal(row.Cells["gross"].Value)),
    new SQLiteParameter("@net", Convert.IsDBNull(row.Cells["net"].Value) ? 0 : Convert.ToDecimal(row.Cells["net"].Value)),
    new SQLiteParameter("@wholelabouramt", Convert.IsDBNull(row.Cells["wholeLabour"].Value) ? 0 : Convert.ToDecimal(row.Cells["wholeLabour"].Value)),
    new SQLiteParameter("@labour", Convert.IsDBNull(row.Cells["labour"].Value) ? 0 : Convert.ToDecimal(row.Cells["labour"].Value)),
    new SQLiteParameter("@other", Convert.IsDBNull(row.Cells["other"].Value) ? 0 : Convert.ToDecimal(row.Cells["other"].Value)),
    new SQLiteParameter("@huid1", null),
    new SQLiteParameter("@huid2", null),
    new SQLiteParameter("@size", row.Cells["size"].Value?.ToString()),
    new SQLiteParameter("@comment", row.Cells["comment"].Value?.ToString()),
    new SQLiteParameter("@prCode", row.Cells["prcode"].Value?.ToString()),
    // Add parameters for fixed data
    new SQLiteParameter("@coYear", DateTime.Now.ToString("yyyy") + "-" + (DateTime.Now.Year + 1).ToString("yyyy")),
    new SQLiteParameter("@coBook", "015"),
    new SQLiteParameter("@vchNo", "SYA00"),
    new SQLiteParameter("@ittype", "S"),
    new SQLiteParameter("@vchDate", DateTime.Now),
    new SQLiteParameter("@price", row.Cells["price"].Value?.ToString()),
    new SQLiteParameter("@status", "INSTOCK"),
    new SQLiteParameter("@acCode", null),
    new SQLiteParameter("@acName", null)
    };

                    // Execute the insert query
                    if (helper.RunQueryWithParametersSYADataBase(InsertQuery, parameters))
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error inserting data: {ex.Message}");
                MessageBox.Show($"Error inserting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        // --------------------------------------------------------------------------------------------
        // Printing Tags
        // --------------------------------------------------------------------------------------------
        private void btnAddSilverPrintTag_Click(object sender, EventArgs e)
        {
            PrintData(true);
        }
        private void btnAddSiilverSelectAll_Click(object sender, EventArgs e)
        {
            // Iterate through all rows except the last one and set the value of the "select" column to true
            for (int i = 0; i < addSilverDataGridView.Rows.Count - 1; i++)
            {
                addSilverDataGridView.Rows[i].Cells["select"].Value = true;
            }
        }
        private void PrintData(bool single)
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "TSC_TE244";


                pd.PrintPage += new PrintPageEventHandler(PrintPageSilver925);
                if (!single)
                {


                    for (int rowIndex = 0; rowIndex < addSilverDataGridView.Rows.Count; rowIndex++)
                    {
                        // Set the current row using the index
                        DataGridViewRow row = addSilverDataGridView.Rows[rowIndex];
                        addSilverDataGridView.CurrentCell = row.Cells[0]; // Set the current cell in the first column

                        // Print the current row
                        pd.Print();
                    }
                }
                else
                {
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PrintPageSilver925(object sender, PrintPageEventArgs e)
        {
            DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;

            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                string caret = (selectedRow.Cells["caret"].Value ?? "").ToString() ?? "0";

                if (tagNumber.Length > 1 && caret == "925" && tagtype == "price")
                //if (tagNumber.Length > 1 && caret == "925")
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

                    //price
                    e.Graphics.DrawString("\u20B9" + selectedRow.Cells["price"].Value.ToString(), new Font("Arial", (float)14, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //caret
                    e.Graphics.DrawString(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

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

                    //price2
                    e.Graphics.DrawString("\u20B9" + selectedRow.Cells["price"].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)27), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

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
                    string huid1 = (selectedRow.Cells[10].Value ?? "").ToString();
                    if (huid1.Length > 0)
                    {
                        e.Graphics.DrawString(huid1, new Font("Arial", (float)7.5, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                }
                else if (tagNumber.Length > 1 && caret == "925" && tagtype == "weight")
                //if (tagNumber.Length > 1 && caret == "925")
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
                    if ((selectedRow.Cells["type"].Value ?? "0").ToString() == "KADALI" || (selectedRow.Cells["size"].Value ?? "0").ToString() != null)
                    {
                        if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                        {
                            e.Graphics.DrawString((selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, (float)4, (float)75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                            e.Graphics.DrawString((selectedRow.Cells["size"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        else
                        {
                            e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                            e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        string huid1 = (selectedRow.Cells["size"].Value ?? "").ToString();
                        if (huid1.Length > 0)
                        {
                            e.Graphics.DrawString(huid1, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                    }
                    else
                    {
                        if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                        {
                            e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        else
                        {
                            e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                            e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        }
                        //huid

                        string huid1 = (selectedRow.Cells["size"].Value ?? "").ToString();
                        if (huid1.Length > 0)
                        {
                            e.Graphics.DrawString(huid1, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        }
                    }
                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //caret
                    e.Graphics.DrawString(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

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

                    //net
                    e.Graphics.DrawString(selectedRow.Cells["net"].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)4, (float)56.5, (float)17), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //test
                    //tagno
                    e.Graphics.DrawString(tagNumber, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)38, (float)105.5, (float)12), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    //labour                
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)21, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)19, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)28.5, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                }
                else if (tagNumber.Length > 1 && caret == "SLO" && tagtype == "weight")
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
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    //caret
                    e.Graphics.DrawString(selectedRow.Cells["caret"].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

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

                    //net
                    e.Graphics.DrawString(selectedRow.Cells["net"].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)4, (float)56.5, (float)17), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });

                    //test
                    //tagno
                    e.Graphics.DrawString(tagNumber, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)115.5, (float)38, (float)105.5, (float)12), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    //labour                
                    string labour = "0";
                    if ((selectedRow.Cells["labour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)21, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)19, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)119.5, (float)28.5, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }




                }
            }
        }


        private void messageBoxTimer_Tick(object sender, EventArgs e)
        {
            // Clear the TextBox after the timer interval
            txtMessageBox.Text = string.Empty;

            // Stop the timer
            messageBoxTimer.Stop();
        }

        private void btnQuickSaveAndPrint_Click(object sender, EventArgs e)
        {
            if (btnQuickSaveAndPrint.Text == "Enable Quick Save & Print")
            {
                btnQuickSaveAndPrint.Text = "Disable Quick Save & Print";
                txtMessageBox.Text = "Quick Save & Print Enabled.";
                messageBoxTimer.Start();
                quickSaveAndPrint = true;
            }
            else if (btnQuickSaveAndPrint.Text == "Disable Quick Save & Print")
            {
                btnQuickSaveAndPrint.Text = "Enable Quick Save & Print";
                txtMessageBox.Text = "Quick Save & Print Disabled.";
                messageBoxTimer.Start();
                quickSaveAndPrint = false;
            }
        }

        private void buttonquicksave_Click(object sender, EventArgs e)
        {
            if (buttonquicksave.Text == "Enable Quick Save")
            {
                buttonquicksave.Text = "Disable Quick Save";
                txtMessageBox.Text = "Quick Save Enabled.";
                messageBoxTimer.Start();
                quickSave = true;
            }
            else if (buttonquicksave.Text == "Disable Quick Save")
            {
                buttonquicksave.Text = "Enable Quick Save";
                txtMessageBox.Text = "Quick Save Disabled.";
                messageBoxTimer.Start();
                quickSave = false;
            }
        }

        private void addSilver_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            DataGridView dataGridView10 = addSilverDataGridView;
            string currentColumnName1 = dataGridView10.Columns[dataGridView10.CurrentCell.ColumnIndex].Name;
            DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
            if (currentColumnName1 == "net")
            {
                selectedRow.Cells["net"].Value = helper.correctWeight(selectedRow.Cells["net"].Value);
            }
            if (currentColumnName1 == "gross")
            {
                selectedRow.Cells["gross"].Value = helper.correctWeight(selectedRow.Cells["gross"].Value);
            }

            if (quickSaveAndPrint)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        DataGridViewRow empty = new DataGridViewRow();
                        //   DataGridViewRow selectedRow = dataGridView1.CurrentRow;

                        if (SaveData(selectedRow, 1))
                        {
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintData(true);
                            }
                        }
                    }

                }
            }
            //quick print

            else if (quickSave)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        //    DataGridViewRow empty = new DataGridViewRow();
                        //   DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        SaveData(selectedRow, 1);

                    }

                }
            }

        }

        private void addSilverDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView10 = addSilverDataGridView;
            string currentColumnName1 = dataGridView10.Columns[dataGridView10.CurrentCell.ColumnIndex].Name;
            if (currentColumnName1 == "net")
            {
                DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
                selectedRow.Cells["net"].Value = helper.correctWeight(selectedRow.Cells["net"].Value);
            }
            if (quickSaveAndPrint)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        DataGridViewRow empty = new DataGridViewRow();
                        DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;

                        if (SaveData(selectedRow, 1))
                        {
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintData(true);
                            }
                        }
                    }

                }
            }
            //quick print

            else if (quickSave)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        //    DataGridViewRow empty = new DataGridViewRow();
                        DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
                        SaveData(selectedRow, 1);

                    }

                }
            }
        }

        private void addSilverDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.PreviewKeyDown -= dataGridView_EditingControl_PreviewKeyDown;
            e.Control.PreviewKeyDown += dataGridView_EditingControl_PreviewKeyDown;
        }
        private void dataGridView_EditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            DataGridView dataGridView10 = addSilverDataGridView;
            string currentColumnName1 = dataGridView10.Columns[dataGridView10.CurrentCell.ColumnIndex].Name;
            DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;

            if (quickSaveAndPrint)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        DataGridViewRow empty = new DataGridViewRow();
                        //   DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        if (SaveData(selectedRow, 1))
                        {
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintData(true);
                            }
                        }
                    }

                }
            }

            else if (quickSave)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        //    DataGridViewRow empty = new DataGridViewRow();
                        //   DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        SaveData(selectedRow, 1);

                    }

                }
            }
        }

        private void BTNTAGTYPE_Click(object sender, EventArgs e)
        {
            if (BTNTAGTYPE.Text == "Weight Tag")
            {
                BTNTAGTYPE.Text = "Price Tag";
                tagtype = "price";
            }
            else if (BTNTAGTYPE.Text == "Price Tag")
            {
                BTNTAGTYPE.Text = "Weight Tag";
                tagtype = "weight";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintData(false);
        }
    }
}
