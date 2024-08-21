using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Queries.GetRentalById
{

    public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalEntity>
    {
        private readonly IRentalService _rentalService;

        public GetRentalByIdQueryHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<RentalEntity> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            return await _rentalService.GetRentalByIdAsync(request.Id);
        }
    }

}
