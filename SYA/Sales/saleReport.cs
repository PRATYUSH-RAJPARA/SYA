using Microsoft.Office.Interop.Excel;
using SYA.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Font = System.Drawing.Font;
namespace SYA
{
    public partial class saleReport : Form
    {
        DataTable slDataTable = new DataTable();
        string currentMonth;
        string previousMonth;
        string previous_previousMonth;
        public saleReport()
        {
            InitializeComponent();
            datagridviewload();
            // Subscribe to the RowPrePaint event
            dataGridView1.RowPrePaint += dataGridView1_RowPrePaint;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
        }
        private void saleReport_Load(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panel10.Visible = false;
            getMonth();
        }
        private void datagridviewload()
        {
            // Set default text alignment for all columns
            // Assuming you have a DataGridView named dataGridView1
            // Add VCH_DATE column
            DataGridViewTextBoxColumn coBookColumn = new DataGridViewTextBoxColumn();
            coBookColumn.HeaderText = "CO_BOOK";
            coBookColumn.Name = "CO_BOOK";
            dataGridView1.Columns.Add(coBookColumn);
            dataGridView1.Columns["CO_BOOK"].Visible = false;
            // Add VCH_DATE column
            DataGridViewTextBoxColumn vchDateColumn = new DataGridViewTextBoxColumn();
            vchDateColumn.HeaderText = "DATE";
            vchDateColumn.Name = "VCH_DATE";
            dataGridView1.Columns.Add(vchDateColumn);
            // Add VCH_NO column
            DataGridViewTextBoxColumn vchNoColumn = new DataGridViewTextBoxColumn();
            vchNoColumn.HeaderText = "BILL NO";
            vchNoColumn.Name = "VCH_NO";
            dataGridView1.Columns.Add(vchNoColumn);
            // Add AC_NAME column
            DataGridViewTextBoxColumn acNameColumn = new DataGridViewTextBoxColumn();
            acNameColumn.HeaderText = "NAME";
            acNameColumn.Name = "AC_NAME";
            dataGridView1.Columns.Add(acNameColumn);
            // Add GR_WT column
            DataGridViewTextBoxColumn ntWtColumn = new DataGridViewTextBoxColumn();
            ntWtColumn.HeaderText = "NET WEIGHT";
            ntWtColumn.Name = "NT_WT";
            dataGridView1.Columns.Add(ntWtColumn);
            // Add NET_AMT column
            DataGridViewTextBoxColumn netAmtColumn = new DataGridViewTextBoxColumn();
            netAmtColumn.HeaderText = "NET AMOUNT";
            netAmtColumn.Name = "NET_AMT";
            dataGridView1.Columns.Add(netAmtColumn);
            // Add CGST_TAX column
            DataGridViewTextBoxColumn cgstTaxColumn = new DataGridViewTextBoxColumn();
            cgstTaxColumn.HeaderText = "CGST";
            cgstTaxColumn.Name = "CGST_TAX";
            dataGridView1.Columns.Add(cgstTaxColumn);
            // Add SGST_TAX column
            DataGridViewTextBoxColumn sgstTaxColumn = new DataGridViewTextBoxColumn();
            sgstTaxColumn.HeaderText = "SGST";
            sgstTaxColumn.Name = "SGST_TAX";
            dataGridView1.Columns.Add(sgstTaxColumn);
            // Add TOT_AMT column
            DataGridViewTextBoxColumn totAmtColumn = new DataGridViewTextBoxColumn();
            totAmtColumn.HeaderText = "TOTAL AMOUNT";
            totAmtColumn.Name = "TOT_AMT";
            dataGridView1.Columns.Add(totAmtColumn);
            // Add CASH_AMT column
            DataGridViewTextBoxColumn cashAmtColumn = new DataGridViewTextBoxColumn();
            cashAmtColumn.HeaderText = "CASH";
            cashAmtColumn.Name = "CASH_AMT";
            dataGridView1.Columns.Add(cashAmtColumn);
            // Add CARD_AMT column
            DataGridViewTextBoxColumn cardAmtColumn = new DataGridViewTextBoxColumn();
            cardAmtColumn.HeaderText = "CARD";
            cardAmtColumn.Name = "CARD_AMT";
            dataGridView1.Columns.Add(cardAmtColumn);
            // Add CHQ_AMT column
            DataGridViewTextBoxColumn chqAmtColumn = new DataGridViewTextBoxColumn();
            chqAmtColumn.HeaderText = "CHEQUE";
            chqAmtColumn.Name = "CHQ_AMT";
            dataGridView1.Columns.Add(chqAmtColumn);
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                // Set horizontal alignment for content cells
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // Set horizontal and vertical alignments for header cells
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; // Disable text wrapping
            }
            ApplyCustomStyles();
            dataGridView1.Refresh();
        }
        private void ApplyCustomStyles()
        {
            dataGridView1.RowHeadersVisible = false;
            // Define the widths for each column
            int[] columnWidths = { 150, 100, 100, 285, 75,100,90,90,100,100,100,100 }; // Example widths, adjust as needed
            // Apply styles to the DataGridView columns
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                DataGridViewColumn column = dataGridView1.Columns[i];
                // Set individual column width
                if (i < columnWidths.Length)
                {
                    column.Width = columnWidths[i];
                }
                else
                {
                    column.Width = 100; // Default width if not specified
                }
                // Set column header style
                column.HeaderCell.Style.BackColor = Color.LightBlue;
                column.HeaderCell.Style.ForeColor = Color.Black;
                column.HeaderCell.Style.Font = new Font("Arial", 10F, FontStyle.Bold);
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                // Set default cell style for the entire column
                column.DefaultCellStyle.BackColor = Color.White;
                column.DefaultCellStyle.ForeColor = Color.Black;
                column.DefaultCellStyle.Font = new Font("Arial", 10F, FontStyle.Regular);
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
            private void btnShowData_Click(object sender, EventArgs e)
        {
            DateTime startDate = startDatePicker.Value.Date;
            DateTime endDate = endDatePicker.Value.Date;
            showdata(startDate, endDate);
        }
        private void ExportDataWithDynamicFileName(DataGridView dataGridView, string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    MessageBox.Show("The specified folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DateTime startDate = startDatePicker.Value.Date;
                DateTime endDate = endDatePicker.Value.Date;
                string dateRange = $"{startDate.ToString("dd-MM-yyyy")} to {endDate.ToString("dd-MM-yyyy")}";
                string month = startDate.ToString("MMMM");
                string baseFileName = $"{month}_{dateRange}";
                string fileName = baseFileName + ".xlsx";
                string fullPath = Path.Combine(folderPath, fileName);
                int counter = 1;
                while (File.Exists(fullPath))
                {
                    fileName = $"{baseFileName}_{counter}.xlsx";
                    fullPath = Path.Combine(folderPath, fileName);
                    counter++;
                }
                ExportToExcel(dataGridView, fullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportToExcel(DataGridView dataGridView, string filePath)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook excelWorkbook = excelApp.Workbooks.Add();
                Worksheet excelWorksheet = (Worksheet)excelWorkbook.Sheets[1];
                // Copy the column headers from the DataGridView to Excel
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    excelWorksheet.Cells[1, i + 1] = dataGridView.Columns[i].HeaderText;
                    ApplyHeaderCellFormatting(excelWorksheet.Cells[1, i + 1]);
                }
                int rowIndex = 2;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        excelWorksheet.Cells[rowIndex, j + 1] = row.Cells[j].Value;
                        ApplyDataCellFormatting(excelWorksheet.Cells[rowIndex, j + 1]);
                    }
                    rowIndex++;
                }
                excelWorksheet.Columns.AutoFit();
                excelWorkbook.SaveAs(filePath);
                excelWorkbook.Close();
                excelApp.Quit();
                MessageBox.Show("Data exported to Excel successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyHeaderCellFormatting(Microsoft.Office.Interop.Excel.Range cell)
        {
            cell.Font.Bold = true;
            cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            cell.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            cell.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        }
        private void ApplyDataCellFormatting(Microsoft.Office.Interop.Excel.Range cell)
        {
            cell.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            cell.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        }
        private void AddMergedRow(Microsoft.Office.Interop.Excel.Worksheet worksheet, int rowIndex, string value, int columnCount)
        {
            worksheet.Cells[rowIndex, 1] = value;
            worksheet.Range[worksheet.Cells[rowIndex, 1], worksheet.Cells[rowIndex, columnCount]].Merge();
        }
        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            if ((row.Cells[0].Value ?? "0").ToString() != "0")
            {
                // Check if NET_WT is <= 0
                if (Convert.ToDecimal(row.Cells["NT_WT"].Value) <= 0)
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cell's background color to red
                    row.Cells["NT_WT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                // Check if total_amt / weight <= 4000
                decimal totalAmt = Convert.ToDecimal(row.Cells["TOT_AMT"].Value);
                decimal weight = Convert.ToDecimal(row.Cells["NT_WT"].Value);
                if (weight != 0 && totalAmt / weight <= 4000 && (((row.Cells[0].Value ?? "0").ToString() == "026") || (row.Cells[0].Value ?? "0").ToString() == "26"))
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["NT_WT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["TOT_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                else if (weight != 0 && totalAmt / weight <= 60 && (((row.Cells[0].Value ?? "0").ToString() == "027") || (row.Cells[0].Value ?? "0").ToString() == "27"))
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["NT_WT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["TOT_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                // Check if cash + card + cheque > total_amt
                decimal cashAmt = Convert.ToDecimal(row.Cells["CASH_AMT"].Value);
                decimal cardAmt = Convert.ToDecimal(row.Cells["CARD_AMT"].Value);
                decimal chqAmt = Convert.ToDecimal(row.Cells["CHQ_AMT"].Value);
                if (cashAmt + cardAmt + chqAmt > totalAmt)
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["CASH_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["CARD_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["CHQ_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                // Check if SGST != CGST
                if (row.Cells["SGST_TAX"].Value.ToString() != row.Cells["CGST_TAX"].Value.ToString())
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["SGST_TAX"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["CGST_TAX"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Trigger the RowPrePaint event to force repaint after cell value changes
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                // Reset the cell colors to default values
                ResetCellColors(row);
                // Check conditions and update cell colors if needed
                CheckAndHighlightErrors(row);
                dataGridView1.InvalidateRow(e.RowIndex);
            }
        }
        private void ResetCellColors(DataGridViewRow row)
        {
            // Reset the background color of the entire row
            row.DefaultCellStyle.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            // Reset the background color of specific cells
            row.Cells["NT_WT"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            row.Cells["TOT_AMT"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            row.Cells["CASH_AMT"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            row.Cells["CARD_AMT"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            row.Cells["CHQ_AMT"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            row.Cells["SGST_TAX"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
            row.Cells["CGST_TAX"].Style.BackColor = dataGridView1.DefaultCellStyle.BackColor;
        }
        private void CheckAndHighlightErrors(DataGridViewRow row)
        {
            if ((row.Cells[0].Value ?? "0").ToString() != "0")
            {
                // Check if NET_WT is <= 0
                if (Convert.ToDecimal(row.Cells["NT_WT"].Value) <= 0)
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cell's background color to red
                    row.Cells["NT_WT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                // Check if total_amt / weight <= 4000
                decimal totalAmt = Convert.ToDecimal(row.Cells["TOT_AMT"].Value);
                decimal weight = Convert.ToDecimal(row.Cells["NT_WT"].Value);
                if (weight != 0 && totalAmt / weight <= 4000 && (((row.Cells[0].Value ?? "0").ToString() == "026") || (row.Cells[0].Value ?? "0").ToString() == "26"))
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["NT_WT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["TOT_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                else if (weight != 0 && totalAmt / weight <= 60 && (((row.Cells[0].Value ?? "0").ToString() == "027") || (row.Cells[0].Value ?? "0").ToString() == "27"))
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["NT_WT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["TOT_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                // Check if cash + card + cheque > total_amt
                decimal cashAmt = Convert.ToDecimal(row.Cells["CASH_AMT"].Value);
                decimal cardAmt = Convert.ToDecimal(row.Cells["CARD_AMT"].Value);
                decimal chqAmt = Convert.ToDecimal(row.Cells["CHQ_AMT"].Value);
                if (cashAmt + cardAmt + chqAmt > totalAmt)
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["CASH_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["CARD_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["CHQ_AMT"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
                object sgstValue = row.Cells["SGST_TAX"].Value;
                object cgstValue = row.Cells["CGST_TAX"].Value;
                // Check if SGST != CGST
                if (sgstValue != null && cgstValue != null && sgstValue.ToString() != cgstValue.ToString())
                {
                    // Set the entire row's background color to yellow
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 183);
                    // Set the specific cells' background color to red
                    row.Cells["SGST_TAX"].Style.BackColor = Color.FromArgb(250, 94, 31);
                    row.Cells["CGST_TAX"].Style.BackColor = Color.FromArgb(250, 94, 31);
                }
            }
        }
        private void printbtn_Click(object sender, EventArgs e)
        {
            // Export the new DataTable to Excel
            //ExportToExcel(dataGridView1, "C:\\SYA_SOFT\\config\\ExportedData.xlsx");
            DateTime startDate = startDatePicker.Value.Date;
            DateTime endDate = endDatePicker.Value.Date;
            string dateRange = $"{startDate.ToString("dd-MM-yyyy")} to {endDate.ToString("dd-MM-yyyy")}";
            ExportDataWithDynamicFileName(dataGridView1, helper.excelFile);
        }
        private void OpenFolderInExplorer(string folderPath)
        {
            try
            {
                Process.Start("explorer.exe", folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFolderInExplorer(helper.excelFile);
        }
        private void getMonth()
        {
            DateTime now = DateTime.Now;
            currentMonth = now.ToString("MMMM");
            previousMonth = now.AddMonths(-1).ToString("MMMM");
            previous_previousMonth = now.AddMonths(-2).ToString("MMMM");
            button2.Text = currentMonth;
            button3.Text = previousMonth;
            button4.Text = previous_previousMonth;
        }
        private void showdata(DateTime startDate, DateTime endDate)
        {
            try
            {
                string query = "SELECT * FROM SL_DATA WHERE VCH_DATE >= #" + startDate.ToString("MM/dd/yyyy") + "# AND VCH_DATE <= #" + endDate.ToString("MM/dd/yyyy") + "# AND CO_BOOK IN ('26', '27', '026', '027') ORDER BY CInt(CO_BOOK), VCH_DATE, VCH_NO";
                slDataTable = helper.FetchDataTableFromDataCareDataBase(query);
                // Clear existing rows in the DataGridView
                dataGridView1.Rows.Clear();
                // Create a new DataTable with the same structure as slDataTable
                DataTable exportDataTable = slDataTable.Clone();
                // Manually map the columns and populate the new DataTable
                decimal[,] sums = new decimal[4, 9];
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        sums[row, col] = 0m; // Although this is redundant as the default value is already 0
                    }
                }
                foreach (DataRow row in slDataTable.Rows)
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells["CO_BOOK"].Value = row["CO_BOOK"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["VCH_DATE"].Value = Convert.ToDateTime(row["VCH_DATE"]).ToString("dd-MM-yyyy");
                    dataGridView1.Rows[rowIndex].Cells["VCH_NO"].Value = row["VCH_NO"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["AC_NAME"].Value = row["AC_NAME"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["NT_WT"].Value = row["NT_WT"].ToString();
                    // Calculate NET_AMT as TOT_AMT - TAX_AMT
                    decimal totAmt = Convert.ToDecimal(row["TOT_AMT"]);
                    decimal taxAmt = Convert.ToDecimal(row["TAX_AMT"]);
                    decimal netAmt = totAmt - taxAmt;
                    dataGridView1.Rows[rowIndex].Cells["NET_AMT"].Value = netAmt.ToString();
                    dataGridView1.Rows[rowIndex].Cells["CGST_TAX"].Value = row["CGST_TAX"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["SGST_TAX"].Value = row["SGST_TAX"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["TOT_AMT"].Value = row["TOT_AMT"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["CASH_AMT"].Value = row["CASH_AMT"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["CARD_AMT"].Value = row["CARD_AMT"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["CHQ_AMT"].Value = row["CHQ_AMT"].ToString();
                    if (row["CO_BOOK"].ToString() == "026")
                    {
                        sums[1, 1] += Convert.ToDecimal(row["NT_WT"]);
                        sums[1, 2] += netAmt;
                        sums[1, 3] += Convert.ToDecimal(row["CGST_TAX"]);
                        sums[1, 4] += Convert.ToDecimal(row["SGST_TAX"]);
                        sums[1, 5] += Convert.ToDecimal(row["TOT_AMT"]);
                        sums[1, 6] += Convert.ToDecimal(row["CASH_AMT"]);
                        sums[1, 7] += Convert.ToDecimal(row["CARD_AMT"]);
                        sums[1, 8] += Convert.ToDecimal(row["CHQ_AMT"]);
                    }
                    if (row["CO_BOOK"].ToString() == "027")
                    {
                        sums[2, 1] += Convert.ToDecimal(row["NT_WT"]);
                        sums[2, 2] += netAmt;
                        sums[2, 3] += Convert.ToDecimal(row["CGST_TAX"]);
                        sums[2, 4] += Convert.ToDecimal(row["SGST_TAX"]);
                        sums[2, 5] += Convert.ToDecimal(row["TOT_AMT"]);
                        sums[2, 6] += Convert.ToDecimal(row["CASH_AMT"]);
                        sums[2, 7] += Convert.ToDecimal(row["CARD_AMT"]);
                        sums[2, 8] += Convert.ToDecimal(row["CHQ_AMT"]);
                    }
                    sums[3, 1] += Convert.ToDecimal(row["NT_WT"]);
                    sums[3, 2] += netAmt;
                    sums[3, 3] += Convert.ToDecimal(row["CGST_TAX"]);
                    sums[3, 4] += Convert.ToDecimal(row["SGST_TAX"]);
                    sums[3, 5] += Convert.ToDecimal(row["TOT_AMT"]);
                    sums[3, 6] += Convert.ToDecimal(row["CASH_AMT"]);
                    sums[3, 7] += Convert.ToDecimal(row["CARD_AMT"]);
                    sums[3, 8] += Convert.ToDecimal(row["CHQ_AMT"]);
                    // Populate the new DataTable
                    exportDataTable.Rows.Add(row.ItemArray);
                }
                label9.Text = sums[1, 1].ToString();
                label13.Text = sums[1, 2].ToString();
                label17.Text = sums[1, 3].ToString();
                label21.Text = sums[1, 4].ToString();
                label25.Text = sums[1, 5].ToString();
                label29.Text = sums[1, 6].ToString();
                label33.Text = sums[1, 7].ToString();
                label37.Text = sums[1, 8].ToString();
                label10.Text = sums[2, 1].ToString();
                label14.Text = sums[2, 2].ToString();
                label18.Text = sums[2, 3].ToString();
                label22.Text = sums[2, 4].ToString();
                label26.Text = sums[2, 5].ToString();
                label30.Text = sums[2, 6].ToString();
                label34.Text = sums[2, 7].ToString();
                label38.Text = sums[2, 8].ToString();
                label11.Text = sums[3, 1].ToString();
                label15.Text = sums[3, 2].ToString();
                label19.Text = sums[3, 3].ToString();
                label23.Text = sums[3, 4].ToString();
                label27.Text = sums[3, 5].ToString();
                label31.Text = sums[3, 6].ToString();
                label35.Text = sums[3, 7].ToString();
                label39.Text = sums[3, 8].ToString();
                panel8.Visible = true;
                panel10.Visible = true;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., show a message box
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfPreviousMonth = now.AddMonths(-2).Date;
            firstDayOfPreviousMonth = new DateTime(firstDayOfPreviousMonth.Year, firstDayOfPreviousMonth.Month, 1);
            DateTime lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);
            startDatePicker.Value = firstDayOfPreviousMonth;
            endDatePicker.Value = lastDayOfPreviousMonth;
            showdata(firstDayOfPreviousMonth, lastDayOfPreviousMonth);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime firstDayOfPreviousMonth = now.AddMonths(-1).Date;
            firstDayOfPreviousMonth = new DateTime(firstDayOfPreviousMonth.Year, firstDayOfPreviousMonth.Month, 1);
            DateTime lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);
            startDatePicker.Value = firstDayOfPreviousMonth;
            endDatePicker.Value = lastDayOfPreviousMonth;
            showdata(firstDayOfPreviousMonth, lastDayOfPreviousMonth);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            startDatePicker.Value = startDate;
            endDatePicker.Value = endDate;
            showdata(startDate, endDate);
        }
    }
}
