using Dev.Challenge.Application.Repository;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Infrastructure.Repository.Base;
using MongoDB.Driver;

namespace Dev.Challenge.Infrastructure.Repository
{
    public class RentalRepository : BaseRepository<RentalEntity>, IRentalRepository
    {
        public RentalRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
