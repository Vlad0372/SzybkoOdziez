using System;
using System.Collections.Generic;
using System.Text;

namespace SzybkoOdziez.Models
{
    public class Product
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImageUrl { get; set; }
        public List<Comment> Comments { get; set; }
        public string Category { get; set; }
        public string Producer { get; set; }
        public string Color { get; set; }
        public string Season { get; set; }
        public string Material { get; set; }
        public string Pattern { get; set; }
        public string Model { get; set; }
        public int Size { get; set; }

    }

}
