using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYA
{
    internal class Contact
    {
        DataTable RawData = new DataTable();
        DataTable ExcludedData = new DataTable();
        DataTable VerifiedCustomerData = new DataTable();
        DataTable OtherVerifiedData = new DataTable();
        DataTable UnverifiedData = new DataTable();

        DataTable DataCareDataSorted = new DataTable();
        public void ParentDataCareData()
        {
            DataTable DataCareData = new DataTable();
            initDataCareDataSorted();

            void initDataCareDataSorted()
            {
                DataCareDataSorted.Columns.Add("acCode", typeof(string));
                DataCareDataSorted.Columns.Add("acName", typeof(string));
                DataCareDataSorted.Columns.Add("acAdd", typeof(string));
                DataCareDataSorted.Columns.Add("acCity", typeof(string));
                DataCareDataSorted.Columns.Add("acMobile", typeof(string));
                DataCareDataSorted.Columns.Add("acPan", typeof(string));
                DataCareDataSorted.Columns.Add("acAdhaar", typeof(string));
            }
            void SortDataCareData()
            {
                // ---------------------------------------------------------------

                string acCode, acName, acAdd, acCity, acPhone, acMobile, acMobile2, acPanNO, acAdhaRNO;
                foreach (DataRow row in DataCareData.Rows)
                {
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
                    setVariables();
                    void addRow(string mobileField)
                    {
                        DataRow newRow = DataCareDataSorted.NewRow();
                        newRow["acCode"] = row.Field<string>("AC_CODE");
                        newRow["acName"] = row.Field<string>("AC_NAME");
                        newRow["acAdd"] = row.Field<string>("AC_ADD1") + "   " + row.Field<string>("AC_ADD2") + "   " + row.Field<string>("AC_ADD3") + "   " + row.Field<string>("AC_PIN");
                        newRow["acCity"] = row.Field<string>("AC_CITY");
                        newRow["acMobile"] = row.Field<string>(mobileField);
                        newRow["acPan"] = row.Field<string>("AC_PANNO");
                        newRow["acAdhaar"] = row.Field<string>("AC_ADHARNO");
                        DataCareDataSorted.Rows.Add(newRow);
                    }
                    if (!string.IsNullOrEmpty(acPhone))
                    {
                        addRow("acPhone");
                    }
                    if (!string.IsNullOrEmpty(acMobile))
                    {
                        addRow("acMobile");
                    }
                    if (!string.IsNullOrEmpty(acMobile2))
                    {
                        addRow("acMobile2");
                    }
                }

                // ---------------------------------------------------------------

                foreach (DataRow row in DataCareData.Rows)
                {
                    string acName = row["AC_NAME"].ToString();
                    string acPhone = row["AC_PHONE"].ToString();
                    string acMobile = row["AC_MOBILE"].ToString();
                    string acMobile2 = row["AC_MOBILE2"].ToString();

                    if (DataCareDataSorted.AsEnumerable().Any(DataCareDataSorted => DataCareDataSorted.Field<string>("number") == acPhone))
                    {

                    }
                    else if (DataCareDataSorted.AsEnumerable().Any(DataCareDataSorted => DataCareDataSorted.Field<string>("AC_NAME") == acName))
                    {

                    }


                    if (DataCareDataSorted.AsEnumerable().Any(DataCareDataSorted => DataCareDataSorted.Field<string>("AC_NAME") == acName))
                    {

                    }
                    else
                    {
                        DataCareDataSorted.ImportRow(row);
                    }
                }
            }
        }

        private void SortDataCareData()
        {
            // ---------------------------------------------------------------

            foreach (DataRow row in DataCareData.Rows)
            {
                string name = row.Field<string>("name");
                string mobile1 = row.Field<string>("mobile1");
                string mobile2 = row.Field<string>("mobile2");
                string mobile3 = row.Field<string>("mobile3");

                if (!string.IsNullOrEmpty(mobile1))
                {
                    DataRow newRow = DataCareDataSorted.NewRow(); // Create a new DataRow for the DataCareDataSorted DataTable

                    newRow["acCode"] = "123";
                    newRow["acName"] = "John Doe";
                    newRow["acAdd"] = "123 Main St";
                    newRow["acCity"] = "New York";
                    newRow["acMobile"] = "555-1234";
                    newRow["acPan"] = "ABCDE1234F";
                    newRow["acAdhaar"] = "123456789012";

                    DataCareDataSorted.Rows.Add(newRow);
                }
                if (!string.IsNullOrEmpty(mobile2))
                {
                    table2.Rows.Add(name, mobile2);
                }
                if (!string.IsNullOrEmpty(mobile3))
                {
                    table2.Rows.Add(name, mobile3);
                }
            }

            // ---------------------------------------------------------------

            foreach (DataRow row in DataCareData.Rows)
            {
                string acName = row["AC_NAME"].ToString();
                string acPhone = row["AC_PHONE"].ToString();
                string acMobile = row["AC_MOBILE"].ToString();
                string acMobile2 = row["AC_MOBILE2"].ToString();

                if (DataCareDataSorted.AsEnumerable().Any(DataCareDataSorted => DataCareDataSorted.Field<string>("number") == acPhone))
                {

                }
                else if (DataCareDataSorted.AsEnumerable().Any(DataCareDataSorted => DataCareDataSorted.Field<string>("AC_NAME") == acName))
                {

                }


                if (DataCareDataSorted.AsEnumerable().Any(DataCareDataSorted => DataCareDataSorted.Field<string>("AC_NAME") == acName))
                {

                }
                else
                {
                    DataCareDataSorted.ImportRow(row);
                }
            }
        }
        private void compare()
        {
            foreach (DataRow row in RawData.Rows)
            {
                string number = row["number"].ToString();
                if (ExcludedData.AsEnumerable().Any(excludedRow => excludedRow.Field<string>("number") == number))
                {
                    row.Delete();
                }
                else if (VerifiedCustomerData.AsEnumerable().Any(VerifiedCustomerData => VerifiedCustomerData.Field<string>("number") == number))
                {
                    if (VerifiedCustomerData.AsEnumerable().Any(VerifiedCustomerData => VerifiedCustomerData.Field<string>("name") == number))
                    {
                        row.Delete();
                    }
                    else
                    {
                        // Add that name from row data to name field in respective data
                    }
                }
                else if (OtherVerifiedData.AsEnumerable().Any(OtherVerifiedData => OtherVerifiedData.Field<string>("number") == number))
                {
                    if (OtherVerifiedData.AsEnumerable().Any(OtherVerifiedData => OtherVerifiedData.Field<string>("name") == number))
                    {
                        row.Delete();
                    }
                    else
                    {
                        // Add that name from row data to name field in respective data
                    }
                }
                else
                {
                    UnverifiedData.ImportRow(row);
                }
            }
            RawData.AcceptChanges();

        }
    }

}
