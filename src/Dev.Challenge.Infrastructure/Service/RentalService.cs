using Dev.Challenge.Application.Repository;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;
using Dev.Challenge.Infrastructure.Exceptions;

namespace Dev.Challenge.Infrastructure.Service
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public RentalService(IRentalRepository rentalRepository, ICourierRepository courierRepository, IMotorcycleRepository motorcycleRepository)
        {
            _rentalRepository = rentalRepository;
            _courierRepository = courierRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task RentMotorcycleAsync(RentalEntity rental)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(rental.MotorcycleId);
            if (motorcycle == null)
                throw new ServiceException("Moto não encontrada.");

            var courier = await _courierRepository.GetByIdAsync(rental.CourierId);
            if (courier == null || courier.DriverLicenseType != DriverLicenseType.A)
                throw new ServiceException("Entregador não habilitado ou não possui CNH tipo A.");

            // Verifica se a data de início é o primeiro dia após a data de criação
            if (rental.StartDate.Date != DateTime.Now.Date.AddDays(1))
                throw new ServiceException("A data de início deve ser o primeiro dia após a data de criação.");

            await _rentalRepository.AddAsync(rental);
        }

        public async Task<IEnumerable<RentalEntity>> GetAllRentalsAsync()
        {
            return await _rentalRepository.GetAllAsync();
        }

        public async Task<RentalEntity> GetRentalByIdAsync(Guid id)
        {
            return await _rentalRepository.GetByIdAsync(id);
        }

        public async Task UpdateRentalAsync(RentalEntity rental)
        {
            await _rentalRepository.UpdateAsync(rental);
        }

        public async Task DeleteRentalAsync(Guid id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            if (rental != null)
            {
                await _rentalRepository.DeleteAsync(rental);
            }
        }

        public async Task<decimal> CalculateRentalFeeAsync(Guid rentalId, DateTime returnDate)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null)
                throw new ServiceException("Aluguel não encontrado.");

            rental.CalculateRentalFee(returnDate);

            await _rentalRepository.UpdateAsync(rental);
            return rental.TotalAmount;
        }
    }

}
