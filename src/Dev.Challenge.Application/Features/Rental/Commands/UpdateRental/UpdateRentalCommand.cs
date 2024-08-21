using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Application.Features.Rental.Commands.UpdateRental
{
    using MediatR;
    using System;

    public class UpdateRentalCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
    }

}
