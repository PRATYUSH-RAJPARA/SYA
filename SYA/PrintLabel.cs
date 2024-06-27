using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public partial class PrintLabel : Form
    {
        public PrintLabel()
        {
            InitializeComponent();
        }

        private void PrintLabel_Load(object sender, EventArgs e)
        {

        }



        public string name;
        public string price;
        public string type;
        private void pp()
        {
            try
            {

                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = helper.TagPrinterName;
                pd.PrintPage += new PrintPageEventHandler(Print);
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Print(object sender, PrintPageEventArgs e)
        {
            print.PrintLabel(sender, e, name, price, type);
        }
        private void names()
        {
            type = "name";
            name = textBox2.Text.ToString();
            price = textBox4.Text.ToString();
            pp();
        }
        private void prices()
        {
            type = "price";
            name = textBox2.Text.ToString();
            price = textBox4.Text.ToString();
            pp();
        }
        private void nameandprice()
        {
            type = "nameandprice";
            name = textBox2.Text.ToString();
            price = textBox4.Text.ToString();
            pp();
        }
        private void both()
        {
            type = "name";
            name = textBox2.Text.ToString();
            price = textBox4.Text.ToString();
            pp();
            type = "price";
            name = textBox2.Text.ToString();
            price = textBox4.Text.ToString();
            pp();
        }






       

        private void button1_Click(object sender, EventArgs e)
        {
            names();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            prices();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            nameandprice();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            names();
            prices();
        }
    }
}

