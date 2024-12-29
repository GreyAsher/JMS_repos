using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class Inbox : Window
    {
        private List<Email> emaiList = DataStructures.EmailList;
        public Inbox()
        {
            InitializeComponent();
         DataStructures.GetEmailFromDataBase();
            LoadEmails(emaiList);
        }
        private void LoadEmails(List<Email> emailsToDisplay)
        {
            MessagesListView.Items.Clear();

            foreach (var email in emailsToDisplay)
            {
                ListViewItem item = new ListViewItem();
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

                CheckBox checkBox = new CheckBox { VerticalAlignment = VerticalAlignment.Center };
                TextBlock senderText = new TextBlock { Text = email.Sender, FontWeight = FontWeights.Bold, Margin = new Thickness(5, 0, 0, 0) };
                TextBlock subjectText = new TextBlock { Text = email.Subject, Margin = new Thickness(5, 0, 0, 0) };

                stackPanel.Children.Add(checkBox);
                stackPanel.Children.Add(senderText);
                stackPanel.Children.Add(subjectText);

                item.Content = stackPanel;
                item.MouseLeftButtonUp += Email_Selected;

                MessagesListView.Items.Add(item);
            }
        }


        private void Email_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is ListViewItem item && item.Content is StackPanel stackPanel)
            {
                var index = MessagesListView.Items.IndexOf(item);
                if (index >= 0 && index < emaiList.Count)
                {
                    var email = emaiList[index];
                    if (MessageContentTextBlock != null)
                    {
                        MessageContentTextBlock.Text = email.Content;
                    }
                }
            }
        }

        // Search Button Click Event Handler
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.ToLower();
            var filteredEmails = emaiList.Where(email =>
                email.Sender.ToLower().StartsWith(searchTerm) ||
                email.Subject.ToLower().StartsWith(searchTerm) ||
                email.Content.ToLower().StartsWith(searchTerm)).ToList();

            LoadEmails(filteredEmails);
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Applicant_DashBoard ad = new Applicant_DashBoard();
            this.Close();
            ad.Show();
        }

        private void Compose_Email_Click(object sender, RoutedEventArgs e)
        {
            ComposeEmail ce = new ComposeEmail();
            ce.Show();
        }
    }
}

