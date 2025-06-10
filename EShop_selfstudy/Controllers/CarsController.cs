using AutoMapper;
using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using EShop_selfstudy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop_selfstudy.Controllers
{
    public class CarsController : Controller
    {
        private readonly IAllCars _allCars;
        private readonly ICarsCategory _allCategories;
        private readonly ILogger<CarsController> _logger;
        public CarsController(IAllCars iAllCars, ICarsCategory iCarsCat, ILogger<CarsController> logger)
        {
            _allCars = iAllCars;
            _allCategories = iCarsCat;
            _logger = logger;
        }
        [Route("Cars/List")]
        [Route("Cars/List/{category}")]
        public async Task<ViewResult> List(string category)
        {

            string _category = category;
            IEnumerable<Car> cars = Enumerable.Empty<Car>();
            string currCategory = string.Empty;
            if (string.IsNullOrEmpty(_category))
            {
                cars = await _allCars.getCars().OrderBy(i => i.carId).ToListAsync();
                currCategory = "AllCars";
                _logger.LogInformation("{CarsCount} cars were fetched from table {CarTable}", cars.Count(), typeof(Car).Name);
            }
            else
            {
                if (string.Equals("electro", _category, StringComparison.OrdinalIgnoreCase))
                {
                    cars = await _allCars.getCars().Where(i => i.category != null && i.category.categoryName.Equals("Electrocars")).OrderBy(i => i.carId).ToListAsync();
                    if (!cars.Any())
                    {
                        _logger.LogError("There are no {CarType} with such category {Category}", typeof(Car).Name, _category);
                        throw new InvalidOperationException($"There are no {typeof(Car)} with such category {_category}");
                    }
                    currCategory = "Electrocars";
                }
                else if (string.Equals("fuel", _category, StringComparison.OrdinalIgnoreCase))
                {
                    cars = await _allCars.getCars().Where(i => i.category != null && i.category.categoryName.Equals("Classic cars")).OrderBy(i => i.carId).ToListAsync();
                    if (!cars.Any())
                    {
                        _logger.LogError("There are no {CarType} with such category {Category}", typeof(Car).Name, _category);
                        throw new InvalidOperationException($"There are no {typeof(Car)} with such category {_category}");
                    }
                    currCategory = "Classic cars";
                }

            }
            var carObj = new CarsListViewModel
            {
                AllCars = cars,
                currCategory = currCategory
            };
            ViewBag.Title = "Page with cars";
            _logger.LogInformation("There will be shown {CarsCount} cars from table {CarsType} with category {Category}", cars.Count(), typeof(Car), currCategory);
            return View(carObj);
            
        }
    }
}
