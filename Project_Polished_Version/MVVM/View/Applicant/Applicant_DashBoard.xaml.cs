 using Project_Polished_Version.Classes;
using System.Collections.Generic;
using System.Windows;
using Project_Polished_Version.Methods;
namespace Project_Polished_Version
{
    public partial class Applicant_DashBoard : Window
    {
        public List<string> names = new List<string>();
        public static int CurrentUserKey = MainWindowMethods.applicantID;
        public static int WindowTracker;

        public Applicant_DashBoard()
        {
            Applicant_Profile.windowNumber = 1;
            InitializeComponent();


            if (MainWindowMethods.userType == true)
            {
                Pending_Applications_RB.Visibility = Visibility.Collapsed;
            }
            else
            {
                CheckApplicant_RB.Visibility = Visibility.Collapsed;
            }
        }


        private void Search_Jobs_Click(object sender, RoutedEventArgs e)
        {
            SearchJob_Window sjw = new SearchJob_Window();
            this.Close();
            sjw.Show();
        }

        private void Log_Out_Button(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            this.Close();
            loginWindow.Show();
        }

        private void Application_Check_Button(object sender, RoutedEventArgs e)
        {
            Applicant_Search2 ap = new Applicant_Search2();
            this.Close();
            ap.Show();
        }

        private void Profile_Button(object sender, RoutedEventArgs e)
        {
           
            if (MainWindowMethods.userType)
            {
                Company_Profile cp = new Company_Profile();
                this.Close();
                cp.Show();
            }
            else
            {
                Applicant_Profile ap = new Applicant_Profile();
                this.Close();
                ap.Show();
            }
        }

        private void Notification_Button(object sender, RoutedEventArgs e)
        {
            Inbox inbox = new Inbox();
            this.Close();
            inbox.Show();
        }

        private void Company_search_Click(object sender, RoutedEventArgs e)
        {
            Company_Profile.WindowNumber = 1;
            Company_Search cs = new Company_Search();
            this.Close();
            cs.Show();
        }

        private void Applicantsearch_btn(object sender, RoutedEventArgs e)
        {
            Applicant_Search2 applicant_Search = new Applicant_Search2();
            Applicant_Profile.windowNumber = 2;
            this.Close();
            applicant_Search.Show();

        }
     
        private void Pending_Button(object sender, RoutedEventArgs e)
        {
            Applicant_Tracker ap = new Applicant_Tracker();
            this.Close();
            ap.Show();
        }

        private void CheckApplicant_RB_Click(object sender, RoutedEventArgs e)
        {
            Company_Check cc = new Company_Check();
            this.Close();
            cc.Show();
        }

        private void CheckApplicant_RB_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}



