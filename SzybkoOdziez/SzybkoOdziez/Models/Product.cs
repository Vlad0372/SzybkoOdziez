﻿using System;
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
        //TODO: dodac kategorie
    }

}
