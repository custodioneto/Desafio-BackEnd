using Dev.Challenge.Application.Repository.Base;
using Dev.Challenge.Domain.Entities;

namespace Dev.Challenge.Application.Repository
{
    public interface IMotorcycleRepository : IBaseRepository<MotorcycleEntity>
    {
        Task<MotorcycleEntity> GetByLicensePlateAsync(string licensePlate);
    }
}
