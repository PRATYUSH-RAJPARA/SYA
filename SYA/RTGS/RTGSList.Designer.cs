namespace SYA
{
    partial class RTGSList
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
            textBoxFilter = new TextBox();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // textBoxFilter
            // 
            textBoxFilter.Dock = DockStyle.Bottom;
            textBoxFilter.Location = new Point(0, 238);
            textBoxFilter.Name = "textBoxFilter";
            textBoxFilter.Size = new Size(584, 23);
            textBoxFilter.TabIndex = 2;
            textBoxFilter.TextChanged += textBoxFilter_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(584, 238);
            dataGridView1.TabIndex = 1;
            dataGridView1.KeyDown += dataGridView1_KeyDown;
            dataGridView1.KeyPress += dataGridView1_KeyPress;
            // 
            // RTGSList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(584, 261);
            Controls.Add(dataGridView1);
            Controls.Add(textBoxFilter);
            Name = "RTGSList";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RTGSList";
            TopMost = true;
            Load += RTGSList_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private TextBox textBoxFilter;
        private DataGridView dataGridView1;
    }
}