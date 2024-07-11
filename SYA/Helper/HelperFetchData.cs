using System.Data;
using System.Data.SQLite;
using System.Text;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using TextBox = System.Windows.Forms.TextBox;
namespace SYA.Helper
{
    public class HelperFetchData
    {
        public static string connectionToSYADatabase = helper.SYAConnectionString;
        public static NotifyForm notifyForm;
        public static void InsertInStockDataIntoSQLite(string queryToFetchFromMSAccess)
        {
            DataTable data = helper.FetchDataTableFromDataCareDataBase(queryToFetchFromMSAccess);
            List<int> errorRows = new List<int>();
            int insertedCount = 0;
            int updatedCount = 0;
            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                {
                    sqliteConnection.Open();
                    notifyForm = new NotifyForm();
                    notifyForm.Show();
                    for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
                    {
                        DataRow row = data.Rows[rowIndex];
                        try
                        {
                            using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM MAIN_DATA WHERE TAG_NO = @TAG_NO", sqliteConnection))
                            {
                                checkCommand.Parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
                                int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                                if (rowCount > 0)
                                {
                                    using (SQLiteCommand updateCommand = new SQLiteCommand("UPDATE MAIN_DATA SET CO_YEAR = @CO_YEAR, CO_BOOK = @CO_BOOK, VCH_NO = @VCH_NO, VCH_DATE = @VCH_DATE, GW = @GW, NW = @NW,IT_TYPE = @IT_TYPE, ITEM_CODE = @ITEM_CODE, ITEM_PURITY = @ITEM_PURITY, AC_CODE = @AC_CODE, AC_NAME = @AC_NAME, ITEM_DESC = @ITEM_DESC WHERE TAG_NO = @TAG_NO", sqliteConnection))
                                    {
                                        MapInStockParameters(updateCommand.Parameters, row);
                                        updateCommand.ExecuteNonQuery();
                                        updatedCount++;
                                    }
                                }
                                else
                                {
                                    using (SQLiteCommand insertCommand = new SQLiteCommand("INSERT INTO MAIN_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW,HUID1,HUID2, LABOUR_AMT, OTHER_AMT,IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT) VALUES (@CO_YEAR, @CO_BOOK, @VCH_NO, @VCH_DATE, @TAG_NO, @GW, @NW,@HUID1,@HUID2, @LABOUR_AMT, @OTHER_AMT, @IT_TYPE, @ITEM_CODE, @ITEM_PURITY, @ITEM_DESC, @SIZE, @PRICE, @STATUS, @AC_CODE, @AC_NAME, @COMMENT)", sqliteConnection))
                                    {
                                        MapInStockParameters(insertCommand.Parameters, row);
                                        insertCommand.ExecuteNonQuery();
                                        insertedCount++;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errorRows.Add(rowIndex + 1);
                            MessageBox.Show($"InsertInStockDataIntoSQLite Error in row {rowIndex + 1}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            try
                            {
                                using (SQLiteCommand retrieveCommand = new SQLiteCommand("SELECT * FROM MAIN_DATA WHERE TAG_NO = @TAG_NO", sqliteConnection))
                                {
                                    retrieveCommand.Parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
                                    using (SQLiteDataReader reader = retrieveCommand.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            StringBuilder rowData = new StringBuilder();
                                            for (int i = 0; i < reader.FieldCount; i++)
                                            {
                                                rowData.AppendLine($"{reader.GetName(i)}: {reader.GetValue(i)}");
                                            }
                                            MessageBox.Show($"InsertInStockDataIntoSQLite 2 : : : Data in the database for TAG_NO = {row["TAG_NO"]}:\n{rowData.ToString()}", "Database Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                            catch (Exception retrievalEx)
                            {
                                MessageBox.Show($"InsertInStockDataIntoSQLite 3 : : : Error retrieving data from the database for TAG_NO = {row["TAG_NO"]}: {retrievalEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        string str1 = string.Empty, str2 = string.Empty;
                        str1 = "Fetching Data .....";
                        str2 = "Inserted Rows : " + insertedCount + " Updated Rows : " + updatedCount;
                        Application.DoEvents();
                        notifyForm.ShowNotification2(str2);
                        notifyForm.ShowNotification1(str1);
                        Application.DoEvents();
                    }
                    notifyForm.Close();
                }
                if (errorRows.Count > 0)
                {
                    ShowInStockErrorRowsDialog(errorRows, data);
                }
                else
                {
                    MessageBox.Show($"InsertInStockDataIntoSQLite 4 : : : Data fetched from Access and inserted/updated in SQLite successfully.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"InsertInStockDataIntoSQLite 5 : : : Error inserting/updating data into SQLite: {ex.Message}.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void MapInStockParameters(SQLiteParameterCollection parameters, DataRow row)
        {
            parameters.AddWithValue("@CO_YEAR", row["CO_YEAR"]);
            parameters.AddWithValue("@CO_BOOK", row["CO_BOOK"]);
            parameters.AddWithValue("@VCH_NO", "SYA00");
            parameters.AddWithValue("@VCH_DATE", row["VCH_DATE"]);
            parameters.AddWithValue("@TAG_NO", row["TAG_NO"]);
            parameters.AddWithValue("@GW", row["ITM_GWT"]);
            parameters.AddWithValue("@NW", row["ITM_NWT"]);
            parameters.AddWithValue("@LABOUR_AMT", row["LBR_RATE"]);
            parameters.AddWithValue("@OTHER_AMT", row["OTH_AMT"]);
            parameters.AddWithValue("@IT_TYPE", row["IT_TYPE"]);
            parameters.AddWithValue("@ITEM_CODE", row["PR_CODE"]);
            string PR_CODE = row["PR_CODE"].ToString();
            parameters.AddWithValue("@ITEM_PURITY", GetItemPurity(row["IT_CODE"].ToString(), PR_CODE));
            string IT_TYPE = row["IT_TYPE"].ToString(); string itemDesc = GetItemDescFromSQLite(PR_CODE, IT_TYPE);
            parameters.AddWithValue("@ITEM_DESC", itemDesc);
            parameters.AddWithValue("@HUID1", DBNull.Value); parameters.AddWithValue("@HUID2", DBNull.Value); parameters.AddWithValue("@SIZE", row["ITM_SIZE"]);
            parameters.AddWithValue("@PRICE", row["MRP"]);
            parameters.AddWithValue("@STATUS", "INSTOCK"); parameters.AddWithValue("@AC_CODE", DBNull.Value); parameters.AddWithValue("@AC_NAME", DBNull.Value); parameters.AddWithValue("@COMMENT", row["DESIGN"]);
        }
        private static void ShowInStockErrorRowsDialog(List<int> errorRows, DataTable data)
        {
            Form errorForm = new Form();
            errorForm.Text = "Error Rows";
            int rowHeight = 22; int formHeight = Math.Min(errorRows.Count * rowHeight + 100, Screen.PrimaryScreen.WorkingArea.Height);
            errorForm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            errorForm.StartPosition = FormStartPosition.CenterScreen;
            DataGridView errorGridView = new DataGridView();
            errorGridView.Dock = DockStyle.Fill;
            errorGridView.AllowUserToAddRows = false;
            errorGridView.ReadOnly = true;
            errorGridView.Columns.Add("RowNumber", "Row Number");
            errorGridView.Columns.Add("ErrorMessage", "Error Message");
            foreach (string columnName in new[] { "TAG_NO", "VCH_DATE", "ITM_GWT", "ITM_NWT", "LBR_RATE", "OTH_AMT", "PR_CODE", "IT_CODE", "IT_DESC", "ITM_SIZE", "MRP", "DESIGN" })
            {
                errorGridView.Columns.Add(columnName, columnName);
            }
            foreach (int rowNum in errorRows)
            {
                errorGridView.Rows.Add(rowNum, "Error occurred in this row");
                DataRow errorRow = data.Rows[rowNum - 1]; object[] rowData = new object[]
{
                    "","",
            errorRow["TAG_NO"],
            errorRow["VCH_DATE"],
            errorRow["ITM_GWT"],
            errorRow["ITM_NWT"],
            errorRow["LBR_RATE"],
            errorRow["OTH_AMT"],
            errorRow["PR_CODE"],
            errorRow["IT_CODE"],
            errorRow["IT_DESC"],
            errorRow["ITM_SIZE"],
            errorRow["MRP"],
            errorRow["DESIGN"],
};
                errorGridView.Rows.Add(rowData);
            }
            errorForm.Controls.Add(errorGridView);
            int titleBarHeight = errorForm.Height - errorForm.ClientRectangle.Height;
            errorForm.Height = formHeight + titleBarHeight;
            errorForm.ShowDialog();
        }
        public static void UpdateMessageBox(string message, TextBox txtMessageBox)
        {
            if (txtMessageBox.InvokeRequired)
            {
                txtMessageBox.Invoke((MethodInvoker)delegate
                {
                    txtMessageBox.Text = message;
                });
            }
            else
            {
                txtMessageBox.Text = message;
            }
        }
        public static string GetItemPurity(string itCode, string prcode)
        {
            return itCode.Replace(prcode, "");
        }
        public static string GetItemDescFromSQLite(string PR_CODE, string IT_TYPE)
        {
            string query = "SELECT IT_NAME FROM ITEM_MASTER WHERE PR_CODE = '" + PR_CODE + "' AND IT_TYPE = '" + IT_TYPE + "'";
            object result = helper.RunQueryWithoutParametersSYADataBase(query, "ExecuteScalar");
            if (result != null)
            {
                return result.ToString();
            }
            return string.Empty;
        }
    }
}
