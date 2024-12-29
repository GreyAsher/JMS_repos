using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;


namespace Project_Polished_Version.Methods
{
    public class DataStructures
    {
        //Use Control + F to search for the list, <Class Name> List

        public static List<Email> EmailList = new List<Email>();

        //Company UserList
         public static List<CompanyUser> GetCompanyDataBase()
        {
            List<CompanyUser> companyList = new List<CompanyUser>();


            using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM company_account";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CompanyUser item = new CompanyUser
                            {
                                CompanyName = reader["Company_Name"].ToString(),
                                CompanyEmail = reader["Company_Email"].ToString(),
                                CompanyPassword = reader["Company_Password"].ToString(),
                                CompanyAddress = reader["Company_Address"].ToString(),
                                CompanyId = Convert.ToInt32(reader["Company_Number"]),
                                User_Type = reader["User_Type"].ToString(),
                                TimeCreates = reader["Created_At"].ToString(),
                                Photo = reader["Company_Photo"].ToString(),
                                Company_About = reader["Company_About"].ToString()

                            };
                            companyList.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }

            return companyList;
        }

        //Applicant
        public static List<ApplicantUser> GetApplicantData()
        {
            List<ApplicantUser> applicantUserList = new List<ApplicantUser>();
            try
            {
                
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    string query = "SELECT * FROM applicant_account";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader =  cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ApplicantUser applicant = new ApplicantUser()
                                {
                                    Applicant_First_Name = reader.GetValueOrDefault<string>("Applicant_First_Name"),
                                    Applicant_Address = reader.GetValueOrDefault<string>("Applicant_Address"),
                                    Applicant_Photo = reader.GetValueOrDefault<string>("Applicant_Photo"),
                                    Applicant_Email = reader.GetValueOrDefault<string>("Applicant_Email"),
                                    Applicant_Gender = reader.GetValueOrDefault<string>("Applicant_Gender"),
                                    Id = reader.GetValueOrDefault<int>("Applicant_Id"),
                                    Applicant_JobTitle = reader.GetValueOrDefault<string>("Applicant_JobTitle"),
                                    Applicant_PhoneNumber = reader.GetValueOrDefault<string>("Applicant_PhoneNumber"),
                                    Applicant_Last_Name = reader.GetValueOrDefault<string>("Applicant_Last_Name"),
                                    IsAccepted = reader.GetValueOrDefault<string>("IsAccepted")
                                };
                                applicantUserList.Add(applicant);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error GetApplicantData(): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return applicantUserList;
        }


        ///Resume List 
          public static List<Resume> GetResumeFromDatabase()
        {
            List<Resume> pendingFeed = new List<Resume>();
            using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM resume_database";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Resume item = new Resume
                            {
                                Applicant_id = Convert.ToInt32(reader["Applicant_id"]),
                                CompanyID = Convert.ToInt32(reader["CompanyId"]),
                                Status = reader["Status"].ToString(),
                                Reusme_id = Convert.ToInt32(reader["Reusme_id"]),
                                Resume_pdf = reader["Resume_pdf"].ToString(),
                                Resume_Job_Position = reader["Resume_Job_Position"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                Submission_Date = DateTime.Now.ToString(),
                            };
                            pendingFeed.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                        MessageBox.Show("Error: " + ex.Message);
                    }
            }
            return pendingFeed;
        }


        //Inboxs 

        public static void GetEmailFromDataBase()
        {
            string query = "SELECT * FROM emails_db";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Email email = new Email
                        {
                            Id = Convert.ToInt32(row["emailIDNumber"]),
                            Sender = row["Sender"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Content = row["Message"].ToString()
                        };
                        EmailList.Add(email);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void GetListFromDataBase()
        {
            string query = "SELECT * FROM emails_db";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Email email = new Email
                        {
                            Id = Convert.ToInt32(row["emailIDNumber"]),
                            Sender = row["Sender"].ToString(),
                            Subject = row["Subject"].ToString(),
                            Content = row["Message"].ToString()
                        };
                        EmailList.Add(email);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static async Task<List<Jobs>> GetJobDataBase()
        {
            var jobFeedList = new List<Jobs>();

            using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM job_database";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            jobFeedList.Add(new Jobs
                            {
                                Job_Title = reader["Job_Title"].ToString(),
                                Job_Position = reader["Job_Position"].ToString(),
                                Job_Description = reader["Job_Description"].ToString(),
                                IsFilled = reader["isFilled"].ToString(),
                                Job_id = Convert.ToInt32(reader["Job_Id"]),
                                Company_userNumber = Convert.ToInt32(reader["Company_userNumber"]),
                                Job_Salary = reader["Job_Salary"].ToString(),
                                Job_Specification = reader["Specification"].ToString(),
                                Job_PostedTime = reader["Job_PostedTime"].ToString(),
                                Job_CompanyName = reader["Job_CompanyName"].ToString(),
                                Job_Location = reader["Job_Location"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error Job_DataBaseAsync(): {ex.Message}");
                }
            }

            return jobFeedList;
        }
    }
}

