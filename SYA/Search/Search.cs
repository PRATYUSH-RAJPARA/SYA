using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using QRCoder;
using Serilog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace SYA
{
    public partial class Search : Form
    {
        bool quickSaveAndPrint = true;
        private SQLiteConnection connectionToSYADatabase;
        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection(helper.SYAConnectionString);
        }
        bool quickSave = false;
        //ok
        public Search()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }
        private void Search_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            gridviewstyle();
            InitializeLogging();
            LoadDataFromSQLite("SELECT * FROM MAIN_DATA ORDER BY VCH_DATE DESC ;");
        }
        //ok
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(helper.LogsFolder + "\\logs_tagno.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        }
        //ok
        private void messageBoxTimer_Tick(object sender, EventArgs e)
        {
            txtMessageBox.Text = string.Empty;
            messageBoxTimer.Stop();
        }
        //ok
        private void gridviewstyle()
        {
            int formWidth = this.ClientSize.Width;
            dataGridViewSearch.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(191, 135, 212); dataGridViewSearch.Columns["select"].HeaderCell.Style.BackColor = Color.FromArgb(208, 209, 255);
            dataGridViewSearch.Columns["tagno"].HeaderCell.Style.BackColor = Color.FromArgb(216, 187, 255);
            dataGridViewSearch.Columns["vchno"].HeaderCell.Style.BackColor = Color.FromArgb(222, 170, 255);
            dataGridViewSearch.Columns["vchdate"].HeaderCell.Style.BackColor = Color.FromArgb(226, 175, 255);
            dataGridViewSearch.Columns["itemdesc"].HeaderCell.Style.BackColor = Color.FromArgb(229, 179, 254);
            dataGridViewSearch.Columns["gross"].HeaderCell.Style.BackColor = Color.FromArgb(236, 188, 253);
            dataGridViewSearch.Columns["net"].HeaderCell.Style.BackColor = Color.FromArgb(243, 196, 251);
            dataGridViewSearch.Columns["huid1"].HeaderCell.Style.BackColor = Color.FromArgb(255, 203, 242);
            dataGridViewSearch.Columns["huid2"].HeaderCell.Style.BackColor = Color.FromArgb(243, 196, 251);
            dataGridViewSearch.Columns["labour"].HeaderCell.Style.BackColor = Color.FromArgb(236, 188, 253);
            dataGridViewSearch.Columns["wholeLabour"].HeaderCell.Style.BackColor = Color.FromArgb(229, 179, 254);
            dataGridViewSearch.Columns["other"].HeaderCell.Style.BackColor = Color.FromArgb(226, 175, 255);
            dataGridViewSearch.Columns["size"].HeaderCell.Style.BackColor = Color.FromArgb(222, 170, 255);
            dataGridViewSearch.Columns["price"].HeaderCell.Style.BackColor = Color.FromArgb(216, 187, 255);
            dataGridViewSearch.Columns["comment"].HeaderCell.Style.BackColor = Color.FromArgb(208, 209, 255);
            dataGridViewSearch.Columns["select"].Visible = false;
            dataGridViewSearch.Columns["tagno"].Width = (int)(formWidth * (8.0 / 68));
            dataGridViewSearch.Columns["vchno"].Width = (int)(formWidth * (4.0 / 68));
            dataGridViewSearch.Columns["vchdate"].Width = (int)(formWidth * (6.0 / 68));
            dataGridViewSearch.Columns["itemdesc"].Width = (int)(formWidth * (9.5 / 68));
            dataGridViewSearch.Columns["gross"].Width = (int)(formWidth * (4.0 / 68));
            dataGridViewSearch.Columns["net"].Width = (int)(formWidth * (4.0 / 68));
            dataGridViewSearch.Columns["huid1"].Width = (int)(formWidth * (4.0 / 68));
            dataGridViewSearch.Columns["huid2"].Width = (int)(formWidth * (4.0 / 68));
            dataGridViewSearch.Columns["labour"].Width = (int)(formWidth * (3.5 / 68));
            dataGridViewSearch.Columns["wholeLabour"].Width = (int)(formWidth * (3.5 / 68));
            dataGridViewSearch.Columns["other"].Width = (int)(formWidth * (3.5 / 68));
            dataGridViewSearch.Columns["size"].Width = (int)(formWidth * (3.0 / 68));
            dataGridViewSearch.Columns["price"].Width = (int)(formWidth * (3.0 / 68));
            dataGridViewSearch.Columns["comment"].Width = (int)(formWidth * (8.0 / 68));
            float fontSize = (float)(formWidth * 0.0066) > 12.5 ? (float)(formWidth * 0.0066) : 12.5f;
            int desiredRowHeight = (int)(fontSize + 45);
            dataGridViewSearch.RowTemplate.Height = desiredRowHeight;
            foreach (DataGridViewColumn column in dataGridViewSearch.Columns)
            {
                column.DefaultCellStyle.Font = new Font("Arial", fontSize, FontStyle.Regular);
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.Font = new Font("Arial", (float)11.5, FontStyle.Bold);
            }
        }
        //ok
        private void LoadDataFromSQLite(String query)
        {
            try
            {
                using (SQLiteDataReader reader = helper.FetchDataFromSYADataBase(query))
                {
                    if (reader != null)
                    {
                        dataGridViewSearch.DataSource = null;
                        dataGridViewSearch.Rows.Clear();
                        // Pratyush add this count to settings so user can change
                        int count = 0;
                        while (reader.Read() && count < 100)
                        {
                            count++;
                            int rowIndex = dataGridViewSearch.Rows.Add();
                            dataGridViewSearch.Rows[rowIndex].Cells["tagno"].Value = reader["TAG_NO"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["vchno"].Value = reader["VCH_NO"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["vchdate"].Value = Convert.ToDateTime(reader["VCH_DATE"]).ToString("dd-MM-yyyy");
                            dataGridViewSearch.Rows[rowIndex].Cells["itemdesc"].Value = reader["ITEM_PURITY"].ToString() + "  -  " + reader["ITEM_DESC"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["gross"].Value = string.Format("{0:0.000}", reader["GW"]);
                            dataGridViewSearch.Rows[rowIndex].Cells["net"].Value = string.Format("{0:0.000}", reader["NW"]);
                            dataGridViewSearch.Rows[rowIndex].Cells["huid1"].Value = reader["HUID1"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["huid2"].Value = reader["HUID2"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["labour"].Value = reader["LABOUR_AMT"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["wholeLabour"].Value = reader["WHOLE_LABOUR_AMT"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["other"].Value = reader["OTHER_AMT"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["size"].Value = reader["size"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["price"].Value = reader["price"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["comment"].Value = reader["COMMENT"].ToString();
                            if ((dataGridViewSearch.Rows[rowIndex].Cells["vchno"].Value?.ToString() ?? "0") == "SYA00")
                            {
                                dataGridViewSearch.Rows[rowIndex].Cells["tagno"].ReadOnly = true;
                                dataGridViewSearch.Rows[rowIndex].Cells["vchdate"].ReadOnly = true;
                                dataGridViewSearch.Rows[rowIndex].Cells["itemdesc"].ReadOnly = true;
                                dataGridViewSearch.Rows[rowIndex].Cells["gross"].ReadOnly = true;
                                dataGridViewSearch.Rows[rowIndex].Cells["net"].ReadOnly = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //okok
        private bool SaveDataToSQLite(DataGridViewRow selectedRow)
        {
            string tagNo = string.Empty;
            string gross = string.Empty;
            string net = string.Empty;
            string labour = string.Empty;
            string wholeLabour = string.Empty;
            string other = string.Empty;
            string huid1 = string.Empty;
            string huid2 = string.Empty;
            string size = string.Empty;
            string price = string.Empty;
            string comment = string.Empty;
            try
            {
                tagNo = selectedRow.Cells["tagno"].Value?.ToString() ?? string.Empty;
                gross = selectedRow.Cells["gross"].Value?.ToString() ?? "0";
                net = selectedRow.Cells["net"].Value?.ToString() ?? "0";
                labour = selectedRow.Cells["labour"].Value?.ToString() ?? "0";
                wholeLabour = selectedRow.Cells["wholeLabour"].Value?.ToString() ?? "0";
                other = selectedRow.Cells["other"].Value?.ToString() ?? "0";
                huid1 = selectedRow.Cells["huid1"].Value?.ToString()?.ToUpper() ?? string.Empty;
                huid2 = selectedRow.Cells["huid2"].Value?.ToString()?.ToUpper() ?? string.Empty;
                size = selectedRow.Cells["size"].Value?.ToString() ?? string.Empty;
                price = selectedRow.Cells["price"].Value?.ToString() ?? "0";
                comment = selectedRow.Cells["comment"].Value?.ToString() ?? string.Empty;
                if (!helper.validateHUID(huid1, huid2))
                {
                    SelectCell(dataGridViewSearch, selectedRow.Index, "huid1");
                    return false;
                }
                if ((selectedRow.Cells["vchno"].Value ?? "").ToString() != "SYA00")
                {
                    if (!helper.validateWeight(gross, net))
                    {
                        MessageBox.Show($"Gross weight should be greater than or equal to net weight for Tag Number : " + tagNo + " .");
                        SelectCell(dataGridViewSearch, selectedRow.Index, "gross");
                        return false;
                    }
                    if (!helper.validateWeight(gross))
                    {
                        MessageBox.Show($"Gross weight should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                        SelectCell(dataGridViewSearch, selectedRow.Index, "gross");
                        return false;
                    }
                    if (!helper.validateWeight(net))
                    {
                        MessageBox.Show($"Net weight should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                        SelectCell(dataGridViewSearch, selectedRow.Index, "net");
                        return false;
                    }
                }
                if (!helper.validateLabour(labour))
                {
                    MessageBox.Show($"Labour amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                    SelectCell(dataGridViewSearch, selectedRow.Index, "labour");
                    return false;
                }
                if (!helper.validateOther(wholeLabour))
                {
                    MessageBox.Show($"Other amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                    SelectCell(dataGridViewSearch, selectedRow.Index, "other");
                    return false;
                }
                if (!helper.validateOther(other))
                {
                    MessageBox.Show($"Other amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                    SelectCell(dataGridViewSearch, selectedRow.Index, "other");
                    return false;
                }
                string updateQuery = $"UPDATE MAIN_DATA SET HUID1 = '{huid1}', HUID2 = '{huid2}', COMMENT = '{comment}',LABOUR_AMT = '{labour}',OTHER_AMT = '{other}',WHOLE_LABOUR_AMT = '{wholeLabour}', GW = '{gross}',NW = '{net}',SIZE = '{size}',PRICE = '{price}'  WHERE TAG_NO = '{tagNo}'";
                helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                txtMessageBox.Text = "Data Saved Successfully.";
                messageBoxTimer.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to SQLite. " +
                                $"TagNo: {tagNo}, Gross: {gross}, Net: {net}, " +
                                $"Labour: {labour}, WholeLabour: {wholeLabour}, " +
                                $"Other: {other}, HUID1: {huid1}, HUID2: {huid2}, " +
                                $"Size: {size}, Price: {price}, Comment: {comment}. " +
                                $"Error: {ex.Message} \n\nStackTrace: {ex.StackTrace}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show($"Error saving data to SQLite. TagNo: {tagNo}, Gross: {gross}, Net: {net}, Labour: {labour}, WholeLabour: {wholeLabour}, Other: {other}, HUID1: {huid1}, HUID2: {huid2}, Size: {size}, Price: {price}, Comment: {comment}. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //to be removed
        private void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
        }
        //ok
        private void PrintLabels()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "TSC_TE244";
                pd.PrintPage += new PrintPageEventHandler(PrintData);
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //okok
        private void PrintData(object sender, PrintPageEventArgs e)
        {
            print.PrintPageSearch(sender, e, dataGridViewSearch);
        }
        private bool leaveEventFlag = false;
        //ok
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
        //ok
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
        private void dataGridViewSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string currentColumnName1 = dataGridViewSearch.Columns[dataGridViewSearch.CurrentCell.ColumnIndex].Name;
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
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
            if (e.KeyCode == Keys.Tab)
            {
                DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                string currentColumnName = dataGridViewSearch.Columns[dataGridViewSearch.CurrentCell.ColumnIndex].Name;
                int currentRowIndex = dataGridViewSearch.CurrentCell.RowIndex;
                if (currentColumnName == "comment")
                {
                    if (SaveDataToSQLite(selectedRow))
                    {
                        if (quickSaveAndPrint)
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
        }
        private void txtTagno_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtTagno.Text = null;
            leaveEventFlag = false;
        }
        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE GW LIKE '%{txtWeight.Text}%' OR NW LIKE '%{txtWeight.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtWeight_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtWeight.Text = null;
            leaveEventFlag = false;
        }
        private void txtBillNo_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE VCH_NO LIKE '%{txtBillNo.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtBillNo_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtBillNo.Text = null;
            leaveEventFlag = false;
        }
        private void txtHUID_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE HUID1 LIKE '%{txtHUID.Text}%' OR HUID2 LIKE '%{txtHUID.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtHUID_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtHUID.Text = null;
            leaveEventFlag = false;
        }
        private void txtSearchAnything_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string searchValue = txtSearchAnything.Text;
                string query = $"SELECT * FROM MAIN_DATA WHERE " +
                                $"TAG_NO LIKE '%{searchValue}%' OR " +
                                $"VCH_NO LIKE '%{searchValue}%' OR " +
                                $"ITEM_PURITY LIKE '%{searchValue}%' OR " +
                                $"ITEM_DESC LIKE '%{searchValue}%' OR " +
                                $"COMMENT LIKE '%{searchValue}%' OR " +
                                $"CO_YEAR LIKE '%{searchValue}%' OR " +
                                $"CO_BOOK LIKE '%{searchValue}%' OR " +
                                $"VCH_DATE LIKE '%{searchValue}%' OR " +
                                $"GW LIKE '%{searchValue}%' OR " +
                                $"NW LIKE '%{searchValue}%' OR " +
                                $"LABOUR_AMT LIKE '%{searchValue}%' OR " +
                                $"WHOLE_LABOUR_AMT LIKE '%{searchValue}%' OR " +
                                $"OTHER_AMT LIKE '%{searchValue}%' OR " +
                                $"IT_TYPE LIKE '%{searchValue}%' OR " +
                                $"ITEM_CODE LIKE '%{searchValue}%' OR " +
                                $"HUID1 LIKE '%{searchValue}%' OR " +
                                $"HUID2 LIKE '%{searchValue}%' OR " +
                                $"SIZE LIKE '%{searchValue}%' OR " +
                                $"PRICE LIKE '%{searchValue}%' OR " +
                                $"STATUS LIKE '%{searchValue}%' OR " +
                                $"AC_CODE LIKE '%{searchValue}%' OR " +
                                $"AC_NAME LIKE '%{searchValue}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtSearchAnything_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtSearchAnything.Text = null;
            leaveEventFlag = false;
        }
        private void dataGridViewSearch_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.PreviewKeyDown -= dataGridView_EditingControl_PreviewKeyDown;
            e.Control.PreviewKeyDown += dataGridView_EditingControl_PreviewKeyDown;
        }
        private void dataGridView_EditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (quickSaveAndPrint)
            {
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    if (editingControl != null)
                    {
                        DataGridView dataGridView = dataGridViewSearch;
                        string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                        int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                        if (currentColumnName == "net")
                        {
                            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                            selectedRow.Cells["net"].Value = helper.correctWeight(selectedRow.Cells["net"].Value);
                        }
                        if (currentColumnName == "comment")
                        {
                            DataGridViewRow empty = new DataGridViewRow();
                            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                            if (SaveDataToSQLite(selectedRow))
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
            }
        }
        private void txtTagno_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE TAG_NO LIKE '%{txtTagno.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void dataGridViewSearch_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "gross")
                {
                    dataGridViewSearch.Rows[e.RowIndex].Cells["net"].Value = dataGridViewSearch.Rows[e.RowIndex].Cells["gross"].Value;
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                else if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "net")
                {
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                else if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "huid1")
                {
                    DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }
                else if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "huid2")
                {
                    DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }
            }
        }
        string queryToFetchFromMSAccess = null;
        private void btnFetch_Click(object sender, EventArgs e)
        {
            queryToFetchFromMSAccess = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '015' OR CO_BOOK = '15'";
            HelperFetchData.InsertInStockDataIntoSQLite(txtMessageBox, queryToFetchFromMSAccess);
        }
        private void btnFetchSaleData_Click(object sender, EventArgs e)
        {
            //  queryToFetchFromMSAccess = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '026' OR CO_BOOK = '26'";
            //  HelperFetchData.InsertSaleDataIntoSQLite(queryToFetchFromMSAccess);
            HelperFetchData.fetchSaleData();
        }
        private void dataGridViewSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
