using SYA.Helper;
using System.Data;
namespace SYA
{
    public class SaleReportAutomation : Form
    {
        public DataTable raw_pdf_data = new DataTable();
        public DataTable sl_data = new DataTable();
        public DataTable amount_count = new DataTable();
        public DataTable match_data_result = new DataTable();
        public DataTable final = new DataTable();
        public SaleReportAutomation()
        {
            // Initialize DataGridView
            // Initialize other components or variables if needed
        }
        public void main_fnc()
        {
            get_raw_pdf_data();
            get_sl_data();
            get_amount_count();
            set_column_result();
            set_column_final();
            start();
        }
        void get_raw_pdf_data()
        {
            raw_pdf_data = helper.FetchDataTableFromSYADataBase("select * from RAW_PDF_DATA where TR_TYPE = 'Cr'");
        }
        void get_sl_data()
        {
            DateTime startDate = new DateTime(2024, 6, 1);
            DateTime endDate = new DateTime(2024, 6, 30);
            string startDateString = startDate.ToString("MM/dd/yyyy");
            string endDateString = endDate.ToString("MM/dd/yyyy");
            string query = @"
                                SELECT VCH_NO AS BILL_NO, CO_YEAR, Format(VCH_DATE, 'dd-mm-yy') AS BILL_DATE, CASH_AMT, CARD_AMT, CHQ_AMT, AC_NAME
                                FROM SL_DATA
                                WHERE VCH_DATE >= #" + startDateString + "# AND VCH_DATE <= #" + endDateString + "#";
            sl_data = helper.FetchDataTableFromDataCareDataBase(query);
        }
        void get_amount_count()
        {
            string query = "SELECT AMOUNT, COUNT(*) AS COUNT FROM RAW_PDF_DATA  where TR_TYPE = 'Cr' GROUP BY AMOUNT ORDER BY COUNT";
            amount_count = helper.FetchDataTableFromSYADataBase(query);
        }
        void set_column_result()
        {
            match_data_result.Columns.Add("SRNO", typeof(string));
            match_data_result.Columns.Add("DATE", typeof(string));
            match_data_result.Columns.Add("NAME", typeof(string));
            match_data_result.Columns.Add("AMOUNT", typeof(string));
            match_data_result.Columns.Add("TR_TYPE", typeof(string));
            match_data_result.Columns.Add("CO_YEAR", typeof(string));
            match_data_result.Columns.Add("BILL_NO", typeof(string));
            match_data_result.Columns.Add("BILL_DATE", typeof(string));
            match_data_result.Columns.Add("CASH_AMT", typeof(string));
            match_data_result.Columns.Add("CARD_AMT", typeof(string));
            match_data_result.Columns.Add("CHQ_AMT", typeof(string));
            match_data_result.Columns.Add("AC_NAME", typeof(string));
        }
        void set_column_final()
        {
            final.Columns.Add("CO_YEAR", typeof(string));
            final.Columns.Add("SRNO", typeof(string));
            final.Columns.Add("bill_no", typeof(string));
            final.Columns.Add("BILL_DATE", typeof(string));
            final.Columns.Add("DATE", typeof(string));
            final.Columns.Add("NAME", typeof(string));
            final.Columns.Add("AC_NAME", typeof(string));
            final.Columns.Add("AMOUNT", typeof(string));
            final.Columns.Add("TR_TYPE", typeof(string));
            final.Columns.Add("CASH_AMT", typeof(string));
            final.Columns.Add("CARD_AMT", typeof(string));
            final.Columns.Add("CHQ_AMT", typeof(string));
            final.Columns.Add("NAMEMATCH", typeof(string));
            final.Columns.Add("CASHMATCH", typeof(string));
            final.Columns.Add("CARDMATCH", typeof(string));
            final.Columns.Add("CHQMATCH", typeof(string));
            final.Columns.Add("DATEMATCH", typeof(string));
            final.Columns.Add("STATUS", typeof(string));
            final.Columns.Add("ERROR", typeof(string));
        }
        string ExtractName(string input)
        {
            // Split the string by the '/' character
            string[] parts = input.Split('/');
            // Get the last part which contains the name
            if (parts.Length > 0)
            {
                return parts[parts.Length - 1];
            }
            return string.Empty; // Return an empty string if the input is not in the expected format
        }
        void start()
        {
            foreach (DataRow pdf_row in raw_pdf_data.Rows)
            {
                int SRNO = Convert.ToInt32(pdf_row["SRNO"]);
                string DATE = pdf_row["DATE"].ToString();
                string NAME = ExtractName(pdf_row["NAME"].ToString());
                decimal AMOUNT = Convert.ToDecimal(pdf_row["AMOUNT"]);
                DataRow[] result = amount_count.Select("AMOUNT = '" + (pdf_row["AMOUNT"].ToString() + "'"));
                decimal count_amount = 0;
                if (result.Length > 0)
                {
                    DataRow row = result[0];
                    count_amount = Convert.ToDecimal(row["COUNT"]);
                }
                string TR_TYPE = pdf_row["TR_TYPE"].ToString();
                foreach (DataRow sl_row in sl_data.Rows)
                {
                    string CO_YEAR = sl_row["CO_YEAR"].ToString();
                    string BILL_NO = sl_row["BILL_NO"].ToString();
                    string BILL_DATE = sl_row["BILL_DATE"].ToString();
                    decimal CASH_AMT = Convert.ToDecimal(sl_row["CASH_AMT"]);
                    decimal CARD_AMT = Convert.ToDecimal(sl_row["CARD_AMT"]);
                    decimal CHQ_AMT = Convert.ToDecimal(sl_row["CHQ_AMT"]);
                    string AC_NAME = sl_row["AC_NAME"].ToString();
                    bool NAMEMATCH = false;
                    bool CASHMATCH = false;
                    bool CARDMATCH = false;
                    bool CHQMATCH = false;
                    bool DATEMATCH = false;
                    void amount_name_date_match(decimal amt)
                    {
                        if (NAME == AC_NAME)
                        {
                            NAMEMATCH = true;
                        }
                        if (amt == CASH_AMT)
                        {
                            CASHMATCH = true;
                        }
                        if (amt == CARD_AMT)
                        {
                            CARDMATCH = true;
                        }
                        if (amt == CHQ_AMT)
                        {
                            CHQMATCH = true;
                        }
                        if (DATE == BILL_DATE)
                        {
                            DATEMATCH = true;
                        }
                    }
                    if (count_amount == 1)
                    {
                        amount_name_date_match(AMOUNT);
                    }
                    else
                    {
                        amount_name_date_match(AMOUNT);
                    }
                    if (DATEMATCH && (CARDMATCH || CASHMATCH || CHQMATCH))
                    {
                        final.Rows.Add(
                            CO_YEAR,
                            SRNO,
                            BILL_NO,
                            BILL_DATE,
                            DATE,
                            NAME,
                            AC_NAME,
                            AMOUNT,
                            TR_TYPE,
                            CASH_AMT,
                            CARD_AMT,
                            CHQ_AMT,
                            NAMEMATCH.ToString(),
                            CASHMATCH.ToString(),
                            CARDMATCH.ToString(),
                            CHQMATCH.ToString(),
                            DATEMATCH.ToString(),
                            NAMEMATCH && CASHMATCH && CARDMATCH && CHQMATCH && DATEMATCH ? "Matched" : "Not Matched",
                            "hi"
                        );
                    }
                    else if (CARDMATCH || CASHMATCH || CHQMATCH)
                    {
                        final.Rows.Add(
                            CO_YEAR,
                            SRNO,
                            BILL_NO,
                            BILL_DATE,
                            DATE,
                            NAME,
                            AC_NAME,
                            AMOUNT,
                            TR_TYPE,
                            CASH_AMT,
                            CARD_AMT,
                            CHQ_AMT,
                            NAMEMATCH.ToString(),
                            CASHMATCH.ToString(),
                            CARDMATCH.ToString(),
                            CHQMATCH.ToString(),
                            DATEMATCH.ToString(),
                            NAMEMATCH && CASHMATCH && CARDMATCH && CHQMATCH && DATEMATCH ? "Matched" : "Not Matched",
                            "hello"
                        );
                    }
                }
            }
        }
        void find_matching_data()
        {
            string amount = "";
            foreach (DataRow rawdt in raw_pdf_data.Rows)
            {
                bool match = false;
                // Extract amount as string
                amount = rawdt["AMOUNT"].ToString();
                // Parse amount to double
                if (Double.TryParse(amount, out double parsedAmount))
                {
                    // Iterate through each row in sl_data
                    foreach (DataRow sl_data_row in sl_data.Rows)
                    {
                        string str = "";
                        // Parse CASH_AMT to double
                        if ((Double.TryParse(sl_data_row["CASH_AMT"].ToString(), out double cashAmt) && parsedAmount == cashAmt) ||
                            (Double.TryParse(sl_data_row["CARD_AMT"].ToString(), out double cardAmt) && parsedAmount == cardAmt) ||
                            (Double.TryParse(sl_data_row["CHQ_AMT"].ToString(), out double chequeAmt) && parsedAmount == chequeAmt))
                        {
                            DataRow[] result = match_data_result.Select("bill_no = '" + sl_data_row["BILL_NO"].ToString() + "' AND CO_YEAR = '" + sl_data_row["CO_YEAR"].ToString() + "' AND BILL_DATE = '" + sl_data_row["BILL_DATE"].ToString() + "' AND CASH_AMT = '" + sl_data_row["CASH_AMT"].ToString() + "' AND CARD_AMT = '" + sl_data_row["CARD_AMT"].ToString() + "' AND CHQ_AMT = '" + sl_data_row["CHQ_AMT"].ToString() + "' AND AC_NAME = '" + sl_data_row["AC_NAME"].ToString() + "'");
                            if (result.Length != 0)
                            {
                                match_data_result.Rows.Remove(result[0]);
                                match_data_result.AcceptChanges();
                            }
                            str += "1";
                            match = true;
                            // Add row to result DataTable
                            match_data_result.Rows.Add(
                                rawdt["SRNO"].ToString(),
                                rawdt["DATE"].ToString(),
                                rawdt["NAME"].ToString(),
                                rawdt["AMOUNT"].ToString(),
                                rawdt["TR_TYPE"].ToString(),
                                sl_data_row["BILL_NO"].ToString(),
                                sl_data_row["CO_YEAR"].ToString(),
                                sl_data_row["BILL_DATE"].ToString(),
                                sl_data_row["CASH_AMT"].ToString(),
                                sl_data_row["CARD_AMT"].ToString(),
                                sl_data_row["CHQ_AMT"].ToString(),
                                sl_data_row["AC_NAME"].ToString()
                            );
                        }
                        else
                        {
                            DataRow[] result = match_data_result.Select("bill_no = '" + sl_data_row["BILL_NO"].ToString() + "' AND CO_YEAR = '" + sl_data_row["CO_YEAR"].ToString() + "' AND BILL_DATE = '" + sl_data_row["BILL_DATE"].ToString() + "' AND CASH_AMT = '" + sl_data_row["CASH_AMT"].ToString() + "' AND CARD_AMT = '" + sl_data_row["CARD_AMT"].ToString() + "' AND CHQ_AMT = '" + sl_data_row["CHQ_AMT"].ToString() + "' AND AC_NAME = '" + sl_data_row["AC_NAME"].ToString() + "'");
                            if (result.Length == 0)
                            {
                                str += "2";
                                match_data_result.Rows.Add(
                                    "",
                                    "",
                                    "",
                                    "0",
                                    "",
                                    sl_data_row["BILL_NO"].ToString(),
                                    sl_data_row["CO_YEAR"].ToString(),
                                    sl_data_row["BILL_DATE"].ToString(),
                                    sl_data_row["CASH_AMT"].ToString(),
                                    sl_data_row["CARD_AMT"].ToString(),
                                    sl_data_row["CHQ_AMT"].ToString(),
                                    sl_data_row["AC_NAME"].ToString()
                                );
                            }
                        }
                    }
                }
                if (!match)
                {
                    DataRow[] result = match_data_result.Select("AMOUNT = '" + rawdt["AMOUNT"].ToString() + "' AND SRNO = '" + rawdt["SRNO"].ToString() + "' AND DATE = '" + rawdt["DATE"].ToString() + "' AND TR_TYPE = '" + rawdt["TR_TYPE"].ToString() + "' AND NAME = '" + rawdt["NAME"].ToString() + "'");
                    if (result.Length == 0)
                    {
                        match_data_result.Rows.Add(
                            rawdt["SRNO"].ToString(),
                            rawdt["DATE"].ToString(),
                            rawdt["NAME"].ToString(),
                            rawdt["AMOUNT"].ToString(),
                            rawdt["TR_TYPE"].ToString(),
                            "",
                            "",
                            "",
                            "0",
                            "0",
                            "0",
                            ""
                        );
                    }
                }
            }
        }
        void check_data()
        {
            int count = 0;
            int count1 = 0;
            int count2 = 0;
            string str = "";
            foreach (DataRow dt in amount_count.Rows)
            {
                count++;
                if (dt["COUNT"].ToString() == "1")
                {
                    count1++;
                    string amount_condition = "AMOUNT = '" + dt["AMOUNT"].ToString() + "'";
                    DataRow[] result = match_data_result.Select(amount_condition);
                    if (result.Length > 0)
                    {
                        for (int i = 0; i < result.Length; i++)
                        {
                            count2++;
                            DataRow row = result[i];
                            // Convert amounts to decimal
                            decimal rawAmount = Convert.ToDecimal(row["AMOUNT"]);
                            decimal cashAmount = Convert.ToDecimal(row["CASH_AMT"]);
                            decimal cardAmount = Convert.ToDecimal(row["CARD_AMT"]);
                            decimal chequeAmount = Convert.ToDecimal(row["CHQ_AMT"]);
                            bool NAMEMATCH = false;
                            bool CASHMATCH = false;
                            bool CARDMATCH = false;
                            bool CHQMATCH = false;
                            bool DATEMATCH = false;
                            if (row["DATE"].ToString() == row["BILL_DATE"].ToString())
                            {
                                DATEMATCH = true;
                            }
                            if (row["NAME"].ToString() == row["AC_NAME"].ToString())
                            {
                                NAMEMATCH = true;
                            }
                            if (rawAmount == cashAmount)
                            {
                                CASHMATCH = true;
                            }
                            if (rawAmount == cardAmount)
                            {
                                CARDMATCH = true;
                            }
                            if (rawAmount == chequeAmount)
                            {
                                CHQMATCH = true;
                            }
                            final.Rows.Add(
                                row["CO_YEAR"].ToString(),
                                row["SRNO"].ToString(),
                                row["bill_no"].ToString(),
                                row["BILL_DATE"].ToString(),
                                row["DATE"].ToString(),
                                row["NAME"].ToString(),
                                row["AC_NAME"].ToString(),
                                row["AMOUNT"].ToString(),
                                row["TR_TYPE"].ToString(),
                                row["CASH_AMT"].ToString(),
                                row["CARD_AMT"].ToString(),
                                row["CHQ_AMT"].ToString(),
                                NAMEMATCH.ToString(),
                                CASHMATCH.ToString(),
                                CARDMATCH.ToString(),
                                CHQMATCH.ToString(),
                                DATEMATCH.ToString(),
                                NAMEMATCH && CASHMATCH && CARDMATCH && CHQMATCH && DATEMATCH ? "Matched" : "Not Matched",
                                ""
                            );
                        }
                    }
                    else
                    {
                        amount_condition = "AMOUNT = '" + dt["AMOUNT"].ToString() + "'";
                        DataRow[] result1 = raw_pdf_data.Select(amount_condition);
                        if (result1.Length > 0)
                        {
                            count2++;
                            DataRow row = result1[0];
                            final.Rows.Add(
                                "",
                                row["SRNO"].ToString(),
                                "",
                                "",
                                row["DATE"].ToString(),
                                row["NAME"].ToString(),
                                "",
                                row["AMOUNT"].ToString(),
                                row["TR_TYPE"].ToString(),
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                "",
                                "Not Found",
                                ""
                            );
                        }
                    }
                }
                else
                {
                    // Implement logic for count != 1
                }
            }
            MessageBox.Show(count + " " + count1 + " " + count2 + "   " + str);
        }
    }
}
