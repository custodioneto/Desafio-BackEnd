using System;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Motorcycle.Commands.UpdateMotorcycle;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;


namespace Dev.Challenge.Test.Application.Handlers.Motorcycle.Commands
{
    public class UpdateMotorcycleCommandHandlerTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly UpdateMotorcycleCommandHandler _handler;

        public UpdateMotorcycleCommandHandlerTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _handler = new UpdateMotorcycleCommandHandler(_motorcycleServiceMock.Object);
        }

    

        [Fact]
        public async Task Handle_DoesNotUpdateMotorcycleLicensePlate_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var command = new UpdateMotorcycleCommand { Id = Guid.NewGuid(), LicensePlate = "XYZ5678" };

            _motorcycleServiceMock.Setup(m => m.GetMotorcycleByLicensePlateAsync(command.LicensePlate)).ReturnsAsync((MotorcycleEntity)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _motorcycleServiceMock.Verify(m => m.UpdateMotorcycleLicensePlateAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
            Assert.Equal(Unit.Value, result);
        }
    }

}
