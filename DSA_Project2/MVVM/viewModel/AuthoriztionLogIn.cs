using DSA_Project2.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DSA_Project2.MVVM.viewModel
{
    public class AuthoriztionLogIn
    {
 
        //public static async Task LoadDataIntoMemoryApplicantAsync(string username)
        //{
        //    using (HttpClient con = new HttpClient())
        //    {
        //        await con.OpenAsync();
        //        string query = @"SELECT * FROM applicant_accounts WHERE Email = @applicant_Email LIMIT 1";

        //        using (MySqlCommand cmd = new MySqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@applicant_Email", username);

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    MessageBox.Show($"Retrieved Email: {reader["Email"]}");
        //                    MessageBox.Show($"Retrieved UserType: {reader["UserType"]}");
        //                    if (username.Equals(reader["Email"].ToString(), StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        applicantID = reader.GetValueOrDefault<int>("id");
        //                        password = reader.GetValueOrDefault<string>("password");
        //                        applicant_type = reader.GetValueOrDefault<string>("UserType");

        //                        ApplicantUser = new ApplicantUser()
        //                        {
        //                            First_Name = reader.GetValueOrDefault<string>("First_Name"),
        //                            Applicant_Photo = reader.GetValueOrDefault<string>("Applicant_Photo"),
        //                            JobTitle = reader.GetValueOrDefault<string>("JobTitle")
        //                        };
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public static async Task LoadDataIntoMemoryCompanyAsync(string username)
        //{
        //    using (MySqlConnection con = new MySqlConnection(ConnectionClass.ConnectionString))
        //    {
        //        await con.OpenAsync();

        //        string query = @"SELECT * FROM company_accounts WHERE Email = @applicant_Email LIMIT 1";
        //        using (MySqlCommand cmd = new MySqlCommand(query, con))
        //        {
        //            cmd.Parameters.AddWithValue("@applicant_Email", username);

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    if (username.Equals(reader["Email"].ToString(), StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        companyID = reader.GetValueOrDefault<int>("id");
        //                        password = reader.GetValueOrDefault<string>("password");
        //                        applicant_type = reader.GetValueOrDefault<string>("UserType");

        //                        CompanyUser = new CompanyUser()
        //                        {
        //                            CompanyName = reader.GetValueOrDefault<string>("company_name"),
        //                            CompanyAddress = reader.GetValueOrDefault<string>("company_address"),
        //                            CompanyEmail = reader.GetValueOrDefault<string>("Email"),
        //                            CompanyId = reader.GetValueOrDefault<int>("company_id"),
        //                            IsAccepted = reader.GetValueOrDefault<string>("_Status"),
        //                        };
        //                    }
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //}


    }
}
