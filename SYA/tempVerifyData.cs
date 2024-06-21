using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Media;
using DataTable = System.Data.DataTable;
namespace SYA
{
    public partial class tempVerifyData : Form
    {
        decimal grosswt = 0;
        decimal netwt = 0;
        decimal count = 0;
        public tempVerifyData()
        {
            InitializeComponent();
            panel6.Visible = false;
            textBox1.Focus();
            button1.Text = "Normal Insert";
        }
        bool force = false;
        private void dataGridViewSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                panel6.Visible = true;
                label30.Text = "Loading.............";
                label30.BackColor = Color.White;
                try
                {
                    string tagno = textBox1.Text;
                    // Check if the input is not null or empty
                    if (!string.IsNullOrEmpty(tagno))
                    {
                        string str = "INSERT INTO TAGNOS (TAG_NO) VALUES ('" + tagno + "'); ";
                        helper.RunQueryWithoutParametersSYADataBase(str, "ExecuteNonQuery");
                        string query = $"SELECT * FROM MAIN_DATA WHERE TAG_NO LIKE '%{tagno}%'";
                        using (SQLiteDataReader reader = helper. FetchDataFromSYADataBase(query))
                        {
                            if (reader != null && reader.Read())
                            {
                                setDataToLables(reader);
                            }
                            else
                            {
                                label30.BackColor = Color.Orange;
                                label30.Text = "No data found.";
                            }
                        }
                        textBox1.Text = null;
                    }
                    else
                    {
                        label30.BackColor = Color.Orange;
                        label30.Text = "Tag number is null or empty.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error2: {ex.Message}\nStackTrace: {ex.StackTrace}");
                }
                finally
                {
                    loaddata();
                }
            }
        }
        private void setDataToLables(SQLiteDataReader reader)
        {
            bool allOK = true;
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
            string coYear = reader["CO_YEAR"].ToString();
            string coBook = reader["CO_BOOK"].ToString();
            string vchNo = reader["VCH_NO"].ToString();
            string vchDate = reader["VCH_DATE"].ToString();
            string tagNo = TagNo; // Replace with the actual variable value
            string gross = Gross; // Replace with the actual variable value
            string net = Net; // Replace with the actual variable value
            string labour = Labour; // Replace with the actual variable value
            string wholeLabour = WholeLabour; // Replace with the actual variable value
            string other = Other; // Replace with the actual variable value
            string itType = reader["IT_TYPE"].ToString();
            string itemCode = reader["ITEM_CODE"].ToString();
            string itemPurity = reader["ITEM_PURITY"].ToString();
            string itemDesc = label24.Text; // Replace with the actual variable value
            string huid1 = HUID1; // Replace with the actual variable value
            string huid2 = HUID2; // Replace with the actual variable value
            string size = Size; // Replace with the actual variable value
            string price = Price; // Replace with the actual variable value
            string status = reader["STATUS"].ToString();
            string acCode = reader["AC_CODE"].ToString();
            string acName = reader["AC_NAME"].ToString();
            string comment = reader["COMMENT"]?.ToString() ?? "";
            string print = reader["PRINT"].ToString();
            reader.Close();
            using (SQLiteDataReader reader1 = helper.FetchDataFromSYADataBase(query))
            {
                if (reader1 != null && reader1.Read())
                {
                    ItemName = reader1["IT_NAME"].ToString();
                    reader1.Close();
                }
            }
            label16.BackColor = Color.White;
            label17.BackColor = Color.White;
            label18.BackColor = Color.White;
            label19.BackColor = Color.White;
            label20.BackColor = Color.White;
            label21.BackColor = Color.White;
            label22.BackColor = Color.White;
            label23.BackColor = Color.White;
            string checkQuery = "SELECT count(TAG_NO) FROM MAIN_DATA_VERIFIED WHERE TAG_NO = '" + tagNo + "'";
            int rowCount = Convert.ToInt32(helper.RunQueryWithoutParametersSYADataBase(checkQuery, "ExecuteScalar"));
            MessageBox.Show(rowCount+"");
            if (Type == "GOLD" && rowCount <= 0)
            {
                if (!ValidateCaret(Caret)) { label23.BackColor = Color.Crimson; allOK = false; }
                else if (!ValidateGrossAndNet(Gross, Net)) { label22.BackColor = Color.Crimson; label21.BackColor = Color.Crimson; allOK = false; }
                else if (!ValidateLabourAndOther(Labour, WholeLabour, Other)) { label20.BackColor = Color.Crimson; label19.BackColor = Color.Crimson; label18.BackColor = Color.Crimson; allOK = false; }
                else if (!ValidateHUIDs(HUID1, HUID2)) { label16.BackColor = Color.Crimson; label17.BackColor = Color.Crimson; allOK = false; }
                else if (!AdditionalLabourAndWholeLabourValidations(Labour, WholeLabour, Net)) { label20.BackColor = Color.Crimson; label19.BackColor = Color.Crimson; allOK = false; }
                // Assuming 'reader' is an instance of SQLiteDataReader
            }
            else
            {
                label30.BackColor = Color.Orange;
                label30.Text = "Data Already there for Tagnumber :::   " + tagNo + "   :::";
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
            if (allOK || force)
            {
                force = false;
                button1.Text = "Normal Insert";
                textBox1.Focus();
                if (Type == "GOLD")
                {
                    if (rowCount <= 0)
                    {
                        InsertDataIntoMainDataVerified(coYear, coBook, vchNo, vchDate, tagNo, gross, net, labour, wholeLabour, other, itType, itemCode, itemPurity, itemDesc, huid1, huid2, size, price, status, acCode, acName, comment, print);
                        label30.BackColor = Color.LightGreen;
                        grosswt += decimal.Parse(gross);
                        netwt += decimal.Parse(net);
                        count++;
                        label35.Text = "Count : " + count;
                        label30.Text = "Data Inserted for Tagnumber :::   " + tagNo + "   :::";
                        textBox1.Focus();
                        SystemSounds.Asterisk.Play();
                        SystemSounds.Beep.Play();
                        SystemSounds.Exclamation.Play();
                        SystemSounds.Hand.Play();
                        SystemSounds.Question.Play();
                    }
                    else
                    {
                        label30.BackColor = Color.Orange;
                        label30.Text = "Data Already there for Tagnumber :::   " + tagNo + "   :::";
                    }
                }
                else
                {
                    label30.BackColor = Color.Orange;
                    label30.Text = "Silver Data ...";
                }
            }
        }
        private void InsertDataIntoMainDataVerified(string coYear, string coBook, string vchNo, string vchDate, string tagNo,
    string gross, string net, string labour, string wholeLabour, string other, string itType, string itemCode,
    string itemPurity, string itemDesc, string huid1, string huid2, string size, string price, string status,
    string acCode, string acName, string comment, string print)
        {
            try
            {
                string insertQuery = $@"
            INSERT INTO MAIN_DATA_VERIFIED (
                CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, 
                IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, 
                COMMENT, PRINT, item
            ) VALUES (
                '{coYear}', '{coBook}', '{vchNo}', '{vchDate}', '{tagNo}', '{gross}', '{net}', '{labour}', 
                '{wholeLabour}', '{other}', '{itType}', '{itemCode}', '{itemPurity}', '{itemDesc}', '{huid1}', 
                '{huid2}', '{size}', '{price}', '{status}', '{acCode}', '{acName}', '{comment}', '{print}','{textBox2.Text}'
            );";
                helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error1: {ex.Message}");
            }
        }
        private void ClearValidationMessage()
        {
            label30.Text = string.Empty;
        }
        private void AppendValidationMessage(string message)
        {
            label30.Text += message + Environment.NewLine;
        }
        private bool ValidateCaret(string input)
        {
            ClearValidationMessage();
            // Convert the input to uppercase for case-insensitive comparison
            string upperInput = input.ToUpper();
            // Valid caret values
            string[] validCarets = { "18K", "916", "20K", "KDM", "SLO", "925" };
            // Check if the input is in the list of valid carets
            if (validCarets.Contains(upperInput))
            {
                // Input is valid
                return true;
            }
            else
            {
                // Input is not valid
                AppendValidationMessage("Invalid caret value.");
                button1.Focus();
                return false;
            }
        }
        private bool ValidateGrossAndNet(string grossStr, string netStr)
        {
            // Try to parse strings to decimals
            if (decimal.TryParse(grossStr, out decimal gross) && decimal.TryParse(netStr, out decimal net))
            {
                // Check if gross and net have 3 decimal places
                if (IsValidDecimal(gross) && IsValidDecimal(net))
                {
                    // Check if gross is greater than or equal to net
                    if (gross >= net)
                    {
                        // Check if both gross and net are greater than 0
                        if (gross > 0 && net > 0)
                        {
                            // Validation successful
                            return true;
                        }
                        else
                        {
                            // Either gross or net is not greater than 0
                            AppendValidationMessage("Gross and net must be greater than 0.");
                            button1.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        // Gross is less than net
                        AppendValidationMessage("Gross must be greater than or equal to net.");
                        button1.Focus();
                        return false;
                    }
                }
                else
                {
                    // Gross or net doesn't have 3 decimal places
                    AppendValidationMessage("Gross and net must have 3 decimal places.");
                    button1.Focus();
                    return false;
                }
            }
            else
            {
                // Parsing failed, either grossStr or netStr is not a valid decimal string
                AppendValidationMessage("Invalid decimal value for gross or net.");
                button1.Focus();
                return false;
            }
        }
        private bool ValidateLabourAndOther(string labourStr, string wholeLabourStr, string otherStr)
        {
            // Try to parse strings to decimals
            if (decimal.TryParse(labourStr, out decimal labour) &&
                decimal.TryParse(wholeLabourStr, out decimal wholeLabour) &&
                decimal.TryParse(otherStr, out decimal other))
            {
                // If null, make it 0
                labour = labourStr == null ? 0 : labour;
                wholeLabour = wholeLabourStr == null ? 0 : wholeLabour;
                // Check if all values are greater than or equal to 0
                if (labour >= 0 && wholeLabour >= 0 && other >= 0)
                {
                    // Check conditions related to labour and whole labour
                    if ((labour == 0 && wholeLabour > 0) ||
                        (wholeLabour == 0 && labour > 0) ||
                        (labour > 0 && wholeLabour == 0))
                    {
                        // Validation successful
                        return true;
                    }
                    else
                    {
                        // Invalid conditions for labour and whole labour
                        AppendValidationMessage("Invalid conditions for labour and whole labour.");
                        button1.Focus();
                        return false;
                    }
                }
                else
                {
                    // Any value is less than 0
                    AppendValidationMessage("Values must be greater than or equal to 0.");
                    button1.Focus();
                    return false;
                }
            }
            else
            {
                // Parsing failed, either labourStr, wholeLabourStr, or otherStr is not a valid decimal string
                AppendValidationMessage("Invalid decimal value for labour, whole labour, or other.");
                button1.Focus();
                return false;
            }
        }
        private bool AdditionalLabourAndWholeLabourValidations(string labourStr, string wholeLabourStr, string netStr)
        {
            // Parse strings to decimals
            if (decimal.TryParse(labourStr, out decimal labour) &&
                decimal.TryParse(wholeLabourStr, out decimal wholeLabour) &&
                decimal.TryParse(netStr, out decimal net))
            {
                // Additional conditions for labor and whole labor
                if (labour > 0 && net * labour < 2000)
                {
                    AppendValidationMessage("Error: Net weight * labor should be greater than or equal to 2000 when labor is not equal to 0.");
                    button1.Focus();
                    return false;
                }
                if (wholeLabour > 0)
                {
                    if (wholeLabour < net * 650)
                    {
                        AppendValidationMessage("Error: Whole labour is less then net*650.");
                        button1.Focus();
                        return false;
                    }
                    if (net >= 2 && net <= 3)
                    {
                        if (wholeLabour != 2000)
                        {
                            AppendValidationMessage("Error: Whole labor should be equal to 2000 when net weight is between 2 and 3.");
                            button1.Focus();
                            return false;
                        }
                    }
                    else if (net >= 1 && net < 2)
                    {
                        if (wholeLabour != 1500)
                        {
                            AppendValidationMessage("Error: Whole labor should be equal to 1500 when net weight is between 1 and 2.");
                            button1.Focus();
                            return false;
                        }
                    }
                    else if ((double)net >= 0.4 && net < 1)
                    {
                        if (wholeLabour != 1100)
                        {
                            AppendValidationMessage("Error: Whole labor should be equal to 1100 when net weight is between 0.4 and 1.");
                            button1.Focus();
                            return false;
                        }
                    }
                    else if (net >= 0 && (double)net < 0.4)
                    {
                        if (wholeLabour != 950)
                        {
                            AppendValidationMessage("Error: Whole labor should be equal to 950 when net weight is between 0 and 0.4.");
                            button1.Focus();
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                // Parsing failed, either labourStr, wholeLabourStr, or netStr is not a valid decimal string
                AppendValidationMessage("Invalid decimal value for labour, whole labour, or net weight.");
                button1.Focus();
                return false;
            }
        }
        private bool ValidateHUIDs(string huid1, string huid2)
        {
            // Check HUID1 and HUID2 length conditions
            if ((huid1 == null || (huid1.Length == 0 || huid1.Length == 6)) &&
                (huid2 == null || (huid2.Length == 0 || huid2.Length == 6 && huid1 != null)))
            {
                // Validation successful
                return true;
            }
            else
            {
                // Validation failed
                AppendValidationMessage("Invalid length conditions for HUIDs.");
                button1.Focus();
                return false;
            }
        }
        // Helper method to check if a decimal has 3 decimal places
        private bool IsValidDecimal(decimal value)
        {
            try
            {
                // Format the decimal part to always have three decimal places
                string decimalPart = (value - Math.Floor(value)).ToString("F3");
                // Check if the length of the formatted decimal part is 5 (including the dot)
                return decimalPart.Length == 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error2: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
            return false;
        }
        private void label13_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (force)
            {
                button1.Text = "Normal Insert";
                force = false;
                textBox1.Focus();
            }
            else
            {
                button1.Text = "Force Insert";
                force = true;
                textBox1.Focus();
            }
        }
        private void tempVerifyData_Load(object sender, EventArgs e)
        {
        }
        private void loaddata()
        {
            string query = "SELECT item, ROUND(SUM(GW), 3) AS TOTAL_GW, ROUND(SUM(NW), 3) AS TOTAL_NW, COUNT(item) AS ITEM_COUNT FROM MAIN_DATA_VERIFIED GROUP BY item UNION ALL SELECT 'GRAND TOTAL' AS item, ROUND(SUM(GW), 3) AS TOTAL_GW, ROUND(SUM(NW), 3) AS TOTAL_NW, COUNT(item) AS ITEM_COUNT FROM MAIN_DATA_VERIFIED;";
            DataTable datatable = helper.FetchDataTableFromSYADataBase(query);
            dataGridView1.DataSource = datatable;
            string query1 = "SELECT TAG_NO, COUNT(TAG_NO) AS Count FROM TAGNOS GROUP BY TAG_NO HAVING COUNT(TAG_NO) > 1";
            DataTable datatable1 = helper.FetchDataTableFromSYADataBase(query1);
            dataGridView2.DataSource = datatable1;
        }
    }
}
