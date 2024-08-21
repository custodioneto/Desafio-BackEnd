using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Motorcycle.Commands.DeleteMotorcycle;
using Dev.Challenge.Application.Service;
using MediatR;

namespace Dev.Challenge.Test.Application.Handlers.Motorcycle.Commands
{
    public class DeleteMotorcycleCommandHandlerTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly DeleteMotorcycleCommandHandler _handler;

        public DeleteMotorcycleCommandHandlerTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _handler = new DeleteMotorcycleCommandHandler(_motorcycleServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DeletesMotorcycle_WhenMotorcycleExists()
        {
            // Arrange
            var command = new DeleteMotorcycleCommand { Id = Guid.NewGuid() };

            _motorcycleServiceMock.Setup(m => m.DeleteMotorcycleAsync(command.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _motorcycleServiceMock.Verify(m => m.DeleteMotorcycleAsync(command.Id), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenMotorcycleServiceFails()
        {
            // Arrange
            var command = new DeleteMotorcycleCommand { Id = Guid.NewGuid() };

            _motorcycleServiceMock.Setup(m => m.DeleteMotorcycleAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
