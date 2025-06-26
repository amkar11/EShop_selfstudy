using EShop_selfstudy.Data.DTO;
using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace EShop_selfstudy.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IAllCars _carRep;
        private readonly ILogger<ShopCartController> _logger;
        private readonly HttpClient _client;

        public ShopCartController(IAllCars carRep, ILogger<ShopCartController> logger, IHttpClientFactory client)
        {
            _carRep = carRep;
            _logger = logger;
            _client = client.CreateClient("ShoppingCart");
        }

        public async Task<ViewResult> Index()
        {
            var cartId = Request.Cookies["cartId"];
            ArgumentNullException.ThrowIfNull(cartId);
            try
            {
                var response = await _client.GetAsync($"api/Cart/get-products?cartId={cartId}");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("All items from cart {cartId} were successfully fetched", cartId);
                }
                var json = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<List<ShopCartDto>>(json);
                ArgumentNullException.ThrowIfNull(items);
                List<ShopCartViewModel> items_view_model = new List<ShopCartViewModel>();
                foreach (var item in items)
                {
                    var car = await _carRep.getObjectCarAsync(item.productId);
                    items_view_model.Add(new ShopCartViewModel
                    {
                        name = car.name,
                        shortDesc = car.shortDesc,
                        price = car.price,
                        quantity = item.quantity
                    });
                }
                return View(items_view_model);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            
        }

        public async Task<RedirectToActionResult> AddToCartAsync(int id)
        {
            var cartId = Request.Cookies["cartId"];
            var item = new ShopCartItemDto { cartId = Convert.ToInt32(cartId), productId = id };
            var item_json = JsonSerializer.Serialize(item);
            var content = new StringContent(item_json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _client.PostAsync("api/Cart/add-product", content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Product successfullt added to cart!");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
