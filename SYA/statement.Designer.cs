namespace SYA
{
    partial class statement
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
            richTextBox1 = new RichTextBox();
            ProcessDataButton = new Button();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(192, 255, 192);
            richTextBox1.Location = new Point(25, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(889, 172);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // ProcessDataButton
            // 
            ProcessDataButton.Location = new Point(971, 54);
            ProcessDataButton.Name = "ProcessDataButton";
            ProcessDataButton.Size = new Size(94, 29);
            ProcessDataButton.TabIndex = 1;
            ProcessDataButton.Text = "button1";
            ProcessDataButton.UseVisualStyleBackColor = true;
            ProcessDataButton.Click += ProcessDataButton_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(53, 214);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(1000, 386);
            dataGridView1.TabIndex = 2;
            // 
            // statement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(1109, 626);
            Controls.Add(dataGridView1);
            Controls.Add(ProcessDataButton);
            Controls.Add(richTextBox1);
            Name = "statement";
            Text = "statement";
            WindowState = FormWindowState.Maximized;
            Load += statement_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button ProcessDataButton;
        private DataGridView dataGridView1;
    }
}