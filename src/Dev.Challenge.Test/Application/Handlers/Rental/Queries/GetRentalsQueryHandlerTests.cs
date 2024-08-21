using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Rental.Queries.GetRentals;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;

namespace Dev.Challenge.Test.Application.Handlers.Rental.Queries
{
    public class GetRentalsQueryHandlerTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly GetRentalsQueryHandler _handler;

        public GetRentalsQueryHandlerTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _handler = new GetRentalsQueryHandler(_rentalServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllRentals()
        {
            // Arrange
            var rentals = new List<RentalEntity>
        {
            new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7), DateTime.Now.AddDays(7)),
            new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(15), DateTime.Now.AddDays(15))
        };

            _rentalServiceMock.Setup(r => r.GetAllRentalsAsync()).ReturnsAsync(rentals);

            var query = new GetRentalsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(rentals, result);
            _rentalServiceMock.Verify(r => r.GetAllRentalsAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoRentals()
        {
            // Arrange
            var rentals = new List<RentalEntity>();
            _rentalServiceMock.Setup(r => r.GetAllRentalsAsync()).ReturnsAsync(rentals);

            var query = new GetRentalsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            _rentalServiceMock.Verify(r => r.GetAllRentalsAsync(), Times.Once);
        }
    }

}
