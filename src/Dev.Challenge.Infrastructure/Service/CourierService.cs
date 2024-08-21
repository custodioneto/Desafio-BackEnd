using Dev.Challenge.Application.Repository;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Application.Storage;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Infrastructure.Service
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IStorageService _storageService;

        public CourierService(ICourierRepository courierRepository, IStorageService storageService)
        {
            _courierRepository = courierRepository;
            _storageService = storageService;
        }

        private async Task Validate(CourierEntity courier)
        {
            var entregadorExistenteCnpj = await _courierRepository.GetByCnpjAsync(courier.Cnpj);
            if (entregadorExistenteCnpj != null) throw new ServiceException("O CNPJ já está em uso.");

            var entregadorExistenteCnh = await _courierRepository.GetByDriverLicenseNumberAsync(courier.DriverLicenseNumber);
            if (entregadorExistenteCnh != null) throw new ServiceException("O número da CNH já está em uso.");
        }

        public async Task RegisterCourierAsync(CourierEntity courier)
        {
            await Validate(courier);

            await _courierRepository.AddAsync(courier);
        }

        public async Task<CourierEntity> GetCourierByCnpjAsync(string cnpj)
        {
            return await _courierRepository.GetByCnpjAsync(cnpj);
        }

        public async Task<CourierEntity> GetCourierByDriverLicenseNumberAsync(string driverLicenseNumber)
        {
            return await _courierRepository.GetByDriverLicenseNumberAsync(driverLicenseNumber);
        }

        public async Task UpdateCourierAsync(CourierEntity courier)
        {
            await _courierRepository.UpdateAsync(courier);
        }

        public async Task DeleteCourierAsync(Guid id)
        {
            var courier = await _courierRepository.GetByIdAsync(id);
            if (courier != null)
            {
                await _courierRepository.DeleteAsync(courier);
            }
        }

        public async Task UpdateDriverLicenseImageAsync(Guid id, Stream fileStream, string fileName)
        {
            var courier = await _courierRepository.GetByIdAsync(id);
            if (courier == null) throw new ServiceException("Entregador não encontrado");

            // Verifica se a extensão do arquivo é válida
            var validExtensions = new[] { ".png", ".bmp" };
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (!validExtensions.Contains(fileExtension))
            {
                throw new ServiceException("Formato de arquivo inválido. Somente arquivos PNG e BMP são permitidos.");
            }

            var fileId = await _storageService.UploadFileAsync(fileStream, fileName);

            courier.UpdateDriverLicenseImage(fileId);
            await _courierRepository.UpdateAsync(courier);
        }


        public async Task<IEnumerable<CourierEntity>> GetAllCouriersAsync()
        {
            return await _courierRepository.GetAllAsync();
        }
    }
}
