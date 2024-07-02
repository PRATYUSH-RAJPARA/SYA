using SYA.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SYA.Sales
{
    public static class FetchSaleDataHelper
    {
        public static string str1 = "";
        public static string str2 = "";
        public static int totalInserted = 0;
        public static int totalUpdated = 0;
        public static string connectionString = helper.accessConnectionString;
        public static NotifyForm notifyForm;
        public static void fetchSaleData()
        {
            string query = "SELECT * FROM DataCareFiles";
            DataTable dt = helper.FetchDataTableFromSYASettingsDataBase(query);
            if (dt.Rows.Count > 0)
            {
                notifyForm = new NotifyForm();
                notifyForm.Show();
                int i = 0;
                foreach (DataRow dt_row in dt.Rows)
                {
                    SetFileName(dt_row["DataCareFileName"].ToString());
                    DataTable accessData = FetchDataTableFromDataCareDataBase("SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '026' OR CO_BOOK = '26'");
                    // DataTable accessData = helper.FetchDataTableFromDataCareDataBase("SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '026' OR CO_BOOK = '26'");
                    fetchData(accessData, dt_row["DataCareFileName"].ToString());
                }
                notifyForm.Close();
            }
        }
        public static DataTable FetchDataTableFromDataCareDataBase(string query)
        {
            DataTable dataCareDataTable = new DataTable();
            try
            {
                using (OleDbConnection accessConnection = new OleDbConnection(connectionString))
                {
                    accessConnection.Open();
                    try
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, accessConnection))
                        {
                            adapter.Fill(dataCareDataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show("FetchFromDataCareDataBase Error executing query: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show("Outer FetchFromDataCareDataBase Error executing query: " + ex.Message);
            }
            return dataCareDataTable;
        }
        public static void SetFileName(string newFileName)
        {
            string s = connectionString;
            var parts = connectionString.Split(';');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                {
                    var currentFilePath = parts[i].Substring("Data Source=".Length).Trim('"');
                    var directoryPath = Path.GetDirectoryName(currentFilePath);
                    var newFilePath = Path.Combine(directoryPath, newFileName);
                    parts[i] = $"Data Source=\"{newFilePath}.mdb\"";
                    break;
                }
            }
            connectionString = string.Join(";", parts);
            s += "\n" + connectionString;
        }
        public static void fetchData(DataTable accessData, string FileName)
        {
            string CO_YEAR = null;
            string CO_BOOK = null;
            string VCH_NO = null;
            string VCH_DATE = null;
            string AC_CODE = null;
            string TAG_NO = null;
            string ITM_GWT = null;
            string ITM_NWT = null;
            string IT_TYPE = null;
            string PR_CODE = null;
            string IT_CODE = null;
            string ITM_RAT = null;
            string ITM_AMT = null;
            string LBR_RATE = null;
            string LBR_AMT = null;
            string OTH_AMT = null;
            string NET_AMT = null;
            string SYA_LABOUR_AMT = null;
            string SYA_WHOLE_LABOUR_AMT = null;
            string SYA_OTHER_AMT = null;
            string SYA_ITEM_CODE = null;
            string SYA_ITEM_PURITY = null;
            string SYA_ITEM_DESC = null;
            string SYA_HUID1 = null;
            string SYA_HUID2 = null;
            string SYA_SIZE = null;
            string SYA_PRICE = null;
            string SYA_STATUS = null;
            string AC_NAME = null;
            string SYA_COMMENT = null;
            string SYA_PRINT = null;
            void nullit()
            {
                CO_YEAR = null;
                CO_BOOK = null;
                VCH_NO = null;
                VCH_DATE = null;
                AC_CODE = null;
                TAG_NO = null;
                ITM_GWT = null;
                ITM_NWT = null;
                IT_TYPE = null;
                PR_CODE = null;
                IT_CODE = null;
                ITM_RAT = null;
                ITM_AMT = null;
                LBR_RATE = null;
                LBR_AMT = null;
                OTH_AMT = null;
                NET_AMT = null;
                SYA_LABOUR_AMT = null;
                SYA_WHOLE_LABOUR_AMT = null;
                SYA_OTHER_AMT = null;
                SYA_ITEM_CODE = null;
                SYA_ITEM_PURITY = null;
                SYA_ITEM_DESC = null;
                SYA_HUID1 = null;
                SYA_HUID2 = null;
                SYA_SIZE = null;
                SYA_PRICE = null;
                SYA_STATUS = null;
                AC_NAME = null;
                SYA_COMMENT = null;
                SYA_PRINT = null;
            }
            int accessDataCount = accessData.Rows.Count;
            int updateCount = 0;
            int insertCount = 0;
            if (accessData.Rows.Count > 0)
            {
                foreach (DataRow row in accessData.Rows)
                {
                    CO_YEAR = row["CO_YEAR"] != DBNull.Value ? row["CO_YEAR"].ToString() : "";
                    CO_BOOK = row["CO_BOOK"] != DBNull.Value ? row["CO_BOOK"].ToString() : "";
                    VCH_NO = row["VCH_NO"] != DBNull.Value ? row["VCH_NO"].ToString() : "";
                    VCH_DATE = row["VCH_DATE"] != DBNull.Value ? row["VCH_DATE"].ToString() : "";
                    AC_CODE = row["AC_CODE"] != DBNull.Value ? row["AC_CODE"].ToString() : "";
                    TAG_NO = row["TAG_NO"] != DBNull.Value ? row["TAG_NO"].ToString() : "";
                    ITM_GWT = row["ITM_GWT"] != DBNull.Value ? row["ITM_GWT"].ToString() : "";
                    ITM_NWT = row["ITM_NWT"] != DBNull.Value ? row["ITM_NWT"].ToString() : "";
                    IT_TYPE = row["IT_TYPE"] != DBNull.Value ? row["IT_TYPE"].ToString() : "";
                    ITM_RAT = row["ITM_RAT"] != DBNull.Value ? row["ITM_RAT"].ToString() : "";
                    ITM_AMT = row["ITM_AMT"] != DBNull.Value ? row["ITM_AMT"].ToString() : "";
                    LBR_RATE = row["LBR_RATE"] != DBNull.Value ? row["LBR_RATE"].ToString() : "";
                    LBR_AMT = row["LBR_AMT"] != DBNull.Value ? row["LBR_AMT"].ToString() : "";
                    OTH_AMT = row["OTH_AMT"] != DBNull.Value ? row["OTH_AMT"].ToString() : "";
                    NET_AMT = row["NET_AMT"] != DBNull.Value ? row["NET_AMT"].ToString() : "";
                    PR_CODE = row["PR_CODE"] != DBNull.Value ? row["PR_CODE"].ToString() : "";
                    IT_CODE = row["IT_CODE"] != DBNull.Value ? row["IT_CODE"].ToString() : "";
                    SYA_STATUS = "SOLD";
                    DataTable ac_dt = helper.FetchDataTableFromDataCareDataBase("SELECT AC_NAME FROM AC_MAST WHERE AC_CODE = '" + AC_CODE + "'");
                    if (ac_dt.Rows.Count > 0)
                    {
                        AC_NAME = ac_dt.Rows[0]["AC_NAME"].ToString();
                    }
                    DataTable sya_dt = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + TAG_NO + "'");
                    if (sya_dt.Rows.Count > 0)
                    {
                        foreach (DataRow sya_row in sya_dt.Rows)
                        {
                            SYA_LABOUR_AMT = sya_row["LABOUR_AMT"] != DBNull.Value ? sya_row["LABOUR_AMT"].ToString() : "";
                            SYA_WHOLE_LABOUR_AMT = sya_row["WHOLE_LABOUR_AMT"] != DBNull.Value ? sya_row["WHOLE_LABOUR_AMT"].ToString() : "";
                            SYA_OTHER_AMT = sya_row["OTHER_AMT"] != DBNull.Value ? sya_row["OTHER_AMT"].ToString() : "";
                            SYA_ITEM_CODE = sya_row["ITEM_CODE"] != DBNull.Value ? sya_row["ITEM_CODE"].ToString() : "";
                            SYA_ITEM_PURITY = sya_row["ITEM_PURITY"] != DBNull.Value ? sya_row["ITEM_PURITY"].ToString() : "";
                            SYA_ITEM_DESC = sya_row["ITEM_DESC"] != DBNull.Value ? sya_row["ITEM_DESC"].ToString() : "";
                            SYA_HUID1 = sya_row["HUID1"] != DBNull.Value ? sya_row["HUID1"].ToString() : "";
                            SYA_HUID2 = sya_row["HUID2"] != DBNull.Value ? sya_row["HUID2"].ToString() : "";
                            SYA_SIZE = sya_row["SIZE"] != DBNull.Value ? sya_row["SIZE"].ToString() : "";
                            SYA_PRICE = sya_row["PRICE"] != DBNull.Value ? sya_row["PRICE"].ToString() : "";
                            SYA_STATUS = "SOLD";
                            SYA_COMMENT = sya_row["COMMENT"] != DBNull.Value ? sya_row["COMMENT"].ToString() : "";
                            SYA_PRINT = sya_row["PRINT"] != DBNull.Value ? sya_row["PRINT"].ToString() : "";
                        }
                    }
                    void update()
                    {
                        if (SYA_LABOUR_AMT == null)
                        {
                            SYA_LABOUR_AMT = "0";
                        }
                        if (SYA_WHOLE_LABOUR_AMT == null)
                        {
                            SYA_WHOLE_LABOUR_AMT = LBR_AMT;
                        }
                        if (SYA_OTHER_AMT == null)
                        {
                            SYA_OTHER_AMT = OTH_AMT;
                        }
                        if (SYA_ITEM_CODE == null)
                        {
                            SYA_ITEM_CODE = PR_CODE;
                        }
                        if (SYA_ITEM_PURITY == null)
                        {
                            try
                            {
                                IT_CODE = IT_CODE.Replace(PR_CODE, "");
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message + "   \n" + IT_CODE + "   " + PR_CODE); }
                            SYA_ITEM_PURITY = IT_CODE;
                        }
                        if (SYA_ITEM_DESC == null)
                        {
                            string ItemName = null;
                            string query = $"SELECT * FROM ITEM_MASTER WHERE PR_CODE = '{PR_CODE}' AND IT_TYPE = '{IT_TYPE}'";
                            using (SQLiteDataReader reader1 = helper.FetchDataFromSYADataBase(query))
                            {
                                if (reader1 != null && reader1.Read())
                                {
                                    ItemName = reader1["IT_NAME"].ToString();
                                    reader1.Close();
                                }
                            }
                            SYA_ITEM_DESC = ItemName;
                        }
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
                        if (SYA_LABOUR_AMT == null)
                        {
                            SYA_LABOUR_AMT = "0";
                        }
                        if (SYA_WHOLE_LABOUR_AMT == null)
                        {
                            SYA_WHOLE_LABOUR_AMT = LBR_AMT;
                        }
                        if (SYA_OTHER_AMT == null)
                        {
                            SYA_OTHER_AMT = OTH_AMT;
                        }
                        if (SYA_ITEM_CODE == null)
                        {
                            SYA_ITEM_CODE = PR_CODE;
                        }
                        if (SYA_ITEM_PURITY == null)
                        {
                            try
                            {
                                IT_CODE = IT_CODE.Replace(PR_CODE, "");
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message + "   \n" + IT_CODE + "   " + PR_CODE); }
                            SYA_ITEM_PURITY = IT_CODE;
                        }
                        if (SYA_ITEM_DESC == null)
                        {
                            string ItemName = null;
                            string query = $"SELECT * FROM ITEM_MASTER WHERE PR_CODE = '{PR_CODE}' AND IT_TYPE = '{IT_TYPE}'";
                            using (SQLiteDataReader reader1 = helper.FetchDataFromSYADataBase(query))
                            {
                                if (reader1 != null && reader1.Read())
                                {
                                    ItemName = reader1["IT_NAME"].ToString();
                                    reader1.Close();
                                }
                            }
                            SYA_ITEM_DESC = ItemName;
                        }
                        string insertQuery = "INSERT INTO SYA_SALE_DATA (CO_YEAR, CO_BOOK, VCH_NO, VCH_DATE, TAG_NO, GW, NW, LABOUR_AMT, WHOLE_LABOUR_AMT, OTHER_AMT, IT_TYPE, ITEM_CODE, ITEM_PURITY, ITEM_DESC, HUID1, HUID2, SIZE, PRICE, STATUS, AC_CODE, AC_NAME, COMMENT, PRINT, ITM_RAT, ITM_AMT, LBR_RATE, LBR_AMT, OTH_AMT, NET_" +
                            "AMT) " +
                                             "VALUES ('" + CO_YEAR + "', '" + CO_BOOK + "', '" + VCH_NO + "', '" + Convert.ToDateTime(VCH_DATE).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + TAG_NO + "', '" + ITM_GWT + "', '" + ITM_NWT + "', '" + SYA_LABOUR_AMT + "', '" + SYA_WHOLE_LABOUR_AMT + "', '" + SYA_OTHER_AMT + "', '" + IT_TYPE + "', '" + SYA_ITEM_CODE + "', '" + SYA_ITEM_PURITY + "', '" + SYA_ITEM_DESC + "', '" + SYA_HUID1 + "', '" + SYA_HUID2 + "', '" + SYA_SIZE + "', '" + SYA_PRICE + "', '" + "SOLD" + "', '" + AC_CODE + "', '" + AC_NAME + "', '" + SYA_COMMENT + "', '" + SYA_PRINT + "', '" + ITM_RAT + "', '" + ITM_AMT + "', '" + LBR_RATE + "', '" + LBR_AMT + "', '" + OTH_AMT + "', '" + NET_AMT + "')";
                        helper.RunQueryWithoutParametersSYADataBase(insertQuery, "ExecuteNonQuery");
                        nullit();
                    }
                    DataTable sale_dt = helper.FetchDataTableFromSYADataBase("SELECT TAG_NO FROM SYA_SALE_DATA WHERE TAG_NO = '" + TAG_NO + "' AND VCH_NO ='" + VCH_NO + "' AND CO_YEAR ='" + CO_YEAR + "'");
                    if (sale_dt.Rows.Count > 0)
                    {
                        update();
                        updateCount++;
                        totalUpdated++;
                    }
                    else
                    {
                        insert();
                        insertCount++;
                        totalInserted++;
                    }
                    str1 = "Fetching Sale Data" + Environment.NewLine + Environment.NewLine + " Total Updated Count: " + totalUpdated + Environment.NewLine + " Total Inserted Count: " + totalInserted;
                    str2 = FileName + Environment.NewLine + Environment.NewLine + "  Total_Count: " + accessDataCount + Environment.NewLine + " Updated_Count: " + updateCount + Environment.NewLine + " Inserted_Count: " + insertCount;
                    notifyForm.ShowNotification2(str2);
                    notifyForm.ShowNotification1(str1);
                    Application.DoEvents();
                }
            }
        }
    }
}
