using Dev.Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Service
{
    public interface IMotorcycleService
    {
        Task RegisterMotorcycleAsync(MotorcycleEntity motorcycle);
        Task<IEnumerable<MotorcycleEntity>> GetAllMotorcyclesAsync();
        Task<MotorcycleEntity> GetMotorcycleByLicensePlateAsync(string licensePlate);
        Task<MotorcycleEntity> GetMotorcycleByIdAsync(Guid id);
        Task UpdateMotorcycleLicensePlateAsync(Guid id, string newLicensePlate);
        Task DeleteMotorcycleAsync(Guid id);
    }

}
