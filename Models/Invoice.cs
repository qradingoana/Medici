using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Medici.Models;
using Medici.Models.Medici.Models;

namespace Medici.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int ClientID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceItem> Items { get; set; }

        // New fields
        public string VATNo { get; set; }
        public string RegNo { get; set; }
        public string OurRefNo { get; set; }
        public string YourOrderNo { get; set; }
        public string TermsOfPayment { get; set; }  // New field
    }
}
