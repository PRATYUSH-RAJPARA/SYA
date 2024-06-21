using QRCoder;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SYA
{
    public partial class VerifyData : Form
    {

        public VerifyData()
        {
            InitializeComponent();
        }

        public string name;
        public string price;
        public string type;

        private void VerifyData_Load(object sender, EventArgs e)
        {

        }


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
            print.PrintLabel(sender, e, name,price,type);
        }
        private void names() { type = "name";
            name = textBox2.Text.ToString();
            price=textBox4.Text.ToString();
            pp();
        }
        private void prices() { type = "price";
            name = textBox2.Text.ToString();
            price = textBox4.Text.ToString();
            pp();
        }
        private void nameandprice() { type = "nameandprice";
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



        private void a(string t)
        {
            DataTable dt = new DataTable();
            t = t.ToUpper();
            dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + t + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                InsertRowIntoMainData2(row);
            }
            else
            {
                label3.Text = $"Data Not Found for TagNumber : {t}";
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void InsertRowIntoMainData2(DataRow row)
        {
            try
            {
                DataTable existingData = helper.FetchDataTableFromSYADataBase($"SELECT * FROM MAIN_DATA2 WHERE TAG_NO = '{row["TAG_NO"]}'");
                if (existingData.Rows.Count > 0)
                {
                    label3.Text = $"Data Already Exists for TagNumber : {row["TAG_NO"]}";
                    textBox1.Text = "";
                    textBox1.Focus();
                    return;
                }
                else
                {
                    string insertQuery = $@"
                    INSERT INTO MAIN_DATA2 
                    (ID, CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT, PRINT)
                    VALUES 
                    ({row["ID"]}, '{row["CO_YEAR"]}', '{row["CO_BOOK"]}', '{row["VCH_NO"]}', '{row["VCH_DATE"]}', '{row["TAG_NO"]}', '{row["GW"]}', '{row["NW"]}', '{row["LABOUR_AMT"]}', '{row["WHOLE_LABOUR_AMT"]}', '{row["OTHER_AMT"]}', '{row["IT_TYPE"]}', '{row["ITEM_CODE"]}', '{row["ITEM_PURITY"]}', '{row["ITEM_DESC"]}', '{row["HUID1"]}', '{row["HUID2"]}', '{row["SIZE"]}', '{row["PRICE"]}', '{row["STATUS"]}', '{row["AC_CODE"]}', '{row["AC_NAME"]}', '{row["COMMENT"]}', '{row["PRINT"]}')";
                    helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
                    label3.Text = $"Data Inserted Successfully for TagNumber : {row["TAG_NO"]}";
                    textBox1.Text = "";
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                a(textBox1.Text);
            }
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
            both();
        }
    }
}
