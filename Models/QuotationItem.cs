using Medici.Models.Medici.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medici.Models
{
    public class QuotationItem : LineItem
    {
        public int ItemID { get; set; }
        public int QuotationID { get; set; }

        // Constructor calls base constructor
        public QuotationItem(string description, int quantity, decimal unitPrice)
        {
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}

