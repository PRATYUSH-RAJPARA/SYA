using System.Data;
using System.Data.SQLite;
using SYA.Helper;
namespace SYA
{
    public partial class VerifyStock : Form
    {
        public VerifyStock()
        {
            InitializeComponent();
        }
        private void VerifyData_Load(object sender, EventArgs e)
        {
            panel11.Visible = false;
            panel10.Visible = false;
        }
        private void DataVerificationFetchAndInsert(string t)
        {
            DataTable dt = new DataTable();
            t = t.ToUpper();

            dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM " + helper.DataVerificationOldTable + " WHERE TAG_NO = '" + t + "'");
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                setDataToLables(row);
                InsertRowIntoNewTable(row);
            }
            else
            {
                label3.Text = $"Data Not Found for TagNumber : {t}";
                textBox1.Text = "";
                textBox1.Focus();
            }
        }
        private void InsertRowIntoNewTable(DataRow row)
        {
            try
            {
                DataTable existingData = helper.FetchDataTableFromSYADataBase($"SELECT * FROM {helper.DataVerificationNewTable} WHERE TAG_NO = '{row["TAG_NO"]}'");
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
                    INSERT INTO {helper.DataVerificationNewTable} 
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
                DataVerificationFetchAndInsert(textBox1.Text);
            }
        }
        private void setDataToLables(DataRow reader)
        {
            panel11.Visible = true;
            panel10.Visible = true;
            string TagNo = reader["TAG_NO"].ToString();
            string ItemName = "";
            string Caret = reader["ITEM_PURITY"].ToString();
            string Gross = reader["GW"].ToString();
            string Net = reader["NW"].ToString();
            string Labour = reader["LABOUR_AMT"].ToString();
            string WholeLabour = reader["WHOLE_LABOUR_AMT"].ToString();
            string Other = reader["OTHER_AMT"].ToString();
            string HUID1 = reader["HUID1"].ToString();
            string HUID2 = reader["HUID2"].ToString();
            string Size = reader["SIZE"].ToString();
            string Price = reader["PRICE"].ToString();
            string Type = reader["IT_TYPE"].ToString();
            Type = (Type == "G") ? "GOLD" : (Type == "S") ? "SILVER" : Type;
            string query = $"SELECT * FROM ITEM_MASTER WHERE PR_CODE = '{reader["ITEM_CODE"]}' AND IT_TYPE = '{reader["IT_TYPE"]}'";
            string comment = reader["COMMENT"]?.ToString() ?? "";
            using (SQLiteDataReader reader1 = helper.FetchDataFromSYADataBase(query))
            {
                if (reader1 != null && reader1.Read())
                {
                    ItemName = reader1["IT_NAME"].ToString();
                    reader1.Close();
                }
            }
            label25.Text = TagNo;
            label24.Text = ItemName;
            label23.Text = Caret;
            label22.Text = Gross;
            label21.Text = Net;
            label20.Text = Labour;
            label19.Text = WholeLabour;
            label18.Text = Other;
            label17.Text = HUID1;
            label16.Text = HUID2;
            label15.Text = Size;
            label14.Text = Price;
            label29.Text = Type;
            label28.Text = comment;
        }
    }
}
