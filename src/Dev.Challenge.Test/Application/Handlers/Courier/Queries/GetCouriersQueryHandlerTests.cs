using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dev.Challenge.Application.Features.Courier.Queries.GetCouriers;
using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;

namespace Dev.Challenge.Test.Application.Handlers.Courier.Queries
{
    public class GetCouriersQueryHandlerTests
    {
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly GetCouriersQueryHandler _handler;

        public GetCouriersQueryHandlerTests()
        {
            _courierServiceMock = new Mock<ICourierService>();
            _handler = new GetCouriersQueryHandler(_courierServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllCouriers()
        {
            // Arrange
            var couriers = new List<CourierEntity>
        {
            new CourierEntity(Guid.NewGuid(), "John Doe", "12345678000100", DateTime.Now.AddYears(-30), "CNH12345", DriverLicenseType.A)
        };
            _courierServiceMock.Setup(c => c.GetAllCouriersAsync()).ReturnsAsync(couriers);

            // Act
            var result = await _handler.Handle(new GetCouriersQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(couriers, result);
            _courierServiceMock.Verify(c => c.GetAllCouriersAsync(), Times.Once);
        }
    }

}
