using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Motorcycle.Commands.RegisterMotorcycle;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;

namespace Dev.Challenge.Test.Application.Handlers.Motorcycle.Commands
{
    public class RegisterMotorcycleCommandHandlerTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly RegisterMotorcycleCommandHandler _handler;

        public RegisterMotorcycleCommandHandlerTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _handler = new RegisterMotorcycleCommandHandler(_motorcycleServiceMock.Object);
        }

        [Fact]
        public async Task Handle_RegistersNewMotorcycle()
        {
            // Arrange
            var command = new RegisterMotorcycleCommand
            {
                Id = Guid.NewGuid(),
                Year = 2022,
                Model = "ModelX",
                LicensePlate = "ABC1234"
            };

            _motorcycleServiceMock.Setup(m => m.RegisterMotorcycleAsync(It.IsAny<MotorcycleEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _motorcycleServiceMock.Verify(m => m.RegisterMotorcycleAsync(It.IsAny<MotorcycleEntity>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenMotorcycleServiceFails()
        {
            // Arrange
            var command = new RegisterMotorcycleCommand
            {
                Id = Guid.NewGuid(),
                Year = 2022,
                Model = "ModelX",
                LicensePlate = "ABC1234"
            };

            _motorcycleServiceMock.Setup(m => m.RegisterMotorcycleAsync(It.IsAny<MotorcycleEntity>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
