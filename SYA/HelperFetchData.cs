﻿using System.Data;
using System.Data.SQLite;
using System.Text;
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
            void nullit()
            {
                CO_CODE = null;
                CO_YEAR = null;
                CO_BOOK = null;
                VCH_NO = null;
                SR_NO = null;
                VCH_DATE = null;
                BOOK_NAME = null;
                VCH_TYPE = null;
                AC_CODE = null;
                CUST_AC = null;
                IT_CODE = null;
                IT_DESC = null;
                PR_CODE = null;
                IT_TYPE = null;
                TAG_SRNO = null;
                TAG_NO = null;
                DESIGN = null;
                ITM_SIZE = null;
                ITM_PCS = null;
                ITM_GWT = null;
                TAG_NWT = null;
                ITM_NWT = null;
                ITM_TOUCH = null;
                ITM_WEST = null;
                ITM_FINE = null;
                ITM_RAT = null;
                ITM_AMT = null;
                LBR_RATE = null;
                LBR_AMT = null;
                OTH_AMT = null;
                NET_AMT = null;
                NET_COST = null;
                MRP = null;
                KR_NAME = null;
                RATE_TYPE = null;
                TR_TYPE = null;
                SIGN = null;
                EMP_CODE = null;
                KR_OTH_WT = null;
                KR_ITM_NWT = null;
                KR_ITM_TOUCH = null;
                KR_ITM_WEST = null;
                KR_ITM_FINE = null;
                HD_VCH_NO = null;
                VCH_SRNO = null;
                AND_TRFR = null;
                BAG_WT = null;
                BOX_WT = null;
                SYA_ID = null;
                SYA_CO_YEAR = null;
                SYA_CO_BOOK = null;
                SYA_VCH_NO = null;
                SYA_VCH_DATE = null;
                SYA_TAG_NO = null;
                SYA_GW = null;
                SYA_NW = null;
                SYA_LABOUR_AMT = null;
                SYA_WHOLE_LABOUR_AMT = null;
                SYA_OTHER_AMT = null;
                SYA_IT_TYPE = null;
                SYA_ITEM_CODE = null;
                SYA_ITEM_PURITY = null;
                SYA_ITEM_DESC = null;
                SYA_HUID1 = null;
                SYA_HUID2 = null;
                SYA_SIZE = null;
                SYA_PRICE = null;
                SYA_STATUS = null;
                SYA_AC_CODE = null;
                SYA_AC_NAME = null;
                SYA_COMMENT = null;
                SYA_PRINT = null;
            }
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
                                             "PRINT = '" + SYA_PRINT + "', " +
                                             "ITM_RAT = '" + ITM_RAT + "', " +
                                             "ITM_AMT = '" + ITM_AMT + "' ," +
                                             "LBR_RATE = '" + LBR_RATE + "' ," +
                                             "LBR_AMT = '" + LBR_AMT + "' ," +
                                             "OTH_AMT = '" + OTH_AMT + "' ," +
                                             "NET_AMT = '" + NET_AMT + "' " +
                                             "WHERE TAG_NO = '" + TAG_NO + "'";
                        helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                        nullit();
                    }
                    void insert()
                    {
                        string insertQuery = "INSERT INTO SYA_SALE_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT, PRINT,ITM_RAT" +
                            " ,ITM_AMT,LBR_RATE,LBR_AMT,OTH_AMT,NET_AMT) VALUES ('" + CO_YEAR + "','" + CO_BOOK + "','" + VCH_NO + "','" + Convert.ToDateTime(VCH_DATE).ToString("yyyy-MM-dd HH:mm:ss") + "','" + TAG_NO + "','" + ITM_GWT + "','" + ITM_NWT + "','" + SYA_LABOUR_AMT + "','" + SYA_WHOLE_LABOUR_AMT + "','" + SYA_OTHER_AMT + "','" + IT_TYPE + "','" + SYA_ITEM_CODE + "','" + SYA_ITEM_PURITY + "','" + SYA_ITEM_DESC + "','" + SYA_HUID1 + "','" + SYA_HUID2 + "','" + SYA_SIZE + "','" + SYA_PRICE + "','" + SYA_STATUS + "','" + AC_CODE + "', '" + AC_NAME + "','" + SYA_COMMENT + "','" + SYA_PRINT + "' ,'" + ITM_RAT + "','" + ITM_AMT + "','" + LBR_RATE + "','" + LBR_AMT + "','" + OTH_AMT + "','" + NET_AMT + "'); ";
                        nullit();
                        helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
                    }
                    DataTable update_check = helper.FetchDataTableFromSYADataBase("SELECT TAG_NO FROM SYA_SALE_DATA WHERE TAG_NO ='" + TAG_NO + "'");
                    if (update_check.Rows.Count > 0)
                    {
                        update();
                        nullit();
                    }
                    else
                    {
                        insert();
                        nullit();
                    }
                }
                MessageBox.Show("Done");
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
            parameters.AddWithValue("@ITEM_PURITY", HelperFetchData.GetItemPurity(row["IT_CODE"].ToString(), PR_CODE));
            string IT_TYPE = row["IT_TYPE"].ToString(); string itemDesc = HelperFetchData.GetItemDescFromSQLite(PR_CODE, IT_TYPE);
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
