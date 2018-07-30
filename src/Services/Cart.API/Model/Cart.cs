using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cart.API.Model
{
    public class Cart
    {
        [Key]
        public string BuyerId { get; set; }
        public List<CartItem> Items { get; set; }

        public Cart(string cartId)
        {
            BuyerId = cartId;
            Items=new List<CartItem>();
        }
    }
}
