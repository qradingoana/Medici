using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medici.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CompanyRegNo { get; set; }
        public string VatNo { get; set; }

    }

}
