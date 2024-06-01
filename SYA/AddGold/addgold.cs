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
using System.Diagnostics.Eventing.Reader;
namespace SYA
{
    public partial class addgold : Form
    {
        private SQLiteConnection connectionToSYADatabase;
        private SQLiteConnection connectionToDatacare;
        private const int ItemNameColumnIndex = 2;
        bool quickSave = false;
        bool quickSaveAndPrint = true;
        public addgold()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            dataGridView1.AutoGenerateColumns = false;
            gridviewstyle();
            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
            textBoxColumn.HeaderText = "PR_CODE";
            textBoxColumn.Name = "prcode";
            dataGridView1.Columns.Add(textBoxColumn);
            dataGridView1.Columns["prcode"].Visible = false;
            InitializeComboBoxColumns();
            dataGridView1.DataSource = GetEmptyDataTable();
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.RowsAdded += (s, args) => UpdateRowNumbers();
            dataGridView1.RowsRemoved += (s, args) => UpdateRowNumbers();
        }
        private void addgold_Load(object sender, EventArgs e)
        {
            InitializeLogging();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            UpdateRowNumbers();
        }
        private void addgold_SizeChanged(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;
            int ww = (int)(formWidth * 0.008);
            dataGridView1.Columns["select"].Width = 0; dataGridView1.Columns["tagno"].Width = (int)(ww * 16); dataGridView1.Columns["type"].Width = (int)(ww * 18); dataGridView1.Columns["caret"].Width = (int)(ww * 6); dataGridView1.Columns["gross"].Width = (int)(ww * 8); dataGridView1.Columns["net"].Width = (int)(ww * 8); dataGridView1.Columns["labour"].Width = (int)(ww * 10); dataGridView1.Columns["wholeLabour"].Width = (int)(ww * 10); dataGridView1.Columns["other"].Width = (int)(ww * 10); dataGridView1.Columns["huid1"].Width = (int)(ww * 10); dataGridView1.Columns["huid2"].Width = (int)(ww * 10); dataGridView1.Columns["size"].Width = (int)(ww * 6); dataGridView1.Columns["comment"].Width = (int)(ww * 8);
            if ((float)(formWidth * 0.0066) > 12.5)
            {
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.DefaultCellStyle.Font = new Font("Arial", (float)(formWidth * 0.0066), FontStyle.Regular);
                }
            }
            else
            {
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.DefaultCellStyle.Font = new Font("Arial", (float)12.5, FontStyle.Regular);
                }
            }
        }
        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection(helper.SYAConnectionString);
            connectionToDatacare = new SQLiteConnection(helper.accessConnectionString);
        }
        private void InitializeComboBoxColumns()
        {
            LoadComboBoxValues("G", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)dataGridView1.Columns["type"]);
            LoadComboBoxValues("GQ", "IT_NAME", "IT_NAME", (DataGridViewComboBoxColumn)dataGridView1.Columns["caret"]);
        }
        private void messageBoxTimer_Tick(object sender, EventArgs e)
        {
            txtMessageBox.Text = string.Empty;
            messageBoxTimer.Stop();
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell is DataGridViewTextBoxCell)
            {
                dataGridView1.BeginEdit(true);
                if (dataGridView1.EditingControl is TextBox textBox)
                {
                    textBox.SelectAll();
                }
            }
            if (e.ColumnIndex == 1 && e.RowIndex == dataGridView1.Rows.Count - 1)
            {
                dataGridView1.Rows[e.RowIndex].Cells["labour"].Value = "650";
                dataGridView1.Rows[e.RowIndex].Cells["wholeLabour"].Value = "0";
                dataGridView1.Rows[e.RowIndex].Cells["other"].Value = "0";
                if (dataGridView1.Rows.Count > 1)
                {
                    DataGridViewRow previousRow = dataGridView1.Rows[dataGridView1.Rows.Count - 2];
                    DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["type"];
                    DataGridViewComboBoxCell caretCell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells["caret"];
                    typeCell.Value = previousRow.Cells["type"].Value;
                    caretCell.Value = previousRow.Cells["caret"].Value;
                    dataGridView1.Rows[e.RowIndex].Cells["labour"].Value = (previousRow.Cells["labour"].Value ?? "0").ToString();
                    dataGridView1.Rows[e.RowIndex].Cells["wholeLabour"].Value = (previousRow.Cells["wholeLabour"].Value ?? "0").ToString();
                    dataGridView1.Rows[e.RowIndex].Cells["other"].Value = (previousRow.Cells["other"].Value ?? "0").ToString();
                }
            }
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "gross")
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells["net"].Value == null)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells["net"].Value = dataGridView1.Rows[e.RowIndex].Cells["gross"].Value;
                    }
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "net")
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "huid1")
                {
                    DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "huid2")
                {
                    DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView10 = dataGridView1;
            string currentColumnName1 = dataGridView10.Columns[dataGridView10.CurrentCell.ColumnIndex].Name;
            if (quickSaveAndPrint)
            {
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = dataGridView1;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow empty = new DataGridViewRow();
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        if (SaveData(selectedRow, 1))
                        {
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintLabels();
                            }
                        }
                    }
                }
            }
            else if (false)
            {
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = dataGridView1;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
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
            else if (quickSave)
            {
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = dataGridView1;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow selectedRow = dataGridView1.CurrentRow;
                        SaveData(selectedRow, 1);
                    }
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
            DataGridView dataGridView10 = dataGridView1;
            string currentColumnName1 = dataGridView10.Columns[dataGridView10.CurrentCell.ColumnIndex].Name;
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;
            if (currentColumnName1 == "net")
            {
                selectedRow.Cells["net"].Value = helper.correctWeight(selectedRow.Cells["net"].Value);
            }
            if (currentColumnName1 == "gross")
            {
                selectedRow.Cells["gross"].Value = helper.correctWeight(selectedRow.Cells["gross"].Value);
            }
            if (currentColumnName1 == "huid1")
            {
                selectedRow.Cells["huid1"].Value = (selectedRow.Cells["huid1"].Value ?? "").ToString().ToUpper();
            }
            if (currentColumnName1 == "huid2")
            {
                selectedRow.Cells["huid2"].Value = (selectedRow.Cells["huid2"].Value ?? "").ToString().ToUpper();
            }
            if (quickSaveAndPrint)
            {
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    DataGridView dataGridView = dataGridView1;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        DataGridViewRow empty = new DataGridViewRow();
                        if (SaveData(selectedRow, 1))
                        {
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintLabels();
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
                    DataGridView dataGridView = dataGridView1;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    if (currentColumnName == "comment")
                    {
                        SaveData(selectedRow, 1);
                    }
                }
            }
        }
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == -1)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(114, 131, 89)), e.CellBounds);
                e.Handled = true;
            }
        }
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(helper.LogsFolder + "\\logs_tagno.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        }
        private void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
        }
        private void gridviewstyle()
        {
            dataGridView1.Columns["select"].Visible = false;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(114, 131, 89); dataGridView1.Columns["select"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124);
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
            dataGridView1.Columns["comment"].HeaderCell.Style.BackColor = Color.FromArgb(151, 169, 124); foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", (float)12.5); if (column.Name == "select")
                {
                    column.Width = 40;
                }
                else if (column.Name == "type")
                {
                    column.Width = 225;
                }
                else if (column.Name == "tagno")
                {
                    column.Width = 200;
                }
                else if (column.Name == "caret")
                {
                    column.Width = 75;
                }
                else if (column.Name == "gross")
                {
                    column.Width = 100;
                }
                else if (column.Name == "net")
                {
                    column.Width = 100;
                }
                else if (column.Name == "size")
                {
                    column.Width = 80;
                }
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Font = new Font("Arial", (float)12.5, FontStyle.Bold);
            }
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
            using (SQLiteDataReader reader = helper.FetchDataFromSYADataBase($"SELECT DISTINCT {columnName} FROM ITEM_MASTER WHERE IT_TYPE = '{itemType}'"))
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
        private bool SaveData(DataGridViewRow selectedRow, int check)
        {
            if (check == 0)
            {
                try
                {
                    if (dataGridView1.Rows.Count == 0)
                    {
                        MessageBox.Show("DataGridView is empty. Check your data population logic.");
                        return false;
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            if (row.Cells["tagno"].Value != null && !string.IsNullOrEmpty(row.Cells["tagno"].Value.ToString()) && row.Cells["tagno"].Value.ToString() != "0")
                            {
                                if (UpdateData(row))
                                {
                                    row.Cells["type"].ReadOnly = true;
                                    row.Cells["caret"].ReadOnly = true;
                                    txtMessageBox.Text = "Data Updated Successfully for " + row.Cells["tagno"].Value.ToString() + ".";
                                    messageBoxTimer.Start();
                                }
                            }
                            else
                            {
                                if (InsertData(row))
                                {
                                    row.Cells["type"].ReadOnly = true;
                                    row.Cells["caret"].ReadOnly = true;
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
                    return false;
                }
            }
            else if (check == 1)
            {
                try
                {
                    if (selectedRow.Cells["tagno"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["tagno"].Value.ToString()) && selectedRow.Cells["tagno"].Value.ToString() != "0")
                    {
                        if (UpdateData(selectedRow))
                        {
                            selectedRow.Cells["type"].ReadOnly = true;
                            selectedRow.Cells["caret"].ReadOnly = true;
                            txtMessageBox.Text = "Data Updated Successfully for " + selectedRow.Cells["tagno"].Value.ToString() + ".";
                            messageBoxTimer.Start();
                        }
                    }
                    else
                    {
                        if (InsertData(selectedRow))
                        {
                            selectedRow.Cells["type"].ReadOnly = true;
                            selectedRow.Cells["caret"].ReadOnly = true;
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
            return false;
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
                            dataGridView1.Rows[rowIndex].Cells["prcode"].Value = prCode;
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
            prefix ??= "";
            prCode ??= "";
            caret ??= "";
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
            if (!helper.validateHUID(huid1, huid2))
            {
                SelectCell(dataGridView1, row.Index, "huid1");
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
            MessageBox.Show(row.Cells["comment"].Value?.ToString());
            string updateQuery = "UPDATE MAIN_DATA SET ITEM_DESC = @type, ITEM_PURITY = @caret, GW = @gross, NW = @net, " +
                     "LABOUR_AMT = @labour, WHOLE_LABOUR_AMT = @wholeLabour, OTHER_AMT = @other, HUID1 = @huid1, HUID2 = @huid2, SIZE = @size, " +
                     "\"COMMENT\" = @comment, ITEM_CODE = @prCode WHERE TAG_NO = @tagNo";
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
            if (helper.RunQueryWithParametersSYADataBase(updateQuery, parameters))
            {
                return true;
            }
            return false;
        }
        private bool InsertData(DataGridViewRow row)
        {
            try
            {
                MessageBox.Show(row.Cells["comment"].Value?.ToString());
                string InsertQuery = "INSERT INTO MAIN_DATA ( TAG_NO, ITEM_DESC, ITEM_PURITY, GW, NW, LABOUR_AMT,WHOLE_LABOUR_AMT, OTHER_AMT, HUID1, HUID2, SIZE, \"COMMENT\",IT_TYPE, ITEM_CODE, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, PRICE, STATUS, AC_CODE, AC_NAME) VALUES ( @tagNo, @type, @caret, @gross, @net, @labour,@wholeLabour, @other, @huid1, @huid2, @size, @comment,@ittype, @prCode, @coYear, @coBook, @vchNo, @vchDate, @price, @status, @acCode, @acName)";
                {
                    UpdateTagNo(row.Index);
                    if (!ValidateData(row))
                    {
                        row.Cells["tagno"].Value = null;
                        return false;
                    }
                    int currentYear = DateTime.Now.Year;
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
        private void PrintLabels()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "TSC_TE244";
                pd.PrintPage += new PrintPageEventHandler(Print);
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Print(object sender, PrintPageEventArgs e)
        {
            print.PrintPageAddGold(sender, e, dataGridView1.CurrentRow);
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
        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
        }
    }
}
