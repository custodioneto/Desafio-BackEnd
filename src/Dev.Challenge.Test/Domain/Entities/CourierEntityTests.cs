using Bogus;
using Bogus.Extensions.Brazil;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Enums;
using Xunit;

namespace Dev.Challenge.Test.Domain.Entities
{
    public class CourierEntityTests
    {
        private readonly Faker _faker;

        public CourierEntityTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Constructor_CreatesCourier_WithValidParameters()
        {
            // Arrange
            var name = _faker.Name.FullName();
            var cnpj = _faker.Company.Cnpj();
            var dateOfBirth = _faker.Date.Past(30);
            var driverLicenseNumber = _faker.Random.AlphaNumeric(10);
            var driverLicenseType = DriverLicenseType.A;

            // Act
            var courier = new CourierEntity(name, cnpj, dateOfBirth, driverLicenseNumber, driverLicenseType);

            // Assert
            Assert.Equal(name, courier.Name);
            Assert.Equal(cnpj, courier.Cnpj);
            Assert.Equal(dateOfBirth, courier.DateOfBirth);
            Assert.Equal(driverLicenseNumber, courier.DriverLicenseNumber);
            Assert.Equal(driverLicenseType, courier.DriverLicenseType);
        }

        [Fact]
        public void UpdateDriverLicenseImage_UpdatesImageUrl()
        {
            // Arrange
            var courier = new CourierEntity(_faker.Name.FullName(), _faker.Company.Cnpj(), _faker.Date.Past(30), _faker.Random.AlphaNumeric(10), DriverLicenseType.A);
            var imageUrl = _faker.Internet.Url();

            // Act
            courier.UpdateDriverLicenseImage(imageUrl);

            // Assert
            Assert.Equal(imageUrl, courier.DriverLicenseImageUrl);
        }
    }

}
