using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            dataGridView1.DataSource = saleReportAutomation.match_data_result;
        }
    }
}
