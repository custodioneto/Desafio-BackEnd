using Dev.Challenge.Domain.Contracts;
using System.Linq.Expressions;

namespace Dev.Challenge.Application.Repository.Base
{
    public interface IBaseRepository<TEntity> where TEntity :  IEntity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filterExpression);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
