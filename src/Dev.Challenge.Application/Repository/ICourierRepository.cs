using Dev.Challenge.Application.Repository.Base;
using Dev.Challenge.Domain.Entities;

namespace Dev.Challenge.Application.Repository
{
    public interface ICourierRepository : IBaseRepository<CourierEntity>
    {
        Task<CourierEntity> GetByCnpjAsync(string cnpj);
        Task<CourierEntity> GetByDriverLicenseNumberAsync(string driverLicenseNumber);
    }
}
