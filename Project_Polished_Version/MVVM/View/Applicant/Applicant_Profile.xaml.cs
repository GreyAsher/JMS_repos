using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Project_Polished_Version.Methods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Project_Polished_Version.Classes
{
    public partial class Applicant_Profile : Window
    {
        public static int windowNumber { get; set; }
        private string currentUserName { get; set; }
        public static int searchUserKey { get; set; }
       
        public static int searchUserKey_Company { get; set; }
        public static List<int> keys = new List<int>();
        public ApplicantUser CurrentResume { get; private set; }

        public Applicant_Profile()
        {
            InitializeComponent();
            ShowInfo();
            MessageBox.Show($"Show: {Applicant_DashBoard.CurrentUserKey}");
            GetAboutInfo();
            GetExperienceInfo();
            GetEducationInfo();
            LoadProfilePicture();
            Connect_Friend_Btn.Visibility = Visibility.Collapsed;

        }

        public Applicant_Profile(ApplicantUser userInfo) 
        {
            InitializeComponent();
            CurrentResume = userInfo;
            PopulateProfile(userInfo);
            currentUserName = Full_Name.Text; 
            EditProfile_btn.Visibility = Visibility.Collapsed;
            Edit_About_Click.Visibility = Visibility.Collapsed;
            SaveEducation_btn.Visibility= Visibility.Collapsed;
            Edit_Experience_Click.Visibility = Visibility.Collapsed;
            Experience_TextBox.IsEnabled = false;
            About_TextBox.IsEnabled = false;
            Education_TextBox.IsEnabled= false;
        }

        private void Edit_Profile(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a Profile Picture",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                Directory.CreateDirectory(imagesDirectory);


                string newFileName = Path.GetFileName(selectedFilePath);
                string newFilePath = Path.Combine(imagesDirectory, newFileName);
                File.Copy(selectedFilePath, newFilePath, true);

                string relativePath = Path.Combine("Images", newFileName);

                MessageBox.Show("Show: " + relativePath);

                try
                {
                    string query = "UPDATE applicant_account SET Applicant_Photo = @Applicant_Photo WHERE Applicant_Id = @Applicant_Id";
                    using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Applicant_Photo", relativePath);
                            cmd.Parameters.AddWithValue("@Applicant_Id", Applicant_DashBoard.CurrentUserKey);

                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Upload Successfully");

                                BitmapImage bt = new BitmapImage();
                                bt.BeginInit();
                                bt.UriSource = new Uri(newFilePath, UriKind.Absolute);
                                bt.CacheOption = BitmapCacheOption.OnLoad;
                                bt.EndInit();
                                User_Profile.Source = bt;
                            }
                            else
                            {
                                MessageBox.Show("Nothing Happen");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Show: " + ex.Message);
                }
            }
        }

        private void LoadProfilePicture()
        {
               
            try
            {
                string imagePath = MainWindowMethods.ApplicantUser.Applicant_Photo;
                string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagesDirectory, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                User_Profile.Source = bitmap;
                MessageBox.Show("Profile picture loaded successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error LoadProfilePicture(): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopulateProfile(ApplicantUser resume)
        {
            try
            {
                DisableButton();

                Full_Name.Text = $"{resume.Applicant_First_Name} {resume.Applicant_Last_Name}";
                Job_Title_txtbox.Text = resume.Applicant_JobTitle;
                Address_txtbox.Text = resume.Applicant_Address;
                searchUserKey = resume.Id;
                searchUserKey_Company = resume.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error PopulateProfile(ApplicantUser resume): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (windowNumber == 1)
                {
                    Applicant_DashBoard ap = new Applicant_DashBoard();
                    this.Close();
                    ap.Show();
                }
                else if (windowNumber == 2)
                {
                    Applicant_Search2 ap = new Applicant_Search2();
                    this.Close();
                    ap.Show();
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while navigating back: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }
        private void AddFriend_Btn(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    string query = @"INSERT INTO connectionfriend_database (UserIdNumber, UserFriendIDNumber, UserFullName, UserFriendName, User_type,ConnectionStatus)
                 VALUES (@UserId, @FriendId, @UserFullName, @UserFriendName, @UserType,@ConnectionStatus)";


                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", MainWindowMethods.applicantID);
                        cmd.Parameters.AddWithValue("@FriendId", searchUserKey);
                        cmd.Parameters.AddWithValue("@UserFullName",MainWindowMethods.ApplicantUser.Applicant_First_Name + " " + MainWindowMethods.ApplicantUser.Applicant_Last_Name );
                        cmd.Parameters.AddWithValue("@UserFriendName", currentUserName);
                        cmd.Parameters.AddWithValue("@ConnectionStatus", "pending");
                        cmd.Parameters.AddWithValue("@UserType", MainWindowMethods.userType ? "company" : "applicant");

                        int rowsAffected = cmd.ExecuteNonQuery();

                        MessageBox.Show(rowsAffected > 0 ? "Friend request sent successfully!" : "Failed to send friend request.");
                        Connect_Friend_Btn.Content = "Pending";
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error AddFriend_Btn(object sender, RoutedEventArgs e): {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowInfo()
        {
          
            Full_Name.Text = $"{MainWindowMethods.ApplicantUser.Applicant_First_Name} {MainWindowMethods.ApplicantUser.Applicant_Last_Name}";
            Job_Title_txtbox.Text = MainWindowMethods.ApplicantUser.Applicant_JobTitle;
           Address_txtbox.Text = MainWindowMethods.ApplicantUser.Applicant_Address;

        }

        
  
        private void ShowFriendList_btn(object sender, RoutedEventArgs e)
        {
            //if (visitedUser != null) // Assume `visitedUser` is the ApplicantUser object of the visited profile
            //{
            //    Friend_List_Window friendListWindow = new Friend_List_Window(visitedUser);
            //    friendListWindow.Show();
            //}
            //else
            //{
            //    MessageBox.Show("No user profile selected.");
            //}
            Friend_List_Window flw = new Friend_List_Window();
            flw.Show();
        }

        private void DisableButton()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();

                    string query = "SELECT * FROM connectionfriend_database WHERE UserIdNumber = @UserIdNumber " +
                        " AND UserFriendIDNumber = @UserFriendIDNumber ";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserIdNumber", MainWindowMethods.applicantID);
                        cmd.Parameters.AddWithValue("@UserFriendIDNumber", searchUserKey);

                        int friendshipCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (friendshipCount > 0)
                        {
                            Connect_Friend_Btn.IsEnabled = false;
                            Connect_Friend_Btn.Content = "Already Connected";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking friendship: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void GetAboutInfo()
        {
            About_TextBox.Text = await ApplicantProfileMethods.GetSectionDataAsync("About_Post");
        }

        private async void GetEducationInfo()
        {
            Education_TextBox.Text = await ApplicantProfileMethods.GetSectionDataAsync("Education_Post");
        }

        private async void GetExperienceInfo()
        {
            Experience_TextBox.Text = await ApplicantProfileMethods.GetSectionDataAsync("Experience_Post");
        }
        public void ChangeInfo(string name, string jobTitle, string address)
        {
            Full_Name.Text = name;
            Job_Title_txtbox.Text = jobTitle;
            Address_txtbox.Text = address;  
        }

        private void SaveAbout_Click(object sender, RoutedEventArgs e)
        {
            if (Edit_About_Click.Content.ToString() == "Edit")
            {
                About_TextBox.IsReadOnly = false;
                About_TextBox.Focus();
                Edit_About_Click.Content = "Save";
            }
            else if (Edit_About_Click.Content.ToString() == "Save")
            {
                About_TextBox.IsReadOnly = true;
                ApplicantProfileMethods.SaveData(About_TextBox, MainWindowMethods.applicantID, "Applicant_About");
                Edit_About_Click.Content = "Edit";
            }
        
        }

        private void SaveExperience_Click(object sender, RoutedEventArgs e)
        {
            if (Edit_Experience_Click.Content.ToString() == "Edit")
            {
                Experience_TextBox.IsReadOnly = false;
                Experience_TextBox.Focus();
                Edit_Experience_Click.Content = "Save";
            }
            else if (Edit_Experience_Click.Content.ToString() == "Save")
            {
                Edit_Experience_Click.Content = "Edit";
                ApplicantProfileMethods.SaveData(About_TextBox, MainWindowMethods.applicantID, "Applicant_Experience");
                Experience_TextBox.IsReadOnly = true;
            }
        }

        private void SaveEducationClick(object sender, RoutedEventArgs e)
        {
            if (SaveEducation_btn.Content.ToString() == "Edit")
            {
                Education_TextBox.IsReadOnly = false;
                Education_TextBox.Focus();
                Edit_About_Click.Content = "Save";
            }
            else if (SaveEducation_btn.ToString() == "Save")
            {
                SaveEducation_btn.Content = "Edit";
                ApplicantProfileMethods.SaveData(About_TextBox, MainWindowMethods.applicantID, "Applicant_Education");
                Education_TextBox.IsReadOnly = true;
            }
   
        }
    }
}
