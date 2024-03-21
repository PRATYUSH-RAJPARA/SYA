namespace SYA
{
    partial class saleReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            panel2 = new Panel();
            richTextBox1 = new RichTextBox();
            dataGridView1 = new DataGridView();
            panel5 = new Panel();
            panel4 = new Panel();
            printbtn = new Button();
            panel7 = new Panel();
            btnShowData = new Button();
            panel6 = new Panel();
            endDatePicker = new DateTimePicker();
            label3 = new Label();
            startDatePicker = new DateTimePicker();
            label2 = new Label();
            panel3 = new Panel();
            button1 = new Button();
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(26, 22, 26, 22);
            panel1.Size = new Size(983, 484);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(richTextBox1);
            panel2.Controls.Add(dataGridView1);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(26, 22);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(931, 440);
            panel2.TabIndex = 0;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(643, 74);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(201, 323);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(0, 69);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(931, 333);
            dataGridView1.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(0, 402);
            panel5.Margin = new Padding(3, 2, 3, 2);
            panel5.Name = "panel5";
            panel5.Size = new Size(931, 38);
            panel5.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(printbtn);
            panel4.Controls.Add(panel7);
            panel4.Controls.Add(btnShowData);
            panel4.Controls.Add(panel6);
            panel4.Controls.Add(endDatePicker);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(startDatePicker);
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 38);
            panel4.Margin = new Padding(3, 2, 3, 2);
            panel4.Name = "panel4";
            panel4.Size = new Size(931, 31);
            panel4.TabIndex = 1;
            // 
            // printbtn
            // 
            printbtn.Dock = DockStyle.Left;
            printbtn.FlatStyle = FlatStyle.Popup;
            printbtn.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            printbtn.Location = new Point(627, 0);
            printbtn.Margin = new Padding(3, 2, 3, 2);
            printbtn.Name = "printbtn";
            printbtn.Size = new Size(144, 31);
            printbtn.TabIndex = 7;
            printbtn.Text = "Print Data";
            printbtn.UseVisualStyleBackColor = true;
            printbtn.Click += printbtn_Click;
            // 
            // panel7
            // 
            panel7.Dock = DockStyle.Left;
            panel7.Location = new Point(609, 0);
            panel7.Margin = new Padding(3, 2, 3, 2);
            panel7.Name = "panel7";
            panel7.Size = new Size(18, 31);
            panel7.TabIndex = 6;
            // 
            // btnShowData
            // 
            btnShowData.Dock = DockStyle.Left;
            btnShowData.FlatStyle = FlatStyle.Popup;
            btnShowData.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            btnShowData.Location = new Point(465, 0);
            btnShowData.Margin = new Padding(3, 2, 3, 2);
            btnShowData.Name = "btnShowData";
            btnShowData.Size = new Size(144, 31);
            btnShowData.TabIndex = 5;
            btnShowData.Text = "Show Data";
            btnShowData.UseVisualStyleBackColor = true;
            btnShowData.Click += btnShowData_Click;
            // 
            // panel6
            // 
            panel6.Dock = DockStyle.Left;
            panel6.Location = new Point(447, 0);
            panel6.Margin = new Padding(3, 2, 3, 2);
            panel6.Name = "panel6";
            panel6.Size = new Size(18, 31);
            panel6.TabIndex = 4;
            // 
            // endDatePicker
            // 
            endDatePicker.Dock = DockStyle.Left;
            endDatePicker.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            endDatePicker.Format = DateTimePickerFormat.Short;
            endDatePicker.Location = new Point(295, 0);
            endDatePicker.Margin = new Padding(3, 2, 3, 2);
            endDatePicker.Name = "endDatePicker";
            endDatePicker.Size = new Size(152, 34);
            endDatePicker.TabIndex = 3;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Left;
            label3.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(229, 0);
            label3.Name = "label3";
            label3.Size = new Size(66, 31);
            label3.TabIndex = 2;
            label3.Text = "  To :";
            // 
            // startDatePicker
            // 
            startDatePicker.Dock = DockStyle.Left;
            startDatePicker.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            startDatePicker.Format = DateTimePickerFormat.Short;
            startDatePicker.Location = new Point(77, 0);
            startDatePicker.Margin = new Padding(3, 2, 3, 2);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(152, 34);
            startDatePicker.TabIndex = 1;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Left;
            label2.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(77, 31);
            label2.TabIndex = 0;
            label2.Text = "From :";
            // 
            // panel3
            // 
            panel3.Controls.Add(button1);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Margin = new Padding(3, 2, 3, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(931, 38);
            panel3.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(635, 10);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 17.5F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(931, 38);
            label1.TabIndex = 0;
            label1.Text = "SALE REPORT";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // saleReport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(983, 484);
            Controls.Add(panel1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "saleReport";
            WindowState = FormWindowState.Maximized;
            Load += saleReport_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private DataGridView dataGridView1;
        private Panel panel5;
        private Panel panel4;
        private Panel panel3;
        private Label label1;
        private DateTimePicker endDatePicker;
        private Label label3;
        private DateTimePicker startDatePicker;
        private Label label2;
        private Button btnShowData;
        private Panel panel6;
        private Button printbtn;
        private Panel panel7;
        private RichTextBox richTextBox1;
        private Button button1;
    }
}