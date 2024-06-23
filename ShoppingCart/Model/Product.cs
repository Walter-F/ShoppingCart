using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class Product
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool isChecked { get; set; }
    }
}
