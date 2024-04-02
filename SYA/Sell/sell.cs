using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA.Sell
{
    public partial class sell : Form
    {
        DataTable sellItem = new DataTable();
        public sell()
        {
            InitializeComponent();
        }

        private void sell_Load(object sender, EventArgs e)
        {

        }
        public void getSellItem(string tagNo)
        {
            sellItem.Clear();
            // Try from here if not fount then search in other table and show already sold
            sellItem = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + tagNo + "'");
            sortAndSell(sellItem);
        }
        public void sortAndSell(DataTable itemToSell)
        {
            if (itemToSell.Rows.Count!=0)
            {
                sort();
            }

            void sort()
            {
                if (itemToSell.Rows[0]["IT_TYPE"] == "S")
                {
                    silverItem();
                }
                else if (itemToSell.Rows[0]["IT_TYPE"] == "G" && itemToSell.Rows[0]["TAG_NO"].ToString().Contains("SYA"))
                {
                    if (itemToSell.Rows[0]["HUID1"].ToString().Length == 0 && itemToSell.Rows[0]["HUID2"].ToString().Length == 0)
                    {
                        syaItem();
                    }
                    else if (itemToSell.Rows[0]["HUID1"].ToString().Length == 6 && itemToSell.Rows[0]["HUID2"].ToString().Length == 6)
                    {
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
            void silverItem()
            {
                loadDataInLabels(itemToSell,"sya");
              //  helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
            }
            void dataCareItem()
            {
                MessageBox.Show("as");
                DataTable DT = helper.FetchDataTableFromSYADataBase("SELECT TAG_NO FROM DATACARE_SALE_DATA WHERE TAG_NO = '" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'");
                if (DT.Rows.Count!=0)
                {
                    loadDataInLabels(itemToSell, "datacare");
                   // helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
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
            void syaItem()
            {
                loadDataInLabels(itemToSell,"sya");
               // helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
            }
            void syaHUIDItem()
            {
                loadDataInLabels(itemToSell, "sya");
                string insertQuery = @"INSERT INTO SYA_SALE_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT, PRINT) 
                    VALUES (
                        " + itemToSell.Rows[0]["CO_YEAR"].ToString() + @",
                        " + itemToSell.Rows[0]["CO_BOOK"].ToString() + @",
                        " + itemToSell.Rows[0]["VCH_NO"].ToString() + @",
                        " + itemToSell.Rows[0]["VCH_DATE"].ToString() + @",
                        " + itemToSell.Rows[0]["TAG_NO"].ToString() + @",
                        " + Convert.ToDecimal(itemToSell.Rows[0]["GW"]) + @",
                        " + Convert.ToDecimal(itemToSell.Rows[0]["NW"]) + @",
                        " + Convert.ToDecimal(itemToSell.Rows[0]["LABOUR_AMT"]) + @",
                        " + Convert.ToDecimal(itemToSell.Rows[0]["WHOLE_LABOUR_AMT"]) + @",
                        " + Convert.ToDecimal(itemToSell.Rows[0]["OTHER_AMT"]) + @",
                        " + itemToSell.Rows[0]["IT_TYPE"].ToString() + @",
                        " + itemToSell.Rows[0]["ITEM_CODE"].ToString() + @",
                        " + itemToSell.Rows[0]["ITEM_PURITY"].ToString() + @",
                        " + itemToSell.Rows[0]["ITEM_DESC"].ToString() + @",
                        " + itemToSell.Rows[0]["HUID1"].ToString() + @",
                        " + itemToSell.Rows[0]["HUID2"].ToString() + @",
                        " + itemToSell.Rows[0]["SIZE"].ToString() + @",
                        " + Convert.ToDecimal(itemToSell.Rows[0]["PRICE"]) + @",
                        " + itemToSell.Rows[0]["STATUS"].ToString() + @",
                        " + itemToSell.Rows[0]["AC_CODE"].ToString() + @",
                        " + itemToSell.Rows[0]["AC_NAME"].ToString() + @",
                        " + itemToSell.Rows[0]["COMMENT"].ToString() + @",
                        " + itemToSell.Rows[0]["PRINT"].ToString() + @"
                    )";
                helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
              //  helper.RunQueryWithoutParametersSYADataBase("DELETE FROM MAIN_DATA WHERE TAG_NO ='" + itemToSell.Rows[0]["TAG_NO"].ToString() + "'", "ExecuteNonQuery");
            }

            
        }

        private void txtTAGNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                getSellItem(txtTAGNO.Text);
            }
        }

        private void loadDataInLabels(DataTable itemToSell,string origin)
        {
            itemDetails();
            if (origin == "datacare")
            {
                billDetails();
                accDetails();
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
                billno.Text = (bill.Rows[0]["VCH_NO"] ?? "-").ToString();
                rate.Text = (bill.Rows[0]["ITM_RATE"] ?? "-").ToString();
                netamount.Text = (bill.Rows[0]["ITM_AMT"] ?? "-").ToString();
                lbrrate.Text = (bill.Rows[0]["LBR_RATE"] ?? "-").ToString();
                lbramount.Text = (bill.Rows[0]["LBR_AMT"] ?? "-").ToString();
                otheramount.Text = (bill.Rows[0]["OTH_AMT"] ?? "-").ToString();
                totalamount.Text = (bill.Rows[0]["TOT_AMT"] ?? "-").ToString();
                billdiscount.Text = (bill.Rows[0]["BIL_DISC"] ?? "-").ToString();
                billtax.Text = (bill.Rows[0]["BIL_TAX"] ?? "-").ToString();
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
    }
}
