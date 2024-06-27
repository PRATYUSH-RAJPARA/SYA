using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA.Stocks
{
    public partial class goldStockDetailedSummary : Form
    {
        public goldStockDetailedSummary()
        {
            InitializeComponent();
        }

        private void StockDetailedSummary_Load(object sender, EventArgs e)
        {
            GoldDetailSummary();
        }

        private void AddColumnToDataGridView(DataGridView dg)
        {
            // Add columns to DataGridView916
            DataGridViewColumn itemCodeColumn = new DataGridViewTextBoxColumn();
            itemCodeColumn.Name = "ITEM_CODE";
            itemCodeColumn.HeaderText = "Item Code";
            dg.Columns.Add(itemCodeColumn);

            DataGridViewColumn itemNameColumn = new DataGridViewTextBoxColumn();
            itemNameColumn.Name = "ITEM_NAME";
            itemNameColumn.HeaderText = "Item Name";
            dg.Columns.Add(itemNameColumn);

            DataGridViewColumn countColumn = new DataGridViewTextBoxColumn();
            countColumn.Name = "COUNT";
            countColumn.HeaderText = "Count";
            dg.Columns.Add(countColumn);

            DataGridViewColumn netWeightColumn = new DataGridViewTextBoxColumn();
            netWeightColumn.Name = "NET_WEIGHT";
            netWeightColumn.HeaderText = "Net Weight";
            dg.Columns.Add(netWeightColumn);

            DataGridViewColumn grossWeightColumn = new DataGridViewTextBoxColumn();
            grossWeightColumn.Name = "GROSS_WEIGHT";
            grossWeightColumn.HeaderText = "Gross Weight";
            dg.Columns.Add(grossWeightColumn);
        }

        private void DesignDataGridView(DataGridView dgv)
        {
            // Hide the row header column (0th column)
            dgv.RowHeadersVisible = false;

            // Set each column width to 100
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.Width = 100;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }

        public void GoldDetailSummary()
        {
            string query = "SELECT * FROM MAIN_DATA WHERE IT_TYPE = 'G'";

            DataTable MAIN_DATA = helper.FetchDataTableFromSYADataBase(query);
            DataTable Table_916 = MAIN_DATA.Clone();
            PopulateTable(Table_916, "916");
            loadDG(Table_916, dataGridView1);

            DataTable Table_18k = MAIN_DATA.Clone();
            PopulateTable(Table_18k, "18K");
            loadDG(Table_18k, dataGridView2);
            DataTable Table_20k = MAIN_DATA.Clone();
            PopulateTable(Table_20k, "20K");
            loadDG(Table_20k, dataGridView3);
            DataTable Table_KDM = MAIN_DATA.Clone();
            PopulateTable(Table_KDM, "KDM");
            loadDG(Table_KDM, dataGridView4);

            dataGridView1.Sort(dataGridView1.Columns["ITEM_NAME"], ListSortDirection.Ascending);
            dataGridView2.Sort(dataGridView2.Columns["ITEM_NAME"], ListSortDirection.Ascending);
            dataGridView3.Sort(dataGridView3.Columns["ITEM_NAME"], ListSortDirection.Ascending);
            dataGridView4.Sort(dataGridView4.Columns["ITEM_NAME"], ListSortDirection.Ascending);

            void PopulateTable(DataTable dt, string purity)
            {
                foreach (DataRow row in MAIN_DATA.Rows)
                {
                    if (row["ITEM_PURITY"].ToString() == purity)
                    {
                        dt.ImportRow(row);
                    }
                }
            }
        }
        //pratyush
        public void loadDG(DataTable dt, DataGridView dg)
        {
            AddColumnToDataGridView(dg);
            DesignDataGridView(dg);
            StyleDataGridView(dg);

            // Clear existing rows
            dg.Rows.Clear();

            // Group the rows by ITEM_CODE and calculate the total count, net weight, and gross weight for each unique ITEM_CODE
            var groupedRows = dt.AsEnumerable()
                .GroupBy(row => row.Field<string>("ITEM_CODE"))
                .Select(group =>
                {
                    string itemCode = group.Key;
                    string itemName = GetItemDescFromSQLite(itemCode, "G");
                    int count = group.Count(); // Manually count the items in the group
                    decimal netWeight = group.Sum(row => row.Field<decimal>("NW"));
                    decimal grossWeight = group.Sum(row => row.Field<decimal>("GW"));

                    return new
                    {
                        ItemCode = itemCode,
                        ItemName = itemName,
                        Count = count,
                        NetWeight = netWeight,
                        GrossWeight = grossWeight
                    };
                });

            // Fill DataGridView with data from groupedRows
            foreach (var row in groupedRows)
            {
                string itemCode = row.ItemCode;
                string itemName = row.ItemName;
                int count = row.Count;
                decimal netWeight = row.NetWeight;
                decimal grossWeight = row.GrossWeight;

                dg.Rows.Add(itemCode, itemName, count, netWeight, grossWeight);
            }

            // Calculate totals directly from the DataGridView
            int totalCount = 0;
            decimal totalNetWeight = 0;
            decimal totalGrossWeight = 0;

            foreach (DataGridViewRow row in dg.Rows)
            {
                if (row.IsNewRow) continue; // Skip the new row if any

                totalCount += Convert.ToInt32(row.Cells["COUNT"].Value);
                totalNetWeight += Convert.ToDecimal(row.Cells["NET_WEIGHT"].Value);
                totalGrossWeight += Convert.ToDecimal(row.Cells["GROSS_WEIGHT"].Value);
            }

            dg.Rows.Add("", "", "", "", "");
            // Add the total row at the bottom
            dg.Rows.Add("---", " ------ T O T A L ------ ", totalCount, totalNetWeight, totalGrossWeight);

            // Style the total row differently if needed
            int totalRowIndex = dg.Rows.Count - 1;
            DataGridViewCellStyle totalRowStyle = new DataGridViewCellStyle();
            totalRowStyle.Font = new Font("Arial", 11, FontStyle.Bold);
            totalRowStyle.BackColor = Color.LightGray; // Example color for total row background

            foreach (DataGridViewColumn column in dg.Columns)
            {
                dg.Rows[totalRowIndex].Cells[column.Index].Style = totalRowStyle;
            }

            // Ensure the total row is always at the bottom after sorting
            dg.Sorted += (s, e) =>
            {
                DataGridView dataGridView = (DataGridView)s;
                DataGridViewRow totalRow = dataGridView.Rows.Cast<DataGridViewRow>()
                                                .FirstOrDefault(r => r.Cells["ITEM_CODE"].Value?.ToString() == "---");
                DataGridViewRow EMPTYRow = dataGridView.Rows.Cast<DataGridViewRow>()
                                                .FirstOrDefault(r => r.Cells["ITEM_CODE"].Value?.ToString() == "");
                if (EMPTYRow != null)
                {
                    dataGridView.Rows.Remove(EMPTYRow);
                    dataGridView.Rows.Add(EMPTYRow);
                }
                if (totalRow != null)
                {
                    dataGridView.Rows.Remove(totalRow);
                    dataGridView.Rows.Add(totalRow);
                }
            };
        }


        public string GetItemDescFromSQLite(string PR_CODE, string IT_TYPE)
        {
            string query = "SELECT IT_NAME FROM ITEM_MASTER WHERE PR_CODE = '" + PR_CODE + "' AND IT_TYPE = '" + IT_TYPE + "'";
            object result = helper.RunQueryWithoutParametersSYADataBase(query, "ExecuteScalar");
            if (result != null)
            {
                return result.ToString();
            }
            return string.Empty;
        }

        // goldStockDetailedSummary.cs

        private void StyleDataGridView(DataGridView dgv)
        {
            // Set custom column widths
            dgv.Columns["ITEM_CODE"].Width = 60;
            dgv.Columns["ITEM_NAME"].Width = 200;
            dgv.Columns["COUNT"].Width = 60;
            dgv.Columns["NET_WEIGHT"].Width = 90;
            dgv.Columns["GROSS_WEIGHT"].Width = 90;

            // Center-align text in all cells
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Set default row font size
            dgv.DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold); // Adjust the font size as needed

            // Set text color for all cells
            dgv.DefaultCellStyle.ForeColor = Color.Black; // <-- Set text color to black

            // Define colors for columns
            Dictionary<string, Color> columnColors = new Dictionary<string, Color>
            {
                { "ITEM_CODE", Color.FromArgb(113, 131, 85) },
                { "ITEM_NAME", Color.FromArgb(135, 152, 106) },
                { "COUNT", Color.FromArgb(151, 169, 124) },
                { "NET_WEIGHT", Color.FromArgb(181, 201, 154) },
                { "GROSS_WEIGHT", Color.FromArgb(207, 225, 185) }
            };

            // Loop through each column to apply styles
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                // Set font size for headers
                column.HeaderCell.Style.Font = new Font("Arial", 13, FontStyle.Bold); // Adjust the font size as needed

                // Set background color based on column name mapping or default
                if (columnColors.ContainsKey(column.Name))
                {
                    column.DefaultCellStyle.BackColor = columnColors[column.Name];
                    column.HeaderCell.Style.BackColor = columnColors[column.Name];
                }
                else
                {
                    column.HeaderCell.Style.BackColor = Color.FromArgb(113, 131, 85); // Default color for headers
                }
            }

            // Refresh the DataGridView to apply styles
            dgv.Refresh();
        }

       
    }
}
