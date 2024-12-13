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
using System.Text.RegularExpressions;

namespace Medici.Views
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : UserControl
    {
        public ClientWindow()
        {
            InitializeComponent();
            //LoadClientList(); // Load client names into ComboBox
        }

        private void AddNewClientRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectClientComboBox.IsEnabled = false;
            //ConfirmButton.IsEnabled = false;
            ClearFields();
            SelectClientComboBox.Items.Clear();
            //LoadClientList();
        }

        private void EditClientRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectClientComboBox.IsEnabled = true;
            //ConfirmButton.IsEnabled = true;
            //ClearFields();
            SelectClientComboBox.Items.Clear();
            LoadClientList();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Get input values
            string clientName = ClientNameTextBox.Text;
            string email = ClientEmailTextBox.Text;
            string phone = ClientPhoneTextBox.Text;
            string address = new TextRange(ClientAddressRichTextBox.Document.ContentStart, ClientAddressRichTextBox.Document.ContentEnd).Text;
            address = address.Trim();
            // Remove multiple consecutive spaces, newlines, or tabs by replacing them with a single space
            //address = Regex.Replace(address, @"\s+", " ");
            string companyRegNo = ClientRegNoTextBox.Text;
            string vatNo = ClientVatNoTextBox.Text;

            // Validate required fields
            if (string.IsNullOrWhiteSpace(clientName))
            {
                MessageBox.Show("Client Name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Phone number is required.");
                return;
            }

            // Proceed with saving to the database
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query;
                if (EditClientRadioButton.IsChecked == true)
                {
                    // Update existing client
                    query = @"UPDATE Clients 
                      SET Email = @Email, Phone = @Phone, Address = @Address, CompanyRegNo = @CompanyRegNo, VatNo = @VatNo
                      WHERE ClientName = @ClientName";
                }
                else
                {
                    // Insert new client
                    query = @"INSERT INTO Clients (ClientName, Email, Phone, Address, CompanyRegNo, VatNo) 
                      VALUES (@ClientName, @Email, @Phone, @Address, @CompanyRegNo, @VatNo)";
                }

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientName", clientName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@CompanyRegNo", companyRegNo);
                    command.Parameters.AddWithValue("@VatNo", vatNo);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Client information saved successfully!");
            ClearFields();
        }



        private void ClearFields()
        {
            ClientNameTextBox.Clear();
            ClientEmailTextBox.Clear();
            ClientPhoneTextBox.Clear();
            ClientAddressRichTextBox.Document.Blocks.Clear();
            ClientRegNoTextBox.Clear();
            ClientVatNoTextBox.Clear();
            //SelectClientComboBox.SelectedIndex = 0;
        }

        private void LoadClientList()
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();
                string query = "SELECT ClientName FROM Clients";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectClientComboBox.Items.Add(reader["ClientName"].ToString());
                        }
                    }
                }
            }
        }


        // Load client details into the input fields
        private void LoadClientDetails(string clientName)
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Clients WHERE ClientName = @ClientName"; // Ensure the table name is correct
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientName", clientName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Log available columns
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader.GetName(i)); // You can use a logging mechanism instead
                            }

                            // Assuming these are the correct columns in your Clients table
                            ClientNameTextBox.Text = reader["ClientName"]?.ToString() ?? string.Empty; // Use null-coalescing operator for safety
                            ClientEmailTextBox.Text = reader["Email"]?.ToString() ?? string.Empty;
                            ClientPhoneTextBox.Text = reader["Phone"]?.ToString() ?? string.Empty;
                            ClientAddressRichTextBox.Document.Blocks.Clear();
                            ClientAddressRichTextBox.Document.Blocks.Add(new Paragraph(new Run(reader["Address"]?.ToString() ?? string.Empty)));
                            ClientRegNoTextBox.Text = reader["CompanyRegNo"]?.ToString() ?? string.Empty;
                            ClientVatNoTextBox.Text = reader["VatNo"]?.ToString() ?? string.Empty;
                        }
                        else
                        {
                            // Handle the case where no client was found
                            MessageBox.Show("Client not found.");
                        }
                    }
                }
            }
        }


        private void SelectClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectClientComboBox.SelectedItem != null)
            {
                string selectedClient = SelectClientComboBox.SelectedItem.ToString();
                LoadClientDetails(selectedClient); // Load the selected client's details
            }
        }
    }
}
