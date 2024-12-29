using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project_Polished_Version
{
    /// <summary>
    /// Interaction logic for Company_Search.xaml
    /// </summary>
    public partial class Company_Search : Window
    {
        private List<CompanyUser> allCompanies = new List<CompanyUser>();
        public Company_Search()
        {
            InitializeComponent();
            PopulateListBox();
        }
        private void PopulateListBox()
        {
            try
            {
                allCompanies = DataStructures.GetCompanyDataBase();
                CompanyList.ItemsSource = allCompanies;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error populating the list: " + ex.Message);
            }
        }
        //para sa search bar
        private void SearchBox_txtchange(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            string searchText = SearchBox.Text.ToLower();
            var filteredList = allCompanies.Where(c =>
                c.CompanyName.ToLower().Contains(searchText) ||
                c.CompanyEmail.ToLower().Contains(searchText) ||
                c.CompanyAddress.ToLower().Contains(searchText)
            ).ToList();

            CompanyList.ItemsSource = filteredList;
        }

        CompanyUser TheSelectedCompany;
        private void Company_Profile(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CompanyList.SelectedItem is CompanyUser selectedCompany)
            {
                TheSelectedCompany = selectedCompany;
            }
        }

        private void viewProfile_btn(object sender, RoutedEventArgs e)
        {
            if (CompanyList.SelectedItem is CompanyUser TheSelectedCompany)
            {
                Company_Profile cp = new Company_Profile(TheSelectedCompany);
                this.Close();
                cp.Show();
            }
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Applicant_DashBoard cdb = new Applicant_DashBoard();
            this.Close();
            cdb.Show();
        }

        //ma change ang list listbox when typed
        private void CompanyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompanyList.SelectedItem is CompanyUser cu)
            {
                CompanyNameTextBlock.Text = cu.CompanyName;
                LocationName_Textblock.Text = cu.CompanyAddress;
                EmailAdress_Textblock.Text = cu.CompanyEmail;
                AboutCompany_Textbox.Text = cu.Company_About;
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string photoPath = System.IO.Path.Combine(basePath, cu.Photo);

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(photoPath, UriKind.Absolute);
                bitmap.EndInit();

                Company_Image.Source = bitmap;
            }
        }

        private void CompanySearch_BackButton(object sender, RoutedEventArgs e)
        {
            
            Applicant_DashBoard cdb = new Applicant_DashBoard();
            cdb.Show();
            this.Close();
        }
    }
}
