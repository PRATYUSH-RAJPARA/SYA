using Humanizer;
using System;
using System.Data;
using System.Drawing.Printing;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;
namespace SYA
{
    public partial class PrintRTGS : Form
    {
        private string selectedID;
        List<string> RTGSDATA = new List<string>();
        public PrintRTGS()
        {
            InitializeComponent();
        }
        private void PrintRTGS_Load(object sender, EventArgs e)
        {
            panel9.Visible = false;
            panel42.Visible = false;
            panel47.Visible = false;
            panel54.Visible = false;
            panel8.Visible = false;
            button1.Focus();
        }
        private void PrintLabels()
        {
            try
            {
                //PrintDocument pd = new PrintDocument();
                //pd.PrinterSettings.PrinterName = "HP LaserJet MFP M129-M134";
                //PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                //printPreviewDialog.Document = pd;
                //printPreviewDialog.WindowState = FormWindowState.Maximized;
                //printPreviewDialog.PrintPreviewControl.Zoom = 1.0;
                //pd.PrintPage += new PrintPageEventHandler(Print);
                //printPreviewDialog.ShowDialog();
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = helper.NormalPrinterName;
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
            print.PrintRTGS(sender, e,RTGSDATA);
        }
        private void SecondForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RTGSList secondForm = (RTGSList)sender;
            FetchAndLoadData(secondForm.GetSelectedID());
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            RTGSList secondForm = new RTGSList();
            secondForm.FormClosed += SecondForm_FormClosed;
            secondForm.ShowDialog();
        }
        private void FetchAndLoadData(string id)
        {
            DataTable dt = new DataTable();
            fetch();
            load();
            void fetch()
            {
                if (!string.IsNullOrEmpty(id))
                {
                    dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM RTGSData where ID = " + id);
                }
            }
            void load()
            {
                label15.Text = dt.Rows[0][3].ToString();
                DateTime currentDate = DateTime.Today;
                label16.Text = currentDate.ToString("dd-MM-yy");
                label18.Text = dt.Rows[0][2].ToString();
                label19.Text = dt.Rows[0][3].ToString();
                label20.Text = dt.Rows[0][4].ToString();
                label21.Text = dt.Rows[0][5].ToString();
                label23.Text = dt.Rows[0][7].ToString();
                label24.Text = dt.Rows[0][8].ToString();
                panel9.Visible = true;
                panel42.Visible = true;
                panel47.Visible = true;
                panel54.Visible = true;
                panel8.Visible = true;
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        textBox3.Focus();
                    }));
                } catch { }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SortAndPrintRTGS(textBox3.Text);
        }
        private void SortAndPrintRTGS(string amt)
        {
            string amountWord = string.Empty;
            numberToWord();
            createList();
            printRTGS();
            void numberToWord()
            {
                if (int.TryParse(amt, out int amount) && amount > 0)
                {
                    amountWord = ConvertToWords(amount) + " only";
                }
                else
                {
                    MessageBox.Show("Please enter a valid positive non-zero numeric value.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            string ConvertToWords(int number)
            {
                string[] ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
                string[] teens = { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                string[] tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
                if (number == 0)
                    return "Zero";
                else if (number < 0)
                    return "Minus " + ConvertToWords(Math.Abs(number));
                string words = "";
                if ((number / 10000000) > 0)
                {
                    words += ConvertToWords(number / 10000000) + " Crore ";
                    number %= 10000000;
                }
                if ((number / 100000) > 0)
                {
                    words += ConvertToWords(number / 100000) + " Lakh ";
                    number %= 100000;
                }
                if ((number / 1000) > 0)
                {
                    words += ConvertToWords(number / 1000) + " Thousand ";
                    number %= 1000;
                }
                if ((number / 100) > 0)
                {
                    words += ConvertToWords(number / 100) + " Hundred ";
                    number %= 100;
                }
                if (number > 0)
                {
                    if (words != "")
                        words += "and ";
                    if (number < 10)
                        words += ones[number];
                    else if (number < 20)
                        words += teens[number - 10];
                    else
                    {
                        words += tens[number / 10];
                        if ((number % 10) > 0)
                            words += " " + ones[number % 10];
                    }
                }
                return words;
            }
            void createList()
            {
                RTGSDATA.Clear();
                string RTGS_NEFT = null;
                if (checkBox1.Checked == true)
                {
                    RTGS_NEFT = "RTGS";
                }
                else if (checkBox2.Checked == true)
                {
                    RTGS_NEFT = "NEFT";
                }
                string SA_CA = null;
                if (checkBox3.Checked == true)
                {
                    SA_CA = "SA";
                }
                else if (checkBox4.Checked == true)
                {
                    SA_CA = "CA";
                }
                RTGSDATA.Add("V V NAGAR");//1
                RTGSDATA.Add(label16.Text);//2
                RTGSDATA.Add(RTGS_NEFT);//3
                RTGSDATA.Add(label18.Text);//4
                RTGSDATA.Add(label19.Text);//5
                RTGSDATA.Add(label20.Text);//6
                RTGSDATA.Add(label21.Text);//7
                RTGSDATA.Add(SA_CA);//8
                RTGSDATA.Add(label23.Text);//9
                RTGSDATA.Add(label24.Text);//10
                RTGSDATA.Add(amt);//11
                RTGSDATA.Add("-");//12
                RTGSDATA.Add(amt);//13
                RTGSDATA.Add(amountWord);//14
                RTGSDATA.Add("0036102000016232");//15
                RTGSDATA.Add("SHREE YAMUNA ABHUSHAN");//16
                RTGSDATA.Add(" ");//17
                RTGSDATA.Add(" ");//18
                RTGSDATA.Add("SHREE YAMUNA ABHUSHAN - PARTNER");//19
            }
            void printRTGS()
            {
                PrintLabels();
            }
        }
        public void PrintRTGS_API(string id,string amt)
        {
            FetchAndLoadData(id);
            SortAndPrintRTGS(amt);
        }
    }
}
