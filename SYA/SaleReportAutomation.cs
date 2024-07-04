using Humanizer;
using SYA.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYA
{
    public class SaleReportAutomation
    {
        public DataTable raw_pdf_data = new DataTable();
        public DataTable sl_data = new DataTable();
        public DataTable amount_count = new DataTable();
        public DataTable match_data_result = new DataTable();
        public DataTable final = new DataTable();
        public void main_fnc()
        {
            get_raw_pdf_data();
            get_sl_data();
            get_amount_count();
            set_column_result();
            find_matching_data();
            set_column_final();
            check_data();
        }
        void get_raw_pdf_data()
        {
            raw_pdf_data = helper.FetchDataTableFromSYADataBase("select * from RAW_PDF_DATA");
          //  adjust_date_format(raw_pdf_data, "DATE", "dd-MM-yy");
        }
        void get_sl_data()
        {
            DateTime startDate = new DateTime(2024, 6, 1);
            DateTime endDate = new DateTime(2024, 6, 30);
            string startDateString = startDate.ToString("MM/dd/yyyy");
            string endDateString = endDate.ToString("MM/dd/yyyy");
            string query = @"
                SELECT VCH_NO AS BILL_NO, CO_YEAR, VCH_DATE AS BILL_DATE, CASH_AMT, CARD_AMT, CHQ_AMT
                FROM SL_DATA
                WHERE VCH_DATE >= #" + startDateString + "# AND VCH_DATE <= #" + endDateString + "#";
            sl_data = helper.FetchDataTableFromDataCareDataBase(query);
            adjust_date_format(sl_data, "BILL_DATE", "dd/MM/yyyy");
        }
        void get_amount_count()
        {
            string query = "SELECT AMOUNT, COUNT(*) AS COUNT FROM RAW_PDF_DATA    GROUP BY AMOUNT   ORDER BY COUNT";
            amount_count = helper.FetchDataTableFromSYADataBase(query);
        }
        void set_column_result()
        {
            match_data_result.Columns.Add("srno", typeof(string));
            match_data_result.Columns.Add("raw_date", typeof(string));
            match_data_result.Columns.Add("name", typeof(string));
            match_data_result.Columns.Add("raw_amount", typeof(string));
            match_data_result.Columns.Add("tr_typr", typeof(string));
            match_data_result.Columns.Add("bill_no", typeof(string));
            match_data_result.Columns.Add("co_year", typeof(string));
            match_data_result.Columns.Add("bill_date", typeof(string));
            match_data_result.Columns.Add("cash_amount", typeof(string));
            match_data_result.Columns.Add("card_amount", typeof(string));
            match_data_result.Columns.Add("cheque_amount", typeof(string));
        }
        void set_column_final()
        {
            final.Columns.Add("srno", typeof(string));
            final.Columns.Add("raw_date", typeof(string));
            final.Columns.Add("name", typeof(string));
            final.Columns.Add("raw_amount", typeof(string));
            final.Columns.Add("tr_typr", typeof(string));
            final.Columns.Add("status", typeof(string));
            final.Columns.Add("error", typeof(string));

        }
        void find_matching_data()
        {
            string amount = "";
            foreach (DataRow rawdt in raw_pdf_data.Rows)
            {
                // Extract amount as string
                amount = rawdt["AMOUNT"].ToString();

                // Parse amount to double
                if (Double.TryParse(amount, out double parsedAmount))
                {
                    // Iterate through each row in sl_data
                    foreach (DataRow sl_data_row in sl_data.Rows)
                    {
                        // Parse CASH_AMT to double
                        if ((Double.TryParse(sl_data_row["CASH_AMT"].ToString(), out double cashAmt) && parsedAmount == cashAmt) ||
                            (Double.TryParse(sl_data_row["CARD_AMT"].ToString(), out double cardAmt) && parsedAmount == cardAmt) ||
                            (Double.TryParse(sl_data_row["CHQ_AMT"].ToString(), out double chequeAmt) && parsedAmount == chequeAmt))
                        {
                            MessageBox.Show(sl_data_row["BILL_DATE"].ToString());
                            // Add row to result DataTable
                            match_data_result.Rows.Add(rawdt["SRNO"].ToString(),
                                            rawdt["DATE"].ToString(),
                                            rawdt["NAME"].ToString(),  // Adjust column name as per your table structure
                                            rawdt["AMOUNT"].ToString(),
                                            rawdt["TR_TYPE"].ToString(),  // Adjust column name as per your table structure
                                            sl_data_row["BILL_NO"].ToString(),
                                            sl_data_row["CO_YEAR"].ToString(),
                                            sl_data_row["BILL_DATE"].ToString(),
                                            sl_data_row["CASH_AMT"].ToString(),
                                            sl_data_row["CARD_AMT"].ToString(),
                                            sl_data_row["CHQ_AMT"].ToString());
                        }

                        // Parse CARD_AMT to double

                    }
                }
            }

        }
        void check_data()
        {
            foreach (DataRow dt in amount_count.Rows)
            {
                if (Convert.ToInt32(dt["COUNT"]) == 1)
                {

                }
                else
                {
                }
            }
        }
        void adjust_date_format(DataTable dt, string columnname, string format)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string dateStr = dr[columnname].ToString();
                string parsedDate = "";

                // Parse date based on format
                if (format == "dd-MM-yy")
                {
                    //  MessageBox.Show("dateStr: " + dateStr);
                    parsedDate = parse_date_ddmmmyy(dateStr);
                }
                else if (format == "dd/MM/yyyy")
                {
                    parsedDate = parse_date_ddmmyyyy(dateStr);

                }

                // Inside adjust_date_format method
                if (!string.IsNullOrEmpty(parsedDate))
                {
                    // Update the DataRow with parsedDate
                    dr[columnname] = parsedDate;

                    // No need to call dt.AcceptChanges() here

                    // Debugging MessageBoxes
                    MessageBox.Show("Updated date for row " + dt.Rows.IndexOf(dr) + ": " + parsedDate);
                    MessageBox.Show("Date in DataTable after update: " + dt.Rows[dt.Rows.IndexOf(dr)][columnname]);

                    // Convert the DateTime in the DataTable to the desired display format
                    dt.Rows[dt.Rows.IndexOf(dr)][columnname] = dt.Rows[dt.Rows.IndexOf(dr)].Field<DateTime>(columnname).ToString("dd-MM-yy");
                    MessageBox.Show("Date in DataTable after update: " + dt.Rows[dt.Rows.IndexOf(dr)][columnname]);
                }
                else
                {
                    MessageBox.Show("Failed to parse date: " + dateStr);
                    // Handle the error accordingly
                }

            }
        }

        string parse_date_ddmmmyy(string date)
        {
            DateTime dt;
            if (DateTime.TryParseExact(date, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt.ToString("dd-MM-yy");
            }
            else
            {
                return ""; // Return empty string on failure
            }
        }

        string parse_date_ddmmyyyy(string date)
        {
            date = date.Trim();

            DateTime dt;
            // Define the formats to parse (including the possibility of time component)
            string[] formats = { "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy" };

            if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                // Return the date portion in the desired format
                return dt.ToString("dd-MM-yy");
            }
            else
            {
                return ""; // Return empty string on failure
            }
        }









    }
}
