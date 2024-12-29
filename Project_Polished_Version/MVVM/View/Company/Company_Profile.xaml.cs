using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Project_Polished_Version
{
    public partial class Company_Profile : Window
    {
        public static int WindowNumber;

        public Company_Profile(CompanyUser company)
        {
            InitializeComponent();
            Company_Name.Text = company.CompanyName;
            Company_Email.Text = company.CompanyEmail;
            Company_About_Btn.Visibility = Visibility.Collapsed;
        }

        public Company_Profile()
        {
            InitializeComponent();
            if (MainWindowMethods.companyID <= 0)
            {
                MessageBox.Show("Invalid company ID. Cannot load profile.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            About_TextBox.Text = CompanyProfileMethods.GetAboutData();
            Company_Name.Text = MainWindowMethods.CompanyUser.CompanyName;
            Company_Email.Text = MainWindowMethods.CompanyUser.CompanyEmail;
        }
     
        private void CompanyAbout(object sender, RoutedEventArgs e)
        {
            if (Company_About_Btn.Content.ToString() == "Edit")
            {
                About_TextBox.IsReadOnly = false;
                About_TextBox.Focus();
                Company_About_Btn.Content = "Save";
            }
            else if (Company_About_Btn.Content.ToString() == "Save")
            {
                Company_About_Btn.Content = "Edit";
                CompanyProfileMethods.SaveData(About_TextBox, "Company");
                About_TextBox.IsReadOnly = true;
            }
        }

        private void BackButton_Btn(object sender, RoutedEventArgs e)
        {
            Applicant_DashBoard ad = new Applicant_DashBoard();
            this.Close();
            ad.Show();
        }

        private void Create_Jobs_Btn(object sender, RoutedEventArgs e)
        {
            Create_Jobs_Window cjw = new Create_Jobs_Window();
            cjw.Show();

        }
    }
}
