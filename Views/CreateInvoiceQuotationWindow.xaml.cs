using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using Medici.Data;
using Medici.Models;
using System.Collections.ObjectModel;
using Medici.Models.Medici.Models;

namespace Medici.Views
{
    public partial class CreateInvoiceQuotationWindow : UserControl
    {
        public ObservableCollection<LineItem> LineItems { get; set; } = new ObservableCollection<LineItem>();

        public CreateInvoiceQuotationWindow()
        {
            InitializeComponent();
            LoadClientNames();
            DataContext = this;
        }

        private void LoadClientNames()
        {
            var clientData = DatabaseHelper.GetInstance().GetClients();
            SelectClientComboBox.ItemsSource = clientData;
            SelectClientComboBox.DisplayMemberPath = "ClientName";
            SelectClientComboBox.SelectedValuePath = "ClientID";
        }

        private void InvoiceSelectRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DetailsTextBlock.Text = "Invoice Details";
            ItemsTextBlock.Text = "Invoice Items";
            ClearForm();
            long nextInvoiceNumber = GetNextInvoiceOrQuotationId("Invoice");
            InvoiceNumberTextBlock.Text = $"INV-{nextInvoiceNumber:D6}";
        }

        private void QuotationSelectRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DetailsTextBlock.Text = "Quotation Details";
            ItemsTextBlock.Text = "Quotation Items";
            ClearForm();
            long nextQuotationNumber = GetNextInvoiceOrQuotationId("Quotation");
            InvoiceNumberTextBlock.Text = $"QUO-{nextQuotationNumber:D6}";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if client is selected
            if (SelectClientComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a client.");
                return;
            }

            string invoiceType = InvoiceSelectRadioButton.IsChecked == true ? "Invoice" : "Quotation";
            long nextId = GetNextInvoiceOrQuotationId(invoiceType);
            InvoiceNumberTextBlock.Text = nextId.ToString();

            // Gather invoice/quotation data
            int clientID = (int)SelectClientComboBox.SelectedValue;
            DateTime issueDate = InvoiceDatePicker.SelectedDate ?? DateTime.Now;
            DateTime dueDate = DueDatePicker.SelectedDate ?? DateTime.Now;

            // New fields
            string vatNo = VATNoTextBox.Text;
            string regNo = RegNoTextBox.Text;
            string ourRefNo = OurRefNoTextBox.Text;
            string yourOrderNo = YourOrderNoTextBox.Text;
            string termsOfPayment = TermsOfPaymentTextBox.Text;

            decimal totalAmount = CalculateTotalAmount();

            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert header into Invoices or Quotations
                        string insertHeaderQuery = invoiceType == "Invoice"
                            ? @"INSERT INTO Invoices (ClientID, IssueDate, DueDate, TotalAmount, VATNo, RegNo, OurRefNo, YourOrderNo, TermsOfPayment) 
                              VALUES (@ClientID, @IssueDate, @DueDate, @TotalAmount, @VATNo, @RegNo, @OurRefNo, @YourOrderNo, @TermsOfPayment);
                              SELECT last_insert_rowid();"
                            : @"INSERT INTO Quotations (ClientID, IssueDate, DueDate, TotalAmount, VATNo, RegNo, OurRefNo, YourOrderNo, TermsOfPayment) 
                              VALUES (@ClientID, @IssueDate, @DueDate, @TotalAmount, @VATNo, @RegNo, @OurRefNo, @YourOrderNo, @TermsOfPayment);
                              SELECT last_insert_rowid();";

                        long documentId;

                        using (var command = new SQLiteCommand(insertHeaderQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ClientID", clientID);
                            command.Parameters.AddWithValue("@IssueDate", issueDate.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@DueDate", dueDate.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            command.Parameters.AddWithValue("@VATNo", vatNo);
                            command.Parameters.AddWithValue("@RegNo", regNo);
                            command.Parameters.AddWithValue("@OurRefNo", ourRefNo);
                            command.Parameters.AddWithValue("@YourOrderNo", yourOrderNo);
                            command.Parameters.AddWithValue("@TermsOfPayment", termsOfPayment);
                            documentId = (long)command.ExecuteScalar();
                        }

                        // Check if LineItems is initialized and not null
                        if (LineItems == null || LineItems.Count == 0)
                        {
                            MessageBox.Show("No line items to save.");
                            return;
                        }

                        // Insert line items
                        foreach (var item in LineItems)
                        {
                            string insertItemQuery = invoiceType == "Invoice"
                                ? @"INSERT INTO InvoiceItems (InvoiceID, ItemDescription, Quantity, UnitPrice) 
                                  VALUES (@DocumentID, @ItemDescription, @Quantity, @UnitPrice);"
                                : @"INSERT INTO QuotationItems (QuotationID, ItemDescription, Quantity, UnitPrice) 
                                  VALUES (@DocumentID, @ItemDescription, @Quantity, @UnitPrice);";

                            using (var itemCommand = new SQLiteCommand(insertItemQuery, connection))
                            {
                                itemCommand.Parameters.AddWithValue("@DocumentID", documentId);
                                itemCommand.Parameters.AddWithValue("@ItemDescription", item.Description);
                                itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                                itemCommand.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                                itemCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show($"{invoiceType} saved successfully.");
                        InvoiceNumberTextBlock.Text = documentId.ToString();
                        ClearForm(); // Clear the form after saving
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error saving {invoiceType}: {ex.Message}");
                    }
                }
            }
        }

        private decimal CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (var item in LineItems)
            {
                totalAmount += item.Total;
            }
            return totalAmount;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            string itemDescription = ItemDescTextBox.Text;
            int quantity = (int)QuantityUpDown.Value;
            decimal unitPrice;

            if (decimal.TryParse(UnitPriceTextBox.Text, out unitPrice))
            {
                var item = new LineItem
                {
                    Description = itemDescription,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                LineItems.Add(item);
                //Console.WriteLine($"Added item: {item.Description}, Quantity: {item.Quantity}, Unit Price: {item.UnitPrice}");

                UpdateTotalAmount();
                ClearItemInputs();
            }
            else
            {
                MessageBox.Show("Please enter a valid unit price.");
            }
        }

        private void ClearForm()
        {
            InvoiceNumberTextBlock.Text = string.Empty;
            SelectClientComboBox.SelectedIndex = -1;
            InvoiceDatePicker.SelectedDate = DateTime.Now;
            DueDatePicker.SelectedDate = null;
            TotalAmountTextBox.Text = string.Empty;

            // Clear new fields
            //VATNoTextBox.Text = string.Empty;
            //RegNoTextBox.Text = string.Empty;
            OurRefNoTextBox.Text = string.Empty;
            YourOrderNoTextBox.Text = string.Empty;
            TermsOfPaymentTextBox.Text = string.Empty;

            LineItems.Clear();
        }

        private void UpdateTotalAmount()
        {
            decimal subtotal = CalculateTotalAmount();
            decimal vat = subtotal * 0.15m; // 15% VAT
            decimal totalAmount = subtotal + vat;

            SubtotalTextBox.Text = subtotal.ToString("F2");
            VATTextBox.Text = vat.ToString("F2");
            TotalAmountTextBox.Text = totalAmount.ToString("F2");
        }

        private void ClearItemInputs()
        {
            ItemDescTextBox.Text = string.Empty;
            QuantityUpDown.Value = 1;
            UnitPriceTextBox.Text = string.Empty;
        }

        private long GetNextInvoiceOrQuotationId(string invoiceType)
        {
            string query = invoiceType == "Invoice"
                ? "SELECT COALESCE(MAX(InvoiceID), 0) + 1 FROM Invoices"
                : "SELECT COALESCE(MAX(QuotationID), 0) + 1 FROM Quotations";

            using (var connection = DatabaseHelper.GetInstance().GetConnection())
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    return result is long ? (long)result : 1;
                }
            }
        }

        private void DeleteItemsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemDataGrid.SelectedItem as LineItem;

            if (selectedItem != null)
            {
                LineItems.Remove(selectedItem);
                UpdateTotalAmount();
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Any unsaved changes will be lost. Continue?",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                ClearForm();
                ClearItemInputs();
                InvoiceSelectRadioButton.IsChecked = false;
                QuotationSelectRadioButton.IsChecked = false;
            }
        }
    }
}
