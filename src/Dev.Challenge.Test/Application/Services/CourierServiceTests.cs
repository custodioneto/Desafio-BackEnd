using Bogus;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus.Extensions.Brazil;
using Dev.Challenge.Application.Repository;
using Dev.Challenge.Application.Storage;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;
using Dev.Challenge.Infrastructure.Exceptions;
using Dev.Challenge.Infrastructure.Service;

namespace Dev.Challenge.Test.Application.Services
{
    public class CourierServiceTests
    {
        private readonly Mock<ICourierRepository> _courierRepositoryMock;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly CourierService _courierService;
        private readonly Faker _faker;

        public CourierServiceTests()
        {
            _courierRepositoryMock = new Mock<ICourierRepository>();
            _storageServiceMock = new Mock<IStorageService>();
            _courierService = new CourierService(_courierRepositoryMock.Object, _storageServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task RegisterCourierAsync_ThrowsException_WhenCnpjIsNotUnique()
        {
            // Arrange
            var existingCourier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByCnpjAsync(It.IsAny<string>())).ReturnsAsync(existingCourier);

            var newCourier = new CourierEntity(_faker.Name.FullName(), existingCourier.Cnpj, _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _courierService.RegisterCourierAsync(newCourier));
            Assert.Equal("O CNPJ já está em uso.", exception.Message);
        }

        [Fact]
        public async Task RegisterCourierAsync_ThrowsException_WhenDriverLicenseNumberIsNotUnique()
        {
            // Arrange
            var existingCourier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByDriverLicenseNumberAsync(It.IsAny<string>())).ReturnsAsync(existingCourier);

            var newCourier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), existingCourier.DriverLicenseNumber, DriverLicenseType.A);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _courierService.RegisterCourierAsync(newCourier));
            Assert.Equal("O número da CNH já está em uso.", exception.Message);
        }

        [Fact]
        public async Task RegisterCourierAsync_Success_WhenCourierIsValid()
        {
            // Arrange
            _courierRepositoryMock.Setup(c => c.GetByCnpjAsync(It.IsAny<string>())).ReturnsAsync((CourierEntity)null);
            _courierRepositoryMock.Setup(c => c.GetByDriverLicenseNumberAsync(It.IsAny<string>())).ReturnsAsync((CourierEntity)null);

            var newCourier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);

            // Act
            await _courierService.RegisterCourierAsync(newCourier);

            // Assert
            _courierRepositoryMock.Verify(c => c.AddAsync(newCourier), Times.Once);
        }

        [Fact]
        public async Task UpdateDriverLicenseImageAsync_ThrowsException_WhenCourierNotFound()
        {
            // Arrange
            _courierRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((CourierEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _courierService.UpdateDriverLicenseImageAsync(Guid.NewGuid(), new MemoryStream(), "image.png"));
            Assert.Equal("Entregador não encontrado", exception.Message);
        }

        [Fact]
        public async Task UpdateDriverLicenseImageAsync_ThrowsException_WhenFileFormatIsInvalid()
        {
            // Arrange
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(courier);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _courierService.UpdateDriverLicenseImageAsync(courier.Id, new MemoryStream(), "image.jpg"));
            Assert.Equal("Formato de arquivo inválido. Somente arquivos PNG e BMP são permitidos.", exception.Message);
        }

        [Fact]
        public async Task UpdateDriverLicenseImageAsync_Success_WhenFileFormatIsValid()
        {
            // Arrange
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(courier);

            var fileStream = new MemoryStream();
            var fileName = "image.png";
            var fileId = _faker.Random.Guid().ToString();
            _storageServiceMock.Setup(s => s.UploadFileAsync(fileStream, fileName)).ReturnsAsync(fileId);

            // Act
            await _courierService.UpdateDriverLicenseImageAsync(courier.Id, fileStream, fileName);

            // Assert
            _courierRepositoryMock.Verify(c => c.UpdateAsync(courier), Times.Once);
            Assert.Equal(fileId, courier.DriverLicenseImageUrl);
        }

        [Fact]
        public async Task GetCourierByCnpjAsync_ReturnsCourier_WhenFound()
        {
            // Arrange
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByCnpjAsync(It.IsAny<string>())).ReturnsAsync(courier);

            // Act
            var result = await _courierService.GetCourierByCnpjAsync(courier.Cnpj);

            // Assert
            Assert.Equal(courier, result);
        }

        [Fact]
        public async Task GetCourierByDriverLicenseNumberAsync_ReturnsCourier_WhenFound()
        {
            // Arrange
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByDriverLicenseNumberAsync(It.IsAny<string>())).ReturnsAsync(courier);

            // Act
            var result = await _courierService.GetCourierByDriverLicenseNumberAsync(courier.DriverLicenseNumber);

            // Assert
            Assert.Equal(courier, result);
        }

        [Fact]
        public async Task GetAllCouriersAsync_ReturnsAllCouriers()
        {
            // Arrange
            var couriers = new List<CourierEntity>
        {
            new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A)
        };
            _courierRepositoryMock.Setup(c => c.GetAllAsync()).ReturnsAsync(couriers);

            // Act
            var result = await _courierService.GetAllCouriersAsync();

            // Assert
            Assert.Equal(couriers, result);
        }

        [Fact]
        public async Task DeleteCourierAsync_DeletesCourier_WhenCourierExists()
        {
            // Arrange
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            _courierRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(courier);

            // Act
            await _courierService.DeleteCourierAsync(courier.Id);

            // Assert
            _courierRepositoryMock.Verify(c => c.DeleteAsync(courier), Times.Once);
        }
    }

}
