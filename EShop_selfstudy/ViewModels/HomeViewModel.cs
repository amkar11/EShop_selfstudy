using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Car> favCars { get; set; } = Enumerable.Empty<Car>();
    }
}
