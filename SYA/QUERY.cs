using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SYA
{
    public class QUERY
    {
        static string getMonths = "SELECT DISTINCT FORMAT(VCH_DATE, 'mm') AS unique_months FROM SL_DATA;";
        public static StringBuilder sb = new StringBuilder();
        public static void GetSummaryData()
        {
            string CO_YEAR;
            string YEAR;
            string MONTH;
            string ITEM;
            string BILL_COUNT;
            string NET_AMT;
            string DISC_AMT;
            string TAX_AMT;
            string TOT_AMT;
            string CASH_AMT;
            string CARD_AMT;
            string CHQ_AMT;
            string ITEM_COUNT;
            string ITM_GWT;
            string ITM_NWT;
            string ITM_RATE;
            string _916_COUNT;
            string _18K_COUNT;
            string _20K_COUNT;
            string LBR_RATE;
            string LBR_AMT;
            string OTH_AMT;
             void DisplayVariablesInMessageBox()
            {
                StringBuilder SS = new StringBuilder();

                SS.AppendLine($"CO_YEAR: {CO_YEAR}");
                SS.AppendLine($"YEAR: {YEAR}");
                SS.AppendLine($"MONTH: {MONTH}");
                SS.AppendLine($"ITEM: {ITEM}");
                SS.AppendLine($"BILL_COUNT: {BILL_COUNT}");
                SS.AppendLine($"NET_AMT: {NET_AMT}");
                SS.AppendLine($"DIS_AMT: {DISC_AMT}");
                SS.AppendLine($"TAX_AMT: {TAX_AMT}");
                SS.AppendLine($"TOT_AMT: {TOT_AMT}");
                SS.AppendLine($"CASH_AMT: {CASH_AMT}");
                SS.AppendLine($"CARD_AMT: {CARD_AMT}");
                SS.AppendLine($"CHQ_AMT: {CHQ_AMT}");
                SS.AppendLine($"ITEM_COUNT: {ITEM_COUNT}");
                SS.AppendLine($"ITM_GWT: {ITM_GWT}");
                SS.AppendLine($"ITM_NWT: {ITM_NWT}");
                SS.AppendLine($"ITM_RATE: {ITM_RATE}");
                SS.AppendLine($"916_COUNT: {_916_COUNT}");
                SS.AppendLine($"18K_COUNT: {_18K_COUNT}");
                SS.AppendLine($"20K_COUNT: {_20K_COUNT}");
                SS.AppendLine($"LBR_RATE: {LBR_RATE}");
                SS.AppendLine($"LBR_AMT: {LBR_AMT}");
                SS.AppendLine($"OTH_AMT: {OTH_AMT}");

                MessageBox.Show(SS.ToString(), "Variable Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            DataTable dt = helper.FetchDataTableFromDataCareDataBase(getMonths);
            foreach (DataRow row in dt.Rows)
            {
                MessageBox.Show("As");
                DataTable SL_DATA_GOLD = getSL_DATA(row["unique_months"].ToString(), "026");
                MessageBox.Show("As1");
                DataTable SL_DETL_GOLD = getSL_DETL(row["unique_months"].ToString(), "026");
                MessageBox.Show("As2");
                setVar(SL_DATA_GOLD, SL_DETL_GOLD,"GOLD", row["unique_months"].ToString());
                MessageBox.Show("As3");
                DisplayVariablesInMessageBox();
                DataTable SL_DATA_SILVER = getSL_DATA(row["unique_months"].ToString(), "027");
                DataTable SL_DETL_SILVER = getSL_DETL(row["unique_months"].ToString(), "027");
                setVar(SL_DATA_SILVER, SL_DETL_SILVER, "SILVER", row["unique_months"].ToString());
                DisplayVariablesInMessageBox();

                string updateStatement = @"
    UPDATE Summary
    SET 
        CO_YEAR = @CO_YEAR,
        YEAR = @YEAR,
        MONTH = @MONTH,
        ITEM = @ITEM,
        NET_AMT = @NET_AMT,
        DIS_AMT = @DIS_AMT,
        TAX_AMT = @TAX_AMT,
        TOT_AMT = @TOT_AMT,
        CASH_AMT = @CASH_AMT,
        CARD_AMT = @CARD_AMT,
        CHQ_AMT = @CHQ_AMT,
        ITEM_COUNT = @ITEM_COUNT,
        ITM_GWT = @ITM_GWT,
        ITM_NWT = @ITM_NWT,
        ITM_RATE = @ITM_RATE,
        916_COUNT = @916_COUNT,
        18K_COUNT = @18K_COUNT,
        20K_COUNT = @20K_COUNT,
        LBR_RATE = @LBR_RATE,
        LBR_AMT = @LBR_AMT,
        OTH_AMT = @OTH_AMT
    WHERE 
        <condition>;
";
                string insertStatement = @"
    INSERT INTO Summary (
        CO_YEAR,
        YEAR,
        MONTH,
        ITEM,
        NET_AMT,
        DIS_AMT,
        TAX_AMT,
        TOT_AMT,
        CASH_AMT,
        CARD_AMT,
        CHQ_AMT,
        ITEM_COUNT,
        ITM_GWT,
        ITM_NWT,
        ITM_RATE,
        916_COUNT,
        18K_COUNT,
        20K_COUNT,
        LBR_RATE,
        LBR_AMT,
        OTH_AMT
    ) VALUES (
        @CO_YEAR,
        @YEAR,
        @MONTH,
        @ITEM,
        @NET_AMT,
        @DIS_AMT,
        @TAX_AMT,
        @TOT_AMT,
        @CASH_AMT,
        @CARD_AMT,
        @CHQ_AMT,
        @ITEM_COUNT,
        @ITM_GWT,
        @ITM_NWT,
        @ITM_RATE,
        @916_COUNT,
        @18K_COUNT,
        @20K_COUNT,
        @LBR_RATE,
        @LBR_AMT,
        @OTH_AMT
    );
";
            }
            void setVar(DataTable data,DataTable detl,string item,string month)
            {
                CO_YEAR = data.Rows[0]["CO_YEAR"].ToString();
                 YEAR = data.Rows[0]["CO_BOOK"].ToString();
                 MONTH = month;
                ITEM = item;
                BILL_COUNT = data.Rows[0]["BILL_COUNT"].ToString();
                 NET_AMT = data.Rows[0]["NET_AMT"].ToString();
                 DISC_AMT = data.Rows[0]["DISC_AMT"].ToString();
                 TAX_AMT = data.Rows[0]["TAX_AMT"].ToString();
                 TOT_AMT = data.Rows[0]["TOT_AMT"].ToString();
                 CASH_AMT = data.Rows[0]["CASH_AMT"].ToString();
                 CARD_AMT = data.Rows[0]["CARD_AMT"].ToString();
                 CHQ_AMT = data.Rows[0]["CHQ_AMT"].ToString();
                 ITEM_COUNT = detl.Rows[0]["VCH_NO"].ToString();
                 ITM_GWT = detl.Rows[0]["ITM_GWT"].ToString();
                 ITM_NWT = detl.Rows[0]["ITM_NWT"].ToString();
                 ITM_RATE = detl.Rows[0]["ITM_RATE"].ToString();
                _916_COUNT = "0";
                 _18K_COUNT = "0";
                _20K_COUNT = "0";
                 LBR_RATE = detl.Rows[0]["LBR_RATE"].ToString();
                 LBR_AMT = detl.Rows[0]["LBR_AMT"].ToString();
                 OTH_AMT = detl.Rows[0]["OTH_AMT"].ToString();
            }
            //DataTable getSL_DATA(string month, string co_book)
            //{
            //    string sql = "SELECT  (SELECT TOP 1 CO_YEAR FROM SL_DATA ) AS CO_YEAR,(SELECT TOP 1 CO_BOOK FROM SL_DATA ) AS CO_BOOK,COUNT(VCH_NO) AS BILL_COUNT, SUM(NET_AMT) AS NET_AMT,SUM(DISC_AMT) AS DISC_AMT,SUM(TAX_AMT) AS TAX_AMT,SUM(TOT_AMT) AS TOT_AMT,SUM(CASH_AMT) AS CASH_AMT,SUM(CARD_AMT) AS CARD_AMT,SUM(CHQ_AMT) AS CHQ_AMT FROM SL_DATA WHERE FORMAT(VCH_DATE, 'mm') = '" + month + "' AND CO_BOOK = '" + co_book + "'";
            //    return helper.FetchDataTableFromDataCareDataBase(sql);

            //}
            //DataTable getSL_DETL(string month, string co_book)
            //{
            //    string sql = "SELECT COUNT(VCH_NO) AS VCH_NO ,SUM(ITM_GWT) AS ITM_GWT , SUM(ITM_NWT) AS ITM_NWT,AVG(ITM_RATE) AS ITM_RATE,AVG(LBR_RATE) AS LBR_RATE,SUM(LBR_AMT) AS LBR_AMT,SUM(OTH_AMT) AS OTH_AMT FROM SL_DETL WHERE FORMAT(VCH_DATE, 'mm') = '" + month + "' AND CO_BOOK = '" + co_book + "'";
            //    return helper.FetchDataTableFromDataCareDataBase(sql);

            //}
            DataTable getSL_DATA(string month, string co_book)
            {
                string sql = @"
        SELECT  
            (SELECT TOP 1 CO_YEAR FROM SL_DATA) AS CO_YEAR,
            (SELECT TOP 1 CO_BOOK FROM SL_DATA) AS CO_BOOK,
            COUNT(VCH_NO) AS BILL_COUNT,
            SUM(NET_AMT) AS NET_AMT,
            SUM(DISC_AMT) AS DISC_AMT,
            SUM(TAX_AMT) AS TAX_AMT,
            SUM(TOT_AMT) AS TOT_AMT,
            SUM(CASH_AMT) AS CASH_AMT,
            SUM(CARD_AMT) AS CARD_AMT,
            SUM(CHQ_AMT) AS CHQ_AMT 
        FROM 
            SL_DATA 
        WHERE 
            FORMAT(VCH_DATE, 'mm') = '" + month + "' AND CO_BOOK = '" + co_book + "'";
                return helper.FetchDataTableFromDataCareDataBase(sql);
            }

            DataTable getSL_DETL(string month, string co_book)
            {
                string sql = @"
        SELECT 
            COUNT(VCH_NO) AS VCH_NO,
            SUM(ITM_GWT) AS ITM_GWT,
            SUM(ITM_NWT) AS ITM_NWT,
            AVG(IIF(ITM_RATE <> 0, ITM_RATE, NULL)) AS ITM_RATE,
            AVG(IIF(LBR_RATE <> 0, LBR_RATE, NULL)) AS LBR_RATE,
            SUM(LBR_AMT) AS LBR_AMT,
            SUM(OTH_AMT) AS OTH_AMT 
        FROM 
            SL_DETL 
        WHERE 
            FORMAT(VCH_DATE, 'mm') = '" + month + "' AND CO_BOOK = '" + co_book + "'";
                return helper.FetchDataTableFromDataCareDataBase(sql);
            }

            ShowMsg();
        }

        public static void DisplayDataTableInMessageBox(DataTable dataTable)
        {


            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    sb.Append(item).Append("\t");
                }
                sb.AppendLine();
            }

            // Display in a message box
            string message = sb.ToString();
            // MessageBox.Show(message);
        }
        public static void ShowMsg()
        {
            string message = sb.ToString();
            MessageBox.Show(message);
        }
    }
}
