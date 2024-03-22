using System;



using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public static class Contact
    {
        static DataTable RawData = new DataTable();
        static DataTable ExcludedData = new DataTable();
        static DataTable VerifiedCustomerData = new DataTable();
        static DataTable OtherVerifiedData = new DataTable();
        static DataTable UnverifiedData = new DataTable();
        static DataTable DataCareDataSorted = new DataTable();
        static string rawMobile;
        static string rawName;
        static string rawSource;
        static string mobile;
        static RichTextBox richText = new RichTextBox();
        static void fetchData()
        {
            clearData(RawData);
            RawData = helper.FetchDataTableFromSYADataBase("SELECT * FROM RawData");
            clearData(ExcludedData);
            ExcludedData = helper.FetchDataTableFromSYADataBase("SELECT * FROM ExcludedData");
            clearData(VerifiedCustomerData);
            VerifiedCustomerData = helper.FetchDataTableFromSYADataBase("SELECT * FROM VerifiedCustomerData");
            clearData(OtherVerifiedData);
            OtherVerifiedData = helper.FetchDataTableFromSYADataBase("SELECT * FROM OtherVerifiedData");
            clearData(UnverifiedData);
            UnverifiedData = helper.FetchDataTableFromSYADataBase("SELECT * FROM UnverifiedData");
        }
        static void setDataTableColumns(DataTable dt)
        {
            clearData(dt);
            dt.Columns.Add("acCode", typeof(string));
            dt.Columns.Add("acName0", typeof(string));
            dt.Columns.Add("acName1", typeof(string));
            dt.Columns.Add("acName2", typeof(string));
            dt.Columns.Add("acName3", typeof(string));
            dt.Columns.Add("acName4", typeof(string));
            dt.Columns.Add("acName5", typeof(string));
            dt.Columns.Add("acName6", typeof(string));
            dt.Columns.Add("acName7", typeof(string));
            dt.Columns.Add("acName8", typeof(string));
            dt.Columns.Add("acName9", typeof(string));
            dt.Columns.Add("acName10", typeof(string));
            dt.Columns.Add("acAdd", typeof(string));
            dt.Columns.Add("acCity", typeof(string));
            dt.Columns.Add("acMobile", typeof(string));
            dt.Columns.Add("acPan", typeof(string));
            dt.Columns.Add("acAdhaar", typeof(string));
            dt.Columns.Add("acGroup", typeof(string));
            dt.Columns.Add("acSource", typeof(string));
        }
        static void AddText(string text)
        {
            richText.Text += "\n\n" + text;
        }
        static void printDT(DataTable dt)
        {
            StringBuilder message = new StringBuilder();

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    message.AppendLine($"{column.ColumnName}: {row[column]}");
                }
                message.AppendLine();
            }
        }
        static void clearData(DataTable dt)
        {
            dt.Clear();
            dt.Columns.Clear();
        }
        public static void ParentDataCareData(RichTextBox richTextBox1)
        {

            richText = richTextBox1;
            DataTable DataCareData;
            getDataFromDataCare();
            setDataTableColumns(DataCareDataSorted);
            SortDataCareData();
            void getDataFromDataCare()
            {
                DataCareData = helper.FetchFromDataCareDataBase("SELECT * FROM AC_MAST ");
            }
            void SortDataCareData()
            {
                string acCode, acName, acAdd, acCity, acPhone, acMobile, acMobile2, acPanNO, acAdhaRNO;
                foreach (DataRow row in DataCareData.Rows)
                {
                    setVariables();
                    if (!string.IsNullOrEmpty(acPhone))
                    {
                        addRow("AC_PHONE");
                    }
                    if (!string.IsNullOrEmpty(acMobile))
                    {
                        addRow("AC_MOBILE");
                    }
                    if (!string.IsNullOrEmpty(acMobile2))
                    {
                        addRow("AC_MOBILE2");
                    }
                    void setVariables()
                    {
                        acCode = row.Field<string>("AC_CODE");
                        acName = row.Field<string>("AC_NAME");
                        acAdd = row.Field<string>("AC_ADD1") + "   " + row.Field<string>("AC_ADD2") + "   " + row.Field<string>("AC_ADD3") + "   " + row.Field<string>("AC_PIN");
                        acCity = row.Field<string>("AC_CITY");
                        acPhone = row.Field<string>("AC_PHONE");
                        acMobile = row.Field<string>("AC_MOBILE");
                        acMobile2 = row.Field<string>("AC_MOBILE2");
                        acPanNO = row.Field<string>("AC_PANNO");
                        acAdhaRNO = row.Field<string>("AC_ADHARNO");
                    }
                    void addRow(string mobileField)
                    {
                        DataRow newRow = DataCareDataSorted.NewRow();
                        newRow["acCode"] = row.Field<string>("AC_CODE");
                        newRow["acName0"] = row.Field<string>("AC_NAME");
                        newRow["acAdd"] = row.Field<string>("AC_ADD1") + "   " + row.Field<string>("AC_ADD2") + "   " + row.Field<string>("AC_ADD3") + "   " + row.Field<string>("AC_PIN");
                        newRow["acCity"] = row.Field<string>("AC_CITY");
                        newRow["acMobile"] = row.Field<string>(mobileField);
                        newRow["acPan"] = row.Field<string>("AC_PANNO");
                        newRow["acAdhaar"] = row.Field<string>("AC_ADHARNO");
                        DataCareDataSorted.Rows.Add(newRow);
                    }
                }
            }
            compare(DataCareDataSorted);
        }
        static bool compareInTable(DataRow row_RawData, DataTable dt, bool addInUnverifiedData, string namee)
        {
            rawMobile = row_RawData["acMobile"].ToString();
            rawName = row_RawData["acName0"].ToString();
            rawSource = row_RawData["acSource"].ToString();
            DataRow[] dtRows = dt.Select();
            bool r = false;
            for (int rowIndex = 0; rowIndex < dtRows.Length; rowIndex++)
            {
                mobile = dtRows[rowIndex]["acMobile"].ToString();
                if (mobile == rawMobile)
                {
                    bool nameFound = false;
                    for (int i = 0; i <= 10; i++)
                    {
                        if (rawName == dtRows[rowIndex][$"acName{i}"].ToString())
                        {
                            nameFound = true;
                            row_RawData.Delete();
                            break;
                        }
                    }
                    if (!nameFound)
                    {
                        for (int i = 0; i <= 10; i++)
                        {
                            if (string.IsNullOrEmpty(dtRows[rowIndex][$"acName{i}"].ToString()))
                            {
                                dtRows[rowIndex][$"acName{i}"] = rawName;
                                row_RawData.Delete();
                                break;
                            }
                        }
                    }
                    r = true;
                }
            }
            return r;
        }

        private static void compare(DataTable dt)
        {
            fetchData();
            DataRow[] dtRows = dt.Select();
            for (int rowIndex = 0; rowIndex < dtRows.Length; rowIndex++)
            {
                if (compareInTable(dtRows[rowIndex], ExcludedData, false, "ExcludedData")) { }
                else if (compareInTable(dtRows[rowIndex], VerifiedCustomerData, false, "VerifiedCustomerData")) { }
                else if (compareInTable(dtRows[rowIndex], OtherVerifiedData, false, "OtherVerifiedData")) { }
                else if (compareInTable(dtRows[rowIndex], UnverifiedData, true, "UnverifiedData")) { }
                UnverifiedData.ImportRow(dtRows[rowIndex]);
            }
            dt.AcceptChanges();
            ExcludedData.AcceptChanges();
            VerifiedCustomerData.AcceptChanges();
            OtherVerifiedData.AcceptChanges();
            UnverifiedData.AcceptChanges();
            addDatatavleToDatabase(OtherVerifiedData, "O" +
                "therVerifiedData");
            addDatatavleToDatabase(UnverifiedData, "UnverifiedData");
            printDT(OtherVerifiedData);
        }
        public static void addDatatavleToDatabase(DataTable dt, string tableName)
        {
            // Remove all data from the table
            string deleteQuery = $"DELETE FROM {tableName}";
            helper.RunQueryWithoutParametersSYADataBase(deleteQuery, "ExecuteNonQuery");

           

            // Insert all data from the DataTable to the table
            foreach (DataRow row in dt.Rows)
            {
                StringBuilder columns = new StringBuilder();
                StringBuilder values = new StringBuilder();

                foreach (DataColumn column in dt.Columns)
                {
                    columns.Append($"{column.ColumnName},");
                    values.Append($"@{column.ColumnName},");
                }

                // Remove the trailing comma
                columns.Remove(columns.Length - 1, 1);
                values.Remove(values.Length - 1, 1);

                string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                SQLiteParameter[] parameters = new SQLiteParameter[dt.Columns.Count];

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    parameters[i] = new SQLiteParameter($"@{dt.Columns[i].ColumnName}", row[i]);
                }

                bool insertSuccess = helper.RunQueryWithParametersSYADataBase(insertQuery, parameters);

                if (!insertSuccess)
                {
                    MessageBox.Show($"Failed to insert data into {tableName} table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show($"Data successfully inserted into {tableName} table.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
