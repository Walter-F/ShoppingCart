using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShoppingCart.Model
{
    public class PurchaseUIElements
    {
        public TextBlock NameTextBlock { get; set; }
        public TextBox QuantityTextBox { get; set; }
        public TextBlock PriceTextBlock { get; set; }
        public TextBlock PriceDetailTextBlock { get; set; }
        public TextBlock TotalPriceTextBlock {  get; set; }
        public TextBlock TotalPriceDetailTextBlock { get; set; }
    }
}
