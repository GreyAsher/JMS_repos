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
    public class CompanyProfileMethods
    {
        public static string GetAboutData()
        {
            string content = "";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT User_Content FROM Company_Information WHERE Company_Number = @Company_Number AND User_Type = @User_Type";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Company_Number", MainWindowMethods.companyID);
                        cmd.Parameters.AddWithValue("@User_Type", "Company");

                        using (MySqlDataReader reader =  cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                content = reader["User_Content"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show($"MainWindowMethods.companyID { MainWindowMethods.companyID}");
            return content;
        }

        public static void SaveData(TextBox textBox, string infoType)
        {
            string queryUpdate = "UPDATE company_account SET Company_About = @Company_Content WHERE Company_Number = @Company_Number AND User_Type = @User_Type";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(queryUpdate, connection))
                    {
                        command.Parameters.AddWithValue("@Company_Content", textBox.Text);
                        command.Parameters.AddWithValue("@User_Type", infoType);
                        command.Parameters.AddWithValue("@Company_Number", MainWindowMethods.companyID);

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

    }
}
