using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.DTO;
using User.Domain.Repositories;
using LoginRequest = User.Domain.Models.LoginRequest;
namespace User_EShop.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly ILogger<LoginController> _logger;
        private readonly IUserRepository _repository;
        private readonly IJwtTokenValidator _tokenValidator;
        private readonly HttpClient _client;

        public LoginController(ILoginService loginService, ILogger<LoginController> logger,
            IUserRepository repository, IJwtTokenValidator tokenValidator, IHttpClientFactory client)
        {
            _loginService = loginService;
            _logger = logger;
            _repository = repository;
            _tokenValidator = tokenValidator;
            _client = client.CreateClient("ShoppingCart");
        }

        [HttpGet]
        [Route("LoginView")]
        public IActionResult LoginView()
        {
            string? validate_token = Request.Cookies["access_token"];

            _logger.LogInformation("\"Remember me\" access token exists: {token}", !string.IsNullOrEmpty(validate_token));

            
            bool isRemembered = false;

            if (!string.IsNullOrEmpty(validate_token))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(validate_token);
                string? isRemembered_string = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
                if (isRemembered_string != null)
                {
                    isRemembered = Convert.ToBoolean(isRemembered_string);
                }
            }

            if (!string.IsNullOrEmpty(validate_token) && _tokenValidator.IsTokenValid(validate_token) && isRemembered)
            {
                _logger.LogInformation("Succefull validation, user is remembered");
                return Redirect("http://localhost:5062/");
            }

            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return View("LoginView", loginRequest);
            }

            var user = await _repository.GetUserByNameAndPasswordAsync(loginRequest.Username, loginRequest.Password);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "There is no such user registered");
                return View("LoginView", loginRequest);
            }

            var access_token = await _loginService.LoginAsync(loginRequest.Username, loginRequest.Password, loginRequest.RememberMe);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };


            if (loginRequest.RememberMe)
            {
                cookieOptions.Expires = DateTime.UtcNow.AddDays(14);
            }
            if (Request.Cookies["cartId"] is null)
            {
                var create_cart_dto = new CreateCartDto
                {
                    userId = user.Id,
                    is_checkout = null
                };
                var json = JsonSerializer.Serialize(create_cart_dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/Cart/create-cart", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to access \"create-cart\" method in Cart controller");
                    throw new InvalidOperationException("Failed to access \"create-cart\" method in Cart controller");
                }
                var cartId = await response.Content.ReadAsStringAsync();
                Response.Cookies.Append("cartId", cartId, cookieOptions);
            }

            Response.Cookies.Append("access_token", access_token, cookieOptions);

            _logger.LogInformation("User {Username} was succefully logged in", loginRequest.Username);
            return Redirect("http://localhost:5062/");
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("Session was cleared");

            foreach (var cookie in HttpContext.Request.Cookies.Keys)
            {
                HttpContext.Response.Cookies.Delete(cookie);
            }
            _logger.LogInformation("Cookies deleted");

            _logger.LogInformation("Session and cookie succefully deleted, refresh token is revoked");
            return Ok("Session and cookie succefully deleted, refresh token is revoked");
        }

        [HttpGet("admin")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminPage()
        {
            return Ok("Dane tylko dla administratora");
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult ForEveryAuthorizedPage()
        {
            return Ok("Dane dla wszystkich autoryzowanych");
        }
    }
}
