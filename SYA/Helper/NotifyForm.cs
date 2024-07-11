namespace SYA
{
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
        }
        public void ShowNotification1(string message)
        {
            textBox1.Text = message;
        }
        public void ShowNotification2(string message)
        {
            textBox2.Text = message;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void NotifyForm_Load(object sender, EventArgs e)
        {
        }
    }
}
