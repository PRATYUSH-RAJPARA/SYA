using Microsoft.Office.Interop.Excel;
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
    public partial class STOCKSummary : Form
    {
        public STOCKSummary()
        {
            InitializeComponent();
        }
        private void STOCKSummary_Load(object sender, EventArgs e)
        {
            panel57.Visible = false;
            colorss();
            LOAD("1");
            label98.Focus();
        }
        private void LOAD(string huid)
        {
            if (huid == "huid")
            {
                try
                {
                    string query = "SELECT * FROM MAIN_DATA";
                    DataTable dt = helper.FetchDataTableFromSYADataBase(query);
                    // Create a new DataTable with the same structure as slDataTable
                    // Manually map the columns and populate the new DataTable
                    decimal[,] sums = new decimal[6, 6];
                    decimal[] count = new decimal[6];
                    for (int row = 0; row < 6; row++)
                    {
                        count[row] = 0;
                        for (int col = 0; col < 6; col++)
                        {
                            sums[row, col] = 0m; // Although this is redundant as the default value is already 0
                        }
                    }
                    int COUNT = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        COUNT++;
                        decimal nw = Convert.ToDecimal(row["NW"] == DBNull.Value ? 0 : row["NW"]);
                        decimal gw = Convert.ToDecimal(row["GW"] == DBNull.Value ? 0 : row["GW"]);
                        decimal labourAmt = Convert.ToDecimal(row["LABOUR_AMT"] == DBNull.Value ? 0 : row["LABOUR_AMT"]);
                        decimal wholeLabourAmt = Convert.ToDecimal(row["WHOLE_LABOUR_AMT"] == DBNull.Value ? 0 : row["WHOLE_LABOUR_AMT"]);
                        decimal otherAmt = Convert.ToDecimal(row["OTHER_AMT"] == DBNull.Value ? 0 : row["OTHER_AMT"]);
                        decimal price = Convert.ToDecimal(row["PRICE"] == DBNull.Value ? 0 : row["PRICE"]);
                        if (row["IT_TYPE"].ToString() == "G" && ((row["HUID1"] == DBNull.Value ? 0 : row["HUID1"]).ToString().Length == 6 || (row["HUID2"] == DBNull.Value ? 0 : row["HUID2"]).ToString().Length == 6))
                        {
                            if (row["ITEM_PURITY"].ToString() == "916")
                            {
                                sums[0, 0]++;
                                sums[1, 0] += nw;
                                sums[2, 0] += gw;
                                sums[3, 0] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 0] += otherAmt;
                                sums[5, 0] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "20K")
                            {
                                sums[0, 1]++;
                                sums[1, 1] += nw;
                                sums[2, 1] += gw;
                                sums[3, 1] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 1] += otherAmt;
                                sums[5, 1] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "18K")
                            {
                                sums[0, 2]++;
                                sums[1, 2] += nw;
                                sums[2, 2] += gw;
                                sums[3, 2] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 2] += otherAmt;
                                sums[5, 2] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "KDM")
                            {
                                sums[0, 3]++;
                                sums[1, 3] += nw;
                                sums[2, 3] += gw;
                                sums[3, 3] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 3] += otherAmt;
                                sums[5, 3] += price;
                            }
                        }
                    }
                    label92.Text = sums[0, 0].ToString();
                    label91.Text = sums[1, 0].ToString();
                    label90.Text = sums[2, 0].ToString();
                    label89.Text = sums[3, 0].ToString();
                    label88.Text = sums[4, 0].ToString();
                    label87.Text = sums[5, 0].ToString();
                    label85.Text = sums[0, 1].ToString();
                    label84.Text = sums[1, 1].ToString();
                    label83.Text = sums[2, 1].ToString();
                    label82.Text = sums[3, 1].ToString();
                    label81.Text = sums[4, 1].ToString();
                    label80.Text = sums[5, 1].ToString();
                    label78.Text = sums[0, 2].ToString();
                    label77.Text = sums[1, 2].ToString();
                    label76.Text = sums[2, 2].ToString();
                    label75.Text = sums[3, 2].ToString();
                    label74.Text = sums[4, 2].ToString();
                    label73.Text = sums[5, 2].ToString();
                    label71.Text = sums[0, 3].ToString();
                    label70.Text = sums[1, 3].ToString();
                    label69.Text = sums[2, 3].ToString();
                    label68.Text = sums[3, 3].ToString();
                    label67.Text = sums[4, 3].ToString();
                    label66.Text = sums[5, 3].ToString();
                    panel8.Visible = true;
                    panel10.Visible = true;
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately, e.g., show a message box
                    MessageBox.Show($"Error2: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    string query = "SELECT * FROM MAIN_DATA";
                    DataTable dt = helper.FetchDataTableFromSYADataBase(query);
                    // Create a new DataTable with the same structure as slDataTable
                    // Manually map the columns and populate the new DataTable
                    decimal[,] sums = new decimal[6, 6];
                    decimal[] count = new decimal[6];
                    for (int row = 0; row < 6; row++)
                    {
                        count[row] = 0;
                        for (int col = 0; col < 6; col++)
                        {
                            sums[row, col] = 0m; // Although this is redundant as the default value is already 0
                        }
                    }
                    int COUNT = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        COUNT++;
                        decimal nw = Convert.ToDecimal(row["NW"] == DBNull.Value ? 0 : row["NW"]);
                        decimal gw = Convert.ToDecimal(row["GW"] == DBNull.Value ? 0 : row["GW"]);
                        decimal labourAmt = Convert.ToDecimal(row["LABOUR_AMT"] == DBNull.Value ? 0 : row["LABOUR_AMT"]);
                        decimal wholeLabourAmt = Convert.ToDecimal(row["WHOLE_LABOUR_AMT"] == DBNull.Value ? 0 : row["WHOLE_LABOUR_AMT"]);
                        decimal otherAmt = Convert.ToDecimal(row["OTHER_AMT"] == DBNull.Value ? 0 : row["OTHER_AMT"]);
                        decimal price = Convert.ToDecimal(row["PRICE"] == DBNull.Value ? 0 : row["PRICE"]);
                        if (row["IT_TYPE"].ToString() == "G")
                        {
                            if (row["ITEM_PURITY"].ToString() == "916")
                            {
                                sums[0, 0]++;
                                sums[1, 0] += nw;
                                sums[2, 0] += gw;
                                sums[3, 0] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 0] += otherAmt;
                                sums[5, 0] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "20K")
                            {
                                sums[0, 1]++;
                                sums[1, 1] += nw;
                                sums[2, 1] += gw;
                                sums[3, 1] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 1] += otherAmt;
                                sums[5, 1] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "18K")
                            {
                                sums[0, 2]++;
                                sums[1, 2] += nw;
                                sums[2, 2] += gw;
                                sums[3, 2] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 2] += otherAmt;
                                sums[5, 2] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "KDM")
                            {
                                sums[0, 3]++;
                                sums[1, 3] += nw;
                                sums[2, 3] += gw;
                                sums[3, 3] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 3] += otherAmt;
                                sums[5, 3] += price;
                            }
                        }
                        else if (row["IT_TYPE"].ToString() == "S")
                        {
                            if (row["ITEM_PURITY"].ToString() == "SLO")
                            {
                                sums[0, 4]++;
                                sums[1, 4] += nw;
                                sums[2, 4] += gw;
                                sums[3, 4] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 4] += otherAmt;
                                sums[5, 4] += price;
                            }
                            else if (row["ITEM_PURITY"].ToString() == "925")
                            {
                                sums[0, 5]++;
                                sums[1, 5] += nw;
                                sums[2, 5] += gw;
                                sums[3, 5] += (nw * labourAmt) + wholeLabourAmt;
                                sums[4, 5] += otherAmt;
                                sums[5, 5] += price;
                            }
                        }
                    }
                    label9.Text = sums[0, 0].ToString();
                    label13.Text = sums[0, 1].ToString();
                    label17.Text = sums[0, 2].ToString();
                    label21.Text = sums[0, 3].ToString();
                    label29.Text = sums[0, 4].ToString();
                    label33.Text = sums[0, 5].ToString();
                    label10.Text = sums[1, 0].ToString();
                    label14.Text = sums[1, 1].ToString();
                    label18.Text = sums[1, 2].ToString();
                    label22.Text = sums[1, 3].ToString();
                    label30.Text = sums[1, 4].ToString();
                    label34.Text = sums[1, 5].ToString();
                    label11.Text = sums[2, 0].ToString();
                    label15.Text = sums[2, 1].ToString();
                    label19.Text = sums[2, 2].ToString();
                    label23.Text = sums[2, 3].ToString();
                    label31.Text = sums[2, 4].ToString();
                    label35.Text = sums[2, 5].ToString();
                    label45.Text = sums[3, 0].ToString();
                    label48.Text = sums[3, 1].ToString();
                    label51.Text = sums[3, 2].ToString();
                    label54.Text = sums[3, 3].ToString();
                    label26.Text = sums[3, 4].ToString();
                    label37.Text = sums[3, 5].ToString();
                    label44.Text = sums[4, 0].ToString();
                    label47.Text = sums[4, 1].ToString();
                    label50.Text = sums[4, 2].ToString();
                    label53.Text = sums[4, 3].ToString();
                    label25.Text = sums[4, 4].ToString();
                    label36.Text = sums[4, 5].ToString();
                    label43.Text = sums[5, 0].ToString();
                    label46.Text = sums[5, 1].ToString();
                    label49.Text = sums[5, 2].ToString();
                    label52.Text = sums[5, 3].ToString();
                    label24.Text = sums[5, 4].ToString();
                    label27.Text = sums[5, 5].ToString();
                    panel8.Visible = true;
                    panel10.Visible = true;
                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately, e.g., show a message box
                    MessageBox.Show($"Error1: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void colorss()
        {
            label9.BackColor = Color.FromArgb(113, 131, 85);
            label13.BackColor = Color.FromArgb(113, 131, 85);
            label17.BackColor = Color.FromArgb(113, 131, 85);
            label21.BackColor = Color.FromArgb(113, 131, 85);
            label10.BackColor = Color.FromArgb(135, 152, 106);
            label14.BackColor = Color.FromArgb(135, 152, 106);
            label18.BackColor = Color.FromArgb(135, 152, 106);
            label22.BackColor = Color.FromArgb(135, 152, 106);
            label11.BackColor = Color.FromArgb(151, 169, 124);
            label15.BackColor = Color.FromArgb(151, 169, 124);
            label19.BackColor = Color.FromArgb(151, 169, 124);
            label23.BackColor = Color.FromArgb(151, 169, 124);
            label45.BackColor = Color.FromArgb(181, 201, 154);
            label48.BackColor = Color.FromArgb(181, 201, 154);
            label51.BackColor = Color.FromArgb(181, 201, 154);
            label54.BackColor = Color.FromArgb(181, 201, 154);
            label44.BackColor = Color.FromArgb(207, 225, 185);
            label47.BackColor = Color.FromArgb(207, 225, 185);
            label50.BackColor = Color.FromArgb(207, 225, 185);
            label53.BackColor = Color.FromArgb(207, 225, 185);
            label43.BackColor = Color.FromArgb(233, 245, 219);
            label46.BackColor = Color.FromArgb(233, 245, 219);
            label49.BackColor = Color.FromArgb(233, 245, 219);
            label52.BackColor = Color.FromArgb(233, 245, 219);
            // --------------
            label29.BackColor = Color.FromArgb(163, 163, 152);    // Color 1
            label33.BackColor = Color.FromArgb(163, 163, 152);   // Color 1
            // Second group of four
            label30.BackColor = Color.FromArgb(181, 181, 169);   // Color 2
            label34.BackColor = Color.FromArgb(181, 181, 169);   // Color 2
            // Third group of four
            label31.BackColor = Color.FromArgb(201, 201, 188);   // Color 3
            label35.BackColor = Color.FromArgb(201, 201, 188);   // Color 3
            // Fourth group of four
            label26.BackColor = Color.FromArgb(226, 226, 212);   // Color 4
            label37.BackColor = Color.FromArgb(226, 226, 212);   // Color 4
            // Fifth group of four
            label25.BackColor = Color.FromArgb(230, 230, 220);   // Color 5
            label36.BackColor = Color.FromArgb(230, 230, 220);   // Color 5
            // Sixth group of four
            label24.BackColor = Color.FromArgb(240, 240, 230);    // Color 6
            label27.BackColor = Color.FromArgb(240, 240, 230);    // Color 6
            label92.BackColor = Color.FromArgb(113, 131, 85);
            label85.BackColor = Color.FromArgb(113, 131, 85);
            label78.BackColor = Color.FromArgb(113, 131, 85);
            label71.BackColor = Color.FromArgb(113, 131, 85);
            label91.BackColor = Color.FromArgb(135, 152, 106);
            label84.BackColor = Color.FromArgb(135, 152, 106);
            label77.BackColor = Color.FromArgb(135, 152, 106);
            label70.BackColor = Color.FromArgb(135, 152, 106);
            label90.BackColor = Color.FromArgb(151, 169, 124);
            label83.BackColor = Color.FromArgb(151, 169, 124);
            label76.BackColor = Color.FromArgb(151, 169, 124);
            label69.BackColor = Color.FromArgb(151, 169, 124);
            label89.BackColor = Color.FromArgb(181, 201, 154);
            label82.BackColor = Color.FromArgb(181, 201, 154);
            label75.BackColor = Color.FromArgb(181, 201, 154);
            label68.BackColor = Color.FromArgb(181, 201, 154);
            label88.BackColor = Color.FromArgb(207, 225, 185);
            label81.BackColor = Color.FromArgb(207, 225, 185);
            label74.BackColor = Color.FromArgb(207, 225, 185);
            label67.BackColor = Color.FromArgb(207, 225, 185);
            label87.BackColor = Color.FromArgb(233, 245, 219);
            label80.BackColor = Color.FromArgb(233, 245, 219);
            label73.BackColor = Color.FromArgb(233, 245, 219);
            label66.BackColor = Color.FromArgb(233, 245, 219);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LOAD("huid");
                panel57.Visible = true;
            }
        }
        

    }
}
