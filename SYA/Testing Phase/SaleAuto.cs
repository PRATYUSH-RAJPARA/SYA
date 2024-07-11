namespace SYA.Testing_Phase
{
    public partial class SaleAuto : Form
    {
        public SaleAuto()
        {
            InitializeComponent();
        }

        private void SaleAuto_Load(object sender, EventArgs e)
        {
            SaleReportAutomation saleReportAutomation = new SaleReportAutomation();
            saleReportAutomation.main_fnc();
            dataGridView1.DataSource = saleReportAutomation.final;
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;
            dataGridView1.Refresh();
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ApplyConditionalFormatting();
        }

        void ApplyConditionalFormatting()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.OwningColumn.Name == "DATEMATCH" && Convert.ToBoolean(cell.Value))
                    {
                        cell.Style.BackColor = Color.LightGreen;
                    }
                    else if (cell.OwningColumn.Name == "CARDMATCH" && Convert.ToBoolean(cell.Value))
                    {
                        cell.Style.BackColor = Color.LightGreen;
                    }
                    else if (cell.OwningColumn.Name == "CASHMATCH" && Convert.ToBoolean(cell.Value))
                    {
                        cell.Style.BackColor = Color.LightGreen;
                    }
                    else if (cell.OwningColumn.Name == "CHQMATCH" && Convert.ToBoolean(cell.Value))
                    {
                        cell.Style.BackColor = Color.LightGreen;
                    }
                }
            }
        }
    }
}
