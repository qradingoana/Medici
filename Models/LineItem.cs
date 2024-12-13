using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medici.Models
{
    // LineItem.cs
    namespace Medici.Models
    {
        public class LineItem
        {
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Total => Quantity * UnitPrice; // Calculate total for each line item
        }
    }


}
