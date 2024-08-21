using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Queries.CalculateRentalFee
{
    public class CalculateRentalFeeQuery : IRequest<decimal>
    {
        public Guid RentalId { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
