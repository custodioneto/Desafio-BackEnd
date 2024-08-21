using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Rental.Queries.CalculateRentalFee;
using Dev.Challenge.Application.Service;

namespace Dev.Challenge.Test.Application.Handlers.Rental.Queries
{
    

    public class CalculateRentalFeeQueryHandlerTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly CalculateRentalFeeQueryHandler _handler;

        public CalculateRentalFeeQueryHandlerTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _handler = new CalculateRentalFeeQueryHandler(_rentalServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCalculatedRentalFee()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var returnDate = DateTime.Now;
            var expectedFee = 150m;

            _rentalServiceMock.Setup(r => r.CalculateRentalFeeAsync(rentalId, returnDate)).ReturnsAsync(expectedFee);

            var query = new CalculateRentalFeeQuery { RentalId = rentalId, ReturnDate = returnDate };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedFee, result);
            _rentalServiceMock.Verify(r => r.CalculateRentalFeeAsync(rentalId, returnDate), Times.Once);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenRentalServiceFails()
        {
            // Arrange
            var rentalId = Guid.NewGuid();
            var returnDate = DateTime.Now;

            _rentalServiceMock.Setup(r => r.CalculateRentalFeeAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).ThrowsAsync(new Exception("Falha no serviço"));

            var query = new CalculateRentalFeeQuery { RentalId = rentalId, ReturnDate = returnDate };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }
    }

}
