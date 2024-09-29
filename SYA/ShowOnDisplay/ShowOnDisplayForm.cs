using SYA.Helper;
using SYA.Sales;
using System;
using System.Data;
using System.IO.Ports;
using System.Media;
using System.Windows.Forms;

namespace SYA
{
    public partial class ShowOnDisplayForm : Form
    {
        SerialPort serialPort;
        private SoundPlayer deleteSound;
        DataTable sellItem = new DataTable();

        public ShowOnDisplayForm()
        {
            InitializeComponent();

            serialPort = new SerialPort("COM3", 9600);  // Change "COM3" to your Arduino's COM port
        }

        private void ShowOnDisplayForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening serial port: {ex.Message}");
            }
        }

        private void ShowOnDisplayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing serial port: {ex.Message}");
            }
        }

        private void ReplaceNullValuesWithEmptyString(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (row.IsNull(col) || string.IsNullOrEmpty(row[col].ToString()))
                    {
                        if (col.DataType == typeof(string))
                        {
                            row[col] = "";  // Set empty string for string columns
                        }
                        else if (col.DataType == typeof(decimal))
                        {
                            row[col] = 0;  // Set 0 for decimal columns
                        }
                    }
                }
            }
        }

        private void showPrice(string tagNo)
        {
            sellItem.Clear();

            // Fetch the data based on TAG_NO
            sellItem = helper.FetchDataTableFromSYADataBase("SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + tagNo + "'");

            if (sellItem.Rows.Count > 0)
            {
                ReplaceNullValuesWithEmptyString(sellItem);

                // Parse the necessary fields from the fetched data
                float gw = float.Parse(sellItem.Rows[0]["GW"].ToString());
                float nw = float.Parse(sellItem.Rows[0]["NW"].ToString());
                float l = float.Parse(sellItem.Rows[0]["LABOUR_AMT"].ToString());
                float wl = float.Parse(sellItem.Rows[0]["WHOLE_LABOUR_AMT"].ToString());
                float o = float.Parse(sellItem.Rows[0]["OTHER_AMT"].ToString());

                // Calculate the price
                float price = (nw * (6950 + l) + wl + o);

                // Convert price to an integer (removing decimal)
                long priceInt = (long)Math.Floor(price);

                // Send the entire number to Arduino
                if (serialPort.IsOpen)
                {
                    string numberToSend = priceInt.ToString();  // Convert the entire price to a string

                    serialPort.WriteLine(numberToSend);  // Send the entire number to Arduino
                }

                // Display the detailed values on the label
                label25.Text = $"{gw} {nw} {l} {o} {wl} {priceInt}";
            }
            else
            {
                label25.Text = $"Item {tagNo} was not found or might have been sold and deleted.";
            }
        }


        private void showOnDisplay(long numberToSend)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    
                    serialPort.WriteLine(numberToSend.ToString());  // Write the number followed by a newline
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending data to the display: {ex.Message}");
            }
        }

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.Text = textBox1.Text.ToUpper();

                showPrice(textBox1.Text);
                textBox1.Text = string.Empty;
            }
        }
    }
}
