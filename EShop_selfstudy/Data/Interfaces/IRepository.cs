using System.Linq.Expressions;

namespace EShop_selfstudy.Data.Interfaces
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>(params Expression<Func<T, object?>>[] includes) where T : class;
        Task<T?> GetByIdAsync<T>(object id, params Expression<Func<T, object?>>[] includes) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(int id) where T : class;

    }
}
