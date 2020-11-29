using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> CartList { get; set; }
        public OrderHeader OrderHeader{ get; set; }

    }
}
