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
    public partial class silverStockDetailedSummary : Form
    {
        public silverStockDetailedSummary()
        {
            InitializeComponent();

        }

        private void silverStocksDetailedSummary_Load(object sender, EventArgs e)
        {
            SilverDetailSummary();
        }

        private void AddColumnToDataGridView(DataGridView dg)
        {
            DataGridViewColumn itemCodeColumn = new DataGridViewTextBoxColumn();
            itemCodeColumn.Name = "ITEM_CODE";
            itemCodeColumn.HeaderText = "Item Code";
            itemCodeColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            dg.Columns.Add(itemCodeColumn);

            DataGridViewColumn itemNameColumn = new DataGridViewTextBoxColumn();
            itemNameColumn.Name = "ITEM_NAME";
            itemNameColumn.HeaderText = "Item Name";
            itemNameColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            dg.Columns.Add(itemNameColumn);

            DataGridViewColumn countColumn = new DataGridViewTextBoxColumn();
            countColumn.Name = "COUNT";
            countColumn.HeaderText = "Count";
            countColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            dg.Columns.Add(countColumn);

            DataGridViewColumn netWeightColumn = new DataGridViewTextBoxColumn();
            netWeightColumn.Name = "NET_WEIGHT";
            netWeightColumn.HeaderText = "Net Weight";
            netWeightColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            dg.Columns.Add(netWeightColumn);

            DataGridViewColumn grossWeightColumn = new DataGridViewTextBoxColumn();
            grossWeightColumn.Name = "GROSS_WEIGHT";
            grossWeightColumn.HeaderText = "Gross Weight";
            grossWeightColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            dg.Columns.Add(grossWeightColumn);
        }

        private void DesignDataGridView(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.Width = 100;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }

        public void SilverDetailSummary()
        {
            string query = "SELECT * FROM MAIN_DATA WHERE IT_TYPE = 'S'";

            DataTable MAIN_DATA = helper.FetchDataTableFromSYADataBase(query);
            DataTable Table_SLO = MAIN_DATA.Clone();
            PopulateTable(Table_SLO, "SLO");
            loadDG(Table_SLO, dataGridView1);


            DataTable Table_925 = MAIN_DATA.Clone();
            PopulateTable(Table_925, "925");
            loadDG(Table_925, dataGridView2);

            dataGridView1.Sort(dataGridView1.Columns["ITEM_NAME"], ListSortDirection.Ascending);
            dataGridView2.Sort(dataGridView2.Columns["ITEM_NAME"], ListSortDirection.Ascending);
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
                    string itemName = GetItemDescFromSQLite(itemCode, "S");
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

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.Columns["ITEM_CODE"].Width = 60;
            dgv.Columns["ITEM_NAME"].Width = 200;
            dgv.Columns["COUNT"].Width = 60;
            dgv.Columns["NET_WEIGHT"].Width = 90;
            dgv.Columns["GROSS_WEIGHT"].Width = 90;

            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
            dgv.DefaultCellStyle.ForeColor = Color.Black;

            Dictionary<string, Color> columnColors = new Dictionary<string, Color>
            {
                { "ITEM_CODE", Color.FromArgb(113, 131, 85) },
                { "ITEM_NAME", Color.FromArgb(135, 152, 106) },
                { "COUNT", Color.FromArgb(151, 169, 124) },
                { "NET_WEIGHT", Color.FromArgb(181, 201, 154) },
                { "GROSS_WEIGHT", Color.FromArgb(207, 225, 185) }
            };

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderCell.Style.Font = new Font("Arial", 13, FontStyle.Bold);

                if (columnColors.ContainsKey(column.Name))
                {
                    column.DefaultCellStyle.BackColor = columnColors[column.Name];
                    column.HeaderCell.Style.BackColor = columnColors[column.Name];
                }
                else
                {
                    column.HeaderCell.Style.BackColor = Color.FromArgb(113, 131, 85);
                }
            }

            dgv.Refresh();
        }

        

        
    }
}
