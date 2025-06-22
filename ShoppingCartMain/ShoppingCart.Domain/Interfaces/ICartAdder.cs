using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICartAdder
    {
        Task AddProductToCartAsync(int cartId, ShopCartItem product);
    }
}
