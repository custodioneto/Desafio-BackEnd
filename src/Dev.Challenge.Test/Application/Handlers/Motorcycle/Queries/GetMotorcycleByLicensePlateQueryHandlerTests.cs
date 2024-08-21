using System;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Motorcycle.Queries.GetMotorcycleByLicensePlate;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;

namespace Dev.Challenge.Test.Application.Handlers.Motorcycle.Queries
{
    public class GetMotorcycleByLicensePlateQueryHandlerTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly GetMotorcycleByLicensePlateQueryHandler _handler;

        public GetMotorcycleByLicensePlateQueryHandlerTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _handler = new GetMotorcycleByLicensePlateQueryHandler(_motorcycleServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsMotorcycle_WhenFound()
        {
            // Arrange
            var motorcycle = new MotorcycleEntity(2021, "ModelX", "ABC1234");
            _motorcycleServiceMock.Setup(m => m.GetMotorcycleByLicensePlateAsync(It.IsAny<string>())).ReturnsAsync(motorcycle);

            var query = new GetMotorcycleByLicensePlateQuery { LicensePlate = "ABC1234" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(motorcycle, result);
            _motorcycleServiceMock.Verify(m => m.GetMotorcycleByLicensePlateAsync(query.LicensePlate), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _motorcycleServiceMock.Setup(m => m.GetMotorcycleByLicensePlateAsync(It.IsAny<string>())).ReturnsAsync((MotorcycleEntity)null);

            var query = new GetMotorcycleByLicensePlateQuery { LicensePlate = "XYZ5678" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _motorcycleServiceMock.Verify(m => m.GetMotorcycleByLicensePlateAsync(query.LicensePlate), Times.Once);
        }
    }

}
