using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models
{
    public class ShopCartItem
    {
        [Key]
        public Guid shopCartItemId { get; set; }
        public int productId { get; set; }
        public int cartId { get; set; }
        public int quantity { get; set; } = 1;
        public Cart cart { get; set; } = default!;
    }
}
