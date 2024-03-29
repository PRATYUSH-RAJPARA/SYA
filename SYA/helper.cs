using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace SYA
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
                        .SetBasePath(@"F:\SYA_APP\SYA_SOFT_TEST\config")
                        .AddJsonFile("appsettings.json")
                        .Build();
                }

                return _configuration;
            }
        }

        public static string SYAConnectionString
        {
            get { return Configuration["ConnectionStrings:SYADatabase"]; }
        }

        public static string accessConnectionString
        {
            get { return Configuration["ConnectionStrings:DataCareDatabase"]; }
        }
        public static string excelFile
        {
            get { return Configuration["FolderLocations:ExcelFile"]; }
        }
        public static string ImageFolder
        {
            get { return Configuration["FolderLocations:Images"]; }
        }
        public static string LogsFolder
        {
            get { return Configuration["FolderLocations:Logs"]; }
        }

        //i am chaning int to object so where we are implementing use proper conversion
        public static object RunQueryWithoutParametersSYADataBase(string query, string commandType)
        {
            int result = 0;
            object res =null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SYAConnectionString))
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
                               // result = Convert.ToInt32(res);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"Error executing query:\n\n{query}\n\nCommand Type: {commandType}\n\nError Message: {ex.Message}");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // Display the error message in a custom form
                MessageBox.Show($"Outer Error executing query:\n\n{query}\n\nCommand Type: {commandType}\n\nError Message: {ex.Message}");

                // Re-throw the exception for higher-level handling, if needed
                throw;
            }

            return res;
        }

        public static object RunQueryWithoutParametersSYADataBasemain(string query, string commandType)
        {
            object result = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SYAConnectionString))
                {
                    connection.Open();
                    try
                    {
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            if (commandType == "ExecuteNonQuery")
                            {
                                result = command.ExecuteNonQuery();
                            }
                            else if (commandType == "ExecuteScalar")
                            {
                                result = command.ExecuteScalar();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show($"RunQueryWithoutParametersSYADataBasemain Error executing non-query: {ex.Message}");
                    }
                   
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Outer RunQueryWithoutParametersSYADataBasemain Error executing non-query: {ex.Message}");
            }
            return result;
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

        public static DataTable FetchDataTableFromSYADataBase(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(SYAConnectionString))
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
                        MessageBox.Show($"FetchDataTableFromSYADataBase Error executing query and filling DataTable: {ex.Message}");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show($"Outer FetchDataTableFromSYADataBase Error executing query and filling DataTable: {ex.Message}");
            }

            return dataTable;
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
          //  public static SQLiteDataReader FetchDataFromSYADataBase(string query)
     //   {
      //      SQLiteDataReader reader = null;

       //     try
       //     {
       //         using (SQLiteConnection connection = new SQLiteConnection(SYAConnectionString))
         //       {
        //            connection.Open();
         //           try
          //          {
             //           using (SQLiteCommand command = new SQLiteCommand(query, connection))
        //                {
        //                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
        //                    // Read data from the reader here, within the same using block
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle or log the exception as needed
        //                MessageBox.Show($"FetchDataFromSYADataBase Error executing reader: {ex.Message}");
        //            }
        //        } // The connection will be automatically closed here.
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle or log the exception as needed
        //        MessageBox.Show($"Outer FetchDataFromSYADataBase Error: {ex.Message}");
        //    }

        //    return reader;
        //}


        public static DataTable FetchFromDataCareDataBase(string query)
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

        public static DataTable FetchFromDataCareDataBaseWithParameters(string query, params OleDbParameter[] parameters)
        {
            DataTable dataCareDataTable = new DataTable();
            try
            {
                using (OleDbConnection accessConnection = new OleDbConnection(accessConnectionString))
                {
                    accessConnection.Open();
                    try
                    {
                        using (OleDbCommand command = new OleDbCommand(query, accessConnection))
                        {
                            if (parameters != null && parameters.Length > 0)
                            {
                                command.Parameters.AddRange(parameters);
                            }

                            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                            {
                                adapter.Fill(dataCareDataTable);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception as needed
                        MessageBox.Show("FetchFromDataCareDataBaseWithParameters Error executing query: " + ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                MessageBox.Show("Outer FetchFromDataCareDataBaseWithParameters Error executing query: " + ex.Message);
            }
            return dataCareDataTable;
        }

        public static bool validateHUID(string huid1, string huid2)
        {
            if ((!string.IsNullOrWhiteSpace(huid1) && huid1.Length != 6) || (!string.IsNullOrWhiteSpace(huid2) && huid2.Length != 6))
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
