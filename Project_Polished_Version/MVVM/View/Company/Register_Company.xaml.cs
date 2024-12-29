using System;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Linq;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;

namespace Project_Polished_Version
{  
    public partial class Register_Company : Window
    {
        public Register_Company()
        {
            InitializeComponent();
        }

        private void Next_btn(object sender, RoutedEventArgs e)
        {
            string companyName = Company_Name_TxtBox.Text.Trim();
            string companyEmail = Company_Email_TxtBox.Text.Trim();
            string companyMobileNumber = Company_PassWord_txtBox.Password;
            string companyAddress = Address_TxtBox.Text.Trim();

           
            if (string.IsNullOrWhiteSpace(companyName) ||
                string.IsNullOrWhiteSpace(companyEmail) ||
                string.IsNullOrWhiteSpace(companyMobileNumber) ||
                string.IsNullOrWhiteSpace(companyAddress))
            {
                MessageBox.Show("All fields are required. Please fill out all the information.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (companyMobileNumber.Any(c => !char.IsDigit(c)))
            {
                MessageBox.Show("The mobile number must contain only digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (companyEmail.Length < 8)
            {
                MessageBox.Show("The password must be at least 8 characters long.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string query = "INSERT INTO company_account (Company_Name, Company_address, password, email) " +
                           "VALUES (@Company_Name, @Company_address, @password, @Email,)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                   
                    connection.Open();
 
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                       
                        command.Parameters.AddWithValue("@Company_Name", companyName);
                        command.Parameters.AddWithValue("@Company_address", companyAddress);
                        command.Parameters.AddWithValue("@password", companyMobileNumber);
                        command.Parameters.AddWithValue("@Email", companyEmail);


                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            AddCompanyInfo();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCompanyInfo()
        {
            string insertQuery = "INSERT INTO company_info (userId, info_type, date_started, date_ended, content) VALUES (@userId, @info_type, @date_started, @date_ended, @content)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@userId", MainWindowMethods.companyID);
                        command.Parameters.AddWithValue("@info_type", "About_Post");
                        command.Parameters.AddWithValue("@date_started", DateTime.Now);
                        command.Parameters.AddWithValue("@date_ended", DateTime.Now);
                        command.Parameters.AddWithValue("@content", DBNull.Value);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Company info added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inserting company info: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
