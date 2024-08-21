using Dev.Challenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Service
{
    public interface IRentalService
    {
        Task RentMotorcycleAsync(RentalEntity rental);
        Task<IEnumerable<RentalEntity>> GetAllRentalsAsync();
        Task<RentalEntity> GetRentalByIdAsync(Guid id);
        Task UpdateRentalAsync(RentalEntity rental);
        Task DeleteRentalAsync(Guid id);
        Task<decimal> CalculateRentalFeeAsync(Guid rentalId, DateTime returnDate);
    }


}
