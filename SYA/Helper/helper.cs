﻿using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;
namespace SYA.Helper
{
    public static class helper
    {
        private static IConfigurationRoot _configuration;
        private static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(@"D:\\SYA_SOFT\\config")
                        //.SetBasePath(@"F:\SYA_APP\SYA_SOFT_TEST\config")
                        .AddJsonFile("appsettings.json")
                        .Build();
                }
                return _configuration;
            }
        }
        public static string SYAContactConnectionString;
        public static string SyaSettingsDataBase => Configuration["ConnectionStrings:SyaSettingsDataBase"];
        public static string SYAConnectionString;
        public static string SYASummaryConnectionString => Configuration["ConnectionStrings:SYASummaryDatabase"];
        public static string accessConnectionString;
        public static string excelFile;
        public static string ImageFolder;
        public static string LogsFolder;
        public static string TagPrinterName;
        public static string NormalPrinterName;
        public static string DataVerificationOldTable;
        public static string DataVerificationNewTable;
        public static string GoldPerGramLabour;
        public static string SilverPerGramLabour;
        public static DataTable tableLabour = new DataTable();
        //i am chaning int to object so where we are implementing use proper conversion
        public static object RunQueryWithoutParameters(string connectionString, string query, string commandType)
        {
            object res = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            if (commandType == "ExecuteNonQuery")
                            {
                                res = command.ExecuteNonQuery();
                            }
                            else if (commandType == "ExecuteScalar")
                            {
                                res = command.ExecuteScalar();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"Error executing query:\n\n{query}\n\nCommand Type: {commandType}\n\nError Message: {ex.Message}\n\n{ex.StackTrace}\n\n{ex.InnerException}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display the error message in a custom form
                MessageBox.Show($"Outerr Error executing query:\n\n{query}\n\nCommand Type: {commandType}\n\nError Message: {ex.Message}");
                // Re-throw the exception for higher-level handling, if needed
                throw;
            }
            return res;
        }
        public static DataTable dt1 = new DataTable();
        public static void loadSettingsValues()
        {
            string query = "SELECT * FROM Settings";
            dt1 = FetchDataTableFromSYASettingsDataBase(query);
            foreach (DataRow row in dt1.Rows)
            {
                SYAConnectionString = row["SYADatabase"].ToString();
                accessConnectionString = row["DataCareDatabase"].ToString();
                SYAContactConnectionString = row["ContactDataBase"].ToString();
                excelFile = row["ExcelFile"].ToString();
                ImageFolder = row["Images"].ToString();
                LogsFolder = row["Logs"].ToString();
                TagPrinterName = row["TagPrinterName"].ToString();
                NormalPrinterName = row["NormalPrinterName"].ToString();
                DataVerificationOldTable = row["DataVerificationOldTable"].ToString();
                DataVerificationNewTable = row["DataVerificationNewTable"].ToString();
                GoldPerGramLabour = row["GoldPerGramLabour"].ToString();
                SilverPerGramLabour = row["SilverPerGramLabour"].ToString();
            }
        }
        public static void loadLabourTable()
        {
            string query = "SELECT * FROM Labour";
             tableLabour = FetchDataTableFromSYASettingsDataBase(query);
        }
        public static DataTable FetchDataTable(string connectionString, string query)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            SQLiteDataReader reader = command.ExecuteReader();
                            dataTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"Error executing query and filling DataTable: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Outer Error executing query and filling DataTable: {ex.Message}");
            }
            return dataTable;
        }
        public static object RunQueryWithoutParametersSYAContactDataBase(string query, string commandType)
        {
            return RunQueryWithoutParameters(SYAContactConnectionString, query, commandType);
        }
        public static object RunQueryWithoutParametersSyaSettingsDataBase(string query, string commandType)
        {
            return RunQueryWithoutParameters(SyaSettingsDataBase, query, commandType);
        }
        public static object RunQueryWithoutParametersSYADataBase(string query, string commandType)
        {
            return RunQueryWithoutParameters(SYAConnectionString, query, commandType);
        }
        public static bool RunQueryWithParametersSYAContactDataBase(string query, SQLiteParameter[] parameters = null)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SYAContactConnectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            if (parameters != null)
                            {
                                command.Parameters.AddRange(parameters);
                            }
                            command.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"RunQueryWithParametersSYADataBase Error executing query: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Outer RunQueryWithParametersSYADataBase Error executing query: {ex.Message}");
                return false;
            }
        }
        public static bool RunQueryWithParametersSYADataBase(string query, SQLiteParameter[] parameters = null)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SYAConnectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            if (parameters != null)
                            {
                                command.Parameters.AddRange(parameters);
                            }
                            command.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"RunQueryWithParametersSYADataBase Error executing query: {ex.Message}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Outer RunQueryWithParametersSYADataBase Error executing query: {ex.Message}");
                return false;
            }
        }
        public static DataTable FetchDataTableFromSYASettingsDataBase(string query)
        {
            return FetchDataTable(SyaSettingsDataBase, query);
        }
        public static DataTable FetchDataTableFromSYAContactDataBase(string query)
        {
            return FetchDataTable(SYAContactConnectionString, query);
        }
        public static DataTable FetchDataTableFromSYADataBase(string query)
        {
            return FetchDataTable(SYAConnectionString, query);
        }
        public static DataTable FetchDataTableFromSYADataBaseSummary(string query)
        {
            return FetchDataTable(SYASummaryConnectionString, query);
        }
        public static SQLiteDataReader FetchDataFromSYADataBase(string query)
        {
            SQLiteDataReader reader = null;
            try
            {
                SQLiteConnection connection = new SQLiteConnection(SYAConnectionString);
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(query, connection);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error executing reader: {ex.Message}");
            }
            return reader;
        }
        public static DataTable FetchDataTableFromDataCareDataBase(string query)
        {
            DataTable dataCareDataTable = new DataTable();
            try
            {
                using (OleDbConnection accessConnection = new OleDbConnection(accessConnectionString))
                {
                    accessConnection.Open();
                    try
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, accessConnection))
                        {
                            adapter.Fill(dataCareDataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show("FetchFromDataCareDataBase Error executing query: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show("Outer FetchFromDataCareDataBase Error executing query: " + ex.Message);
            }
            return dataCareDataTable;
        }
        public static bool validateHUID(string huid1, string huid2)
        {
            if (!string.IsNullOrWhiteSpace(huid1) && huid1.Length != 6 || !string.IsNullOrWhiteSpace(huid2) && huid2.Length != 6)
            {
                MessageBox.Show("HUID1 length must be 6 characters if not null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(huid2) && huid2.Length != 6)
            {
                MessageBox.Show("HUID2 length must be 6 characters if not null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(huid1) && !string.IsNullOrWhiteSpace(huid2))
            {
                MessageBox.Show("Please insert HUID1 at the correct place.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static bool validateType(string type)
        {
            if (type == null || string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Validation error: Type cannot be null or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static bool validateWeight(string weight)
        {
            if (!decimal.TryParse(weight, out decimal grossWeight) || grossWeight < 0)
            {
                MessageBox.Show("Validation error: Invalid or negative weight value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static bool validateWeight(string gross, string net)
        {
            if (decimal.Parse(gross) < decimal.Parse(net))
            {
                MessageBox.Show("Validation error: Gross weight cannot be less than net weight.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static bool validateLabour(string labour)
        {
            if (!decimal.TryParse(labour, out decimal labour1) || labour1 < 0)
            {
                MessageBox.Show("Validation error: Invalid or negative labour value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static bool validateOther(string other)
        {
            if (other != null && (!decimal.TryParse(other, out decimal other1) || other1 < 0))
            {
                MessageBox.Show("Validation error: Invalid or negative other value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        public static string correctWeight(object cellValue)
        {
            // Check if the entered value is not null and can be converted to a decimal
            if (cellValue != null && decimal.TryParse(cellValue.ToString(), out decimal weight))
            {
                // Format the entered value to have three decimal places
                return weight.ToString("0.000");
            }
            return (cellValue ?? "").ToString();
        }
    }
}
