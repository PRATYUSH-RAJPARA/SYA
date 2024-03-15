using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public partial class saleReport : Form
    {
        DataTable slDataTable = new DataTable();
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

            dataGridView1.Refresh();

        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = startDatePicker.Value.Date;
                DateTime endDate = endDatePicker.Value.Date;

                string query = "SELECT * FROM SL_DATA WHERE VCH_DATE >= #" + startDate.ToString("MM/dd/yyyy") + "# AND VCH_DATE <= #" + endDate.ToString("MM/dd/yyyy") + "# AND CO_BOOK IN ('26', '27', '026', '027') ORDER BY CInt(CO_BOOK), VCH_DATE, VCH_NO";





                slDataTable = helper.FetchFromDataCareDataBase(query);

                // Clear existing rows in the DataGridView
                dataGridView1.Rows.Clear();

                // Create a new DataTable with the same structure as slDataTable
                DataTable exportDataTable = slDataTable.Clone();

                // Manually map the columns and populate the new DataTable
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

                    // Populate the new DataTable
                    exportDataTable.Rows.Add(row.ItemArray);
                }

               
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., show a message box
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportToExcel(DataGridView dataGridView, string filePath)
        {
            //try
            //{
            //    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //    Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
            //    Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.Sheets[1];

            //    // Copy the column headers from the DataGridView to Excel
            //    for (int i = 0; i < dataGridView.Columns.Count; i++)
            //    {
            //        excelWorksheet.Cells[1, i + 1] = dataGridView.Columns[i].HeaderText;

            //        // Apply formatting to header cells
            //        ApplyHeaderCellFormatting(excelWorksheet.Cells[1, i + 1]);
            //    }

            //    // Initialize row index for data
            //    int rowIndex = 2;
            //    bool goldOccured = false;
            //    bool silverOccured = false;

            //    // Export data from DataGridView to Excel
            //    foreach (DataGridViewRow row in dataGridView.Rows)
            //    {
            //        if (!goldOccured && (row.Cells[0].Value.ToString() == "026" || row.Cells[0].Value.ToString() == "26"))
            //        {
            //            goldOccured = true;
            //            // Add a new row for "Gold Invoice" with merged cells
            //            AddMergedRow(excelWorksheet, rowIndex, "Gold Invoice", dataGridView.Columns.Count);

            //            // Apply formatting to the merged cell
            //            ApplyMergedCellFormatting(excelWorksheet.Range[excelWorksheet.Cells[rowIndex, 1], excelWorksheet.Cells[rowIndex, dataGridView.Columns.Count]]);

            //            rowIndex++;
            //        }

            //        if (!silverOccured && (row.Cells[0].Value.ToString() == "027" || row.Cells[0].Value.ToString() == "27"))
            //        {
            //            silverOccured = true;
            //            // Add a new row for "Silver Invoice" with merged cells
            //            AddMergedRow(excelWorksheet, rowIndex, "Silver Invoice", dataGridView.Columns.Count);

            //            // Apply formatting to the merged cell
            //            ApplyMergedCellFormatting(excelWorksheet.Range[excelWorksheet.Cells[rowIndex, 1], excelWorksheet.Cells[rowIndex, dataGridView.Columns.Count]]);

            //            rowIndex++;
            //        }

            //        // Export data for each row
            //        for (int j = 0; j < dataGridView.Columns.Count; j++)
            //        {
            //            excelWorksheet.Cells[rowIndex, j + 1] = row.Cells[j].Value;

            //            // Apply formatting to data cells
            //            ApplyDataCellFormatting(excelWorksheet.Cells[rowIndex, j + 1]);
            //        }
            //        rowIndex++;
            //    }

            //    // AutoFit column widths
            //    excelWorksheet.Columns.AutoFit();

            //    // Save the Excel file
            //    excelWorkbook.SaveAs(filePath);
            //    excelWorkbook.Close();
            //    excelApp.Quit();

            //    MessageBox.Show("Data exported to Excel successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }


        //private void ApplyHeaderCellFormatting(Microsoft.Office.Interop.Excel.Range cell)
        //{
        //    cell.Font.Bold = true;
        //    cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
        //    cell.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        //    cell.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        //}

        //private void ApplyMergedCellFormatting(Microsoft.Office.Interop.Excel.Range cellRange)
        //{
        //    cellRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gold);
        //    cellRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    cellRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        //    cellRange.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        //}

        //private void ApplyDataCellFormatting(Microsoft.Office.Interop.Excel.Range cell)
        //{
        //    cell.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //    cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        //    cell.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
        //}

        //private void AddMergedRow(Microsoft.Office.Interop.Excel.Worksheet worksheet, int rowIndex, string value, int columnCount)
        //{
        //    worksheet.Cells[rowIndex, 1] = value;
        //    worksheet.Range[worksheet.Cells[rowIndex, 1], worksheet.Cells[rowIndex, columnCount]].Merge();
        //}


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
            ExportToExcel(dataGridView1, helper.excelFile);

        }
    }
}
