using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Project_Polished_Version
{

    public partial class SearchJob_Window : Window
    {
        public static List<Jobs> jobFeed = new List<Jobs>();
        int CompanyNumber;
        public SearchJob_Window()
        {
            DataContext = this;
            InitializeComponent();
            PopulateListBox();

            _ = MainWindowMethods.userType == true ?
                Apply_Btn.Visibility = Visibility.Collapsed : Apply_Btn.Visibility = Visibility.Visible;
        }

        private List<Jobs> allJobs = new List<Jobs>();
        private async void PopulateListBox()
        {
            try
            {
                allJobs = await DataStructures.GetJobDataBase();
                JobList.ItemsSource = allJobs; 
                if (allJobs.Count == 0)
                {
                    MessageBox.Show("No jobs found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error populating jobs PopulateListBox(): {ex.Message}");
            }
        }

        private void SearchBox_txtchange(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

            string searchText = SearchBox.Text.ToLower();
            var filteredList = allJobs.Where(c =>
                c.Job_Position.ToLower().Contains(searchText) ||
                c.Job_Description.ToLower().Contains(searchText) || 
                c.Job_CompanyName.ToLower().Contains(searchText)
            ).ToList();

            JobList.ItemsSource = filteredList;
        }

        public static int Company_Number { get; set; }

       
        private void Search_btn(object sender, RoutedEventArgs e)
        {
            if (JobList.SelectedItem is Jobs selectedCompany)
            {
                CompanyNumber = selectedCompany.Company_userNumber;
            }
        }

        private void Back_Button(object sender, RoutedEventArgs e)
        {
            Applicant_DashBoard db = new Applicant_DashBoard();
            this.Close();
            db.Show();
        }

        private void SearchBox_SelectionChange(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (JobList.SelectedItem is Jobs selectedJobs)
            {
                Company_Number = selectedJobs.Company_userNumber;
                JobName_Textblock.Text = selectedJobs.Job_Title;
                Description_TextBox.Text = selectedJobs.Job_Description;
                TimePosted_Textblock.Text = selectedJobs.Job_PostedTime;
                JobSpecify_Textblock.Text = selectedJobs.Job_Specification;
                JobSalary_Textblock.Text = selectedJobs.Job_Salary;
                TimePosted_Textblock.Text = selectedJobs.Job_PostedTime;
                CompanyName_Textblock.Text = selectedJobs.Job_CompanyName;
                JobLocation_Textblock .Text = selectedJobs.Job_Location;
                _CompanyName = selectedJobs.Job_CompanyName;
                _Job_id = selectedJobs.Job_id;
                _CompanyID = selectedJobs.Company_userNumber;
                _Job_Name = selectedJobs.Job_Title;
            }

        }
        private string _CompanyName { get; set; }
        private int _Job_id { get; set; }
        private int _CompanyID { get; set; } 
        private string _Job_Name { get; set; }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Select a PDF Resume"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(selectedFilePath);

                FileInfo fileInfo = new FileInfo(selectedFilePath);
                if (fileInfo.Length > 5 * 1024 * 1024)
                {
                    MessageBox.Show("The selected file is too large. Please upload a file smaller than 5MB.");
                    return;
                }
                try
                {
                    var applicantResume = new Resume()
                    {
                        Resume_pdf = $"\\Resume\\{fileName}",
                        Submission_Date = DateTime.Now.ToString(),
                        Applicant_id = MainWindowMethods.applicantID,
                        CompanyName = _CompanyName,
                        Job_id = _Job_id,
                        CompanyID = _CompanyID,
                        Resume_Job_Position = _Job_Name, 
                        Status = "Pending", 

                    };
                    byte[] pdfData = File.ReadAllBytes(selectedFilePath);

                    using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                    {
                        connection.Open();
                        Global_Repository.AddMultiple_Rows_Columns_DB("resume_database", applicantResume, connection);
                    }

                    MessageBox.Show("Job application submitted successfully!");

                    var jobToRemove = allJobs.FirstOrDefault(j => j.Job_id == _Job_id);
                    if (jobToRemove != null)
                    {
                        allJobs.Remove(jobToRemove);
                        JobList.ItemsSource = null; 
                        JobList.ItemsSource = allJobs; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while submitting your application: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No file selected. Please select a PDF file to upload.");
            }
        }
    }
}

