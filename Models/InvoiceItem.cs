using Medici.Models.Medici.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medici.Models
{
    public class InvoiceItem : LineItem
    {
        public int ItemID { get; set; }
        public int InvoiceID { get; set; }

        // Constructor calls base constructor
        public InvoiceItem(string description, int quantity, decimal unitPrice)
        {
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }

}
