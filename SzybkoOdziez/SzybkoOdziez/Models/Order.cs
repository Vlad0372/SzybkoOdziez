using System;
using System.Collections.Generic;
using System.Text;

namespace SzybkoOdziez.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
