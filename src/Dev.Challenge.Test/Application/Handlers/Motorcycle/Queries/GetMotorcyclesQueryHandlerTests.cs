using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycles;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;

namespace Dev.Challenge.Test.Application.Handlers.Motorcycle.Queries
{
    public class GetMotorcyclesQueryHandlerTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly GetMotorcyclesQueryHandler _handler;

        public GetMotorcyclesQueryHandlerTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _handler = new GetMotorcyclesQueryHandler(_motorcycleServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllMotorcycles()
        {
            // Arrange
            var motorcycles = new List<MotorcycleEntity>
        {
            new MotorcycleEntity(2021, "ModelX", "ABC1234"),
            new MotorcycleEntity(2020, "ModelY", "XYZ5678")
        };
            _motorcycleServiceMock.Setup(m => m.GetAllMotorcyclesAsync()).ReturnsAsync(motorcycles);

            var query = new GetMotorcyclesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(motorcycles, result);
            _motorcycleServiceMock.Verify(m => m.GetAllMotorcyclesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenNoMotorcycles()
        {
            // Arrange
            var motorcycles = new List<MotorcycleEntity>();
            _motorcycleServiceMock.Setup(m => m.GetAllMotorcyclesAsync()).ReturnsAsync(motorcycles);

            var query = new GetMotorcyclesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            _motorcycleServiceMock.Verify(m => m.GetAllMotorcyclesAsync(), Times.Once);
        }
    }

}
