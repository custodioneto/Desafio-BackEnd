using Bogus;
using Bogus.Extensions.Brazil;
using Dev.Challenge.Application.Repository;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;
using Dev.Challenge.Infrastructure.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dev.Challenge.Test.Application.Services
{
    public class RentalServiceTests
    {
        private readonly Mock<IRentalRepository> _rentalRepositoryMock;
        private readonly Mock<ICourierRepository> _courierRepositoryMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly RentalService _rentalService;
        private readonly Faker _faker;

        public RentalServiceTests()
        {
            _rentalRepositoryMock = new Mock<IRentalRepository>();
            _courierRepositoryMock = new Mock<ICourierRepository>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _rentalService = new RentalService(_rentalRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object);
            _faker = new Faker();
        }


        [Fact]
        public async Task RentMotorcycleAsync_Success_WhenValidRental()
        {
            // Arrange
            var motorcycle = new MotorcycleEntity(_faker.Random.Int(2000, 2023), _faker.Vehicle.Model(), _faker.Random.String2(7, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"));
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);

            _motorcycleRepositoryMock.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(motorcycle);
            _courierRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(courier);

            var creationDate = DateTime.Now;
            var startDate = creationDate.Date.AddDays(1);
            var endDate = startDate.AddDays(7);
            var expectedEndDate = endDate;

            var rental = new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), startDate, endDate, expectedEndDate);

            // Act
            await _rentalService.RentMotorcycleAsync(rental);

            // Assert
            _rentalRepositoryMock.Verify(r => r.AddAsync(rental), Times.Once);
        }



        [Fact]
        public async Task CalculateRentalFeeAsync_CalculatesCorrectAmount()
        {
            // Arrange
            var rental = new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(8), DateTime.Now.AddDays(8));
            _rentalRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(rental);

            // Act
            var totalAmount = await _rentalService.CalculateRentalFeeAsync(rental.Id, DateTime.Now.AddDays(6)); // Returning 2 days early

            // Assert
            Assert.Equal(rental.TotalAmount, totalAmount);
            _rentalRepositoryMock.Verify(r => r.UpdateAsync(rental), Times.Once);
        }

        [Fact]
        public async Task GetAllRentalsAsync_ReturnsRentals()
        {
            // Arrange
            var rentals = new List<RentalEntity> { new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(8), DateTime.Now.AddDays(8)) };
            _rentalRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rentals);

            // Act
            var result = await _rentalService.GetAllRentalsAsync();

            // Assert
            Assert.Equal(rentals, result);
        }


    }
}
