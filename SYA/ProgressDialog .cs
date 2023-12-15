using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SYA
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }
        public void ProgressDialog_Load(object sender, EventArgs e)
        {
        }
        public void SetProgress(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() => SetProgress(value)));
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        public void SetMessage(string message)
        {
            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke(new Action(() => SetMessage(message)));
            }
            else
            {
                lblMessage.Text = message;
            }
        }
    }
}
