using AdditionalTools.InMemoryCache;
using AutoMapper;
using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using EShop_selfstudy.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EShop_selfstudy.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAllCars _carRep;
        private readonly ICacheService _cacheService;
        private readonly ILogger<HomeController > _logger;
        string key = "FavouriteCars";

        public HomeController(IAllCars carRep, ICacheService cacheService,
            ILogger<HomeController> logger)
        {
            _carRep = carRep;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<ViewResult> Index()
        {
            var favCars = await _cacheService.GetOrAddValueAsync(key, () => _carRep.getFavCarsAsync());
            var homeCars = new HomeViewModel { favCars = favCars };
            _logger.LogInformation("There will be shown {CarsCount} cars from table {CarsType} with attribute IsFavourite", favCars.Count(), typeof(Car));
            return View(homeCars);
        }
    }
}
