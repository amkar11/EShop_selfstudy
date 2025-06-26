using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Database;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Infrastructure.Repositories
{
    public class ShopCartItemRepository : IShopCartItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ShopCartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductToCartAsync(ShopCartItem item)
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ShopCartItem>> FindAllProductsByCartIdAsync(int cartId)
        {
            return await _context.Products.Where(x => x.cartId == cartId).ToListAsync();
        }

        public async Task<ShopCartItem?> FindProductByIdAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.productId == productId);
        }

        public async Task<ShopCartItem?> FindProductByIdAndCartAsync(int cartId, int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.cartId == cartId && x.productId == productId);
        }

        public async Task RemoveProductFromCartAsync(int productId)
        {
            var item = await FindProductByIdAsync(productId);
            ArgumentNullException.ThrowIfNull(item);
            _context.Products.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ShopCartItem item)
        {
            _context.Products.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
