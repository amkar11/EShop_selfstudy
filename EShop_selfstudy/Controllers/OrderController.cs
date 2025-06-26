using System.Security.Claims;
using System.Text;
using System.Text.Json;
using EShop_selfstudy.Data;
using EShop_selfstudy.Data.DTO;
using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop_selfstudy.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IAllOrders _allOrders;
        private readonly ILogger<OrderController> _logger;
        private readonly HttpClient _client;
        public OrderController(IAllOrders allOrders, ILogger<OrderController> logger, IHttpClientFactory client)
        {
            _allOrders = allOrders;
            _logger = logger;
            _client = client.CreateClient("ShoppingCart");
        }

        [HttpGet]
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        [Route("Checkout")]
        public async Task<IActionResult> Checkout(Order order)
        {
            if (ModelState.IsValid) { 
                _allOrders.CreateOrder(order);
                var user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ArgumentNullException.ThrowIfNull(user_id);
                var create_cart_dto = new CreateCartDto
                {
                    userId = Convert.ToInt32(user_id),
                    is_checkout = "true"
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
                Response.Cookies.Append("cartId", cartId);

                return RedirectToAction("Complete");
            }

            return View(order);
        }

        [HttpGet]
        [Route("Complete")]
        public IActionResult Complete()
        {
            ViewBag.Message = "Order is succesfully processed";
            return View();
        }
    }
}
