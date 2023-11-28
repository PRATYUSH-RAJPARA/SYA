using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void btnAddGold_Click(object sender, EventArgs e)
        {
            // Load and show Form1 in the panel
            LoadForm(new addgold());
        }
        private void LoadForm(Form form)
        {
            // Close the currently displayed form (if any)
            if (panelChild.Controls.Count > 0)
            {
                Form currentForm = panelChild.Controls[0] as Form;
                if (currentForm != null)
                {
                    currentForm.Close();
                    currentForm.Dispose();
                }
            }

            // Set the properties of the new form
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Add the new form to the panel
            panelChild.Controls.Add(form);

            // Show the new form
            form.Show();
        }
    }
}
