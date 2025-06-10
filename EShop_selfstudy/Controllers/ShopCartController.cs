using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using EShop_selfstudy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EShop_selfstudy.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IAllCars _carRep;
        private readonly ShopCart _shopCart;
        private readonly ILogger<ShopCartController> _logger;

        public ShopCartController(IAllCars carRep, ShopCart shopCart, ILogger<ShopCartController> logger)
        {
            _carRep = carRep;
            _shopCart = shopCart;
            _logger = logger;
        }

        public async Task<ViewResult> Index()
        {
            var items = await _shopCart.getShopItemsAsync(_shopCart.ShopCartId);
            _shopCart.listShopItems = items;

            var obj = new ShopCartViewModel { shopCart = _shopCart };
            return View(obj);
        }

        public async Task<RedirectToActionResult> AddToCartAsync(int id)
        {
            var item = await _carRep.getObjectCarAsync(id);            
            await _shopCart.AddToCartAsync(item);
            return RedirectToAction("Index");
        }
    }
}
