using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Infrastructure.Repositories
{
    public interface ICartRepository
    {
        Task AddCartAsync(int userId, Cart cart);
        Task RemoveCartAsync(int cartId);
        Task<Cart> FindCartByIdAsync(int cartId);
        Task<List<Cart>> FindAllCartsByUserIdAsync(int userId);
    }
}
