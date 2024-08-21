using Dev.Challenge.Application.Repository.Base;
using Dev.Challenge.Domain.Contracts;
using MongoDB.Driver;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using Dev.Challenge.Infrastructure.Extensions;

namespace Dev.Challenge.Infrastructure.Repository.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        protected IMongoCollection<TEntity> Collection => _collection;

        protected BaseRepository(IMongoDatabase database)
        {
            string collectionName = typeof(TEntity).Name.Replace("Entity",string.Empty).Pluralize();
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await _collection.AsQueryable().Where(filterExpression).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }
    }
}
