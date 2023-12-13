using System;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
                        .SetBasePath(@"C:\Users\pvraj\OneDrive\Desktop\SYA\SYA\config")
                         .AddJsonFile("appsettings.json")
                        .Build();
                }
                return _configuration;
            }
        }

        public static string ConnectionString
        {
            get { return Configuration["ConnectionStrings:SYADatabase"]; }
        }

        public static string accessConnectionString
        {
            get { return Configuration["ConnectionStrings:DataCareDatabase"]; }
        }

      //   private static readonly string ConnectionString = "Data Source=C:\\Users\\pvraj\\OneDrive\\Desktop\\SYA\\SYADataBase.db;Version=3;";
      //  private static readonly string accessConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\pvraj\\OneDrive\\Desktop\\DataCare23.mdb\"";
     //   private static readonly string ConnectionString = "Data Source=C:\\Users\\91760\\Desktop\\SYA\\SYADataBase.db;Version=3;";
     //   private static readonly string accessConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\91760\\Desktop\\SYA\\DataCare23.mdb\"";
        //SYA Database Query
        public static object RunQueryWithoutParametersSYADataBase(string query,string commandType)
        {
           // int rowsAffected = 0;
           object result = null;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {   if (commandType == "ExecuteNonQuery")
                        {
                           result =  command.ExecuteNonQuery();
                        }
                    else if(commandType == "ExecuteScalar")
                        {
                           result =  command.ExecuteScalar();
                        }
                      //  rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error executing non-query: {ex.Message}");
            }
            return result;
           // return rowsAffected;
        }
        public static void RunQueryWithParametersSYADataBase(string query, SQLiteParameter[] parameters = null)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                Console.WriteLine($"Error executing query: {ex.Message}");
            }
        }
        public static SQLiteDataReader FetchDFromSYADataBase(string query)
        {
            SQLiteDataReader reader = null;

            try
            {
                SQLiteConnection connection = new SQLiteConnection(ConnectionString);
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
        //Datacare Database Query
        public static DataTable FetchFromDataCareDataBase(string query)
        {
            DataTable dataCareDataTable = new DataTable();
            try
            {
                using (OleDbConnection accessConnection = new OleDbConnection(accessConnectionString))
                {
                    accessConnection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, accessConnection))
                    {
                        adapter.Fill(dataCareDataTable);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Failed to get or create table from datacare data : "+ex.Message); }
            return dataCareDataTable;
        }
        //validations
        public static bool validateHUID(string huid1,string huid2)
        {
            if ((!string.IsNullOrWhiteSpace(huid1) && huid1.Length != 6) || (!string.IsNullOrWhiteSpace(huid2) && huid2.Length != 6))
            {
                MessageBox.Show("HUID1 length must be 6 characters if not null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(!string.IsNullOrWhiteSpace(huid2) && huid2.Length != 6) 
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
           
                return false;
            }
            return true;
        }
        public static bool validateWeight(string weight)
        {
            if (!decimal.TryParse(weight, out decimal grossWeight) || grossWeight < 0)
            {

                return false;
            }
            return true;
        }
        public static bool validateWeight(string gross,string net)
        {
            if (decimal.Parse(gross) < decimal.Parse(net))
            {
                return false;
            }
            return true;
        }
        public static bool validateLabour(string labour)
        {
            if (!decimal.TryParse(labour, out decimal labour1) || labour1 < 0)
            {
                
                return false;
            }
            return true;
        }
        public static bool validateOther(string other)
        {
            if (other != null && (!decimal.TryParse(other, out decimal other1) || other1 < 0))
            {
             
                return false;
            }
            return true;
        }
    }
}
