using Dev.Challenge.Domain.Contracts;
using Dev.Challenge.Domain.Exceptions;

namespace Dev.Challenge.Domain.Entities
{
    public class RentalEntity : IEntity
    {
        private static readonly Dictionary<int, decimal> RentalPlans = new()
    {
        { 7, 30m },
        { 15, 28m },
        { 30, 22m },
        { 45, 20m },
        { 50, 18m }
    };

        public RentalEntity(Guid motorcycleId, Guid courierId, DateTime startDate, DateTime endDate, DateTime expectedEndDate)
        {
            MotorcycleId = motorcycleId;
            CourierId = courierId;
            StartDate = startDate;
            EndDate = endDate;
            ExpectedEndDate = expectedEndDate;

            var rentalPeriod = (endDate - startDate).Days;

            if (!RentalPlans.TryGetValue(rentalPeriod, out var dailyRate))
            {
                if (rentalPeriod > 7 && rentalPeriod <= 15)
                    dailyRate = RentalPlans[15];
                else if (rentalPeriod > 15 && rentalPeriod <= 30)
                    dailyRate = RentalPlans[30];
                else if (rentalPeriod > 30 && rentalPeriod <= 45)
                    dailyRate = RentalPlans[45];
                else if (rentalPeriod > 45 && rentalPeriod <= 50)
                    dailyRate = RentalPlans[50];
                else
                    throw new DomainException("Período de aluguel inválido");
            }

            TotalAmount = dailyRate * rentalPeriod;
        }

        public Guid Id { get; private set; }
        public Guid MotorcycleId { get; private set; }
        public Guid CourierId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public decimal PenaltyFee { get; private set; }

        public void CalculateRentalFee(DateTime returnDate)
        {
            decimal penaltyFee = 0;
            var rentalPeriod = (EndDate - StartDate).Days;
            var daysNotUsed = (ExpectedEndDate - returnDate).Days;

            if (returnDate < ExpectedEndDate)
            {
                if (rentalPeriod <= 7)
                {
                    penaltyFee = daysNotUsed * 30 * 0.2m;
                }
                else if (rentalPeriod > 7 && rentalPeriod <= 15)
                {
                    penaltyFee = daysNotUsed * 28 * 0.4m;
                }
            }
            else if (returnDate > ExpectedEndDate)
            {
                var extraDays = (returnDate - ExpectedEndDate).Days;
                penaltyFee = extraDays * 50;
            }

            PenaltyFee = penaltyFee;
            TotalAmount += penaltyFee;
        }
    }
}
