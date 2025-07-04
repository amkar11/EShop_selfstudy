﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Domain.Database;
using ShoppingCart.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cart>> FindAllCartsByUserIdAsync(int userId)
        {
            var carts = await _context.Carts.Where(x => x.userId == userId).ToListAsync();
            return carts;
        }

        public async Task<Cart?> FindCartByIdAsync(int cartId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.cartId == cartId);
            return cart;
        }

        public async Task RemoveCartAsync(int cartId)
        {
            var cart = await FindCartByIdAsync(cartId);
            ArgumentNullException.ThrowIfNull(cart);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
