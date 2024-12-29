using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using DSA1.Main;

/// <summary>
/// Interaction logic for Company_Check.xaml
/// </summary>
namespace Project_Polished_Version
{
    public partial class Company_Check : Window
    {
        private List<Resume> list;

        public ICommand ViewProfileCommand { get; }

        public Company_Check()
        {
            InitializeComponent();
            ViewProfileCommand = new RelayCommand<ApplicantUser>(ViewProfile);
            DataContext = this;
            LoadApplicants();
        }

        private List<Resume> FetchApplicantsForCompany(int companyId)
        {
            var applicants = new List<Resume>();

            using (var connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM resume_db WHERE CompanyId = @CompanyId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var resume = new Resume
                            {
                                Applicant_id = Convert.ToInt32(reader["Profile_Number"]),
                                CompanyID = Convert.ToInt32(reader["CompanyId"]),
                                Status = reader["Status"].ToString(),
                                Reusme_id = Convert.ToInt32(reader["ResumeNumber"]),
                                Resume_pdf = reader["ResumePDF"].ToString(),
                                Resume_Job_Position = reader["Position"].ToString(),
                                CompanyName = reader["Company_Name"].ToString(),
                                Submission_Date = DateTime.Now.ToString(),
                            };
                            applicants.Add(resume);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception instead of showing a MessageBox
                    Console.WriteLine($"Error fetching applicants: {ex.Message}");
                }
            }

            return applicants;
        }

        private void LoadApplicants()
        {
            list = FetchApplicantsForCompany(Applicant_DashBoard.CurrentUserKey);
            Job_Sent_Table.ItemsSource = list;

            if (list.Count == 0)
            {
                MessageBox.Show("No applicants found for the selected company.");
            }
        }

        private void ViewProfile(ApplicantUser user)
        {
            var profileWindow = new Applicant_Profile(user);
            profileWindow.Show();
        }

        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            var dashboard = new Applicant_DashBoard();
            this.Close();
            dashboard.Show();
        }

        private void Submit_Btn(object sender, RoutedEventArgs e)
        {
            if (Job_Sent_Table.SelectedItem is Resume selectedUser)
            {
                string recipientEmail = selectedUser.CompanyName;
                string subject = "From the hiring team";
                string messageFilePath = "C:\\Users\\Admin\\source\\repos\\NewWin\\UMHire_JSrepos\\UMHire_JSrepos\\Project_Polished_Version\\Company_Message.txt"; 

                try
                {
                    if (string.IsNullOrWhiteSpace(recipientEmail) || string.IsNullOrWhiteSpace(subject))
                    {
                        MessageBox.Show("Please fill in all fields before sending the email.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                   
                    if (!File.Exists(messageFilePath))
                    {
                        MessageBox.Show("Message file not found. Please ensure the file exists at the specified location.", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string message = File.ReadAllText(messageFilePath);

                 
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        MessageBox.Show("The message file is empty. Please provide content in the file.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                 
                    string smtpServer = "smtp.gmail.com";
                    int smtpPort = 587; 
                    string senderEmail = "DaleDonalds31@gmail.com"; 
                    string senderAppPassword = "yohc kvol hqht ubup";

                 
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(recipientEmail);

                    
                    var smptClient = new SmtpClient(smtpServer)
                    {
                        Port = smtpPort,
                        Credentials = new NetworkCredential(senderEmail, senderAppPassword),
                        EnableSsl = true
                    };
                    smptClient.Send(mailMessage);

                    MessageBox.Show("Email sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SmtpException)
                {
                    MessageBox.Show("Error: Please check your email credentials, SMTP configuration, or internet connection.", "SMTP Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Error: Please ensure the recipient's email address is correct.", "Format Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\nPlease check your internet connection or firewall settings.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}

