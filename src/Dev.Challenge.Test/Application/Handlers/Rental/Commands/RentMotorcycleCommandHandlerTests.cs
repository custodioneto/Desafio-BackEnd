using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Rental.Commands.RentMotorcycle;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;

namespace Dev.Challenge.Test.Application.Handlers.Rental.Commands
{
    public class RentMotorcycleCommandHandlerTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly RentMotorcycleCommandHandler _handler;

        public RentMotorcycleCommandHandlerTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _handler = new RentMotorcycleCommandHandler(_rentalServiceMock.Object);
        }

        [Fact]
        public async Task Handle_RentsMotorcycleSuccessfully()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                MotorcycleId = Guid.NewGuid(),
                CourierId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                ExpectedEndDate = DateTime.Now.AddDays(7)
            };

            _rentalServiceMock.Setup(r => r.RentMotorcycleAsync(It.IsAny<RentalEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _rentalServiceMock.Verify(r => r.RentMotorcycleAsync(It.IsAny<RentalEntity>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenRentalServiceFails()
        {
            // Arrange
            var command = new RentMotorcycleCommand
            {
                MotorcycleId = Guid.NewGuid(),
                CourierId = Guid.NewGuid(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                ExpectedEndDate = DateTime.Now.AddDays(7)
            };

            _rentalServiceMock.Setup(r => r.RentMotorcycleAsync(It.IsAny<RentalEntity>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
