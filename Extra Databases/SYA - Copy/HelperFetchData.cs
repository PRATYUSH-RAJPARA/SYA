using System.Data;
using System.Data.SQLite;
using System.Text;

namespace SYA
{
    public class HelperFetchData
    {
       


        public static  string connectionToSYADatabase = helper.SYAConnectionString;

        public static void InsertInStockDataIntoSQLite( TextBox txtMessageBox,string queryToFetchFromMSAccess)
        {
            DataTable data = FetchDataFromMSAccessAndInsertIntoSQLite(queryToFetchFromMSAccess);
            List<int> errorRows = new List<int>();
            int insertedCount = 0;
            int updatedCount = 0;

            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                {
                    sqliteConnection.Open();

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


                        HelperFetchData.UpdateMessageBox("Inserted Rows : " + insertedCount + " Updated Rows : " + updatedCount, txtMessageBox);
                        Application.DoEvents();

                    }
                }

                if (errorRows.Count > 0)
                {
                    ShowInStockErrorRowsDialog(errorRows, data);
                }
                else
                {
                    MessageBox.Show($"InsertInStockDataIntoSQLite 4 : : : Data fetched from Access and inserted/updated in SQLite successfully.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // UpdateMessageBox("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"InsertInStockDataIntoSQLite 5 : : : Error inserting/updating data into SQLite: {ex.Message}.\nInserted Rows: {insertedCount}\nUpdated Rows: {updatedCount}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static DataTable FetchDataFromMSAccessAndInsertIntoSQLite(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                // Query to select data from your Access table
               
                DataTable accessData = helper.FetchFromDataCareDataBase(query);
                //InsertInStockDataIntoSQLite(accessData, txtMessageBox);
                // Perform insertion directly without using background worker or progress bar
                return accessData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"FetchDataFromMSAccessAndInsertIntoSQLite Error inserting into SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return dt;
            }
        }
        public static void InsertSaleDataIntoSQLite( string queryToFetchFromMSAccess) 
        {
            DataTable accessData = FetchDataFromMSAccessAndInsertIntoSQLite(queryToFetchFromMSAccess);
            if (accessData != null && accessData.Rows.Count > 0)
            {
                // Loop through each row
                foreach (DataRow row in accessData.Rows)
                {
                    // Store values in variables
                    string coYear = row["CO_YEAR"].ToString();
                    string coBook = row["CO_BOOK"].ToString();
                    string vchNo = row["VCH_NO"].ToString();
                    string srNo = row["SR_NO"].ToString();
                    string vchDate = row["VCH_DATE"].ToString();
                    string acCode = row["AC_CODE"].ToString();
                    string itCode = row["IT_CODE"].ToString();
                    string itDesc = row["IT_DESC"].ToString();
                    string prCode = row["PR_CODE"].ToString();
                    string itType = row["IT_TYPE"].ToString();
                    string tagNo = row["TAG_NO"].ToString();
                    string design = row["DESIGN"].ToString();
                    string itmPcs = row["ITM_PCS"].ToString();
                    string itmGwt = row["ITM_GWT"].ToString();
                    string itmNwt = row["ITM_NWT"].ToString();
                    string itmRat = row["ITM_RAT"].ToString();
                    string itmAmt = row["ITM_AMT"].ToString();
                    string lbrRate = row["LBR_RATE"].ToString();
                    string lbrAmt = row["LBR_AMT"].ToString();
                    string othAmt = row["OTH_AMT"].ToString();
                    string netAmt = row["NET_AMT"].ToString();
                    DataTable syadt = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + tagNo + "'");
                    foreach(DataRow syaRow in syadt.Rows)
                    {
                        int syaId = Convert.ToInt32(syaRow["ID"]);
                        string syaCOYear = syaRow["CO_YEAR"].ToString();
                        string syaCOBook = syaRow["CO_BOOK"].ToString();
                        string syaVchNo = syaRow["VCH_NO"].ToString();
                        string syaVchDate = syaRow["VCH_DATE"].ToString();
                        string syaTagNo = syaRow["TAG_NO"].ToString();
                        string syaGW = syaRow["GW"].ToString(); // Convert to string
                        string syaNW = syaRow["NW"].ToString(); // Convert to string
                        string syaLabourAmt = syaRow["LABOUR_AMT"].ToString(); // Convert to string
                        string syaWholeLabourAmt = syaRow["WHOLE_LABOUR_AMT"].ToString(); // Convert to string
                        string syaOtherAmt = syaRow["OTHER_AMT"].ToString(); // Convert to string
                        string syaItType = syaRow["IT_TYPE"].ToString();
                        string syaItemCode = syaRow["ITEM_CODE"].ToString();
                        string syaItemPurity = syaRow["ITEM_PURITY"].ToString();
                        string syaItemDesc = syaRow["ITEM_DESC"].ToString();
                        string syaHUID1 = syaRow["HUID1"].ToString();
                        string syaHUID2 = syaRow["HUID2"].ToString();
                        string syaSize = syaRow["SIZE"].ToString();
                        string syaPrice = syaRow["PRICE"].ToString(); // Convert to string
                        string syaStatus = syaRow["STATUS"].ToString();
                        string syaACCode = syaRow["AC_CODE"].ToString();
                        string syaACName = syaRow["AC_NAME"].ToString();
                        string syaComment = syaRow["COMMENT"].ToString();
                        string syaPrint = syaRow["PRINT"].ToString(); // Convert to string
                        string insertupdatequery = null;
                        using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                        {
                            sqliteConnection.Open();

                            using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM SALE_DATA WHERE TAG_NO = @TAG_NO", sqliteConnection))
                            {
                                checkCommand.Parameters.AddWithValue("@TAG_NO", tagNo);
                                int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                                if (rowCount > 0)
                                {
                                    insertupdatequery = $@"UPDATE SALE_DATA SET
                                CO_YEAR = '{coYear}',
                                CO_BOOK = '{coBook}',
                                VCH_NO = '{vchNo}',
                                SR_NO = '{srNo}',
                                VCH_DATE = '{vchDate}',
                                AC_CODE = '{acCode}',
                                IT_CODE = '{itCode}',
                                IT_DESC = '{itDesc}',
                                PR_CODE = '{prCode}',
                                IT_TYPE = '{itType}',
                                DESIGN = '{design}',
                                ITM_PCS = '{itmPcs}',
                                ITM_GWT = '{itmGwt}',
                                ITM_NWT = '{itmNwt}',
                                ITM_RAT = '{itmRat}',
                                ITM_AMT = '{itmAmt}',
                                LBR_RATE = '{lbrRate}',
                                LBR_AMT = '{lbrAmt}',
                                OTH_AMT = '{othAmt}',
                                NET_AMT = '{netAmt}',
                                SYA_ID = {syaId},
                                SYA_CO_YEAR = '{syaCOYear}',
                                SYA_CO_BOOK = '{syaCOBook}',
                                SYA_VCH_NO = '{syaVchNo}',
                                SYA_VCH_DATE = '{syaVchDate}',
                                SYA_TAG_NO = '{syaTagNo}',
                                SYA_GW = '{syaGW}',
                                SYA_NW = '{syaNW}',
                                SYA_LABOUR_AMT = '{syaLabourAmt}',
                                SYA_WHOLE_LABOUR_AMT = '{syaWholeLabourAmt}',
                                SYA_OTHER_AMT = '{syaOtherAmt}',
                                SYA_IT_TYPE = '{syaItType}',
                                SYA_ITEM_CODE = '{syaItemCode}',
                                SYA_ITEM_PURITY = '{syaItemPurity}',
                                SYA_ITEM_DESC = '{syaItemDesc}',
                                SYA_HUID1 = '{syaHUID1}',
                                SYA_HUID2 = '{syaHUID2}',
                                SYA_SIZE = '{syaSize}',
                                SYA_PRICE = '{syaPrice}',
                                SYA_STATUS = '{syaStatus}',
                                SYA_AC_CODE = '{syaACCode}',
                                SYA_AC_NAME = '{syaACName}',
                                SYA_COMMENT = '{syaComment}',
                                SYA_PRINT = '{syaPrint}'
                                WHERE TAG_NO = '{tagNo}'";
                                }
                                else
                                {
                                    insertupdatequery = $@"INSERT INTO SALE_DATA (
                                CO_YEAR, CO_BOOK, VCH_NO, SR_NO, VCH_DATE, AC_CODE, IT_CODE, IT_DESC, PR_CODE, IT_TYPE,
                                TAG_NO, DESIGN, ITM_PCS, ITM_GWT, ITM_NWT, ITM_RAT, ITM_AMT, LBR_RATE, LBR_AMT, OTH_AMT,
                                NET_AMT, SYA_ID, SYA_CO_YEAR, SYA_CO_BOOK, SYA_VCH_NO, SYA_VCH_DATE, SYA_TAG_NO, SYA_GW,
                                SYA_NW, SYA_LABOUR_AMT, SYA_WHOLE_LABOUR_AMT, SYA_OTHER_AMT, SYA_IT_TYPE, SYA_ITEM_CODE,
                                SYA_ITEM_PURITY, SYA_ITEM_DESC, SYA_HUID1, SYA_HUID2, SYA_SIZE, SYA_PRICE, SYA_STATUS,
                                SYA_AC_CODE, SYA_AC_NAME, SYA_COMMENT, SYA_PRINT
                                ) VALUES (
                                '{coYear}', '{coBook}', '{vchNo}', '{srNo}', '{vchDate}', '{acCode}', '{itCode}', '{itDesc}', '{prCode}', '{itType}',
                                '{tagNo}', '{design}', '{itmPcs}', '{itmGwt}', '{itmNwt}', '{itmRat}', '{itmAmt}', '{lbrRate}', '{lbrAmt}', '{othAmt}',
                                '{netAmt}', {syaId}, '{syaCOYear}', '{syaCOBook}', '{syaVchNo}', '{syaVchDate}', '{syaTagNo}', '{syaGW}',
                                '{syaNW}', '{syaLabourAmt}', '{syaWholeLabourAmt}', '{syaOtherAmt}', '{syaItType}', '{syaItemCode}',
                                '{syaItemPurity}', '{syaItemDesc}', '{syaHUID1}', '{syaHUID2}', '{syaSize}', '{syaPrice}', '{syaStatus}',
                                '{syaACCode}', '{syaACName}', '{syaComment}', '{syaPrint}'
                                )";
                                }

                            }
                        }

                               
                        try
                        {
                        object res = helper.RunQueryWithoutParametersSYADataBase(insertupdatequery, "ExecuteNonQuery");
                         int result = Convert.ToInt32(res);

                        }
                        catch (Exception ex) { MessageBox.Show("InsertSaleDataIntoSQLite : : "+ex.Message); }
                    }
                    
                }
            }
            else
            {

                MessageBox.Show("InsertSaleDataIntoSQLite : : : No rows found.");
            }
            // Store values in string variables
            

        }
        private static void MapInStockParameters(SQLiteParameterCollection parameters, DataRow row)
        {
            // Map MS Access column values to SQLite parameters
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
            parameters.AddWithValue("@ITEM_PURITY", HelperFetchData.GetItemPurity(row["IT_CODE"].ToString(), PR_CODE));

            // Extract ITEM_DESC based on mappings
            string IT_TYPE = row["IT_TYPE"].ToString(); // Assuming this is a constant value
            string itemDesc = HelperFetchData.GetItemDescFromSQLite(PR_CODE, IT_TYPE);
            parameters.AddWithValue("@ITEM_DESC", itemDesc);
            // UNCOMMENT BELOW
            parameters.AddWithValue("@HUID1", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@HUID2", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@SIZE", row["ITM_SIZE"]);
            parameters.AddWithValue("@PRICE", row["MRP"]);
            parameters.AddWithValue("@STATUS", "INSTOCK"); // Assuming this is a constant value
            parameters.AddWithValue("@AC_CODE", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@AC_NAME", DBNull.Value); // Set to DBNull since it's NULL in MS Access
            parameters.AddWithValue("@COMMENT", row["DESIGN"]);
        }
        private static void ShowInStockErrorRowsDialog(List<int> errorRows, DataTable data)
        {
            // Create a new form to display error rows
            Form errorForm = new Form();
            errorForm.Text = "Error Rows";

            // Set the height of the form based on the number of rows
            int rowHeight = 22; // Adjust this value based on the actual row height
            int formHeight = Math.Min(errorRows.Count * rowHeight + 100, Screen.PrimaryScreen.WorkingArea.Height);

            // Set the width of the form to the full available width
            errorForm.Width = Screen.PrimaryScreen.WorkingArea.Width;

            // Center the form on the screen
            errorForm.StartPosition = FormStartPosition.CenterScreen;

            // Create a DataGridView to display error rows along with original data
            DataGridView errorGridView = new DataGridView();
            errorGridView.Dock = DockStyle.Fill;
            errorGridView.AllowUserToAddRows = false;
            errorGridView.ReadOnly = true;

            // Add columns to display both error information and relevant original data
            errorGridView.Columns.Add("RowNumber", "Row Number");
            errorGridView.Columns.Add("ErrorMessage", "Error Message");

            // Add columns relevant to insertion
            foreach (string columnName in new[] { "TAG_NO", "VCH_DATE", "ITM_GWT", "ITM_NWT", "LBR_RATE", "OTH_AMT", "PR_CODE", "IT_CODE", "IT_DESC", "ITM_SIZE", "MRP", "DESIGN" })
            {
                errorGridView.Columns.Add(columnName, columnName);
            }

            // Populate the DataGridView with error row numbers and error messages
            foreach (int rowNum in errorRows)
            {
                errorGridView.Rows.Add(rowNum, "Error occurred in this row");

                // Add the original data from the row with an error
                DataRow errorRow = data.Rows[rowNum - 1]; // Adjusting to 0-based index

                // Populate relevant data columns
                object[] rowData = new object[]
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

            // Add the DataGridView to the form
            errorForm.Controls.Add(errorGridView);

            // Calculate the height of the form title bar and add it to the form height
            int titleBarHeight = errorForm.Height - errorForm.ClientRectangle.Height;
            errorForm.Height = formHeight + titleBarHeight;

            // Show the form
            errorForm.ShowDialog();
        }
        public static void UpdateMessageBox(string message , TextBox txtMessageBox)
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
            // Assuming itCode has a format like "PR_CODEXXX" where XXX is the item purity
            return itCode.Replace(prcode, "");
        }

        public static string GetItemDescFromSQLite(string PR_CODE, string IT_TYPE)
        {
            // Implement logic to fetch ITEM_DESC from SQLite based on PR_CODE and IT_TYPE
            // You can modify this method based on your database schema and logic
            // For example, you might need to query the ITEM_MASTER table

            // Sample logic (replace with actual query and logic)
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
