using Dev.Challenge.Application.Features.Courier.Commands.DeleteCourier;
using Dev.Challenge.Application.Service;
using MediatR;
using Moq;

namespace Dev.Challenge.Test.Application.Handlers.Courier.Commands
{

    public class DeleteCourierCommandHandlerTests
    {
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly DeleteCourierCommandHandler _handler;

        public DeleteCourierCommandHandlerTests()
        {
            _courierServiceMock = new Mock<ICourierService>();
            _handler = new DeleteCourierCommandHandler(_courierServiceMock.Object);
        }

        [Fact]
        public async Task Handle_SuccessfullyDeletesCourier()
        {
            // Arrange
            var command = new DeleteCourierCommand { Id = Guid.NewGuid() };

            _courierServiceMock.Setup(c => c.DeleteCourierAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _courierServiceMock.Verify(c => c.DeleteCourierAsync(command.Id), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenCourierServiceFails()
        {
            // Arrange
            var command = new DeleteCourierCommand { Id = Guid.NewGuid() };

            _courierServiceMock.Setup(c => c.DeleteCourierAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
