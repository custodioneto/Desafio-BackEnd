using Dev.Challenge.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Commands.UpdateRental
{
 
    public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand, Unit>
    {
        private readonly IRentalService _rentalService;

        public UpdateRentalCommandHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<Unit> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            var rental = await _rentalService.GetRentalByIdAsync(request.Id);
            if (rental != null)
            {
                //TODO: 
                //rental.EndDate = request.EndDate;
                //rental.ExpectedEndDate = request.ExpectedEndDate;
                //await _rentalService.UpdateRentalAsync(rental);
            }

            return Unit.Value;
        }
    }

}
