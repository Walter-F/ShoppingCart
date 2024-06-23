using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class Purchase
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int PriceTotal { get; set; }
    }
}
