using Microsoft.AspNetCore.Mvc;

namespace User_EShop.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
