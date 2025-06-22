using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Application.Services
{
    public class CartService : ICartAdder, ICartRemover, ICartReader, IProductQuantityChanger
    {
        private readonly ICartRepository _cartRepository;
        private readonly IShopCartItemRepository _itemRepository;

        public CartService(ICartRepository cartRepository, IShopCartItemRepository itemRepository)
        {
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
        }

        public async Task AddProductQuantityAsync(int cartId, int itemId)
        {
            var item = await _itemRepository.FindProductByIdAndCartAsync(itemId, itemId);
            item.quantity += 1;
            await _itemRepository.UpdateProductAsync(item);
        }

        public async Task AddProductToCartAsync(int cartId, ShopCartItem item)
        {
            var cart = await GetCartAsync(cartId);
            await _itemRepository.AddProductToCartAsync(item);
        }

        public async Task<List<Cart>> GetAllCartsAsync(int userId)
        {
            return await _cartRepository.FindAllCartsByUserIdAsync(userId);
        }

        public async Task<Cart> GetCartAsync(int cartId)
        {
            return await _cartRepository.FindCartByIdAsync(cartId);
        }

        public async Task RemoveProductFromCartAsync(int cartId, int productId)
        {
            var cart = await GetCartAsync(cartId);
            await _itemRepository.RemoveProductFromCartAsync(productId);
        }

        public async Task SubstractProductQuantityAsync(int cartId, int itemId)
        {
            var item = await _itemRepository.FindProductByIdAndCartAsync(cartId, itemId);
            item.quantity -= 1;
            await _itemRepository.UpdateProductAsync(item);
        }
    }
}
