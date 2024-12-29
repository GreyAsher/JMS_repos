using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Project_Polished_Version.Classes
{
    public class Global_Repository
    {
        public static List<Jobs> JobFeed { get; set; } = new List<Jobs>();

        public static async Task<List<Jobs>> GetJobsFromDataBase()
        {
            var jobFeedList = new List<Jobs>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM jobs_database";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            jobFeedList.Add(new Jobs
                            {
                                Job_Title = reader["Job_Title"].ToString(),
                                Job_Position = reader["Job_Position"].ToString(),
                                Job_Description = reader["Job_Description"].ToString(),
                                IsFilled = reader["is_filled"].ToString(),
                                Job_id = Convert.ToInt32(reader["Job_id"]),
                                Company_userNumber = Convert.ToInt32(reader["Company_userNumber"]),
                                Job_Salary = reader["Salary"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching jobs: {ex.Message}", "Error", MessageBoxButton.OK);
            }

            return jobFeedList;
        }


        public static void AddMultiple_Rows_Columns_DB<T>(string table, T item, MySqlConnection connection)
        {
            try
            {
                var database_Properties = typeof(T).GetProperties().Where(p => p.GetValue(item) != null);
                string columnsDataBase = string.Join(", ", database_Properties.Select(p => p.Name));
                string parametersDataBase = string.Join(", ", database_Properties.Select(p => "@" + p.Name));

                string myQuery = $"Insert into {table} ({columnsDataBase}) Values ({parametersDataBase})";
                MessageBox.Show($"Generated Query: {myQuery}");
                using (var cmd = new MySqlCommand(myQuery, connection))
                {
                    foreach (var propertiesDB in database_Properties)
                    {
                        var value = propertiesDB.GetValue(item) ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@" + propertiesDB.Name, propertiesDB.GetValue(item) ?? DBNull.Value);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show($"{rowsAffected} row(s) inserted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error1: {ex.Message}");
            }

        }

        public static void Insert_WithoutID<T>(string table, T item, MySqlConnection connection)
        {
            try
            {

                var database_Properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "Id" && p.GetValue(item) != null);

                string columnsDataBase = string.Join(", ", database_Properties.Select(p => p.Name));
                string parametersDataBase = string.Join(", ", database_Properties.Select(p => "@" + p.Name));

                string myQuery = $"INSERT INTO {table} ({columnsDataBase}) VALUES ({parametersDataBase})";
                var debugParameters = string.Join(", ", database_Properties.Select(p =>
                    $"{p.Name} = '{p.GetValue(item) ?? "NULL"}'"));

                MessageBox.Show($"Generated Query: {myQuery}\nParameters: {debugParameters}");

                using (var cmd = new MySqlCommand(myQuery, connection))
                {
                    foreach (var propertiesDB in database_Properties)
                    {
                        var value = propertiesDB.GetValue(item) ?? DBNull.Value;
                        cmd.Parameters.AddWithValue("@" + propertiesDB.Name, value);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show($"{rowsAffected} row(s) inserted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }
    }
}
