using Bogus;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Exceptions;
using Xunit;

namespace Dev.Challenge.Test.Domain.Entities
{
    

    public class MotorcycleEntityTests
    {
        private readonly Faker _faker;

        public MotorcycleEntityTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Constructor_ThrowsException_WhenYearIsInvalid()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new MotorcycleEntity(1999, _faker.Vehicle.Model(), "ABC1234"));
        }

        [Fact]
        public void Constructor_ThrowsException_WhenModelIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new MotorcycleEntity(2021, null, "ABC1234"));
        }

        [Fact]
        public void Constructor_ThrowsException_WhenLicensePlateIsInvalid()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new MotorcycleEntity(2021, _faker.Vehicle.Model(), "AB123"));
        }

        [Fact]
        public void UpdateLicensePlate_UpdatesLicensePlate_WhenValid()
        {
            // Arrange
            var motorcycle = new MotorcycleEntity(2021, _faker.Vehicle.Model(), "ABC1234");

            // Act
            motorcycle.UpdateLicensePlate("XYZ5678");

            // Assert
            Assert.Equal("XYZ5678", motorcycle.LicensePlate);
        }
    }

}
