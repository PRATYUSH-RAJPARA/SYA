using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace SYA
{
    public partial class statement : Form
    {
        public class Transaction
        {
            public DateTime Date { get; set; }
            public string Particulars { get; set; }
            public string ChqNo { get; set; }
            public decimal Withdrawals { get; set; }
            public decimal Deposits { get; set; }
            public decimal Balance { get; set; }
        }
        private void statement_Load(object sender, EventArgs e)
        {
            // Leave this method empty for now, or add any initialization code you need.
        }
        public statement()
        {
            InitializeComponent();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Add("Date", "Date");
            dataGridView1.Columns.Add("Particulars", "Particulars");
            dataGridView1.Columns.Add("ChqNo", "Chq. no");
            dataGridView1.Columns.Add("Withdrawals", "Withdrawals");
            dataGridView1.Columns.Add("Deposits", "Deposits");
            dataGridView1.Columns.Add("Balance", "Balance");
        }

        private void ProcessDataButton_Click(object sender, EventArgs e)
        {
            string data = richTextBox1.Text;
            List<Transaction> transactions = ProcessData(data);
            dataGridView1.Rows.Clear(); // Clear existing rows
            foreach (var transaction in transactions)
            {
                dataGridView1.Rows.Add(
                    transaction.Date.ToString("dd-MM-yyyy"),
                    transaction.Particulars,
                    transaction.ChqNo,
                    transaction.Withdrawals.ToString("N2"),
                    transaction.Deposits.ToString("N2"),
                    transaction.Balance.ToString("N2")
                );
            }
        }

        private List<Transaction> ProcessData(string data)
        {
            List<Transaction> transactions = new List<Transaction>();

            string[] lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split('\t', StringSplitOptions.RemoveEmptyEntries);

                if (columns.Length >= 6)
                {
                    Transaction transaction = new Transaction();
                    transaction.Date = DateTime.ParseExact(columns[0], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    transaction.Particulars = columns[1];
                    transaction.ChqNo = columns[2];

                    decimal.TryParse(columns[3].Replace(",", ""), out decimal withdrawals);
                    transaction.Withdrawals = withdrawals;

                    decimal.TryParse(columns[4].Replace(",", ""), out decimal deposits);
                    transaction.Deposits = deposits;

                    decimal.TryParse(columns[5].Replace(",", "").Replace("Cr", "").Replace("Dr", ""), out decimal balance);
                    transaction.Balance = balance;

                    transactions.Add(transaction);
                }
            }

            return transactions;
        }
    }
}
