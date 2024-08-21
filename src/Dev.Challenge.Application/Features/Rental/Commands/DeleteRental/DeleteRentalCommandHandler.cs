using Dev.Challenge.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Commands.DeleteRental
{
    public class DeleteRentalCommandHandler : IRequestHandler<DeleteRentalCommand, Unit>
    {
        private readonly IRentalService _rentalService;

        public DeleteRentalCommandHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<Unit> Handle(DeleteRentalCommand request, CancellationToken cancellationToken)
        {
            await _rentalService.DeleteRentalAsync(request.Id);
            return Unit.Value;
        }
    }

}
