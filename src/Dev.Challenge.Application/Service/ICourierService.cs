using Dev.Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Service
{
    public interface ICourierService
    {
        Task RegisterCourierAsync(CourierEntity courier);
        Task<CourierEntity> GetCourierByCnpjAsync(string cnpj);
        Task<CourierEntity> GetCourierByDriverLicenseNumberAsync(string driverLicenseNumber);
        Task UpdateCourierAsync(CourierEntity courier);
        Task DeleteCourierAsync(Guid id);
        Task UpdateDriverLicenseImageAsync(Guid id, Stream fileStream, string fileName);
        Task<IEnumerable<CourierEntity>> GetAllCouriersAsync();
    }
}
