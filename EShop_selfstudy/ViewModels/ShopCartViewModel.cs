using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.ViewModels
{
    public class ShopCartViewModel
    {
        public string name { get; set; } = default!;
        public string shortDesc { get; set; } = default!;
        public decimal price { get; set; } = default!;
        public int quantity { get; set; } = default!;
    }
}
