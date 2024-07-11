using SYA.Helper;
using System.Data;
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
        public void GoldDetailSummary()
        {
            string query = "SELECT * FROM MAIN_DATA WHERE IT_TYPE = 'G'";
            DataTable MAIN_DATA = helper.FetchDataTableFromSYADataBase(query);
            DataTable Table_916 = MAIN_DATA.Clone();
            PopulateTable(Table_916, "916");
            StockHelper.LoadDataGridView(Table_916, dataGridView1, "G");
            DataTable Table_18k = MAIN_DATA.Clone();
            PopulateTable(Table_18k, "18K");
            StockHelper.LoadDataGridView(Table_18k, dataGridView2, "G");
            DataTable Table_20k = MAIN_DATA.Clone();
            PopulateTable(Table_20k, "20K");
            StockHelper.LoadDataGridView(Table_20k, dataGridView3, "G");
            DataTable Table_KDM = MAIN_DATA.Clone();
            PopulateTable(Table_KDM, "KDM");
            StockHelper.LoadDataGridView(Table_KDM, dataGridView4, "G");
            dataGridView1.Sort(dataGridView1.Columns["ITEM_NAME"], System.ComponentModel.ListSortDirection.Ascending);
            dataGridView2.Sort(dataGridView2.Columns["ITEM_NAME"], System.ComponentModel.ListSortDirection.Ascending);
            dataGridView3.Sort(dataGridView3.Columns["ITEM_NAME"], System.ComponentModel.ListSortDirection.Ascending);
            dataGridView4.Sort(dataGridView4.Columns["ITEM_NAME"], System.ComponentModel.ListSortDirection.Ascending);
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
            StockHelper.LoadDataGridView(dt, dg, "G");
        }
        private void StyleDataGridView(DataGridView dgv)
        {
            StockHelper.StyleDataGridView(dgv);
        }
    }
}
