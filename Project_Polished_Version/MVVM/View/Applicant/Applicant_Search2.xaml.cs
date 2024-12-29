using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Project_Polished_Version
{
    public partial class Applicant_Search2 : Window
    {
        private List<ApplicantUser> applicantCollections = new List<ApplicantUser>();
        public static int WindowTracker;
        public static int As_WindowTracker;
        public Applicant_Search2()
        {
            InitializeComponent();
                PopulateListBox();
        }

        private void PopulateListBox()
        {
            try
            {
                applicantCollections = DataStructures.GetApplicantData();
                ApplicantList.ItemsSource = applicantCollections;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error populating the list: " + ex.Message);

            }
        }

        public static int applicantNumber { get; set; }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
         Applicant_DashBoard db = new Applicant_DashBoard();
         this.Close();
         db.Show();
        }

        private void SearchBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (ApplicantList.SelectedItem == null)
                return;
            if (ApplicantList.SelectedItem is ApplicantUser au)
            {
                Applicant_FullName.Text = au.Applicant_First_Name + au.Applicant_Last_Name;
                Applicant_Location.Text = au.Applicant_Address;
                Applicant_Email.Text = au.Applicant_Email;
                Applicant_Contanct.Text = au.Applicant_PhoneNumber;
                Applicant_Tp.Text = "Just Now";
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string photoPath = System.IO.Path.Combine(basePath, au.Applicant_Photo);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(photoPath, UriKind.Absolute);
                bitmap.EndInit();

                ApplicantImage.Source = bitmap;
            }
        }

        //search bar function
        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            var filteredList = applicantCollections.Where(c =>
                c.Applicant_First_Name.ToLower().Contains(searchText) ||
                c.Applicant_Last_Name.ToLower().Contains(searchText)
            ).ToList();

            ApplicantList.ItemsSource = filteredList;
        }

        private void ViewProfileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ApplicantList.SelectedItem is ApplicantUser selectedUser)
                {
                    applicantNumber = selectedUser.Id;
                   
                    Applicant_Profile userProfile = new Applicant_Profile(selectedUser);
                    userProfile.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select a valid user from the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}\nStackTrace: {ex.StackTrace}", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
