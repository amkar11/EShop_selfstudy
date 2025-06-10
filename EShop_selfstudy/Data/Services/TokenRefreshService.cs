
using System.Text;
using AdditionalTools.InMemoryCache;
using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.Data.Services
{
    public class TokenRefreshService : BackgroundService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string? _refreshToken = string.Empty;
        private string? _accessToken = string.Empty;
        private readonly ILogger<TokenRefreshService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public TokenRefreshService(IHttpClientFactory httpClientFactory, ILogger<TokenRefreshService> logger,
            IServiceProvider serviceProvider, IConfiguration configuration, ICacheService cacheService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _cacheService = cacheService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var cacheService = _serviceProvider.GetRequiredService<ICacheService>();
                ArgumentNullException.ThrowIfNull(cacheService, nameof(cacheService));

                var jwtSettings = _configuration.GetRequiredSection("Jwt");
                int delay = Convert.ToInt32(jwtSettings.GetValue(typeof(int), "ExpiresInMinutes"));

                await Task.Delay(TimeSpan.FromMinutes(delay - 5), stoppingToken);
                var client = _httpClientFactory.CreateClient("AuthClient");

                var refresh = _cacheService.GetRefreshTokenFromCache("refresh_token");
                var access = _cacheService.GetRefreshTokenFromCache("access_token");
                if (refresh is null || access is null)
                {
                    _logger.LogError("Refresh is null {refresh}; acces is null {access}",
                        string.IsNullOrEmpty(refresh), string.IsNullOrEmpty(access));
                }

                var request = new HttpRequestMessage(HttpMethod.Post, "api/Token/refresh");
                request.Headers.Add("Cookie", $"refresh_token={refresh}; access_token={access};");
                request.Content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    TokenPairResponse? json = await response.Content.ReadFromJsonAsync<TokenPairResponse>();

                    if (json == null) throw new InvalidOperationException("Failed to recreate tokens in TokenController!");

                    _accessToken = json.access_token;
                    _refreshToken = json.refresh_token;
                    
                    _logger.LogInformation("Access token refreshed succefully!");
                }
                else
                {
                    _logger.LogError("Failed to refresh tokens!");
                }
            }
        }
    }
}
