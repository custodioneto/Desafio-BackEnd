using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Commands.RentMotorcycle
{

    public class RentMotorcycleCommandHandler : IRequestHandler<RentMotorcycleCommand, Unit>
    {
        private readonly IRentalService _rentalService;

        public RentMotorcycleCommandHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<Unit> Handle(RentMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var rental = new RentalEntity( request.MotorcycleId, request.CourierId, request.StartDate, request.EndDate, request.ExpectedEndDate);

            await _rentalService.RentMotorcycleAsync(rental);
            return Unit.Value;
        }
    }

}
