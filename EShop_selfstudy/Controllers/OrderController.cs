using EShop_selfstudy.Data;
using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EShop_selfstudy.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly IAllOrders _allOrders;
        private readonly ShopCart _shopCart;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IAllOrders allOrders, ShopCart cart, ILogger<OrderController> logger)
        {
            _allOrders = allOrders;
            _shopCart = cart;
            _logger = logger;
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
            _shopCart.listShopItems = await _shopCart.getShopItemsAsync(order.shopCartId);

            if(!_shopCart.listShopItems.Any())
            {

                ModelState.AddModelError("", "You must have items in cart");
            }

            if (ModelState.IsValid) { 
                _allOrders.CreateOrder(order);
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
