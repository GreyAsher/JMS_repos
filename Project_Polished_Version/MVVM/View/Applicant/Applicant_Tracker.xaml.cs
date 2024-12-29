using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Applicant_Tracker.xaml
    /// </summary>
    public partial class Applicant_Tracker : Window
    {
        private List<Resume> list;
        public Applicant_Tracker()
        {
            InitializeComponent();
            GetResumeList();
        }


        private void GetResumeList()
        {
            list = new List<Resume>();

            foreach (var x in DataStructures.GetResumeFromDatabase())
            {
                if (x.Applicant_id == MainWindowMethods.applicantID)
                {
                    list.Add(x);
                }
            }

            Job_Sent_Table.ItemsSource = list;

            if (list.Count == 0)
            {
                MessageBox.Show("No data found for the current user.");
            }
        }

        private void RefreshData()
        {
            list.Clear();

            foreach (var x in DataStructures.GetResumeFromDatabase())
            {
                if (x.Applicant_id == MainWindowMethods.ApplicantUser.Id)
                {
                    list.Add(x);
                }
            }

            Job_Sent_Table.ItemsSource = list;
            Job_Sent_Table.Items.Refresh();
        }


        private void RemoveJob_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Resume selectedResume)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
                    {
                        connection.Open();
                        string query = "DELETE FROM resume_database WHERE Reusme_id = @ResumeId";
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ResumeId", selectedResume.Reusme_id);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Job application removed successfully.");
                                list.Remove(selectedResume);
                                RefreshData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to remove the job application.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing job application: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid job application selected.");
            }
        }


        private void Back_Btn_Click(object sender, RoutedEventArgs e)
        {
            Applicant_DashBoard db = new Applicant_DashBoard();
            this.Close();
            db.Show();
        }

    }
}

