using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SzybkoOdziez.Models
{
    public class DeliveryDestination
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Voivodship { get; set; }
        public int UserId { get; set; }
    }
}
