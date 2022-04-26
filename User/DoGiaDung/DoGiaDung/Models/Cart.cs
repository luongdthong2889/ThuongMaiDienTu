using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoGiaDung.Models
{
    public class Cart
    {
        public PRODUCT Product { get; set; }
        public int Quantity { get; set; }
        public Cart(PRODUCT product, int quantity) 
        {
            Product = product;
            Quantity = quantity;
        }
    }
}