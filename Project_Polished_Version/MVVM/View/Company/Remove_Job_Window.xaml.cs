using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Linq;
using System.Windows;

namespace Project_Polished_Version
{
    public partial class Remove_Job_Window : Window
    {
        public Remove_Job_Window()
        {
            InitializeComponent();
            PopulateJobListBox();
        }

        private void PopulateJobListBox()
        {
            // Filter jobs by company ID
            var companyJobs = SearchJob_Window.jobFeed
                .Where(job => job.Company_userNumber == MainWindowMethods.companyID)
                .ToList();

            Jobs_ListView.ItemsSource = companyJobs;

            if (!companyJobs.Any())
            {
                MessageBox.Show("No jobs available for this company.");
            }
        }

        private void RemoveJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (Jobs_ListView.SelectedItem is Jobs selectedJob)
            {
                if (MessageBox.Show($"Are you sure you want to remove the job: {selectedJob.Job_Position}?",
                                    "Confirm Removal", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    RemoveJobFromDatabase(selectedJob);
                }
            }
            else
            {
                MessageBox.Show("Please select a job to remove.");
            }
        }

        private void RemoveJobFromDatabase(Jobs selectedJob)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                {
                    connection.Open();
                    string query = "DELETE FROM jobs_db WHERE Job_id = @Job_id";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@JobID", selectedJob.Job_id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Job '{selectedJob.Job_Position}' removed successfully!");
                            Global_Repository.JobFeed.Remove(selectedJob); 
                            PopulateJobListBox(); 
                        }
                        else
                        {
                            MessageBox.Show("Failed to remove the job. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
