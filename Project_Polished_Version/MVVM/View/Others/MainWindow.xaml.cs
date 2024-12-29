
using Project_Polished_Version.Methods;
using System;
using System.Windows;



namespace Project_Polished_Version
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool UserType { get; set; }
        public MainWindow()
        {
            InitializeComponent();
         
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {  
            new Register_Window().Show();
            this.Close();
        }

        private async void Log_In_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PasswordTxtBox.Password) || string.IsNullOrEmpty(UsernameTextBox.Text))
                {
                    MessageBox.Show("Please enter both email and password.");
                    return;
                }
               
                await MainWindowMethods.AuthenticateUserAsync(PasswordTxtBox.Password, UsernameTextBox);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Register_Button_Click_Company(object sender, RoutedEventArgs e)
        {
            Register_Company rc = new Register_Company();
            this.Close();
            rc.Show();
        }

        private void unhashed_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordTxtBox.Visibility == Visibility.Visible)
            {
                Password.Text = PasswordTxtBox.Password;
                Password.Visibility = Visibility.Visible;
                PasswordTxtBox.Visibility = Visibility.Collapsed;

                UnhashedPassword.Content = "Hide Password";
            }
            else
            {
                PasswordTxtBox.Password = Password.Text;
                Password.Visibility = Visibility.Collapsed;
                PasswordTxtBox.Visibility = Visibility.Visible;

                UnhashedPassword.Content = "Show Password";
            }
        }
    }
}

