using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;

namespace SYA
{
    public partial class SaleItem : Form
    {
        DataTable syaTable = new DataTable();

        public SaleItem()
        {
            InitializeComponent();
        }

        private void SaleItem_Load(object sender, EventArgs e)
        {
            btnSell.Visible = false;
            dataGridView1.Visible = false;
            textBox1.Text = "BUG0809";
        }

        private void LoadData(string query)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(helper.SYAConnectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            SQLiteDataReader reader = command.ExecuteReader();
                            syaTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"FetchDataTableFromSYADataBase Error executing query and filling DataTable: {ex.Message}");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                using (SQLiteConnection connection = new SQLiteConnection(helper.SYAConnectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            DataTable transposedTable = TransposeDataReader(reader);
                            LoadDataIntoDataGridView(transposedTable);
                            SetSaleItemProperties(this, transposedTable);
                            dataGridView1.Visible = true;
                            btnSell.Visible = true;
                        }
                        else
                        {
                            textBox1.Focus();
                            textBox1.Clear();
                            MessageBox.Show("No rows found in the result set.", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.Focus();
                textBox1.Clear();
                MessageBox.Show($"Error: {ex.Message} ", "Error");
            }
        }

        private void LoadDataIntoDataGridView(DataTable dataTable)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.RowHeadersVisible = false;

            foreach (DataColumn col in dataTable.Columns)
                dataGridView1.Columns.Add(col.ColumnName, col.ColumnName);

            dataGridView1.Columns[0].Width = 198;
            dataGridView1.Columns[1].Width = 198;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataRow row in dataTable.Rows)
            {
                dataGridView1.Rows.Add(row.ItemArray);
            }
        }

        private DataTable TransposeDataReader(SQLiteDataReader reader)
        {
            DataTable transposedTable = new DataTable();
            transposedTable.Columns.Add("Field", typeof(string));
            transposedTable.Columns.Add("Value", typeof(object));

            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    transposedTable.Rows.Add(reader.GetName(i), reader[i]);
            }

            return transposedTable;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.Text = textBox1.Text.ToUpper();
                string query = "SELECT * FROM MAIN_DATA WHERE TAG_NO = '" + textBox1.Text + "'";
                LoadData(query);
            }
        }

        // sell
        private void btnSell_Click(object sender, EventArgs e)
        {
            DataRow syaDataRow = syaTable.Rows[0];
            DataTable saleTable = helper.FetchDataTableFromSYADataBase("SELECT * FROM SALE_DATA WHERE TAG_NO = '" + syaDataRow["TAG_NO"].ToString() + "'");
            DataRow saleDataRow = saleTable.Rows.Count > 0 ? saleTable.Rows[0] : null;

            if (syaDataRow != null)
            {
                try
                {
                    if (syaDataRow["IT_TYPE"].ToString() == "G")
                    {
                        if (syaDataRow["VCH_NO"].ToString() == "SYA00")
                        {
                            if (saleTable != null && saleTable.Rows.Count > 0)
                            {
                                string updateQuery = "SELECT * FROM MAIN_TAG_DATA WHERE TAG_NO = '" + syaDataRow["TAG_NO"].ToString() + "' AND (CO_BOOK = '026' OR CO_BOOK = '26')";
                                HelperFetchData.InsertSaleDataIntoSQLite(updateQuery);

                                String deleteQuery = "DELETE FROM MAIN_DATA WHERE TAG_NO = '" + syaDataRow["TAG_NO"].ToString() + "'";
                                helper.RunQueryWithoutParametersSYADataBase(deleteQuery, "ExecuteNonQuery");
                                MessageBox.Show("selling sya00 huid");
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("Bill for Tag-Number " + syaDataRow["TAG_NO"].ToString() + " is not generated, Do you want to sell?", "Confirmation - " + syaDataRow["TAG_NO"].ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    MessageBox.Show("selling sya00 yes");
                                    // sell
                                }
                                else
                                {
                                    MessageBox.Show("No");
                                    // User clicked No or closed the dialog, handle accordingly
                                    // Add your logic here
                                }
                            }
                        }
                        else if (syaDataRow["VCH_NO"].ToString() == "SYA01")
                        {
                            if (syaDataRow["HUID1"].ToString().Length > 0 || syaDataRow["HUID2"].ToString().Length > 0)
                            {
                                MessageBox.Show("selling sya01 huid");
                                //sell
                                //check if already sell or not
                            }
                            else
                            {
                                MessageBox.Show("delete and count sya01");
                                //delete and add count
                            }
                        }
                        else
                        {
                            MessageBox.Show("Wrong VCH_NO (: " + syaDataRow["VCH_NO"].ToString() + " :) . Please contact the Developer ....");
                        }
                    }
                    else if (syaDataRow["IT_TYPE"].ToString() == "S")
                    {
                        // Additional logic for IT_TYPE == "S"
                    }
                    else
                    {
                        MessageBox.Show("Wrong Item Type (IT_TYPE) (: " + syaDataRow["IT_TYPE"].ToString() + " :). Please contact the Developer ....");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("No rows found or reader not initialized.");
            }

            btnSell.Visible = false;
            textBox1.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Visible = false;
        }

        private void SetSaleItemProperties(SaleItem saleItem, DataTable transposedTable)
        {
            foreach (DataRow row in transposedTable.Rows)
            {
                string columnName = row["Field"].ToString();
                var property = typeof(SaleItem).GetProperty(columnName);

                if (property != null)
                {
                    object value = Convert.ChangeType(row["Value"], property.PropertyType);
                    property.SetValue(saleItem, value);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Get the SQL query from the richTextBox
            //  string query = richTextBox1.Text;

            // Call the helper function to fetch data
            //DataTable dataCareDataTable = helper.FetchFromDataCareDataBase(query);

            // Set the DataTable as the DataSource for the DataGridView
            //  dataGridView2.DataSource = dataCareDataTable;

            // Display column names in the first row
            // dataGridView2.ColumnHeadersVisible = true;
        }
    }
}
