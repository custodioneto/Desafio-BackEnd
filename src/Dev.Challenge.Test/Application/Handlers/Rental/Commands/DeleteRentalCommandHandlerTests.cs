using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Rental.Commands.DeleteRental;
using Dev.Challenge.Application.Service;
using MediatR;


namespace Dev.Challenge.Test.Application.Handlers.Rental.Commands
{ 
    public class DeleteRentalCommandHandlerTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly DeleteRentalCommandHandler _handler;

        public DeleteRentalCommandHandlerTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _handler = new DeleteRentalCommandHandler(_rentalServiceMock.Object);
        }

        [Fact]
        public async Task Handle_DeletesRentalSuccessfully()
        {
            // Arrange
            var command = new DeleteRentalCommand { Id = Guid.NewGuid() };

            _rentalServiceMock.Setup(r => r.DeleteRentalAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _rentalServiceMock.Verify(r => r.DeleteRentalAsync(command.Id), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenRentalServiceFails()
        {
            // Arrange
            var command = new DeleteRentalCommand { Id = Guid.NewGuid() };

            _rentalServiceMock.Setup(r => r.DeleteRentalAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
