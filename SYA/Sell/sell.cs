using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace SYA.Sell
{
    public partial class sell : Form
    {
        private SoundPlayer deleteSound;
        DataTable sellItem = new DataTable();
        bool autoSell = true;
        public sell()
        {
            InitializeComponent();
            deleteSound = new SoundPlayer(@"F:\SYA_APP\SYA_SOFT_TEST\config\Audio\delete.wav");
        }
        private void sell_Load(object sender, EventArgs e)
        {
            button1.Text = "Auto Sell : ON";
            button2.Visible = false;
        }
        private void ReplaceNullValuesWithEmptyString(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    // Check if the cell is DBNull or empty string
                    if (row.IsNull(col) || string.IsNullOrEmpty(row[col].ToString()))
                    {
                        // Check the data type of the column
                        if (col.DataType == typeof(string))
                        {
                            row[col] = ""; // Set empty string for string columns
                        }
                        else if (col.DataType == typeof(decimal))
                        {
                            row[col] = 0; // Set 0 for decimal columns
                        }
                        // You can add more checks for other data types as needed
                    }
                }
            }
        }
        public void getSellItem(string tagNo)
        {
            sellItem.Clear();
            // Try from here if not fount then search in other table and show already sold
            sellItem = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + tagNo + "'");
            if (sellItem.Rows.Count>0) {
            ReplaceNullValuesWithEmptyString(sellItem);
            sortAndSell(sellItem, autoSell);
            }
            else
            {
                sellItem = helper.FetchDataTableFromSYADataBase("SELECT * FROM SYA_SALE_DATA WHERE TAG_NO = '" + tagNo + "'");
                if (sellItem.Rows.Count > 0)
                {
                    button2.Visible=false;
                    sortAndSell(sellItem, false);
                    label25.Text="Item "+ sellItem.Rows[0]["TAG_NO"].ToString() + " is already Sold and Saved in Sales.";
                }
                else
                {
                    button2.Visible = false;
                    label25.Text = "Item " + tagNo + " was not found or might have been sold and deleted.";
                }
            }
        }
        public void sortAndSell(DataTable itemToSell, bool sell)
        {
            if (itemToSell.Rows.Count != 0)
            {
                sort();
            }
            void sort()
            {
                MessageBox.Show("s34");
                MessageBox.Show("s3111   :   " + itemToSell.Rows[0]["HUID1"].ToString().Length + "  :  " + itemToSell.Rows[0]["HUID2"].ToString().Length);
                if (itemToSell.Rows[0]["IT_TYPE"].ToString() == "S")
                {
                    silverItem();
                }
                else if (itemToSell.Rows[0]["IT_TYPE"].ToString() == "G" && itemToSell.Rows[0]["TAG_NO"].ToString().Contains("SYA"))
                {
                    MessageBox.Show("s3   :   "+ itemToSell.Rows[0]["HUID1"].ToString().Length+"  :  "+ itemToSell.Rows[0]["HUID2"].ToString().Length);
                    if (itemToSell.Rows[0]["HUID1"].ToString().Length == 0 && itemToSell.Rows[0]["HUID2"].ToString().Length == 0)
                    {
                        MessageBox.Show("s1");
                        syaItem();
                    }
                    else if (itemToSell.Rows[0]["HUID1"].ToString().Length == 6 || itemToSell.Rows[0]["HUID2"].ToString().Length == 6)
                    {
                        MessageBox.Show("s2");
                        syaHUIDItem();
                    }
                }
                else if (itemToSell.Rows[0]["IT_TYPE"].ToString() == "G")
                {
                    dataCareItem();
                }
                else
                {
                    MessageBox.Show(itemToSell.Rows[0]["IT_TYPE"].ToString());
                }
            }
            bool check(string s)
            {
                DataTable dt = new DataTable();
                bool check = false;
                if (s == "delete")
                {
                    dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'");
                    if (dt.Rows.Count == 0)
                    {
                        check = true;
                    }
                }
                else if (s == "insert")
                {
                    dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM SYA_SALE_DATA WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        check = true;
                    }
                }
                return check;
            }
            void silverItem()
            {
                loadDataInLabels(itemToSell, "sya");
                if (sell)
                {
                    helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
                    if (check("delete"))
                    {
                        timer1.Start();
                        label25.Text = "Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " SOLD and DELETED Successfully.";
                    }
                    else
                    {
                        MessageBox.Show("Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " failed to delete. Please contact the developer ( +91 7600771961 ).");
                    }
                }
            }
            void dataCareItem()
            {
                 loadDataInLabels(itemToSell, "datacare");
                if (sell)
                {
                    DataTable DT = helper.FetchDataTableFromSYADataBase("SELECT TAG_NO FROM SYA_SALE_DATA WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'");
                    if (DT.Rows.Count != 0)
                    {
                        helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
                        if (check("delete"))
                        {
                            timer1.Start();
                            label25.Text = "Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " SOLD and DELETED Successfully.";
                        }
                        else
                        {
                            MessageBox.Show("Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " failed to delete. Please contact the developer ( +91 7600771961 ).");
                        }
                    }
                    else
                    {
                        DataTable checkDT = helper.FetchDataTableFromDataCareDataBase("SELECT TAG_NO FROM MAIN_TAG_DATA WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"].ToString() + "' AND CO_BOOK = '026'");
                        if (checkDT.Rows.Count != 0)
                        {
                            DialogResult result = MessageBox.Show("Item bill is created but data is not fetched yet, Do you wanna fetch data right now?", "Confirmation", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                MessageBox.Show("User clicked Yes");
                            }
                            else if (result == DialogResult.No)
                            {
                                MessageBox.Show("User clicked No");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please create a bill for item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " in DataCare software");
                        }
                    }
                }
            }
            void syaItem()
            {
                loadDataInLabels(itemToSell, "sya");
                if (sell)
                {
                    helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
                    if (check("delete"))
                    {
                        timer1.Start();
                        label25.Text = "Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " SOLD and DELETED Successfully.";
                    }
                    else
                    {
                        MessageBox.Show("Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " failed to delete. Please contact the developer ( +91 7600771961 ).");
                    }
                }
            }
            void syaHUIDItem()
            {
                MessageBox.Show("s");
                loadDataInLabels(itemToSell, "sya");
                if (sell)
                {
                    MessageBox.Show(itemToSell.Rows[0]["HUID1"].ToString());
                    string insertQuery =  
                        @"INSERT INTO SYA_SALE_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT, PRINT) 
        VALUES (
            " + (itemToSell.Rows[0]["CO_YEAR"] == DBNull.Value ? "NULL" : itemToSell.Rows[0]["CO_YEAR"].ToString()) + @",
            " + (itemToSell.Rows[0]["CO_BOOK"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["CO_BOOK"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["VCH_NO"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["VCH_NO"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["VCH_DATE"] == DBNull.Value ? "NULL" : "'" + Convert.ToDateTime(itemToSell.Rows[0]["VCH_DATE"]).ToString("yyyy-MM-dd HH:mm:ss") + "'") + @",
            " + (itemToSell.Rows[0]["TAG_NO"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["GW"] == DBNull.Value ? "NULL" : Convert.ToDecimal(itemToSell.Rows[0]["GW"]).ToString()) + @",
            " + (itemToSell.Rows[0]["NW"] == DBNull.Value ? "NULL" : Convert.ToDecimal(itemToSell.Rows[0]["NW"]).ToString()) + @",
            " + (itemToSell.Rows[0]["LABOUR_AMT"] == DBNull.Value ? "NULL" : Convert.ToDecimal(itemToSell.Rows[0]["LABOUR_AMT"]).ToString()) + @",
            " + (itemToSell.Rows[0]["WHOLE_LABOUR_AMT"] == DBNull.Value ? "NULL" : Convert.ToDecimal(itemToSell.Rows[0]["WHOLE_LABOUR_AMT"]).ToString()) + @",
            " + (itemToSell.Rows[0]["OTHER_AMT"] == DBNull.Value ? "NULL" : Convert.ToDecimal(itemToSell.Rows[0]["OTHER_AMT"]).ToString()) + @",
            " + (itemToSell.Rows[0]["IT_TYPE"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["IT_TYPE"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["ITEM_CODE"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["ITEM_CODE"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["ITEM_PURITY"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["ITEM_PURITY"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["ITEM_DESC"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["ITEM_DESC"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["HUID1"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["HUID1"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["HUID2"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["HUID2"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["SIZE"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["SIZE"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["PRICE"] == DBNull.Value ? "NULL" : Convert.ToDecimal(itemToSell.Rows[0]["PRICE"]).ToString()) + @",
            " + (itemToSell.Rows[0]["STATUS"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["STATUS"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["AC_CODE"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["AC_CODE"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["AC_NAME"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["AC_NAME"].ToString() + "'") + @",
            " + (itemToSell.Rows[0]["COMMENT"] == DBNull.Value ? "NULL" : "'" + itemToSell.Rows[0]["COMMENT"].ToString() + "'") + @",
            " + itemToSell.Rows[0]["PRINT"].ToString() + @"
        )";
                    helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
                    if (check("insert"))
                    {
                        helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
                        if (check("delete"))
                        {
                            timer1.Start();
                            label25.Text = "Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " SOLD and SAVED Successfully.";
                        }
                        else
                        {
                            MessageBox.Show("Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " inserted but failed to delete. Please contact the developer ( +91 7600771961 ).");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item : " + itemToSell.Rows[0]["TAG_NO"].ToString() + " failed to insert. Please contact the developer ( +91 7600771961 ).");
                    }
                }
            }
        }
        private void txtTAGNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtTAGNO.Text = txtTAGNO.Text.ToUpper();
                if (!autoSell)
                {
                    button2.Visible = true;
                    button2.Focus();
                }
                getSellItem(txtTAGNO.Text);
                txtTAGNO.Text = string.Empty;
            }
        }
        private void EmptyLabels()
        {
            itemDetails();
            billDetails();
            accDetails();
            void itemDetails()
            {
                tagno.Text = string.Empty;
                gw.Text = string.Empty;
                nw.Text = string.Empty;
                itemtype.Text = string.Empty;
                purity.Text = string.Empty;
                huid1.Text = string.Empty;
                huid2.Text = string.Empty;
                size.Text = string.Empty;
                comment.Text = string.Empty;
            }
            void billDetails()
            {
                billno.Text = string.Empty;
                rate.Text = string.Empty;
                netamount.Text = string.Empty;
                lbrrate.Text = string.Empty;
                lbramount.Text = string.Empty;
                otheramount.Text = string.Empty;
                totalamount.Text = string.Empty;
                billdiscount.Text = string.Empty;
                billtax.Text = string.Empty;
            }
            void accDetails()
            {
                name.Text = string.Empty;
                mobile1.Text = string.Empty;
                mobile2.Text = string.Empty;
                mobile3.Text = string.Empty;
            }
        }
        private void loadDataInLabels(DataTable itemToSell, string origin)
        {
            itemDetails();
            if (origin == "datacare")
            {
                billDetails();
            }
            void itemDetails()
            {
                tagno.Text = (itemToSell.Rows[0]["TAG_NO"] ?? "-").ToString();
                gw.Text = (itemToSell.Rows[0]["GW"] ?? "-").ToString();
                nw.Text = (itemToSell.Rows[0]["NW"] ?? "-").ToString();
                itemtype.Text = itemToSell.Rows[0]["IT_TYPE"].ToString();
                purity.Text = (itemToSell.Rows[0]["ITEM_PURITY"] ?? "-").ToString();
                huid1.Text = (itemToSell.Rows[0]["HUID1"] ?? "-").ToString();
                huid2.Text = (itemToSell.Rows[0]["HUID2"] ?? "-").ToString();
                size.Text = (itemToSell.Rows[0]["SIZE"] ?? "-").ToString();
                comment.Text = (itemToSell.Rows[0]["ITEM_DESC"] ?? "-").ToString() + "\n" + (itemToSell.Rows[0]["COMMENT"] ?? "").ToString();
            }
            DataTable bill = new DataTable();
            void billDetails()
            {
                bill = helper.FetchDataTableFromDataCareDataBase("SELECT * FROM SL_DETL WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"] + "'");
                if (bill.Rows.Count > 0)
                {
                    billno.Text = (bill.Rows[0]["VCH_NO"] ?? "-").ToString();
                    rate.Text = (bill.Rows[0]["ITM_RATE"] ?? "-").ToString();
                    netamount.Text = (bill.Rows[0]["ITM_AMT"] ?? "-").ToString();
                    lbrrate.Text = (bill.Rows[0]["LBR_RATE"] ?? "-").ToString();
                    lbramount.Text = (bill.Rows[0]["LBR_AMT"] ?? "-").ToString();
                    otheramount.Text = (bill.Rows[0]["OTH_AMT"] ?? "-").ToString();
                    totalamount.Text = (bill.Rows[0]["TOT_AMT"] ?? "-").ToString();
                    billdiscount.Text = (bill.Rows[0]["BIL_DISC"] ?? "-").ToString();
                    billtax.Text = (bill.Rows[0]["BIL_TAX"] ?? "-").ToString();
                    accDetails();
                }
                else
                {
                    bill = helper.FetchDataTableFromSYADataBase("SELECT * FROM SYA_SALE_DATA WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"] + "'");
                    billno.Text = (bill.Rows[0]["VCH_NO"] ?? "-").ToString();
                    name.Text = (bill.Rows[0]["AC_NAME"] ?? "-").ToString();
                }
                }
                void accDetails()
            {
                DataTable acc = new DataTable();
                acc = helper.FetchDataTableFromDataCareDataBase("SELECT AC_NAME,AC_PHONE,AC_MOBILE,AC_MOBILE2 FROM AC_MAST WHERE AC_CODE = '" + bill.Rows[0]["AC_CODE"] + "'");
                name.Text = (acc.Rows[0]["AC_NAME"] ?? "-").ToString();
                mobile1.Text = (acc.Rows[0]["AC_PHONE"] ?? "-").ToString();
                mobile2.Text = (acc.Rows[0]["AC_MOBILE"] ?? "-").ToString();
                mobile3.Text = (acc.Rows[0]["AC_MOBILE2"] ?? "-").ToString();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            EmptyLabels();
            button2.Visible = false;
            txtTAGNO.Text = string.Empty;
            label25.Text = string.Empty;
            timer1.Stop();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (autoSell)
            {
                autoSell = false;
                button1.Text = "Auto Sell : OFF";
            }
            else
            {
                autoSell = true;
                button1.Text = "Auto Sell : ON";
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            sortAndSell(sellItem, true);
        }
    }
}
