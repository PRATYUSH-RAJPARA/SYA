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
        public static void h()
        {
            
            DataTable dt = helper.FetchDataTableFromDataCareDataBase(getMonths);
            DisplayDataTableInMessageBox(dt);
            foreach (DataRow row in dt.Rows)
            {
                string month = row["unique_months"].ToString();
                string getData = "SELECT SUM(NET_AMT),SUM(DISC_AMT),SUM(TAX_AMT),SUM(TOT_AMT),SUM(CASH_AMT),SUM(CARD_AMT),SUM(CHQ_AMT) FROM SL_DATA WHERE FORMAT(VCH_DATE, 'mm') = '" + month + "' AND CO_BOOK = '026'";
                string getData1 = "SELECT COUNT(VCH_NO) ,SUM(ITM_GWT) , SUM(ITM_NWT),AVG(ITM_RATE),AVG(LBR_RATE),SUM(LBR_AMT),SUM(OTH_AMT) FROM SL_DETL WHERE FORMAT(VCH_DATE, 'mm') = '" + month + "' AND CO_BOOK = '026'";
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
0

                DataTable dt1 = helper.FetchDataTableFromDataCareDataBase(getData);
                DataTable DT2 = helper.FetchDataTableFromDataCareDataBase(getData1);
             //   DataTable dt3 = helper.FetchDataTableFromDataCareDataBase(getData2);
             //   DisplayDataTableInMessageBox(dt1);
                DisplayDataTableInMessageBox(DT2 );
              //  DisplayDataTableInMessageBox(dt3);

            }
            s();
        }
           
        public static void DisplayDataTableInMessageBox(DataTable dataTable)
        {

            //// Append column names
            //foreach (DataColumn column in dataTable.Columns)
            //{
            //    sb.Append(column.ColumnName).Append("\t");
            //}
            //sb.AppendLine();

            // Append data rows
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
        public static void s() {
            string message = sb.ToString();
             MessageBox.Show(message);
        }
    }
}
