using Humanizer;
using SYA.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
namespace SYA
{
    public partial class ContactDisplay : Form
    {
        // -------------------------------- Variables needed -------------------------------
        Dictionary<string, string> columnDataDict = new Dictionary<string, string>();
        int selectedRowIndex = -1; // Initialize to -1 (no row selected)

        int row_index = 0;
        ContactHelper contactHelper = new ContactHelper();
        Color WrongDataColor = Color.FromArgb(255, 99, 71);      // Tomato Red
        Color VepariDataColor = Color.FromArgb(70, 130, 180);    // Steel Blue
        Color UnverifiedDataColor = Color.FromArgb(255, 215, 0); // Goldenrod
        Color RelativeDataColor = Color.FromArgb(147, 112, 219); // Medium Purple
        Color OtherDataColor = Color.FromArgb(211, 211, 211);    // Light Gray
        Color KarigarDataColor = Color.FromArgb(135, 206, 235);  // Sky Blue
        Color CustomerDataColor = Color.FromArgb(60, 179, 113);  // Medium Sea Green
        Color ExcludedDataColor = Color.FromArgb(169, 169, 169); // Dark Gray
        Color RawDataColor = Color.FromArgb(255, 140, 0);        // Dark Orange
        Color AllDataColor = Color.FromArgb(245, 245, 245);      // White Smoke
        Color WrongDataFontColor = Color.White;         // Bright background, white font
        Color VepariDataFontColor = Color.White;        // Dark background, white font
        Color UnverifiedDataFontColor = Color.Black;    // Bright background, black font
        Color RelativeDataFontColor = Color.White;      // Dark background, white font
        Color OtherDataFontColor = Color.Black;         // Light background, black font
        Color KarigarDataFontColor = Color.Black;       // Light background, black font
        Color CustomerDataFontColor = Color.White;      // Dark background, white font
        Color ExcludedDataFontColor = Color.Black;      // Medium background, black font
        Color RawDataFontColor = Color.White;           // Bright background, white font
        Color AllDataFontColor = Color.Black;           // Light background, black font
        // ------------------------------------- main --------------------------------
        public ContactDisplay()
        {
            InitializeComponent();
        }
        private void ContactDisplay_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.Sorted += dataGridView1_Sorted;
            dataGridView1.DataBindingComplete += dataGridView1_DataBindingComplete;
            dataGridView1.KeyDown += dataGridView1_KeyDown;
            dataGridView1.DoubleClick += dataGridView1_DoubleClick;
            dataGridView2.KeyDown += dataGridView2_KeyDown;
            checkboxChanges(sender, e);
            panel19.Visible = false;
        }
        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string tableName = null;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Value?.ToString() == "tableName")
                        {
                            if (i + 1 < row.Cells.Count)
                            {
                                tableName = row.Cells[i + 1].Value?.ToString();
                            }
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(tableName))
                {
                    switch (tableName)
                    {
                        case "CustomerData":
                            btnSaveCustomer.Focus();
                            break;
                        case "KarigarData":
                            btnSaveKarigar.Focus();
                            break;
                        case "OtherData":
                            btnSaveOther.Focus();
                            break;
                        case "RelativeData":
                            btnSaveRelative.Focus();
                            break;
                        case "VepariData":
                            btnSaveVepari.Focus();
                            break;
                        case "WrongData":
                            btnSaveWrong.Focus();
                            break;
                    }
                }
            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.RowTemplate.Height = 70;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                SetRowColors(row);
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridView1.CurrentRow != null)
            {
                int selectedRowIndex = dataGridView1.CurrentRow.Index;
                row_index = selectedRowIndex;
                UpdateDataGridView2(selectedRowIndex);
                MoveFocusToDataGridView2();
                e.Handled = true;
            }
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int selectedRowIndex = dataGridView1.CurrentRow.Index;
                UpdateDataGridView2(selectedRowIndex);
                MoveFocusToDataGridView2();
            }
        }
        private void MoveFocusToDataGridView2()
        {
            if (dataGridView2.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["Field"].Value.ToString() == "Rating")
                    {
                        dataGridView2.CurrentCell = row.Cells["Value"];
                        dataGridView2.Focus();
                        break;
                    }
                }
            }
        }
        // ------------------ inser and update query --------------------------------
        private void updateInsertData(string newTableName)
        {
            if (columnDataDict.ContainsKey("tableName"))
            {
                string currentTableName = columnDataDict["tableName"];
                if (currentTableName == newTableName)
                {

                    string conditionColumn = "id";
                    string conditionValue = columnDataDict["id"].ToString();
                    string updateQuery = GenerateUpdateQuery(columnDataDict, currentTableName, conditionColumn, conditionValue);
                    helper.RunQueryWithoutParametersSYAContactDataBase(updateQuery, "ExecuteNonQuery");
                }
                else
                {
                    string insertQuery = GenerateInsertQuery(columnDataDict, newTableName);
                    helper.RunQueryWithoutParametersSYAContactDataBase(insertQuery, "ExecuteNonQuery");


                    string deleteQuery = $"DELETE FROM {currentTableName} WHERE id = '{columnDataDict["id"].ToString()}';";
                    helper.RunQueryWithoutParametersSYAContactDataBase(deleteQuery, "ExecuteNonQuery");
                    //dataGridView1.Rows[row_index].DefaultCellStyle.BackColor = Color.Red;
                    row_index = -1;


                    if (selectedRowIndex >= 0 && selectedRowIndex < dataGridView1.Rows.Count)
                    {
                        dataGridView1.Rows.RemoveAt(selectedRowIndex);
                        selectedRowIndex = -1;
                    }
                }
            }
            else
            {
                string insertQuery = GenerateInsertQuery(columnDataDict, newTableName);
                columnDataDict["tableName"] = newTableName;
                helper.RunQueryWithoutParametersSYAContactDataBase(insertQuery, "ExecuteNonQuery");
            }
            dataGridView1.Focus();
            

        }

        private string GenerateInsertQuery(Dictionary<string, string> columnDataDict, string tableName)
        {
            UpdateDictionaryFromDataGridView2();
            columnDataDict["tableName"] = tableName;
            string columns = string.Join(", ", columnDataDict.Keys);
            string values = string.Join(", ", columnDataDict.Values.Select(v => $"'{v}'"));
            string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";
            return insertQuery;
        }
        private string GenerateUpdateQuery(Dictionary<string, string> columnDataDict, string tableName, string conditionColumn, string conditionValue)
        {
            UpdateDictionaryFromDataGridView2();
            string setClause = string.Join(", ", columnDataDict.Select(kv => $"{kv.Key} = '{kv.Value}'"));
            string updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {conditionColumn} = '{conditionValue}';";
            return updateQuery;
        }
        private void UpdateDictionaryFromDataGridView2()
        {
            //   ShowDictionary(columnDataDict);
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    string key = row.Cells["Field"].Value?.ToString();
                    string value = row.Cells["Value"].Value?.ToString();
                    if (key != null)
                    {
                        columnDataDict[key] = value ?? "null";
                    }
                }
            }
            //  ShowDictionary(columnDataDict);
        }
        // -------------------------------------- searchinggggggg -------------------------
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            search_all();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            search_by_name();
        }

        private void search_by_name()
        {
            string searchText = textBox2.Text;

            string query = $@"
SELECT * FROM (
    SELECT * FROM CustomerData
    UNION ALL
    SELECT * FROM ExcludedData
    UNION ALL
    SELECT * FROM KarigarData
    UNION ALL
    SELECT * FROM OtherData
    UNION ALL
    SELECT * FROM RelativeData
    UNION ALL
    SELECT * FROM UnverifiedData
    UNION ALL
    SELECT * FROM VepariData
    UNION ALL
    SELECT * FROM WrongData
)
WHERE 
    acName0 LIKE '%{searchText}%' OR
    acName1 LIKE '%{searchText}%' OR
    acName2 LIKE '%{searchText}%' OR
    acName3 LIKE '%{searchText}%' OR
    acName4 LIKE '%{searchText}%' OR
    acName5 LIKE '%{searchText}%' OR
    acName6 LIKE '%{searchText}%' OR
    acName7 LIKE '%{searchText}%' OR
    acName8 LIKE '%{searchText}%' OR
    acName9 LIKE '%{searchText}%' OR
    acName10 LIKE '%{searchText}%';
";

            DataTable table = new DataTable();
            table = helper.FetchDataTableFromSYAContactDataBase(query);
            showData(table);
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            search_by_mobile();
        }

        private void search_by_mobile()
        {
            string searchText = textBox3.Text;

            string query = $@"
SELECT * FROM (
    SELECT * FROM CustomerData
    UNION ALL
    SELECT * FROM ExcludedData
    UNION ALL
    SELECT * FROM KarigarData
    UNION ALL
    SELECT * FROM OtherData
    UNION ALL
    SELECT * FROM RelativeData
    UNION ALL
    SELECT * FROM UnverifiedData
    UNION ALL
    SELECT * FROM VepariData
    UNION ALL
    SELECT * FROM WrongData
)
WHERE 
    acMobile LIKE '%{searchText}%';
";

            DataTable table = new DataTable();
            table = helper.FetchDataTableFromSYAContactDataBase(query);
            showData(table);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            search_by_pan();
        }

        private void search_by_pan()
        {
            string searchText = textBox4.Text;

            string query = $@"
SELECT * FROM (
    SELECT * FROM CustomerData
    UNION ALL
    SELECT * FROM ExcludedData
    UNION ALL
    SELECT * FROM KarigarData
    UNION ALL
    SELECT * FROM OtherData
    UNION ALL
    SELECT * FROM RelativeData
    UNION ALL
    SELECT * FROM UnverifiedData
    UNION ALL
    SELECT * FROM VepariData
    UNION ALL
    SELECT * FROM WrongData
)
WHERE 
    acPan LIKE '%{searchText}%';
";

            DataTable table = new DataTable();
            table = helper.FetchDataTableFromSYAContactDataBase(query);
            showData(table);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            search_by_aadhaar();
        }

        private void search_by_aadhaar()
        {
            string searchText = textBox5.Text;

            string query = $@"
SELECT * FROM (
    SELECT * FROM CustomerData
    UNION ALL
    SELECT * FROM ExcludedData
    UNION ALL
    SELECT * FROM KarigarData
    UNION ALL
    SELECT * FROM OtherData
    UNION ALL
    SELECT * FROM RelativeData
    UNION ALL
    SELECT * FROM UnverifiedData
    UNION ALL
    SELECT * FROM VepariData
    UNION ALL
    SELECT * FROM WrongData
)
WHERE 
    acAdhaar LIKE '%{searchText}%';
";

            DataTable table = new DataTable();
            table = helper.FetchDataTableFromSYAContactDataBase(query);
            showData(table);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            search_by_address();
        }

        private void search_by_address()
        {
            string searchText = textBox6.Text;

            string query = $@"
SELECT * FROM (
    SELECT * FROM CustomerData
    UNION ALL
    SELECT * FROM ExcludedData
    UNION ALL
    SELECT * FROM KarigarData
    UNION ALL
    SELECT * FROM OtherData
    UNION ALL
    SELECT * FROM RelativeData
    UNION ALL
    SELECT * FROM UnverifiedData
    UNION ALL
    SELECT * FROM VepariData
    UNION ALL
    SELECT * FROM WrongData
)
WHERE 
    acAdd LIKE '%{searchText}%' OR
    acCity LIKE '%{searchText}%';
";

            DataTable table = new DataTable();
            table = helper.FetchDataTableFromSYAContactDataBase(query);
            showData(table);
        }
        private void search_all()
        {
            string searchText = textBox1.Text;

            string query = $@"
SELECT * FROM (
    SELECT * FROM CustomerData
    UNION ALL
    SELECT * FROM ExcludedData
    UNION ALL
    SELECT * FROM KarigarData
    UNION ALL
    SELECT * FROM OtherData
    UNION ALL
    SELECT * FROM RelativeData
    UNION ALL
    SELECT * FROM UnverifiedData
    UNION ALL
    SELECT * FROM VepariData
    UNION ALL
    SELECT * FROM WrongData
)
WHERE 
    acCode LIKE '%{searchText}%' OR
    acName0 LIKE '%{searchText}%' OR
    acName1 LIKE '%{searchText}%' OR
    acName2 LIKE '%{searchText}%' OR
    acName3 LIKE '%{searchText}%' OR
    acName4 LIKE '%{searchText}%' OR
    acName5 LIKE '%{searchText}%' OR
    acName6 LIKE '%{searchText}%' OR
    acName7 LIKE '%{searchText}%' OR
    acName8 LIKE '%{searchText}%' OR
    acName9 LIKE '%{searchText}%' OR
    acName10 LIKE '%{searchText}%' OR
    acAdd LIKE '%{searchText}%' OR
    acCity LIKE '%{searchText}%' OR
    acMobile LIKE '%{searchText}%' OR
    acPan LIKE '%{searchText}%' OR
    acAdhaar LIKE '%{searchText}%' OR
    acGroup LIKE '%{searchText}%' OR
    acSource LIKE '%{searchText}%' OR
    lastModifiedDate LIKE '%{searchText}%' OR
    tableName LIKE '%{searchText}%';
";

            DataTable table = new DataTable();
            table = helper.FetchDataTableFromSYAContactDataBase(query);
            showData(table);
        }
        // -------------------------------------- Save Buttons ----------------------------
        private void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            updateInsertData("CustomerData");
            dataGridView1.Focus();
        }
        private void btnSaveKarigar_Click(object sender, EventArgs e)
        {
            updateInsertData("KarigarData");
        }
        private void btnSaveOther_Click(object sender, EventArgs e)
        {
            updateInsertData("OtherData");
        }
        private void btnSaveVepari_Click(object sender, EventArgs e)
        {
            updateInsertData("VepariData");
        }
        private void btnSaveWrong_Click(object sender, EventArgs e)
        {
            updateInsertData("WrongData");
        }
        private void btnSaveRelative_Click(object sender, EventArgs e)
        {
            updateInsertData("RelativeData");
        }
        // ------------------ Update DatagridView2 base on datagridview1 ------------------------------
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedRowIndex = e.RowIndex; // Store the selected row index
                UpdateDataGridView2(selectedRowIndex);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                selectedRowIndex = dataGridView1.CurrentRow.Index; // Store the selected row index
                UpdateDataGridView2(selectedRowIndex);
            }
        }
        private void UpdateDataGridView2(int rowIndex)
        {
            columnDataDict.Clear();
            DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];
            labelDetailName.Text = selectedRow.Cells["acName0"].Value?.ToString();
            DataTable dt = new DataTable();
            dt.Columns.Add("Field");
            dt.Columns.Add("Value");
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                var value = selectedRow.Cells[col.Index].Value?.ToString();
                columnDataDict[col.HeaderText] = value ?? "null";
                if (!string.IsNullOrEmpty(value))
                {
                    DataRow row = dt.NewRow();
                    row["Field"] = col.HeaderText;
                    row["Value"] = value;
                    dt.Rows.Add(row);
                }
            }
            dataGridView2.DataSource = dt;
            int rowHeight = dataGridView2.RowTemplate.Height;
            int numRows = dt.Rows.Count;
            dataGridView2.Height = numRows * rowHeight + dataGridView2.ColumnHeadersHeight;
        }
        private void ShowDictionary(Dictionary<string, string> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in data)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            MessageBox.Show(sb.ToString(), "Dictionary Contents");
        }
        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            datagridview1_look();
        }
        // -----------------------------------------------------------------------------------
        private void showData(DataTable dt)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = dt;
            labelContactCount.Text = "Contact Count : " + dt.Rows.Count;
            UpdateDataGridViewColumnsVisibility();
            datagridview1_look();
        }
        private void SetRowColors(DataGridViewRow row)
        {
            string tableName = row.Cells["tableName"].Value?.ToString();
            if (!string.IsNullOrEmpty(tableName))
            {
                switch (tableName)
                {
                    case "WrongData":
                        row.DefaultCellStyle.BackColor = WrongDataColor;
                        row.DefaultCellStyle.ForeColor = WrongDataFontColor;
                        break;
                    case "VepariData":
                        row.DefaultCellStyle.BackColor = VepariDataColor;
                        row.DefaultCellStyle.ForeColor = VepariDataFontColor;
                        break;
                    case "UnverifiedData":
                        row.DefaultCellStyle.BackColor = UnverifiedDataColor;
                        row.DefaultCellStyle.ForeColor = UnverifiedDataFontColor;
                        break;
                    case "RelativeData":
                        row.DefaultCellStyle.BackColor = RelativeDataColor;
                        row.DefaultCellStyle.ForeColor = RelativeDataFontColor;
                        break;
                    case "OtherData":
                        row.DefaultCellStyle.BackColor = OtherDataColor;
                        row.DefaultCellStyle.ForeColor = OtherDataFontColor;
                        break;
                    case "KarigarData":
                        row.DefaultCellStyle.BackColor = KarigarDataColor;
                        row.DefaultCellStyle.ForeColor = KarigarDataFontColor;
                        break;
                    case "CustomerData":
                        row.DefaultCellStyle.BackColor = CustomerDataColor;
                        row.DefaultCellStyle.ForeColor = CustomerDataFontColor;
                        break;
                    case "ExcludedData":
                        row.DefaultCellStyle.BackColor = ExcludedDataColor;
                        row.DefaultCellStyle.ForeColor = ExcludedDataFontColor;
                        break;
                    case "RawData":
                        row.DefaultCellStyle.BackColor = RawDataColor;
                        row.DefaultCellStyle.ForeColor = RawDataFontColor;
                        break;
                    case "AllData":
                        row.DefaultCellStyle.BackColor = AllDataColor;
                        row.DefaultCellStyle.ForeColor = AllDataFontColor;
                        break;
                    default:
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;
                }
            }
        }
        // Updated datagridview1_look function
        private void datagridview1_look()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                SetRowColors(row);
            }
            dataGridView1.RowTemplate.Height = 70;
        }
        //------------------------------------- Done ----------------------------------------------------
        //------------------------------------------- Check Box Changes -------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            if (panel19.Visible) { panel19.Visible = false; }
            else { panel19.Visible = true; }
        }
        private void checkboxChanges(object sender, EventArgs e)
        {
            checkBox1.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox2.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox3.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox4.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox5.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox6.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox7.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox8.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox9.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox10.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox11.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox12.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox13.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox14.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox15.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox16.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox17.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox18.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox19.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox20.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox21.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox22.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox23.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox24.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox25.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox26.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox27.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox28.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox29.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
            checkBox30.CheckedChanged += (s, e) => UpdateDataGridViewColumnsVisibility();
        }
        private void UpdateDataGridViewColumnsVisibility()
        {
            Dictionary<CheckBox, string> checkBoxColumnMapping = new Dictionary<CheckBox, string>
    {
        { checkBox1, "id" }, // replace "ColumnName1" with the actual column name
        { checkBox2, "acCode" }, // replace "ColumnName2" with the actual column name
        { checkBox3, "acAdd" }, // replace "ColumnName3" with the actual column name
        { checkBox4, "acCity" }, // replace "ColumnName4" with the actual column name
        { checkBox5, "acMobile" }, // replace "ColumnName5" with the actual column name
        { checkBox6, "acPan" }, // and so on...
        { checkBox7, "acAdhaar" },
        { checkBox8, "acGroup" },
        { checkBox9, "acSource" },
        { checkBox10, "lastModifiedDate" },
        { checkBox11, "tableName" },
        { checkBox12, "acName0" },
        { checkBox13, "acName1" },
        { checkBox14, "acName2" },
        { checkBox15, "acName3" },
        { checkBox16, "acName4" },
        { checkBox17, "acName5" },
        { checkBox18, "acName6" },
        { checkBox19, "acName7" },
        { checkBox20, "acName8" },
        { checkBox21, "acName9" },
        { checkBox22, "acName10" },
        { checkBox23, "ColumnName23" },
        { checkBox24, "ColumnName24" },
        { checkBox25, "ColumnName25" },
        { checkBox26, "ColumnName26" },
        { checkBox27, "ColumnName27" },
        { checkBox28, "ColumnName28" },
        { checkBox29, "ColumnName29" },
        { checkBox30, "ColumnName30" }
    };
            foreach (var mapping in checkBoxColumnMapping)
            {
                CheckBox checkBox = mapping.Key;
                string columnName = mapping.Value;
                if (dataGridView1.Columns.Contains(columnName))
                {
                    dataGridView1.Columns[columnName].Visible = checkBox.Checked;
                }
            }
        }
        // -------------------------------------- Show Data Table Buttons --------------------------------------------
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("CustomerData"));
        }
        private void btnExcluded_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("ExcludedData"));
        }
        private void btnKarigar_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("KarigarData"));
        }
        private void btnOther_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("OtherData"));
        }
        private void btnRelative_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("RelativeData"));
        }
        private void btnVepari_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("VepariData"));
        }
        private void btnWrong_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("WrongData"));
        }
        private void btnUnverified_Click(object sender, EventArgs e)
        {
            showData(contactHelper.getDataTable("UnverifiedData"));
        }
        private void btnRaw_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Coded Yet .............");
        }
        private void btnAll_Click(object sender, EventArgs e)
        {
            DataTable allData = new DataTable();
            List<string> tableNames = new List<string>
        {
            "WrongData",
            "VepariData",
            "UnverifiedData",
            "RelativeData",
            "OtherData",
            "KarigarData",
            "CustomerData",
            "ExcludedData"
        };
            foreach (string tableName in tableNames)
            {
                DataTable tableData = contactHelper.getDataTable(tableName);
                allData.Merge(tableData);
            }
            showData(allData);
        }

       
    }
}
