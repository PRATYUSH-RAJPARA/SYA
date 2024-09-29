using System;
using System.Windows.Forms;
namespace SYA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupDataGridView();
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void SetupDataGridView()
        {
            // Create and set properties for DataGridView
            DataGridView dataGridView = new DataGridView();
            dataGridView.Name = "dataGridView1"; // Name to reference in event handler
            dataGridView.Dock = DockStyle.Fill; // Make the DataGridView fill the form
            dataGridView.AllowUserToAddRows = true; // Allow adding rows manually
            dataGridView.AllowUserToDeleteRows = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // Add five columns to the DataGridView
            for (int i = 1; i <= 5; i++)
            {
                dataGridView.Columns.Add("Column" + i, "Column " + i);
            }
            // Set event handler for handling key press (specifically the Enter key)
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            dataGridView.KeyDown += DataGridView_KeyDown;
            // Add the DataGridView to the form
            this.Controls.Add(dataGridView);
        }
        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView1 = sender as DataGridView;
            if (e.KeyCode == Keys.Enter)
            {
                // Suppress the default Enter key behavior
                e.SuppressKeyPress = true;
                // Get the current column and row index
                int iColumn = dataGridView1.CurrentCell.ColumnIndex;
                int iRow = dataGridView1.CurrentCell.RowIndex;
                // If it's the last column, move to the first column of the next row
                if (iColumn == dataGridView1.ColumnCount - 1)
                {
                    // Check if there's a next row
                    if (dataGridView1.RowCount > (iRow + 1))
                    {
                        // Move to the first column of the next row
                        dataGridView1.CurrentCell = dataGridView1[0, iRow + 1];
                    }
                    else
                    {
                        // Focus on the next control if no next row exists
                        this.SelectNextControl(dataGridView1, true, true, true, true);
                    }
                }
                else
                {
                    // Move to the next cell in the same row
                    dataGridView1.CurrentCell = dataGridView1[iColumn + 1, iRow];
                }
            }
        }
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView gridDetails = sender as DataGridView;
            if (MouseButtons != 0) return;
            if (_lastCellEndEdit != null && gridDetails.CurrentCell != null)
            {
                // if we are currently in the next line of last edit cell
                if (gridDetails.CurrentCell.RowIndex == _lastCellEndEdit.RowIndex + 1 &&
                    gridDetails.CurrentCell.ColumnIndex == _lastCellEndEdit.ColumnIndex)
                {
                    int iColNew;
                    int iRowNew = 0;
                    var lastCellEndEditColumn = gridDetails.Columns[_lastCellEndEdit.ColumnIndex];
                    if (lastCellEndEditColumn.DisplayIndex >= GetVisibleColumnsMaxDisplayIndex(sender))
                    {
                        iColNew = GetFirstVisibleColumnIndex(sender);
                        iRowNew = gridDetails.CurrentCell.RowIndex;
                    }
                    else
                    {
                        iColNew = GetNextVisibleColumnIndex(sender,lastCellEndEditColumn.DisplayIndex);
                        iRowNew = _lastCellEndEdit.RowIndex;
                    }
                    gridDetails.CurrentCell = gridDetails[iColNew, iRowNew];
                }
            }
            _lastCellEndEdit = null;
        }
        private DataGridViewCell _lastCellEndEdit;
        private void gridDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gridDetails = sender as DataGridView;
            _lastCellEndEdit = gridDetails[e.ColumnIndex, e.RowIndex];
        }
        int GetVisibleColumnsMaxDisplayIndex(object sender)
        {
            DataGridView gridDetails = sender as DataGridView;
            int maxDisplayIndex = -1;
            foreach (DataGridViewColumn column in gridDetails.Columns)
            {
                if (column.Visible && column.DisplayIndex > maxDisplayIndex)
                {
                    maxDisplayIndex = column.DisplayIndex;
                }
            }
            return maxDisplayIndex;
        }
        int GetFirstVisibleColumnIndex(object sender)
        {
            DataGridView gridDetails = sender as DataGridView;
            var firstVisibleColumnDisplayIndex = gridDetails.Columns.Count - 1;
            var firstVisibleColumnIndex = 0;
            foreach (DataGridViewColumn column in gridDetails.Columns)
            {
                if (column.Visible && column.DisplayIndex < firstVisibleColumnDisplayIndex)
                {
                    firstVisibleColumnDisplayIndex = column.DisplayIndex;
                    firstVisibleColumnIndex = column.Index;
                }
            }
            return firstVisibleColumnIndex;
        }
        int GetNextVisibleColumnIndex(object sender, int previousColumnDisplayIndex)
        {
            DataGridView gridDetails = sender as DataGridView;
            var nextVisibleColumnDisplayIndex = gridDetails.Columns.Count;
            var nextVisibleColumnIndex = previousColumnDisplayIndex;
            foreach (DataGridViewColumn column in gridDetails.Columns)
            {
                if (column.Visible && column.DisplayIndex > previousColumnDisplayIndex
                    && (column.DisplayIndex - previousColumnDisplayIndex) <= (nextVisibleColumnDisplayIndex - previousColumnDisplayIndex))
                {
                    nextVisibleColumnDisplayIndex = column.DisplayIndex;
                    nextVisibleColumnIndex = column.Index;
                }
            }
            return nextVisibleColumnIndex;
        }
    }
}
