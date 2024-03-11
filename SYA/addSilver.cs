using QRCoder;
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
        private SQLiteConnection connectionToSYADatabase;
        private const int ItemNameColumnIndex = 2;
        bool quickSave = false;
        bool quickSaveAndPrint = true;
        string tagtype = "weight";
        public addSilver()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            addSilverDataGridView.RowsAdded += (s, args) => UpdateRowNumbers();
            addSilverDataGridView.RowsRemoved += (s, args) => UpdateRowNumbers();
        }
        private void addSilver_Load(object sender, EventArgs e)
        {
            InitializeLogging();
            addSilverDataGridView.AutoGenerateColumns = false;
            addSilverDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gridviewstyle();
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderText = "PR_CODE";
            textBoxColumn.Name = "prcode";
            addSilverDataGridView.Columns.Add(textBoxColumn);
            addSilverDataGridView.Columns["prcode"].Visible = false;
            InitializeComboBoxColumns();
            addSilverDataGridView.DataSource = GetEmptyDataTable();
            addSilverDataGridView.RowHeadersVisible = true;
            UpdateRowNumbers();
        }
        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection(helper.SYAConnectionString);
        }
        private void InitializeComboBoxColumns()
        {
            LoadComboBoxValues("S", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)addSilverDataGridView.Columns["type"]);
            LoadComboBoxValues("SQ", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)addSilverDataGridView.Columns["caret"]);
        }
        private void addSilverDataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (addSilverDataGridView.CurrentCell is DataGridViewTextBoxCell)
            {
                addSilverDataGridView.BeginEdit(true);
                if (addSilverDataGridView.EditingControl is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }

            if (e.ColumnIndex == 1 && e.RowIndex == addSilverDataGridView.Rows.Count - 1)
            {

                addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = "20";
                addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = "0";
                addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value = "0";
                if (addSilverDataGridView.Rows.Count > 1)
                {
                    DataGridViewRow previousRow = addSilverDataGridView.Rows[addSilverDataGridView.Rows.Count - 2];
                    DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)addSilverDataGridView.Rows[e.RowIndex].Cells["type"];
                    DataGridViewComboBoxCell caretCell = (DataGridViewComboBoxCell)addSilverDataGridView.Rows[e.RowIndex].Cells["caret"];
                    typeCell.Value = previousRow.Cells["type"].Value;
                    caretCell.Value = previousRow.Cells["caret"].Value;
                    addSilverDataGridView.Rows[e.RowIndex].Cells["labour"].Value = (previousRow.Cells["labour"].Value ?? "0").ToString();
                    addSilverDataGridView.Rows[e.RowIndex].Cells["wholeLabour"].Value = (previousRow.Cells["wholeLabour"].Value ?? "0").ToString();
                    addSilverDataGridView.Rows[e.RowIndex].Cells["other"].Value = (previousRow.Cells["other"].Value ?? "0").ToString();
                    addSilverDataGridView.Rows[e.RowIndex].Cells["size"].Value = (previousRow.Cells["size"].Value ?? "0").ToString();
                }
            }
        }
        private void addSilverDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (addSilverDataGridView.Columns[e.ColumnIndex].Name == "gross")
                {
                    if (addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value == null)
                    {
                        addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value =
                            addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value;
                    }

                    addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(addSilverDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
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
                decimal? gross = Convert.ToDecimal((addSilverDataGridView.Rows[e.RowIndex].Cells["gross"].Value ?? "0")?.ToString() ?? "0");
                decimal? net = Convert.ToDecimal(addSilverDataGridView.Rows[e.RowIndex].Cells["net"].Value?.ToString() ?? "0");
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
                //  MessageBox.Show("error is gross please check again");
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
                return value;
            }
            else
            {
                return value + (step - remainder);
            }
        }
        private void addSilverDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(helper.LogsFolder + "\\logs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        }
        private void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
        }
        private void gridviewstyle()
        {
            int formWidth = this.ClientSize.Width;
            int ww = (int)(formWidth * 0.03125);
            addSilverDataGridView.Columns["select"].Visible = false;
            addSilverDataGridView.Columns["select"].Width = 0;
            addSilverDataGridView.Columns["tagno"].Width = (int)(ww * 5);
            addSilverDataGridView.Columns["type"].Width = (int)(ww * 5);
            addSilverDataGridView.Columns["caret"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["gross"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["net"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["labour"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["wholeLabour"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["other"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["price"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["size"].Width = (int)(ww * 2);
            addSilverDataGridView.Columns["comment"].Width = (int)(ww * 4);
            if ((float)(formWidth * 0.0066) > 12.5)
            {
                foreach (DataGridViewColumn column in addSilverDataGridView.Columns)
                {
                    column.DefaultCellStyle.Font = new Font("Arial", (float)(formWidth * 0.0066), FontStyle.Regular);
                }
            }
            else
            {
                foreach (DataGridViewColumn column in addSilverDataGridView.Columns)
                {
                    column.DefaultCellStyle.Font = new Font("Arial", (float)12.5, FontStyle.Regular);
                }
            }
            addSilverDataGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(114, 131, 89); addSilverDataGridView.Columns["select"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124);
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
            addSilverDataGridView.Columns["comment"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124); foreach (DataGridViewColumn column in addSilverDataGridView.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Font = new Font("Arial", (float)12.5, FontStyle.Bold);
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
        private bool SaveData(DataGridViewRow selectedRow)
        {

            try
            {
                if (selectedRow.Cells["tagno"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["tagno"].Value.ToString()))
                {
                    if (UpdateData(selectedRow))
                    {
                        txtMessageBox.Text = "Data Updated Successfully for " + selectedRow.Cells["tagno"].Value.ToString() + ".";
                        messageBoxTimer.Start();
                    }
                }
                else
                {
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
                return false;
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
                            addSilverDataGridView.Rows[rowIndex].Cells["prcode"].Value = prCode;
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
            return true;
        }
        private bool UpdateData(DataGridViewRow row)
        {
            if (!ValidateData(row))
            {
                return false;
            }
            string updateQuery = "UPDATE MAIN_DATA SET ITEM_DESC = @type, ITEM_PURITY = @caret, GW = @gross, NW = @net, LABOUR_AMT = @labour," +
                "WHOLE_LABOUR_AMT = @wholelable, OTHER_AMT = @other, HUID1 = @huid1, HUID2 = @huid2,PRICE= @price, SIZE = @size, COMMENT = @comment, ITEM_CODE = @prCode WHERE TAG_NO = @tagNo";
            {
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
                    UpdateTagNo(row.Index);
                    if (!ValidateData(row))
                    {
                        row.Cells["tagno"].Value = null;
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
                    if (helper.RunQueryWithParametersSYADataBase(InsertQuery, parameters))
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                MessageBox.Show($"Error inserting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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
                        DataGridViewRow row = addSilverDataGridView.Rows[rowIndex];
                        addSilverDataGridView.CurrentCell = row.Cells[0]; pd.Print();
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
            print.PrintPageAddSilver(sender, e, addSilverDataGridView, tagtype);
        }
        private void messageBoxTimer_Tick(object sender, EventArgs e)
        {
            txtMessageBox.Text = string.Empty;
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
            MessageBox.Show("ajhadada");
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
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow empty = new DataGridViewRow();
                        if (SaveData(selectedRow))
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
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        SaveData(selectedRow);
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
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow empty = new DataGridViewRow();
                        DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
                        if (SaveData(selectedRow))
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
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow selectedRow = addSilverDataGridView.CurrentRow;
                        SaveData(selectedRow);
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
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow empty = new DataGridViewRow();
                        if (SaveData(selectedRow))
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
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = addSilverDataGridView;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        SaveData(selectedRow);
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
    }
}
