using Microsoft.AspNetCore.Mvc;

namespace EShop_selfstudy.Controllers
{
    [Route("[controller]")]
    public class LogoutController : Controller
    {
        private readonly HttpClient _client;
        private readonly ILogger<LogoutController> _logger;

        public LogoutController(IHttpClientFactory client, ILogger<LogoutController> logger)
        {
            _client = client.CreateClient("AuthClient");
            _logger = logger;
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            

            var response = await _client.PostAsync("Login/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User successfully logged out!");
                return Redirect("http://localhost:5090/Login/LoginView");
            }
            else
            {
                _logger.LogError("Logout failed, whether something wrong with request to User_EShop or something else went wrong");
                return StatusCode((int)response.StatusCode, "Logout failed");
            }
        }
    }
}
