namespace SYA
{
    partial class SaleItem
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
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            dataGridView3 = new DataGridView();
            label2 = new Label();
            panel9 = new Panel();
            dataGridView1 = new DataGridView();
            panel7 = new Panel();
            panel6 = new Panel();
            panel10 = new Panel();
            btnSell = new Button();
            panel8 = new Panel();
            textBox1 = new TextBox();
            label1 = new Label();
            panel5 = new Panel();
            panel4 = new Panel();
            panel3 = new Panel();
            panel2 = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridView3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(panel9);
            panel1.Controls.Add(panel7);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1604, 818);
            panel1.TabIndex = 0;
            // 
            // dataGridView3
            // 
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(857, 144);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.RowTemplate.Height = 29;
            dataGridView3.Size = new Size(695, 658);
            dataGridView3.TabIndex = 9;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 19F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(857, 42);
            label2.Name = "label2";
            label2.Size = new Size(695, 79);
            label2.TabIndex = 8;
            label2.Text = "label2";
            // 
            // panel9
            // 
            panel9.Controls.Add(dataGridView1);
            panel9.Dock = DockStyle.Left;
            panel9.Location = new Point(395, 10);
            panel9.Name = "panel9";
            panel9.Size = new Size(400, 798);
            panel9.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(400, 798);
            dataGridView1.TabIndex = 4;
            // 
            // panel7
            // 
            panel7.Dock = DockStyle.Left;
            panel7.Location = new Point(367, 10);
            panel7.Name = "panel7";
            panel7.Size = new Size(28, 798);
            panel7.TabIndex = 7;
            // 
            // panel6
            // 
            panel6.Controls.Add(panel10);
            panel6.Controls.Add(btnSell);
            panel6.Controls.Add(panel8);
            panel6.Controls.Add(textBox1);
            panel6.Controls.Add(label1);
            panel6.Dock = DockStyle.Left;
            panel6.Location = new Point(10, 10);
            panel6.Name = "panel6";
            panel6.Size = new Size(357, 798);
            panel6.TabIndex = 6;
            // 
            // panel10
            // 
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(0, 234);
            panel10.Name = "panel10";
            panel10.Size = new Size(357, 34);
            panel10.TabIndex = 5;
            // 
            // btnSell
            // 
            btnSell.Dock = DockStyle.Top;
            btnSell.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            btnSell.Location = new Point(0, 184);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(357, 50);
            btnSell.TabIndex = 3;
            btnSell.Text = "SELL";
            btnSell.UseVisualStyleBackColor = true;
            btnSell.Click += btnSell_Click;
            // 
            // panel8
            // 
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 150);
            panel8.Name = "panel8";
            panel8.Size = new Size(357, 34);
            panel8.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Top;
            textBox1.Font = new Font("Segoe UI", 19F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(0, 100);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(357, 50);
            textBox1.TabIndex = 1;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(357, 100);
            label1.TabIndex = 0;
            label1.Text = "SELL ITEM";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(10, 808);
            panel5.Name = "panel5";
            panel5.Size = new Size(1584, 10);
            panel5.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(10, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1584, 10);
            panel4.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Right;
            panel3.Location = new Point(1594, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(10, 818);
            panel3.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(10, 818);
            panel2.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // SaleItem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1604, 818);
            Controls.Add(panel1);
            Name = "SaleItem";
            Text = "SaleItem";
            WindowState = FormWindowState.Maximized;
            Load += SaleItem_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel5;
        private Panel panel4;
        private Panel panel3;
        private Panel panel2;
        private DataGridView dataGridView1;
        private Panel panel6;
        private Panel panel8;
        private Label label1;
        private TextBox textBox1;
        private Panel panel9;
        private Panel panel7;
        private Button btnSell;
        private Panel panel10;
        private Label label2;
        private System.Windows.Forms.Timer timer1;
        private DataGridView dataGridView3;
    }
}