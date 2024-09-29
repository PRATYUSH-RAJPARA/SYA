using SYA.Helper;
using System.Data;
using System.Data.SQLite;
using System.Text;
namespace SYA
{
    public class Contact
    {
        DataTable RawData = new DataTable();
        DataTable ExcludedData = new DataTable();
        DataTable CustomerData = new DataTable();
        DataTable OtherData = new DataTable();
        DataTable RelativeData = new DataTable();
        DataTable KarigarData = new DataTable();
        DataTable VepariData = new DataTable();
        DataTable UnverifiedData = new DataTable();
        DataTable RawDataSorted = new DataTable();
        DataTable WrongData = new DataTable();
        string rawMobile;
        string rawName;
        string rawSource;
        string mobile;
        RichTextBox richText = new RichTextBox();
        void checkNumber(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt.Rows[i];
                string number = row["acMobile"].ToString();
                number = new string(Array.FindAll(number.ToCharArray(), char.IsDigit));
                if (number.Length == 12 && number.StartsWith("91"))
                {
                    number = number.Substring(2);
                }
                if (number.Length != 10)
                {
                    row.Delete();
                }
                else
                {
                    row["acMobile"] = number;
                }
            }
            dt.AcceptChanges();
        }
        void fetchData(string tableName)
        {
            clearData(RawData);
            if (tableName == "datacare")
            {
                RawData = helper.FetchDataTableFromDataCareDataBase("SELECT * FROM AC_MAST ");
            }
            else
            {
                RawData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM RawData");
            }
            clearData(ExcludedData);
            ExcludedData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM ExcludedData");
            clearData(CustomerData);
            CustomerData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM CustomerData");
            clearData(OtherData);
            OtherData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM OtherData");
            clearData(UnverifiedData);
            UnverifiedData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM UnverifiedData");
            clearData(WrongData);
            WrongData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM WrongData");
            clearData(RelativeData);
            RelativeData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM RelativeData");
            clearData(KarigarData);
            KarigarData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM KarigarData");
            clearData(VepariData);
            VepariData = helper.FetchDataTableFromSYAContactDataBase("SELECT * FROM VepariData");
        }
        void setDataTableColumns(DataTable dt)
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
        void AddText(string text)
        {
            richText.Text += "\n\n" + text;
        }
        void printDT(DataTable dt)
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
        void clearData(DataTable dt)
        {
            dt.Clear();
            dt.Columns.Clear();
        }
        public void SortContactData(RichTextBox richTextBox1, string tableName)
        {
            richText = richTextBox1;
            fetchData(tableName);
            setDataTableColumns(RawDataSorted);
            SortDataCareData();
            void SortDataCareData()
            {
                string acCode, acName, acAdd, acCity, acPhone, acMobile, acMobile2, acPanNO, acAdhaRNO;
                int count = 0;
                foreach (DataRow row in RawData.Rows)
                {
                    count++;
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
                        acCode = row["AC_CODE"].ToString();
                        acName = row["AC_NAME"].ToString();
                        if (tableName == "datacare")
                        {
                            acAdd = row["AC_ADD1"].ToString() + "   " + row["AC_ADD2"].ToString() + "   " + row["AC_ADD3"].ToString() + "   " + row["AC_PIN"].ToString();
                        }
                        else
                        {
                            acAdd = row["AC_ADD1"].ToString();
                        }
                        acCity = row["AC_CITY"].ToString();
                        acPhone = row["AC_PHONE"].ToString();
                        acMobile = row["AC_MOBILE"].ToString();
                        acMobile2 = row["AC_MOBILE2"].ToString();
                        acPanNO = row["AC_PANNO"].ToString();
                        acAdhaRNO = row["AC_ADHARNO"].ToString();
                    }
                    void addRow(string mobileField)
                    {
                        DataRow newRow = RawDataSorted.NewRow();
                        newRow["acCode"] = row["AC_CODE"].ToString();
                        newRow["acName0"] = row["AC_NAME"].ToString();
                        if (tableName == "datacare")
                        {
                            newRow["acAdd"] = row["AC_ADD1"].ToString() + "   " + row["AC_ADD2"].ToString() + "   " + row["AC_ADD3"].ToString() + "   " + row["AC_PIN"].ToString();
                        }
                        else
                        {
                            newRow["acAdd"] = row["AC_ADD1"].ToString();
                        }
                        newRow["acCity"] = row["AC_CITY"].ToString();
                        newRow["acMobile"] = row[mobileField].ToString();
                        newRow["acPan"] = row["AC_PANNO"].ToString();
                        newRow["acAdhaar"] = row["AC_ADHARNO"].ToString();
                        RawDataSorted.Rows.Add(newRow);
                    }
                }
            }
            checkNumber(RawDataSorted);
            compare(RawDataSorted);
        }
        bool compareInTable(DataRow row_RawData, DataTable dt, bool addInUnverifiedData, string namee)
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
        private void compare(DataTable dt)
        {
            UpdateLogForm logForm = new UpdateLogForm();
            logForm.Show();  // Open the log form
            DataRow[] dtRows = dt.Select();
            for (int rowIndex = 0; rowIndex < dtRows.Length; rowIndex++)
            {
                if (compareInTable(dtRows[rowIndex], ExcludedData, false, "ExcludedData")) { }
                else if (compareInTable(dtRows[rowIndex], CustomerData, false, "CustomerData")) { }
                else if (compareInTable(dtRows[rowIndex], RelativeData, false, "RelativeData")) { }
                else if (compareInTable(dtRows[rowIndex], KarigarData, false, "KarigarData")) { }
                else if (compareInTable(dtRows[rowIndex], VepariData, false, "VepariData")) { }
                else if (compareInTable(dtRows[rowIndex], OtherData, false, "OtherData")) { }
                else if (compareInTable(dtRows[rowIndex], WrongData, false, "WrongData")) { }
                else if (compareInTable(dtRows[rowIndex], UnverifiedData, true, "UnverifiedData")) { }
                UnverifiedData.ImportRow(dtRows[rowIndex]);
            }
            dt.AcceptChanges();
            ExcludedData.AcceptChanges();
            CustomerData.AcceptChanges();
            RelativeData.AcceptChanges();
            KarigarData.AcceptChanges();
            VepariData.AcceptChanges();
            OtherData.AcceptChanges();
            WrongData.AcceptChanges();
            UnverifiedData.AcceptChanges();
            addDatatableToDatabase(ExcludedData, "ExcludedData", logForm);
            addDatatableToDatabase(CustomerData, "CustomerData", logForm);
            addDatatableToDatabase(RelativeData, "RelativeData", logForm);
            addDatatableToDatabase(KarigarData, "KarigarData", logForm);
            addDatatableToDatabase(VepariData, "VepariData", logForm);
            addDatatableToDatabase(OtherData, "OtherData", logForm);
            addDatatableToDatabase(WrongData, "WrongData", logForm);
            addDatatableToDatabase(UnverifiedData, "UnverifiedData", logForm);
            printDT(OtherData);
        }
        public void addDatatableToDatabase(DataTable dt, string tableName, UpdateLogForm logForm)
        {
            // Remove all data from the table
            string deleteQuery = $"DELETE FROM {tableName}";
            helper.RunQueryWithoutParametersSYAContactDataBase(deleteQuery, "ExecuteNonQuery");
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
                bool insertSuccess = helper.RunQueryWithParametersSYAContactDataBase(insertQuery, parameters);
                if (!insertSuccess)
                {
                    logForm.AddLog($"Failed to insert data into {tableName} table.");
                    return;
                }
            }
            logForm.AddLog($"Data successfully inserted into {tableName} table.");
        }
    }
}
