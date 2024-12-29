using MySql.Data.MySqlClient;
using Project_Polished_Version.Classes;
using Project_Polished_Version.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project_Polished_Version.Methods
{
    public class FriendListMethods
    {

        public static void GetDataByStatus(string statusFilter, ObservableCollection<Friend> ApList)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                string query = @"SELECT * from connectionfriend_database where UserIdNumber = @UserIdNumber AND ConnectionStatus = @ConnectionStatus";
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserIdNumber", MainWindowMethods.applicantID);
                    command.Parameters.AddWithValue("@ConnectionStatus", statusFilter);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Friend f = new Friend()
                            {
                                UserFriendIDNumber = Convert.ToInt32(reader["UserFriendIDNumber"].ToString()),
                                ConnectionStatus = reader["ConnectionStatus"].ToString(),
                                UserFriendName = reader["UserFriendName"].ToString(),
                                UserFullName = reader["UserFullName"].ToString(),
                                UserIdNumber = Convert.ToInt32(reader["UserFriendIDNumber"].ToString()),
                                UserType = reader["User_type"].ToString()
                            };
                            ApList.Add(f);
                        }
                    }
                }
            }
        }

        public static void GetFriends(ObservableCollection<Friend> ApList,int user)
        {
            ApList.Clear();

        
            using (MySqlConnection connection = new MySqlConnection(ConnectionClass.ConnectionString()))
            {
                string query = @"SELECT * from connectionfriend_database where UserIdNumber = @UserIdNumber";
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserIdNumber", user);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Friend f = new Friend()
                            {
                                UserFriendIDNumber = Convert.ToInt32(reader["UserFriendIDNumber"].ToString()),
                                ConnectionStatus = reader["ConnectionStatus"].ToString(),
                                UserFriendName = reader["UserFriendName"].ToString(),
                                UserFullName = reader["UserFullName"].ToString(),
                                UserIdNumber = Convert.ToInt32(reader["UserFriendIDNumber"].ToString()),
                                UserType = reader["User_type"].ToString()
                            };
                            ApList.Add(f);
                        }
                    }
                }
            }
         }
    }
}

