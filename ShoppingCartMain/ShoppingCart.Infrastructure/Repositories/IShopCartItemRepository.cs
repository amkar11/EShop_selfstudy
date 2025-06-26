using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Infrastructure.Repositories
{
    public interface IShopCartItemRepository
    {
        Task AddProductToCartAsync(ShopCartItem item);
        Task RemoveProductFromCartAsync(int productId);
        Task<ShopCartItem?> FindProductByIdAsync(int productId);
        Task<ShopCartItem?> FindProductByIdAndCartAsync(int cartId, int productId);
        Task<List<ShopCartItem>> FindAllProductsByCartIdAsync(int cartId);

        Task UpdateProductAsync(ShopCartItem item);
    }
}
