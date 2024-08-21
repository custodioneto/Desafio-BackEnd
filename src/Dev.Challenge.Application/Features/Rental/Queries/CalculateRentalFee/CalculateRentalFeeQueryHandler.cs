using Dev.Challenge.Application.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Queries.CalculateRentalFee
{
    public class CalculateRentalFeeQueryHandler : IRequestHandler<CalculateRentalFeeQuery, decimal>
    {
        private readonly IRentalService _rentalService;

        public CalculateRentalFeeQueryHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<decimal> Handle(CalculateRentalFeeQuery request, CancellationToken cancellationToken)
        {
            return await _rentalService.CalculateRentalFeeAsync(request.RentalId, request.ReturnDate);
        }
    }
}
