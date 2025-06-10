using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop_selfstudy.Data.Repository
{
    public class CarRepository : IAllCars
    {
        private readonly IRepository _repository;

        public CarRepository(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Car> getCars()
        {
            return _repository.GetAll<Car>();
        }

        public async Task<IEnumerable<Car>> getFavCarsAsync()
        {
            return await _repository.GetAll<Car>(x => x.category).Where(x => x.isFavourite).ToListAsync();
        }

        public async Task<Car> getObjectCarAsync(int carId)
        {
            var car = await _repository.GetByIdAsync<Car>(carId);
            if (car == null) throw new KeyNotFoundException($"There is no {typeof(Car).Name} with such id {carId}");
            return car;
        }
    }
}
