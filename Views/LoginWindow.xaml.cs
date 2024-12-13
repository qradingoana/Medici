using Medici.Data;
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
using System.Data.SQLite;
using Medici.Models;

namespace Medici.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Collapsed;
            PasswordTextBox.Visibility = Visibility.Visible;

        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;  // Copy the password from TextBox
            PasswordTextBox.Visibility = Visibility.Collapsed;  // Hide TextBox
            PasswordBox.Visibility = Visibility.Visible;  // Show PasswordBox
        }

        public bool ValidateUser(string username, string password)
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); // Remember to hash passwords in production!

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 1; // Returns true if the user is found
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password; // Using PasswordBox for security

            if (PasswordTextBox.Visibility == Visibility.Visible)
            {
                password = PasswordTextBox.Text;
            }
            else
            {
                password = PasswordBox.Password;
            }

            if (ValidateUser(username, password))
            {
                UserSession.Username = username;
                // Proceed to the next window
                MessageBox.Show("Login successful!");

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                // Show main window or navigate to another appropriate view
                Window startupWindow = Window.GetWindow(this);
                {
                    startupWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }
    }
}
