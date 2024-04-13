using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using TextBox = System.Windows.Forms.TextBox;

namespace SYA
{
    public class HelperFetchData
    {



        public static string connectionToSYADatabase = helper.SYAConnectionString;

        public static void InsertInStockDataIntoSQLite(TextBox txtMessageBox, string queryToFetchFromMSAccess)
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
        public static void fetchSaleData()
        {
            string CO_CODE = null;
            string CO_YEAR = null;
            string CO_BOOK = null;
            string VCH_NO = null;
            string SR_NO = null;
            string VCH_DATE = null;
            string BOOK_NAME = null;
            string VCH_TYPE = null;
            string AC_CODE = null;
            string CUST_AC = null;
            string IT_CODE = null;
            string IT_DESC = null;
            string PR_CODE = null;
            string IT_TYPE = null;
            string TAG_SRNO = null;
            string TAG_NO = null;
            string DESIGN = null;
            string ITM_SIZE = null;
            string ITM_PCS = null;
            string ITM_GWT = null;
            string TAG_NWT = null;
            string ITM_NWT = null;
            string ITM_TOUCH = null;
            string ITM_WEST = null;
            string ITM_FINE = null;
            string ITM_RAT = null;
            string ITM_AMT = null;
            string LBR_RATE = null;
            string LBR_AMT = null;
            string OTH_AMT = null;
            string NET_AMT = null;
            string NET_COST = null;
            string MRP = null;
            string KR_NAME = null;
            string RATE_TYPE = null;
            string TR_TYPE = null;
            string SIGN = null;
            string EMP_CODE = null;
            string KR_OTH_WT = null;
            string KR_ITM_NWT = null;
            string KR_ITM_TOUCH = null;
            string KR_ITM_WEST = null;
            string KR_ITM_FINE = null;
            string HD_VCH_NO = null;
            string VCH_SRNO = null;
            string AND_TRFR = null;
            string BAG_WT = null;
            string BOX_WT = null;

            string SYA_ID = null;
            string SYA_CO_YEAR = null;
            string SYA_CO_BOOK = null;
            string SYA_VCH_NO = null;
            string SYA_VCH_DATE = null;
            string SYA_TAG_NO = null;
            string SYA_GW = null;
            string SYA_NW = null;
            string SYA_LABOUR_AMT = null;
            string SYA_WHOLE_LABOUR_AMT = null;
            string SYA_OTHER_AMT = null;
            string SYA_IT_TYPE = null;
            string SYA_ITEM_CODE = null;
            string SYA_ITEM_PURITY = null;
            string SYA_ITEM_DESC = null;
            string SYA_HUID1 = null;
            string SYA_HUID2 = null;
            string SYA_SIZE = null;
            string SYA_PRICE = null;
            string SYA_STATUS = null;
            string SYA_AC_CODE = null;
            string SYA_AC_NAME = null;
            string SYA_COMMENT = null;
            string SYA_PRINT = null;

            DataTable accessData = helper.FetchDataTableFromDataCareDataBase("SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '026' OR CO_BOOK = '26'");
            if (accessData.Rows.Count > 0)
            {
                foreach (DataRow row in accessData.Rows)
                {
                    CO_CODE = row["CO_CODE"] != DBNull.Value ? row["CO_CODE"].ToString() : "";
                    CO_YEAR = row["CO_YEAR"] != DBNull.Value ? row["CO_YEAR"].ToString() : "";
                    CO_BOOK = row["CO_BOOK"] != DBNull.Value ? row["CO_BOOK"].ToString() : "";
                    VCH_NO = row["VCH_NO"] != DBNull.Value ? row["VCH_NO"].ToString() : "";
                    SR_NO = row["SR_NO"] != DBNull.Value ? row["SR_NO"].ToString() : "";
                    VCH_DATE = row["VCH_DATE"] != DBNull.Value ? row["VCH_DATE"].ToString() : "";
                    BOOK_NAME = row["BOOK_NAME"] != DBNull.Value ? row["BOOK_NAME"].ToString() : "";
                    VCH_TYPE = row["VCH_TYPE"] != DBNull.Value ? row["VCH_TYPE"].ToString() : "";
                    AC_CODE = row["AC_CODE"] != DBNull.Value ? row["AC_CODE"].ToString() : "";
                    CUST_AC = row["CUST_AC"] != DBNull.Value ? row["CUST_AC"].ToString() : "";
                    IT_CODE = row["IT_CODE"] != DBNull.Value ? row["IT_CODE"].ToString() : "";
                    IT_DESC = row["IT_DESC"] != DBNull.Value ? row["IT_DESC"].ToString() : "";
                    PR_CODE = row["PR_CODE"] != DBNull.Value ? row["PR_CODE"].ToString() : "";
                    IT_TYPE = row["IT_TYPE"] != DBNull.Value ? row["IT_TYPE"].ToString() : "";
                    TAG_SRNO = row["TAG_SRNO"] != DBNull.Value ? row["TAG_SRNO"].ToString() : "";
                    TAG_NO = row["TAG_NO"] != DBNull.Value ? row["TAG_NO"].ToString() : "";
                    DESIGN = row["DESIGN"] != DBNull.Value ? row["DESIGN"].ToString() : "";
                    ITM_SIZE = row["ITM_SIZE"] != DBNull.Value ? row["ITM_SIZE"].ToString() : "";
                    ITM_PCS = row["ITM_PCS"] != DBNull.Value ? row["ITM_PCS"].ToString() : "";
                    ITM_GWT = row["ITM_GWT"] != DBNull.Value ? row["ITM_GWT"].ToString() : "";
                    TAG_NWT = row["TAG_NWT"] != DBNull.Value ? row["TAG_NWT"].ToString() : "";
                    ITM_NWT = row["ITM_NWT"] != DBNull.Value ? row["ITM_NWT"].ToString() : "";
                    ITM_TOUCH = row["ITM_TOUCH"] != DBNull.Value ? row["ITM_TOUCH"].ToString() : "";
                    ITM_WEST = row["ITM_WEST"] != DBNull.Value ? row["ITM_WEST"].ToString() : "";
                    ITM_FINE = row["ITM_FINE"] != DBNull.Value ? row["ITM_FINE"].ToString() : "";
                    ITM_RAT = row["ITM_RAT"] != DBNull.Value ? row["ITM_RAT"].ToString() : "";
                    ITM_AMT = row["ITM_AMT"] != DBNull.Value ? row["ITM_AMT"].ToString() : "";
                    LBR_RATE = row["LBR_RATE"] != DBNull.Value ? row["LBR_RATE"].ToString() : "";
                    LBR_AMT = row["LBR_AMT"] != DBNull.Value ? row["LBR_AMT"].ToString() : "";
                    OTH_AMT = row["OTH_AMT"] != DBNull.Value ? row["OTH_AMT"].ToString() : "";
                    NET_AMT = row["NET_AMT"] != DBNull.Value ? row["NET_AMT"].ToString() : "";
                    NET_COST = row["NET_COST"] != DBNull.Value ? row["NET_COST"].ToString() : "";
                    MRP = row["MRP"] != DBNull.Value ? row["MRP"].ToString() : "";
                    KR_NAME = row["KR_NAME"] != DBNull.Value ? row["KR_NAME"].ToString() : "";
                    RATE_TYPE = row["RATE_TYPE"] != DBNull.Value ? row["RATE_TYPE"].ToString() : "";
                    TR_TYPE = row["TR_TYPE"] != DBNull.Value ? row["TR_TYPE"].ToString() : "";
                    SIGN = row["SIGN"] != DBNull.Value ? row["SIGN"].ToString() : "";
                    EMP_CODE = row["EMP_CODE"] != DBNull.Value ? row["EMP_CODE"].ToString() : "";
                    KR_OTH_WT = row["KR_OTH_WT"] != DBNull.Value ? row["KR_OTH_WT"].ToString() : "";
                    KR_ITM_NWT = row["KR_ITM_NWT"] != DBNull.Value ? row["KR_ITM_NWT"].ToString() : "";
                    KR_ITM_TOUCH = row["KR_ITM_TOUCH"] != DBNull.Value ? row["KR_ITM_TOUCH"].ToString() : "";
                    KR_ITM_WEST = row["KR_ITM_WEST"] != DBNull.Value ? row["KR_ITM_WEST"].ToString() : "";
                    KR_ITM_FINE = row["KR_ITM_FINE"] != DBNull.Value ? row["KR_ITM_FINE"].ToString() : "";
                    HD_VCH_NO = row["HD_VCH_NO"] != DBNull.Value ? row["HD_VCH_NO"].ToString() : "";
                    VCH_SRNO = row["VCH_SRNO"] != DBNull.Value ? row["VCH_SRNO"].ToString() : "";
                    AND_TRFR = row["AND_TRFR"] != DBNull.Value ? row["AND_TRFR"].ToString() : "";
                    BAG_WT = row["BAG_WT"] != DBNull.Value ? row["BAG_WT"].ToString() : "";
                    BOX_WT = row["BOX_WT"] != DBNull.Value ? row["BOX_WT"].ToString() : "";

                    DataTable ac_dt = helper.FetchDataTableFromDataCareDataBase("SELECT AC_NAME FROM AC_MAST WHERE AC_CODE = '" + AC_CODE + "'");
                    string AC_NAME = string.Empty;
                    if (ac_dt.Rows.Count > 0)
                    {
                        AC_NAME = ac_dt.Rows[0]["AC_NAME"].ToString();
                    }

                    DataTable sya_dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + TAG_NO + "'");
                    if (sya_dt.Rows.Count > 0)
                    {
                        foreach (DataRow sya_row in sya_dt.Rows)
                        {
                            SYA_ID = sya_row["ID"] != DBNull.Value ? sya_row["ID"].ToString() : "";
                            SYA_CO_YEAR = sya_row["CO_YEAR"] != DBNull.Value ? sya_row["CO_YEAR"].ToString() : "";
                            SYA_CO_BOOK = sya_row["CO_BOOK"] != DBNull.Value ? sya_row["CO_BOOK"].ToString() : "";
                            SYA_VCH_NO = sya_row["VCH_NO"] != DBNull.Value ? sya_row["VCH_NO"].ToString() : "";
                            SYA_VCH_DATE = sya_row["VCH_DATE"] != DBNull.Value ? sya_row["VCH_DATE"].ToString() : "";
                            SYA_TAG_NO = sya_row["TAG_NO"] != DBNull.Value ? sya_row["TAG_NO"].ToString() : "";
                            SYA_GW = sya_row["GW"] != DBNull.Value ? sya_row["GW"].ToString() : "";
                            SYA_NW = sya_row["NW"] != DBNull.Value ? sya_row["NW"].ToString() : "";
                            SYA_LABOUR_AMT = sya_row["LABOUR_AMT"] != DBNull.Value ? sya_row["LABOUR_AMT"].ToString() : "";
                            SYA_WHOLE_LABOUR_AMT = sya_row["WHOLE_LABOUR_AMT"] != DBNull.Value ? sya_row["WHOLE_LABOUR_AMT"].ToString() : "";
                            SYA_OTHER_AMT = sya_row["OTHER_AMT"] != DBNull.Value ? sya_row["OTHER_AMT"].ToString() : "";
                            SYA_IT_TYPE = sya_row["IT_TYPE"] != DBNull.Value ? sya_row["IT_TYPE"].ToString() : "";
                            SYA_ITEM_CODE = sya_row["ITEM_CODE"] != DBNull.Value ? sya_row["ITEM_CODE"].ToString() : "";
                            SYA_ITEM_PURITY = sya_row["ITEM_PURITY"] != DBNull.Value ? sya_row["ITEM_PURITY"].ToString() : "";
                            SYA_ITEM_DESC = sya_row["ITEM_DESC"] != DBNull.Value ? sya_row["ITEM_DESC"].ToString() : "";
                            SYA_HUID1 = sya_row["HUID1"] != DBNull.Value ? sya_row["HUID1"].ToString() : "";
                            SYA_HUID2 = sya_row["HUID2"] != DBNull.Value ? sya_row["HUID2"].ToString() : "";
                            SYA_SIZE = sya_row["SIZE"] != DBNull.Value ? sya_row["SIZE"].ToString() : "";
                            SYA_PRICE = sya_row["PRICE"] != DBNull.Value ? sya_row["PRICE"].ToString() : "";
                            SYA_STATUS = sya_row["STATUS"] != DBNull.Value ? sya_row["STATUS"].ToString() : "";
                            SYA_AC_CODE = sya_row["AC_CODE"] != DBNull.Value ? sya_row["AC_CODE"].ToString() : "";
                            SYA_AC_NAME = sya_row["AC_NAME"] != DBNull.Value ? sya_row["AC_NAME"].ToString() : "";
                            SYA_COMMENT = sya_row["COMMENT"] != DBNull.Value ? sya_row["COMMENT"].ToString() : "";
                            SYA_PRINT = sya_row["PRINT"] != DBNull.Value ? sya_row["PRINT"].ToString() : "";

                            

                        }



                    }
                    void update()
                    {
                        string updateQuery = "UPDATE SYA_SALE_DATA SET " +
                                             "CO_YEAR = '" + CO_YEAR + "', " +
                                             "CO_BOOK = '" + CO_BOOK + "', " +
                                             "VCH_NO = '" + VCH_NO + "', " +
                                             "VCH_DATE = '" + Convert.ToDateTime(VCH_DATE).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                             "GW = '" + ITM_GWT + "', " +
                                             "NW = '" + ITM_NWT + "', " +
                                             "LABOUR_AMT = '" + SYA_LABOUR_AMT + "', " +
                                             "WHOLE_LABOUR_AMT = '" + SYA_WHOLE_LABOUR_AMT + "', " +
                                             "OTHER_AMT = '" + SYA_OTHER_AMT + "', " +
                                             "IT_TYPE = '" + IT_TYPE + "', " +
                                             "ITEM_CODE = '" + SYA_ITEM_CODE + "', " +
                                             "ITEM_PURITY = '" + SYA_ITEM_PURITY + "', " +
                                             "ITEM_DESC = '" + SYA_ITEM_DESC + "', " +
                                             "HUID1 = '" + SYA_HUID1 + "', " +
                                             "HUID2 = '" + SYA_HUID2 + "', " +
                                             "SIZE = '" + SYA_SIZE + "', " +
                                             "PRICE = '" + SYA_PRICE + "', " +
                                             "STATUS = '" + SYA_STATUS + "', " +
                                             "AC_CODE = '" + AC_CODE + "', " +
                                             "AC_NAME = '" + AC_NAME + "', " +
                                             "COMMENT = '" + SYA_COMMENT + "', " +
                                             "PRINT = '" + SYA_PRINT + "' " +
                                             "WHERE TAG_NO = '" + TAG_NO + "'";
                        helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                    }





                    void insert()
                    {
                        string insertQuery = "INSERT INTO SYA_SALE_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT, PRINT) VALUES ('" + CO_YEAR + "','" + CO_BOOK + "','" + VCH_NO + "','" + Convert.ToDateTime(VCH_DATE).ToString("yyyy-MM-dd HH:mm:ss") + "','" + TAG_NO + "','" + ITM_GWT + "','" + ITM_NWT + "','" + SYA_LABOUR_AMT + "','" + SYA_WHOLE_LABOUR_AMT + "','" + SYA_OTHER_AMT + "','" + IT_TYPE + "','" + SYA_ITEM_CODE + "','" + SYA_ITEM_PURITY + "','" + SYA_ITEM_DESC + "','" + SYA_HUID1 + "','" + SYA_HUID2 + "','" + SYA_SIZE + "','" + SYA_PRICE + "','" + SYA_STATUS + "','" + AC_CODE + "', '" + AC_NAME + "','" + SYA_COMMENT + "','" + SYA_PRINT + "');";
                        helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");

                    }
                    DataTable update_check = helper.FetchDataTableFromSYADataBase("SELECT TAG_NO FROM SYA_SALE_DATA WHERE TAG_NO ='" + TAG_NO + "'");
                    if (update_check.Rows.Count > 0)
                    {
                        update();
                    }
                    else
                    {
                        insert();
                    }
                    
                }
            }
        }
        public static void InsertSaleDataIntoSQLite______old(string queryToFetchFromMSAccess)
        {
            DataTable accessData = helper.FetchDataTableFromDataCareDataBase(queryToFetchFromMSAccess);
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
                    foreach (DataRow syaRow in syadt.Rows)
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
                        catch (Exception ex) { MessageBox.Show("InsertSaleDataIntoSQLite : : " + ex.Message); }
                    }

                }
            }
            else
            {

                MessageBox.Show("InsertSaleDataIntoSQLite : : : No rows found.");
            }
            // Store values in string variables


        }
        public static void InsertSaleDataIntoSQLite(string queryToFetchFromMSAccess)
        {
            DataTable accessData = helper.FetchDataTableFromDataCareDataBase(queryToFetchFromMSAccess);
            if (accessData != null && accessData.Rows.Count > 0)
            {
                // Loop through each row
                foreach (DataRow row in accessData.Rows)
                {
                    // Store values in variables
                    string coYear = row["CO_YEAR"].ToString();
                    string coBook = row["CO_BOOK"].ToString();
                    string vchNo = row["VCH_NO"].ToString();
                    string vchDate = row["VCH_DATE"].ToString();
                    string tagNo = row["TAG_NO"].ToString();
                    string itmGwt = row["ITM_GWT"].ToString();
                    string itmNwt = row["ITM_NWT"].ToString();
                    string labour_amt = "0";
                    string othAmt = row["OTH_AMT"].ToString();
                    string itType = row["IT_TYPE"].ToString();
                    string itemCode = row["PR_CODE"].ToString();
                    string itemCode_1 = row["IT_CODE"].ToString();
                    string purity = itemCode_1[2].ToString() + itemCode_1[3].ToString() + itemCode_1[4].ToString();
                    string itDesc = row["IT_DESC"].ToString();


                    string whole_labour_amt = row["LBR_AMT"].ToString();
                    string prCode = row["PR_CODE"].ToString();
                    string design = row["DESIGN"].ToString();
                    string itmPcs = row["ITM_PCS"].ToString();
                    string itmRat = row["ITM_RAT"].ToString();
                    string itmAmt = row["ITM_AMT"].ToString();
                    string lbrRate = row["LBR_RATE"].ToString();
                    string lbrAmt = row["LBR_AMT"].ToString();
                    string netAmt = row["NET_AMT"].ToString();
                    string acCode = row["AC_CODE"].ToString();
                    DataTable dt = helper.FetchDataTableFromDataCareDataBase("SELECT * FROM AC_MAST WHERE AC_CODE = '" + acCode + "'");
                    string acName = dt.Rows[0]["AC_NAME"].ToString();



                    string insertupdatequery = null;
                    using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                    {
                        sqliteConnection.Open();

                        using (SQLiteCommand checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM SYA_SALE_DATA WHERE TAG_NO = @TAG_NO", sqliteConnection))
                        {
                            checkCommand.Parameters.AddWithValue("@TAG_NO", tagNo);
                            int rowCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                            if (rowCount > 0)
                            {
                                insertupdatequery = $@"UPDATE SYA_SALE_DATA SET
                                CO_YEAR = '{coYear}',
                                CO_BOOK = '{coBook}',
                                VCH_NO = '{vchNo}',
                              
                                VCH_DATE = '{vchDate}',
                                AC_CODE = '{acCode}',
                                IT_DESC = '{itDesc}',
                                PR_CODE = '{prCode}',
                                IT_TYPE = '{itType}',
                                DESIGN = '{design}',
                                ITM_PCS = '{itmPcs}',
                                GW = '{itmGwt}',
                                NW = '{itmNwt}',
                                ITM_RAT = '{itmRat}',
                                ITM_AMT = '{itmAmt}',
                                LBR_RATE = '{lbrRate}',
                                LBR_AMT = '{lbrAmt}',
                                OTH_AMT = '{othAmt}',
                                NET_AMT = '{netAmt}',
                               
                                WHERE TAG_NO = '{tagNo}'";
                            }
                            else
                            {
                                insertupdatequery = $@"INSERT INTO SYA_SALE_DATA (
                                CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, AC_CODE, ITEM_CODE, IT_DESC, PR_CODE, IT_TYPE,
                                TAG_NO, DESIGN, ITM_PCS, GW
, NW, ITM_RAT, ITM_AMT, LBR_RATE, LBR_AMT, OTH_AMT,
                                NET_AMT
                                ) VALUES (
                                '{coYear}', '{coBook}', '{vchNo}',  '{vchDate}', '{acCode}', '', '{itDesc}', '{prCode}', '{itType}',
                                '{tagNo}', '{design}', '{itmPcs}', '{itmGwt}', '{itmNwt}', '{itmRat}', '{itmAmt}', '{lbrRate}', '{lbrAmt}', '{othAmt}',
                                '{netAmt}',
                                )";
                            }

                        }
                    }


                    try
                    {
                        object res = helper.RunQueryWithoutParametersSYADataBase(insertupdatequery, "ExecuteNonQuery");
                        int result = Convert.ToInt32(res);

                    }
                    catch (Exception ex) { MessageBox.Show("InsertSaleDataIntoSQLite : : " + ex.Message); }


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
