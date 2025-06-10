using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task CreateRefreshTokenAsync(string token, int userId);

        Task<List<RefreshTokenDb>?> GetRefreshTokensByUserIdAsync(int userId);

        Task<RefreshTokenDb?> GetRefreshTokenByRefreshTokenAsync(string token);

        Task RevokeRefreshTokenByRefreshTokenAsync(string token);

        Task RevokeRefreshTokensByUserIdAsync(int userId);

        Task DeleteRefreshTokensByUserIdAsync(int userId);

        Task DeleteRefreshTokenByRefreshTokenAsync(string token);
        Task ChangeRefreshTokenAsync(RefreshTokenDb refreshToken);
    }
}
