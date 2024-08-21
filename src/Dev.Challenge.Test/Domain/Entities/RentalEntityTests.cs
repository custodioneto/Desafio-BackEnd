using Bogus;
using Dev.Challenge.Domain.Entities;
using Dev.Challenge.Domain.Exceptions;

namespace Dev.Challenge.Test.Domain.Entities
{


    public class RentalEntityTests
    {
        private readonly Faker _faker;

        public RentalEntityTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Constructor_ThrowsException_WhenRentalPeriodIsInvalid()
        {
            // Arrange
            var motorcycleId = Guid.NewGuid();
            var courierId = Guid.NewGuid();
            var startDate = DateTime.Now.AddDays(1);
            var endDate = DateTime.Now.AddDays(6); // Invalid period (6 days)

            // Act & Assert
            Assert.Throws<DomainException>(() => new RentalEntity(motorcycleId, courierId, startDate, endDate, endDate));
        }

        [Fact]
        public void CalculateRentalFee_SimplifiedTest_WhenReturnedEarly()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);  // Data fixa
            var endDate = new DateTime(2024, 1, 8);    // 7 dias depois
            var expectedEndDate = new DateTime(2024, 1, 8);  // Mesma data que endDate
            var returnDate = new DateTime(2024, 1, 6);  // 2 dias antes do fim do aluguel

            var rental = new RentalEntity(
                Guid.NewGuid(),
                Guid.NewGuid(),
                startDate,
                endDate,
                expectedEndDate
            );

            // Act
            rental.CalculateRentalFee(returnDate);

            // Assert
            Assert.Equal(12m, rental.PenaltyFee); // 2 dias * 30 * 0.2m = 12
            Console.WriteLine($"PenaltyFee Expected: 12, Actual: {rental.PenaltyFee}");
        }


        [Fact]
        public void CalculateRentalFee_CalculatesPenaltyFee_WhenReturnedLate()
        {
            // Arrange
            var rental = new RentalEntity(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.AddDays(1), DateTime.Now.AddDays(15), DateTime.Now.AddDays(15));

            // Act
            rental.CalculateRentalFee(DateTime.Now.AddDays(17)); // Returning 2 days late

            // Assert
            Assert.Equal(100m, rental.PenaltyFee); // 2 extra days * 50 = 100
        }
    }

}
