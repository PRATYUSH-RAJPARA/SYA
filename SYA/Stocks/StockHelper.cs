using SYA.Helper;
using System.Data;
namespace SYA.Stocks
{
    public static class StockHelper
    {
        public static void AddColumns(DataGridView dg)
        {
            // Add columns to DataGridView
            AddTextBoxColumn(dg, "ITEM_CODE", "Item Code", 60);
            AddTextBoxColumn(dg, "ITEM_NAME", "Item Name", 200);
            AddTextBoxColumn(dg, "COUNT", "Count", 60);
            AddTextBoxColumn(dg, "NET_WEIGHT", "Net Weight", 90);
            AddTextBoxColumn(dg, "GROSS_WEIGHT", "Gross Weight", 90);
        }
        public static void DesignDataGridView(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }
        public static void StyleDataGridView(DataGridView dgv)
        {
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
        private static void AddTextBoxColumn(DataGridView dg, string name, string headerText, int width)
        {
            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = name;
            column.HeaderText = headerText;
            column.Width = width;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dg.Columns.Add(column);
        }
        public static void LoadDataGridView(DataTable dt, DataGridView dg, string itemDescType)
        {
            AddColumns(dg);
            DesignDataGridView(dg);
            StyleDataGridView(dg);
            dg.Rows.Clear();
            var groupedRows = dt.AsEnumerable()
                .GroupBy(row => row.Field<string>("ITEM_CODE"))
                .Select(group =>
                {
                    string itemCode = group.Key;
                    string itemName = GetItemDescFromSQLite(itemCode, itemDescType);
                    int count = group.Count();
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
            foreach (var row in groupedRows)
            {
                dg.Rows.Add(row.ItemCode, row.ItemName, row.Count, row.NetWeight, row.GrossWeight);
            }
            int totalCount = 0;
            decimal totalNetWeight = 0;
            decimal totalGrossWeight = 0;
            foreach (DataGridViewRow row in dg.Rows)
            {
                if (!row.IsNewRow)
                {
                    totalCount += Convert.ToInt32(row.Cells["COUNT"].Value);
                    totalNetWeight += Convert.ToDecimal(row.Cells["NET_WEIGHT"].Value);
                    totalGrossWeight += Convert.ToDecimal(row.Cells["GROSS_WEIGHT"].Value);
                }
            }
            //  dg.Rows.Add("", "", 0,0,0);
            dg.Rows.Add("---", " ------ T O T A L ------ ", totalCount, totalNetWeight, totalGrossWeight);
            dg.Sorted += (s, e) =>
            {
                DataGridView dataGridView = (DataGridView)s;
                DataGridViewRow totalRow = dataGridView.Rows.Cast<DataGridViewRow>()
                                                .FirstOrDefault(r => r.Cells["ITEM_CODE"].Value?.ToString() == "---");
                DataGridViewRow emptyRow = dataGridView.Rows.Cast<DataGridViewRow>()
                                                .FirstOrDefault(r => r.Cells["ITEM_CODE"].Value?.ToString() == "");
                if (emptyRow != null)
                {
                    dataGridView.Rows.Remove(emptyRow);
                    dataGridView.Rows.Add(emptyRow);
                }
                if (totalRow != null)
                {
                    dataGridView.Rows.Remove(totalRow);
                    dataGridView.Rows.Add(totalRow);
                }
            };
        }
        private static string GetItemDescFromSQLite(string PR_CODE, string IT_TYPE)
        {
            string query = "SELECT IT_NAME FROM ITEM_MASTER WHERE PR_CODE = '" + PR_CODE + "' AND IT_TYPE = '" + IT_TYPE + "'";
            object result = helper.RunQueryWithoutParametersSYADataBase(query, "ExecuteScalar");
            return result?.ToString() ?? string.Empty;
        }
    }
}
