using System.Data;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Text;
using SYA.Helper;
namespace SYA.Reparing
{
    public class reparing
    {
        List<string> reparingData = new List<string>();
        public void printReparingTag(List<string> data)
        {
            reparingData.Clear();
            reparingData = data;
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = helper.TagPrinterName;
                pd.PrintPage += new PrintPageEventHandler(p);
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void p(object sender, PrintPageEventArgs e)
        {
            PrintHelper.PrintReparing(sender, e, reparingData);
        }
    }
}
