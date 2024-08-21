using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Dev.Challenge.Application.Features.Courier.Commands.UpdateCourier;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;
using MediatR;

namespace Dev.Challenge.Test.Application.Handlers.Courier.Commands
{
   

    public class UpdateCourierCommandHandlerTests
    {
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly UpdateCourierCommandHandler _handler;

        public UpdateCourierCommandHandlerTests()
        {
            _courierServiceMock = new Mock<ICourierService>();
            _handler = new UpdateCourierCommandHandler(_courierServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateCourier_WhenCourierExists()
        {
            // Arrange
            var existingCourier = new CourierEntity(
                Guid.NewGuid(),
                "John Doe",
                "12345678901234",
                new DateTime(1980, 1, 1),
                "ABC12345",
                DriverLicenseType.AB
            );

            _courierServiceMock
                .Setup(s => s.GetCourierByDriverLicenseNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(existingCourier);

            var command = new UpdateCourierCommand
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Cnpj = "98765432101234",
                DateOfBirth = new DateTime(1990, 2, 2),
                DriverLicenseNumber = "DEF67890",
                DriverLicenseType = "A"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _courierServiceMock.Verify(s => s.UpdateCourierAsync(It.Is<CourierEntity>(c =>
                c.Name == command.Name &&
                c.Cnpj == command.Cnpj &&
                c.DateOfBirth == command.DateOfBirth &&
                c.DriverLicenseNumber == command.DriverLicenseNumber &&
                c.DriverLicenseType == DriverLicenseType.A &&
                c.DriverLicenseImageUrl == existingCourier.DriverLicenseImageUrl
            )), Times.Once);

            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldNotUpdateCourier_WhenCourierDoesNotExist()
        {
            // Arrange
            _courierServiceMock
                .Setup(s => s.GetCourierByDriverLicenseNumberAsync(It.IsAny<string>()))
                .ReturnsAsync((CourierEntity)null);

            var command = new UpdateCourierCommand
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Cnpj = "98765432101234",
                DateOfBirth = new DateTime(1990, 2, 2),
                DriverLicenseNumber = "DEF67890",
                DriverLicenseType = "A"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _courierServiceMock.Verify(s => s.UpdateCourierAsync(It.IsAny<CourierEntity>()), Times.Never);
            result.Should().Be(Unit.Value);
        }
    }

}
