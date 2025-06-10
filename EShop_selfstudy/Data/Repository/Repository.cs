using System.Linq.Expressions;
using EShop_selfstudy.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EShop_selfstudy.Data.Repository
{
    public class Repository : IRepository
    {
        public readonly AppDBContext _context;
        public Repository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll<T>(params Expression<Func<T, object?>>[] includes) where T : class
        {
            var queries = _context.Set<T>().AsNoTracking();
            foreach (var include in includes) {
                queries = queries.Include(include);
            }
            return queries;
        }

        public async Task<T?> GetByIdAsync<T>(object id, params Expression<Func<T, object?>>[] includes) where T : class
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            if (entityType == null) {
                throw new InvalidOperationException($"Entity type {typeof(T).Name} not found in the current DbContext");
            }

            var keyName = entityType.FindPrimaryKey()?.Properties.Select(p => p.Name).SingleOrDefault();
            if (keyName == null)
            {
                throw new InvalidOperationException($"Primary key not found for {typeof(T).Name}");
            }

            var query = _context.Set<T>().AsNoTracking().Where(e => EF.Property<object>(e, keyName).Equals(id));

            foreach (var include in includes) { 
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(int id) where T : class
        {
            T? entity = await GetByIdAsync<T>(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            else throw new NullReferenceException($"There is no {typeof(T).Name} with such id {id} in database!");
        }
    }
}
