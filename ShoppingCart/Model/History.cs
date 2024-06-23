using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Model
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Purchase> Purchases { get; set; }
        public int TotalPrice { get; set; }
    }
}
