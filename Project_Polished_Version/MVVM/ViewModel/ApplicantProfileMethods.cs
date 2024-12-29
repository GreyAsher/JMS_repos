using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project_Polished_Version.Methods
{
    public class ApplicantProfileMethods
    {
        public static void SaveData(TextBox textBox, int id,string content)
        {
            string queryUpdate = $"UPDATE applicant_account SET {content} = @User_Content WHERE Applicant_Id = @Applicant_Id";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(queryUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@User_Content", textBox.Text);
                        command.Parameters.AddWithValue("@Applicant_Id", id);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("No changes were made to the database.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static async Task<string> GetSectionDataAsync(string infoType)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM applicant_account WHERE Applicant_Id = @Applicant_Id AND User_Type = @User_Type";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Applicant_Id", MainWindowMethods.applicantID);
                        cmd.Parameters.AddWithValue("@User_Type", infoType);

                        using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return reader["User_Content"]?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Task<string> GetSectionDataAsync(string infoType):  {infoType} data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return string.Empty;
        }
    }
}
