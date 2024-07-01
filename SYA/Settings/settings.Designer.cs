namespace SYA
{
    partial class settings
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
            panel3 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            panel6 = new Panel();
            button1 = new Button();
            panel8 = new Panel();
            panel7 = new Panel();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(1401, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(15, 797);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 782);
            panel2.Name = "panel2";
            panel2.Size = new Size(1401, 15);
            panel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 15);
            panel3.Name = "panel3";
            panel3.Size = new Size(15, 767);
            panel3.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(1401, 15);
            panel4.TabIndex = 1;
            // 
            // panel5
            // 
            panel5.AutoScroll = true;
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(1386, 681);
            panel5.TabIndex = 2;
            // 
            // panel6
            // 
            panel6.Controls.Add(button1);
            panel6.Controls.Add(panel8);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(15, 15);
            panel6.Name = "panel6";
            panel6.Size = new Size(1386, 86);
            panel6.TabIndex = 3;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Left;
            button1.Font = new Font("Arial Rounded MT Bold", 18F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(257, 71);
            button1.TabIndex = 3;
            button1.Text = "Save Data";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel8
            // 
            panel8.Dock = DockStyle.Bottom;
            panel8.Location = new Point(0, 71);
            panel8.Name = "panel8";
            panel8.Size = new Size(1386, 15);
            panel8.TabIndex = 2;
            // 
            // panel7
            // 
            panel7.Controls.Add(panel5);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(15, 101);
            panel7.Name = "panel7";
            panel7.Size = new Size(1386, 681);
            panel7.TabIndex = 4;
            // 
            // settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1416, 797);
            Controls.Add(panel7);
            Controls.Add(panel6);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel4);
            Controls.Add(panel1);
            Name = "settings";
            Text = "settings";
            WindowState = FormWindowState.Maximized;
            Load += settings_Load;
            panel6.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Button button1;
    }
}