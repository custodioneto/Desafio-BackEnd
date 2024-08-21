using Dev.Challenge.Application.Event;
using Dev.Challenge.Application.Queue;
using Dev.Challenge.Application.Repository;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Infrastructure.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Infrastructure.Service
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository, IRabbitMQService rabbitMQService,
            IConfiguration configuration)
        {
            _motorcycleRepository = motorcycleRepository;
            _rabbitMQService = rabbitMQService;
            _configuration = configuration;
        }

        private async Task Validate(MotorcycleEntity motorcycle)
        {
            var motorcyleWithSamePlate = await _motorcycleRepository.GetByLicensePlateAsync(motorcycle.LicensePlate);

            if (motorcyleWithSamePlate != null && motorcycle.Id != motorcyleWithSamePlate.Id) 
            {

                throw new ServiceException("Já existe uma moto com a mesma placa");
            }
        }

        public async Task RegisterMotorcycleAsync(MotorcycleEntity motorcycle)
        {
            await Validate(motorcycle);

            await _motorcycleRepository.AddAsync(motorcycle);

            _rabbitMQService.Publish(new MotorcycleRegisteredEvent
            {
                MotorcycleId = motorcycle.Id,
                Year = motorcycle.Year,
                Model = motorcycle.Model,
                LicensePlate = motorcycle.LicensePlate
            }, _configuration["Queue"]);
        }

        public async Task<IEnumerable<MotorcycleEntity>> GetAllMotorcyclesAsync()
        {
            return await _motorcycleRepository.GetAllAsync();
        }

        public async Task<MotorcycleEntity> GetMotorcycleByLicensePlateAsync(string licensePlate)
        {
            return await _motorcycleRepository.GetByLicensePlateAsync(licensePlate);
        }

        public async Task UpdateMotorcycleLicensePlateAsync(Guid id, string newLicensePlate)
        {
            var existingMotorcycle = await _motorcycleRepository.GetByIdAsync(id);

            if (existingMotorcycle == null) throw new Exception("Registro inválido");

            await Validate(existingMotorcycle);

            existingMotorcycle.UpdateLicensePlate(newLicensePlate);

            await _motorcycleRepository.UpdateAsync(existingMotorcycle);
        }

        public async Task DeleteMotorcycleAsync(Guid id)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(id);
            if (motorcycle != null)
            {
                await _motorcycleRepository.DeleteAsync(motorcycle);
            }
        }

        public async Task<MotorcycleEntity> GetMotorcycleByIdAsync(Guid id)
        {
            return await _motorcycleRepository.GetByIdAsync(id);
        }
    }
}
