using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using Medici.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;


namespace Medici.Data
{
    public class DatabaseHelper
    {
        private static DatabaseHelper instance;
        private string connectionString = @"Data Source=C:\Users\27765\source\repos\Medici\Medici\Resources\medici.db;Version=3;";

        // Private constructor to prevent instantiation
        private DatabaseHelper() { }

        // Singleton instance accessor
        public static DatabaseHelper GetInstance()
        {
            if (instance == null)
            {
                instance = new DatabaseHelper();
            }
            return instance;
        }

        public List<string> GetClientNames()
        {
            List<string> clientNames = new List<string>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT ClientName FROM Clients"; // Adjust this query if needed

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientNames.Add(reader.GetString(0)); // Assuming ClientName is the first column
                        }
                    }
                }
            }

            return clientNames;
        }


        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Initialize Users table
                string createUsersTableQuery = @"CREATE TABLE IF NOT EXISTS Users (
                                                UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                                                Username TEXT NOT NULL UNIQUE,
                                                Password TEXT NOT NULL,
                                                IsAdmin INTEGER NOT NULL DEFAULT 0
                                                )";
                using (var command = new SQLiteCommand(createUsersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Initialize Clients table
                InitializeClientTable(connection);
                // Initialize Invoices, Quotations, and their respective line items
                InitializeInvoiceTable(connection);
                InitializeQuotationTable(connection);
                InitializeInvoiceItemsTable(connection);
                InitializeQuotationItemsTable(connection);
            }
        }

        // Method to initialize the Clients table
        private void InitializeClientTable(SQLiteConnection connection)
        {
            string createClientTableQuery = @"
                CREATE TABLE IF NOT EXISTS Clients (
                    ClientID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ClientName TEXT NOT NULL,
                    Email TEXT,
                    Phone TEXT,
                    Address TEXT,
                    CompanyRegNo TEXT,
                    VatNo TEXT
                )";

            using (var command = new SQLiteCommand(createClientTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void InitializeInvoiceTable(SQLiteConnection connection)
        {
            string createInvoiceTableQuery = @"
    CREATE TABLE IF NOT EXISTS Invoices (
        InvoiceID INTEGER PRIMARY KEY AUTOINCREMENT,
        ClientID INTEGER NOT NULL,
        IssueDate TEXT NOT NULL,
        DueDate TEXT NOT NULL,
        TotalAmount REAL NOT NULL,
        VATNo TEXT,            -- New Field
        RegNo TEXT,           -- New Field
        OurRefNo TEXT,        -- New Field
        YourOrderNo TEXT,     -- New Field
        TermsOfPayment TEXT,   -- New Field
        FOREIGN KEY (ClientID) REFERENCES Clients(ClientID)
    )";

            using (var command = new SQLiteCommand(createInvoiceTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }



        private void InitializeQuotationTable(SQLiteConnection connection)
        {
            string createQuotationTableQuery = @"
    CREATE TABLE IF NOT EXISTS Quotations (
        QuotationID INTEGER PRIMARY KEY AUTOINCREMENT,
        ClientID INTEGER NOT NULL,
        IssueDate TEXT NOT NULL,
        DueDate TEXT NOT NULL,
        TotalAmount REAL NOT NULL,
        VATNo TEXT,            -- New Field
        RegNo TEXT,           -- New Field
        OurRefNo TEXT,        -- New Field
        YourOrderNo TEXT,     -- New Field
        TermsOfPayment TEXT,   -- New Field
        FOREIGN KEY (ClientID) REFERENCES Clients(ClientID)
    )";

            using (var command = new SQLiteCommand(createQuotationTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }



        // Ensure your item tables are properly initialized as well
        private void InitializeInvoiceItemsTable(SQLiteConnection connection)
        {
            string createInvoiceItemsTableQuery = @"
        CREATE TABLE IF NOT EXISTS InvoiceItems (
            ItemID INTEGER PRIMARY KEY AUTOINCREMENT,
            InvoiceID INTEGER NOT NULL,
            ItemDescription TEXT NOT NULL,
            Quantity INTEGER NOT NULL,
            UnitPrice REAL NOT NULL,
            FOREIGN KEY (InvoiceID) REFERENCES Invoices(InvoiceID)
        )";

            using (var command = new SQLiteCommand(createInvoiceItemsTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void InitializeQuotationItemsTable(SQLiteConnection connection)
        {
            string createQuotationItemsTableQuery = @"
        CREATE TABLE IF NOT EXISTS QuotationItems (
            ItemID INTEGER PRIMARY KEY AUTOINCREMENT,
            QuotationID INTEGER NOT NULL,
            ItemDescription TEXT NOT NULL,
            Quantity INTEGER NOT NULL,
            UnitPrice REAL NOT NULL,
            FOREIGN KEY (QuotationID) REFERENCES Quotations(QuotationID)
        )";

            using (var command = new SQLiteCommand(createQuotationItemsTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }


        // Method to insert an invoice
        public void InsertInvoice(Invoice invoice)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string insertInvoiceQuery = @"INSERT INTO Invoices (ClientID, IssueDate, DueDate, TotalAmount, VATNo, RegNo, OurRefNo, YourOrderNo, TermsOfPayment) 
                                  VALUES (@ClientID, @IssueDate, @DueDate, @TotalAmount, @VATNo, @RegNo, @OurRefNo, @YourOrderNo, @TermsOfPayment)";

                using (var command = new SQLiteCommand(insertInvoiceQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientID", invoice.ClientID);
                    command.Parameters.AddWithValue("@IssueDate", invoice.IssueDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@DueDate", invoice.DueDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount);
                    command.Parameters.AddWithValue("@VATNo", invoice.VATNo);
                    command.Parameters.AddWithValue("@RegNo", invoice.RegNo);
                    command.Parameters.AddWithValue("@OurRefNo", invoice.OurRefNo);
                    command.Parameters.AddWithValue("@YourOrderNo", invoice.YourOrderNo);
                    command.Parameters.AddWithValue("@TermsOfPayment", invoice.TermsOfPayment);  // New field
                    command.ExecuteNonQuery();
                }
            }
        }




        // Method to insert a quotation
        public void InsertQuotation(Quotation quotation)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string insertQuotationQuery = @"INSERT INTO Quotations (ClientID, IssueDate, DueDate, TotalAmount, VATNo, RegNo, OurRefNo, YourOrderNo, TermsOfPayment) 
                                    VALUES (@ClientID, @IssueDate, @DueDate, @TotalAmount, @VATNo, @RegNo, @OurRefNo, @YourOrderNo, @TermsOfPayment)";

                using (var command = new SQLiteCommand(insertQuotationQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientID", quotation.ClientID);
                    command.Parameters.AddWithValue("@IssueDate", quotation.IssueDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@DueDate", quotation.DueDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@TotalAmount", quotation.TotalAmount);
                    command.Parameters.AddWithValue("@VATNo", quotation.VATNo);
                    command.Parameters.AddWithValue("@RegNo", quotation.RegNo);
                    command.Parameters.AddWithValue("@OurRefNo", quotation.OurRefNo);
                    command.Parameters.AddWithValue("@YourOrderNo", quotation.YourOrderNo);
                    command.Parameters.AddWithValue("@TermsOfPayment", quotation.TermsOfPayment);  // New field
                    command.ExecuteNonQuery();
                }
            }
        }



        public int GetClientIDByName(string clientName)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT ClientID FROM Clients WHERE ClientName = @ClientName";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientName", clientName);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Client not found.");
                    }
                }
            }
        }

        public List<Client> GetClients()
        {
            var clients = new List<Client>();
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT ClientID, ClientName FROM Clients";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            ClientID = reader.GetInt32(0),
                            ClientName = reader.GetString(1)
                        });
                    }
                }
            }
            return clients;
        }

        public void GeneratePdf(string filePath, Invoice invoice, string headerImagePath)
        {
            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    // Add header image if available
                    if (!string.IsNullOrEmpty(headerImagePath) && File.Exists(headerImagePath))
                    {
                        var imageData = ImageDataFactory.Create(headerImagePath); // Corrected usage
                        var image = new Image(imageData);
                        document.Add(image);
                    }

                    // Add Invoice details
                    document.Add(new Paragraph($"Invoice ID: {invoice.InvoiceID}"));
                    document.Add(new Paragraph($"Client ID: {invoice.ClientID}"));
                    document.Add(new Paragraph($"Issue Date: {invoice.IssueDate:dd-MM-yyyy}"));
                    document.Add(new Paragraph($"Due Date: {invoice.DueDate:dd-MM-yyyy}"));
                    document.Add(new Paragraph($"Total Amount: {invoice.TotalAmount:C}"));

                    // Add Items details
                    document.Add(new Paragraph("Items:"));
                    var table = new Table(new float[] { 2, 1, 1 }); // Adjust the column sizes as needed
                    table.AddHeaderCell("Description");
                    table.AddHeaderCell("Quantity");
                    table.AddHeaderCell("Unit Price");

                    // Loop through each InvoiceItem and add it to the table
                    foreach (var item in invoice.Items)
                    {
                        table.AddCell(item.Description);
                        table.AddCell(item.Quantity.ToString());
                        table.AddCell(item.UnitPrice.ToString("C"));
                    }

                    // Add the table to the document
                    document.Add(table);

                    // Add footer or additional details if needed

                    // Close the document
                    document.Close();
                }
            }
        }




    }
}
