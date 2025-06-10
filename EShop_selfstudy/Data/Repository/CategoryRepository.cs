using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop_selfstudy.Data.Repository
{
    public class CategoryRepository : ICarsCategory
    {
        private readonly IRepository _repository;

        public CategoryRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> getAllCategoriesAsync()
        {
            return await _repository.GetAll<Category>(x => x.Cars).ToListAsync();
        }
    }
}
