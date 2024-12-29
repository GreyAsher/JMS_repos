using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;
using Project_Polished_Version.Classes;
using Project_Polished_Version.Methods;
using Project_Polished_Version.MVVM.Model;

namespace Project_Polished_Version
{
    public partial class Friend_List_Window : Window
    {
        public static ObservableCollection<Friend> UserFriendList { get; set; } = new ObservableCollection<Friend>();
        private static ObservableCollection<Friend> AllUserFriendList { get; set; } = new ObservableCollection<Friend>();
        public static List<int> ConnectList { get; set; } = new List<int>();

        private Friend FriendObject { get; set; }

        public Friend_List_Window()
        {
            InitializeComponent();
            DataContext = this;
            FriendListMethods.GetFriends(UserFriendList, MainWindowMethods.applicantID);

        }

        public Friend_List_Window(ApplicantUser user)
        {
            InitializeComponent();
            DataContext = this;
            FriendListMethods.GetFriends(UserFriendList, user.Id);
            DataContext = this;

            if (user != null)
            {
                FriendListMethods.GetFriends(UserFriendList, user.Id); 
            }
            else
            {
                FriendListMethods.GetFriends(UserFriendList, MainWindowMethods.applicantID); 
            }
        }


        private void All_Friends(object sender, RoutedEventArgs e)
        {
            try
            {
                UserFriendList.Clear();
                FriendListMethods.GetDataByStatus("pending", UserFriendList);
                FriendListMethods.GetDataByStatus("accepted", AllUserFriendList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching all friends: {ex.Message}");
            }
        }

        private void Pending_Friends(object sender, RoutedEventArgs e)
        {
            try
            {
                UserFriendList.Clear();
                FriendListMethods.GetDataByStatus("pending", UserFriendList);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching pending friends: {ex.Message}");
            }
        }

        private void Accepted_Friends(object sender, RoutedEventArgs e)
        {
            try
            {
                UserFriendList.Clear();
                FriendListMethods.GetDataByStatus("accepted", UserFriendList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching accepted friends: {ex.Message}");
            }
        }


        private void Friend_Action(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                string query = @"UPDATE connectionfriend_database  
                                 SET status = 'Accepted' 
                                 WHERE (UserIdNumber  = @CurrentUserId AND UserFriendIDNumber  = @FriendId)";

                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrentUserId", MainWindowMethods.applicantID);
                    command.Parameters.AddWithValue("@FriendId", FriendObject.UserFriendIDNumber);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        FriendObject.ConnectionStatus = "accepted";
                        MessageBox.Show($"Friend request from {FriendObject.UserFriendName} accepted.");
                    }
                }
            }
        }

        private async Task RemoveFriendAsync(Friend user)
        {
            using (var connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                string query = @"DELETE FROM connectionfriend_database 
                         WHERE (UserIdNumber = @CurrentUserId AND UserFriendIDNumber = @FriendId) 
                         OR (UserIdNumber = @FriendId AND UserFriendIDNumber = @CurrentUserId)";

                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrentUserId", MainWindowMethods.applicantID);
                    command.Parameters.AddWithValue("@FriendId", user.UserFriendIDNumber);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        UserFriendList.Remove(user);
                        MessageBox.Show($"You have unfriended {user.UserFriendName}.");
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DisableButtonForSpecificItem(object dataItem)
        {
            var container = FriendList_ListBox.ItemContainerGenerator.ContainerFromItem(dataItem) as ListViewItem;
            if (container != null)
            {
                var rejectButton = container.ContentTemplate.FindName("Reject_Btn", container) as Button;

                if (rejectButton != null)
                {
                    rejectButton.IsEnabled = false;
                }
            }
        }

        private void SelectedFriend(object sender, SelectionChangedEventArgs e)
        {

            if (FriendList_ListBox.SelectedItem is Friend selectedUser)
            {
                FriendObject = new Friend()
                {
                    UserFriendIDNumber = selectedUser.UserFriendIDNumber,
                    ConnectionStatus = selectedUser.ConnectionStatus,
                    UserFullName = selectedUser.UserFullName,
                    UserFriendName = selectedUser.UserFriendName,
                    UserIdNumber = selectedUser.UserIdNumber,
                    UserType = selectedUser.UserType,
                };
            }
        }
    }
}
