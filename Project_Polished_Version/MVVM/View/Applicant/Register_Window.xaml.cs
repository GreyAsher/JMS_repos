using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using System;
using System.IO;
using System.Windows;

namespace Project_Polished_Version
{

    public partial class Register_Window : Window
    {
        FileInfo fi;
        public Register_Window()
        {
            InitializeComponent();

        }
        private int AddAccount()
        {
            int userId = 0;
            try
            {

                string First_Name = First_Name_txtbox.Text;
                string Last_Name = Last_Name_txtBox.Text;
                string Email = emal_txtBox.Text;
                string JobTitle = Job_Position_txtBox.Text;
                string Password = PassWord_txtBox.Password;
                string Gender = Male_Option.IsChecked == true ? "Male" : "Female";
                string Mobile_Number = Mobile_Number_txtBox.Text;
                string Address = Address_TxtBox.Text;


                if (string.IsNullOrWhiteSpace(First_Name) || string.IsNullOrWhiteSpace(Last_Name) ||
                    string.IsNullOrWhiteSpace(JobTitle) || string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Mobile_Number) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Address))
                {
                    MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }

                if (Mobile_Number.Length != 11 || !Mobile_Number.StartsWith("09"))
                {
                    MessageBox.Show("Invalid phone number. Phone number should start with 09 and be 11 digits long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
                if (!Email.ToLower().Contains("@") || !Email.ToLower().Contains("@"))
                {
                    MessageBox.Show("Please enter you yahoo or gmail account", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }

                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                { 
                    connection.Open();

                    var Applicant = new ApplicantUser()
                    {
                        Applicant_First_Name = First_Name_txtbox.Text,
                        Applicant_Last_Name = Last_Name_txtBox.Text,
                        Applicant_About = " ",
                        Applicant_Email = emal_txtBox.Text.ToUpper(), 
                        Applicant_Education = " ",
                        Applicant_Address = Address_TxtBox.Text,
                        Applicant_Experience = " ",
                        Applicant_InfoDatePosted = DateTime.Now.ToString(),
                        Applicant_Gender = Male_Option.IsChecked == true ? "Male" : "Female",
                       Applicant_JobTitle = Job_Position_txtBox.Text, 
                       Applicant_Password = PassWord_txtBox.Password, 
                       Applicant_PhoneNumber = Mobile_Number_txtBox.Text,
                       User_Type = "Applicant",
                       Applicant_Photo = "Images\\Default.jpg"
                    };

                    Global_Repository.Insert_WithoutID("applicant_account", Applicant, connection);

                }

                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mn = new MainWindow();
                this.Close();
                mn.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($" 2 An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return userId;
        }
        private void Next_btn(object sender, RoutedEventArgs e)
        {
            int userId = AddAccount();
       
        }

        private void Cancl_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mn = new MainWindow();
            this.Close();
            mn.Show();
        }
    }
}



