using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SYA.Helper;
namespace SYA
{
    public partial class Labour : Form
    {
        public Labour()
        {
            InitializeComponent();
            textBox16.Leave += new EventHandler(TextBox_Leave);
            textBox15.Leave += new EventHandler(TextBox_Leave);
            textBox14.Leave += new EventHandler(TextBox_Leave);
            textBox13.Leave += new EventHandler(TextBox_Leave);
            textBox12.Leave += new EventHandler(TextBox_Leave);
            textBox36.Leave += new EventHandler(TextBox_Leave);
            textBox21.Leave += new EventHandler(TextBox_Leave);
            textBox20.Leave += new EventHandler(TextBox_Leave);
            textBox19.Leave += new EventHandler(TextBox_Leave);
            textBox18.Leave += new EventHandler(TextBox_Leave);
            textBox17.Leave += new EventHandler(TextBox_Leave);
            textBox38.Leave += new EventHandler(TextBox_Leave);
           
        }
        private void Labour_Load(object sender, EventArgs e)
        {
            labourLoad("general");
            labourLoad("recommanded");
          //  decimal[] a = Verification.getLabourAndWholeLabour("1");
          //  MessageBox.Show(a[0].ToString()+" : " + a[1].ToString());   

        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && double.TryParse(textBox.Text, out double value))
            {
                double roundedValue = RoundToNearest50(value);
                textBox.Text = roundedValue.ToString();
            }
        }
      
        private void labourLoad(string s)
        {
            string query = "SELECT * FROM Labour";
            DataTable labour = helper.FetchDataTableFromSYASettingsDataBase(query);
            query = "SELECT * FROM Settings";
            DataTable settings = helper.FetchDataTableFromSYASettingsDataBase(query);
            DataRow settingsRow = settings.Rows[0];
            double[] slabMin = new double[6];
            double[] slabMax = new double[6];
            double[] oldLabourMin = new double[6];
            double[] oldLabourMax = new double[6];



            void getLabourData(DataTable dt)
            {
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    double SLAB_MIN = Convert.ToDouble(row["SLAB_MIN"]);
                    double SLAB_MAX = Convert.ToDouble(row["SLAB_MAX"]);
                    double PRICE_MIN = Convert.ToDouble(row["PRICE_MIN"]);
                    double PRICE_MAX = Convert.ToDouble(row["PRICE_MAX"]);
                    oldLabourMin[i] = PRICE_MIN;
                    oldLabourMax[i] = PRICE_MAX;
                    slabMin[i] = SLAB_MIN;
                    slabMax[i] = SLAB_MAX;
                    i++;
                }
            }
            void setSlabValues()
            {
                textBox2.Text = slabMin[0].ToString();
                textBox3.Text = slabMin[1].ToString();
                textBox4.Text = slabMin[2].ToString();
                textBox5.Text = slabMin[3].ToString();
                textBox6.Text = slabMin[4].ToString();
                textBox32.Text = slabMin[5].ToString();
                textBox11.Text = slabMax[0].ToString();
                textBox10.Text = slabMax[1].ToString();
                textBox9.Text = slabMax[2].ToString();
                textBox8.Text = slabMax[3].ToString();
                textBox7.Text = slabMax[4].ToString();
                textBox34.Text = slabMax[5].ToString();
            }
            void setOnDataLabourValues()
            {
                textBox16.Text = oldLabourMin[0].ToString();
                textBox15.Text = oldLabourMin[1].ToString();
                textBox14.Text = oldLabourMin[2].ToString();
                textBox13.Text = oldLabourMin[3].ToString();
                textBox12.Text = oldLabourMin[4].ToString();
                textBox36.Text = oldLabourMin[5].ToString();
                textBox37.Text = "0";
                textBox21.Text = oldLabourMax[0].ToString();
                textBox20.Text = oldLabourMax[1].ToString();
                textBox19.Text = oldLabourMax[2].ToString();
                textBox18.Text = oldLabourMax[3].ToString();
                textBox17.Text = oldLabourMax[4].ToString();
                textBox38.Text = oldLabourMax[5].ToString();
                textBox39.Text = "0";
            }
            void setOnDataWholeLabourValues()
            {
                textBox27.Text = "0";
                textBox28.Text = "0";
                textBox29.Text = "0";
                textBox30.Text = "0";
                textBox31.Text = "0";
                textBox40.Text = "0";
                textBox41.Text = settingsRow["GoldPerGramLabour"].ToString();
            }
            if (s == "general")
            {
                textBox1.Text = settingsRow["GoldPerGramLabour"].ToString();
                textBox44.Text = ("0.5").ToString();
                getLabourData(labour);
                setSlabValues();
                setOnDataLabourValues();
                setOnDataWholeLabourValues();

            }

            double newLabour = textBox1.Text == "" ? 0 : Convert.ToDouble(textBox1.Text);


            if (s == "recommanded")
            {
                query = "SELECT * FROM labourExample";
                DataTable labourExample = helper.FetchDataTableFromSYASettingsDataBase(query);
                double[] newLabourMin = new double[6];
                double[] newLabourMax = new double[6];
                double oldLabour = 0;
                getLabourRecommandedData();
                setRecommandedLabourValues();
                setRecommandedWholeLabourValues();
                void getLabourRecommandedData()
                {
                    getLabourData(labourExample);
                    oldLabour = Convert.ToDouble(labourExample.Rows[0]["LABOUR_PRICE"]);
                    for (int i = 0; i < 6; i++)
                    {
                        if (slabMin[i] == 0 || slabMin[i] == 0.5)
                        {
                            newLabourMin[i] = oldLabourMin[i] - oldLabour + newLabour;
                            newLabourMax[i] = oldLabourMax[i] - oldLabour + newLabour;
                            continue;
                        }
                        double ratio = (textBox44.Text == "" ? 0 : Convert.ToDouble(textBox44.Text));
                        newLabourMin[i] = oldLabourMin[i] + (((oldLabourMin[i] * newLabour / oldLabour) - oldLabourMin[i]) * ratio);
                        newLabourMax[i] = oldLabourMax[i] + ((oldLabourMax[i] * newLabour / oldLabour) - oldLabourMax[i]) * (textBox44.Text == "" ? 0 : Convert.ToDouble(textBox44.Text));
                    }
                }
                void setRecommandedLabourValues()
                {
                    textBox71.Text = RoundToNearest50(newLabourMin[0]).ToString();
                    textBox70.Text = RoundToNearest50(newLabourMin[1]).ToString();
                    textBox69.Text = RoundToNearest50(newLabourMin[2]).ToString();
                    textBox68.Text = RoundToNearest50(newLabourMin[3]).ToString();
                    textBox67.Text = RoundToNearest50(newLabourMin[4]).ToString();
                    textBox66.Text = RoundToNearest50(newLabourMin[5]).ToString();
                    textBox65.Text = "0";
                    textBox64.Text = RoundToNearest50(newLabourMax[0]).ToString();
                    textBox63.Text = RoundToNearest50(newLabourMax[1]).ToString();
                    textBox62.Text = RoundToNearest50(newLabourMax[2]).ToString();
                    textBox61.Text = RoundToNearest50(newLabourMax[3]).ToString();
                    textBox60.Text = RoundToNearest50(newLabourMax[4]).ToString();
                    textBox59.Text = RoundToNearest50(newLabourMax[5]).ToString();
                    textBox58.Text = "0";

                    UpdateIndividualTextBoxColor(textBox16, textBox71);
                    UpdateIndividualTextBoxColor(textBox15, textBox70);
                    UpdateIndividualTextBoxColor(textBox14, textBox69);
                    UpdateIndividualTextBoxColor(textBox13, textBox68);
                    UpdateIndividualTextBoxColor(textBox12, textBox67);
                    UpdateIndividualTextBoxColor(textBox36, textBox66);
                    UpdateIndividualTextBoxColor(textBox21, textBox64);
                    UpdateIndividualTextBoxColor(textBox20, textBox63);
                    UpdateIndividualTextBoxColor(textBox19, textBox62);
                    UpdateIndividualTextBoxColor(textBox18, textBox61);
                    UpdateIndividualTextBoxColor(textBox17, textBox60);
                    UpdateIndividualTextBoxColor(textBox38, textBox59);
                    textBox4.ForeColor=Color.Black;
                }
                void setRecommandedWholeLabourValues()
                {
                    textBox22.Text = "0";
                    textBox23.Text = "0";
                    textBox24.Text = "0";
                    textBox25.Text = "0";
                    textBox26.Text = "0";
                    textBox42.Text = "0";
                    textBox43.Text = RoundToNearest50(Convert.ToDouble(textBox1.Text)).ToString();
                    UpdateIndividualTextBoxColor(textBox41, textBox43);
                }
                double RoundToNearest50(double value)
                {
                    return Math.Round(value / 50) * 50;
                }

            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Enter)
            {
                //updateLabour();
            }
        }
        private void SaveLabourData()
        {
            double[] slabMin = new double[6];
            double[] slabMax = new double[6];
            double[] numbersMin = new double[6];
            double[] numbersMax = new double[6];
            numbersMin[0] = Convert.ToDouble(textBox16.Text);
            numbersMin[1] = Convert.ToDouble(textBox15.Text);
            numbersMin[2] = Convert.ToDouble(textBox14.Text);
            numbersMin[3] = Convert.ToDouble(textBox13.Text);
            numbersMin[4] = Convert.ToDouble(textBox12.Text);
            numbersMin[5] = Convert.ToDouble(textBox36.Text);
            numbersMax[0] = Convert.ToDouble(textBox21.Text);
            numbersMax[1] = Convert.ToDouble(textBox20.Text);
            numbersMax[2] = Convert.ToDouble(textBox19.Text);
            numbersMax[3] = Convert.ToDouble(textBox18.Text);
            numbersMax[4] = Convert.ToDouble(textBox17.Text);
            numbersMax[5] = Convert.ToDouble(textBox38.Text);
            slabMin[0] = Convert.ToDouble(textBox2.Text);
            slabMin[1] = Convert.ToDouble(textBox3.Text);
            slabMin[2] = Convert.ToDouble(textBox4.Text);
            slabMin[3] = Convert.ToDouble(textBox5.Text);
            slabMin[4] = Convert.ToDouble(textBox6.Text);
            slabMin[5] = Convert.ToDouble(textBox32.Text);
            slabMax[0] = Convert.ToDouble(textBox11.Text);
            slabMax[1] = Convert.ToDouble(textBox10.Text);
            slabMax[2] = Convert.ToDouble(textBox9.Text);
            slabMax[3] = Convert.ToDouble(textBox8.Text);
            slabMax[4] = Convert.ToDouble(textBox7.Text);
            slabMax[5] = Convert.ToDouble(textBox34.Text);
            // Update the database with the new values
            for (int i = 0; i < 6; i++)
            {
                string updateQuery = $"UPDATE Labour SET SLAB_MIN = {slabMin[i]}, SLAB_MAX = {slabMax[i]}, PRICE_MIN = {numbersMin[i]}, PRICE_MAX = {numbersMax[i]} WHERE ID = {i + 1}";
                helper.RunQueryWithoutParametersSyaSettingsDataBase(updateQuery, "ExecuteNonQuery");
            }
            string updateQuery1 = $"UPDATE Settings SET GoldPerGramLabour = {textBox1.Text} where ID = 1";
            helper.RunQueryWithoutParametersSyaSettingsDataBase(updateQuery1, "ExecuteNonQuery");
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
            labourLoad("recommanded");
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveLabourData();
        }
        private double RoundToNearest50(double value)
        {
            return Math.Round(value / 50.0) * 50;
        }

        private Color GetColorBasedOnDifference(double difference)
        {
            int maxIntensity = 255;
            int scaledIntensity = (int)(Math.Min(Math.Abs(difference), maxIntensity));

            if (difference < 0) // On-data value is lower than recommended
            {
                return Color.FromArgb(255, 255, maxIntensity - scaledIntensity, maxIntensity - scaledIntensity); // Shades of red
            }
            else if (difference > 0) // On-data value is higher than recommended
            {
                return Color.FromArgb(255, maxIntensity - scaledIntensity, 255, maxIntensity - scaledIntensity); // Shades of green
            }
            else // On-data value is equal to recommended
            {
                return Color.Black; // Default font color
            }
        }

        public  decimal[] getLabourAndWholeLabour(string wt)
        {
            decimal weight = decimal.Parse(wt);
            decimal[] l = new decimal[2];
            void parseLabour(int index)
            {

                if (weight >= 3)
                {
                    l[0] = CustomRound(decimal.Parse(helper.GoldPerGramLabour),50);
                    l[1] = 0;
                }
                else
                {
                    l[0] = 0;
                    l[1] = decimal.Parse(helper.tableLabour.Rows[index]["PRICE_MIN"].ToString())
                        + ((weight - decimal.Parse(helper.tableLabour.Rows[index]["SLAB_MIN"].ToString())) / (decimal.Parse(helper.tableLabour.Rows[index]["SLAB_MAX"].ToString()) - decimal.Parse(helper.tableLabour.Rows[index]["SLAB_MIN"].ToString()))) * (decimal.Parse(helper.tableLabour.Rows[index]["PRICE_MAX"].ToString()) - decimal.Parse(helper.tableLabour.Rows[index]["PRICE_MIN"].ToString()));
                    l[1]= CustomRound(l[1],50);
                }
            }

            if (weight is > 0 and < (decimal)0.5)
            {
                parseLabour(0);
            }
            else if (weight is >= (decimal)0.5 and < (decimal)1)
            {
                parseLabour(1);
            }
            else if (weight is >= (decimal)1 and < (decimal)1.5)
            {
                parseLabour(2);
            }
            else if (weight is >= (decimal)1.5 and < (decimal)2)
            {
                parseLabour(3);
            }
            else if (weight is >= (decimal)2 and < (decimal)2.5)
            {
                parseLabour(4);
            }
            else if (weight is >= (decimal)2.5 and < (decimal)3)
            {
                parseLabour(5);
            }
            else if (weight is >= 3)
            {
                parseLabour(5);
            }
            return l;

        }
        private decimal CustomRound(decimal value, decimal step)
        {
            decimal remainder = value % step;
            if (remainder == 0)
            {
                return value;
            }
            else
            {
                return value + (step - remainder);
            }
        }

        public void UpdateIndividualTextBoxColor(TextBox onDataTextBox, TextBox recommendedTextBox)
        {
            if (onDataTextBox != null && recommendedTextBox != null &&
                double.TryParse(onDataTextBox.Text, out double onDataValue) &&
                double.TryParse(recommendedTextBox.Text, out double recommendedValue))
            {
                double difference = onDataValue - recommendedValue;

                // Choose the color based on the difference
                onDataTextBox.ForeColor = GetColorBasedOnDifference(difference);
            }
        }



    }

}
