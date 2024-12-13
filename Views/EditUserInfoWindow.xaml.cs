using Medici.Data;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Medici.Views
{
    public partial class EditUserInfoWindow : UserControl
    {
        private string currentUsername;

        public EditUserInfoWindow(string username)
        {
            InitializeComponent();
            currentUsername = username;
            LoadUserData(currentUsername);
        }

        private void LoadUserData(string username)
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();
                string query = "SELECT Username, Password FROM Users WHERE Username = @Username";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EditUsernameTextBox.Text = reader["Username"].ToString();
                            EnterUserPasswordTextBox.Text = reader["Password"].ToString();
                        }
                    }
                }
            }
        }

        private void SaveUserInfoButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = EnterUserPasswordTextBox.Text;
            string confirmedPassword = ConfirmUserPasswordTextBox.Text;

            if (newPassword == confirmedPassword)
            {
                using (var connection = DatabaseHelper.GetInstance().GetConnection())
                {
                    connection.Open();
                    string updateQuery = "UPDATE Users SET Password = @Password WHERE Username = @Username";
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Password", newPassword);
                        command.Parameters.AddWithValue("@Username", currentUsername);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User information updated successfully!");
            }
            else
            {
                MessageBox.Show("Passwords do not match!");
            }
        }
    }
}
