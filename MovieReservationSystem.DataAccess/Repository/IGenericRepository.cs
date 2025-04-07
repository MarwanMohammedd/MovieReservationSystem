using System.Collections.Generic;
using System.Linq.Expressions;

namespace MovieReservationSystem.DataAccess.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        
        Task<bool> Delete(Expression<Func<TEntity,bool>>Predicate);
        Task<bool> UpdateAsync(Expression<Func<TEntity,bool>>Predicate, TEntity entity);
        Task<TEntity?> GetItemAsync(Expression<Func<TEntity, bool>> expression, string? Selector = null);
        Task<IEnumerable<TEntity>> GetAllAsync(string? Selector = null);
        Task<IEnumerable<TEntity>> ReadAllAsync(string? Selector = null);
    }
}