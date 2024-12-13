using Medici.Models;
using Medici.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Medici
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserTextBlock.Text = "Welcome, " + UserSession.Username;
            
        }

        private void NewInvoiceQuotationButton_Click(object sender, RoutedEventArgs e)
        {
            var createInvoiceQuotationWindow = new CreateInvoiceQuotationWindow();

            MainContentControl.Content = createInvoiceQuotationWindow;
        }

        private void AddEditClientButton_Click(object sender, RoutedEventArgs e)
        {
            var clientWindow = new ClientWindow();

            MainContentControl.Content = clientWindow;
        }

        private void EditUserButton_Click(Object sender, RoutedEventArgs e)
        {
            string loggedInUsername = UserSession.Username;
            var editUserInfoWindow = new EditUserInfoWindow(loggedInUsername);

            MainContentControl.Content = editUserInfoWindow;
        }



        private void LogoutButton_Click(Object sender, RoutedEventArgs e)
        {
            // Confirm if the user wants to log out
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Close the current window (MainWindow)
                

                // Redirect to Startup Window
                StartupWindow startupWindow = new StartupWindow();
                startupWindow.Show();

                this.Close();
            }
            // If the result is "No", simply return to MainWindow.
        }

        private void ViewDocListButton_Click(object sender, RoutedEventArgs e)
        {
            var viewInvoiceQuotationWindow = new ViewInvoiceQuotationWindow();
            MainContentControl.Content = viewInvoiceQuotationWindow;
        }
    }
}