using Dev.Challenge.Application.Repository;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Infrastructure.Repository.Base;
using MongoDB.Driver;

namespace Dev.Challenge.Infrastructure.Repository
{
    public class CourierRepository : BaseRepository<CourierEntity>, ICourierRepository
    {
        public CourierRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<CourierEntity> GetByCnpjAsync(string cnpj)
        {
            var filter = Builders<CourierEntity>.Filter.Eq(c => c.Cnpj, cnpj);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<CourierEntity> GetByDriverLicenseNumberAsync(string driverLicenseNumber)
        {
            var filter = Builders<CourierEntity>.Filter.Eq(c => c.DriverLicenseNumber, driverLicenseNumber);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
