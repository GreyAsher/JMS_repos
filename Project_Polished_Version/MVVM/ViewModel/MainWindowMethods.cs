using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Project_Polished_Version.Methods
{
    public class MainWindowMethods
    {
        private static string Password { get; set; }
        public static int companyID { get; set; }
        public static int applicantID { get; set; }
        public static string applicant_type { get; set; }
        public static bool userType { get; set; }

        public static ApplicantUser ApplicantUser { get; set; } = new ApplicantUser();

        public static CompanyUser CompanyUser { get; set; }
        public static async Task LoadDataIntoMemoryApplicantAsync(string username)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                await con.OpenAsync();
                string query = @"SELECT * FROM applicant_account WHERE Applicant_Email = @applicant_Email LIMIT 1";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@applicant_Email", username);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            MessageBox.Show($"Retrieved Email: {reader["Applicant_Email"]}");
                            MessageBox.Show($"Retrieved UserType: {reader["User_Type"]}");
                            if (username.Equals(reader["Applicant_Email"].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                applicantID = reader.GetValueOrDefault<int>("Applicant_Id");
                                Password = reader["Applicant_Password"].ToString();
                                applicant_type = reader["User_Type"].ToString();

                                ApplicantUser = new ApplicantUser()
                                {
                                    Applicant_First_Name = reader.GetValueOrDefault<string>("Applicant_First_Name"),
                                    Applicant_Photo = reader.GetValueOrDefault<string>("Applicant_Photo"),
                                    Applicant_JobTitle = reader.GetValueOrDefault<string>("Applicant_JobTitle"),
                                    Applicant_Address = reader.GetValueOrDefault<string>("Applicant_Address"),
                                    Applicant_Gender = reader.GetValueOrDefault<string>("Applicant_Gender"),
                                    Applicant_Last_Name = reader.GetValueOrDefault<string>("Applicant_Last_Name"),
                                    Applicant_PhoneNumber = reader.GetValueOrDefault<string>("Applicant_PhoneNumber"),
                                    Applicant_Email = reader.GetValueOrDefault<string>("Applicant_Email"),
                                 
                                };
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static async Task LoadDataIntoMemoryCompanyAsync(string username)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                await con.OpenAsync();

                string query = @"SELECT * FROM company_account WHERE Company_Email = @Company_Email LIMIT 1";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Company_Email", username);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (username.Equals(reader["Company_Email"].ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                companyID = reader.GetValueOrDefault<int>("Company_Number");
                                Password = reader.GetValueOrDefault<string>("Company_Password");
                                applicant_type = reader.GetValueOrDefault<string>("User_Type");

                                CompanyUser = new CompanyUser()
                                {
                                    CompanyName = reader.GetValueOrDefault<string>("Company_Name"),
                                    CompanyAddress = reader.GetValueOrDefault<string>("Company_Address"),
                                    CompanyEmail = reader.GetValueOrDefault<string>("Company_Email"),
                                    CompanyId = reader.GetValueOrDefault<int>("Company_Number"),
                                    IsAccepted = reader.GetValueOrDefault<string>("IsAccepted"),
                                    TimeCreates = reader.GetValueOrDefault<string>("Created_At"),
                                    Photo = reader.GetValueOrDefault<string>("Company_Photo"),
                                };
                            }
                            return;
                        }
                    }
                }
            }
        }
        public static async Task AuthenticateUserAsync(string password1, TextBox tx)
        {
            string username = tx.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password1))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }
            MessageBox.Show($"applicant_type: {applicant_type}");
            MessageBox.Show($"ApplicantUser: {ApplicantUser}");

            await LoadDataIntoMemoryApplicantAsync(username);
            await LoadDataIntoMemoryCompanyAsync(username);


            if (ApplicantUser == null && CompanyUser == null)
            {
                MessageBox.Show("User not found. Please check your email.");
                return;
            }


           
            if (applicant_type == "Applicant" && Password == password1)
            {
                userType = false;
                new Applicant_DashBoard().Show();
                return;
            }

          

            if (applicant_type == "Company" && Password == password1)
            {
                userType = true;
                //Application.Current.MainWindow.Close();
                new Applicant_DashBoard().Show();  
                return;
            }

            MessageBox.Show("Wrong Email or Password");
        }

    }
}
