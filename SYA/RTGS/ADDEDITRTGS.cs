using SYA.Helper;
using System.Data;
using System.Drawing.Printing;
using Form = System.Windows.Forms.Form;
namespace SYA
{
    public partial class ADDEDITRTGS : Form
    {
        private string selectedID;
        List<string> RTGSDATA = new List<string>();
        string main_id = "";
        public ADDEDITRTGS()
        {
            InitializeComponent();
        }
        private void ADDEDITRTGS_Load(object sender, EventArgs e)
        {
        }
        private void SecondForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RTGSList secondForm = (RTGSList)sender;
            FetchAndLoadData(secondForm.GetSelectedID());
        }
        private void button1_Click(object sender, EventArgs e)
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
                main_id = id;
                textBox1.Text = dt.Rows[0][2].ToString();
                textBox2.Text = dt.Rows[0][3].ToString();
                textBox4.Text = dt.Rows[0][4].ToString();
                textBox5.Text = dt.Rows[0][5].ToString();
                textBox6.Text = dt.Rows[0][7].ToString();
                textBox7.Text = dt.Rows[0][8].ToString();
            }
        }
        private void insertquery()
        {
            string branch = null;
            string payableAt = textBox1.Text;
            string bName = textBox2.Text;
            string bAddress = textBox4.Text;
            string bAcNo = textBox5.Text;
            string SA_CA = null;
            if (checkBox3.Checked == true)
            {
                SA_CA = "SA";
            }
            else if (checkBox4.Checked == true)
            {
                SA_CA = "CA";
            }
            string bAcType = SA_CA;
            string bBank = textBox6.Text;
            string bIfs = textBox7.Text;
            if (string.IsNullOrEmpty(main_id))
            {
                // Insert new record if main_id is not set (new entry)
                string insertQuery = $@"
                    INSERT INTO RTGSData (Branch, PayableAt, BName, BAddress, BAcNo, BAcType, BBank, BIfs)
                    VALUES ('{branch}', '{payableAt}', '{bName}', '{bAddress}', '{bAcNo}', '{bAcType}', '{bBank}', '{bIfs}')";
                helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
            }
            else
            {
                // Update existing record if main_id is set
                string updateQuery = $@"
                    UPDATE RTGSData
                    SET Branch = '{branch}', PayableAt = '{payableAt}', BName = '{bName}', BAddress = '{bAddress}', 
                        BAcNo = '{bAcNo}', BAcType = '{bAcType}', BBank = '{bBank}', BIfs = '{bIfs}'
                    WHERE ID = {main_id}";
                helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
            }
            // Nullify main_id after operation
            main_id = null;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            insertquery();
        }
    }
}
