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
using Medici.ViewModels;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using Microsoft.Win32;
using System.IO;
using Medici.Models;
using Medici.Services;
using Medici.Models.Medici.Models;
using Org.BouncyCastle.Crypto;  // Standard Bouncy Castle
using Org.BouncyCastle.Security; // For any security-related functions



namespace Medici.Views
{
    public partial class ViewInvoiceQuotationWindow : UserControl
    {
        public ViewInvoiceQuotationWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadInvoicesAndQuotations();
        }

        private void LoadInvoicesAndQuotations()
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            var documents = new List<InvoiceQuotationViewModel>();

            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = @"
                SELECT 'Invoice' AS DocumentType, printf('INV-%06d', InvoiceID) AS DocumentNumber, 
                       Clients.ClientName, Invoices.IssueDate, Invoices.TotalAmount
                FROM Invoices
                JOIN Clients ON Invoices.ClientID = Clients.ClientID
                WHERE (@StartDate IS NULL OR Invoices.IssueDate >= @StartDate) 
                  AND (@EndDate IS NULL OR Invoices.IssueDate <= @EndDate)
                  
                UNION ALL

                SELECT 'Quotation' AS DocumentType, printf('QUO-%06d', QuotationID) AS DocumentNumber, 
                       Clients.ClientName, Quotations.IssueDate, Quotations.TotalAmount
                FROM Quotations
                JOIN Clients ON Quotations.ClientID = Clients.ClientID
                WHERE (@StartDate IS NULL OR Quotations.IssueDate >= @StartDate) 
                  AND (@EndDate IS NULL OR Quotations.IssueDate <= @EndDate)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", endDate ?? (object)DBNull.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            documents.Add(new InvoiceQuotationViewModel
                            {
                                DocumentType = reader["DocumentType"].ToString(),
                                InvoiceNumber = reader["DocumentNumber"].ToString(),
                                ClientName = reader["ClientName"].ToString(),
                                DateIssued = Convert.ToDateTime(reader["IssueDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                            });
                        }
                    }
                }
            }

            InvoicesDataGrid.ItemsSource = documents;
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDocument = InvoicesDataGrid.SelectedItem as InvoiceQuotationViewModel;

            if (selectedDocument != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"{selectedDocument.DocumentType}_{selectedDocument.InvoiceNumber}.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        // Show the loading cursor
                        Mouse.OverrideCursor = Cursors.Wait;

                        if (selectedDocument.DocumentType == "Invoice")
                        {
                            Invoice invoice = GetInvoiceById(selectedDocument.InvoiceNumber);
                            Client client = GetClientById(invoice.ClientID);

                            string headerImagePath = HeaderImagePathTextBox.Text;

                            PdfGenerator pdfGenerator = new PdfGenerator();
                            pdfGenerator.GenerateInvoice(filePath, invoice, client, headerImagePath);
                            MessageBox.Show($"Invoice PDF generated successfully at {filePath}");
                        }
                        else if (selectedDocument.DocumentType == "Quotation")
                        {
                            Quotation quotation = GetQuotationById(selectedDocument.InvoiceNumber);
                            Client client = GetClientById(quotation.ClientID);

                            string headerImagePath = HeaderImagePathTextBox.Text;

                            PdfGenerator pdfGenerator = new PdfGenerator();
                            pdfGenerator.GenerateQuotation(filePath, quotation, client, headerImagePath);
                            MessageBox.Show($"Quotation PDF generated successfully at {filePath}");
                        }
                    }
                    finally
                    {
                        // Always reset the cursor back to default
                        Mouse.OverrideCursor = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a document to print.");
            }
        }


        private Invoice GetInvoiceById(string invoiceNumber)
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Invoices WHERE InvoiceID = @InvoiceID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InvoiceID", int.Parse(invoiceNumber.Substring(4)));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Invoice
                            {
                                InvoiceID = reader.GetInt32(reader.GetOrdinal("InvoiceID")),
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                OurRefNo = reader.GetString(reader.GetOrdinal("OurRefNo")),
                                YourOrderNo = reader.GetString(reader.GetOrdinal("YourOrderNo")),
                                TermsOfPayment = reader.GetString(reader.GetOrdinal("TermsOfPayment")),
                                Items = GetInvoiceItems(reader.GetInt32(reader.GetOrdinal("InvoiceID"))) // Ensure items are populated
                            };
                        }
                    }
                }
            }

            return null;
        }


        private Quotation GetQuotationById(string quotationNumber)
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Quotations WHERE QuotationID = @QuotationID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuotationID", int.Parse(quotationNumber.Substring(4)));

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Quotation
                            {
                                QuotationID = reader.GetInt32(reader.GetOrdinal("QuotationID")),
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                IssueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                OurRefNo = reader.GetString(reader.GetOrdinal("OurRefNo")),
                                YourOrderNo = reader.GetString(reader.GetOrdinal("YourOrderNo")),
                                TermsOfPayment = reader.GetString(reader.GetOrdinal("TermsOfPayment")),
                                Items = GetQuotationItems(reader.GetInt32(reader.GetOrdinal("QuotationID")))
                            };
                        }
                    }
                }
            }

            return null;
        }

        private Client GetClientById(int clientId)
        {
            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Clients WHERE ClientID = @ClientID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientID", clientId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Client
                            {
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                ClientName = reader.GetString(reader.GetOrdinal("ClientName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                CompanyRegNo = reader.GetString(reader.GetOrdinal("CompanyRegNo")),
                                VatNo = reader.GetString(reader.GetOrdinal("VatNo")),
                            };
                        }
                    }
                }
            }

            return null;
        }

        private List<InvoiceItem> GetInvoiceItems(int invoiceId)
        {
            var items = new List<InvoiceItem>(); // Changed from LineItem to InvoiceItem

            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT ItemDescription, Quantity, UnitPrice FROM InvoiceItems WHERE InvoiceID = @InvoiceID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InvoiceID", invoiceId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string description = reader.IsDBNull(reader.GetOrdinal("ItemDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("ItemDescription"));
                            int quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantity"));
                            decimal unitPrice = reader.IsDBNull(reader.GetOrdinal("UnitPrice")) ? 0m : reader.GetDecimal(reader.GetOrdinal("UnitPrice"));

                            items.Add(new InvoiceItem(description, quantity, unitPrice)); // Changed to add InvoiceItem
                        }
                    }
                }
            }

            // Debugging output to check the count of retrieved items
            //Console.WriteLine($"Retrieved {items.Count} items for Invoice ID {invoiceId}");

            return items; // Returning List<InvoiceItem>
        }





        private List<QuotationItem> GetQuotationItems(int quotationId)
        {
            var items = new List<QuotationItem>(); // Changed from LineItem to QuotationItem

            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();

                string query = "SELECT ItemDescription, Quantity, UnitPrice FROM QuotationItems WHERE QuotationID = @QuotationID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuotationID", quotationId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string description = reader.IsDBNull(reader.GetOrdinal("ItemDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("ItemDescription"));
                            int quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantity"));
                            decimal unitPrice = reader.IsDBNull(reader.GetOrdinal("UnitPrice")) ? 0m : reader.GetDecimal(reader.GetOrdinal("UnitPrice"));

                            items.Add(new QuotationItem(description, quantity, unitPrice)); // Changed to add QuotationItem
                        }
                    }
                }
            }

            // Debugging output to check the count of retrieved items
            //Console.WriteLine($"Retrieved {items.Count} items for Quotation ID {quotationId}");

            return items; // Returning List<QuotationItem>
        }





        private void UploadHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select a Header Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the file path and display the image
                string imagePath = openFileDialog.FileName;
                HeaderImagePathTextBox.Text = imagePath;

                // Load the image into the Image control
                HeaderImage.Source = new BitmapImage(new Uri(imagePath));
            }
        }

    }
}
