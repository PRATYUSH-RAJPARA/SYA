
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using QRCoder;
using Serilog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SYA
{
    public partial class Search : Form
    {
        // Basics---------------------------
        bool quickSaveAndPrint = false;
        private SQLiteConnection connectionToSYADatabase;
        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection(helper.SYAConnectionString);


        }
        bool quickSave = false;
        public Search()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            progressBar1.Visible = false;

            gridviewstyle();
        }
        private void InitializeLogging()
        {
            Log.Logger = new LoggerConfiguration()

                .WriteTo.File(helper.LogsFolder + "\\logs_tagno.txt", rollingInterval: RollingInterval.Day) // Log to a file with daily rolling
                .CreateLogger();
        }
        private void Search_Load(object sender, EventArgs e)
        {

            string query = "SELECT * FROM MAIN_DATA ORDER BY VCH_DATE DESC;";
            LoadDataFromSQLite(query);
            setCellVisiable();
            InitializeLogging();

        }
        private void messageBoxTimer_Tick(object sender, EventArgs e)
        {
            // Clear the TextBox after the timer interval
            txtMessageBox.Text = string.Empty;

            // Stop the timer
            messageBoxTimer.Stop();
        }
        // GridView Styling ---------------------------
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
        // load data ---------------------------
        private void LoadDataFromSQLite(String query)
        {
            try
            {

                using (SQLiteDataReader reader = helper.FetchDataFromSYADataBase(query))
                {
                    if (reader != null)
                    {
                        dataGridViewSearch.DataSource = null;
                        dataGridViewSearch.Rows.Clear();
                        while (reader.Read())
                        {
                            int rowIndex = dataGridViewSearch.Rows.Add();
                            dataGridViewSearch.Rows[rowIndex].Cells["tagno"].Value = reader["TAG_NO"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["vchno"].Value = reader["VCH_NO"].ToString();
                            dataGridViewSearch.Rows[rowIndex].Cells["vchdate"].Value = Convert.ToDateTime(reader["VCH_DATE"]).ToString("dd-MM-yyyy");
                            dataGridViewSearch.Rows[rowIndex].Cells["itemdesc"].Value = reader["ITEM_PURITY"].ToString() + "  -  " + reader["ITEM_DESC"].ToString();

                            // Format GW and NW with 3 decimal places
                            dataGridViewSearch.Rows[rowIndex].Cells["gross"].Value = string.Format("{0:0.000}", reader["GW"]);
                            dataGridViewSearch.Rows[rowIndex].Cells["net"].Value = string.Format("{0:0.000}", reader["NW"]);

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
        // Save Button ---------------------------
        private void btnSearchSave_Click(object sender, EventArgs e)
        {
            DataGridViewRow empty = new DataGridViewRow();
            SaveDataToSQLite(empty, 0);
        }
        private bool SaveDataToSQLite(DataGridViewRow selectedRow, int check)
        {
            if (check == 0)
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

                        if (!helper.validateHUID(huid1, huid2))
                        {
                            SelectCell(dataGridViewSearch, row.Index, "huid1");
                            return false;
                        }
                        if ((row.Cells["vchno"].Value ?? "").ToString() != "SYA00")
                        {
                            if (!helper.validateWeight(gross, net))
                            {
                                MessageBox.Show($"Gross weight should be greater than or equal to net weight for Tag Number : " + tagNo + " .");
                                SelectCell(dataGridViewSearch, row.Index, "gross");
                                return false;
                            }
                            if (!helper.validateWeight(gross))
                            {
                                MessageBox.Show($"Gross weight should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                                SelectCell(dataGridViewSearch, row.Index, "gross");
                                return false;
                            }
                            if (!helper.validateWeight(net))
                            {
                                MessageBox.Show($"Net weight should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                                SelectCell(dataGridViewSearch, row.Index, "net");
                                return false;
                            }
                        }
                        if (!helper.validateLabour(labour))
                        {
                            MessageBox.Show($"Labour amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                            SelectCell(dataGridViewSearch, row.Index, "labour");
                            return false;
                        }

                        if (!helper.validateOther(wholeLabour))
                        {
                            MessageBox.Show($"Other amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                            SelectCell(dataGridViewSearch, row.Index, "other");
                            return false;
                        }
                        if (!helper.validateOther(other))
                        {
                            MessageBox.Show($"Other amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                            SelectCell(dataGridViewSearch, row.Index, "other");
                            return false;
                        }
                        string updateQuery = $"UPDATE MAIN_DATA SET HUID1 = '{huid1}', HUID2 = '{huid2}', COMMENT = '{comment}',LABOUR_AMT = '{labour}',OTHER_AMT = '{other}',WHOLE_LABOUR_AMT = '{wholeLabour}', GW = '{gross}',NW = '{net}',SIZE = '{size}',PRICE = '{price}'  WHERE TAG_NO = '{tagNo}'";

                        helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
                    }
                    // pratyush uncomment later
                    txtMessageBox.Text = "Data Saved Successfully.";
                    messageBoxTimer.Start();

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
            else if (check == 1)
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
                    tagNo = selectedRow.Cells["tagno"].Value?.ToString() ?? string.Empty;
                    gross = selectedRow.Cells["gross"].Value?.ToString() ?? "0";
                    net = selectedRow.Cells["net"].Value?.ToString() ?? "0";
                    labour = selectedRow.Cells["labour"].Value?.ToString() ?? "0";
                    wholeLabour = selectedRow.Cells["wholeLabour"].Value?.ToString() ?? "0";
                    other = selectedRow.Cells["other"].Value?.ToString() ?? "0";
                    huid1 = selectedRow.Cells["huid1"].Value?.ToString()?.ToUpper() ?? string.Empty;
                    huid2 = selectedRow.Cells["huid2"].Value?.ToString()?.ToUpper() ?? string.Empty;
                    size = selectedRow.Cells["size"].Value?.ToString() ?? string.Empty;
                    price = selectedRow.Cells["price"].Value?.ToString() ?? "0";
                    comment = selectedRow.Cells["comment"].Value?.ToString() ?? string.Empty;

                    if (!helper.validateHUID(huid1, huid2))
                    {
                        SelectCell(dataGridViewSearch, selectedRow.Index, "huid1");
                        return false;
                    }
                    if ((selectedRow.Cells["vchno"].Value ?? "").ToString() != "SYA00")
                    {
                        if (!helper.validateWeight(gross, net))
                        {
                            MessageBox.Show($"Gross weight should be greater than or equal to net weight for Tag Number : " + tagNo + " .");
                            SelectCell(dataGridViewSearch, selectedRow.Index, "gross");
                            return false;
                        }
                        if (!helper.validateWeight(gross))
                        {
                            MessageBox.Show($"Gross weight should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                            SelectCell(dataGridViewSearch, selectedRow.Index, "gross");
                            return false;
                        }
                        if (!helper.validateWeight(net))
                        {
                            MessageBox.Show($"Net weight should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                            SelectCell(dataGridViewSearch, selectedRow.Index, "net");
                            return false;
                        }
                    }
                    if (!helper.validateLabour(labour))
                    {
                        MessageBox.Show($"Labour amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                        SelectCell(dataGridViewSearch, selectedRow.Index, "labour");
                        return false;
                    }

                    if (!helper.validateOther(wholeLabour))
                    {
                        MessageBox.Show($"Other amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                        SelectCell(dataGridViewSearch, selectedRow.Index, "other");
                        return false;
                    }
                    if (!helper.validateOther(other))
                    {
                        MessageBox.Show($"Other amount should be a non-negative numeric value for Tag Number : " + tagNo + " .");
                        SelectCell(dataGridViewSearch, selectedRow.Index, "other");
                        return false;
                    }
                    string updateQuery = $"UPDATE MAIN_DATA SET HUID1 = '{huid1}', HUID2 = '{huid2}', COMMENT = '{comment}',LABOUR_AMT = '{labour}',OTHER_AMT = '{other}',WHOLE_LABOUR_AMT = '{wholeLabour}', GW = '{gross}',NW = '{net}',SIZE = '{size}',PRICE = '{price}'  WHERE TAG_NO = '{tagNo}'";

                    helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");

                    // pratyush uncomment later
                    txtMessageBox.Text = "Data Saved Successfully.";
                    messageBoxTimer.Start();
                    //MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            return false;
        }

        private void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
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
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)12, FontStyle.Bold), brush, new RectangleF(4, 4, (float)75, (float)45), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }
                    else
                    {
                        e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, 4, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                        e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(), new Font("Arial", (float)9.5, FontStyle.Bold), brush, new RectangleF(4, (float)26.5, 75, (float)22.5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    }


                    //logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
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
                    RectangleF qrCodeRect = new RectangleF(174, 2, 37, 37);

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



                    //huid
                    //   e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)37, (float)37, (float)5);
                    //  e.Graphics.DrawRectangle(Pens.Red, (float)174, (float)43, (float)37, (float)5);

                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    string huid2 = (selectedRow.Cells["huid2"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        e.Graphics.DrawString(huid1, new Font("Arial", (float)5, FontStyle.Bold), brush, new RectangleF((float)174, (float)38, (float)37, (float)7), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    if (huid2.Length == 6)
                    {
                        e.Graphics.DrawString(huid2, new Font("Arial", (float)5, FontStyle.Bold), brush, new RectangleF((float)174, (float)44, (float)37, (float)7), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    Log.Information(" TagNo : " + tagNumber);
                    string updateQuery = $"UPDATE MAIN_DATA SET PRINT = '1'   WHERE TAG_NO = '{tagNumber}'";

                    helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
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
        private void PrintPageGoldWorkingAndUsed(object sender, PrintPageEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;

            if (selectedRow != null)
            {
                string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                if (tagNumber.Length > 1)
                {
                    Font font = new Font("Arial Black", 8, FontStyle.Bold);
                    SolidBrush brush = new SolidBrush(Color.Black);

                    // Set the starting position for printing
                    float xPos = 0;
                    float yPos = 0;

                    // Get the printer DPI
                    float dpiX = e.PageSettings.PrinterResolution.X;
                    float dpiY = e.PageSettings.PrinterResolution.Y;

                    // Define sizes and positions using arrays
                    float[] rectParams = new float[] { 4, 4, 211, 45 }; // x, y, width, height
                    float[] grossRectParams = new float[] { 4, 4, 75, 22.5f }; // x, y, width, height
                    float[] netRectParams = new float[] { 4, 26.5f, 75, 22.5f }; // x, y, width, height
                    float[] logoRectParams = new float[] { 83, 4, 22.5f, 22.5f }; // x, y, width, height
                    float[] qrCodeRectParams = new float[] { 174, 2, 37, 37 }; // x, y, width, height
                    float[] netValueRectParams = new float[] { 115.5f, 4, 56.5f, 17 }; // x, y, width, height
                    float[] labourRectParams = new float[] { 117.5f, 27.5f, 32.25f, 12 }; // x, y, width, height
                    float[] otherRectParams = new float[] { 149.75f, 27.5f, 32.25f, 12 }; // x, y, width, height
                    float[] tagNumberRectParams = new float[] { 113.5f, 38, 56.5f, 12 }; // x, y, width, height
                    float[] huid1RectParams = new float[] { 174, 38, 37, 7 }; // x, y, width, height
                    float[] huid2RectParams = new float[] { 174, 44, 37, 7 }; // x, y, width, height
                    float[] logoNameRectParams = new float[] { 79, 26.5f, 30.5f, 11.25f }; // x, y, width, height
                    float[] caretRectParams = new float[] { 79, 37.75f, 30.5f, 11.25f }; // x, y, width, height

                    // Use the array values in the drawing
                    e.Graphics.DrawString("G: " + (selectedRow.Cells["gross"].Value ?? "0").ToString(),
                        new Font("Arial", (float)12, FontStyle.Bold), brush,
                        new RectangleF(grossRectParams[0], grossRectParams[1], grossRectParams[2], grossRectParams[3]),
                        new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    // Draw caret
                    e.Graphics.DrawString((selectedRow.Cells["itemdesc"].Value ?? "0").ToString().Split('-')[0].Trim() ?? "0",
                        new Font("Arial", (float)6, FontStyle.Bold), brush,
                        new RectangleF(caretRectParams[0], caretRectParams[1], caretRectParams[2], caretRectParams[3]),
                        new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    // Draw logo name
                    int hyphenIndex = selectedRow.Cells["itemdesc"].Value.ToString().IndexOf("-");
                    string result = null;
                    if (hyphenIndex != -1 && hyphenIndex < selectedRow.Cells[4].Value.ToString().Length - 1)
                    {
                        result = selectedRow.Cells[4].Value.ToString().Substring(hyphenIndex + 3);
                    }
                    if (result == "BANGAL")
                    {
                        e.Graphics.DrawString((selectedRow.Cells[12].Value ?? "0").ToString(),
                            new Font("Arial", (float)6, FontStyle.Bold), brush,
                            new RectangleF(logoNameRectParams[0], logoNameRectParams[1], logoNameRectParams[2], logoNameRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString("YAMUNA",
                            new Font("Arial", (float)4.5, FontStyle.Bold), brush,
                            new RectangleF(logoNameRectParams[0], logoNameRectParams[1], logoNameRectParams[2], logoNameRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    e.Graphics.DrawString("N: " + (selectedRow.Cells["net"].Value ?? "0").ToString(),
                        new Font("Arial", (float)12, FontStyle.Bold), brush,
                        new RectangleF(netRectParams[0], netRectParams[1], netRectParams[2], netRectParams[3]),
                        new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    // Draw logo
                    Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg");
                    e.Graphics.DrawImage(logoImage, new RectangleF(logoRectParams[0], logoRectParams[1], logoRectParams[2], logoRectParams[3]));

                    // Draw QR code
                    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                    {
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(tagNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        System.Drawing.Bitmap qrCodeBitmap = qrCode.GetGraphic((int)qrCodeRectParams[2], System.Drawing.Color.Black, System.Drawing.Color.White, true);
                        e.Graphics.DrawImage(qrCodeBitmap, new RectangleF(qrCodeRectParams[0], qrCodeRectParams[1], qrCodeRectParams[2], qrCodeRectParams[3]));
                    }

                    // Draw net value
                    e.Graphics.DrawString(selectedRow.Cells["net"].Value.ToString(),
                        new Font("Arial", (float)11, FontStyle.Bold), brush,
                        new RectangleF(netValueRectParams[0], netValueRectParams[1], netValueRectParams[2], netValueRectParams[3]),
                        new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });

                    // Draw labour
                    string labour = "0";
                    if (selectedRow.Cells["labour"].Value.ToString() != "0")
                    {
                        labour = (selectedRow.Cells["labour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("L:" + labour,
                            new Font("Arial", (float)7, FontStyle.Bold), brush,
                            new RectangleF(labourRectParams[0], labourRectParams[1], labourRectParams[2], labourRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                    else if ((selectedRow.Cells["wholeLabour"].Value ?? "-").ToString() != "0")
                    {
                        labour = (selectedRow.Cells["wholeLabour"].Value ?? "-").ToString();
                        e.Graphics.DrawString("TL:" + labour,
                            new Font("Arial", (float)7, FontStyle.Bold), brush,
                            new RectangleF(labourRectParams[0], labourRectParams[1], labourRectParams[2], labourRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                    // Draw other
                    if ((selectedRow.Cells["other"].Value ?? "0").ToString() != "0")
                    {
                        e.Graphics.DrawString("O:" + (selectedRow.Cells["other"].Value ?? "0").ToString(),
                            new Font("Arial", (float)7, FontStyle.Bold), brush,
                            new RectangleF(otherRectParams[0], otherRectParams[1], otherRectParams[2], otherRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }

                    // Draw Tag number
                    string firstPart = null;
                    string secondPart = null;
                    int length = tagNumber.Length;
                    if (length >= 10)
                    {
                        int lastIndex = length - 5;
                        firstPart = tagNumber.Substring(lastIndex);
                        secondPart = tagNumber.Substring(0, lastIndex);
                        e.Graphics.DrawString(secondPart,
                            new Font("Arial", (float)6, FontStyle.Bold), brush,
                            new RectangleF(tagNumberRectParams[0], tagNumberRectParams[1], tagNumberRectParams[2], tagNumberRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    else
                    {
                        e.Graphics.DrawString(tagNumber,
                            new Font("Arial", (float)6, FontStyle.Bold), brush,
                            new RectangleF(tagNumberRectParams[0], tagNumberRectParams[1], tagNumberRectParams[2], tagNumberRectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    // Draw HUID
                    string huid1 = (selectedRow.Cells["huid1"].Value ?? "0").ToString();
                    string huid2 = (selectedRow.Cells["huid2"].Value ?? "0").ToString();
                    if (huid1.Length == 6)
                    {
                        e.Graphics.DrawString(huid1,
                            new Font("Arial", (float)5, FontStyle.Bold), brush,
                            new RectangleF(huid1RectParams[0], huid1RectParams[1], huid1RectParams[2], huid1RectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    if (huid2.Length == 6)
                    {
                        e.Graphics.DrawString(huid2,
                            new Font("Arial", (float)5, FontStyle.Bold), brush,
                            new RectangleF(huid2RectParams[0], huid2RectParams[1], huid2RectParams[2], huid2RectParams[3]),
                            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }

                    Log.Information(" TagNo : " + tagNumber);
                    string updateQuery = $"UPDATE MAIN_DATA SET PRINT = '1' WHERE TAG_NO = '{tagNumber}'";

                    helper.RunQueryWithoutParametersSYADataBase(updateQuery, "ExecuteNonQuery");
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
                Image logoImage = Image.FromFile(helper.ImageFolder + "\\logo.jpg"); // Replace with the actual path
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
        private void btnQuickSaveAndPrint_Click(object sender, EventArgs e)
        {
            if (btnQuickSaveAndPrint.Text == "Enable Quick Save & Print")
            {
                btnQuickSaveAndPrint.Text = "Disable Quick Save & Print";
                txtMessageBox.Text = "Quick Save & Print Enabled.";
                messageBoxTimer.Start();
                quickSaveAndPrint = true;
            }
            else if (btnQuickSaveAndPrint.Text == "Disable Quick Save & Print")
            {
                btnQuickSaveAndPrint.Text = "Enable Quick Save & Print";
                txtMessageBox.Text = "Quick Save & Print Disabled.";
                messageBoxTimer.Start();
                quickSaveAndPrint = false;
            }
        }



        private void buttonquicksave_Click(object sender, EventArgs e)
        {
            if (buttonquicksave.Text == "Enable Quick Save")
            {
                buttonquicksave.Text = "Disable Quick Save";
                txtMessageBox.Text = "Quick Save Enabled.";
                messageBoxTimer.Start();
                quickSave = true;
            }
            else if (buttonquicksave.Text == "Disable Quick Save")
            {
                buttonquicksave.Text = "Enable Quick Save";
                txtMessageBox.Text = "Quick Save Disabled.";
                messageBoxTimer.Start();
                quickSave = false;
            }
        }
        // Events
        private void dataGridViewSearch_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView1 = dataGridViewSearch;
            string currentColumnName1 = dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name;
            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
            if (currentColumnName1 == "net")
            {
                selectedRow.Cells["net"].Value = helper.correctWeight(selectedRow.Cells["net"].Value);
            }
            if (currentColumnName1 == "gross")
            {
                selectedRow.Cells["gross"].Value = helper.correctWeight(selectedRow.Cells["gross"].Value);
            }
            if (currentColumnName1 == "huid1")
            {
                selectedRow.Cells["huid1"].Value = (selectedRow.Cells["huid1"].Value ?? "").ToString().ToUpper();
            }
            if (currentColumnName1 == "huid2")
            {
                selectedRow.Cells["huid2"].Value = (selectedRow.Cells["huid2"].Value ?? "").ToString().ToUpper();
            }
            if (quickSaveAndPrint)
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
                        DataGridViewRow empty = new DataGridViewRow();
                        //DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;

                        if (SaveDataToSQLite(selectedRow, 1))
                        {
                            string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                            if (tagNumber.Length > 1)
                            {
                                PrintLabels();
                            }
                        }
                    }

                }
            }
            //quick print
            else if (false)
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

                        // DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                        string tagNumber = (selectedRow.Cells["tagno"].Value ?? "0").ToString();
                        if (tagNumber.Length > 1)
                        {
                            PrintLabels();
                        }

                    }

                }
            }
            else if (quickSave)
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
                        //    DataGridViewRow empty = new DataGridViewRow();
                        //    DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                        SaveDataToSQLite(selectedRow, 1);

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
            if (quickSaveAndPrint)
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
                        if (currentColumnName == "net")
                        {
                            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                            selectedRow.Cells["net"].Value = helper.correctWeight(selectedRow.Cells["net"].Value);
                        }
                        if (currentColumnName == "comment")
                        {
                            // MessageBox.Show("in comment");
                            // You are moving to the next row in the last column
                            // Call your save and/or print function here
                            DataGridViewRow empty = new DataGridViewRow();
                            DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;

                            if (SaveDataToSQLite(selectedRow, 1))
                            {
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

        private void dataGridViewSearch_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Check if the edited cell is in the "gross" column
                if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "gross")
                {
                    // Copy the value from the "gross" column to the corresponding "net" column
                    dataGridViewSearch.Rows[e.RowIndex].Cells["net"].Value = dataGridViewSearch.Rows[e.RowIndex].Cells["gross"].Value;
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // Update the item count and gross weight sum
                }
                else if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "net")
                {

                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = helper.correctWeight(dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    // Update the item count and gross weight sum
                }
                else if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "huid1")
                {
                    DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }
                else if (dataGridViewSearch.Columns[e.ColumnIndex].Name == "huid2")
                {
                    DataGridViewRow selectedRow = dataGridViewSearch.CurrentRow;
                    dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridViewSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString().ToUpper();
                }


            }
        }


        // fetchingggggggggggggggggggg
        string queryToFetchFromMSAccess = null;
        private void btnFetch_Click(object sender, EventArgs e)
        {
            queryToFetchFromMSAccess = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '015' OR CO_BOOK = '15'";
            HelperFetchData.InsertInStockDataIntoSQLite(txtMessageBox, queryToFetchFromMSAccess);
        }
        private void btnFetchSaleData_Click(object sender, EventArgs e)
        {
            queryToFetchFromMSAccess = "SELECT * FROM MAIN_TAG_DATA WHERE CO_BOOK = '026' OR CO_BOOK = '26'";
            HelperFetchData.InsertSaleDataIntoSQLite( queryToFetchFromMSAccess);
        }
    }
}
