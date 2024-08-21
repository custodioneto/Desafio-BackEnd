using Dev.Challenge.Application.Service;
using Dev.Challenge.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Queries.GetRentals
{

    public class GetRentalsQueryHandler : IRequestHandler<GetRentalsQuery, IEnumerable<RentalEntity>>
    {
        private readonly IRentalService _rentalService;

        public GetRentalsQueryHandler(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        public async Task<IEnumerable<RentalEntity>> Handle(GetRentalsQuery request, CancellationToken cancellationToken)
        {
            return await _rentalService.GetAllRentalsAsync();
        }
    }

}
