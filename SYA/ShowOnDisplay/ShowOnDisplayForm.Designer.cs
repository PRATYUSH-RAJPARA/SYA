namespace SYA
{
    partial class ShowOnDisplayForm
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
            label25 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // label25
            // 
            label25.Font = new Font("Arial", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label25.Location = new Point(0, 42);
            label25.Name = "label25";
            label25.Size = new Size(783, 60);
            label25.TabIndex = 17;
            label25.Text = "MESSAGE";
            label25.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Arial Rounded MT Bold", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(0, 0);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(800, 39);
            textBox1.TabIndex = 18;
            textBox1.KeyPress += textBox1_KeyPress_1;
            // 
            // ShowOnDisplayForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label25);
            Controls.Add(textBox1);
            Name = "ShowOnDisplayForm";
            Text = "ShowOnDisplayForm";
            WindowState = FormWindowState.Maximized;
            Load += ShowOnDisplayForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label25;
        private TextBox textBox1;
    }
}