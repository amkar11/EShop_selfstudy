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
    public class CartService : ICartProductAdder, ICartProductRemover, ICartReader,
        IProductQuantityChanger, ICartCreater, ICartRemover, ICartProductReader, ICartUpdater
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
            var item = await _itemRepository.FindProductByIdAndCartAsync(cartId, itemId);
            item!.quantity += 1;
            await _itemRepository.UpdateProductAsync(item);
        }

        public async Task AddProductToCartAsync(ShopCartItem item)
        {
            await _itemRepository.AddProductToCartAsync(item);
        }

        public async Task CreateCartAsync(int userId)
        {
            var cart = new Cart { userId = userId };
            await _cartRepository.AddCartAsync(cart);
        }

        public async Task DeleteCartAsync(int cartId)
        {
            await _cartRepository.RemoveCartAsync(cartId);
        }

        public async Task<List<Cart>> GetAllCartsAsync(int userId)
        {
            return await _cartRepository.FindAllCartsByUserIdAsync(userId);
        }

        public async Task<List<ShopCartItem>> GetAllItemsFromCartAsync(int cartId)
        {
            return await _itemRepository.FindAllProductsByCartIdAsync(cartId);
        }

        public async Task<Cart?> GetCartAsync(int cartId)
        {
            var cart = await _cartRepository.FindCartByIdAsync(cartId);
            return cart;
        }

        public async Task RemoveProductFromCartAsync(int cartId, int productId)
        {
            var cart = await GetCartAsync(cartId);
            await _itemRepository.RemoveProductFromCartAsync(productId);
        }

        public async Task SubstractProductQuantityAsync(int cartId, int itemId)
        {
            var item = await _itemRepository.FindProductByIdAndCartAsync(cartId, itemId);
            item!.quantity -= 1;
            await _itemRepository.UpdateProductAsync(item);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            await _cartRepository.UpdateCartAsync(cart);
        }
    }
}
