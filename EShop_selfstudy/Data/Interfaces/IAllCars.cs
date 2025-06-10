
using EShop_selfstudy.Data.Models;

namespace EShop_selfstudy.Data.Interfaces
{
    public interface IAllCars
    {
        IQueryable<Car> getCars();
        Task<IEnumerable<Car>> getFavCarsAsync();
        Task<Car> getObjectCarAsync(int carId);
    }
}
