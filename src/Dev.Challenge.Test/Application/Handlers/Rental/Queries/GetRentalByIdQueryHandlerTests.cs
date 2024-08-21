using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Rental.Queries.GetRentalById;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;

namespace Dev.Challenge.Test.Application.Handlers.Rental.Queries
{
    public class GetRentalByIdQueryHandlerTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly GetRentalByIdQueryHandler _handler;

        public GetRentalByIdQueryHandlerTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _handler = new GetRentalByIdQueryHandler(_rentalServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsRentalEntity_WhenFound()
        {
            // Arrange
            var rental = new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7), DateTime.Now.AddDays(7));
            _rentalServiceMock.Setup(r => r.GetRentalByIdAsync(It.IsAny<Guid>())).ReturnsAsync(rental);

            var query = new GetRentalByIdQuery { Id = Guid.NewGuid() };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(rental, result);
            _rentalServiceMock.Verify(r => r.GetRentalByIdAsync(query.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenRentalNotFound()
        {
            // Arrange
            _rentalServiceMock.Setup(r => r.GetRentalByIdAsync(It.IsAny<Guid>())).ReturnsAsync((RentalEntity)null);

            var query = new GetRentalByIdQuery { Id = Guid.NewGuid() };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _rentalServiceMock.Verify(r => r.GetRentalByIdAsync(query.Id), Times.Once);
        }
    }

}
