using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Medici.ViewModels
{
    public class InvoiceQuotationViewModel
    {
        public string DocumentType { get; set; } // Invoice or Quotation
        public string InvoiceNumber { get; set; }
        public string ClientName { get; set; }
        public DateTime DateIssued { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

