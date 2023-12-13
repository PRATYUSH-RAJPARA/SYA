
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Windows.Forms;
using QRCoder;
using Serilog;

namespace SYA
{
    public partial class Search : Form
    {

        public Search()
        {
            InitializeComponent();
            gridviewstyle();
        }
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()

                .WriteTo.File("C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\LOG\\logs_tagno.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
                .CreateLogger();
        }
        private void Search_Load(object sender, EventArgs e)
        {

            string query = "SELECT * FROM MAIN_DATA ORDER BY VCH_DATE DESC;";
            LoadDataFromSQLite(query);
            MessageBox.Show("pratyush3");
            setCellVisiable();
            MessageBox.Show("pratyush4");
            InitializeLogging();
            MessageBox.Show("pratyush5");

        }
        private void setCellVisiable()
        {



            foreach (DataGridViewRow row in dataGridViewSearch.Rows)
            {

                if ((row.Cells["vchno"].Value?.ToString() ?? "0") == "SYA00")
                {
                    row.Cells["tagno"].ReadOnly = true;
                    row.Cells["vchno"].ReadOnly = true;
                    row.Cells["vchdate"].ReadOnly = true;
                    row.Cells["itemdesc"].ReadOnly = true;
                    row.Cells["gross"].ReadOnly = true;
                    row.Cells["net"].ReadOnly = true;



                }

            }

            return;


        }
        private void LoadDataFromSQLite(String query)
        {
            try
            {

                using (SQLiteDataReader reader = helper.FetchDFromSYADataBase(query))
                {
                    if (reader != null)
                    {
                        // Set the DataSource to null or an empty data source
                        dataGridViewSearch.DataSource = null;
                        // Optionally clear any existing rows
                        dataGridViewSearch.Rows.Clear();
                        // Optionally clear any existing columns



                        while (reader.Read())
                        {
                            int rowIndex = dataGridViewSearch.Rows.Add();
                            dataGridViewSearch.Rows[rowIndex].Cells["tagno"].Value = reader["TAG_NO"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["vchno"].Value = reader["VCH_NO"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["vchdate"].Value = reader["VCH_DATE"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["itemdesc"].Value = reader["ITEM_PURITY"].ToString() + "  -  " + reader["ITEM_DESC"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["gross"].Value = reader["GW"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["net"].Value = reader["NW"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["huid1"].Value = reader["HUID1"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["huid2"].Value = reader["HUID2"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["labour"].Value = reader["LABOUR_AMT"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["wholeLabour"].Value = reader["WHOLE_LABOUR_AMT"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["other"].Value = reader["OTHER_AMT"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["size"].Value = reader["size"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["price"].Value = reader["price"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["comment"].Value = reader["COMMENT"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Save Button 
        private void btnSearchSave_Click(object sender, EventArgs e)
        {
            SaveDataToSQLite();
        }
        private bool SaveDataToSQLite()
        {
            string tagNo = string.Empty;
            string gross = string.Empty;
            string net = string.Empty;
            string labour = string.Empty;
            string wholeLabour = string.Empty;
            string other = string.Empty;
            string huid1 = string.Empty;
            string huid2 = string.Empty;
            string size = string.Empty;
            string price = string.Empty;
            string comment = string.Empty;

            try
            {


                foreach (DataGridViewRow row in dataGridViewSearch.Rows)
                {
                    tagNo = row.Cells["tagno"].Value?.ToString() ?? string.Empty;
                    gross = row.Cells["gross"].Value?.ToString() ?? "0";
                    net = row.Cells["net"].Value?.ToString() ?? "0";
                    labour = row.Cells["labour"].Value?.ToString() ?? "0";
                    wholeLabour = row.Cells["wholeLabour"].Value?.ToString() ?? "0";
                    other = row.Cells["other"].Value?.ToString() ?? "0";
                    huid1 = row.Cells["huid1"].Value?.ToString()?.ToUpper() ?? string.Empty;
                    huid2 = row.Cells["huid2"].Value?.ToString()?.ToUpper() ?? string.Empty;
                    size = row.Cells["size"].Value?.ToString() ?? string.Empty;
                    price = row.Cells["price"].Value?.ToString() ?? "0";
                    comment = row.Cells["comment"].Value?.ToString() ?? string.Empty;

                    helper.validateHUID(huid1, huid2);

                    string updateQuery = $"UPDATE MAIN_DATA SET HUID1 = '{huid1}', HUID2 = '{huid2}', COMMENT = '{comment}',LABOUR_AMT = '{labour}',OTHER_AMT = '{other}',WHOLE_LABOUR_AMT = '{wholeLabour}', GW = '{gross}',NW = '{net}',SIZE = '{size}',PRICE = '{price}',COMMENT = '{comment}'  WHERE TAG_NO = '{tagNo}'";

                    helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                }
                // pratyush uncomment later
                // MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to SQLite. " +
                                $"TagNo: {tagNo}, Gross: {gross}, Net: {net}, " +
                                $"Labour: {labour}, WholeLabour: {wholeLabour}, " +
                                $"Other: {other}, HUID1: {huid1}, HUID2: {huid2}, " +
                                $"Size: {size}, Price: {price}, Comment: {comment}. " +
                                $"Error: {ex.Message} \n\nStackTrace: {ex.StackTrace}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show($"Error saving data to SQLite. TagNo: {tagNo}, Gross: {gross}, Net: {net}, Labour: {labour}, WholeLabour: {wholeLabour}, Other: {other}, HUID1: {huid1}, HUID2: {huid2}, Size: {size}, Price: {price}, Comment: {comment}. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //Print Button
        private void btnSearchPrintSelectedTagPrintTag_Click(object sender, EventArgs e)
        {
            PrintLabels();
        }

        private void PrintLabels()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = "TSC_TE244";

                if (true)
                {

                    pd.PrintPage += new PrintPageEventHandler(PrintPageGold);
                }
                else
                {

                    pd.PrintPage += new PrintPageEventHandler(PrintPageSilver925);
                }
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void PrintPageGold(object sender, PrintPageEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;

            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                if (tagNumber.Length > 1)
                {


                    Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Set the starting position for printing
                    float xPos = 0; // Adjust the starting X position
                    float yPos = 0; // Adjust the starting Y position

                    // Get the printer DPI
                    float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;

                    float rectX = 4; // Adjust the X position of the rectangle
                    float rectY = 4; // Adjust the Y position of the rectangle
                    float rectWidth = 211; // Adjust the width of the rectangle
                    float rectHeight = 45; // Adjust the height of the rectangle

                    //gross weight
                    //net weight
                    if ((selectedRow.Cells["gross"].Value ?? "0").ToString() == (selectedRow.Cells["net"].Value ?? "0").ToString())
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)105.5, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }


                    //logo
                    Image logoImage = Image.FromFile("C:\\Users\\91760\\Desktop\\SYA\\Image\\logo.jpg"); // Replace with the actual path
                    e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                    //logo name 
                    int hyphenIndex = selectedRow.Cells["itemdesc"].Value.ToString().IndexOf("-");

                    string result = null;
                    if (hyphenIndex != -1 && hyphenIndex < selectedRow.Cells[4].Value.ToString().Length - 1)
                    {
                        // Extract text after the hyphen
                        result = selectedRow.Cells[4].Value.ToString().Substring(hyphenIndex + 3);
                        // MessageBox.Show(result + " " + result.Length);
                    }
                    if (result == "BANGAL")
                    {
                        e.Graphics.DrawString((selectedRow.Cells[12].Value ?? "0").ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {

                        e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //caret
                    e.Graphics.DrawString((selectedRow.Cells["itemdesc"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw the QR code rectangle
                    RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        // Draw the QR code onto the printing surface
                        e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                    }

                    //labour                
                    string labour = "0";
                    if (selectedRow.Cells["labour"].Value.ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL: " + labour, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {

                        e.Graphics.DrawString("O: " + (selectedRow.Cells["other"].Value ?? "0").ToString(), new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)16, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //Tag number
                    string firstPart = null;
                    string secondPart = null;
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 5;
                        firstPart = tagNumber.Substring(lastIndex);
                        secondPart = tagNumber.Substring(0, lastIndex);
                        e.Graphics.DrawString(secondPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        e.Graphics.DrawString(firstPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString(tagNumber, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    //tempppp
                    string huidd1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    e.Graphics.DrawString(huidd1, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)39, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });


                    //huid
                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        e.Graphics.DrawString("HUID", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    Log.Information(" TagNo : " + tagNumber);
                    //outside box
                    //e.Graphics.DrawRectangle(Pens.Red, 4, 4, 211, 45);
                    //first part
                    //e.Graphics.DrawRectangle(Pens.Red, 4, 4, (float)105.5, 45);
                    //Second Part
                    //e.Graphics.DrawRectangle(Pens.Red, (float)109.5, 4, (float)105.5, 45);
                    //gross weight
                    //e.Graphics.DrawRectangle(Pens.Red, 4, 4, 75, (float)22.5);
                    //net weight
                    //e.Graphics.DrawRectangle(Pens.Red, 4, (float)26.5, 75, (float)22.5);
                    //logo
                    //e.Graphics.DrawRectangle(Pens.Red, 83, 4, (float)22.5, (float)22.5);
                    //logo name 
                    //e.Graphics.DrawRectangle(Pens.Red, 79, (float)26.5, (float)30.5, (float)11.25);
                    //caret
                    //e.Graphics.DrawRectangle(Pens.Red, 79, (float)37.75, (float)30.5, (float)11.25);
                    // Draw the QR code rectangle
                    //RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                    //e.Graphics.DrawRectangle(Pens.Red, qrCodeRect.X, qrCodeRect.Y, qrCodeRect.Width, qrCodeRect.Height);
                    //labour                
                    //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)4, (float)56.5, (float)11);
                    //other
                    //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)15, (float)56.5, (float)11);
                    //Tag number
                    //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)26, (float)56.5, (float)11);
                    //huid
                    //e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)40, (float)37, (float)9);
                }
            }
        }
        private void PrintPageSilver925(object sender, PrintPageEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;

            if (selectedRow != null)
            {

                Font font = new Font("Arial Black", 8, FontStyle.Bold); // Adjust the font size
                SolidBrush brush = new SolidBrush(Color.Black);

                // Set the starting position for printing
                float xPos = 0; // Adjust the starting X position
                float yPos = 0; // Adjust the starting Y position

                // Get the printer DPI
                float dpiX = e.PageSettings.PrinterResolution.X;
                float dpiY = e.PageSettings.PrinterResolution.Y;

                float rectX = 4; // Adjust the X position of the rectangle
                float rectY = 4; // Adjust the Y position of the rectangle
                float rectWidth = 211; // Adjust the width of the rectangle
                float rectHeight = 45; // Adjust the height of the rectangle

                //gross weight
                e.Graphics.DrawString("\u20B9" + selectedRow.Cells[13].Value.ToString(), new Font("Arial", (float)14, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                //net weight
                //e.Graphics.DrawString("N: " + selectedRow.Cells[6].Value.ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                //logo
                Image logoImage = Image.FromFile("C:\\Users\\91760\\Desktop\\SYA\\Image\\logo.jpg"); // Replace with the actual path
                e.Graphics.DrawImage(logoImage, new RectangleF(83, 4, (float)22.5, (float)22.5));

                //logo name 
                int hyphenIndex = selectedRow.Cells[4].Value.ToString().IndexOf("-");

                string result = null;
                if (hyphenIndex != -1 && hyphenIndex < selectedRow.Cells[4].Value.ToString().Length - 1)
                {
                    // Extract text after the hyphen
                    result = selectedRow.Cells[4].Value.ToString().Substring(hyphenIndex + 3);

                }
                if (result == "BANGAL")
                {
                    e.Graphics.DrawString(selectedRow.Cells[12].Value.ToString(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)28, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                else
                {

                    e.Graphics.DrawString("YAMUNA", new Font("Arial", (float)4.5, FontStyle.Bold), brush, new RectangleF(79, (float)26.5, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }

                //caret
                e.Graphics.DrawString(selectedRow.Cells[4].Value.ToString().Split('-')[0].Trim(), new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF(79, (float)37.75, (float)30.5, (float)11.25), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                // Draw the QR code rectangle
                RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                string tagNumber = selectedRow.Cells["tagno"].Value.ToString();
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRect.Width, System.Drawing.Color.Black, System.Drawing.Color.White, true);
                    // Draw the QR code onto the printing surface
                    e.Graphics.DrawImage(qrCodeBitmap, qrCodeRect);
                }

                //labour
                e.Graphics.DrawString("\u20B9" + selectedRow.Cells[13].Value.ToString(), new Font("Arial", (float)10, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)4, (float)56.5, (float)27), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                //other
                //e.Graphics.DrawString("O: " + selectedRow.Cells[11].Value.ToString(), new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)16, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                //Tag number
                string firstPart = null;
                string secondPart = null;
                int length = tagNumber.Length;
                if (length >= 10)
                {
                    int lastIndex = length - 5;
                    firstPart = tagNumber.Substring(lastIndex);
                    secondPart = tagNumber.Substring(0, lastIndex);
                    e.Graphics.DrawString(secondPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)29, (float)56.5, (float)10), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    e.Graphics.DrawString(firstPart, new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)38, (float)56.5, (float)12), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                else
                {
                    e.Graphics.DrawString(tagNumber, new Font("Arial", (float)7, FontStyle.Bold), brush, new RectangleF((float)113.5, (float)30, (float)56.5, (float)11), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }

                //huid
                string huid1 = selectedRow.Cells[7].Value.ToString();
                if (huid1.Length == 6)
                {
                    e.Graphics.DrawString("HUID", new Font("Arial", (float)6, FontStyle.Bold), brush, new RectangleF((float)174, (float)40, (float)37, (float)9), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                //outside box
                //e.Graphics.DrawRectangle(Pens.Red, 4, 4, 211, 45);
                //first part
                //e.Graphics.DrawRectangle(Pens.Red, 4, 4, (float)105.5, 45);
                //Second Part
                //e.Graphics.DrawRectangle(Pens.Red, (float)109.5, 4, (float)105.5, 45);
                //gross weight
                //e.Graphics.DrawRectangle(Pens.Red, 4, 4, 75, (float)22.5);
                //net weight
                //e.Graphics.DrawRectangle(Pens.Red, 4, (float)26.5, 75, (float)22.5);
                //logo
                //e.Graphics.DrawRectangle(Pens.Red, 83, 4, (float)22.5, (float)22.5);
                //logo name 
                //e.Graphics.DrawRectangle(Pens.Red, 79, (float)26.5, (float)30.5, (float)11.25);
                //caret
                //e.Graphics.DrawRectangle(Pens.Red, 79, (float)37.75, (float)30.5, (float)11.25);
                // Draw the QR code rectangle
                //RectangleF qrCodeRect = new RectangleF(174, 4, 37, 37);
                //e.Graphics.DrawRectangle(Pens.Red, qrCodeRect.X, qrCodeRect.Y, qrCodeRect.Width, qrCodeRect.Height);
                //labour                
                //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)4, (float)56.5, (float)11);
                //other
                //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)15, (float)56.5, (float)11);
                //Tag number
                //e.Graphics.DrawRectangle(Pens.Red, (float)113.5, (float)26, (float)56.5, (float)11);
                //huid
                //e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)40, (float)37, (float)9);
            }
        }
        //Buttons
        private void gridviewstyle()
        {

            dataGridViewSearch.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(191, 135, 212); // Color for row headers

            //  dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(233, 245, 219);
            dataGridViewSearch.Columns["select"].HeaderCell.Style.BackColor = Color.FromArgb(208, 209, 255);
            dataGridViewSearch.Columns["tagno"].HeaderCell.Style.BackColor = Color.FromArgb(216, 187, 255);
            dataGridViewSearch.Columns["vchno"].HeaderCell.Style.BackColor = Color.FromArgb(222, 170, 255);
            dataGridViewSearch.Columns["vchdate"].HeaderCell.Style.BackColor = Color.FromArgb(226, 175, 255);
            dataGridViewSearch.Columns["itemdesc"].HeaderCell.Style.BackColor = Color.FromArgb(229, 179, 254);
            dataGridViewSearch.Columns["gross"].HeaderCell.Style.BackColor = Color.FromArgb(236, 188, 253);
            dataGridViewSearch.Columns["net"].HeaderCell.Style.BackColor = Color.FromArgb(243, 196, 251);
            dataGridViewSearch.Columns["huid1"].HeaderCell.Style.BackColor = Color.FromArgb(255, 203, 242);
            dataGridViewSearch.Columns["huid2"].HeaderCell.Style.BackColor = Color.FromArgb(243, 196, 251);
            dataGridViewSearch.Columns["labour"].HeaderCell.Style.BackColor = Color.FromArgb(236, 188, 253);
            dataGridViewSearch.Columns["wholeLabour"].HeaderCell.Style.BackColor = Color.FromArgb(229, 179, 254);
            dataGridViewSearch.Columns["other"].HeaderCell.Style.BackColor = Color.FromArgb(226, 175, 255);
            dataGridViewSearch.Columns["size"].HeaderCell.Style.BackColor = Color.FromArgb(222, 170, 255);
            dataGridViewSearch.Columns["price"].HeaderCell.Style.BackColor = Color.FromArgb(216, 187, 255);
            dataGridViewSearch.Columns["comment"].HeaderCell.Style.BackColor = Color.FromArgb(208, 209, 255);
            foreach (DataGridViewColumn column in dataGridViewSearch.Columns)
            {
                if (column.Name == "huid1") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                else if (column.Name == "huid2") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                else if (column.Name == "wholeLabour") // Adjust the column name
                {
                    column.Width = 115; // Adjust the width as needed
                }
                else if (column.Name == "tagno") // Adjust the column name
                {
                    column.Width = 175; // Adjust the width as needed
                }
                else if (column.Name == "other") // Adjust the column name
                {
                    column.Width = 95; // Adjust the width as needed
                }
                else if (column.Name == "labour") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                else if (column.Name == "net") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                else if (column.Name == "gross") // Adjust the column name
                {
                    column.Width = 100; // Adjust the width as needed
                }
                // Set cell alignment to middle center
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for cells
                column.DefaultCellStyle.Font = new Font("Arial", (float)11); // Adjust the font and size as needed
                                                                             // Add more conditions for other columns as needed
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Set font size for column headers
                column.HeaderCell.Style.Font = new Font("Arial", (float)11.5, FontStyle.Bold); // Adjust the font and size as needed
            }

        }
        private void btnSearchSelectAll_Click(object sender, EventArgs e)
        {
            if (dataGridViewSearch == null)
            {
                // Log or handle the fact that the dataGridViewSearch is null
                return;
            }

            bool isAnyRowNotSelected = false;

            // Check if any row is not selected
            foreach (DataGridViewRow row in dataGridViewSearch.Rows)
            {
                if (!Convert.ToBoolean(row.Cells["select"].Value))
                {
                    isAnyRowNotSelected = true;
                    break;
                }
            }

            // Toggle the selection state for all rows
            foreach (DataGridViewRow row in dataGridViewSearch.Rows)
            {
                row.Cells["select"].Value = isAnyRowNotSelected;
            }

            // Update the button text based on the new state
            UpdateSelectAllButtonText();
        }

        private void UpdateSelectAllButtonText()
        {
            if (dataGridViewSearch == null)
            {
                // Log or handle the fact that the dataGridViewSearch is null
                return;
            }

            bool isAnyRowNotSelected = false;

            // Check if any row is not selected
            foreach (DataGridViewRow row in dataGridViewSearch.Rows)
            {
                if (!Convert.ToBoolean(row.Cells["select"].Value))
                {
                    isAnyRowNotSelected = true;
                    break;
                }
            }

            // Update the button text based on the new state
            btnSearchSelectAll.Text = isAnyRowNotSelected ? "Select All" : "Deselect All";
        }

        private bool leaveEventFlag = false;
        // Events
        private void dataGridViewSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (checkBoxSearch1.Checked == true)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = dataGridViewSearch;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("pratyush  :  " + currentColumnName);
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here
                        if (SaveDataToSQLite())
                        {
                            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintLabels();
                            }
                        }
                    }

                }
            }
            else if (checkBoxSearch2.Checked == true)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;

                    DataGridView dataGridView = dataGridViewSearch;
                    string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                    int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                    //  MessageBox.Show("pratyush1: " + currentColumnName);

                    // Assuming "comment" is the name of the last column
                    if (currentColumnName == "comment")
                    {
                        // MessageBox.Show("in comment");
                        // You are moving to the next row in the last column
                        // Call your save and/or print function here

                        DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                        string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                        if (tagNumber.Length > 1)
                        {
                            PrintLabels();
                        }

                    }

                }
            }
        }
        private void txtTagno_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtTagno.Text = null;
            leaveEventFlag = false;
        }
        private void txtWeight_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE GW LIKE '%{txtWeight.Text}%' OR NW LIKE '%{txtWeight.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtWeight_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtWeight.Text = null;
            leaveEventFlag = false;
        }
        private void txtBillNo_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE VCH_NO LIKE '%{txtBillNo.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtBillNo_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtBillNo.Text = null;
            leaveEventFlag = false;
        }
        private void txtHUID_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE HUID1 LIKE '%{txtHUID.Text}%' OR HUID2 LIKE '%{txtHUID.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtHUID_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtHUID.Text = null;
            leaveEventFlag = false;
        }
        private void txtSearchAnything_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string searchValue = txtSearchAnything.Text;
                string query = $"SELECT * FROM MAIN_DATA WHERE " +
                                $"TAG_NO LIKE '%{searchValue}%' OR " +
                                $"VCH_NO LIKE '%{searchValue}%' OR " +
                                $"ITEM_PURITY LIKE '%{searchValue}%' OR " +
                                $"ITEM_DESC LIKE '%{searchValue}%' OR " +
                                $"COMMENT LIKE '%{searchValue}%' OR " +
                                $"CO_YEAR LIKE '%{searchValue}%' OR " +
                                $"CO_BOOK LIKE '%{searchValue}%' OR " +
                                $"VCH_DATE LIKE '%{searchValue}%' OR " +
                                $"GW LIKE '%{searchValue}%' OR " +
                                $"NW LIKE '%{searchValue}%' OR " +
                                $"LABOUR_AMT LIKE '%{searchValue}%' OR " +
                                $"WHOLE_LABOUR_AMT LIKE '%{searchValue}%' OR " +
                                $"OTHER_AMT LIKE '%{searchValue}%' OR " +
                                $"IT_TYPE LIKE '%{searchValue}%' OR " +
                                $"ITEM_CODE LIKE '%{searchValue}%' OR " +
                                $"HUID1 LIKE '%{searchValue}%' OR " +
                                $"HUID2 LIKE '%{searchValue}%' OR " +
                                $"SIZE LIKE '%{searchValue}%' OR " +
                                $"PRICE LIKE '%{searchValue}%' OR " +
                                $"STATUS LIKE '%{searchValue}%' OR " +
                                $"AC_CODE LIKE '%{searchValue}%' OR " +
                                $"AC_NAME LIKE '%{searchValue}%'";
                LoadDataFromSQLite(query);
            }
        }
        private void txtSearchAnything_Leave(object sender, EventArgs e)
        {
            leaveEventFlag = true;
            txtSearchAnything.Text = null;
            leaveEventFlag = false;
        }

        private void dataGridViewSearch_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.PreviewKeyDown -= dataGridView_EditingControl_PreviewKeyDown;
            e.Control.PreviewKeyDown += dataGridView_EditingControl_PreviewKeyDown;
        }
        private void dataGridView_EditingControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (checkBoxSearch1.Checked == true)
            {

                // Handle the Tab key to trigger the KeyDown event for the text box or combo box
                if (e.KeyCode == Keys.Tab)
                {
                    DataGridViewTextBoxEditingControl editingControl = sender as DataGridViewTextBoxEditingControl;
                    if (editingControl != null)
                    {
                        DataGridView dataGridView = dataGridViewSearch;
                        string currentColumnName = dataGridView.Columns[dataGridView.CurrentCell.ColumnIndex].Name;
                        int currentRowIndex = dataGridView.CurrentCell.RowIndex;
                        //  MessageBox.Show("pratyush1: " + currentColumnName);

                        // Assuming "comment" is the name of the last column
                        if (currentColumnName == "comment")
                        {
                            // MessageBox.Show("in comment");
                            // You are moving to the next row in the last column
                            // Call your save and/or print function here
                            if (SaveDataToSQLite())
                            {
                                DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                                if (tagNumber.Length > 1)
                                {
                                    PrintLabels();
                                }
                            }
                        }
                    }
                }
            }
        }
        private void txtTagno_TextChanged(object sender, EventArgs e)
        {
            if (!leaveEventFlag)
            {
                string query = $"SELECT * FROM MAIN_DATA WHERE TAG_NO LIKE '%{txtTagno.Text}%'";
                LoadDataFromSQLite(query);
            }
        }
    }
}
