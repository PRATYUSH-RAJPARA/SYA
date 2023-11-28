namespace SYA
{
    partial class main
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
            btnAddGold = new Button();
            panelChild = new Panel();
            SuspendLayout();
            // 
            // btnAddGold
            // 
            btnAddGold.Location = new Point(19, 30);
            btnAddGold.Name = "btnAddGold";
            btnAddGold.Size = new Size(94, 29);
            btnAddGold.TabIndex = 0;
            btnAddGold.Text = "Gold";
            btnAddGold.UseVisualStyleBackColor = true;
            btnAddGold.Click += btnAddGold_Click;
            // 
            // panelChild
            // 
            panelChild.Location = new Point(209, 38);
            panelChild.Name = "panelChild";
            panelChild.Size = new Size(562, 380);
            panelChild.TabIndex = 1;
            // 
            // main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panelChild);
            Controls.Add(btnAddGold);
            Name = "main";
            Text = "main";
            Load += main_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnAddGold;
        private Panel panelChild;
    }
}