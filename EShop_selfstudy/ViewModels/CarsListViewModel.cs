using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.ViewModels
{
    public class CarsListViewModel
    {
        public IEnumerable<Car> AllCars {  get; set; } = Enumerable.Empty<Car>();
        public string currCategory { get; set; } = default!;
    }
}
