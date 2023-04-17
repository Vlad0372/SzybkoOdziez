using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SzybkoOdziez.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public ObservableCollection<Product> Products { get; set; }
    }
}
