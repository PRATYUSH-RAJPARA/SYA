using Newtonsoft.Json;
using SYA.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SYA
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }
        private void settings_Load(object sender, EventArgs e)
        {
           
            DataTable dt = helper.dt1;
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                int columnIndex = 0;
                foreach (DataColumn column in dt.Columns)
                {
                    if (columnIndex != 0) // Skip the first column
                    {
                        CreateNestedPanelAndLabels(panel5, i, column.ColumnName, row[column].ToString());
                    }
                    columnIndex++;
                }
            }
            // Call the API when the form loads and print the values in a message box
        }
        private void CreateNestedPanelAndLabels(Panel parentPanel, int index, string columnName, string value)
        {
            // Create a new panel
            Panel newPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.LightGray // Just for better visibility
            };
            parentPanel.Controls.Add(newPanel);
            parentPanel.Controls.SetChildIndex(newPanel, 0); // Add to the top
            // Create the first label
            TextBox text1 = new TextBox
            {
                Text = value,
                Width = 900,
                Dock = DockStyle.Left,
                TextAlign = HorizontalAlignment.Left,
                PlaceholderText = value,
                BackColor = Color.White, // Just for better visibility
                Font = new Font("Arial", (float)12.5) // Change the font and size here
            };
            newPanel.Controls.Add(text1);
            Panel innerPanel1 = new Panel
            {
                Width = 10,
                Dock = DockStyle.Left,
            };
            newPanel.Controls.Add(innerPanel1);
            // Create the inner panel
            Panel innerPanel = new Panel
            {
                Width = 300,
                Dock = DockStyle.Left,
                BackColor = Color.LightCoral // Just for better visibility
            };
            newPanel.Controls.Add(innerPanel);
            // Create the second label inside the inner panel
            Label label2 = new Label
            {
                Text = columnName,
                Width = 300,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(255, 3, 63, 99), // Just for better visibility
                Font = new Font("Arial", (float)15) // Change the font and size here
            };
            label2.ForeColor = Color.White;
            innerPanel.Controls.Add(label2);
        }
        private void UpdateDatabase()
        {
            try
            {
                foreach (Control control in panel5.Controls)
                {
                    if (control is Panel panel)
                    {
                        TextBox textBox = panel.Controls.OfType<TextBox>().FirstOrDefault();
                        Label label = panel.Controls.OfType<Panel>().SelectMany(p => p.Controls.OfType<Label>()).FirstOrDefault();
                        if (textBox != null && label != null)
                        {
                            // Construct the update query
                            string query = $"UPDATE Settings SET {label.Text} = '{textBox.Text}' WHERE ID = '1'"; // Replace 1=1 with the actual condition
                            // Execute the query using the helper function
                            helper.RunQueryWithoutParametersSyaSettingsDataBase(query, "ExecuteNonQuery");
                        }
                    }
                }
                MessageBox.Show("Settings updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDatabase();
            helper.loadSettingsValues();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        // Method to fetch data from the API and display in a message box
    }
}