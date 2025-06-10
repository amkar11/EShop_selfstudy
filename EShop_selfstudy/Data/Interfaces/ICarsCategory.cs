using EShop_selfstudy.Data.Models;
namespace EShop_selfstudy.Data.Interfaces
{
    public interface ICarsCategory
    {
        Task<IEnumerable<Category>> getAllCategoriesAsync();
    }
}
