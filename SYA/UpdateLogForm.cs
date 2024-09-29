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
    public partial class UpdateLogForm : Form
    {
        private RichTextBox logTextBox;
        private Button closeButton;
        public UpdateLogForm()
        {
            logTextBox = new RichTextBox();
            logTextBox.Dock = DockStyle.Top;
            logTextBox.Height = 300;
            logTextBox.ReadOnly = true;
            closeButton = new Button();
            closeButton.Text = "OK";
            closeButton.Dock = DockStyle.Bottom;
            closeButton.Click += (sender, e) => this.Close();
            this.Controls.Add(logTextBox);
            this.Controls.Add(closeButton);
            this.Text = "Update Log";
            this.Size = new Size(400, 400);
        }
        public void AddLog(string text)
        {
            logTextBox.AppendText(text + Environment.NewLine);
        }
        private void UpdateLogForm_Load(object sender, EventArgs e)
        {
        }
    }
}
