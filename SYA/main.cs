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
        }

        private void main_Load(object sender, EventArgs e)
        {
            btnStocks.Visible = false;
            //btnRtgs.Visible = false;
            btnImportData.Visible = false;
            btnPrintTags.Visible = false;
            btnCustomer.Visible = false;
            panelsecond.Visible = false;
            panelChild.Visible = false;
            btnHideAllSecondPanelButtons();
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
        private void LoadForms(Form form)
        {
            // Close and dispose the currently displayed form (if any)
            if (panelChild.Controls.Count > 0)
            {
                Form currentForm = panelChild.Controls[0] as Form;
                if (currentForm != null)
                {
                    // Clear the DataGridView if the current form has one
                    if (currentForm.Controls.Count > 0 && currentForm.Controls[0] is DataGridView dataGridView)
                    {
                        // Set the DataSource to null or an empty data source
                        dataGridView.DataSource = null;
                        // Optionally clear any existing rows
                        dataGridView.Rows.Clear();
                        // Optionally clear any existing columns
                        dataGridView.Columns.Clear();
                    }

                    // Close and dispose the current form
                    currentForm.Close();
                    currentForm.Dispose();

                    // Nullify references to aid garbage collection
                    currentForm = null;

                    // Force garbage collection and finalization
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            // Set the properties of the new form
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Add the new form to the panel
            panelChild.Controls.Add(form);

            // Show the Panel Child
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
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;

        }
        private void btnSellItem_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button21.Visible = true;

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
            button9.Visible = true;
            button10.Visible = true;
            button11.Visible = true;

        }
        private void btnStocks_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button12.Visible = true;
            button13.Visible = true;
        }
        private void btnPrintTags_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button14.Visible = true;
            button15.Visible = true;
            button16.Visible = true;
        }
        private void btnRtgs_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button17.Visible = true;
            button18.Visible = true;
            button19.Visible = true;
        }
        private void btnImportData_Click(object sender, EventArgs e)
        {
            btnHideAllSecondPanelButtons();
            panelsecond.Visible = true;
            button20.Visible = true;

        }
        private void btnHideAllSecondPanelButtons()
        {

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            button15.Visible = false;
            button16.Visible = false;
            button17.Visible = false;
            button18.Visible = false;
            button19.Visible = false;
            button20.Visible = false;
            button21.Visible = false;
        }

        // button add gold
        private void button1_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new addgold());
        }
        // BUTTON DATACARE ADD
        private void button4_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new DataCareData());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new addSilver());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new saleReport());
        }

        private void button22_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new SaleItem());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new tempVerifyData());
        }

        private void btnSortContact_Click(object sender, EventArgs e)
        {
            RichTextBox r = new RichTextBox();
            Contact.SortContactData(r, "datacare");

        }

        private void button17_Click(object sender, EventArgs e)
        {
            panelsecond.Visible = false;
            LoadForm(new PrintRTGS());
        }
    }
}
