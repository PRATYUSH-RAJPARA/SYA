using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public partial class DataCareData : Form
    {
        private SQLiteConnection connectionToSYADatabase;
        public DataCareData()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void DataCareData_Load(object sender, EventArgs e)
        {

        }
        private void InitializeDatabaseConnection()
        {
            connectionToSYADatabase = new SQLiteConnection("Data Source=C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\SYADataBase.db;Version=3;");
            // connectionToDatacare = new SQLiteConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\pvraj\\OneDrive\\Desktop\\DataCare23 - Copy.mdb\";");

        }
        private void LoadDataFromSQLite(String query)
        {
            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                {
                    sqliteConnection.Open();

                    // SELECT query to retrieve data from MAIN_DATA


                    using (SQLiteCommand command = new SQLiteCommand(query, sqliteConnection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Clear existing rows in the DataGridView
                            dataCareDataGridView.Rows.Clear();

                            // Iterate through the result set and populate the DataGridView
                            while (reader.Read())
                            {
                                // Add a new row to the DataGridView
                                int rowIndex = dataCareDataGridView.Rows.Add();

                                // Set values for each column in the DataGridView
                                dataCareDataGridView.Rows[rowIndex].Cells["tagno"].Value = reader["TAG_NO"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["huid1"].Value = reader["HUID1"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["huid2"].Value = reader["HUID2"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["comment"].Value = reader["COMMENT"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["gross"].Value = reader["GW"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["net"].Value = reader["NW"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["type"].Value = reader["ITEM_DESC"].ToString() + " - " + reader["ITEM_PURITY"].ToString();
                                dataCareDataGridView.Rows[rowIndex].Cells["datetime"].Value = reader["VCH_DATE"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowDataCareData_Click(object sender, EventArgs e)
        {
            string query = "SELECT TAG_NO, HUID1, HUID2, COMMENT, GW, NW, ITEM_DESC, ITEM_PURITY,VCH_DATE " +
                                   "FROM MAIN_DATA " +
                                   "WHERE VCH_NO <> 'SYA00' " +
                                   "ORDER BY VCH_DATE DESC";
            LoadDataFromSQLite(query);
        }

        private void btnShowSyaData_Click(object sender, EventArgs e)
        {
            string query = "SELECT TAG_NO, HUID1, HUID2, COMMENT, GW, NW, ITEM_DESC, ITEM_PURITY,VCH_DATE " +
                                   "FROM MAIN_DATA " +
                                   "WHERE VCH_NO = 'SYA00' " +
                                   "ORDER BY VCH_DATE DESC";
            LoadDataFromSQLite(query);
        }


        private void txtDataCareDataNoOfDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LoadDataForLastXDays();
                e.Handled = true; // Prevent the Enter key from being processed further
            }
        }

        private void LoadDataForLastXDays()
        {
            if (int.TryParse(txtDataCareDataNoOfDays.Text, out int numberOfDays))
            {
                DateTime startDate = DateTime.Today.AddDays(-numberOfDays);
                string query = $"SELECT TAG_NO, HUID1, HUID2, COMMENT, GW, NW, ITEM_DESC, ITEM_PURITY, VCH_DATE " +
                               $"FROM MAIN_DATA " +
                               $"WHERE VCH_DATE >= '{startDate.ToString("yyyy-MM-dd")}' " +
                               $"ORDER BY VCH_DATE DESC";
                LoadDataFromSQLite(query);
            }
            else
            {
                MessageBox.Show("Please enter a valid number of days.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDataCareDataSave_Click(object sender, EventArgs e)
        {
            SaveDataToSQLite();
        }
        private void SaveDataToSQLite()
        {
            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(connectionToSYADatabase))
                {
                    sqliteConnection.Open();

                    foreach (DataGridViewRow row in dataCareDataGridView.Rows)
                    {
                        string tagNo = row.Cells["tagno"].Value.ToString();
                        string huid1 = row.Cells["huid1"].Value?.ToString().ToUpper(); // Convert to uppercase
                        string huid2 = row.Cells["huid2"].Value?.ToString().ToUpper(); // Convert to uppercase
                        string comment = row.Cells["comment"].Value?.ToString();

                        // Validate HUID1 and HUID2
                        if ((!string.IsNullOrWhiteSpace(huid1) && huid1.Length != 6) || (!string.IsNullOrWhiteSpace(huid2) && huid2.Length != 6))
                        {
                            MessageBox.Show("HUID length must be 6 characters if not null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Exit the method if validation fails
                        }

                        // Validate if HUID1 is null and HUID2 is not null
                        if (string.IsNullOrWhiteSpace(huid1) && !string.IsNullOrWhiteSpace(huid2))
                        {
                            MessageBox.Show("Please insert HUID1 at the correct place.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // Exit the method if validation fails
                        }

                        // Construct the UPDATE query
                        string updateQuery = $"UPDATE MAIN_DATA SET HUID1 = '{huid1}', HUID2 = '{huid2}', COMMENT = '{comment}' WHERE TAG_NO = '{tagNo}'";

                        using (SQLiteCommand updateCommand = new SQLiteCommand(updateQuery, sqliteConnection))
                        {
                            updateCommand.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to SQLite: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDataCareDataSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataCareDataGridView.Rows.Count; i++)
            {
                dataCareDataGridView.Rows[i].Cells["select"].Value = true;
            }
        }
    }
}
