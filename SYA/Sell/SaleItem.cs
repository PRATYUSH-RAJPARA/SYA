using SYA.Helper;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Text;
using DataTable = System.Data.DataTable;
namespace SYA
{
    public partial class SaleItem : Form
    {
        DataTable syaTable = new DataTable();
        DataTable ms_maintagdata_dt = new DataTable();
        DataTable sya_maindata_dt = new DataTable();
        DataTable sya_saledata_dt = new DataTable();
        DataTable sya_item_master = new DataTable();
        DataTable sya_sales = new DataTable();
        public SaleItem()
        {
            InitializeComponent();
        }
        private void SaleItem_Load(object sender, EventArgs e)
        {
            loadDataInDataGridView();
            btnSell.Visible = false;
            label2.Text = "";
            dataGridView1.Visible = false;
            textBox1.Text = "SYA916DK00001";
            DisplayResults();
            InitializeTimer();
        }
        // Events
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = "";
            label2.Visible = false;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.Text = textBox1.Text.ToUpper();
                string query = "SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + textBox1.Text + "'";
                LoadData(query);
                btnSell.Focus();
            }
        }
        private void btnSell_Click(object sender, EventArgs e)
        {
            string tagNo_TextBox = textBox1.Text;
            try
            {
                string query = "SELECT CO_YEAR, CO_BOOK, VCH_NO, SR_NO, VCH_DATE, AC_CODE, IT_CODE, IT_DESC, PR_CODE, IT_TYPE, TAG_NO, DESIGN, ITM_PCS, ITM_GWT, ITM_NWT, ITM_RAT, ITM_AMT, LBR_RATE, LBR_AMT, OTH_AMT, NET_AMT FROM MAIN_TAG_DATA WHERE TAG_NO = '" + tagNo_TextBox + "' AND (CO_BOOK = '026' OR CO_BOOK = '26')";
                //  HelperFetchData.InsertSaleDataIntoSQLite(query);
                ms_maintagdata_dt = msaccess(query);
                query = "SELECT * FROM MAIN_DATA WHERE TAG_NO ='" + tagNo_TextBox + "'";
                sya_maindata_dt = sqlite(query);
                query = "SELECT * FROM SALE_DATA WHERE TAG_NO ='" + tagNo_TextBox + "'";
                sya_saledata_dt = sqlite(query);
                query = "SELECT * FROM ITEM_MASTER";
                sya_item_master = sqlite(query);
                showallrows(sya_item_master);
                showallrows(sya_maindata_dt);
                MessageBox.Show(sya_item_master.Rows.Count + "");
                string it_type = sya_maindata_dt.Rows[0]["IT_TYPE"].ToString();
                string vch_no_sya = sya_maindata_dt.Rows[0]["VCH_NO"].ToString();
                if (it_type == "G")
                {
                    if (vch_no_sya == "SYA00")
                    {
                        if (sya_saledata_dt.Rows.Count > 0)
                        {
                            DataTable combine_dt = CombineDataTables(ms_maintagdata_dt, sya_maindata_dt, "", "SYA_");
                            UpdateOrInsertDataIntoSQLite(combine_dt, 0);
                            //Do something here to know  the saledata table that tagno is scanned or something
                            String deleteQuery = "DELETE FROM MAIN_DATA WHERE TAG_NO = '" + tagNo_TextBox + "'";
                            //  helper.RunQueryWithoutParametersSYADataBase(deleteQuery, "ExecuteNonQuery");
                            label2.Text = "";
                        }
                        else if (false)
                        {
                            DialogResult result = MessageBox.Show("Bill for Tag-Number " + tagNo_TextBox + " is not generated, Do you want to sell?", "Confirmation - " + tagNo_TextBox, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                MessageBox.Show("selling sya00 yes");
                            }
                            else
                            {
                                MessageBox.Show("No");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Bill for Tag_Number " + tagNo_TextBox + " is not generated.");
                        }
                    }
                    else if (vch_no_sya == "SYA01")
                    {
                        if (sya_maindata_dt.Rows[0]["HUID1"].ToString().Length > 0 || sya_maindata_dt.Rows[0]["HUID2"].ToString().Length > 0)
                        {
                            DataTable combine_dt = CombineDataTables(ms_maintagdata_dt, sya_maindata_dt, "", "SYA_");
                            UpdateOrInsertDataIntoSQLite(combine_dt, 1);
                            MessageBox.Show("sjbdajhgdjhasgdgjhasdhjasdgasjdasd");
                            string id = getID(sya_item_master, sya_maindata_dt.Rows[0]["IT_TYPE"].ToString(), sya_maindata_dt.Rows[0]["ITEM_CODE"].ToString());
                            string GET = "SELECT COLUMN" + id + " FROM SALES";
                            string SET = "UPDATE SALES SET COLUMN" + id + " = ";
                            String deleteQuery = "DELETE FROM MAIN_DATA WHERE TAG_NO = '" + tagNo_TextBox + "'";
                            // helper.RunQueryWithoutParametersSYADataBase(deleteQuery, "ExecuteNonQuery");
                            ShowMessage("selling sya01 huid");
                        }
                        else
                        {
                            updateSales(sya_item_master, sya_maindata_dt);
                            String deleteQuery = "DELETE FROM MAIN_DATA WHERE TAG_NO = '" + tagNo_TextBox + "'";
                            // helper.RunQueryWithoutParametersSYADataBase(deleteQuery, "ExecuteNonQuery");
                            ShowMessage(tagNo_TextBox + "    : Sold !");
                            //delete and add count
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wrong VCH_NO, Please contact the Developer.");
                    }
                }
                else if (it_type == "S")
                {
                    updateSales(sya_item_master, sya_maindata_dt);
                    String deleteQuery = "DELETE FROM MAIN_DATA WHERE TAG_NO = '" + tagNo_TextBox + "'";
                    // helper.RunQueryWithoutParametersSYADataBase(deleteQuery, "ExecuteNonQuery");
                    loadDataInDataGridView();
                    ShowMessage(tagNo_TextBox + "    : Sold !");
                }
                else
                {
                    MessageBox.Show("Wrong IT_TYPE, Please contact the Developer.");
                }
                btnSell.Visible = false;
                textBox1.Clear();
                textBox1.Focus();
                dataGridView1.Rows.Clear();
                dataGridView1.Visible = false;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                string stackTrace = ex.StackTrace;
                Exception innerException = ex.InnerException;
                string source = ex.Source;
                System.Reflection.MethodBase targetSite = ex.TargetSite;
                string exceptionInfo = $"Exception Message: {errorMessage}\n\n" +
                                       $"Stack Trace: {stackTrace}\n\n" +
                                       $"Inner Exception: {innerException}\n\n" +
                                       $"Source: {source}\n\n" +
                                       $"Target Site: {targetSite}";
                // Display the information in a message box
                MessageBox.Show(exceptionInfo, "Exception Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Load data in datagridview
        private void LoadData(string query)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(helper.SYAConnectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            SQLiteDataReader reader = command.ExecuteReader();
                            syaTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($" Error executing query and filling DataTable: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                using (SQLiteConnection connection = new SQLiteConnection(helper.SYAConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            DataTable transposedTable = TransposeDataReader(reader);
                            LoadDataIntoDataGridView(transposedTable);
                            SetSaleItemProperties(this, transposedTable);
                            dataGridView1.Visible = true;
                            btnSell.Visible = true;
                        }
                        else
                        {
                            textBox1.Focus();
                            textBox1.Clear();
                            MessageBox.Show("No rows found in the result set.", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.Focus();
                textBox1.Clear();
                MessageBox.Show($"Error: {ex.Message} ", "Error");
            }
        }
        private void LoadDataIntoDataGridView(DataTable dataTable)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.RowHeadersVisible = false;
            foreach (DataColumn col in dataTable.Columns)
                dataGridView1.Columns.Add(col.ColumnName, col.ColumnName);
            dataGridView1.Columns[0].Width = 198;
            dataGridView1.Columns[1].Width = 198;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (DataRow row in dataTable.Rows)
            {
                dataGridView1.Rows.Add(row.ItemArray);
            }
        }
        private DataTable TransposeDataReader(SQLiteDataReader reader)
        {
            DataTable transposedTable = new DataTable();
            transposedTable.Columns.Add("Field", typeof(string));
            transposedTable.Columns.Add("Value", typeof(object));
            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    transposedTable.Rows.Add(reader.GetName(i), reader[i]);
            }
            return transposedTable;
        }
        private void SetSaleItemProperties(SaleItem saleItem, DataTable transposedTable)
        {
            foreach (DataRow row in transposedTable.Rows)
            {
                string columnName = row["Field"].ToString();
                var property = typeof(SaleItem).GetProperty(columnName);
                if (property != null)
                {
                    object value = Convert.ChangeType(row["Value"], property.PropertyType);
                    property.SetValue(saleItem, value);
                }
            }
        }
        // Encrypt -- Decrypt
        private void updateSales(DataTable itemMaster, DataTable syaMainData)
        {
            try
            {
                // Get item ID
                string itemId = getID(itemMaster, syaMainData.Rows[0]["IT_TYPE"].ToString(), syaMainData.Rows[0]["ITEM_CODE"].ToString());
                // Build SELECT query
                string firstQuery = $"SELECT COLUMN{itemId},ID FROM SALES WHERE ID = '1'";
                string secondQuery = $"SELECT COLUMN{itemId},ID FROM SALES WHERE ID = '2'";
                string thirdQuery = $"SELECT COLUMN{itemId},ID FROM SALES WHERE ID = '3'";
                // Retrieve data from the database
                DataTable firstDataTable = sqlite(firstQuery);
                DataTable secondDataTable = sqlite(secondQuery);
                DataTable thirdDataTable = sqlite(thirdQuery);
                // Extract encrypted value
                string firstEncryptedValue = firstDataTable.Rows[0][$"COLUMN{itemId}"].ToString();
                string secondEncryptedValue = secondDataTable.Rows[0][$"COLUMN{itemId}"].ToString();
                string thirdEncryptedValue = thirdDataTable.Rows[0][$"COLUMN{itemId}"].ToString();
                // Decrypt the value
                long firstDecrypedValue = decrypt(long.Parse(changeorder(firstEncryptedValue)));
                long secondDecrypedValue = decrypt(long.Parse(changeorder(secondEncryptedValue)));
                long thirdDecrypedValue = decrypt(long.Parse(changeorder(thirdEncryptedValue)));
                decimal itemNetWeightDecimal = decimal.Parse(syaMainData.Rows[0]["NW"].ToString());
                string[] itemNetWeightParts = itemNetWeightDecimal.ToString().Split('.');
                long firstItemNetWeightDecimal = 0;
                long secondItemNetWeightDecimal = 0;
                if (itemNetWeightParts.Length > 1)
                {
                    firstItemNetWeightDecimal = long.Parse(itemNetWeightParts[0]);
                    secondItemNetWeightDecimal = long.Parse(itemNetWeightParts[1]);
                }
                else if (itemNetWeightParts.Length == 1)
                {
                    firstItemNetWeightDecimal = long.Parse(itemNetWeightParts[0]);
                }
                if (secondItemNetWeightDecimal.ToString().Length == 1) { secondItemNetWeightDecimal *= 100; }
                else if (secondItemNetWeightDecimal.ToString().Length == 2) { secondItemNetWeightDecimal *= 10; }
                long firstDecryptedValue1 = firstDecrypedValue + firstItemNetWeightDecimal;
                long secondDecryptedValue1 = secondDecrypedValue + secondItemNetWeightDecimal;
                long thirdDecryptedValue1 = thirdDecrypedValue + 1;
                long add = (int)(secondDecryptedValue1 / 1000);
                firstDecryptedValue1 += add;
                secondDecryptedValue1 = secondDecryptedValue1 % 1000;
                string firstEncryptedValue1 = changeorder(encrypt(firstDecryptedValue1).ToString());
                string secondEncryptedValue1 = changeorder(encrypt(secondDecryptedValue1).ToString());
                string thirdEncryptedValue1 = changeorder(encrypt(thirdDecryptedValue1).ToString());
                // Build UPDATE query
                string firstUpdateQuery = $"UPDATE SALES SET COLUMN{itemId} = '{firstEncryptedValue1}' WHERE ID = '1'";
                string secondUpdateQuery = $"UPDATE SALES SET COLUMN{itemId} = '{secondEncryptedValue1}' WHERE ID = '2'";
                string thirdUpdateQuery = $"UPDATE SALES SET COLUMN{itemId} = '{thirdEncryptedValue1}' WHERE ID = '3'";
                // Execute the update query
                helper.RunQueryWithoutParametersSYADataBase(firstUpdateQuery, "ExecuteNonQuery");
                helper.RunQueryWithoutParametersSYADataBase(secondUpdateQuery, "ExecuteNonQuery");
                helper.RunQueryWithoutParametersSYADataBase(thirdUpdateQuery, "ExecuteNonQuery");
                MessageBox.Show($"Item ID: {itemId}\n" +
                                $"First Query: {firstQuery}\nSecond Query: {secondQuery}\nThird Query: {thirdQuery}\n" +
                                $"First Encrypted Value: {firstEncryptedValue}\nSecond Encrypted Value: {secondEncryptedValue}\nThird Encrypted Value: {thirdEncryptedValue}\n" +
                                $"First Decrypted Value: {firstDecrypedValue}\nSecond Decrypted Value: {secondDecrypedValue}\nThird Decrypted Value: {thirdDecrypedValue}\n" +
                                $"Item Net Weight Decimal: {itemNetWeightDecimal}\n" +
                                $"First Item Net Weight Decimal: {firstItemNetWeightDecimal}\nSecond Item Net Weight Decimal: {secondItemNetWeightDecimal}\n" +
                                $"First Decrypted Value (Updated): {firstDecryptedValue1}\n" +
                                $"Second Decrypted Value (Updated): {secondDecryptedValue1}\n" +
                                $"Third Decrypted Value (Updated): {thirdDecryptedValue1}\n" +
                                $"First Encrypted Value (Updated): {firstEncryptedValue1}\n" +
                                $"Second Encrypted Value (Updated): {secondEncryptedValue1}\n" +
                                $"Third Encrypted Value (Updated): {thirdEncryptedValue1}\n" +
                                $"First Update Query: {firstUpdateQuery}\nSecond Update Query: {secondUpdateQuery}\nThird Update Query: {thirdUpdateQuery}");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                string stackTrace = ex.StackTrace;
                Exception innerException = ex.InnerException;
                string source = ex.Source;
                System.Reflection.MethodBase targetSite = ex.TargetSite;
                string exceptionInfo = $"Exception Message: {errorMessage}\n\n" +
                                       $"Stack Trace: {stackTrace}\n\n" +
                                       $"Inner Exception: {innerException}\n\n" +
                                       $"Source: {source}\n\n" +
                                       $"Target Site: {targetSite}";
                // Display the information in a message box
                MessageBox.Show(exceptionInfo, "Exception Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string changeorder(string str)
        {
            string changedSTR = "";
            for (int i = 0; i < str.Length - 1; i++)
            {
                changedSTR += str[i + 1] + "" + str[i];
                i++;
            }
            if (str.Length % 2 != 0)
            {
                changedSTR += str[str.Length - 1];
            }
            return changedSTR;
        }
        private long encrypt(long inputNumber)
        {
            long squaredValue = (((((((((((inputNumber) - 60) * 2) + 57) * 2) - 69) * 2) - 3896) * 2) + 34098) * 2);
            long encryptedResult = squaredValue * squaredValue;
            return encryptedResult;
        }
        private long decrypt(long n)
        {
            long ans = ((((((((((((long)Math.Sqrt(n) / 2) - 34098) / 2) + 3896) / 2) + 69) / 2) - 57) / 2) + 60));
            return ans;
        }
        private void DisplayResults()
        {
            for (decimal i = 0; i <= 30; i++)
            {
                Random random = new Random();
                // Generate a random number between 1 and 10,000
                int randomNumber = random.Next(1, 10001);
                decimal x = i;
                long a = encrypt((long)x);
                string b = changeorder(a.ToString());
                string c = changeorder(b);
                long d = decrypt(long.Parse(c));
                string debugMessage = $"x: {x} a: {a} b: {b} c: {c} d: {d}";
                label2.Text += "\n" + debugMessage;
                //textBox1.Text += "\n" + debugMessage;
            }
            MessageBox.Show(label2.Text);
        }
        // Helping Functions
        private DataTable msaccess(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (OleDbConnection accessConnection = new OleDbConnection(helper.accessConnectionString))
                {
                    accessConnection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, accessConnection))
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Function : msaccess --- Error fetching data from MS Access: {ex.Message}");
            }
            return dataTable;
        }
        private DataTable sqlite(string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(helper.SYAConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Function sqlite --- Error fetching data from SQLite: {ex.Message}");
            }
            return dataTable;
        }
        private void showallrows(DataTable dataTable)
        {
            // Check if there's at least one row
            if (dataTable.Rows.Count > 0)
            {
                DataRow firstRow = dataTable.Rows[0];
                StringBuilder messageBuilder = new StringBuilder("Data:\n");
                foreach (DataColumn column in dataTable.Columns)
                {
                    messageBuilder.AppendLine($"{column.ColumnName}: {firstRow[column]}");
                }
                // Display the data in a MessageBox
                MessageBox.Show(messageBuilder.ToString(), "Row Data");
            }
            else
            {
                MessageBox.Show("No data found.", "Information");
            }
        }
        public DataTable CombineDataTables(DataTable dt1, DataTable dt2, string tableName1, string tableName2)
        {
            DataTable combinedDataTable = new DataTable();
            try
            {
                // Create a new DataTable to store the combined data
                // Add columns from dt1 to the combinedDataTable with a prefix
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataColumn col in dt1.Columns)
                    {
                        combinedDataTable.Columns.Add(new DataColumn($"{tableName1}{col.ColumnName}", col.DataType));
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    // Add columns from dt2 to the combinedDataTable with a prefix
                    foreach (DataColumn col in dt2.Columns)
                    {
                        combinedDataTable.Columns.Add(new DataColumn($"{tableName2}{col.ColumnName}", col.DataType));
                    }
                }
                // Add a new row to the combinedDataTable
                DataRow newRow = combinedDataTable.NewRow();
                // Populate the new row with values from dt1
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataColumn col in dt1.Columns)
                    {
                        newRow[$"{tableName1}{col.ColumnName}"] = dt1.Rows[0][col.ColumnName];
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    // Populate the new row with values from dt2
                    foreach (DataColumn col in dt2.Columns)
                    {
                        newRow[$"{tableName2}{col.ColumnName}"] = dt2.Rows[0][col.ColumnName];
                    }
                }
                // Add the new row to the combinedDataTable
                combinedDataTable.Rows.Add(newRow);
                //MessageBox.Show("dt1 :: " + dt1.Columns.Count + " dt 2 :: " + dt2.Columns.Count + " combine :: " + combinedDataTable.Columns.Count + " combine");
                //showallrows(combinedDataTable);
                // Now, combinedDataTable contains a single row with columns from both DataTables
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in CombineDataTables --- " + ex.Message);
            }
            return combinedDataTable;
        }
        private void UpdateOrInsertDataIntoSQLite(DataTable dataTable, int n)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(helper.SYAConnectionString))
                {
                    connection.Open();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string tagNo = row["SYA_TAG_NO"].ToString();
                        // Check if the record with the specified TAG_NO exists
                        string checkExistenceQuery = $"SELECT COUNT(*) FROM SALE_DATA WHERE SYA_TAG_NO = '{tagNo}'";
                        using (SQLiteCommand checkExistenceCommand = new SQLiteCommand(checkExistenceQuery, connection))
                        {
                            int count = Convert.ToInt32(checkExistenceCommand.ExecuteScalar());
                            if (count > 0)
                            {
                                // If the record exists, update it
                                string updateQuery = $"UPDATE SALE_DATA SET ";
                                if (n == 0)
                                {
                                    updateQuery += $"CO_YEAR = '{row["CO_YEAR"]}', " +
                                    $"CO_BOOK = '{row["CO_BOOK"]}', " +
                                    $"VCH_NO = '{row["VCH_NO"]}', " +
                                    $"SR_NO = '{row["SR_NO"]}', " +
                                    $"VCH_DATE = '{row["VCH_DATE"]}', " +
                                    $"AC_CODE = '{row["AC_CODE"]}', " +
                                    $"IT_CODE = '{row["IT_CODE"]}', " +
                                    $"IT_DESC = '{row["IT_DESC"]}', " +
                                    $"PR_CODE = '{row["PR_CODE"]}', " +
                                    $"IT_TYPE = '{row["IT_TYPE"]}', " +
                                    $"TAG_NO = '{row["TAG_NO"]}', " +
                                    $"DESIGN = '{row["DESIGN"]}', " +
                                    $"ITM_PCS = '{row["ITM_PCS"]}', " +
                                    $"ITM_GWT = '{row["ITM_GWT"]}', " +
                                    $"ITM_NWT = '{row["ITM_NWT"]}', " +
                                    $"ITM_RAT = '{row["ITM_RAT"]}', " +
                                    $"ITM_AMT = '{row["ITM_AMT"]}', " +
                                    $"LBR_RATE = '{row["LBR_RATE"]}', " +
                                    $"LBR_AMT = '{row["LBR_AMT"]}', " +
                                    $"OTH_AMT = '{row["OTH_AMT"]}', " +
                                    $"NET_AMT = '{row["NET_AMT"]}', ";
                                }
                                updateQuery += $"SYA_ID = {row["SYA_ID"]}, " +
                                    $"SYA_CO_YEAR = '{row["SYA_CO_YEAR"]}', " +
                                    $"SYA_CO_BOOK = '{row["SYA_CO_BOOK"]}', " +
                                    $"SYA_VCH_NO = '{row["SYA_VCH_NO"]}', " +
                                    $"SYA_VCH_DATE = '{row["SYA_VCH_DATE"]}', " +
                                    $"SYA_TAG_NO = '{row["SYA_TAG_NO"]}', " +
                                    $"SYA_GW = '{row["SYA_GW"]}', " +
                                    $"SYA_NW = '{row["SYA_NW"]}', " +
                                    $"SYA_LABOUR_AMT = '{row["SYA_LABOUR_AMT"]}', " +
                                    $"SYA_WHOLE_LABOUR_AMT = '{row["SYA_WHOLE_LABOUR_AMT"]}', " +
                                    $"SYA_OTHER_AMT = '{row["SYA_OTHER_AMT"]}', " +
                                    $"SYA_IT_TYPE = '{row["SYA_IT_TYPE"]}', " +
                                    $"SYA_ITEM_CODE = '{row["SYA_ITEM_CODE"]}', " +
                                    $"SYA_ITEM_PURITY = '{row["SYA_ITEM_PURITY"]}', " +
                                    $"SYA_ITEM_DESC = '{row["SYA_ITEM_DESC"]}', " +
                                    $"SYA_HUID1 = '{row["SYA_HUID1"]}', " +
                                    $"SYA_HUID2 = '{row["SYA_HUID2"]}', " +
                                    $"SYA_SIZE = '{row["SYA_SIZE"]}', " +
                                    $"SYA_PRICE = '{row["SYA_PRICE"]}', " +
                                    $"SYA_STATUS = '{row["SYA_STATUS"]}', " +
                                    $"SYA_AC_CODE = '{row["SYA_AC_CODE"]}', " +
                                    $"SYA_AC_NAME = '{row["SYA_AC_NAME"]}', " +
                                    $"SYA_COMMENT = '{row["SYA_COMMENT"]}', " +
                                    $"SYA_PRINT = '{row["SYA_PRINT"]}' " +
                                    $"WHERE TAG_NO = '{tagNo}'";
                                using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, connection))
                                {
                                    updateCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // If the record doesn't exist, insert it
                                string insertQuery = $"INSERT INTO SALE_DATA (";
                                if (n == 0)
                                {
                                    insertQuery += $"CO_YEAR, CO_BOOK, VCH_NO, SR_NO, VCH_DATE, AC_CODE, IT_CODE, IT_DESC, PR_CODE, IT_TYPE, " +
                                    $"TAG_NO, DESIGN, ITM_PCS, ITM_GWT, ITM_NWT, ITM_RAT, ITM_AMT, LBR_RATE, LBR_AMT, OTH_AMT, NET_AMT, ";
                                }
                                insertQuery += $"SYA_ID, SYA_CO_YEAR, SYA_CO_BOOK, SYA_VCH_NO, SYA_VCH_DATE, SYA_TAG_NO, SYA_GW, SYA_NW, " +
                                $"SYA_LABOUR_AMT, SYA_WHOLE_LABOUR_AMT, SYA_OTHER_AMT, SYA_IT_TYPE, SYA_ITEM_CODE, SYA_ITEM_PURITY, " +
                                $"SYA_ITEM_DESC, SYA_HUID1, SYA_HUID2, SYA_SIZE, SYA_PRICE, SYA_STATUS, SYA_AC_CODE, SYA_AC_NAME, " +
                                $"SYA_COMMENT, SYA_PRINT) VALUES (";
                                if (n == 0)
                                {
                                    insertQuery += $"'{row["CO_YEAR"]}', '{row["CO_BOOK"]}', '{row["VCH_NO"]}', '{row["SR_NO"]}', '{row["VCH_DATE"]}', " +
                                    $"'{row["AC_CODE"]}', '{row["IT_CODE"]}', '{row["IT_DESC"]}', '{row["PR_CODE"]}', '{row["IT_TYPE"]}', " +
                                    $"'{row["TAG_NO"]}', '{row["DESIGN"]}', '{row["ITM_PCS"]}', '{row["ITM_GWT"]}', '{row["ITM_NWT"]}', " +
                                    $"'{row["ITM_RAT"]}', '{row["ITM_AMT"]}', '{row["LBR_RATE"]}', '{row["LBR_AMT"]}', '{row["OTH_AMT"]}', " +
                                    $"'{row["NET_AMT"]}',";
                                }
                                insertQuery += $" {row["SYA_ID"]}, '{row["SYA_CO_YEAR"]}', '{row["SYA_CO_BOOK"]}', '{row["SYA_VCH_NO"]}', " +
                                $"'{row["SYA_VCH_DATE"]}', '{row["SYA_TAG_NO"]}', '{row["SYA_GW"]}', '{row["SYA_NW"]}', '{row["SYA_LABOUR_AMT"]}', " +
                                $"'{row["SYA_WHOLE_LABOUR_AMT"]}', '{row["SYA_OTHER_AMT"]}', '{row["SYA_IT_TYPE"]}', '{row["SYA_ITEM_CODE"]}', " +
                                $"'{row["SYA_ITEM_PURITY"]}', '{row["SYA_ITEM_DESC"]}', '{row["SYA_HUID1"]}', '{row["SYA_HUID2"]}', " +
                                $"'{row["SYA_SIZE"]}', '{row["SYA_PRICE"]}', '{row["SYA_STATUS"]}', '{row["SYA_AC_CODE"]}', '{row["SYA_AC_NAME"]}', " +
                                $"'{row["SYA_COMMENT"]}', '{row["SYA_PRINT"]}')";
                                using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                                {
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Error updating/inserting data into SQLite: {ex.Message}");
            }
        }
        private void InitializeTimer()
        {
            // Create and configure the Timer
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 7000; // Set the interval in milliseconds (e.g., 5000 ms = 5 seconds)
            timer1.Tick += timer1_Tick;
            // Start the timer
            //timer1.Start();
        }
        private void ShowMessage(string message)
        {
            // Show the message in the label
            timer1.Start();
            label2.Text = message;
            label2.Visible = true;
            // Start the timer when showing the message
        }
        private string getITTypeFromID(DataTable itemMasterTable, string id)
        {
            // Assuming you have a DataTable named "ITEM_MASTER"
            // Specify the query condition
            string queryCondition = $"ID = '{id}'";
            // Use the Select method to filter rows based on the query condition
            DataRow[] selectedRows = itemMasterTable.Select(queryCondition);
            // Check if any rows match the condition
            if (selectedRows.Length > 0)
            {
                // Access the ID from the first matching row (assuming ID is unique)
                string it_name = selectedRows[0]["IT_TYPE"].ToString();
                // Use the 'id' as needed
                // MessageBox.Show($"ID for id '{id}': {id}");
                return it_name;
            }
            else
            {
                MessageBox.Show($"No matching row found for id '{id}'.");
                return "";
            }
        }
        private string getITNameFromID(DataTable itemMasterTable, string id)
        {
            // Assuming you have a DataTable named "ITEM_MASTER"
            // Specify the query condition
            string queryCondition = $"ID = '{id}'";
            // Use the Select method to filter rows based on the query condition
            DataRow[] selectedRows = itemMasterTable.Select(queryCondition);
            // Check if any rows match the condition
            if (selectedRows.Length > 0)
            {
                // Access the ID from the first matching row (assuming ID is unique)
                string it_name = selectedRows[0]["IT_NAME"].ToString();
                // Use the 'id' as needed
                // MessageBox.Show($"ID for id '{id}': {id}");
                return it_name;
            }
            else
            {
                MessageBox.Show($"No matching row found for id '{id}'.");
                return "";
            }
        }
        private string getID(DataTable itemMasterTable, string itType, string prCode)
        {
            // Assuming you have a DataTable named "ITEM_MASTER"
            // Specify the query condition
            string queryCondition = $"PR_CODE = '{prCode}' AND IT_TYPE = '{itType}'";
            // Use the Select method to filter rows based on the query condition
            DataRow[] selectedRows = itemMasterTable.Select(queryCondition);
            // Check if any rows match the condition
            if (selectedRows.Length > 0)
            {
                // Access the ID from the first matching row (assuming ID is unique)
                int id = Convert.ToInt32(selectedRows[0]["ID"]);
                // Use the 'id' as needed
                MessageBox.Show($"ID for PR_CODE '{prCode}' and IT_TYPE '{itType}': {id}");
                return id.ToString();
            }
            else
            {
                MessageBox.Show($"No matching row found for PR_CODE '{prCode}' and IT_TYPE '{itType}'.");
                return "";
            }
        }
        private void loadDataInDataGridView()
        {
            try
            {
                string query = "SELECT * FROM SALES ORDER BY ID ASC";
                DataTable sya_sales_dt = sqlite(query);
                query = "SELECT * FROM ITEM_MASTER";
                sya_item_master = sqlite(query);
                // Empty the DataGridView
                dataGridView3.Rows.Clear();
                dataGridView3.Columns.Clear();
                // Add Columns (Assuming 3 columns)
                dataGridView3.Columns.Add("Column1", "Header1");
                dataGridView3.Columns.Add("Column2", "Header2");
                dataGridView3.Columns.Add("Column3", "Header3");
                dataGridView3.Columns.Add("Column4", "Header4");
                // Add Rows from DataTable
                for (int i = 1; i <= 53; i++)
                {
                    string name = getITNameFromID(sya_item_master, i.ToString());
                    string type = getITTypeFromID(sya_item_master, i.ToString());
                    string firstweight = decrypt(long.Parse(changeorder(sya_sales_dt.Rows[0]["COLUMN" + i].ToString()))).ToString();
                    string secondweight = decrypt(long.Parse(changeorder(sya_sales_dt.Rows[1]["COLUMN" + i].ToString()))).ToString();
                    string count = decrypt(long.Parse(changeorder(sya_sales_dt.Rows[2]["COLUMN" + i].ToString()))).ToString();
                    object[] rowData = { name, type, firstweight + "." + secondweight, count };
                    if (firstweight != "0")
                    {
                        dataGridView3.Rows.Add(rowData);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                string stackTrace = ex.StackTrace;
                Exception innerException = ex.InnerException;
                string source = ex.Source;
                System.Reflection.MethodBase targetSite = ex.TargetSite;
                string exceptionInfo = $"Exception Message: {errorMessage}\n\n" +
                                       $"Stack Trace: {stackTrace}\n\n" +
                                       $"Inner Exception: {innerException}\n\n" +
                                       $"Source: {source}\n\n" +
                                       $"Target Site: {targetSite}";
                // Display the information in a message box
                MessageBox.Show(exceptionInfo, "Exception Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
