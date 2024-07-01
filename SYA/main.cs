using SYA.Helper;
using SYA.Sales;
using SYA.Sell;
using SYA.Stocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SYA
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(main_KeyDown);
        }
        private void main_Load(object sender, EventArgs e)
        {
            btnStocks.Visible = true;
            //btnRtgs.Visible = false;
            btnImportData.Visible = true;
            btnPrintTags.Visible = false;
            btnCustomer.Visible = false;
            panelsecond.Visible = false;
            btnSortContact.Visible = false;
            //panelchild.visible = false;
            btnHideAllSecondPanelButtons();
            helper.loadSettingsValues();
            helper.loadLabourTable();
        }
        // Loads form by name in panelchild
        private void LoadForm(Form form)
        {
            // Close the currently displayed form (if any)
            if (panelChild.Controls.Count > 0)
            {
                Form currentForm = panelChild.Controls[0] as Form;
                if (currentForm != null)
                {
                    currentForm.Close();
                    currentForm.Dispose();
                }
            }
            // Set the properties of the new form
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            // Add the new form to the panel
            panelChild.Controls.Add(form);
            // Sho the Panel Child
            panelChild.Visible = true;
            // Show the new form
            form.Show();
        }
        // main left panel buttons
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
        }
        private void btnSellItem_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button6.Visible = true;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new Search());
        }
        private void btnSales_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
        }
        private void btnStocks_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button13.Visible = true;
            button12.Visible = true;
            button23.Visible = true;
            button7.Visible = true;
        }
        private void btnPrintTags_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
        }
        private void btnRtgs_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button17.Visible = true;
        }
        private void btnImportData_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button20.Visible = true;
            button3.Visible = true;
            button5.Visible = true;
        }
        private void btnHideAllSecondPanelButtons()
        {
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button13.Visible = false;
            button12.Visible = false;
            button17.Visible = false;
            button20.Visible = false;
            button23.Visible = false;
            button22.Visible = false;
            button7.Visible = false;
        }
        // button add gold
        private void button1_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new addgold());
        }
        // BUTTON DATACARE ADD
        private void button2_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new addSilver());
        }
        private void button10_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new saleReport());
            //  LoadForm(new Form1());
        }
        private void button6_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new sell());
        }
        private void button13_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new VerifyStock());
        }
        private void btnSortContact_Click(object sender, EventArgs e)
        {
            //RichTextBox r = new RichTextBox();
            //Contact.SortContactData(r, "datacare");
        }
        private void button17_Click(object sender, EventArgs e)
        {
            //RichTextBox r = new RichTextBox();
            //Contact contact = new Contact();
            //contact.SortContactData(r, "datacare");
            panelsecond.Visible = false;
            LoadForm(new PrintRTGS());
        }
        private void button11_Click(object sender, EventArgs e)
        {
        }
        private void button12_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new STOCKSummary());
        }
        private void main_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl+S is pressed
            if (e.Control && e.KeyCode == Keys.S)
            {
                // Open the form
                LoadForm(new settings());
            }
            if (e.Control && e.KeyCode == Keys.L)
            {
                // Open the form
                LoadForm(new Labour());
            }
        }
        private void button22_Click_1(object sender, EventArgs e)
        {
            LoadForm(new PrintLabel());
        }
        private void button23_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new goldStockDetailedSummary());
        }
        private void button7_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new silverStockDetailedSummary());
        }
        private void button20_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            FetchSaleDataHelper.fetchSaleData();
            string queryToFetchFromMSAccess = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '015' OR CO_BOOK = '15'";
            HelperFetchData.InsertInStockDataIntoSQLite(queryToFetchFromMSAccess);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            string queryToFetchFromMSAccess = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '015' OR CO_BOOK = '15'";
            HelperFetchData.InsertInStockDataIntoSQLite(queryToFetchFromMSAccess);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            FetchSaleDataHelper.fetchSaleData();
        }
    }
    public class ApiResponseItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string CurrentValue { get; set; }
        public string OpenValue { get; set; }
        public string HighValue { get; set; }
        public string LowValue { get; set; }
    }
}
