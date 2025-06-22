using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Models

{
    public class Cart
    {
        [Key]
        public int cartId { get; set; }
        public int userId { get; set; }
        public bool IsActive { get; set; } = true;
        public List<ShopCartItem> products { get; set; } = new List<ShopCartItem>();
    }
}
