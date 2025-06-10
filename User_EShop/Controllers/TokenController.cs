using System.Security.Claims;
using AdditionalTools.InMemoryCache;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.Database;
using User.Domain.Models;
using User.Domain.Repositories;

namespace User_EShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ApplicationDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public TokenController(IUserRepository repository,
            IRefreshTokenRepository refreshTokenRepository, ApplicationDbContext context,
            IJwtTokenService jwtTokenService)
        {
            _repository = repository;
            _refreshTokenRepository = refreshTokenRepository;
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(Request.Cookies["access_token"]!);
            if (principal == null) return Unauthorized("Invalid access token");

            string? name_identifier = principal.Identity!.Name;
            ArgumentNullException.ThrowIfNullOrEmpty(name_identifier, nameof(name_identifier));

            int user_id = Convert.ToInt32(name_identifier);

            string? refresh_token = Request.Cookies["refresh_token"];
            ArgumentNullException.ThrowIfNullOrEmpty(refresh_token, nameof(refresh_token));

            var refresh_token_db = await _refreshTokenRepository.GetRefreshTokenByRefreshTokenAsync(refresh_token);
            if (refresh_token_db is null) throw new InvalidOperationException($"There is no such refresh token in {nameof(RefreshTokenDb)}");
            if (refresh_token_db.ExpiredAt <= DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            if (refresh_token_db.ExpiredAt - DateTime.UtcNow <= TimeSpan.FromMinutes(5))
            {
                refresh_token_db.isRevoked = true;
                await _refreshTokenRepository.ChangeRefreshTokenAsync(refresh_token_db);
                var generated_token = JwtTokenService.GenerateRefreshToken();
                await _refreshTokenRepository.CreateRefreshTokenAsync(generated_token, user_id);
                HttpContext.Response.Cookies.Append("refresh_token", generated_token);
                refresh_token = generated_token;
            }

            UserDb? user = await _repository.GetUserByIdAsync(user_id);
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            List<string>? roles = principal.Claims
                .Where(x => x.Type == ClaimTypes.Role || x.Type == "role")
                .Select(c => c.Value)
                .ToList();

            var new_access_token = _jwtTokenService.GenerateToken(user_id, roles);
            HttpContext.Response.Cookies.Append("access_token", new_access_token);
            var new_refresh_token = refresh_token;
            HttpContext.Response.Cookies.Append("refresh_token", new_refresh_token);

            return Ok(new TokenResponse { accessToken = new_access_token, refreshToken = new_refresh_token });

        }
    }
}
