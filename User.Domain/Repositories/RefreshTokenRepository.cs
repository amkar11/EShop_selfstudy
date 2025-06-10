using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.Domain.Database;
using User.Domain.Models;

namespace User.Domain.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ChangeRefreshTokenAsync(RefreshTokenDb refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRefreshTokenAsync(string token, int userId)
        {
            var refresh_token = new RefreshTokenDb
            {
                RefreshToken = token,
                userId = userId
            };

            await _context.RefreshTokens.AddAsync(refresh_token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRefreshTokenByRefreshTokenAsync(string token)
        {
            var refresh_token = await GetRefreshTokenByRefreshTokenAsync(token);
            ArgumentNullException.ThrowIfNull(refresh_token, nameof(refresh_token));
            _context.RefreshTokens.Remove(refresh_token);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRefreshTokensByUserIdAsync(int userId)
        {
            var refresh_tokens = await GetRefreshTokensByUserIdAsync(userId);
            ArgumentNullException.ThrowIfNull(refresh_tokens, nameof(refresh_tokens));

            _context.RefreshTokens.RemoveRange(refresh_tokens);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshTokenDb?> GetRefreshTokenByRefreshTokenAsync(string token)
        {
            var refresh_token = await _context.RefreshTokens.FindAsync(token);
            return refresh_token;
        }

        public async Task<List<RefreshTokenDb>?> GetRefreshTokensByUserIdAsync(int userId)
        {
            List<RefreshTokenDb>? refresh_tokens = await _context.RefreshTokens.Where(x => x.userId == userId).ToListAsync();
            return refresh_tokens;
        }

        public async Task RevokeRefreshTokenByRefreshTokenAsync(string token)
        {
            RefreshTokenDb? refresh_token = await GetRefreshTokenByRefreshTokenAsync(token);
            ArgumentNullException.ThrowIfNull(refresh_token, nameof(refresh_token));

            refresh_token.isRevoked = true;

            await _context.SaveChangesAsync();
        }

        public async Task RevokeRefreshTokensByUserIdAsync(int userId)
        {
            var refresh_tokens = await GetRefreshTokensByUserIdAsync(userId);
            ArgumentNullException.ThrowIfNull(refresh_tokens, nameof(refresh_tokens));

            foreach (var token in refresh_tokens){
                token.isRevoked = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
