
using System.Data;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Text;
namespace SYA
{
    public class reparing
    {
         List<string> reparingData = new List<string>();
        public void printReparingTag(List<string> data)
        {
            reparingData.Clear();
            reparingData=data;
            try
            {
                //PrintDocument pd = new PrintDocument();
                //pd.PrinterSettings.PrinterName = "HP LaserJet MFP M129-M134";
                //PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                //printPreviewDialog.Document = pd;
                //printPreviewDialog.WindowState = FormWindowState.Maximized;
                //printPreviewDialog.PrintPreviewControl.Zoom = 1.0;
                //pd.PrintPage += new PrintPageEventHandler(Print);
                //printPreviewDialog.ShowDialog();
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "TSC_TE244";
                pd.PrintPage += new PrintPageEventHandler(p);
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public  void p(object sender, PrintPageEventArgs e) {
            //reparingData.Add("Pratyush");//1
            //reparingData.Add("Rajpara");//1
            //reparingData.Add("7600771961");//1reparingData.Add("V V NAGAR");//1
            //reparingData.Add("89.123");//1
            //reparingData.Add("2300");//1
            //reparingData.Add("2 number ni size nu karvu");//1
            print.PrintReparing(sender, e, reparingData);
        }
    }
}
