using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Layout.Properties;
using iText.IO.Image;
using Medici.Models;
using Medici.Data;

namespace Medici.Services
{
    public class PdfGenerator
    {
        public void GenerateInvoice(string filePath, Invoice invoice, Client client, string headerImagePath)
        {
            // Ensure the PdfWriter is created correctly
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    // Add Header Image
                    if (!string.IsNullOrEmpty(headerImagePath) && File.Exists(headerImagePath))
                    {
                        Image headerImage = new Image(ImageDataFactory.Create(headerImagePath));
                        headerImage.SetAutoScale(true); // Adjust size dynamically
                        document.Add(headerImage);
                    }

                    // Add Title
                    document.Add(new Paragraph("Tax Invoice")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBold()
                        .SetFontSize(20));

                    // Create table for Company and Client Details
                    Table detailsTable = new Table(2); // Two columns for company and client details
                    detailsTable.SetWidth(UnitValue.CreatePercentValue(100)); // Full-width table
                    detailsTable.SetMarginTop(10);

                    // Left column (Company Details)
                    Cell companyDetailsCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.LEFT);
                    string formattedInvoiceNumber = invoice.InvoiceID.ToString().PadLeft(6, '0');
                    companyDetailsCell.Add(new Paragraph($"Invoice Number: {formattedInvoiceNumber}"));
                    companyDetailsCell.Add(new Paragraph($"Issue Date: {invoice.IssueDate:yyyy-MM-dd}"));
                    companyDetailsCell.Add(new Paragraph("VAT No: 4220284485"));
                    companyDetailsCell.Add(new Paragraph("Company Registration No: 2015/242557/07"));
                    detailsTable.AddCell(companyDetailsCell);

                    // Right column (Client Details)
                    Cell clientDetailsCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT);
                    clientDetailsCell.Add(new Paragraph("Invoice to:").SetBold());
                    clientDetailsCell.Add(new Paragraph($"{client.ClientName}\n{client.Email}\n{client.Phone}\n{client.Address}"));
                    clientDetailsCell.Add(new Paragraph($"Client Company Reg No: {client.CompanyRegNo}"));
                    clientDetailsCell.Add(new Paragraph($"Client VAT No: {client.VatNo}"));
                    detailsTable.AddCell(clientDetailsCell);

                    // Add the table to the document
                    document.Add(detailsTable);

                    // Add Reference Info Table
                    document.Add(new Paragraph().SetMarginTop(10)); // Space before next section
                    Table referenceTable = new Table(new float[] { 1, 1, 1 });
                    referenceTable.SetWidth(UnitValue.CreatePercentValue(100));
                    referenceTable.AddHeaderCell("Our Reference No");
                    referenceTable.AddHeaderCell("Your Order No");
                    referenceTable.AddHeaderCell("Terms of Payment");

                    referenceTable.AddCell(invoice.OurRefNo);
                    referenceTable.AddCell(invoice.YourOrderNo);
                    referenceTable.AddCell(invoice.TermsOfPayment);
                    document.Add(referenceTable);

                    // Add Items Table
                    document.Add(new Paragraph().SetMarginTop(10)); // Space before items section
                    Table itemsTable = new Table(new float[] { 1, 3, 1, 1 });
                    itemsTable.SetWidth(UnitValue.CreatePercentValue(100));
                    itemsTable.AddHeaderCell("Quantity");
                    itemsTable.AddHeaderCell("Description");
                    itemsTable.AddHeaderCell("Unit Price");
                    itemsTable.AddHeaderCell("Amount Excl. VAT");

                    decimal subtotal = 0;
                    foreach (var item in invoice.Items)
                    {
                        decimal amountExclVat = item.Quantity * item.UnitPrice;
                        subtotal += amountExclVat;

                        itemsTable.AddCell(item.Quantity.ToString());
                        itemsTable.AddCell(item.Description);
                        itemsTable.AddCell(item.UnitPrice.ToString("C2")); // Format as currency
                        itemsTable.AddCell(amountExclVat.ToString("C2"));  // Format as currency
                    }
                    document.Add(itemsTable);

                    // Calculate VAT (15%)
                    decimal vat = subtotal * 0.15m;
                    document.Add(new Paragraph().SetMarginTop(10));

                    // Add Summary
                    document.Add(new Paragraph($"Subtotal: {subtotal:C2}").SetTextAlignment(TextAlignment.RIGHT));
                    document.Add(new Paragraph($"VAT (15%): {vat:C2}").SetTextAlignment(TextAlignment.RIGHT));
                    document.Add(new Paragraph($"Total Due: {(subtotal + vat):C2}").SetTextAlignment(TextAlignment.RIGHT));

                    // Close the document
                    document.Close();
                }
            }
        }





        public void GenerateQuotation(string filePath, Quotation quotation, Client client, string headerImagePath)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);

                    // Add Header Image
                    if (!string.IsNullOrEmpty(headerImagePath) && File.Exists(headerImagePath))
                    {
                        Image headerImage = new Image(ImageDataFactory.Create(headerImagePath));
                        headerImage.SetAutoScale(true); // Adjust size dynamically
                        document.Add(headerImage);
                    }

                    // Add Title
                    document.Add(new Paragraph("Quotation")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBold()
                        .SetFontSize(20));

                    // Create table for Company and Client Details
                    Table detailsTable = new Table(2); // Two columns for company and client details
                    detailsTable.SetWidth(UnitValue.CreatePercentValue(100)); // Full-width table
                    detailsTable.SetMarginTop(10);

                    // Left column (Company Details)
                    Cell companyDetailsCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.LEFT);
                    string formattedQuotationNumber = quotation.QuotationID.ToString().PadLeft(6, '0');
                    companyDetailsCell.Add(new Paragraph($"Quotation Number: {formattedQuotationNumber}"));
                    companyDetailsCell.Add(new Paragraph($"Issue Date: {quotation.IssueDate:yyyy-MM-dd}"));
                    companyDetailsCell.Add(new Paragraph("VAT No: 4220284485"));
                    companyDetailsCell.Add(new Paragraph("Company Registration No: 2015/242557/07"));
                    detailsTable.AddCell(companyDetailsCell);

                    // Right column (Client Details)
                    Cell clientDetailsCell = new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT);
                    clientDetailsCell.Add(new Paragraph("Quotation to:").SetBold());
                    clientDetailsCell.Add(new Paragraph($"{client.ClientName}\n{client.Email}\n{client.Phone}\n{client.Address}"));
                    clientDetailsCell.Add(new Paragraph($"Client Company Reg No: {client.CompanyRegNo}"));
                    clientDetailsCell.Add(new Paragraph($"Client VAT No: {client.VatNo}"));
                    detailsTable.AddCell(clientDetailsCell);

                    // Add the table to the document
                    document.Add(detailsTable);

                    // Add Reference Info Table
                    document.Add(new Paragraph().SetMarginTop(10)); // Space before next section
                    Table referenceTable = new Table(new float[] { 1, 1, 1 });
                    referenceTable.SetWidth(UnitValue.CreatePercentValue(100));
                    referenceTable.AddHeaderCell("Our Reference No");
                    referenceTable.AddHeaderCell("Your Order No");
                    referenceTable.AddHeaderCell("Terms of Payment");

                    referenceTable.AddCell(quotation.OurRefNo);
                    referenceTable.AddCell(quotation.YourOrderNo);
                    referenceTable.AddCell(quotation.TermsOfPayment);
                    document.Add(referenceTable);

                    // Add Items Table
                    document.Add(new Paragraph().SetMarginTop(10)); // Space before items section
                    Table itemsTable = new Table(new float[] { 1, 3, 1, 1 });
                    itemsTable.SetWidth(UnitValue.CreatePercentValue(100));
                    itemsTable.AddHeaderCell("Quantity");
                    itemsTable.AddHeaderCell("Description");
                    itemsTable.AddHeaderCell("Unit Price");
                    itemsTable.AddHeaderCell("Amount Excl. VAT");

                    decimal subtotal = 0;
                    foreach (var item in quotation.Items)
                    {
                        decimal amountExclVat = item.Quantity * item.UnitPrice;
                        subtotal += amountExclVat;

                        itemsTable.AddCell(item.Quantity.ToString());
                        itemsTable.AddCell(item.Description);
                        itemsTable.AddCell(item.UnitPrice.ToString("C2")); // Format as currency
                        itemsTable.AddCell(amountExclVat.ToString("C2"));  // Format as currency
                    }
                    document.Add(itemsTable);

                    // Calculate VAT (15%)
                    decimal vat = subtotal * 0.15m;
                    document.Add(new Paragraph().SetMarginTop(10));

                    // Add Summary
                    document.Add(new Paragraph($"Subtotal: {subtotal:C2}").SetTextAlignment(TextAlignment.RIGHT));
                    document.Add(new Paragraph($"VAT (15%): {vat:C2}").SetTextAlignment(TextAlignment.RIGHT));
                    document.Add(new Paragraph($"Total Due: {(subtotal + vat):C2}").SetTextAlignment(TextAlignment.RIGHT));

                    // Close the document
                    document.Close();
                }
            }
        }


    }
}
