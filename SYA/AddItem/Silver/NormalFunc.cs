using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SYA
{
    public class NormalFunc
    {
        public static void enterkeyhandle(DataGridView dataGridView1, KeyEventArgs e)
        {
            MessageBox.Show("as");
            if (e.KeyCode == Keys.Enter)
            {
                // Commit the cell edit if the cell is in edit mode
                if (dataGridView1.IsCurrentCellInEditMode)
                {
                    dataGridView1.EndEdit();
                }
                // Get the currently active cell
                var currentCell = dataGridView1.CurrentCell;
                if (currentCell != null)
                {
                    int currentColumnIndex = currentCell.ColumnIndex;
                    int currentRowIndex = currentCell.RowIndex;
                    // Move to the next column
                    int nextColumnIndex = (currentColumnIndex + 1) % dataGridView1.Columns.Count;
                    // If moving to the next column exceeds the number of columns, move to the next row
                    if (nextColumnIndex == 0)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[currentRowIndex].Cells[nextColumnIndex];
                    }
                    else
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[currentRowIndex].Cells[nextColumnIndex];
                    }
                    // Prevent the default behavior of Enter key
                    e.SuppressKeyPress = true;
                }
            }
        }
        public static void SelectCell(DataGridView dataGridView, int rowIndex, string columnName)
        {
            dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[columnName];
            dataGridView.BeginEdit(true);
        }
        public static bool ValidateData(DataGridViewRow row, DataGridView addSilverDataGridView)
        {
            if (row.Cells["type"].Value == null || string.IsNullOrWhiteSpace(row.Cells["type"].Value.ToString()))
            {
                MessageBox.Show($"Please add a valid type for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "type");
                return false;
            }
            if (!decimal.TryParse(row.Cells["gross"].Value?.ToString(), out decimal grossWeight) || grossWeight < 0)
            {
                MessageBox.Show($"Gross weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "gross");
                return false;
            }
            if (!decimal.TryParse(row.Cells["net"].Value?.ToString(), out decimal netWeight) || netWeight < 0)
            {
                MessageBox.Show($"Net weight should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "net");
                return false;
            }
            if (grossWeight < netWeight)
            {
                MessageBox.Show($"Gross weight should be greater than or equal to net weight for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "gross");
                return false;
            }
            if (!decimal.TryParse(row.Cells["labour"].Value?.ToString(), out decimal labour) || labour < 0)
            {
                MessageBox.Show($"Labour should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "labour");
                return false;
            }
            if (!decimal.TryParse(row.Cells["wholeLabour"].Value?.ToString(), out decimal wholeLabour) || wholeLabour < 0)
            {
                MessageBox.Show($"Whole Labour should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "wholeLabour");
                return false;
            }
            if (row.Cells["price"].Value != null && (!decimal.TryParse(row.Cells["price"].Value?.ToString(), out decimal price) || price < 0))
            {
                MessageBox.Show($"Price should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "price");
                return false;
            }
            if (row.Cells["other"].Value != null && (!decimal.TryParse(row.Cells["other"].Value?.ToString(), out decimal other) || other < 0))
            {
                MessageBox.Show($"Other should be a non-negative numeric value for Row {row.Index + 1}.");
                SelectCell(addSilverDataGridView, row.Index, "other");
                return false;
            }
            return true;
        }
    }
}
