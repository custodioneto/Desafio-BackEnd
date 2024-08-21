using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Courier.Queries.GetCourierByDriverLicenseNumber;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;

namespace Dev.Challenge.Test.Application.Handlers.Courier.Queries
{
    public class GetCourierByDriverLicenseNumberQueryHandlerTests
    {
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly GetCourierByDriverLicenseNumberQueryHandler _handler;

        public GetCourierByDriverLicenseNumberQueryHandlerTests()
        {
            _courierServiceMock = new Mock<ICourierService>();
            _handler = new GetCourierByDriverLicenseNumberQueryHandler(_courierServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsCourier_WhenFound()
        {
            // Arrange
            var courier = new CourierEntity(Guid.NewGuid(), "John Doe", "12345678000100", DateTime.Now.AddYears(-30), "CNH12345", DriverLicenseType.A);
            _courierServiceMock.Setup(c => c.GetCourierByDriverLicenseNumberAsync(It.IsAny<string>())).ReturnsAsync(courier);

            var query = new GetCourierByDriverLicenseNumberQuery { DriverLicenseNumber = "CNH12345" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(courier, result);
            _courierServiceMock.Verify(c => c.GetCourierByDriverLicenseNumberAsync(query.DriverLicenseNumber), Times.Once);
        }
    }

}
