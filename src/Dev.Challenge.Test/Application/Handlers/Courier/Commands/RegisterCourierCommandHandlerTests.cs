using Bogus;
using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus.Extensions.Brazil;
using Dev.Challenge.Application.Features.Courier.Commands.RegisterCourier;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;

namespace Dev.Challenge.Test.Application.Handlers.Courier.Commands
{
    public class RegisterCourierCommandHandlerTests
    {
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly RegisterCourierCommandHandler _handler;
        private readonly Faker _faker;

        public RegisterCourierCommandHandlerTests()
        {
            _courierServiceMock = new Mock<ICourierService>();
            _handler = new RegisterCourierCommandHandler(_courierServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_SuccessfullyRegistersCourier()
        {
            // Arrange
            var command = new RegisterCourierCommand
            {
                Id = Guid.NewGuid(),
                Name = _faker.Name.FullName(),
                Cnpj = _faker.Company.Cnpj(),
                DateOfBirth = _faker.Date.Past(30),
                DriverLicenseNumber = _faker.Random.AlphaNumeric(10),
                DriverLicenseType = "A"
            };

            _courierServiceMock.Setup(c => c.RegisterCourierAsync(It.IsAny<CourierEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _courierServiceMock.Verify(c => c.RegisterCourierAsync(It.IsAny<CourierEntity>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenCourierServiceFails()
        {
            // Arrange
            var command = new RegisterCourierCommand
            {
                Id = Guid.NewGuid(),
                Name = _faker.Name.FullName(),
                Cnpj = _faker.Company.Cnpj(),
                DateOfBirth = _faker.Date.Past(30),
                DriverLicenseNumber = _faker.Random.AlphaNumeric(10),
                DriverLicenseType = "A"
            };

            _courierServiceMock.Setup(c => c.RegisterCourierAsync(It.IsAny<CourierEntity>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
