using System;
using System.Data;
using System.Windows.Forms;
using SYA.Helper;

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

        public void SilverDetailSummary()
        {
            string query = "SELECT * FROM MAIN_DATA WHERE IT_TYPE = 'S'";
            DataTable MAIN_DATA = helper.FetchDataTableFromSYADataBase(query);

            DataTable Table_SLO = MAIN_DATA.Clone();
            PopulateTable(Table_SLO, "SLO");
            StockHelper.LoadDataGridView(Table_SLO, dataGridView1, "S");

            DataTable Table_925 = MAIN_DATA.Clone();
            PopulateTable(Table_925, "925");
            StockHelper.LoadDataGridView(Table_925, dataGridView2, "S");

            dataGridView1.Sort(dataGridView1.Columns["ITEM_NAME"], System.ComponentModel.ListSortDirection.Ascending);
            dataGridView2.Sort(dataGridView2.Columns["ITEM_NAME"], System.ComponentModel.ListSortDirection.Ascending);


             void PopulateTable(DataTable dt, string purity)
            {
                foreach (System.Data.DataRow row in MAIN_DATA.Rows)
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
            StockHelper.LoadDataGridView(dt, dg, "S");
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            StockHelper.StyleDataGridView(dgv);
        }
    }
}
