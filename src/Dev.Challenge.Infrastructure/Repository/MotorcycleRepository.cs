using Dev.Challenge.Application.Repository;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Infrastructure.Repository.Base;
using MongoDB.Driver;

namespace Dev.Challenge.Infrastructure.Repository
{
    public class MotorcycleRepository : BaseRepository<MotorcycleEntity>, IMotorcycleRepository
    {
        public MotorcycleRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<MotorcycleEntity> GetByLicensePlateAsync(string licensePlate)
        {
            var filter = Builders<MotorcycleEntity>.Filter.Eq(m => m.LicensePlate, licensePlate);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
