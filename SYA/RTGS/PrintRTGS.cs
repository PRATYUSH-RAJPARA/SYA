using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public partial class PrintRTGS : Form
    {
        public PrintRTGS()
        {
            InitializeComponent();
        }

        private void PrintRTGS_Load(object sender, EventArgs e)
        {

        }

        private void PrintLabels()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "HP LaserJet MFP M129-M134";

                // Create a PrintPreviewDialog instance
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = pd;

                // Set the print preview dialog window state to maximized
                printPreviewDialog.WindowState = FormWindowState.Maximized;

                // Set the zoom level to 200%
                printPreviewDialog.PrintPreviewControl.Zoom = 2.5;

                // Hook up event handler for generating print page
                pd.PrintPage += new PrintPageEventHandler(Print);

                // Show the print preview dialog
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Print(object sender, PrintPageEventArgs e)
        {
            print.PrintRTGS(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RTGSList secondForm = new RTGSList();

            // Set the form's StartPosition property to CenterScreen to open it in the center of the screen
            secondForm.StartPosition = FormStartPosition.CenterScreen;

            // Show the form
            secondForm.Show();
            PrintLabels();
        }
    }
}
