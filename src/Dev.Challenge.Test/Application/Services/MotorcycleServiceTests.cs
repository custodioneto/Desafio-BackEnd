using Bogus;
using Dev.Challenge.Application.Event;
using Dev.Challenge.Application.Queue;
using Dev.Challenge.Application.Repository;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Infrastructure.Service;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Dev.Challenge.Test.Application.Services
{
    public class MotorcycleServiceTests
    {
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly MotorcycleService _motorcycleService;
        private readonly Faker _faker;

        public MotorcycleServiceTests()
        {
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _configurationMock = new Mock<IConfiguration>();
            _motorcycleService = new MotorcycleService(_motorcycleRepositoryMock.Object, _rabbitMQServiceMock.Object, _configurationMock.Object);
            _faker = new Faker();
        }


        [Fact]
        public async Task RegisterMotorcycleAsync_Success_WhenMotorcycleIsValid()
        {
            // Arrange
            _motorcycleRepositoryMock.Setup(m => m.GetByLicensePlateAsync(It.IsAny<string>())).ReturnsAsync((MotorcycleEntity)null);
            _configurationMock.Setup(c => c["Queue"]).Returns("MotorcycleQueue");

            var newMotorcycle = new MotorcycleEntity(
                _faker.Random.Int(2000, 2023),
                _faker.Vehicle.Model(),
                _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
            );

            // Act
            await _motorcycleService.RegisterMotorcycleAsync(newMotorcycle);

            // Assert
            _motorcycleRepositoryMock.Verify(m => m.AddAsync(newMotorcycle), Times.Once);
            _rabbitMQServiceMock.Verify(r => r.Publish(It.IsAny<MotorcycleRegisteredEvent>(), "MotorcycleQueue"), Times.Once);
        }

        [Fact]
        public async Task GetAllMotorcyclesAsync_ReturnsAllMotorcycles()
        {
            // Arrange
            var motorcycles = new List<MotorcycleEntity>
        {
            new MotorcycleEntity(_faker.Random.Int(2000, 2023), _faker.Vehicle.Model(), _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
        };
            _motorcycleRepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(motorcycles);

            // Act
            var result = await _motorcycleService.GetAllMotorcyclesAsync();

            // Assert
            Assert.Equal(motorcycles, result);
        }

        [Fact]
        public async Task GetMotorcycleByLicensePlateAsync_ReturnsMotorcycle_WhenFound()
        {
            // Arrange
            var motorcycle = new MotorcycleEntity(
                _faker.Random.Int(2000, 2023),
                _faker.Vehicle.Model(),
                _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
            );
            _motorcycleRepositoryMock.Setup(m => m.GetByLicensePlateAsync(It.IsAny<string>())).ReturnsAsync(motorcycle);

            // Act
            var result = await _motorcycleService.GetMotorcycleByLicensePlateAsync(motorcycle.LicensePlate);

            // Assert
            Assert.Equal(motorcycle, result);
        }

        [Fact]
        public async Task UpdateMotorcycleLicensePlateAsync_Success_WhenMotorcycleIsValid()
        {
            // Arrange
            var existingMotorcycle = new MotorcycleEntity(
                Guid.NewGuid(),
                _faker.Random.Int(2000, 2023),
                _faker.Vehicle.Model(),
                _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
            );
            _motorcycleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingMotorcycle);
            _motorcycleRepositoryMock.Setup(m => m.GetByLicensePlateAsync(It.IsAny<string>())).ReturnsAsync((MotorcycleEntity)null);

            var newLicensePlate = _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

            // Act
            await _motorcycleService.UpdateMotorcycleLicensePlateAsync(existingMotorcycle.Id, newLicensePlate);

            // Assert
            Assert.Equal(newLicensePlate, existingMotorcycle.LicensePlate);
            _motorcycleRepositoryMock.Verify(m => m.UpdateAsync(existingMotorcycle), Times.Once);
        }

        [Fact]
        public async Task DeleteMotorcycleAsync_DeletesMotorcycle_WhenMotorcycleExists()
        {
            // Arrange
            var motorcycle = new MotorcycleEntity(
                Guid.NewGuid(),
                _faker.Random.Int(2000, 2023),
                _faker.Vehicle.Model(),
                _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
            );
            _motorcycleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(motorcycle);

            // Act
            await _motorcycleService.DeleteMotorcycleAsync(motorcycle.Id);

            // Assert
            _motorcycleRepositoryMock.Verify(m => m.DeleteAsync(motorcycle), Times.Once);
        }

        [Fact]
        public async Task UpdateMotorcycleLicensePlateAsync_ThrowsException_WhenMotorcycleNotFound()
        {
            // Arrange
            _motorcycleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((MotorcycleEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _motorcycleService.UpdateMotorcycleLicensePlateAsync(Guid.NewGuid(), "NEWPLT"));
        }
    }


}
