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

namespace Medici.Views
{
    /// <summary>
    /// Interaction logic for SignupWindow.xaml
    /// </summary>
    public partial class SignupWindow : UserControl
    {
        public SignupWindow()
        {
            InitializeComponent();
        }

        private void ShowPwdCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SignupPwdTextBox.Text = SignupPwdPasswordBox.Password;
            SignupPwdPasswordBox.Visibility = Visibility.Collapsed;
            SignupPwdTextBox.Visibility = Visibility.Visible;

            ConfirmPwdTextBox.Text = ConfirmPwdPasswordBox.Password;
            ConfirmPwdPasswordBox.Visibility = Visibility.Collapsed;
            ConfirmPwdTextBox.Visibility = Visibility.Visible;
        }

        private void ShowPwdCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SignupPwdPasswordBox.Password = SignupPwdTextBox.Text;
            SignupPwdTextBox.Visibility = Visibility.Collapsed;
            SignupPwdPasswordBox.Visibility = Visibility.Visible;

            ConfirmPwdPasswordBox.Password = ConfirmPwdTextBox.Text;
            ConfirmPwdTextBox.Visibility = Visibility.Collapsed;
            ConfirmPwdPasswordBox.Visibility = Visibility.Visible;
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

            DatabaseHelper dbHelper = DatabaseHelper.GetInstance();
            string username = RegisterUsernameTextBox.Text;
            string password = SignupPwdPasswordBox.Password;
            string confirmPassword = ConfirmPwdPasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isAdmin = AdminUserCheckBox.IsChecked == true;

            try
            {
                // Save the user to the database
                using (SQLiteConnection connection = dbHelper.GetConnection())
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Users (Username, Password, IsAdmin) VALUES (@Username, @Password, @IsAdmin)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password); // Consider hashing passwords for security
                        command.Parameters.AddWithValue("@IsAdmin", isAdmin);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Optionally, navigate to another view or clear the fields

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
