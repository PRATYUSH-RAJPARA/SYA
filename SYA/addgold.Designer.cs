using System.Runtime.InteropServices;

namespace SYA
{
    partial class addgold
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle16 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            panel1 = new Panel();
            progressBar1 = new ProgressBar();
            textBox1 = new TextBox();
            panel28 = new Panel();
            panel27 = new Panel();
            panel22 = new Panel();
            panel19 = new Panel();
            checkBoxAddGold1 = new CheckBox();
            panel4 = new Panel();
            dataGridView1 = new DataGridView();
            select = new DataGridViewCheckBoxColumn();
            tagno = new DataGridViewTextBoxColumn();
            type = new DataGridViewComboBoxColumn();
            caret = new DataGridViewComboBoxColumn();
            gross = new DataGridViewTextBoxColumn();
            net = new DataGridViewTextBoxColumn();
            labour = new DataGridViewTextBoxColumn();
            wholeLabour = new DataGridViewTextBoxColumn();
            other = new DataGridViewTextBoxColumn();
            huid1 = new DataGridViewTextBoxColumn();
            huid2 = new DataGridViewTextBoxColumn();
            size = new DataGridViewTextBoxColumn();
            comment = new DataGridViewTextBoxColumn();
            panel31 = new Panel();
            panel24 = new Panel();
            panel20 = new Panel();
            panel5 = new Panel();
            panel34 = new Panel();
            panel16 = new Panel();
            btnSelectAll = new Button();
            panel35 = new Panel();
            itemcountandgrossweight = new TextBox();
            panel32 = new Panel();
            panel23 = new Panel();
            panel21 = new Panel();
            panelBackground = new Panel();
            panel6 = new Panel();
            panel37 = new Panel();
            btnFetch = new Button();
            panel36 = new Panel();
            panel33 = new Panel();
            btnAddGoldSave = new Button();
            panel15 = new Panel();
            btnAddGoldPrintTag = new Button();
            panel13 = new Panel();
            panel12 = new Panel();
            panel14 = new Panel();
            panel11 = new Panel();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel5.SuspendLayout();
            panel34.SuspendLayout();
            panelBackground.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(255, 192, 255);
            panel1.Controls.Add(progressBar1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(panel28);
            panel1.Controls.Add(panel27);
            panel1.Controls.Add(panel22);
            panel1.Controls.Add(panel19);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1604, 65);
            panel1.TabIndex = 0;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(1061, 12);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(319, 42);
            progressBar1.TabIndex = 13;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(233, 245, 219);
            textBox1.Dock = DockStyle.Bottom;
            textBox1.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(10, 8);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(1584, 52);
            textBox1.TabIndex = 12;
            textBox1.TabStop = false;
            textBox1.Text = "ADD GOLD ITEMS";
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // panel28
            // 
            panel28.BackColor = Color.FromArgb(65, 72, 51);
            panel28.Dock = DockStyle.Left;
            panel28.Location = new Point(0, 5);
            panel28.Name = "panel28";
            panel28.Size = new Size(10, 55);
            panel28.TabIndex = 11;
            panel28.Paint += panel28_Paint;
            // 
            // panel27
            // 
            panel27.BackColor = Color.Black;
            panel27.Dock = DockStyle.Right;
            panel27.Location = new Point(1594, 5);
            panel27.Name = "panel27";
            panel27.Size = new Size(10, 55);
            panel27.TabIndex = 10;
            // 
            // panel22
            // 
            panel22.BackColor = SystemColors.ActiveCaptionText;
            panel22.Dock = DockStyle.Top;
            panel22.Location = new Point(0, 0);
            panel22.Name = "panel22";
            panel22.Size = new Size(1604, 5);
            panel22.TabIndex = 4;
            // 
            // panel19
            // 
            panel19.BackColor = SystemColors.ActiveCaptionText;
            panel19.Dock = DockStyle.Bottom;
            panel19.Location = new Point(0, 60);
            panel19.Name = "panel19";
            panel19.Size = new Size(1604, 5);
            panel19.TabIndex = 3;
            // 
            // checkBoxAddGold1
            // 
            checkBoxAddGold1.AutoSize = true;
            checkBoxAddGold1.BackColor = Color.FromArgb(96, 111, 73);
            checkBoxAddGold1.Dock = DockStyle.Right;
            checkBoxAddGold1.FlatAppearance.BorderColor = Color.FromArgb(3, 102, 102);
            checkBoxAddGold1.FlatAppearance.BorderSize = 5;
            checkBoxAddGold1.FlatStyle = FlatStyle.Popup;
            checkBoxAddGold1.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            checkBoxAddGold1.ForeColor = Color.White;
            checkBoxAddGold1.Location = new Point(1274, 0);
            checkBoxAddGold1.Name = "checkBoxAddGold1";
            checkBoxAddGold1.Size = new Size(310, 60);
            checkBoxAddGold1.TabIndex = 13;
            checkBoxAddGold1.Text = "Quick Save && Print          ";
            checkBoxAddGold1.UseVisualStyleBackColor = false;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(255, 192, 128);
            panel4.Controls.Add(dataGridView1);
            panel4.Controls.Add(panel31);
            panel4.Controls.Add(panel24);
            panel4.Controls.Add(panel20);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 65);
            panel4.Name = "panel4";
            panel4.Size = new Size(1604, 753);
            panel4.TabIndex = 3;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.FromArgb(233, 245, 219);
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { select, tagno, type, caret, gross, net, labour, wholeLabour, other, huid1, huid2, size, comment });
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = SystemColors.Window;
            dataGridViewCellStyle15.Font = new Font("Segoe UI", 12.5F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle15.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = Color.FromArgb(227, 180, 124);
            dataGridViewCellStyle15.SelectionForeColor = Color.Black;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle15;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(10, 0);
            dataGridView1.Margin = new Padding(0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle16.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = SystemColors.Control;
            dataGridViewCellStyle16.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle16.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(1584, 748);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CellEnter += dataGridView1_CellEnter;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.KeyDown += dataGridView1_KeyDown;
            // 
            // select
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(151, 169, 124);
            dataGridViewCellStyle2.NullValue = false;
            select.DefaultCellStyle = dataGridViewCellStyle2;
            select.HeaderText = "";
            select.MinimumWidth = 6;
            select.Name = "select";
            select.Width = 125;
            // 
            // tagno
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(166, 185, 139);
            tagno.DefaultCellStyle = dataGridViewCellStyle3;
            tagno.HeaderText = "TAG NO";
            tagno.MinimumWidth = 6;
            tagno.Name = "tagno";
            tagno.ReadOnly = true;
            tagno.Width = 125;
            // 
            // type
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(181, 201, 154);
            type.DefaultCellStyle = dataGridViewCellStyle4;
            type.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            type.HeaderText = "ITEM";
            type.MinimumWidth = 6;
            type.Name = "type";
            type.Width = 125;
            // 
            // caret
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(194, 213, 170);
            caret.DefaultCellStyle = dataGridViewCellStyle5;
            caret.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            caret.HeaderText = "CARET";
            caret.MinimumWidth = 6;
            caret.Name = "caret";
            caret.Width = 125;
            // 
            // gross
            // 
            dataGridViewCellStyle6.BackColor = Color.FromArgb(207, 225, 185);
            gross.DefaultCellStyle = dataGridViewCellStyle6;
            gross.HeaderText = "GROSS WEIGHT";
            gross.MinimumWidth = 6;
            gross.Name = "gross";
            gross.Width = 125;
            // 
            // net
            // 
            dataGridViewCellStyle7.BackColor = Color.FromArgb(220, 235, 202);
            net.DefaultCellStyle = dataGridViewCellStyle7;
            net.HeaderText = "NET WEIGHT";
            net.MinimumWidth = 6;
            net.Name = "net";
            net.Width = 125;
            // 
            // labour
            // 
            dataGridViewCellStyle8.BackColor = Color.FromArgb(233, 245, 219);
            labour.DefaultCellStyle = dataGridViewCellStyle8;
            labour.HeaderText = "PER GRAM LABOUR";
            labour.MinimumWidth = 6;
            labour.Name = "labour";
            labour.Width = 125;
            // 
            // wholeLabour
            // 
            dataGridViewCellStyle9.BackColor = Color.FromArgb(220, 235, 202);
            wholeLabour.DefaultCellStyle = dataGridViewCellStyle9;
            wholeLabour.HeaderText = "WHOLE LABOUR";
            wholeLabour.MinimumWidth = 6;
            wholeLabour.Name = "wholeLabour";
            wholeLabour.Width = 125;
            // 
            // other
            // 
            dataGridViewCellStyle10.BackColor = Color.FromArgb(207, 225, 185);
            other.DefaultCellStyle = dataGridViewCellStyle10;
            other.HeaderText = "OTHER";
            other.MinimumWidth = 6;
            other.Name = "other";
            other.Width = 125;
            // 
            // huid1
            // 
            dataGridViewCellStyle11.BackColor = Color.FromArgb(194, 213, 170);
            huid1.DefaultCellStyle = dataGridViewCellStyle11;
            huid1.HeaderText = "HUID1";
            huid1.MaxInputLength = 6;
            huid1.MinimumWidth = 6;
            huid1.Name = "huid1";
            huid1.Width = 125;
            // 
            // huid2
            // 
            dataGridViewCellStyle12.BackColor = Color.FromArgb(181, 201, 154);
            huid2.DefaultCellStyle = dataGridViewCellStyle12;
            huid2.HeaderText = "HUID2";
            huid2.MaxInputLength = 6;
            huid2.MinimumWidth = 6;
            huid2.Name = "huid2";
            huid2.Width = 125;
            // 
            // size
            // 
            dataGridViewCellStyle13.BackColor = Color.FromArgb(166, 185, 139);
            size.DefaultCellStyle = dataGridViewCellStyle13;
            size.HeaderText = "SIZE";
            size.MinimumWidth = 6;
            size.Name = "size";
            size.Width = 125;
            // 
            // comment
            // 
            comment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle14.BackColor = Color.FromArgb(151, 169, 124);
            comment.DefaultCellStyle = dataGridViewCellStyle14;
            comment.HeaderText = "COMMENT";
            comment.MinimumWidth = 6;
            comment.Name = "comment";
            // 
            // panel31
            // 
            panel31.BackColor = Color.FromArgb(65, 72, 51);
            panel31.Dock = DockStyle.Left;
            panel31.Location = new Point(0, 0);
            panel31.Name = "panel31";
            panel31.Size = new Size(10, 748);
            panel31.TabIndex = 12;
            panel31.Paint += panel31_Paint;
            // 
            // panel24
            // 
            panel24.BackColor = Color.FromArgb(65, 72, 51);
            panel24.Dock = DockStyle.Right;
            panel24.Location = new Point(1594, 0);
            panel24.Name = "panel24";
            panel24.Size = new Size(10, 748);
            panel24.TabIndex = 10;
            // 
            // panel20
            // 
            panel20.BackColor = Color.FromArgb(65, 72, 51);
            panel20.Dock = DockStyle.Bottom;
            panel20.Location = new Point(0, 748);
            panel20.Name = "panel20";
            panel20.Size = new Size(1604, 5);
            panel20.TabIndex = 3;
            panel20.Paint += panel20_Paint;
            // 
            // panel5
            // 
            panel5.BackColor = SystemColors.AppWorkspace;
            panel5.Controls.Add(panel34);
            panel5.Controls.Add(panel32);
            panel5.Controls.Add(panel23);
            panel5.Controls.Add(panel21);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(0, 753);
            panel5.Name = "panel5";
            panel5.Size = new Size(1604, 65);
            panel5.TabIndex = 4;
            // 
            // panel34
            // 
            panel34.BackColor = Color.FromArgb(233, 245, 219);
            panel34.Controls.Add(panel16);
            panel34.Controls.Add(btnSelectAll);
            panel34.Controls.Add(panel35);
            panel34.Controls.Add(checkBoxAddGold1);
            panel34.Controls.Add(itemcountandgrossweight);
            panel34.Dock = DockStyle.Fill;
            panel34.Location = new Point(10, 0);
            panel34.Name = "panel34";
            panel34.Size = new Size(1584, 60);
            panel34.TabIndex = 13;
            // 
            // panel16
            // 
            panel16.BackColor = Color.FromArgb(96, 111, 73);
            panel16.Dock = DockStyle.Right;
            panel16.ForeColor = Color.White;
            panel16.Location = new Point(1198, 0);
            panel16.Name = "panel16";
            panel16.Size = new Size(76, 60);
            panel16.TabIndex = 20;
            // 
            // btnSelectAll
            // 
            btnSelectAll.BackColor = Color.FromArgb(96, 111, 73);
            btnSelectAll.Dock = DockStyle.Left;
            btnSelectAll.FlatAppearance.BorderColor = Color.FromArgb(3, 102, 102);
            btnSelectAll.FlatAppearance.MouseDownBackColor = Color.FromArgb(153, 226, 180);
            btnSelectAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(3, 102, 102);
            btnSelectAll.FlatStyle = FlatStyle.Popup;
            btnSelectAll.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            btnSelectAll.ForeColor = Color.White;
            btnSelectAll.Location = new Point(36, 0);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(173, 60);
            btnSelectAll.TabIndex = 19;
            btnSelectAll.Text = "SELECT ALL";
            btnSelectAll.UseVisualStyleBackColor = false;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // panel35
            // 
            panel35.Dock = DockStyle.Left;
            panel35.Location = new Point(0, 0);
            panel35.Name = "panel35";
            panel35.Size = new Size(36, 60);
            panel35.TabIndex = 19;
            // 
            // itemcountandgrossweight
            // 
            itemcountandgrossweight.BackColor = Color.FromArgb(233, 245, 219);
            itemcountandgrossweight.BorderStyle = BorderStyle.None;
            itemcountandgrossweight.Dock = DockStyle.Fill;
            itemcountandgrossweight.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            itemcountandgrossweight.Location = new Point(0, 0);
            itemcountandgrossweight.Name = "itemcountandgrossweight";
            itemcountandgrossweight.ReadOnly = true;
            itemcountandgrossweight.Size = new Size(1584, 40);
            itemcountandgrossweight.TabIndex = 0;
            itemcountandgrossweight.Text = "Item : 10      Gross Weight : 30.789";
            itemcountandgrossweight.TextAlign = HorizontalAlignment.Center;
            // 
            // panel32
            // 
            panel32.BackColor = Color.FromArgb(65, 72, 51);
            panel32.Dock = DockStyle.Left;
            panel32.Location = new Point(0, 0);
            panel32.Name = "panel32";
            panel32.Size = new Size(10, 60);
            panel32.TabIndex = 12;
            panel32.Paint += panel32_Paint;
            // 
            // panel23
            // 
            panel23.BackColor = Color.FromArgb(65, 72, 51);
            panel23.Dock = DockStyle.Right;
            panel23.Location = new Point(1594, 0);
            panel23.Name = "panel23";
            panel23.Size = new Size(10, 60);
            panel23.TabIndex = 10;
            panel23.Paint += panel23_Paint;
            // 
            // panel21
            // 
            panel21.BackColor = Color.FromArgb(65, 72, 51);
            panel21.Dock = DockStyle.Bottom;
            panel21.Location = new Point(0, 60);
            panel21.Name = "panel21";
            panel21.Size = new Size(1604, 5);
            panel21.TabIndex = 3;
            panel21.Paint += panel21_Paint;
            // 
            // panelBackground
            // 
            panelBackground.BackColor = Color.FromArgb(255, 255, 192);
            panelBackground.Controls.Add(panel6);
            panelBackground.Controls.Add(panel5);
            panelBackground.Controls.Add(panel4);
            panelBackground.Controls.Add(panel1);
            panelBackground.Dock = DockStyle.Fill;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(1604, 818);
            panelBackground.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.BackColor = Color.FromArgb(233, 245, 219);
            panel6.Controls.Add(panel37);
            panel6.Controls.Add(btnFetch);
            panel6.Controls.Add(panel36);
            panel6.Controls.Add(panel33);
            panel6.Controls.Add(btnAddGoldSave);
            panel6.Controls.Add(panel15);
            panel6.Controls.Add(btnAddGoldPrintTag);
            panel6.Controls.Add(panel13);
            panel6.Controls.Add(panel12);
            panel6.Controls.Add(panel14);
            panel6.Controls.Add(panel11);
            panel6.Dock = DockStyle.Bottom;
            panel6.Location = new Point(0, 688);
            panel6.Name = "panel6";
            panel6.Size = new Size(1604, 65);
            panel6.TabIndex = 5;
            // 
            // panel37
            // 
            panel37.Dock = DockStyle.Left;
            panel37.Location = new Point(219, 5);
            panel37.Name = "panel37";
            panel37.Size = new Size(36, 55);
            panel37.TabIndex = 22;
            panel37.Paint += panel37_Paint;
            // 
            // btnFetch
            // 
            btnFetch.BackColor = Color.FromArgb(96, 111, 73);
            btnFetch.Dock = DockStyle.Left;
            btnFetch.FlatAppearance.BorderColor = Color.FromArgb(3, 102, 102);
            btnFetch.FlatAppearance.BorderSize = 5;
            btnFetch.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 102, 102);
            btnFetch.FlatAppearance.MouseOverBackColor = Color.FromArgb(153, 226, 180);
            btnFetch.FlatStyle = FlatStyle.Popup;
            btnFetch.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            btnFetch.ForeColor = Color.White;
            btnFetch.Location = new Point(46, 5);
            btnFetch.Name = "btnFetch";
            btnFetch.Size = new Size(173, 55);
            btnFetch.TabIndex = 21;
            btnFetch.Text = "FETCH";
            btnFetch.UseVisualStyleBackColor = false;
            btnFetch.Click += btnFetch_Click;
            // 
            // panel36
            // 
            panel36.Dock = DockStyle.Left;
            panel36.Location = new Point(10, 5);
            panel36.Name = "panel36";
            panel36.Size = new Size(36, 55);
            panel36.TabIndex = 20;
            // 
            // panel33
            // 
            panel33.BackColor = Color.FromArgb(65, 72, 51);
            panel33.Dock = DockStyle.Left;
            panel33.Location = new Point(0, 5);
            panel33.Name = "panel33";
            panel33.Size = new Size(10, 55);
            panel33.TabIndex = 17;
            panel33.Paint += panel33_Paint;
            // 
            // btnAddGoldSave
            // 
            btnAddGoldSave.BackColor = Color.FromArgb(96, 111, 73);
            btnAddGoldSave.Dock = DockStyle.Right;
            btnAddGoldSave.FlatAppearance.BorderColor = Color.FromArgb(3, 102, 102);
            btnAddGoldSave.FlatAppearance.BorderSize = 5;
            btnAddGoldSave.FlatStyle = FlatStyle.Popup;
            btnAddGoldSave.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            btnAddGoldSave.ForeColor = Color.White;
            btnAddGoldSave.Location = new Point(982, 5);
            btnAddGoldSave.Name = "btnAddGoldSave";
            btnAddGoldSave.Size = new Size(190, 55);
            btnAddGoldSave.TabIndex = 16;
            btnAddGoldSave.Text = "SAVE";
            btnAddGoldSave.UseVisualStyleBackColor = false;
            btnAddGoldSave.Click += btnAddGoldSave_Click;
            // 
            // panel15
            // 
            panel15.Dock = DockStyle.Right;
            panel15.Location = new Point(1172, 5);
            panel15.Name = "panel15";
            panel15.Size = new Size(36, 55);
            panel15.TabIndex = 13;
            // 
            // btnAddGoldPrintTag
            // 
            btnAddGoldPrintTag.BackColor = Color.FromArgb(96, 111, 73);
            btnAddGoldPrintTag.Dock = DockStyle.Right;
            btnAddGoldPrintTag.FlatAppearance.BorderColor = Color.FromArgb(3, 102, 102);
            btnAddGoldPrintTag.FlatAppearance.BorderSize = 5;
            btnAddGoldPrintTag.FlatStyle = FlatStyle.Popup;
            btnAddGoldPrintTag.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            btnAddGoldPrintTag.ForeColor = Color.White;
            btnAddGoldPrintTag.Location = new Point(1208, 5);
            btnAddGoldPrintTag.Name = "btnAddGoldPrintTag";
            btnAddGoldPrintTag.Size = new Size(376, 55);
            btnAddGoldPrintTag.TabIndex = 12;
            btnAddGoldPrintTag.Text = "SAVE && PRINT SELECTED TAG";
            btnAddGoldPrintTag.UseVisualStyleBackColor = false;
            btnAddGoldPrintTag.Click += btnAddGoldPrintTag_Click;
            // 
            // panel13
            // 
            panel13.Dock = DockStyle.Right;
            panel13.Location = new Point(1584, 5);
            panel13.Name = "panel13";
            panel13.Size = new Size(10, 55);
            panel13.TabIndex = 11;
            // 
            // panel12
            // 
            panel12.BackColor = Color.FromArgb(65, 72, 51);
            panel12.Dock = DockStyle.Right;
            panel12.Location = new Point(1594, 5);
            panel12.Name = "panel12";
            panel12.Size = new Size(10, 55);
            panel12.TabIndex = 9;
            panel12.Paint += panel12_Paint;
            // 
            // panel14
            // 
            panel14.BackColor = SystemColors.ActiveCaptionText;
            panel14.Dock = DockStyle.Bottom;
            panel14.Location = new Point(0, 60);
            panel14.Name = "panel14";
            panel14.Size = new Size(1604, 5);
            panel14.TabIndex = 8;
            // 
            // panel11
            // 
            panel11.BackColor = SystemColors.ActiveCaptionText;
            panel11.Dock = DockStyle.Top;
            panel11.Location = new Point(0, 0);
            panel11.Name = "panel11";
            panel11.Size = new Size(1604, 5);
            panel11.TabIndex = 1;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // addgold
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1604, 818);
            Controls.Add(panelBackground);
            Name = "addgold";
            Text = "addgold";
            WindowState = FormWindowState.Maximized;
            Load += addgold_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel5.ResumeLayout(false);
            panel34.ResumeLayout(false);
            panel34.PerformLayout();
            panelBackground.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel4;
        private Panel panel5;
        private Panel panelBackground;
        private Panel panel6;
        private Panel panel11;
        private Button btnAddGoldSave;
        private Panel panel15;
        private Button btnAddGoldPrintTag;
        private Panel panel13;
        private Button btnAddGoldPrintPdf;
        private Panel panel12;
        private Panel panel28;
        private Panel panel27;
        private Panel panel22;
        private Panel panel31;
        private Panel panel24;
        private Panel panel20;
        private Panel panel32;
        private Panel panel23;
        private Panel panel21;
        private Panel panel33;
        private Panel panel34;
        private TextBox itemcountandgrossweight;
        private DataGridView dataGridView1;
        private Button btnSelectAll;
        private Button btnFetch;
        private Panel panel36;
        private Panel panel37;
        private CheckBox checkBoxAddGold1;
        private Panel panel35;
        private Panel panel16;
        private DataGridViewCheckBoxColumn select;
        private DataGridViewTextBoxColumn tagno;
        private DataGridViewComboBoxColumn type;
        private DataGridViewComboBoxColumn caret;
        private DataGridViewTextBoxColumn gross;
        private DataGridViewTextBoxColumn net;
        private DataGridViewTextBoxColumn labour;
        private DataGridViewTextBoxColumn wholeLabour;
        private DataGridViewTextBoxColumn other;
        private DataGridViewTextBoxColumn huid1;
        private DataGridViewTextBoxColumn huid2;
        private DataGridViewTextBoxColumn size;
        private DataGridViewTextBoxColumn comment;
        private TextBox textBox1;
        private Panel panel19;
        private Panel panel14;
        private ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}